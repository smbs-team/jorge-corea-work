// filename
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AxiosLoader } from './AxiosLoader';

//eslint-disable-next-line
export const freezeProject = async (projectId: number): Promise<any> => {
  const loader = new AxiosLoader<{}, {}>();
  const data = await loader.PutInfo(
    `CustomSearches/FreezeProject/${projectId}`,
    {},
    {}
  );
  console.log(`data`, data);
  return data;
};
