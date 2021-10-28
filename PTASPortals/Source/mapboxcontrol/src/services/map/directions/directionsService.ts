// directionsService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import axios from 'axios';
import { ApiServiceResult, handleReq } from 'services/common';
import { MapboxDirections } from '../model/directions';

/**
 * General info about navigation in mapbox: https://docs.mapbox.com/help/how-mapbox-works/directions/
 * Mapbox Directions API docs:  https://docs.mapbox.com/help/tutorials/getting-started-directions-api/
 *                              https://docs.mapbox.com/api/navigation/#directions
 */
class DirectionsService {
  async getDirections(
    profile: 'walking' | 'driving' | 'cycling',
    originCoords: { lat: number; lng: number },
    destinationCoords: { lat: number; lng: number }
  ): Promise<ApiServiceResult<MapboxDirections | undefined>> {
    const url =
      process.env.REACT_APP_MAPBOX_DIRECTIONS_API_URL +
      profile +
      '/' +
      `${originCoords.lng},${originCoords.lat};` +
      `${destinationCoords.lng},${destinationCoords.lat}` +
      `?geometries=geojson&access_token=${process.env.REACT_APP_MAPBOX_GL_ACCESS_TOKEN}`;
    return handleReq(async () => {
      const directionsRes = (await axios.get(url)).data;

      return new ApiServiceResult({
        data:
          typeof directionsRes === 'object'
            ? (directionsRes as MapboxDirections)
            : undefined,
      });
    });
  }

  async getWalkingDistance(
    originCoords: { lat: number; lng: number },
    destinationCoords: { lat: number; lng: number }
  ): Promise<
    { distance: number; directionsData: MapboxDirections } | undefined
  > {
    if (
      !originCoords ||
      originCoords.lat === undefined ||
      originCoords.lng === undefined
    )
      return;
    if (
      !destinationCoords ||
      destinationCoords.lat === undefined ||
      destinationCoords.lng === undefined
    )
      return;

    const directions = await this.getDirections(
      'walking',
      originCoords,
      destinationCoords
    );
    if (!directions.data || !directions.data.routes?.length) return;

    const distanceInMeters = directions.data.routes[0].distance;
    return {
      distance: distanceInMeters * 3.28084, //feet
      directionsData: directions.data,
    };
  }

  async getDrivingTime(
    originCoords: { lat: number; lng: number },
    destinationCoords: { lat: number; lng: number }
  ): Promise<
    { duration: number; directionsData: MapboxDirections } | undefined
  > {
    if (
      !originCoords ||
      originCoords.lat === undefined ||
      originCoords.lng === undefined
    )
      return;
    if (
      !destinationCoords ||
      destinationCoords.lat === undefined ||
      destinationCoords.lng === undefined
    )
      return;

    const directions = await this.getDirections(
      'driving',
      originCoords,
      destinationCoords
    );
    if (!directions.data || !directions.data.routes?.length) return;

    const durationInSeconds = directions.data.routes[0].duration;

    return {
      duration: durationInSeconds / 60, //minutes
      directionsData: directions.data,
    };
  }

  async getCyclingTimeDistance(
    originCoords: { lat: number; lng: number },
    destinationCoords: { lat: number; lng: number }
  ): Promise<{ distance: number; duration: number } | undefined> {
    if (
      !originCoords ||
      originCoords.lat === undefined ||
      originCoords.lng === undefined
    )
      return;
    if (
      !destinationCoords ||
      destinationCoords.lat === undefined ||
      destinationCoords.lng === undefined
    )
      return;

    const directions = await this.getDirections(
      'cycling',
      originCoords,
      destinationCoords
    );
    if (!directions.data || !directions.data.routes?.length) return;

    const durationInSeconds = directions.data.routes[0].duration;
    const distanceInMeters = directions.data.routes[0].distance;

    return {
      distance: distanceInMeters * 3.28084, //feet
      duration: durationInSeconds / 60, //minutes
    };
  }
}

export default new DirectionsService();
