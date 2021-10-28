// filename
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AxiosLoader } from './AxiosLoader';
import { GetUserJobNotifications } from './map.typings';

export const getUserJobNotifications =
  (): Promise<GetUserJobNotifications | null> => {
    const loader = new AxiosLoader<GetUserJobNotifications, {}>();
    return loader.GetInfo(`Jobs/GetUserJobNotifications`, {
      maxJobs: process.env.REACT_APP_MAX_JOBS,
      maxNotifications: process.env.REACT_APP_MAX_NOTIFICATIONS,
    });
  };

export const dismissNotification = async (
  notificationId: number | string
  //eslint-disable-next-line
): Promise<any> => {
  const loader = new AxiosLoader<{}, {}>();
  const data = await loader.PutInfo(
    `Jobs/DismissUserJobNotification/${notificationId}`,
    {},
    {}
  );
  return data;
};
