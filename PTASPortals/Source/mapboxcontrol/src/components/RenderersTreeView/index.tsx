/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useState, Fragment, useEffect } from 'react';
import { HomeContext } from 'contexts';
import { EditingCell } from '@devexpress/dx-react-grid';
import {
  Box,
  makeStyles,
  createStyles,
  PopoverPosition,
} from '@material-ui/core';
import {
  TreeViewRow,
  OptionsMenu,
  MenuOption,
  TreeView,
  Alert,
  Popover,
  ErrorMessageAlertCtx,
} from '@ptas/react-ui-library';
import { mapService } from 'services/map';
import { apiService } from 'services/api';
import { useRenderersTreeView } from './useRenderersTreeView';
import MapLayersManage from '../MapLayersManage';
import { Panel } from './Panel';
import { systemUserMapService } from 'services/map/systemUserMapService';
import { getFolderPath } from 'utils/componentUtil';
import {
  MapsTreeViewColumn,
  MapsTreeViewRow,
} from 'components/MapsTreeView/types';
import SetCategories from './SetCategories';
import { useUpdateEffect } from 'react-use';

const useStyles = makeStyles(() =>
  createStyles({
    treeContainer: {
      padding: 16,
    },
  })
);

const columns: MapsTreeViewColumn[] = [
  { name: 'title', title: 'Title' },
  { name: 'menu', title: '...' },
  { name: 'lastModifiedBy', title: 'Last Modified By' },
];

const columnSizes = [
  { columnName: 'title', width: 400 },
  { columnName: 'menu', width: 40 },
  { columnName: 'lastModifiedBy', width: 400 },
];

/**
 * A component that visualizes the renderers and execute some actions over them
 */
