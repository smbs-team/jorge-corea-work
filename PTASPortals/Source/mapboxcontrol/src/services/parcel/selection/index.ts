/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { cloneDeep, groupBy } from 'lodash';
import {
  Feature,
  Polygon,
  lineIntersect,
  polygonToLine,
  booleanWithin,
  polygon,
  booleanContains,
} from '@turf/turf';
import {
  ParcelClickEvent,
  ParcelFeature,
  ParcelFeatureData,
} from 'services/map/model';
import {
  DataSetDotProps,
  DotDsFeature,
  dotService,
  DotSourceKey,
} from 'services/map/dot';
import { mapBoxDrawService, $drawAreaUpdated } from 'services/map/mapboxDraw';
import { $onMapReady, $onParcelClick } from 'services/map/mapServiceEvents';
import { BehaviorSubject, Subject } from 'rxjs';
import { DEFAULT_DATASET_ID, QUERY_PARAM } from 'appConstants';
import { datasetService } from 'services/api';
import { mapUtilService, rendererService } from 'services/map';
import { CircleTextService } from './CircleTextService';
import { parcelUtil } from 'utils/parcelUtil';
import mapboxgl from 'mapbox-gl';
import MapboxDraw from '@mapbox/mapbox-gl-draw';
import { utilService } from 'services/common';
import { UpdateParcelSelectionData } from 'services/api/datasetService/DsSelection';

type ParcelSelectionOp =
  | 'polygon'
  | 'circle'
  | 'click'
  | 'disabled'
  | 'enabled';

type RenderOptions = { doApiRequest?: boolean; drawDots?: boolean };

/**
 * Parcel selection modes main handler class
 */
class SelectionService {
  /**
   *mapbox-gl map instance
   */
  private _map: mapboxgl.Map | undefined;

  /**
   * The current selected option
   */
  public selectedOp: ParcelSelectionOp = 'disabled';

  /**
   * A subject triggered when selected dots change
   */
  $onDotsChange = new BehaviorSubject<DotDsFeature[]>([]);
  /**
   * Store previous selected parcels here
   */
  prevSelectedDots: Map<string, DotDsFeature> = new Map();
  /**
   * Event fired when the selection mode changes
   */
  $onChangeSelection = new Subject<ParcelSelectionOp>();
  currentPagePin: string | undefined;
  readonly sourceKey: DotSourceKey = 'dot-service-selected';
  selectedDots = dotService.store[this.sourceKey] as Map<string, DotDsFeature>;
  constructor() {
    $onMapReady.subscribe(this._onMapReady);
    $drawAreaUpdated.subscribe(this._onDrawAreaUpdated);
    $onParcelClick.subscribe(this._selectParcelsByClick);
  }

  private _onMapReady = (map: mapboxgl.Map): void => {
    this._map = map;
    datasetService.featuresData.colorRenderer.$onFeaturesDataLoaded.subscribe(
      this._onFeaturesDataLoaded
    );
  };

  private _onFeaturesDataLoaded = (featuresData: ParcelFeatureData[]): void => {
    if (
      rendererService.getDatasetId() === DEFAULT_DATASET_ID &&
      utilService.getUrlSearchParam(QUERY_PARAM.DATASET_ID) === undefined
    )
      return;
    const grouped = groupBy(featuresData, (item) => item.Major + item.Minor);
    for (const arr of Object.values(grouped)) {
      const pin = arr[0].Major + arr[0].Minor;
      const selectedItem = arr.find(
        (item) => item.Selection || item.FilteredDatasetSelection
      );
      if (selectedItem) {
        if (!this.selectedDots.has(pin)) {
          this.createDotByPin(pin, true, 'feature-data');
        }
      } else {
        this.selectedDots.delete(pin);
      }
    }

    this.render({ doApiRequest: false });
  };

  private _onDrawAreaUpdated = (draw: MapboxDraw): void => {
    if (!this.isEnabled()) return;
    try {
      if (!this._map) return;
      if (mapBoxDrawService.measure?.enabled) return;
      new CircleTextService(this._map, draw).render();
      const fCollection = draw.getAll();
      if (!fCollection.features.length) {
        //  this.clearSelection();
        //  this._polygonId && this._clearPolygonDots(this._polygonId);
        // this._polygonId = undefined;
        return;
      }

      for (const [, dot] of this.selectedDots) {
        if (
          // dot.properties.selectionType === 'click-selection' ||
          dot.properties.selectionType === 'polygon-selection'
        ) {
          this.selectedDots.delete(dot.properties.pin);
        }
      }

      for (const f of fCollection.features) {
        if (f.geometry.type === 'Polygon') {
          this._selectParcelsByPolygon(f as Feature<Polygon>);
        }
      }
      this.render();
    } catch (e) {
      console.error(e);
    }
  };

