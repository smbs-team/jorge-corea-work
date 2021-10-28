// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  FC,
  Fragment,
  PropsWithChildren,
  useCallback,
  useEffect,
  useState
} from "react";
import {
  CircularProgress,
  createStyles,
  Theme,
  withStyles,
  WithStyles
} from "@material-ui/core";
import {
  SelectionState,
  TreeDataState,
  CustomTreeData,
  DataTypeProvider
} from "@devexpress/dx-react-grid";
import {
  Grid,
  TableTreeColumn,
  Table
} from "@devexpress/dx-react-grid-material-ui";

import { omit } from "lodash";
import RemoveIcon from "@material-ui/icons/Remove";
import AddIcon from "@material-ui/icons/Add";
import { GenericWithStyles } from "../common/types";
import tinycolor from "tinycolor2";
import { useToggle } from "react-use";

export interface SideTreeRow {
  id: React.ReactText;
  parentId: React.ReactText | null;
  name: string;
  isCheckable: boolean;
  isChecked: boolean;
  isSelected: boolean;
  shapeType?: "pill" | "square" | "line";
  fillColor?: string;
  outlineColor?: string;
  disableCheckBox?: boolean;
  isGroup?: boolean;
}

interface Props extends WithStyles<typeof useStyles> {
  rows: SideTreeRow[];
  width?: number;
  collapseAll?: boolean;
  isLoading?: boolean;
  noDataMessage?: string;
  disableSelection?: boolean;
  iconClicked?: boolean;
  isIconClicked?: (state: boolean) => void;
  onSelected?: (row: SideTreeRow) => void;
  onChecked?: (row: SideTreeRow) => void;
  isCollapsed?: (state: boolean) => void;
  onContextMenu?: (
    e: React.MouseEvent<HTMLElement, MouseEvent>,
    row: SideTreeRow
  ) => void;
}

export type SideTreeProps = GenericWithStyles<Props>;

const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      marginBottom: "0px !important",
      width: (props: Props) => (props.width ? props.width : 400),
      "& tbody tr": {
        height: 24
      },
      "& tbody tr td": {
        fontFamily: theme.ptas.typography.bodyFontFamily,
        fontSize: "1.125rem",
        fontWeight: "normal",
        borderBottom: "none",
        padding: 0
      }
    },
    expandButton: {
      display: "flex",
      cursor: "pointer"
    },
    expandIcon: {
      fontSize: 20,
      marginLeft: -23
    },
    colorSquare: {
      width: 10,
      height: 10,
      boxSizing: "unset"
    },
    colorPill: {
      width: 52,
      height: 12,
      borderRadius: 12,
      boxSizing: "unset"
    },
    tableContainer: {
      overflowX: "hidden"
    },
    td: {
      display: "flex",
      justifyContent: "center"
    },
    loadingIcon: {},
    noDataMessage: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: "bold"
    },
    rootComponent: {
      height: "inherit"
    },
    colorContainer: {
      width: 60,
      height: 20,
      borderRadius: 12,
      marginRight: 14,
      display: "flex",
      alignItems: "center",
      justifyContent: "center"
    },
    colorSquareContainer: {
      width: 20,
      height: 20,
      marginRight: 14,
      display: "flex",
      alignItems: "center",
      justifyContent: "center"
    },
    nameContainer: {
      display: "flex",
      alignItems: "center",
      cursor: "pointer",
      "&:hover": {
        fontWeight: "bold"
      }
    }
  });

const getChildRows = (row: SideTreeRow, rootRows: SideTreeRow[]) => {
  const childRows = rootRows.filter(
    (r) => r.parentId === (row ? row.id : null)
  );
  return childRows.length ? childRows : null;
};

/**
 * SideTree
 *
 * @param props - Component props
 * @returns A JSX element
 */
