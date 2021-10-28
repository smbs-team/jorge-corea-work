// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useEffect, Fragment, useCallback } from "react";
import {
  TreeDataState,
  CustomTreeData,
  Column,
  DataTypeProvider,
  SelectionState
} from "@devexpress/dx-react-grid";
import {
  Grid,
  VirtualTable,
  TableTreeColumn,
  TableSelection,
  Table
} from "@devexpress/dx-react-grid-material-ui";
import {
  ButtonBase,
  CircularProgress,
  createStyles,
  Theme,
  withStyles,
  WithStyles
} from "@material-ui/core";
import DetailsIcon from "@material-ui/icons/Details";
import FolderIcon from "@material-ui/icons/Folder";
import InsertDriveFileIcon from "@material-ui/icons/InsertDriveFile";
import ErrorIcon from "@material-ui/icons/Error";
import WarningIcon from "@material-ui/icons/Warning";

import { GenericWithStyles } from "../common";
import { useUpdateEffect } from "react-use";

interface Props<T> extends WithStyles<typeof useStyles> {
  rows: GenericRow<T>[];
  isLoading?: boolean;
  onDataLoad?: (rowIds: React.ReactText[]) => void;
  onSelect?: (row: GenericRow<T>) => void;
  expandedRowIds?: React.ReactText[];
  onExpandedRowIdsChange?: (expandedRowIds: React.ReactText[]) => void;
}

export type GenericRow<T> = T & LazyRow;
export type LazyTreeProps<T> = GenericWithStyles<Props<T>>;

interface LazyRow {
  id: React.ReactText;
  name: string;
  parentId: React.ReactText;
  isDirectory: boolean;
  hasItems: boolean;
  isEmpty?: boolean;
  hasErrors?: boolean;
}

const ROOT_ID = "";

const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      marginBottom: "20px",
      width: "unset !important",
      "& tbody tr": {
        height: 24
      },
      "& tbody tr td": {
        fontFamily: theme.ptas.typography.bodyFontFamily,
        fontSize: theme.ptas.typography.bodySmall.fontSize,
        fontWeight: "normal",
        borderBottom: "none",
        padding: 0,
        "&:last-child": {
          paddingLeft: 0
        }
      }
    },
    nameContainer: {
      display: "flex",
      alignItems: "center",
      cursor: "pointer",
      "&:hover": {
        fontWeight: "bold"
      }
    },
    rowSelection: {
      backgroundColor: "#d4e693"
    },
    groupCell: {
      fontSize: "1rem",
      fontWeight: "normal",
      cursor: "pointer",
      "&:hover": {
        fontWeight: "bold"
      }
    },
    folderIcon: {
      marginRight: "4px",
      color: "#008db1"
    },
    InsertDriveFileIcon: {
      marginRight: "4px",
      color: "#008db1",
      opacity: "60%"
    },
    loadingIcon: {
      position: "absolute",
      fontSize: "20px",
      top: "calc(45% - 10px)",
      left: "calc(50% - 10px)"
    },
    loadingContainer: {
      position: "absolute",
      top: 0,
      left: 0,
      width: "100%",
      height: "100%",
      background: "rgba(255, 255, 255, .3)"
    },
    gridRoot: {
      height: "100%"
    },
    noDataMessage: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: "bold"
    },
    td: {
      display: "flex",
      justifyContent: "center"
    }
  });

const getRowId = <T extends {}>(row: GenericRow<T>) => row.id;

const getChildRows = <T extends {}>(
  row: GenericRow<T>,
  rootRows: GenericRow<T>[]
) => {
  const childRows = rootRows.filter(
    (r) => r.parentId === (row ? row.id : ROOT_ID)
  );
  if (childRows.length) {
    return childRows;
  }

  return row && row.hasItems ? [] : null;
};

/**
 * LazyGrid
 *
 * @param props - Component props
 * @returns A JSX element
 */
