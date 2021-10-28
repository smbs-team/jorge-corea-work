/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ErrorMessageAlertCtx } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import { useContext } from 'react';
import { apiService } from 'services/api';
import { mapService } from 'services/map';
import { getFolderPath } from 'utils/componentUtil';
import { MapsTreeViewRow } from '../types';

type UseDeleteFolder = (row: MapsTreeViewRow) => void;

export const useDeleteFolder = (): UseDeleteFolder => {
  const { setLinearProgress, mapsTreeView } = useContext(HomeContext);
  const { rows } = mapsTreeView;
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  return async (row: MapsTreeViewRow): Promise<void> => {
    try {
      setLinearProgress(true);
      await apiService.deleteFolder({
        folderPath: getFolderPath<MapsTreeViewRow>(row, rows),
        folderItemType: 'UserMap',
      });
      mapService.refreshUserMapFolders();
    } catch (error) {
      showErrorMessage({
        message: 'Delete folder failed',
      });
    } finally {
      setLinearProgress(false);
    }
  };
};
