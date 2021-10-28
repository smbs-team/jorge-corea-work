import { ErrorSnackStoreProps } from './../stores/useErrorSnackStore';
// AxiosLoader.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import axios, { AxiosResponse, AxiosRequestConfig } from 'axios';

const TOKEN = localStorage.getItem('magicToken');
const TIMEOUT = 4 * 60 * 1000;

const api = axios.create({
  baseURL: process.env.REACT_APP_CUSTOM_SEARCHES_URL,
  headers: {
    Authorization: `Bearer ${TOKEN}`,
  },
  timeout: TIMEOUT,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('magicToken');
  if (token != null) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const setUpInterceptors = (errorStore: ErrorSnackStoreProps): void => {
  api.interceptors.response.use(
    (response) => response,
    (error) => {
      errorStore.newError(undefined, error.message);
    }
  );
};

export const request = <T>(
  config: AxiosRequestConfig
): Promise<AxiosResponse<T>> => {
  return api.request<T>(config);
};

export const get = <T>(
  url: string,
  config?: AxiosRequestConfig
): Promise<AxiosResponse<T>> => {
  return api.get(url, config);
};

export const post = <T>(
  url: string,
  data?: unknown,
  config?: AxiosRequestConfig
): Promise<AxiosResponse<T>> => {
  return api.post(url, data, config);
};
