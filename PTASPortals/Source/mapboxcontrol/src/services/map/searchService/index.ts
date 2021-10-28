/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { wrap } from 'lodash';
import { Api } from './Api';
import { mapUtilService } from '../mapUtilService';
import {
  bbox,
  featureCollection,
  FeatureCollection,
  Point,
  point,
} from '@turf/turf';
import { DotSearchFeature, dotService } from '../dot';
import { parcelUtil } from 'utils/parcelUtil';
import { $onParcelStack } from '../mapServiceEvents';
import { ParcelInfoCtxProps } from 'contexts/ParcelInfoContext';

interface ConstructorProps {
  map: mapboxgl.Map;
}

/**
 * Search related tasks service
 */
export class SearchService extends Api {
  /**
   * mapbox-gl map instance
   */
  private _map: mapboxgl.Map;

  /**
   * Creates an instance of MapSearchService.
   *
   * @param fields - Constructor arguments
   */

  private _dotSource = dotService.store['dot-service-search'];

  constructor(fields: ConstructorProps) {
    super();
    this._map = fields.map;
    this.runParcelSearch(parcelUtil.urlParcelsQuery.join());
  }

  getPointFeaturesStack = (
    p: mapboxgl.Point
  ): ParcelInfoCtxProps['parcelStack'] | undefined => {
    const features = parcelUtil.getFeaturesByPoint(p);
    if (features && features.length > 1) {
      return features.map((item) => {
        return {
          major: item.properties?.MAJOR ?? '',
          minor: item.properties?.MINOR ?? '',
        };
      });
    }
  };

  /**
   * Execute the search parcel query
   *
   * @param q - The search query
   * @returns - Matched parcel information result
   */
  runParcelSearch = async (q: string): Promise<void> => {
    if (!q.length) return;
    this._dotSource?.clear();
    if (parcelUtil.isPin(q)) {
      return await this._searchParcelByPin(q);
    } else if (q.split(',').every(parcelUtil.isPin)) {
      return await this._searchByMultipleParcelsIds(q.split(','));
    } else {
      return await this._searchByAddress(q);
    }
  };

  /**
   * Execute the search parcel address specific query
   *
   * @param val - The parcel address to be searched
   * @returns - Matched parcel information result
   */
  private _searchByAddress = async (val: string): Promise<void> => {
    const r = (await this.getParcelByAddress(val)).shift();
    if (!r) return;
    await mapUtilService.mapFly(this._map, [r.longitude, r.laitude]);

    this._dotSource?.set((r.longitude, r.laitude).toString(), {
      geometry: {
        type: 'Point',
        coordinates: [r.longitude, r.laitude],
      },
      type: 'Feature',
      properties: {
        major: '',
        minor: '',
        circleColor: dotService.dotColors.blue,
        pin: '',
        dotType: 'search',
      },
      id: (r.longitude, r.laitude).toString(),
    });
    dotService.render('dot-service-search');
  };

  /**
   * Search by multiple parcel id, and adjust the screen to display the pulsing dots
   *
   * @remarks
   * Example: http://localhost:3000?parcelsQuery=0016000460,0016000480,0016000491,0016000610,9499200015,3224900070,3224900010
   *
   * @param ids - An array with the parcels pins
   */
  private _searchByMultipleParcelsIds = async (
    ids: string[]
  ): Promise<void> => {
    const boundFitOptions: mapboxgl.FitBoundsOptions = {
      padding: { top: 50, bottom: 50, left: 50, right: 50 },
      maxZoom: 18,
      duration: 0,
    };
    const _fCollection: FeatureCollection<
      Point,
      { pin: string }
    > = featureCollection([]);
    for (const item of ids) {
      _fCollection.features.push(
        point((await this.getParcelByPin(item)).center, {
          pin: item,
        })
      );
    }

    this._map
      .fitBounds(
        bbox(_fCollection) as [number, number, number, number],
        boundFitOptions
      )
      .once('idle', async () => {
        if (parcelUtil.isEconomicUnitSearch) {
          const firstFeature = _fCollection.features.shift();
          if (!firstFeature) return;
          if (!firstFeature.properties?.pin) return;
          dotService.store['dot-service-search']?.set(
            firstFeature.properties.pin,
            {
              ...firstFeature,
              id: firstFeature.properties.pin,
              properties: {
                dotType: 'search',
                major: firstFeature.properties.pin.substring(0, 6),
                minor: firstFeature.properties.pin.substring(6, 10),
                pin: firstFeature.properties.pin,
                'icon-image': dotService.images.starIcon,
                'icon-anchor': 'top',
              },
            }
          );
          if (firstFeature.geometry?.coordinates.length) {
            const stack = this.getPointFeaturesStack(
              this._map.project(
                firstFeature.geometry.coordinates as [number, number]
              )
            );
            if (stack?.length) {
              $onParcelStack.next(stack);
            }
          }
        }

        for (const _point of _fCollection.features) {
          if (!_point.properties?.pin) return;
          const { pin } = _point.properties;
          if (dotService.findInSources(pin)) continue;
          this._dotSource?.set(pin, {
            ..._point,
            properties: {
              dotType: 'search',
              circleColor: dotService.dotColors.blue,
              pin,
              major: pin.substring(0, 6),
              minor: pin.substring(6, 10),
            },
            id: pin,
          });
        }

        dotService.render('dot-service-search');
      });
  };

  /**
   * parcel by id search
   *
   * @param pin - The parcel pin to be searched
   * @returns In case of match, the found parcel information result, otherwise undefined
   *
   */
  private _searchParcelByPin: (pin: string) => Promise<void> = wrap(
    async (pin: string): Promise<void> => {
      const { min, max, center } = await this.getParcelByPin(pin);
      this._map
        .fitBounds([min, max], {
          padding: { top: 50, bottom: 50, left: 50, right: 50 },
          maxZoom: 18,
          duration: 0,
        })
        .once('idle', async () => {
          if (dotService.findInSources(pin)) return;
          const props: DotSearchFeature['properties'] = {
            dotType: 'search',
            pin,
            major: pin.substring(0, 6),
            minor: pin.substring(6, 10),
            circleColor: dotService.dotColors.blue,
          };
          const stack = this.getPointFeaturesStack(this._map.project(center));
          if (stack?.length) {
            $onParcelStack.next(stack);
          }
          this._dotSource.set(pin, {
            id: pin,
            properties: props,
            type: 'Feature',
            geometry: { type: 'Point', coordinates: center },
          });
          dotService.render('dot-service-search');
        });
    },
    (fn, pin) => fn(pin.replace(/-/g, ''))
  );
}
