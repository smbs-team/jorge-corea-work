// types.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { TreeViewRow } from '@ptas/react-ui-library';
import { Folder } from 'services/map/model';

/**
 * Column definition for maps edit tree view
 */
export interface MapsTreeViewColumn {
  name: Exclude<keyof MapsTreeViewRow, keyof TreeViewRow> | 'menu';
  title: string;
}

export interface MapsTreeViewRow
  extends Omit<TreeViewRow, 'isEmptyGroup'>,
    Pick<Folder, 'rootFolder'> {
  title: string;
  isLocked?: boolean | undefined;
  lastModifiedBy?: string | undefined;
  subject: string;
  value?: string;
  isEditable?: boolean;
  folderPath?: string;
  createdBy?: string;
}
