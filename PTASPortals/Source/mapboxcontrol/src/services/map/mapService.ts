/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import mapboxgl, { EventData, MapLayerEventType } from 'mapbox-gl';
import { debounce, once } from 'lodash';
import { Subject } from 'rxjs';
import { first } from 'rxjs/operators';
import html2canvas from 'html2canvas';
import { AppLayers, MAP_INITIAL_COORDINATES } from 'appConstants';
import {
  LayerSource,
  UserDetails,
  UserMap,
  Folder,
  ParcelFeature,
} from './model';
import { SearchService } from './searchService';
import { mapUtilService } from './mapUtilService';
import { layerService } from './layerService';
import { rendererService } from './renderer';
import {
  $onMapReady,
  $onParcelClick,
  $onError,
  $onSelectedUserMapChange,
  $onLayerSourcesReady,
} from './mapServiceEvents';
import { ParcelLineService } from 'services/parcel';
import { ApiServiceResult } from 'services/common';
import userService from 'services/user/userService';
import { tokenService } from 'services/common/tokenService';
import { apiService, datasetService, StoreItemValue } from 'services/api';
import { systemUserMapService } from './systemUserMapService';
import { UserMapsChangeEvent } from './types';
import { AnnotationLabelService } from './annotationLabelService/annotationLabelService';
import { utilService } from 'services/common';
import { RendererLabel } from './renderer/textRenderer/textRenderersService';
import { appInsights } from 'services/appInsights';
// import RulerControl from 'mapbox-gl-controls/lib/ruler';

mapboxgl.accessToken = process.env.REACT_APP_MAPBOX_GL_ACCESS_TOKEN;

interface InitProps {
  mapContainerId: string;
  zoom?: number;
}

/**
 * Main mapbox service
 */
class MapService {
  _mapContainerId = '';
  /**
   * The map initial map zoom level
   *
   */
  readonly defaultZoomLevel = 8.5;
  /**
   * The mapbox map instance
   *
   */
  map?: mapboxgl.Map;
  /**
   *
   * The map search instance
   */
  mapSearch: SearchService | undefined;
  /**
   * User maps
   */
  userMaps: UserMap[] = [];

  userDetails: UserDetails[] = [];

  /**
   * Subject that indicates when the user maps change
   */
  $onUserMapsChange = new Subject<UserMapsChangeEvent>();

  /**
   * Subject that indicates when the editing user map changes
   */
  $onEditingUserMapChange = new Subject<UserMap | undefined>();

  /**
   * User map folders
   */
  userMapFolders: Folder[] = [];
  /**
   * Subject that indicates when the user map folders change
   */
  $onUserMapFoldersChange = new Subject<Folder[]>();

  selectedUserMap: UserMap | undefined;

  constructor() {
    $onMapReady.pipe(first()).subscribe(this._onMapReady);
  }

  private _onMapReady = once((map: mapboxgl.Map): void => {
    layerService.$onRemoveLayer.subscribe((layer) => {
      if (layer.id === AppLayers.PARCEL_LAYER) {
        if (map.getLayer(layer.id)) {
          this._unbindPrevParcelEvents(layer.id);
        }
      }
    });

    layerService.$onLayerAdded.subscribe(this._onLayerAdded);

    map.on('data', (e) => {
      const { dataType, isSourceLoaded } = e;
      if (dataType === 'source' && isSourceLoaded) {
        const { source, sourceId } = e;
        if (sourceId === 'parcelSource') {
          map.fire('parcel-source-data', { source, sourceId });
        }
      }
    });
  });

  /**
   * Initialize map service
   *
   * @param map -mapbox-gl Map instance
   * @param HomeContext -application context
   * @returns -layer configuration result
   */
  init = once(
    async ({ mapContainerId, zoom }: InitProps): Promise<mapboxgl.Map> => {
      const token = utilService.getUrlSearchParam('token');
      await tokenService.exchangeToken(token ?? '');

      this._mapContainerId = mapContainerId;
      this.map = new mapboxgl.Map({
        container: this._mapContainerId,
        style: 'mapbox://styles/mapbox/streets-v11?optimize=true',
        center: MAP_INITIAL_COORDINATES,
        minZoom: 2,
        zoom: zoom ?? this.defaultZoomLevel,
        preserveDrawingBuffer: true,
      });
      mapUtilService.bindCtlClickEvent();
      this.map.on('load', async () => {
        if (!this.map) return;
        appInsights.addTelemetryInitializer((envelope) => {
          if (
            envelope.baseData?.['target'] &&
            process.env.REACT_APP_DEFAULT_MAP_URL &&
            process.env.REACT_APP_USE_DEFAULT_MAP === 'true'
          ) {
            const url = new URL(envelope.baseData?.['target']);
            return (
              url.host !== new URL(process.env.REACT_APP_DEFAULT_MAP_URL).host
            );
          }
          return true;
        });
        await this.loadLayers(this.map);
      });
      this.map.on('moveend', this._onMapMoveEnd);
      return this.map;
    }
  );