function SideTree(props: Props): JSX.Element {
  const { classes } = props;
  const [selection, setSelection] = useState<React.ReactText[]>([]);
  const [expandedRowIds, setExpandedRowIds] = useState<React.ReactText[]>([]);
  const [columns] = useState([{ name: "name", title: "Name" }]);
  const checkerBoardPattern =
    "repeating-conic-gradient(#808080 0% 25%, transparent 0% 50%) 50% / 10px 10px";
  const [on, toggle] = useToggle(false);
  const [iconClicked, toggleIconClicked] = useToggle(false);

  useEffect(() => {
    if (!props.rows || expandedRowIds.length >= 1) return;
    setExpandedRowIds(props.rows.flatMap((_r, i) => i));
  }, [props.rows]);

  useEffect(() => {
    if (props.collapseAll === undefined) return;

    if (on && !props.iconClicked) {
      toggle(false);
      toggleIconClicked(false);
      return;
    }

    if (props.collapseAll) {
      setExpandedRowIds([]);
    } else {
      //expand all
      setExpandedRowIds(props.rows.flatMap((_r, i) => i));
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.collapseAll]);

  useEffect(() => {
    if (!expandedRowIds || expandedRowIds.length < 1) return;
    props.isIconClicked && props.isIconClicked(false);

    if (iconClicked && !on) {
      toggle(false);
      toggleIconClicked(false);
      return;
    }

    const nullParentIndex = props.rows.findIndex((r) => r.parentId === null);

    if (
      expandedRowIds.find((index) => index === nullParentIndex) === undefined
    ) {
      props.isCollapsed && props.isCollapsed(true);
      toggle(true);
    } else {
      props.isCollapsed && props.isCollapsed(false);
      toggle(true);
    }
  }, [expandedRowIds]);

  const TableComponent: React.ComponentType<object> = ({
    ...restProps
  }): JSX.Element => <Table.Table {...restProps} className={classes.root} />;

  const TreeExpandButtonComponent = (
    props: TableTreeColumn.ExpandButtonProps
  ) => {
    const newProps = omit(props, ["visible", "expanded"]);
    return (
      <Fragment>
        {props.visible && (
          <span
            className={classes.expandButton}
            onClick={(e) => {
              props.onToggle();
              e.stopPropagation();
            }}
            {...newProps}
          >
            {props.expanded ? (
              <RemoveIcon className={classes.expandIcon} />
            ) : (
              <AddIcon className={classes.expandIcon} />
            )}
          </span>
        )}
      </Fragment>
    );
  };

  const NameColumnFormatter = (
    nameProps: DataTypeProvider.ValueFormatterProps
  ): JSX.Element => {
    const [isSelected, setIsSelected] = useState<boolean>(
      nameProps.row.isSelected
    );
    const row = nameProps.row as SideTreeRow;
    return (
      <div
        key={row.id}
        style={{
          backgroundColor: !props.disableSelection
            ? nameProps.row.isSelected
              ? "#d4e693"
              : ""
            : ""
        }}
        className={classes.nameContainer}
        onClick={() => {
          if (props.disableSelection) return;
          setIsSelected(!isSelected);
          nameProps.row.isSelected = !nameProps.row.isSelected;
          props.onSelected && props.onSelected(nameProps.row);
        }}
      >
        {row.shapeType !== "line" && row.shapeType && (
          <div
            className={
              row.shapeType === "square"
                ? classes.colorSquareContainer
                : classes.colorContainer
            }
            style={{ background: checkerBoardPattern }}
          >
            <div
              className={
                row.shapeType === "square"
                  ? classes.colorSquare
                  : classes.colorPill
              }
              style={{
                backgroundColor: row.fillColor,
                border: `${row.shapeType === "square" ? 5 : 4}px solid ${
                  tinycolor(row.outlineColor).getAlpha() === 0
                    ? "unset"
                    : row.outlineColor
                }`
              }}
            />
          </div>
        )}
        {nameProps.row.isCheckable && (
          <input
            type='checkbox'
            defaultChecked={nameProps.row.isChecked}
            onClick={(e) => {
              nameProps.row.isChecked = !nameProps.row.isChecked;
              props.onChecked && props.onChecked(nameProps.row);
              e.stopPropagation();
            }}
            disabled={nameProps.row.disableCheckBox}
          />
        )}
        <span
          key={row.id + "-name-el"}
          style={{ marginLeft: nameProps.row.isCheckable ? 6 : 0 }}
          onContextMenu={(e) => {
            e.preventDefault();
            props.onContextMenu?.(e, row);
          }}
        >
          {nameProps.value}
        </span>
      </div>
    );
  };

  const TableContainer = (props: PropsWithChildren<object>): JSX.Element => {
    return (
      <Table.Container {...props} className={classes.tableContainer}>
        {props.children}
      </Table.Container>
    );
  };

  const NoDataComponent = (noDataProps: Table.NoDataCellProps) =>
    props.isLoading ? (
      <td colSpan={noDataProps.colSpan} className={classes.td}>
        <CircularProgress className={classes.loadingIcon} />
      </td>
    ) : (
      <td
        colSpan={noDataProps.colSpan}
        rowSpan={noDataProps.rowSpan}
        className={classes.td}
      >
        <label className={classes.noDataMessage}>
          {props.noDataMessage ?? "No data"}
        </label>
      </td>
    );

  const RootComponent = useCallback(
    (rootProps: Grid.RootProps): JSX.Element => {
      return (
        <Grid.Root {...rootProps} className={classes.rootComponent}>
          {rootProps.children}
        </Grid.Root>
      );
    },
    []
  );

  return (
    <Grid rows={props.rows} columns={columns} rootComponent={RootComponent}>
      <SelectionState selection={selection} onSelectionChange={setSelection} />
      <DataTypeProvider
        key='name'
        for={["name"]}
        formatterComponent={NameColumnFormatter}
      />
      <TreeDataState
        expandedRowIds={expandedRowIds}
        onExpandedRowIdsChange={setExpandedRowIds}
      />
      <CustomTreeData getChildRows={getChildRows} />
      <Table
        noDataCellComponent={NoDataComponent}
        containerComponent={TableContainer}
        tableComponent={TableComponent}
      />
      <TableTreeColumn
        for='name'
        expandButtonComponent={TreeExpandButtonComponent}
      />
    </Grid>
  );
}

export default withStyles(useStyles)(SideTree) as FC<SideTreeProps>;
