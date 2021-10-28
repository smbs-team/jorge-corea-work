/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { SYSTEM_RENDERER_FOLDER } from 'appConstants';
import { MapsTreeViewRow } from 'components/MapsTreeView/types';
import { UserMap } from 'services/map';

export const inRootFolder = (
  row: MapsTreeViewRow,
  folder: 'system' | 'shared' | 'user'
): boolean => row.rootFolder?.toLocaleLowerCase() === folder;

export const isInSystem = (userMap?: UserMap): boolean =>
  userMap?.folderPath === SYSTEM_RENDERER_FOLDER.PATH;
