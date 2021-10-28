/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import axios from 'axios';
import { center, featureCollection, point } from '@turf/turf';
import { wrap } from 'lodash';
import {
  SearchByParcelResult,
  ParcelByAddressItem,
  GetParcelByAddressMapBoxApiRes,
} from '../types';
import { axiosMapTileInstance } from 'services/api/axiosInstances';
import { MAPBOX_SEARCH_API_PARAMS } from 'appConstants';

/**
 * A service that makes the http calls
 */
export class Api {
  private readonly _reqTimeout = 60000;
  /**
   * Search parcel by pin
   *
   * @remarks
   * Notice: In case of fail we have pending to display a message to the user indicating the error
   *
   * @param parcelId - The parcel to be search
   * @returns -Parcel coordinates
   */
  getParcelByPin: (pin: string) => Promise<SearchByParcelResult> = wrap(
    async (pin: string) => {
      const url = `/geoLocation/${pin}`;
      const { data } = await axiosMapTileInstance.get<SearchByParcelResult>(
        url,
        {
          timeout: this._reqTimeout,
        }
      );
      const min = Object.values(data.min) as [number, number];
      const max = Object.values(data.max) as [number, number];
      const _centerCoords = center(featureCollection([point(min), point(max)]))
        .geometry?.coordinates as [number, number];
      return {
        min,
        max,
        center: _centerCoords,
      };
    },
    (fn, pin) => fn(pin.replace(/-/g, ''))
  );

  /**
   * Get parcel by address
   *
   * @param address -The parcel address
   * @returns -Parcel information that matches the address
   */
  async getParcelByAddress(address: string): Promise<ParcelByAddressItem[]> {
    const { data } = await axios.get<GetParcelByAddressMapBoxApiRes>(
      process.env.REACT_APP_MAPBOX_SEARCH_API_URL + address + '.json',
      {
        params: MAPBOX_SEARCH_API_PARAMS,
      }
    );

    return data.features.map((f) => ({
      longitude: f.center[0],
      laitude: f.center[1],
      formattedaddr: f.matching_place_name || f.place_name || '',
      country: '',
      relevance: f.relevance,
      state: '',
      streetname: '',
    }));
  }
}
