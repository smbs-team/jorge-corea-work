/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, {
  useState,
  ReactText,
  useEffect,
  PropsWithChildren,
  useCallback,
  Fragment,
  useRef,
  ReactElement,
  ComponentType
} from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Box,
  Theme,
  CircularProgress
} from "@material-ui/core";
import {
  DataTypeProvider,
  TreeDataState,
  SelectionState,
  CustomTreeData,
  GroupingState,
  IntegratedGrouping,
  Column,
  GroupRow,
  SortingState,
  IntegratedSorting,
  Sorting,
  EditingState,
  EditingCell,
  TableColumnWidthInfo,
  ChangeSet
} from "@devexpress/dx-react-grid";
import {
  Grid,
  Table,
  TableHeaderRow,
  TableTreeColumn,
  TableGroupRow,
  TableSelection,
  VirtualTable,
  VirtualTableProps,
  TableProps,
  TableInlineCellEditing,
  TableColumnResizing
} from "@devexpress/dx-react-grid-material-ui";

import IconButton from "@material-ui/core/IconButton";
import FolderIcon from "@material-ui/icons/Folder";
import VisibilityIcon from "@material-ui/icons/Visibility";
import VisibilityOffIcon from "@material-ui/icons/VisibilityOff";
import { GenericWithStyles } from "../common/types";
import DetailsIcon from "@material-ui/icons/Details";
import { omit } from "lodash";
import clsx from "clsx";

type TreeViewRowId = string | number;

export interface TreeViewRow {
  id: TreeViewRowId;
  parent: TreeViewRowId | null;
  visible?: boolean;
  folder?: boolean;
  isEmptyGroup?: boolean;
  linkTo?: string;
}

type Props<T> = WithStyles<typeof styles> & {
  columns: Column[];
  tableColumnExtensions?: Table.ColumnExtension[];
  groupBy?: keyof T | (keyof T)[];
  sortBy?: Sorting[];
  displayGroupInColumn: keyof T;
  dataTypeProviders?: JSX.Element[];
  rows: T[];
  renderMenu?: (row: T) => JSX.Element;
  virtual?: boolean;
  virtualTableProps?: VirtualTableProps;
  tableProps?: TableProps;
  onSelect?: (row?: T) => void;
  onGroupClick?: (row: GroupRow) => void;
  hideHeader?: boolean;
  hideEye?: boolean;
  hideSelectionHighlight?: boolean;
  groupRowIndentColumnWidth?: number;
  treeIndentColumnWidth?: number;
  expandedGroups?: string[] | undefined;
  rowIcon?: React.ReactNode;
  folderIcon?: React.ReactNode;
  groupFolderIcon?: React.ReactNode;
  editingColumnExtensions?: EditingState.ColumnExtension[];
  editingCells?: EditingCell[];
  enableEditing?: boolean;
  onEditingComplete?: (
    rows: any,
    modifiedRow: any,
    row: any,
    originalRow: any
  ) => void;
  children?: React.ReactNode;
  disableGrouping?: boolean;
  expandedRowIds?: TreeViewRowId[];
  setSelectedItem?: TreeViewRowId[];
  resizeDefaultColumnWidths?: TableColumnWidthInfo[];
  enableColumnResize?: boolean;
  isLoading?: boolean;
  noDataMessage?: string;
  onNameClick?: (row: T) => void;
  disableFolderSelection?: boolean;
};