function RenderersTreeView(): JSX.Element {
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const classes = useStyles();
  const homeContext = useContext(HomeContext);
  const { selectedSystemUserMap, setLinearProgress } = homeContext;
  const {
    expandedGroups,
    selectedRow,
    isLoading,
    setIsLoading,
    rows,
    setRows,
  } = useRenderersTreeView(homeContext);
  const [enableEditing, setEnableEditing] = useState<boolean>(false);
  const [cellToEdit, setCellToEdit] = useState<EditingCell[]>([]);
  const [closePop, setClosePop] = useState<boolean>(false);
  const [clickedRow, setClickedRow] = useState<MapsTreeViewRow>();
  const [catPopPos, setCatPopPos] = useState<PopoverPosition>();
  const openEditUserMap = (row: MapsTreeViewRow & TreeViewRow): void => {
    homeContext.setPanelContent(<MapLayersManage row={row} isSystemUserMap />);
  };

  useUpdateEffect(() => {
    if (!isLoading) {
      homeContext.setLinearProgress(false);
    }
  }, [isLoading]);

  const confirmDeleteRenderer = async (
    row: MapsTreeViewRow | undefined
  ): Promise<void> => {
    if (row?.id) {
      try {
        homeContext.setLinearProgress(true);
        setIsLoading(true);
        await systemUserMapService.deleteSystemUserMap(row.id as number);
        await systemUserMapService.loadCategories();
      } catch (e) {
        homeContext.setLinearProgress(false);
        setIsLoading(false);
        showErrorMessage(JSON.stringify(e));
      }
    }
  };

  const duplicateUserMap = async (
    row: MapsTreeViewRow & TreeViewRow
  ): Promise<void> => {
    try {
      homeContext.setLinearProgress(true);
      setIsLoading(true);
      const rendId = await mapService.duplicateUserMap(
        row?.id as number,
        'Copy of ' + row.title
      );
      if (!rendId) return;
      await systemUserMapService.setCategories(
        rendId,
        homeContext.rendererCategories.flatMap((cat) =>
          cat.userMaps?.find((userMap) => userMap.userMapId === row.id)
            ? {
                id: cat.userMapCategoryId,
                name: cat.categoryName,
              }
            : []
        )
      );
      await systemUserMapService.loadCategories();
    } catch (e) {
      showErrorMessage(JSON.stringify(e));
      homeContext.setLinearProgress(false);
      setIsLoading(false);
    }
  };

  const rename = async (
    editedRow: (MapsTreeViewRow & TreeViewRow) | undefined,
    originalRow: (MapsTreeViewRow & TreeViewRow) | undefined
  ): Promise<void> => {
    if (!editedRow) return;
    if (editedRow.folder) return;
    if (!editedRow.title.trim()) {
      showErrorMessage('Name should not be empty');
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
      showErrorMessage('Name limited to 64 characters');
    }

    if (
      rows
        .filter((r) => r.parent === editedRow.parent)
        .find((r) => r.title === newName)
    ) {
      showErrorMessage('Name already exists');
      setRows(
        rows.map((r) =>
          r.id === editedRow.id ? { ...r, name: originalRow?.title } : r
        )
      );
      return;
    }

    if (!originalRow) return;
    try {
      setLinearProgress(true);
      await mapService.renameUserMap(editedRow.id as number, newName);
      systemUserMapService.loadCategories();
      if (
        selectedSystemUserMap &&
        editedRow.id === selectedSystemUserMap.userMapId
      ) {
        systemUserMapService.applySystemUserMap(editedRow.id as number);
      }
    } catch (e) {
      showErrorMessage(JSON.stringify(e));
    } finally {
      setLinearProgress(false);
    }
  };

  const onMenuItemClick = (
    _action: MenuOption,
    row: (MapsTreeViewRow & TreeViewRow) | undefined
  ): void => {
    if (!row) return;
    switch (_action.id) {
      case 'Edit':
        openEditUserMap(row);
        break;
      case 'Rename': {
        const rowIndex = rows.findIndex((r) => r.id === row.id);
        setEnableEditing(true);
        setRows(
          rows.map((r, i) => (i === rowIndex ? { ...r, isEditable: true } : r))
        );
        setCellToEdit([{ rowId: rowIndex, columnName: 'title' }]);
        break;
      }
      case 'Duplicate':
        if (!row?.folder) {
          duplicateUserMap(row);
        }
        break;
    }
  };

  const deleteFolder = async (row: MapsTreeViewRow): Promise<void> => {
    try {
      await apiService.deleteFolder({
        folderPath: getFolderPath<MapsTreeViewRow>(row, rows),
        folderItemType: 'UserMap',
      });
      mapService.refreshUserMapFolders();
    } catch (e) {
      showErrorMessage(JSON.stringify(e));
    }
  };

  const DeleteMap = (selectedRow: MapsTreeViewRow): JSX.Element => (
    <Alert
      contentText={
        selectedRow.folder
          ? 'Deleting permanently erases this category and its contents.'
          : 'Deleting permanently erases this renderer.'
      }
      okButtonText={
        selectedRow.folder ? 'Delete this category' : 'Delete this renderer'
      }
      okButtonClick={(): void => {
        if (selectedRow.folder) {
          deleteFolder(selectedRow);
        } else {
          confirmDeleteRenderer(selectedRow);
        }
        setClosePop(!closePop);
      }}
    />
  );

  useEffect(() => {
    const handleEsc = (event: KeyboardEvent): void => {
      if (event.key === 'Escape' && enableEditing) {
        setEnableEditing(false);
      }
    };
    window.addEventListener('keydown', handleEsc);

    return (): void => {
      window.removeEventListener('keydown', handleEsc);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [enableEditing]);

  const closeCategoriesPopover = (): void => {
    setCatPopPos(undefined);
    setClickedRow(undefined);
  };

  return (
    <Fragment>
      <Panel>
        <Box className={classes.treeContainer}>
          <TreeView<MapsTreeViewRow>
            isLoading={isLoading}
            onNameClick={(row): void => openEditUserMap(row)}
            enableColumnResize
            onSelect={(r): void => {
              homeContext.setTreeSelection(r);
            }}
            onEditingComplete={(
              newRows,
              _changed,
              changedRow,
              originalRow
            ): void => {
              if (!changedRow) {
                setEnableEditing(false);
                return;
              }
              setEnableEditing(false);
              if (newRows) setRows(newRows);
              rename(changedRow, originalRow);
            }}
            editingCells={cellToEdit}
            disableGrouping
            hideEye
            enableEditing={enableEditing}
            sortBy={[{ columnName: 'title', direction: 'asc' }]}
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
            renderMenu={(row: MapsTreeViewRow): JSX.Element => (
              <OptionsMenu<MapsTreeViewRow>
                row={row}
                onItemClick={onMenuItemClick}
                showTail={false}
                items={[
                  { label: 'Edit', id: 'Edit', disabled: row.folder },
                  {
                    label: 'Categories',
                    id: 'Categories',
                    onClick: (event): void => {
                      const domRect = event.currentTarget.getBoundingClientRect();
                      setCatPopPos({
                        left: domRect.left,
                        top: domRect.top,
                      });
                      setClickedRow(row);
                    },
                  },
                  {
                    label: 'Duplicate',
                    id: 'Duplicate',
                    disabled: row.folder,
                  },
                  {
                    label: 'Rename',
                    id: 'Rename',
                  },
                  {
                    label: 'Delete',
                    id: 'Delete',
                    afterClickContent: DeleteMap(row),
                    isAlert: true,
                  },
                ]}
              ></OptionsMenu>
            )}
          />
        </Box>
      </Panel>
      {!!catPopPos && !!clickedRow && (
        <Popover
          onClose={(): void => {
            closeCategoriesPopover();
          }}
          open={true}
          anchorReference="anchorPosition"
          anchorPosition={catPopPos}
        >
          {typeof clickedRow?.id === 'number' && (
            <SetCategories
              userMapId={clickedRow.id}
              userMapName={clickedRow.title}
              onClose={closeCategoriesPopover}
            />
          )}
        </Popover>
      )}
    </Fragment>
  );
}

export default RenderersTreeView;
