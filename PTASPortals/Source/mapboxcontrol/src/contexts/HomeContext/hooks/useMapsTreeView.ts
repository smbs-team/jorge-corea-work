// useMapsTreeView.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState, useEffect, Dispatch, ReactText } from 'react';
import { uniqueId } from 'lodash';
import { unimplementedStateFn } from '@ptas/react-ui-library';
import { utilService } from 'services/common';
import { HomeContextProps } from 'contexts';
import { UserMap } from 'services/map';
import { MapsTreeViewRow } from 'components/MapsTreeView/types';
import { createFolderRows } from 'utils/userMapFolder';
import userService from 'services/user/userService';

export type UseMapsTreeView = {
  rows: MapsTreeViewRow[];
  setRows: Dispatch<React.SetStateAction<MapsTreeViewRow[]>>;
  setExpandedGroups: Dispatch<React.SetStateAction<React.ReactText[]>>;
  expandedGroups: ReactText[];
  selectedRow: number[];
  isLoading: boolean;
};

type Props = Pick<
  HomeContextProps,
  'userMaps' | 'userMapFolders' | 'selectedUserMap' | 'mapsUserDetails'
>;

export const useMapsTreeViewDefState: UseMapsTreeView = {
  expandedGroups: [],
  isLoading: false,
  rows: [],
  selectedRow: [],
  setExpandedGroups: unimplementedStateFn,
  setRows: unimplementedStateFn,
};

export const useMapsTreeView = ({
  userMaps,
  userMapFolders,
  selectedUserMap,
  mapsUserDetails,
}: Props): UseMapsTreeView => {
  const [rows, setRows] = useState<MapsTreeViewRow[]>([]);
  const [expandedGroups, setExpandedGroups] = useState<React.ReactText[]>([]);
  const [selectedRow, setSelectedRow] = useState<number[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    const _folderRows: MapsTreeViewRow[] = createFolderRows(userMapFolders);
    const addFolder = (name: string): number =>
      _folderRows.push({
        id: uniqueId('tempF_'),
        title: name,
        subject: '',
        parent: null,
        folder: true,
        visible: true,
        createdBy: userService.userInfo.id,
      });

    if (!_folderRows.find((f) => f.title === 'User' && f.parent === null))
      addFolder('User');

    if (!_folderRows.find((f) => f.title === 'Shared' && f.parent === null))
      addFolder('Shared');

    const userMapsRows: MapsTreeViewRow[] = userMaps.map((map: UserMap) => {
      const mapFolder = userMapFolders.find(
        (f) => f.folderId === map.parentFolderId
      );
      const subject = mapFolder?.subject;

      return {
        rootFolder: mapFolder?.rootFolder ?? mapFolder?.folderName,
        id: map.userMapId,
        title: map.userMapName,
        isLocked: map.isLocked,
        lastModifiedBy:
          (mapsUserDetails.find((u) => u.id === map.lastModifiedBy)?.fullName ??
            'John Doe') +
          ' ' +
          utilService.getLocalDateString(map.lastModifiedTimestamp),
        type: map.folderItemType,
        subject: subject ?? '',
        parent: 'f' + map.parentFolderId,
        folder: false,
        visible: true,
        linkTo: map.isLocked ? undefined : '/',
        folderPath: map.folderPath,
        createdBy: map.createdBy,
      };
    });
    const _rows = _folderRows.concat(userMapsRows);

    let rowIdsToExpand: number[] = [];
    rowIdsToExpand = _rows.flatMap((r, i) => (r.parent === null ? i : []));
    setSelectedRow([
      _rows.findIndex((r) => r.id === selectedUserMap?.userMapId),
    ]);

    let parentId = _rows.find((r) => r.id === selectedUserMap?.userMapId)
      ?.parent;

    while (parentId) {
      // eslint-disable-next-line no-loop-func
      const index = _rows.findIndex((r) => r.id === parentId);
      rowIdsToExpand.push(index);
      parentId = _rows[index]?.parent;
    }

    setExpandedGroups([...new Set(rowIdsToExpand)]);

    setRows(_rows);
    setIsLoading(false);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [userMaps, userMapFolders, selectedUserMap]);

  return {
    rows,
    setRows,
    expandedGroups,
    selectedRow,
    isLoading,
    setExpandedGroups,
  };
};
