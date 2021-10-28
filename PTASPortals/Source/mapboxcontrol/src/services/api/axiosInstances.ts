/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import axios from 'axios';
import { tokenService } from 'services/common/tokenService';

tokenService.$onB2CTokenInit.subscribe((token) => {
  for (const instance of [
    axiosCsInstance,
    axiosDataServiceInstance,
    axiosMapTileInstance,
  ]) {
    instance.defaults.headers.common['Authorization'] = 'Bearer ' + token;
  }
});

export const axiosCsInstance = axios.create({
  timeout: 60 * 2 * 1000,
  baseURL: `${process.env.REACT_APP_CUSTOM_SEARCHES_URL}`,
});

export const axiosInstance = axios.create({
  timeout: 60 * 2 * 1000,
});

export const axiosMapTileInstance = axios.create({
  baseURL: `${process.env.REACT_APP_MAP_TILE_SERVICE_HOST}v1.0`,
  timeout: 60 * 2 * 1000,
});

export const axiosDataServiceInstance = axios.create({
  baseURL: process.env.REACT_APP_DATA_SERVICE_API_URL,
  timeout: 60 * 2 * 1000,
});