  createDotByPin = (
    pin: string,
    selected: boolean,
    event: DotDsFeature['properties']['selectionType']
  ): void => {
    if (selected) {
      const parcelFeatures = parcelUtil.getFeaturesByPin(pin);
      const dot = this.createDot(parcelFeatures, {
        selectionType: event,
        refId: this.selectedDots.get(pin)?.properties.refId,
      });
      if (dot) {
        this.selectedDots.set(pin, dot);
      }
    } else {
      this.selectedDots.delete(pin);
    }
  };

  getSelectedDotsList = (): DotDsFeature[] =>
    [...this.selectedDots].flatMap(([, dot]) =>
      dot.properties.dotType === 'dataset-dot' ? dot : []
    ) as DotDsFeature[];

  /**
   * Toggle feature state selected and emits an event that adds/removes a pulsing dot from features
   */
  render = async (options?: RenderOptions): Promise<void> => {
    const sources: DotSourceKey[] = [];
    const doApiRequest = options?.doApiRequest ?? true;
    const drawDots = options?.drawDots ?? true;
    const updateData: UpdateParcelSelectionData[] = [];

    for (const [k, v] of this.prevSelectedDots) {
      if (!this.selectedDots.has(k)) {
        const replacedDot = dotService.findInSources(k, {
          omit: this.sourceKey,
        });
        if (replacedDot) {
          replacedDot.dot.properties.render = true;
          sources.push(replacedDot.source);
        }

        updateData.push({
          Major: v.properties.major ?? '',
          Minor: v.properties.minor ?? '',
          selection: false,
        });
      }
    }

    this.prevSelectedDots.clear();

    for (const [pin, dot] of this.selectedDots) {
      dot.properties.layer =
        this.currentPagePin === dot.properties.pin
          ? 'dot-service-selected-icon'
          : 'dot-service-selected-circle';
      const foundItem = dotService.findInSources(pin, {
        omit: this.sourceKey,
      });

      if (foundItem) {
        foundItem.dot.properties.render = false;
        sources.push(foundItem.source);
      }

      updateData.push({
        Major: dot.properties.major ?? '',
        Minor: dot.properties.minor ?? '',
        selection: true,
      });
    }

    if (updateData.length) {
      if (drawDots) {
        const renderSources: DotSourceKey[] = [
          ...new Set([this.sourceKey, ...sources]),
        ];
        dotService.render(renderSources);
      }
      this.$onDotsChange.next(this.getSelectedDotsList());
      this.prevSelectedDots = cloneDeep(
        this.selectedDots as Map<string, DotDsFeature>
      );
      if (doApiRequest) {
        const ds =
          utilService.getUrlSearchParam(QUERY_PARAM.DATASET_ID) ||
          rendererService.getDatasetId();
        if (!ds) return;
        if (ds === DEFAULT_DATASET_ID) return;
        await datasetService.DsSelection.update(updateData);
      }
    }
  };

  createDot = (
    _features: ParcelFeature[],
    { selectionType, refId }: Pick<DataSetDotProps, 'selectionType' | 'refId'>
  ): DotDsFeature | undefined => {
    if (!_features.length) return;
    const midPoint = parcelUtil.getPointFromFeatures(_features);
    if (!midPoint) return;
    const pin = _features[0].properties.PIN;
    return {
      ...midPoint,
      id: 'selection-point-' + pin,
      properties: {
        circleColor: dotService.dotColors.red,
        'icon-image': dotService.images.locationRed,
        layer: 'dot-service-selected-circle',
        selected: true,
        selectionType,
        pin,
        dotType: 'dataset-dot',
        major: _features[0].properties.MAJOR,
        minor: _features[0].properties.MINOR,
        refId,
      },
    };
  };

  /**
   * Clear this selection
   */
  clearSelection = (renderOptions?: RenderOptions): void => {
    this.selectedDots.clear();
    this.render(renderOptions);
  };

  /**
   * Option change
   * @param val -The new selected option
   *
   */
  changeSelection = (val: ParcelSelectionOp): void => {
    if (this.selectedOp === val) return;
    this.selectedOp = val;
    mapBoxDrawService.deleteAll();
    switch (val) {
      case 'circle': {
        mapBoxDrawService.setMode('draw_circle');
        break;
      }

      case 'polygon': {
        mapBoxDrawService.setMode('draw_polygon');
        break;
      }
      case 'click':
      case 'disabled':
      case 'enabled': {
        mapBoxDrawService.setMode('simple_select');
        break;
      }
    }
    this.$onChangeSelection.next(val);
  };

  /**
   * Add selected parcel by control click
   *
   * @param e - The event that contains the clicked point
   */
  _selectParcelsByClick = (e: ParcelClickEvent): void => {
    if (this.selectedOp !== 'click') return;
    if (mapBoxDrawService.draw?.getMode() !== 'simple_select') return;
    if (!e.feature) return;
    const selected = !!this.selectedDots.get(e.feature.properties.PIN);
    if (!mapUtilService.controlKeyPressed) {
      this.clearSelection();
    }
    if (selected) {
      this.selectedDots.delete(e.feature.properties.PIN);
    } else {
      const dot = this.createDot(
        parcelUtil.getFeaturesByPin(e.feature.properties.PIN),
        {
          selectionType: 'click-selection',
        }
      );

      dot && this.selectedDots.set(e.feature.properties.PIN, dot);
    }

    this.render();
  };