function LazyGrid<T>(props: Props<T>): JSX.Element {
  const { classes, isLoading, onDataLoad, rows } = props;

  const [columns] = useState<Column[]>([{ name: "name", title: "Name" }]);
  const [expandedRowIds, setExpandedRowIds] = useState<React.ReactText[]>(
    props.expandedRowIds ?? []
  );
  const [selectedRows, setSelectedRows] = useState<React.ReactText[]>([]);

  const expandRow = (rowId: React.ReactText): void => {
    const index = expandedRowIds.findIndex((id) => rowId === id);
    const nextExpandedRowIds = [...expandedRowIds];

    if (index > -1) {
      nextExpandedRowIds.splice(index, 1);
    } else {
      nextExpandedRowIds.push(rowId);
    }

    setExpandedRowIds(nextExpandedRowIds);
  };

  useUpdateEffect(() => {
    if (!props.expandedRowIds) return;
    setExpandedRowIds(props.expandedRowIds);
  }, [props.expandedRowIds]);

  useEffect(() => {
    if (!isLoading) {
      const rowIdsWithNotLoadedChilds = [ROOT_ID, ...expandedRowIds].filter(
        (rowId) => rows.findIndex((row) => row.parentId === rowId) === -1
      );
      if (rowIdsWithNotLoadedChilds.length) {
        if (isLoading) return;
        onDataLoad && onDataLoad(rowIdsWithNotLoadedChilds);
      }
    }
  }, [expandedRowIds, isLoading, onDataLoad, rows]);

  const handleOnSelect = (selectedRowsIds: React.ReactText[]): void => {
    const selectedRow = selectedRowsIds.pop();
    if (!selectedRow) return;
    setSelectedRows(selectedRow ? [selectedRow] : []);
  };

  const TableComponent: React.ComponentType<object> = ({
    ...restProps
  }): JSX.Element => (
    <VirtualTable.Table {...restProps} className={classes?.root} />
  );

  const NameColumnFormatter = (
    nameProps: DataTypeProvider.ValueFormatterProps
  ): JSX.Element => {
    const row = nameProps.row as GenericRow<T>;
    return (
      <div
        key={row.id}
        className={classes.nameContainer}
        onClick={() => {
          if (row.isDirectory) {
            expandRow(row.id);
            return;
          }
          if (row.id === selectedRows[0] || row.hasErrors || row.isEmpty)
            return;
          props.onSelect && props.onSelect(row);
        }}
        style={{
          backgroundColor:
            selectedRows[0] === row.id && !row.isDirectory ? "#d4e693" : "",
          marginLeft: !row.isDirectory ? "20px" : "",
          userSelect: "none"
        }}
      >
        {row.isDirectory ? (
          <FolderIcon className={classes.folderIcon} />
        ) : (
          getIcon(row)
        )}
        <span key={row.id + "-name-el"}>{row.name}</span>
      </div>
    );
  };

  const getIcon = (row: GenericRow<T>): JSX.Element => {
    if (row.hasErrors)
      return (
        <ErrorIcon
          className={classes.InsertDriveFileIcon}
          style={{ color: "red" }}
        />
      );
    if (row.isEmpty)
      return (
        <WarningIcon
          className={classes.InsertDriveFileIcon}
          style={{ color: "yellow" }}
        />
      );
    return <InsertDriveFileIcon className={classes.InsertDriveFileIcon} />;
  };

  const ExpandButtonComponent = (
    expandProps: TableTreeColumn.ExpandButtonProps
  ) => (
    <Fragment>
      {expandProps.visible && (
        <ButtonBase onClick={() => expandProps.onToggle()} disableRipple>
          <DetailsIcon
            style={{
              fontSize: 20,
              transform: expandProps.expanded ? "" : "rotate(270deg)"
            }}
          />
        </ButtonBase>
      )}
    </Fragment>
  );

  const Loading = () => (
    <div className={classes.loadingContainer}>
      <CircularProgress className={classes.loadingIcon} />
    </div>
  );

  const RootComponent = useCallback(
    (rootProps: Grid.RootProps): JSX.Element => {
      return (
        <Grid.Root {...rootProps} className={classes.gridRoot}>
          {rootProps.children}
        </Grid.Root>
      );
    },
    [classes.gridRoot]
  );

  const NoDataComponent = (noDataProps: Table.NoDataCellProps) => {
    return !props.isLoading && !props.rows.length ? (
      <td
        colSpan={noDataProps.colSpan}
        rowSpan={noDataProps.rowSpan}
        className={classes.td}
      >
        <label className={classes.noDataMessage}>No data</label>
      </td>
    ) : (
      <td colSpan={noDataProps.colSpan} rowSpan={noDataProps.rowSpan} />
    );
  };

  return (
    <div style={{ position: "relative", height: "100%" }}>
      <Grid
        rows={props.rows}
        columns={columns}
        getRowId={getRowId}
        rootComponent={RootComponent}
      >
        <SelectionState
          selection={selectedRows}
          onSelectionChange={handleOnSelect}
        />
        <DataTypeProvider
          key='name'
          for={["name"]}
          formatterComponent={NameColumnFormatter}
        />
        <TreeDataState
          expandedRowIds={expandedRowIds}
          onExpandedRowIdsChange={(ids) => {
            setExpandedRowIds(ids);
            props.onExpandedRowIdsChange && props.onExpandedRowIdsChange(ids);
          }}
        />
        <CustomTreeData getChildRows={getChildRows} />
        <VirtualTable
          tableComponent={TableComponent}
          height='auto'
          noDataCellComponent={NoDataComponent}
        />
        <TableSelection selectByRowClick showSelectionColumn={false} />
        <TableTreeColumn
          for='name'
          expandButtonComponent={ExpandButtonComponent}
        />
      </Grid>
      {props.isLoading && <Loading />}
    </div>
  );
}

export default withStyles(useStyles, { withTheme: true })(LazyGrid) as <T>(
  props: LazyTreeProps<T>
) => React.ReactElement;
