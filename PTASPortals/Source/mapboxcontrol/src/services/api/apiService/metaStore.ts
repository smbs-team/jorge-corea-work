/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ApiServiceResult, handleReq } from 'services/common';
import { axiosCsInstance } from '../axiosInstances';
import {
  GlobalStoreItem,
  GlobalStoreItemId,
  UserStoreItem,
  UserStoreItemId,
} from './types';

/**
 *
 * @param id - The store item id
 * @returns -ApiServiceResult with the specified object T
 */
export const getUserStoreItem = async <T>(
  id: UserStoreItemId
): Promise<ApiServiceResult<{ itemId: number; value: T } | undefined>> =>
  handleReq(async () => {
    const r = (
      await axiosCsInstance.get<{
        userDataStoreItems: UserStoreItem<T>[];
      }>(`/Shared/GetUserDataStoreItems/${id}`)
    ).data.userDataStoreItems.shift();

    return new ApiServiceResult<{ itemId: number; value: T }>({
      data: r
        ? {
            itemId: r.userDataStoreItemId,
            value: r.value,
          }
        : undefined,
    });
  });
/**
 *
 * @param item - The item to be saved
 * @returns - An object with the record id
 */
export const saveUserStoreItem = async (
  item: Omit<UserStoreItem, 'userDataStoreItemId'>
): Promise<ApiServiceResult<{ id: number }>> =>
  handleReq(
    async () =>
      new ApiServiceResult({
        data: (
          await axiosCsInstance.post<{ data: { id: number } }>(
            `/Shared/ImportUserDataStoreItem`,
            item
          )
        ).data.data,
      })
  );

/**
 *
 * @param id -The store item id to be fetched
 * @returns - The item value or undefined
 */
export const getGlobalStoreItem = async <T>(
  id: GlobalStoreItemId
): Promise<ApiServiceResult<T | undefined>> =>
  handleReq(async () => {
    return new ApiServiceResult<T>({
      data: (
        await axiosCsInstance.get<{ metadataStoreItems: GlobalStoreItem<T>[] }>(
          `/Shared/GetMetadataStoreItem/mapboxcontrol/${id}`
        )
      ).data.metadataStoreItems.shift()?.value,
    });
  });

/**
 *
 * @param item - The global store item to be saved
 * @returns - ApiServiceResult<undefined>
 */
export const saveGlobalStoreItem = async (
  item: GlobalStoreItem
): Promise<ApiServiceResult<undefined>> =>
  handleReq(
    async () =>
      new ApiServiceResult({
        data: (
          await axiosCsInstance.post('/Shared/ImportMetadataStoreItems', item)
        ).data,
      })
  );

export const deleteUserDataStoreItem = async (
  id: string | number
): Promise<void> =>
  await axiosCsInstance.post('/Shared/DeleteUserDataStoreItem/' + id);
