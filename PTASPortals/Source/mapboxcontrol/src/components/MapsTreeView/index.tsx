// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  useContext,
  useState,
  Fragment,
  useEffect,
  useRef,
  useCallback,
} from 'react';
import { DataTypeProvider, EditingCell } from '@devexpress/dx-react-grid';
import { Box, makeStyles, createStyles } from '@material-ui/core';
import LockIcon from '@material-ui/icons/Lock';
import LockOpenIcon from '@material-ui/icons/LockOpen';
import {
  MenuOption,
  TreeView,
  OptionsMenuRefProps,
  ErrorMessageAlertCtx,
} from '@ptas/react-ui-library';
import { MapsTreeViewColumn, MapsTreeViewRow } from './types';
import { HomeContext } from 'contexts';
import { mapService } from 'services/map';
import clsx from 'clsx';
import userService from 'services/user/userService';
import MapsLayersManage from '../MapLayersManage';
import { Panel } from './Panel';
import { getFolderPath } from 'utils/componentUtil';
import { inRootFolder } from 'utils/userMap';
import { useMount, useUpdateEffect } from 'react-use';
import { useRename } from './useRename';
import { useCreateFolder } from 'hooks/api';
import { useNewFolderRow } from './useNewFolderRow';
import MenuItems from './MenuItems';
import { getErrorStr } from 'utils/getErrorStr';

const useStyles = makeStyles(() =>
  createStyles({
    lockComponentIcon: {
      '&:hover': {
        cursor: 'pointer',
      },
    },
    treeContainer: {
      padding: 16,
    },
    isShared: {
      cursor: 'not-allowed !important',
    },
  })
);

const columns: MapsTreeViewColumn[] = [
  { name: 'title', title: 'Title' },
  { name: 'menu', title: '...' },
  { name: 'isLocked', title: 'Lock' },
  { name: 'lastModifiedBy', title: 'Last Modified By' },
];

const columnSizes = [
  { columnName: 'title', width: 400 },
  { columnName: 'menu', width: 40 },
  { columnName: 'isLocked', width: 70 },
  { columnName: 'lastModifiedBy', width: 400 },
];

/**
 * A component that visualizes the maps and execute some actions over them
 */