export type TreeViewProps<T> = GenericWithStyles<Props<T>>;

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    rowContainer: {
      display: "flex",
      alignItems: "center"
    },
    noGroups: {
      fontWeight: "bolder",
      fontSize: "1rem"
    },
    iconButtonDots: {
      marginLeft: "auto",
      padding: 0
    },
    iconButtonEye: {
      padding: 0,
      paddingRight: theme.spacing(0.625)
    },
    customRowIcon: {
      paddingRight: theme.spacing(0.625),
      flexShrink: 0
    },
    folderIcon: {
      color: "white",
      fontSize: 12,
      flexShrink: 0
    },
    root: {
      "& tbody tr": {
        height: 32
      },
      "& tbody tr td": {
        fontFamily: theme.ptas.typography.bodyFontFamily,
        fontSize: "0.875rem",
        fontWeight: "normal",
        borderBottom: "none",
        padding: 0
      }
    },
    groupCell: {
      fontWeight: "bolder",
      fontSize: "1rem"
    },
    treeIndentColumnWidth: {
      marginLeft: 10
    },
    rootComponent: {
      display: "flex",
      maxHeight: "inherit"
    },
    tableContainer: {
      "& table": {
        "& tbody tr td div": {
          overflow: "hidden"
        },
        "& thead tr th": {
          fontFamily: theme.ptas.typography.bodyFontFamily,
          fontSize: "0.875rem",
          fontWeight: "normal",
          padding: 0,
          borderBottom: "1px solid #929292"
        }
      }
    },
    rowSelection: {
      backgroundColor: theme.ptas.colors.utility.selectionLight
    },
    detailsIconWrap: {
      "&:hover": {
        cursor: "pointer"
      }
    },
    detailsIcon: {
      fontSize: 20,
      marginRight: 4
    },
    folderIconBackground: {
      width: 20,
      height: 20,
      backgroundColor: "#616161",
      borderRadius: "50%",
      display: "flex",
      justifyContent: "center",
      alignItems: "center",
      flexShrink: 0
    },
    rowContent: {
      overflow: "hidden",
      textOverflow: "ellipsis",
      lineHeight: "26px",
      margin: theme.spacing(0, 0.4, 0, 0.4),
      whiteSpace: "nowrap"
    },
    noData: {
      "& div big": {
        transform: "none"
      }
    },
    loadingIcon: {
      top: "10%",
      left: "50%",
      position: "sticky"
    },
    nameColumn: {
      "&:hover": {
        textDecoration: "underline",
        cursor: "pointer"
      }
    },
    noDataMessage: {
      display: "flex",
      justifyContent: "center",
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: "bold"
    }
  });

/**
 * get child rows
 *
 * @param row - The current row
 * @param rootRows - Root rows
 */
export const getChildRows = (
  row: TreeViewRow,
  rows: TreeViewRow[]
): TreeViewRow[] | null => {
  const childRows = rows.filter((r) => r.parent === (row ? row.id : null));
  return childRows.length ? childRows : null;
};

export const getGroups = <T,>(rows: T[], group: keyof T): string[] => {
  const groups: unknown[] = [];
  rows.forEach((r) => {
    if (groups.includes(r[group])) return;
    groups.push(r[group]);
  });
  return groups.map(String);
};

/**
 * A component that displays TreeView data
 *
 * @remarks
 * See:https://devexpress.github.io/devextreme-reactive/react/grid/demos/featured/tree-data/
 * @param props -The TreeView props
 */

