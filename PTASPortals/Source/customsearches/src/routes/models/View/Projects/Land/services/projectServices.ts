// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */


import { GetProjectHealthType } from 'services/map.typings';
import { AxiosLoader } from 'services/AxiosLoader';


export const getProjectHealth = (projectId: number | string): Promise<GetProjectHealthType | null> => {
    const ad2 = new AxiosLoader<GetProjectHealthType, {}>();
    return ad2.GetInfo(
      `CustomSearches/GetProjectHealth/${projectId}`,
      {}
    );
  };