function MapsTreeView(): JSX.Element {
  const classes = useStyles();
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const homeContext = useContext(HomeContext);
  const {
    setLinearProgress,
    mapsTreeView,
    setPanelContent,
    saveDefaultMap,
  } = homeContext;
  const createFolder = useCreateFolder();
  const rename = useRename();
  const {
    expandedGroups,
    selectedRow,
    isLoading,
    rows,
    setRows,
  } = mapsTreeView;
  const [enableEditing, setEnableEditing] = useState<boolean>(false);
  const [cellToEdit, setCellToEdit] = useState<EditingCell[]>([]);
  const [selected, setSelected] = useState<MapsTreeViewRow>();

  const optionsMenuRef = useRef<OptionsMenuRefProps>();
  const { setNewFolderRow, newFolderRow } = useNewFolderRow({
    selected,
    setEnableEditing,
    setCellToEdit,
  });

  useMount(
    async (): Promise<void> => {
      try {
        await mapService.refreshUserMaps();
        await mapService.refreshUserMapFolders();
      } catch (e) {
        showErrorMessage(getErrorStr(e));
      }
    }
  );

  useUpdateEffect(() => {
    homeContext.setLinearProgress(isLoading);
  }, [isLoading]);

  /**
   * A formatter applied to column isLocked
   * @param param0 - DataTypeProvider.ValueFormatterProps
   */
  const IsLockedColumnFormatter = (
    valueFormatterProps: DataTypeProvider.ValueFormatterProps
  ): JSX.Element | null => {
    const row: MapsTreeViewRow = valueFormatterProps.row;

    if (!inRootFolder(row, 'shared')) return null;

    const onClick = (): void => {
      if (
        row.createdBy !== userService.userInfo.id &&
        !userService.isAdminUser()
      )
        return;
      setRows(
        rows.map((_row) => {
          return {
            ..._row,
            isLocked: _row.id === row.id ? !_row.isLocked : _row.isLocked,
          };
        })
      );
      mapService.toggleLockUserMap(row.id as number);
    };
    return (
      <Box onClick={onClick}>
        {row.isLocked && (
          <LockIcon
            classes={{
              root:
                row.createdBy !== userService.userInfo.id &&
                !userService.isAdminUser()
                  ? clsx(classes.lockComponentIcon, classes.isShared)
                  : classes.lockComponentIcon,
            }}
          />
        )}
        {row.isLocked === false && (
          <LockOpenIcon
            classes={{
              root:
                row.createdBy !== userService.userInfo.id &&
                !userService.isAdminUser()
                  ? clsx(classes.lockComponentIcon, classes.isShared)
                  : classes.lockComponentIcon,
            }}
          />
        )}
      </Box>
    );
  };

  const openEditUserMap = useCallback(
    (row: MapsTreeViewRow): void =>
      setPanelContent(
        <MapsLayersManage
          row={row}
          duplicate={!userService.isAdminUser() && inRootFolder(row, 'system')}
        />
      ),
    [setPanelContent]
  );

  const duplicateUserMap = useCallback(
    async (row: MapsTreeViewRow): Promise<void> => {
      try {
        if (!row.folderPath) return;
        setLinearProgress(true);
        await mapService.duplicateUserMap(
          row?.id as number,
          'Copy of ' + row.title,
          row.folderPath
        );
        await mapService.refreshUserMaps();
      } finally {
        setLinearProgress(false);
      }
    },
    [setLinearProgress]
  );

  const lockUserMap = useCallback(
    async (row: MapsTreeViewRow): Promise<void> => {
      try {
        setLinearProgress(true);
        await mapService.toggleLockUserMap(row?.id as number);
        await mapService.refreshUserMaps();
      } catch (e) {
        showErrorMessage(
          (e as Error)?.stack ?? 'Error trying to lock user map ' + row.id
        );
      } finally {
        setLinearProgress(false);
      }
    },
    [setLinearProgress, showErrorMessage]
  );

  const onMenuItemClick = useCallback(
    (_action: MenuOption, row?: MapsTreeViewRow): void => {
      if (!row) return;
      switch (_action.id) {
        case 'Edit':
          openEditUserMap(row);
          break;
        case 'Rename': {
          setEnableEditing(true);
          setRows((prev) => {
            const rowIndex = prev.findIndex((r) => r.id === row.id);
            setCellToEdit([{ rowId: rowIndex, columnName: 'title' }]);
            return prev.map((r, i) =>
              i === rowIndex ? { ...r, isEditable: true } : r
            );
          });
          break;
        }
        case 'Lock/unlock':
          lockUserMap(row);
          break;
        case 'set-as-default': {
          saveDefaultMap(+row.id);
          break;
        }
      }
    },
    [lockUserMap, openEditUserMap, saveDefaultMap, setRows]
  );

  const dataTypeProviders = [
    <DataTypeProvider
      key="isLocked"
      for={['isLocked']}
      formatterComponent={IsLockedColumnFormatter}
    />,
  ];

  useEffect(() => {
    const handleEsc = (event: KeyboardEvent): void => {
      if (event.key === 'Escape' && enableEditing) {
        const tempRow = rows.find((r) => r.title === 'Folder Name');
        if (tempRow) {
          setRows(rows.filter((r) => r.id !== tempRow.id));
          setEnableEditing(false);
          setNewFolderRow(undefined);
          return;
        }
      }
    };
    window.addEventListener('keydown', handleEsc);

    return (): void => {
      window.removeEventListener('keydown', handleEsc);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [enableEditing]);

  const onEditingComplete = (
    newRows: MapsTreeViewRow[],
    _changed: MapsTreeViewRow[],
    changedRow: MapsTreeViewRow,
    originalRow: MapsTreeViewRow
  ): void => {
    if (!changedRow) return setEnableEditing(false);
    setEnableEditing(false);
    if (newRows) setRows(newRows);

    if (newFolderRow) {
      let folderName = changedRow.title.trim();
      if (!folderName) {
        const tempRow = rows.find((r) => r.title === 'Folder Name');
        if (tempRow) {
          setRows(rows.filter((r) => r.id !== tempRow.id));
        }
        showErrorMessage({
          message: 'Folder name should not be empty',
        });
        setNewFolderRow(undefined);
        return;
      }
      if (folderName.length > 64) {
        folderName = folderName.substr(0, 64);
      }

      const result = rows
        .filter((r) => r.parent === changedRow.parent)
        .find((c) => c.title === changedRow.title);
      if (result) {
        setRows(rows.filter((r) => r.id !== changedRow.id));
        showErrorMessage({
          message: `Folder ${changedRow.title} already exists`,
        });
        setNewFolderRow(undefined);
        return;
      }

      createFolder(getFolderPath(changedRow, rows));
      setNewFolderRow(undefined);
    } else {
      if (newRows) setRows(newRows);
      rename(changedRow, originalRow);
    }
  };

  const onSelect = (row?: MapsTreeViewRow): void => {
    setSelected(row);
    homeContext.setTreeSelection(row);
  };

  const renderMenu = useCallback(
    (row: MapsTreeViewRow): JSX.Element =>
      row.parent === null ? (
        <Fragment />
      ) : (
        <MenuItems
          row={row}
          optionsMenuRef={optionsMenuRef}
          onMenuItemClick={onMenuItemClick}
          rows={rows}
          duplicateUserMap={duplicateUserMap}
        />
      ),
    [duplicateUserMap, onMenuItemClick, rows]
  );

  return (
    <Panel>
      <Box className={classes.treeContainer}>
        <TreeView<MapsTreeViewRow>
          isLoading={isLoading}
          onNameClick={openEditUserMap}
          enableColumnResize
          onSelect={onSelect}
          onEditingComplete={onEditingComplete}
          editingCells={cellToEdit}
          disableGrouping
          hideEye
          enableEditing={enableEditing}
          sortBy={[{ columnName: 'title', direction: 'desc' }]}
          expandedRowIds={expandedGroups}
          setSelectedItem={selectedRow}
          columns={columns}
          virtual
          virtualTableProps={{
            height: homeContext.panelHeight - 123,
          }}
          resizeDefaultColumnWidths={columnSizes}
          rows={rows}
          groupBy="subject"
          displayGroupInColumn="title"
          dataTypeProviders={dataTypeProviders}
          renderMenu={renderMenu}
        />
      </Box>
    </Panel>
  );
}

export default MapsTreeView;
