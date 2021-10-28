/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { useContext } from 'react';
import { SnackContext } from '@ptas/react-ui-library';
import { apiService } from 'services/api';
import { mapService } from 'services/map';
import userService from 'services/user/userService';
import { HomeContext } from 'contexts';

type UseCreateFolder = (
  folderPath: string,
  options?: { silent?: boolean }
) => Promise<number | undefined>;

export const useCreateFolder = (): UseCreateFolder => {
  const { setLinearProgress } = useContext(HomeContext);
  const { setSnackState } = useContext(SnackContext);

  const createFolder = async (
    folderPath: string,
    options?: { silent?: boolean }
  ): Promise<number | undefined> => {
    try {
      if (options?.silent !== true) {
        setLinearProgress(true);
      }
      const r = await apiService.createFolder({
        FolderPath: folderPath,
        FolderItemType: 'UserMap',
        UserId: userService.userInfo.id,
      });
      if (r.hasError) throw new Error(r.errorMessage);
      if (options?.silent !== true) {
        await mapService.refreshUserMapFolders();
      }
      return r.data;
    } catch (e) {
      console.error(e);
      setSnackState({
        severity: 'error',
        text: 'Could not create folder ' + folderPath,
      });
    } finally {
      if (options?.silent !== true) {
        setLinearProgress(false);
      }
    }
  };

  return createFolder;
};