  useDefaultMap =
    process.env.REACT_APP_USE_DEFAULT_MAP === 'true' &&
    !!process.env.REACT_APP_DEFAULT_MAP_URL;

  public getMapLabels = (userMap: UserMap | undefined): RendererLabel[] =>
    userMap?.mapRenderers.flatMap((renderer) =>
      renderer.rendererRules.labels?.length ? renderer.rendererRules.labels : []
    ) ?? [];

  private _onLayerAdded = (layer: LayerSource['defaultMapboxLayer']): void => {
    if (!this.map) return;
    if (layer.id === AppLayers.PARCEL_LAYER) {
      datasetService.loadParcelsInfo();
      this.map.on('click', layer.id, this._onParcelClick);
    }
  };

  loadUserMaps = async (): Promise<void> => {
    if (!this.map) return;
    const userMapsRes = await apiService.getUserMaps(userService.userInfo.id);
    this.userMaps = userMapsRes.userMaps;
    this.userDetails = userMapsRes.usersDetails;
    let selectedUserMapId: number | undefined;
    if (this.userMaps.length) {
      //Selected user map
      const defaultMap = await apiService.metaStore.getUserStoreItem<
        StoreItemValue['default-map']
      >('default-map');
      if (defaultMap.data) {
        if (
          this.userMaps.every(
            (val) => val.userMapId !== defaultMap.data?.value.mapId
          )
        ) {
          apiService.metaStore.deleteUserDataStoreItem(defaultMap.data.itemId);
        }
      }

      selectedUserMapId =
        defaultMap.data?.value.mapId ??
        (await userService.getFallBackDefaultMap());

      if (selectedUserMapId) {
        if (
          this.userMaps.some(
            (userMap) => userMap.userMapId === selectedUserMapId
          )
        ) {
          const selectedUserMap = await apiService.getUserMap(
            selectedUserMapId
          );
          if (selectedUserMap) {
            this.selectedUserMap = selectedUserMap;
            $onSelectedUserMapChange.next(selectedUserMap);
          }
        }
      }
    }

    this.$onUserMapsChange.next({
      userMaps: this.userMaps,
      userDetails: this.userDetails,
      defaultUserMapId: selectedUserMapId,
    });

    const mapFoldersRes = await apiService.getUserFolders(
      userService.userInfo.id,
      'UserMap'
    );

    this.userMapFolders = mapFoldersRes;
    this.$onUserMapFoldersChange.next(this.userMapFolders);
  };

  /**
   *
   * @param userMapId - Id of newly selected user map.
   * Use undefined to specify that no user map be applied.
   * Use 0 when creating a new user map, without modifying the user map selection
   * @param updateEditingMap - Whether or not to update the editing user map
   * with the user map specified on userMapId
   */
  updateSelectedMap = async (
    userMapId?: number,
    updateEditingMap?: boolean
  ): Promise<void> => {
    if (userMapId === undefined) {
      //Case for selecting "None" user map menu option,
      //or deleting selected map
      this.selectedUserMap = undefined;
      $onSelectedUserMapChange.next(undefined);
    } else if (userMapId === 0) {
      //Case for creating new user map (empty map)
      this.selectedUserMap = undefined;
    } else {
      const selectedUserMap = await apiService.getUserMap(userMapId);
      if (selectedUserMap) {
        this.selectedUserMap = selectedUserMap;
        $onSelectedUserMapChange.next(selectedUserMap);
        if (updateEditingMap) {
          this.$onEditingUserMapChange.next(selectedUserMap);
        }
      }
    }
  };

  refreshUserMaps = async (): Promise<void> => {
    const userMapsRes = await apiService.getUserMaps(userService.userInfo.id);
    this.userMaps = userMapsRes.userMaps;
    this.userDetails = userMapsRes.usersDetails;
    this.$onUserMapsChange.next({
      userMaps: this.userMaps,
      userDetails: this.userDetails,
    });
  };

  refreshUserMapFolders = async (): Promise<void> => {
    const foldersRes = await apiService.getUserFolders(
      userService.userInfo.id,
      'UserMap'
    );
    this.userMapFolders = foldersRes;
    this.$onUserMapFoldersChange.next(this.userMapFolders);
  };

  saveUserMap = async (userMap: UserMap): Promise<number> => {
    const userMapReq = new UserMap({
      createdBy: userMap.createdBy,
      createdTimestamp: userMap.createdTimestamp,
      folderPath: userMap.folderPath,
      isLocked: userMap.isLocked,
      lastModifiedBy: userMap.lastModifiedBy,
      lastModifiedTimestamp: userMap.lastModifiedTimestamp,
      parentFolderId: userMap.parentFolderId,
      userMapId: userMap.userMapId,
      userMapName: userMap.userMapName,
      mapRenderers: [],
    });

    if (userMap.mapRenderers?.length) {
      userMapReq.mapRenderers = userMap.mapRenderers;
    }
    if (userMap.userMapId) {
      const response = await apiService.updateUserMap(
        userMapReq.userMapId,
        userMapReq
      );
      return response.data?.userMapId ?? 0;
    } else {
      const response = await apiService.createUserMap(userMapReq);
      return response;
    }
  };

