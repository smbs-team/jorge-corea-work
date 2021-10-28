/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import {
  useState,
  Dispatch,
  SetStateAction,
  useContext,
  useEffect,
  useMemo,
} from 'react';
import { EditingCell } from '@devexpress/dx-react-grid';
import { HomeContext } from 'contexts';
import userService from 'services/user/userService';
import { getAllIndexes } from 'utils/componentUtil';
import { MapsTreeViewRow } from './types';

type Props = {
  selected: MapsTreeViewRow | undefined;
  setEnableEditing: Dispatch<React.SetStateAction<boolean>>;
  setCellToEdit: React.Dispatch<React.SetStateAction<EditingCell[]>>;
};

type UseNewFolderRow = {
  setNewFolderRow: Dispatch<SetStateAction<MapsTreeViewRow | undefined>>;
  newFolderRow: MapsTreeViewRow | undefined;
};

export const useNewFolderRow = ({
  selected,
  setEnableEditing,
  setCellToEdit,
}: Props): UseNewFolderRow => {
  const [newFolderRow, setNewFolderRow] = useState<MapsTreeViewRow>();
  const { createNewFolder, mapsTreeView, setCreateNewFolder } = useContext(
    HomeContext
  );
  const { rows, setRows, setExpandedGroups } = mapsTreeView;

  const parentFolder = useMemo(() => {
    if (!selected) {
      return rows.find((row) => row.parent === null)?.id ?? null;
    } else if (selected.folder) {
      return selected.id;
    } else {
      return selected.parent;
    }
  }, [rows, selected]);

  useEffect(() => {
    if (!newFolderRow) return;
    setRows((prev) => [...prev, newFolderRow]);
  }, [newFolderRow, setRows]);

  useEffect(() => {
    if (!createNewFolder) return;
    const newRow: MapsTreeViewRow = {
      id: performance.now(),
      parent: parentFolder,
      title: 'Folder Name',
      lastModifiedBy: userService.userInfo.id,
      subject: '',
      isLocked: false,
      isEditable: true,
      folder: true,
      createdBy: userService.userInfo.id,
    };
    setNewFolderRow(newRow);
  }, [
    createNewFolder,
    parentFolder,
    selected,
    setEnableEditing,
    setExpandedGroups,
    setRows,
  ]);

  useEffect(() => {
    if (!newFolderRow) return;
    setEnableEditing(true);
    selected &&
      setExpandedGroups((e) => [...e, ...getAllIndexes(selected, rows)]);
  }, [newFolderRow, rows, selected, setEnableEditing, setExpandedGroups]);

  useEffect(() => {
    if (!newFolderRow) return;
    setCellToEdit([
      {
        rowId: rows.findIndex((r) => r.id === newFolderRow.id),
        columnName: 'title',
      },
    ]);
    setCreateNewFolder(false);
  }, [rows, newFolderRow, setCellToEdit, setCreateNewFolder]);

  return { setNewFolderRow, newFolderRow };
};
