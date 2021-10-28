/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ColorConfiguration } from '@ptas/react-ui-library';
import { UserMap } from 'services/map/model/userMap';

export type UserStoreItemId =
  | 'selected-system-user-map'
  | 'color-ramp'
  | 'default-map';

export type GlobalStoreItemId = 'default-map-c' | 'default-map-r';

export type GlobalStoreItemValue = {
  [k in 'default-map-c' | 'default-map-r']: { id: number };
};

export type StoreItemValue = {
  'selected-system-user-map': {
    usermap: UserMap['userMapId'];
  };
  'color-ramp': ColorConfiguration[];
  'default-map': {
    mapId: UserMap['userMapId'];
  };
};

export type UserStoreItem<T = object> = {
  userDataStoreItemId: number;
  storeType: UserStoreItemId;
  ownerType?: 'NoOwnerType' | 'MapRenderer';
  ownerObjectId?: string;
  itemName?: string;
  value: T;
};

export type GetLayerDownloadUrl = {
  fileSize: number;
  url: string;
};

export type GlobalStoreItem<T = object> = {
  storeType: string;
  itemName: string;
  value: T;
};
