/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useContext } from 'react';
import { ErrorMessageAlertCtx } from '@ptas/react-ui-library';
import { MapsTreeViewRow } from './types';
import { HomeContext } from 'contexts';
import { apiService } from 'services/api';
import { getFolderPath } from 'utils/componentUtil';
import { mapService } from 'services/map';

type UseRename = (
  editedRow: MapsTreeViewRow | undefined,
  originalRow: MapsTreeViewRow | undefined
) => Promise<void>;

export const useRename = (): UseRename => {
  const homeContext = useContext(HomeContext);
  const { rows, setRows } = homeContext.mapsTreeView;
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  return async (
    editedRow: MapsTreeViewRow | undefined,
    originalRow: MapsTreeViewRow | undefined
  ): Promise<void> => {
    if (!editedRow) return;
    if (!editedRow.title.trim()) {
      showErrorMessage({
        message: 'Name should not be empty',
        detail: 'Name should not be empty',
      });
      setRows(
        rows.map((r) =>
          r.id === editedRow.id ? { ...r, name: originalRow?.title } : r
        )
      );
      return;
    }

    let newName = editedRow.title;
    if (newName.length > 64) {
      newName = newName.substr(0, 64);
      showErrorMessage({
        message: 'Name limited to 64 characters',
        detail: 'Name limited to 64 characters',
      });
    }

    if (
      rows
        .filter((r) => r.parent === editedRow.parent)
        .find((r) => r.title === newName)
    ) {
      showErrorMessage({
        message: 'Name already exists',
        detail: 'Name already exists',
      });
      setRows(
        rows.map((r) =>
          r.id === editedRow.id ? { ...r, name: originalRow?.title } : r
        )
      );
      return;
    }

    if (editedRow.folder) {
      if (!originalRow) return;
      try {
        await apiService.renameFolder({
          folderPath: getFolderPath<MapsTreeViewRow>(originalRow, rows),
          folderItemType: 'UserMap',
          newName: newName,
        });
        mapService.refreshUserMaps();
      } catch (error) {
        showErrorMessage({
          message: 'Rename Failed',
          detail: 'Rename Failed',
        });
        mapService.refreshUserMapFolders();
      }
    } else {
      if (!originalRow) return;
      try {
        await mapService.renameUserMap(editedRow.id as number, newName);
        mapService.refreshUserMaps();
      } catch (error) {
        showErrorMessage({
          message: 'Rename Failed',
          detail: 'Rename Failed',
        });
      }
    }
  };
};
