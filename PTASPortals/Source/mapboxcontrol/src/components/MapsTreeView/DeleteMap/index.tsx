/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { Alert } from '@ptas/react-ui-library';
import { MapsTreeViewRow } from '../types';
import { useDeleteFolder } from './useDeleteFolder';
import { useConfirmDeleteUserMap } from './useConfirmDeleteUserMap';

type Props = { row: MapsTreeViewRow };

export default function DeleteMap({ row }: Props): JSX.Element {
  const deleteFolder = useDeleteFolder();
  const confirmDeleteUserMap = useConfirmDeleteUserMap();

  return (
    <Alert
      contentText={
        row.folder
          ? `Deleting permanently erases the folder ${row.title} and its contents.`
          : `Deleting permanently erases ${row.title} map.`
      }
      okButtonText={row.folder ? 'Delete this folder' : 'Delete this user map'}
      okButtonClick={(): void => {
        if (row.folder) {
          deleteFolder(row);
        } else {
          confirmDeleteUserMap(row);
        }
      }}
    />
  );
}