  duplicateUserMap = async (
    userMapId: number,
    userMapName: string,
    folderPath?: string
  ): Promise<number> => {
    const userMap = await apiService.getUserMap(userMapId);
    if (userMap) {
      const newUserMap: UserMap = userMap;
      newUserMap.userMapName = userMapName;
      if (folderPath) {
        newUserMap.folderPath = folderPath;
      }
      const response = await apiService.createUserMap(newUserMap);
      return response;
    }
    throw new Error('Invalid user map ID');
  };

  deleteUserMap = async (
    userMapId: number
  ): Promise<ApiServiceResult<unknown>> => {
    const response = await apiService.deleteUserMap(userMapId);
    if (response.hasError) {
      $onError.next('Error on delete user map');
      console.error(response.errorMessage);
      throw new Error('Error on delete user map');
    } else {
      if (userMapId === this.selectedUserMap?.userMapId) {
        this.updateSelectedMap(undefined);
      }
    }
    return response;
  };

  renameUserMap = async (
    userMapId: number,
    userMapName: string
  ): Promise<ApiServiceResult<unknown>> => {
    const response = await apiService.updateUserMap(userMapId, { userMapName });
    if (
      !response.hasError &&
      response.data &&
      userMapId === this.selectedUserMap?.userMapId
    ) {
      await this.refreshUserMaps();
      $onSelectedUserMapChange.next(response.data);
    }
    return response;
  };

  /**
   * Fetch layers configuration from the api
   * @param map -Mapboxgl map
   * @returns -layer configuration result
   */
  loadLayers = async (
    map: mapboxgl.Map
  ): Promise<ApiServiceResult<LayerSource[] | undefined>> => {
    layerService.map = map;
    const layersRes = await apiService.layerSources.get();
    if (!layersRes.data) {
      $onMapReady.next(map);
      $onLayerSourcesReady.next([]);
      return layersRes;
    }
    await layerService.addMapSources(layersRes.data);
    layerService.layersConfigurationList = layersRes.data;
    $onMapReady.next(map);
    $onLayerSourcesReady.next(layersRes.data);
    return layersRes;
  };

  toggleLockUserMap = async (
    userMapId: number
  ): Promise<ApiServiceResult<unknown>> => {
    const userMap = await apiService.getUserMap(userMapId);
    if (userMap) {
      userMap.isLocked = !userMap.isLocked;
      const response = await apiService.updateUserMap(
        userMap.userMapId,
        userMap
      );
      return response;
    }
    return new Promise((_, reject) => reject('Invalid user map ID'));
  };

  /**
   * Initialize required services
   */
  initializeServices = (): void => {
    if (!this.map) return;
    this.mapSearch = new SearchService({
      map: this.map,
    });
    rendererService.init(this.map);
    systemUserMapService.init(this.map);
    new ParcelLineService(this.map);
    new AnnotationLabelService(this.map);
  };

  /**
   * Load feature data on map move ends, use debounce to reduce multiple calls
   */
  private _onMapMoveEnd = debounce(() => {
    datasetService.loadParcelsInfo();
  }, 100);

  /**
   * Do stuff on layer click depending on the selection mode
   * @param e -Mapboxgl event
   */
  private _onParcelClick = (
    e: MapLayerEventType['click'] & EventData
  ): void => {
    if (!e?.features?.length) return;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const _feature = new ParcelFeature(e.features[0] as any);
    $onParcelClick.next({ feature: _feature, lngLat: e.lngLat });
  };

  /**
   *Removes previous layers when selecting a new one
   *
   * @param previousLayer -The prev selected layer
   * @param map -The mapboxgl Map instance
   */
  private _unbindPrevParcelEvents = (layerId: string): void => {
    this.map?.off('click', layerId, this._onParcelClick);
  };

  /**
   * Show and hide map layer
   *
   * @param layerId -The layerId
   * @param value - A boolean value that represents the visibility
   */
  public updateLayerVisibility = (layerId: string, value: boolean): void => {
    if (!this?.map) return;
    if (!this.map.getLayer(layerId)) return;
    this.map.setLayoutProperty(
      layerId,
      'visibility',
      value ? 'visible' : 'none'
    );
  };

  takeScreenshot = async (): Promise<string> => {
    const { map } = this;
    if (!map) {
      throw new Error('map is undefined');
    }

    const mapContainer = map.getContainer();
    const canvas = await html2canvas(mapContainer as HTMLElement, {
      useCORS: true,
      allowTaint: true,
      scale: 3,
    });

    return canvas.toDataURL('image/png');
  };

  isNewMap = (userMap?: UserMap): boolean => userMap?.userMapId === 0;
}

export const mapService = new MapService();
