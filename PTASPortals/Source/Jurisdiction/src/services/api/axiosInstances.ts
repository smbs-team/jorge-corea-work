//axiosInstances.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import axios from 'axios';
import { appInsights } from '../appInsights';

const exceptionError = (error: Error): Promise<Error> => {
  appInsights.trackException({
    exception: error,
  });

  return Promise.reject(error);
};

export const axiosInstance = axios.create({
  timeout: 60 * 2 * 1000,
  baseURL: `${process.env.REACT_APP_JURISDICTION_API_URL}`,
  headers: {
    Authorization: `Bearer ${process.env.REACT_APP_MAGIC_TOKEN_TEMP}`,
  },
});

export const axiosFileInstance = axios.create({
  timeout: 60 * 2 * 1000,
  baseURL: `${process.env.REACT_APP_URL_UPLOAD_FILE}`,
  headers: {
    Authorization: `Bearer ${process.env.REACT_APP_MAGIC_TOKEN_TEMP}`,
  },
});

axiosInstance.interceptors.response.use((res) => res, exceptionError);
axiosFileInstance.interceptors.response.use((res) => res, exceptionError);
