/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ErrorMessageAlertCtx } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import { useContext } from 'react';
import { mapService } from 'services/map';
import { MapsTreeViewRow } from '../types';

export const useConfirmDeleteUserMap = (): ((
  row: MapsTreeViewRow | undefined
) => Promise<void>) => {
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const { setLinearProgress } = useContext(HomeContext);
  return async (row: MapsTreeViewRow | undefined): Promise<void> => {
    try {
      if (typeof row?.id !== 'number') return;
      setLinearProgress(true);
      const r = await mapService.deleteUserMap(row.id);
      if (r.hasError) throw new Error(r.errorMessage);
      await mapService.refreshUserMaps();
    } catch (e) {
      showErrorMessage({
        message: 'Error deleting user map ' + row?.title,
      });
    } finally {
      setLinearProgress(false);
    }
  };
};
