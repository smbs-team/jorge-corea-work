/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  useState,
  PropsWithChildren,
  useEffect,
  useRef,
  useContext,
} from 'react';
import {
  ChipDataType,
  ColorConfiguration,
  ErrorMessageAlertCtx,
  SnackContext,
} from '@ptas/react-ui-library';
import {
  MapRenderer,
  mapService,
  OptionSetItemResp,
  RendererDataset,
} from 'services/map';
import { rendererService } from 'services/map/renderer';
import { Folder } from 'services/map/model/folder';
import { apiService, StoreItemValue } from 'services/api';
import { useAsync, useMount, useObservable } from 'react-use';
import { tokenService } from 'services/common/tokenService';
import { useSelectedDots } from 'hooks/map/useSelectedDots';
import { MapsTreeViewRow } from 'components/MapsTreeView/types';
import {
  useUserMaps,
  useDataSetPoints,
  useRendererCategories,
  useDefaultMap,
} from './hooks';
import { signalRService } from 'services/parcel/selection/signalRService';
import { useIsOnline } from 'hooks/useIsOnline';
import { isArrayEqual } from 'utils/componentUtil';
import { HomeContextProps } from './types';
import { $onError, $onLayerSourcesReady } from 'services/map/mapServiceEvents';
import { useMapsTreeView } from './hooks/useMapsTreeView';
import { useLayerServiceLayers } from './hooks/useLayerServiceLayers';
import { useLabels } from './hooks/useLabels';
import { getErrorStr } from 'utils/getErrorStr';
import { useGlMap } from 'hooks/map/useGlMap';
import { AppContext } from 'contexts';
import { utilService } from 'services/common';
import { MapCustomEvent, QUERY_PARAM } from 'appConstants';
import { once } from 'lodash';
import userService from 'services/user/userService';
import { dotService } from 'services/map/dot';
import { BaseMapService } from 'services/map/BaseMapService';
import { BookmarkType } from 'components/ContentWrapper/SideInfoBar/BookmarkItem';
import bubbleService from 'services/map/bubble/bubbleService';

const filterDatasetId = utilService.getUrlSearchParam(QUERY_PARAM.DATASET_ID);

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const HomeContext = React.createContext<HomeContextProps>(null as any);

/**
 * A Context provider component
 *
 * @param props - Component children
 */
