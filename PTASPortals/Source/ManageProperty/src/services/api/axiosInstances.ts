// axiosInstance.ts
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

const token =
  'eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IkVMOExuLTFQWUo1WlNheEZDYWxrYms0WDBZZFV1dkMxVmdDN0tfM241RGcifQ.eyJpc3MiOiJodHRwczovL2tjYjJjc2FuZGJveC5iMmNsb2dpbi5jb20vMjgzOGZiMTktMjAzZS00ZjhmLWI2NDQtNDE1NGVmOGVhZDA1L3YyLjAvIiwiZXhwIjoxNjI0NDAwMDEwLCJuYmYiOjE2MjM3OTUyMTAsImF1ZCI6IjllYzQ4ZGE4LTM5OTUtNDE4MS04MzM4LTI1ZGEyY2YxNjYxYiIsInFyVG9rZW4iOiIiLCJzdWIiOiJuLWFicmVuZXNAa2luZ2NvdW50eS5nb3YiLCJhY2NvdW50RW5hYmxlZCI6dHJ1ZSwidGlkIjoiMjgzOGZiMTktMjAzZS00ZjhmLWI2NDQtNDE1NGVmOGVhZDA1Iiwibm9uY2UiOiJmM2NiN2IyODVmNjQ0NWM0ODk1ZTk4MTUzOWRmZTNjMyIsInNjcCI6InVzZXJfaW1wZXJzb25hdGlvbiIsImF6cCI6IjllYzQ4ZGE4LTM5OTUtNDE4MS04MzM4LTI1ZGEyY2YxNjYxYiIsInZlciI6IjEuMCIsImlhdCI6MTYyMzc5NTIxMH0.qs5oXjaTyyKVin5T5Ye-gDFn32JCkZnKutbAseBwT1Y0r7OvT8PSVbIYx0KhLMEPjzkoZBZ1EITZCuVYGDFOliu5pKGe6QtTr-t_Qr35zeCmVe_5-eBN67L5GVFKE3dOF6Th1-jCarlKFM61CYKmlPMR6w3ywYIG61RRvBFEU74D7Vqyfb_LKmA7_IMxlAQJ14JAIqjvt5z0MGepdG-RBjETSVTM4l18nYVJacWVs5XRAyVGVzjw5dM8BdMEJY3-M2-p22NUs2F92cqUuHslpjv7mPWy7PDX8uLLP3bYwhgj3Tde-dPczcHkynCoVR2bMivA_aSaLM8YtAGw-9Pe-A';

export const axiosMpInstance = axios.create({
  timeout: 60 * 2 * 1000,
  baseURL: `${process.env.REACT_APP_MANAGE_PROPERTY_LOCALHOST_URL}`,
});

export const axiosHiInstance = axios.create({
  timeout: 60 * 2 * 1000,
  baseURL: `${process.env.REACT_APP_HOME_IMPROVEMENT_API_URL}`,
  headers: {
    Authorization: `Bearer ${process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP}`,
  },
});

export const axiosInstance = axios.create({
  timeout: 60 * 2 * 1000,
  headers: {
    Authorization: `Bearer ${token}`,
  },
});

export const axiosCsInstance = axios.create({
  timeout: 60 * 2 * 1000,
  baseURL: `${process.env.REACT_APP_CUSTOM_SEARCHES_URL}`,
  headers: {
    Authorization: `Bearer ${token}`,
  },
});

export const axiosFileInstance = axios.create({
  timeout: 60 * 2 * 1000,
  baseURL: `${process.env.REACT_APP_URL_UPLOAD_FILE}`,
  headers: {
    Authorization: `Bearer ${process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP}`,
  },
});

export const axiosSharePointInstance = axios.create({
  timeout: 60 * 2 * 1000,
  baseURL: `${process.env.REACT_APP_URL_SHAREPOINT_FILES}`,
  headers: {
    Authorization: `Bearer ${process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP}`,
  },
});

axiosInstance.interceptors.response.use(res => res, exceptionError);
axiosMpInstance.interceptors.response.use(res => res, exceptionError);
axiosHiInstance.interceptors.response.use(res => res, exceptionError);
axiosCsInstance.interceptors.response.use(res => res, exceptionError);
axiosFileInstance.interceptors.response.use(res => res, exceptionError);
axiosSharePointInstance.interceptors.response.use(res => res, exceptionError);