const TreeView = <T extends TreeViewRow>(props: Props<T>): JSX.Element => {
  const { classes, columns, onSelect, onGroupClick, virtual, children } = props;
  const [rows, setRows] = useState<T[]>(props.rows);
  const [selection, setSelection] = useState<React.ReactText[]>([]);
  const [expandedGroups, setExpandedGroups] = useState<string[] | undefined>(
    props.expandedGroups
  );
  const [sorting, setSorting] = useState<Sorting[] | undefined>(props.sortBy);
  const [expandedRowIds, setExpandedRowIds] = useState<TreeViewRowId[]>([]);

  useEffect(() => {
    if (!props.expandedRowIds) return;
    setExpandedRowIds(props.expandedRowIds);
  }, [props.expandedRowIds]);

  useEffect(() => {
    if (!props.setSelectedItem) return;
    setSelection(props.setSelectedItem);
  }, [props.setSelectedItem]);

  useEffect(() => {
    if (selection && !selection.length) {
      return onSelect?.();
    }
    const row = rows[selection[0]];
    onSelect?.(row);
  }, [selection, rows]);

  useEffect(() => {
    setRows(props.rows);
  }, [props.rows]);

  useEffect(() => {
    setExpandedGroups(props.expandedGroups);
  }, [props.expandedGroups]);

  useEffect(() => {
    setSorting(props.sortBy);
  }, [props.sortBy]);

  const virtualTableRef = useRef(null);

  /**
   * Toggle row and its children display property
   * @param row - The root row
   */
  const handleRowVisible = (row: TreeViewRow): void => {
    setRows((prev) =>
      prev.map((item) =>
        item.id === row.id || item.parent === row.id
          ? { ...item, visible: !item.visible }
          : item
      )
    );
  };

  /**
   * @param index - Selected rows index
   */
  const handleOnSelect = useCallback(
    (index: ReactText[]): void => {
      const lastSelected = index.find(
        (selected) => selection && selection.indexOf(selected) === -1
      );
      if (lastSelected !== undefined) {
        if (props.disableFolderSelection) {
          if (rows[lastSelected].folder) {
            return;
          }
        }
        setSelection([lastSelected]);
      } else {
        setSelection([]);
      }
    },
    [props.disableFolderSelection, selection]
  );

  const TableComponent: ComponentType<object> = ({
    ...restProps
  }): JSX.Element => <Table.Table {...restProps} className={classes.root} />;

  const VirtualTableComponent: ComponentType<object> = ({
    ...restProps
  }): JSX.Element => (
    <VirtualTable.Table {...restProps} className={classes.root} />
  );

  const ContentComponent = ({
    row
  }: TableGroupRow.ContentProps): JSX.Element => (
    <span className={classes.groupCell} onClick={() => onGroupClick?.(row)}>
      {row.value}
    </span>
  );

  /**
   * IndentCell - Allows setting the indentation of the group cell
   * @param level - Object to identify the current level of the tree
   */
  const IndentCell = ({ level }: TableTreeColumn.IndentProps): JSX.Element[] =>
    Array.from({ length: level }).map((_, currentLevel) => (
      <span
        key={currentLevel}
        style={{ marginLeft: props.treeIndentColumnWidth ?? 48 }}
      />
    ));

  /**
   * Controls the expansion of the grouped columns
   * @param expandedGroups - The array of grouped columns
   */
  const changeExpandedGroups = (expandedGroups: string[]) => {
    setExpandedGroups(expandedGroups);
  };

  /**
   * A formatter applied to the column specified to group
   * @param param0 - DataTypeProvider.ValueFormatterProps
   */
  const grouperKeyFormatter = (
    valueFormatterProps: DataTypeProvider.ValueFormatterProps
  ): JSX.Element => {
    const row: typeof props.rows[number] = valueFormatterProps.row;
    return (
      <Box className={classes.rowContainer}>
        {children}
        {!props.hideEye && (
          <IconButton
            className={classes.iconButtonEye}
            onClick={(): void => {
              handleRowVisible(row);
            }}
          >
            {row.visible ? <VisibilityIcon /> : <VisibilityOffIcon />}
          </IconButton>
        )}

        {!!getChildRows(row, rows) || row.folder ? (
          <Fragment>
            {props.folderIcon ? (
              props.folderIcon
            ) : (
              <Fragment>
                {row.parent !== null && (
                  <i className={classes.folderIconBackground}>
                    <FolderIcon className={classes.folderIcon} />
                  </i>
                )}
              </Fragment>
            )}
          </Fragment>
        ) : (
          <Fragment>
            {props.rowIcon && (
              <i className={classes.customRowIcon}>{props.rowIcon}</i>
            )}
          </Fragment>
        )}
        <div
          onClick={(e) => {
            if (!row.linkTo) return;
            e.stopPropagation();
            props.onNameClick?.(row);
          }}
          className={clsx(
            classes.rowContent,
            row.parent === null && props.disableGrouping && classes.noGroups,
            row.linkTo && props.onNameClick && classes.nameColumn
          )}
        >
          {row[props.displayGroupInColumn]}
        </div>
      </Box>
    );
  };

  const menuColumn = (
    valueFormatterProps: DataTypeProvider.ValueFormatterProps
  ): JSX.Element => {
    const row: typeof props.rows[number] = valueFormatterProps.row;
    return <span>{props.renderMenu?.(row)}</span>;
  };

  const RootComponent = (props: Grid.RootProps): JSX.Element => {
    return (
      <Grid.Root {...props} className={classes.rootComponent}>
        {props.children}
      </Grid.Root>
    );
  };

  const TableContainer = (props: PropsWithChildren<object>): JSX.Element => {
    return (
      <Table.Container {...props} className={classes.tableContainer}>
        {props.children}
      </Table.Container>
    );
  };

  const VirtualTableContainer = (
    props: PropsWithChildren<object>
  ): JSX.Element => {
    return (
      <VirtualTable.Container {...props} className={classes.tableContainer}>
        {props.children}
      </VirtualTable.Container>
    );
  };

  const TableGroupRowIconComponent = (tableProps: TableGroupRow.IconProps) => (
    <span>
      {tableProps.expanded ? (
        <Fragment>
          <DetailsIcon
            classes={{ root: classes.detailsIcon }}
            style={{ verticalAlign: "bottom" }}
          />
          {props.groupFolderIcon}
        </Fragment>
      ) : (
        <Fragment>
          <DetailsIcon
            classes={{ root: classes.detailsIcon }}
            style={{
              transform: "rotate(150deg)",
              verticalAlign: "top"
            }}
          />
          {props.groupFolderIcon}
        </Fragment>
      )}
    </span>
  );

  const TreeExpandButtonComponent = (
    props: TableTreeColumn.ExpandButtonProps
  ) => {
    const newProps = omit(props, ["visible", "expanded"]);
    return (
      <Fragment>
        {props.visible && (
          <span
            className={classes.detailsIconWrap}
            onClick={(e) => {
              props.onToggle();
              e.stopPropagation();
            }}
            {...newProps}
          >
            {props.expanded ? (
              <DetailsIcon
                classes={{ root: classes.detailsIcon }}
                style={{
                  verticalAlign: "middle"
                }}
              />
            ) : (
              <DetailsIcon
                classes={{ root: classes.detailsIcon }}
                style={{
                  transform: "rotate(150deg)"
                }}
              />
            )}
          </span>
        )}
      </Fragment>
    );
  };

  const TableSelectionComponent = (
    props: TableSelection.RowProps
  ): JSX.Element => {
    const newProps = omit(props, ["selectByRowClick", "highlighted"]);
    return (
      <Table.StubRow
        {...newProps}
        onClick={() => props.onToggle()}
        className={props.highlighted ? classes.rowSelection : ""}
      />
    );
  };

  const toGrouping = () => {
    if (Array.isArray(props.groupBy)) {
      return props.groupBy.map((s) => {
        return { columnName: s };
      });
    } else {
      return [{ columnName: props.groupBy }];
    }
  };

  const [editingCells, setEditingCells] = useState<EditingCell[]>([]);

  const commitChanges = ({ changed }: ChangeSet) => {
    let changedRows;
    if (changed && changed[Object.keys(changed)[0]]) {
      changedRows = rows.map((row, i) => {
        if (i === parseInt(Object.keys(changed)[0])) {
          return {
            ...row,
            name: changed[Object.keys(changed)[0]].name,
            title: changed[Object.keys(changed)[0]].title,
            isEditable: false
          };
        } else {
          return row;
        }
      });
    }

    if (changedRows) {
      setRows(changedRows);
    } else {
      setRows(
        rows.map((r) =>
          r.id === props.editingCells?.[0].rowId
            ? { ...r, isEditable: false }
            : r
        )
      );
    }

    props.onEditingComplete?.(
      changedRows,
      changed,
      changedRows && changed ? changedRows[Object.keys(changed)[0]] : null,
      changed ? rows[Object.keys(changed)[0]] : null
    );
  };

  const FocusableCell = ({ onClick, ...restProps }: any): JSX.Element => (
    <Table.Cell {...restProps} tabIndex={0} onFocus={onClick} />
  );

  const onFocus = (e: any, row: any): void => {
    row.isEditable ? e.target.select() : setEditingCells([]);
  };

  const TableInlineCellEditingComponent = (
    props: TableInlineCellEditing.CellProps
  ): JSX.Element => (
    <TableInlineCellEditing.Cell
      {...props}
      onFocus={(e) => onFocus(e, props.row)}
    />
  );

  const [isLoading, setIsLoading] = useState<boolean | undefined>(
    props.isLoading
  );
  useEffect(() => {
    if (props.isLoading === undefined) return;
    setIsLoading(props.isLoading);
  }, [props.isLoading]);

  const NoDataComponent = (noDataProps: Table.NoDataCellProps) =>
    isLoading ? (
      <td colSpan={noDataProps.colSpan}>
        <CircularProgress className={classes.loadingIcon} />
      </td>
    ) : (
      <td colSpan={noDataProps.colSpan} rowSpan={noDataProps.rowSpan}>
        <label className={classes.noDataMessage}>
          {props.noDataMessage ?? "No data"}
        </label>
      </td>
    );

  useEffect(() => {
    if (!props.editingCells) return;
    setEditingCells(props.editingCells);
  }, [props.editingCells]);

  return (
    <Grid rows={rows} columns={columns} rootComponent={RootComponent}>
      {props.enableEditing && (
        <EditingState
          onCommitChanges={commitChanges}
          editingCells={editingCells}
          onEditingCellsChange={setEditingCells}
          columnExtensions={[{ columnName: "menu", editingEnabled: false }]}
        />
      )}
      <SortingState sorting={sorting} onSortingChange={setSorting} />
      <IntegratedSorting />
      <SelectionState
        selection={selection}
        onSelectionChange={handleOnSelect}
      />
      <DataTypeProvider
        for={[props.displayGroupInColumn as string]}
        formatterComponent={grouperKeyFormatter}
      />
      <DataTypeProvider for={["menu"]} formatterComponent={menuColumn} />
      {props.dataTypeProviders}
      <TreeDataState
        expandedRowIds={expandedRowIds}
        onExpandedRowIdsChange={setExpandedRowIds}
      />
      <CustomTreeData getChildRows={getChildRows} />
      {!props.disableGrouping && (
        <GroupingState
          grouping={toGrouping() as []}
          expandedGroups={expandedGroups}
          onExpandedGroupsChange={changeExpandedGroups}
        />
      )}
      {!props.disableGrouping && <IntegratedGrouping />}

      {virtual ? (
        <VirtualTable
          noDataCellComponent={NoDataComponent}
          ref={virtualTableRef}
          tableComponent={VirtualTableComponent}
          cellComponent={FocusableCell}
          containerComponent={VirtualTableContainer}
          columnExtensions={[
            ...((props.tableColumnExtensions as []) ?? {}),
            { columnName: "menu", width: "50px" }
          ]}
          {...(props.virtualTableProps || {})}
        />
      ) : (
        <Table
          tableComponent={TableComponent}
          cellComponent={FocusableCell}
          containerComponent={TableContainer}
          columnExtensions={props.tableColumnExtensions}
          {...(props.tableProps || {})}
        />
      )}
      {props.enableColumnResize && (
        <TableColumnResizing
          defaultColumnWidths={props.resizeDefaultColumnWidths}
        />
      )}

      {!props.hideHeader && (
        <TableHeaderRow showSortingControls={props.sortBy ? true : false} />
      )}
      {!props.hideSelectionHighlight ? (
        <TableSelection
          selectByRowClick
          highlightRow
          showSelectionColumn={false}
          rowComponent={TableSelectionComponent}
        />
      ) : (
        <TableSelection selectByRowClick showSelectionColumn={false} />
      )}
      {!props.disableGrouping && (
        <TableGroupRow
          contentComponent={ContentComponent}
          indentColumnWidth={props.groupRowIndentColumnWidth ?? 48}
          iconComponent={TableGroupRowIconComponent}
        />
      )}
      <TableTreeColumn
        for={props.displayGroupInColumn as string}
        indentComponent={IndentCell as any}
        expandButtonComponent={TreeExpandButtonComponent}
      />
      {props.enableEditing && (
        <TableInlineCellEditing
          selectTextOnEditStart
          cellComponent={TableInlineCellEditingComponent}
        />
      )}
    </Grid>
  );
};

export default withStyles(styles, { withTheme: true })(TreeView) as <T>(
  props: TreeViewProps<T>
) => ReactElement;
