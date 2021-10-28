/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import mapboxgl from 'mapbox-gl';

export default (map: mapboxgl.Map): void => {
  return navigator.geolocation.getCurrentPosition(
    function (position) {
      const pos = {
        lat: position.coords.latitude,
        lng: position.coords.longitude,
      };

      map.flyTo({
        center: pos,
        animate: false,
      });
    },
    function () {
      console.error('Error navigating to user position');
    }
  );
};