export const HomeProvider = (
  props: PropsWithChildren<unknown>
): JSX.Element => {
  const [savePendingChanges, setSavePendingChanges] = useState(false);
  const { setShowBackdrop } = useContext(AppContext);
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const $userMapFolders = useObservable(mapService.$onUserMapFoldersChange, []);
  const { setSnackState } = useContext(SnackContext);
  const layerSources = useObservable($onLayerSourcesReady, []);
  const [panelContent, setPanelContent] = useState<JSX.Element | null>(null);
  const [panelHeight, setPanelHeight] = useState<number>(450);
  const [defaultDataSet, setDefaultDataSet] = useState<
    RendererDataset | undefined
  >();
  const [userRendererFolders, setUserRendererFolders] = useState<Folder[]>([]);
  const [currentLayer, setCurrentLayer] = useState<MapRenderer | undefined>();
  const [linearProgress, setLinearProgress] = useState(false);
  const {
    userMaps,
    setUserMaps,
    selectedUserMap,
    setSelectedUserMap,
    selectedSystemUserMap,
    setSelectedSystemUserMap,
    editingUserMap,
    setEditingUserMap,
    saveEditingUserMap,
    saveUserMap,
    colorRuleAction,
    setColorRuleAction,
    mapsUserDetails,
    prevSelectedUserMap,
    setPrevSelectedUserMap,
    prevSelectedSystemUserMap,
    setPrevSelectedSystemUserMap,
    selectedUserMapId,
  } = useUserMaps({
    currentLayer,
    setLinearProgress,
  });
  const {
    rendererCategories,
    setRendererCategories,
    categoriesData,
    setCategoriesData,
    selectedCategories,
    setSelectedCategories,
    loadUserMapsForAllCategories,
    onCategoryClick,
    createNewCategory,
    systemUserMaps,
  } = useRendererCategories(editingUserMap);
  const { defaultMap, saveDefaultMap } = useDefaultMap();
  const [userMapFolders, setUserMapFolders] = useState<Folder[]>([]);
  const [mediaToken, setMediaToken] = useState<string>();
  const [b2cToken, setB2cToken] = useState<string | undefined>();
  const [openExportMapImgModal, setOpenExportMapImgModal] = useState<boolean>(
    false
  );
  const [mapImgCaptureSrc, setMapImgCaptureSrc] = useState<string>('');

  const [userColorRamps, setUserColorRamps] = useState<ColorConfiguration[]>(
    []
  );
  const [createNewFolder, setCreateNewFolder] = useState<boolean>(false);
  const [
    newCategoryPopoverAnchor,
    setNewCategoryPopoverAnchor,
  ] = useState<HTMLElement | null>(null);
  const [treeSelection, setTreeSelection] = useState<
    MapsTreeViewRow | undefined
  >();
  const [
    saveNewMapPopoverAnchor,
    setSaveNewMapPopoverAnchor,
  ] = useState<HTMLElement | null>(null);
  const [
    saveNewRendererPopoverAnchor,
    setSaveNewRendererPopoverAnchor,
  ] = useState<HTMLElement | null>(null);
  const [isEditingUserMap, setIsEditingUserMap] = useState<boolean>(false);
  const [bookmarks, setBookmarks] = useState<BookmarkType[]>([]);
  const [bookmarkTags, setBookmarkTags] = useState<ChipDataType[]>([]);
  const [bookmarkOptionSet, setBookmarkOptionSet] = useState<
    OptionSetItemResp[]
  >([]);
  const selectedParcels = useSelectedDots();
  const dataSetPoints = useDataSetPoints();
  const online = useIsOnline();
  const [mapThumbnail, setMapThumbnail] = useState<boolean>(false);
  const errorText = useObservable($onError);
  const mapsTreeView = useMapsTreeView({
    mapsUserDetails,
    selectedUserMap,
    userMapFolders,
    userMaps,
  });
  const appliedRenderers = useLayerServiceLayers({
    selectedSystemUserMap,
    selectedUserMap,
  });
  const userMapLabels = useLabels({ selectedSystemUserMap, selectedUserMap });
  const map = useGlMap();

  const initServices = useRef(
    once(
      async (map: mapboxgl.Map): Promise<void> => {
        new BaseMapService(map);
        await userService.initUserInfo();
        await mapService.loadUserMaps();
        dotService.init(map);
        mapService.initializeServices();
        bubbleService.init(map);
        setTimeout(() => {
          map.fire(MapCustomEvent.LOAD_DS_POINTS);
        }, 5000);
      }
    )
  );

  useAsync(async () => {
    if (!map) return;
    try {
      if (!filterDatasetId) {
        setShowBackdrop(true);
      }
      await initServices.current(map);
    } catch (error) {
      showErrorMessage({
        detail: getErrorStr(error),
      });
    } finally {
      if (!filterDatasetId) {
        setShowBackdrop(false);
      }
    }
  }, [map, showErrorMessage]);

  useEffect(() => {
    if (!errorText) return;
    setSnackState({
      severity: 'error',
      text: errorText,
    });
  }, [errorText, setSnackState]);

  useMount(() => {
    setMapThumbnail(localStorage.getItem('mapThumbnail') === 'true');
  });

  useEffect(() => {
    localStorage.setItem('mapThumbnail', '' + mapThumbnail);
  }, [mapThumbnail]);

  useEffect(() => {
    if (online === undefined) return;
    if (!online) {
      console.error('Internet connection lost');
    }
  }, [online, setSnackState]);

  //Sets the tab title according to the selected parcels
  useEffect(() => {
    if (selectedParcels.length === 0) document.title = 'GIS Tool';

    if (selectedParcels.length > 1) {
      document.title = 'Multiple Parcels';
    }

    if (selectedParcels.length === 1)
      document.title = `${selectedParcels[0].properties.major}-${selectedParcels[0].properties.minor}`;
  }, [selectedParcels]);

  useEffect(() => {
    apiService.$onB2CTokenChanged.subscribe((token) => {
      token && setB2cToken(token);
    });

    tokenService.$onInvalidToken.subscribe(() => {
      setB2cToken(undefined);
      localStorage.removeItem('magicToken');
      setSnackState({
        severity: 'error',
        text: 'Invalid token. Please authenticate from Dynamics CE.',
      });
    });
  }, [setSnackState]);

  useEffect(() => {
    rendererService.$onGetDefaultDataSet.subscribe(
      (dataset: RendererDataset) => {
        dataset && setDefaultDataSet(dataset);
      }
    );
  }, []);

  useEffect(() => {
    rendererService.$onRendererFoldersChange.subscribe(
      ({ rendererFolders }) => {
        rendererFolders && setUserRendererFolders(rendererFolders);
      }
    );
  }, []);

  const [initialRamps, setInitialRamps] = useState<ColorConfiguration[]>([]);
  const isInitial = useRef<boolean | null>(null);

  //gets the saved user ramps
  const getRamps = async (): Promise<void> => {
    try {
      const storeItem = await apiService.metaStore.getUserStoreItem<
        StoreItemValue['color-ramp']
      >('color-ramp');
      if (storeItem.data?.value?.length) {
        const ramps = storeItem.data.value;
        setUserColorRamps(ramps);
        setInitialRamps(ramps);
      }

      isInitial.current = true;
    } catch (error) {
      setSnackState({
        severity: 'error',
        text: 'Error getting user color ramps',
      });
    }
  };

  useEffect(() => {
    b2cToken && signalRService.initSignalRConnection(b2cToken);
  }, [b2cToken]);

  //gets the saved user ramps
  useEffect(() => {
    if (!b2cToken) return;
    const fetch = async (): Promise<void> => {
      await getRamps();
    };
    fetch();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [b2cToken]);

  //handles ramps changes, on change it calls ImportUserDataStoreItem
  useEffect(() => {
    if (isInitial.current !== true) {
      return;
    }
    if (
      !initialRamps ||
      !userColorRamps ||
      isArrayEqual(initialRamps, userColorRamps)
    ) {
      return;
    }
    const saveRamps = async (): Promise<void> => {
      try {
        await apiService.metaStore.saveUserStoreItem({
          storeType: 'color-ramp',
          value: userColorRamps,
        });
      } catch (error) {
        setSnackState({ severity: 'error', text: 'Error saving user ramps' });
        getRamps();
      }
    };
    saveRamps();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [userColorRamps, initialRamps, setSnackState]);

  useAsync(async () => {
    if (!b2cToken) return;
    const token = await apiService.getMediaToken();
    if (token.hasError) {
      console.error('Error getting media token.');
      return;
    }
    token.data && setMediaToken(token.data);
  }, [b2cToken]);

  useEffect(() => {
    setUserMapFolders($userMapFolders);
  }, [$userMapFolders]);

  return (
    <HomeContext.Provider
      value={{
        savePendingChanges,
        setSavePendingChanges,
        systemUserMaps,
        layerSources,
        mapThumbnail,
        setMapThumbnail,
        panelContent,
        setPanelContent,
        defaultDataSet,
        rendererCategories,
        setRendererCategories,
        categoriesData,
        setCategoriesData,
        selectedCategories,
        setSelectedCategories,
        loadUserMapsForAllCategories,
        onCategoryClick,
        createNewCategory,
        userRendererFolders,
        setUserRendererFolders,
        userMaps,
        setUserMaps,
        userMapFolders,
        setUserMapFolders,
        mediaToken,
        selectedUserMap,
        setSelectedUserMap,
        selectedSystemUserMap,
        setSelectedSystemUserMap,
        editingUserMap,
        setEditingUserMap,
        saveEditingUserMap,
        saveUserMap,
        colorRuleAction,
        setColorRuleAction,
        currentLayer,
        setCurrentLayer,
        b2cToken,
        setB2cToken,
        openExportMapImgModal,
        setOpenExportMapImgModal,
        mapImgCaptureSrc,
        setMapImgCaptureSrc,
        userColorRamps,
        setUserColorRamps,
        panelHeight,
        setPanelHeight,
        createNewFolder,
        setCreateNewFolder,
        newCategoryPopoverAnchor,
        setNewCategoryPopoverAnchor,
        treeSelection,
        setTreeSelection,
        mapsUserDetails,
        saveNewMapPopoverAnchor,
        setSaveNewMapPopoverAnchor,
        saveNewRendererPopoverAnchor,
        setSaveNewRendererPopoverAnchor,
        isEditingUserMap,
        setIsEditingUserMap,
        dataSetPoints,
        bookmarkTags,
        setBookmarkTags,
        bookmarks,
        setBookmarks,
        bookmarkOptionSet,
        setBookmarkOptionSet,
        linearProgress,
        setLinearProgress,
        defaultMap,
        saveDefaultMap,
        mapsTreeView,
        appliedRenderers,
        userMapLabels,
        prevSelectedUserMap,
        setPrevSelectedUserMap,
        prevSelectedSystemUserMap,
        setPrevSelectedSystemUserMap,
        selectedUserMapId,
      }}
    >
      {props.children}
    </HomeContext.Provider>
  );
};
