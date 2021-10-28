// filename
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AxiosLoader } from './AxiosLoader';
import { CurrentUserInfoType } from './map.typings';

export const getCurrentUserInfo =
  async (): Promise<CurrentUserInfoType | null> => {
    try {
      const loader = new AxiosLoader<CurrentUserInfoType, {}>();
      const info = await loader.GetInfo(`Auth/GetCurrentUserInfo`, {});
      return info;
    } catch (error) {
      console.log(`error`, error);
      return null;
    }
  };
