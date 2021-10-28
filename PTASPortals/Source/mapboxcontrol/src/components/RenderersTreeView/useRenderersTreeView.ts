// useMapsTreeView.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState, useEffect } from 'react';
import { utilService } from 'services/common';
import { HomeContextProps } from 'contexts';
import { MapsTreeViewRow } from 'components/MapsTreeView/types';

type UseRenderersTreeView = {
  rows: MapsTreeViewRow[];
  setRows: React.Dispatch<React.SetStateAction<MapsTreeViewRow[]>>;
  setExpandedGroups: React.Dispatch<React.SetStateAction<React.ReactText[]>>;
  expandedGroups: React.ReactText[];
  selectedRow: number[];
  isLoading: boolean;
  setIsLoading: React.Dispatch<React.SetStateAction<boolean>>;
};

export const useRenderersTreeView = (
  homeContext: Pick<
    HomeContextProps,
    | 'selectedSystemUserMap'
    | 'mapsUserDetails'
    | 'rendererCategories'
    | 'loadUserMapsForAllCategories'
  >
): UseRenderersTreeView => {
  const {
    selectedSystemUserMap,
    mapsUserDetails,
    rendererCategories,
    loadUserMapsForAllCategories,
  } = homeContext;
  const [rows, setRows] = useState<MapsTreeViewRow[]>([]);
  const [expandedGroups, setExpandedGroups] = useState<React.ReactText[]>([]);
  const [selectedRow, setSelectedRow] = useState<number[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);

  useEffect(() => {
    if (rendererCategories.some((cat) => !cat.userMaps)) {
      loadUserMapsForAllCategories();
      return;
    }

    const _categoriesRows: MapsTreeViewRow[] = rendererCategories.flatMap(
      (cat) =>
        cat.userMaps && cat.userMaps.length
          ? {
              id: 'c' + cat.userMapCategoryId,
              title: cat.categoryName,
              subject: '',
              parent: null,
              folder: true,
              visible: true,
            }
          : []
    );

    const _rendererRows: MapsTreeViewRow[] = [];
    rendererCategories.forEach((cat) => {
      if (cat.userMaps && cat.userMaps.length) {
        cat.userMaps.forEach((map) => {
          _rendererRows.push({
            id: map.userMapId,
            title: map.userMapName,
            isLocked: map.isLocked,
            lastModifiedBy:
              (mapsUserDetails.find((u) => u.id === map.lastModifiedBy)
                ?.fullName ?? 'John Doe') +
              ' ' +
              utilService.getLocalDateString(map.lastModifiedTimestamp),
            subject: '',
            parent: 'c' + cat.userMapCategoryId,
            folder: false,
            visible: true,
            linkTo: '/',
            folderPath: map.folderPath,
            createdBy: map.createdBy,
          });
        });
      }
    });

    const _rows = _categoriesRows.concat(_rendererRows);
    let rowIdsToExpand: number[] = [];
    rowIdsToExpand = _rows.flatMap((r, i) => (r.parent === null ? i : []));
    setSelectedRow([
      _rows.findIndex((r) => r.id === selectedSystemUserMap?.userMapId),
    ]);

    let parentId = _rows.find((r) => r.id === selectedSystemUserMap?.userMapId)
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
  }, [rendererCategories, selectedSystemUserMap]);

  return {
    rows,
    setRows,
    expandedGroups,
    selectedRow,
    isLoading,
    setIsLoading,
    setExpandedGroups,
  };
};