  private _clearPolygonDots = (polygonId: string): void => {
    for (const [k, v] of this.selectedDots) {
      if (v.properties.refId === polygonId) {
        this.selectedDots.delete(k);
      }
    }
  };

  /**
   * Add selected parcel by a drawing a polygon
   *
   * @param _polygonFeature - The drawn polygon
   */
  private _selectParcelsByPolygon = (
    _polygonFeature: Feature<Polygon>
  ): void => {
    if (!_polygonFeature.properties?.drawn) return;
    const polygonId = _polygonFeature.id?.toString() ?? '';
    const parcelFeatures = this._getCoveredByPolygonFeatures(_polygonFeature);
    this._clearPolygonDots(polygonId);
    for (const [pin, _featuresGroup] of Object.entries(parcelFeatures)) {
      const dot = this.createDot(_featuresGroup, {
        selectionType: 'polygon-selection',
        refId: polygonId,
      });
      dot && this.selectedDots.set(pin, dot);
    }
  };

  // /**
  //  * Add selected parcel by a drawing a line string
  //  *
  //  * @param _polygonFeature - The drawn line string
  //  */
  // selectParcelsByLineString(lineString: Feature<LineString>): void {
  //   const _featuresGroups = this.getCrossedByLineFeatures(
  //     featureCollection([lineString])
  //   );
  //   if (!_featuresGroups) return;
  //   for (const [pin, _features] of _featuresGroups) {
  //     const dot = this.getDot(_features, {
  //       // selectionType: 'line-selection',
  //     });
  //     dot && this.selectedDots.set(pin, dot);
  //   }
  // }

  /**
   * Get the parcel features crossed by the line
   *
   * @param _featureCollection - A feature collection with the linestring and points
   * @returns The parcel intersected features by the line
   */
  // private _getCrossedByLineFeatures = (
  //   _featureCollection: FeatureCollection<LineString>
  // ): Map<string, ParcelFeature[]> | undefined => {
  //   if (!_featureCollection.features.length) return;
  //   if (!this._map) return;
  //   const foundFeatures = new Map<string, ParcelFeature[]>();
  //   const line = _featureCollection.features[0];
  //   if (line) {
  //     for (const [pin, _featureGroup] of Object.entries(
  //       groupBy(
  //         parcelUtil
  //           .getRenderedFeatures()
  //           .filter(
  //             (val) => lineIntersect(line, polygonToLine(val)).features.length
  //           ),
  //         (val) => val.properties.PIN
  //       )
  //     )) {
  //       foundFeatures.set(pin, _featureGroup);
  //     }
  //   }

  //   if (!foundFeatures.size) {
  //     const _coords = chain(coordAll(_featureCollection))
  //       .thru((val) => multiPoint(val))
  //       .thru((val) => cleanCoords(val))
  //       .value()?.geometry?.coordinates;
  //     if (!_coords) return;
  //     for (const _point of _coords) {
  //       const _features = this._map
  //         .queryRenderedFeatures(
  //           this._map.project(_point as [number, number]),
  //           {
  //             layers: [AppLayers.PARCEL_LAYER],
  //           }
  //         )
  //         .map((val) => new ParcelFeature(val as ParcelFeature));

  //       _features.length &&
  //         foundFeatures.set(_features[0]?.properties.PIN, _features);
  //     }
  //   }

  //   return foundFeatures;
  // };

  private _polygonsIntercepts = (
    polygon1: Feature<Polygon>,
    polygon2: Feature<Polygon>
  ): boolean =>
    booleanWithin(polygon2, polygon1) ||
    !!lineIntersect(polygonToLine(polygon2), polygonToLine(polygon1)).features
      .length;

  private _getCoveredByPolygonFeatures = (
    drawPolygon: Feature<Polygon>
  ): Record<string, ParcelFeature[]> => {
    return groupBy(
      parcelUtil.getRenderedFeatures().filter((parcelFeature) => {
        if (parcelFeature.geometry.type === 'Polygon') {
          const parcelPolygon = (parcelFeature as unknown) as Feature<Polygon>;
          return (
            this._polygonsIntercepts(drawPolygon, parcelPolygon) ||
            booleanContains(parcelPolygon, drawPolygon)
          );
        }
        return parcelFeature.geometry.coordinates.some((coords) => {
          const parcelPolygon = polygon(coords);
          return (
            this._polygonsIntercepts(drawPolygon, polygon(coords)) ||
            booleanContains(parcelPolygon, drawPolygon)
          );
        });
      }),
      (val) => val.properties.PIN
    );
  };

  isEnabled = (): boolean => this.selectedOp !== 'disabled';
}

export default new SelectionService();
