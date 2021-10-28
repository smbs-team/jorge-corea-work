// DropDownTree.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, Fragment, useEffect } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Theme,
  Box,
  InputLabel,
  FormControl
} from "@material-ui/core";
import ArrowDropDownIcon from "@material-ui/icons/ArrowDropDown";
import ArrowDropUpIcon from "@material-ui/icons/ArrowDropUp";
import Tooltip from "@material-ui/core/Tooltip";
import TreeView, { TreeViewRow } from "./../TreeView";
import CustomPopover from "../CustomPopover";
import ReactResizeDetector from "react-resize-detector";
import { GenericWithStyles } from "../common/types";

export interface DropdownTreeRow extends TreeViewRow {
  title: string;
  subject: string;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  rows: DropdownTreeRow[];
  title?: string;
  onSelected?: (row: DropdownTreeRow, route: string) => void;
  label?: string;
  placeholder?: string;
  defaultValue?: string;
}

export type DropDownTreeProps = GenericWithStyles<Props>;

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    container: {
      width: "100%",
      border: "1px solid " + theme.ptas.colors.theme.black,
      borderRadius: 3,
      display: "flex",
      alignItems: "center",
      cursor: "pointer",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      padding: theme.spacing(6 / 8, 9 / 8),
      "&:hover": {
        borderColor: "black"
      },
      boxSizing: "border-box"
    },
    label: {
      transform: "translate(9px, -6px) scale(0.75) !important",
      fontSize: theme.ptas.typography.finePrint.fontSize,
      backgroundColor: theme.ptas.colors.theme.white,
      padding: theme.spacing(0, 6 / 8)
    },
    tooltip: {
      whiteSpace: "nowrap",
      overflow: "hidden",
      textOverflow: "ellipsis",
      paddingLeft: theme.spacing(1.25)
    },
    placeholder: {
      color: theme.ptas.colors.theme.grayMedium
    },
    rowContainer: {
      cursor: "pointer",
      "&:hover": {
        background: theme.ptas.colors.theme.grayLightest
      }
    },
    groupCell: {
      "&:hover": {
        background: "yellow"
      }
    },
    tableRoot: {
      marginBottom: "unset !important",
      "& tbody tr td": {
        padding: 0
      }
    },
    treeIndent: {
      marginLeft: 0
    },
    title: {
      display: "inline-block",
      textAlign: "left",
      wordWrap: "break-word",
      overflowX: "hidden",
      marginRight: 0
    },
    root: {
      width: "100%",
      display: "block"
    },
    popoverPaper: {},
    popoverRoot: {},
    treeRootComponent: {},
    textField: {
      width: "100%"
    }
  });

/**
 * DropDownTree
 *
 * @param props - Component props
 * @returns A JSX element
 */
function DropDownTree(props: Props): JSX.Element {
  const { classes, title, onSelected, label, placeholder } = props;

  const columns = [{ name: "title", title: "Title" }];

  const [fieldValue, setValue] = useState<string>();
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>();
  const [rows, setRows] = useState<DropdownTreeRow[]>(props.rows);

  useEffect(() => {
    if (!fieldValue) return;
    if (fieldValue?.startsWith("/")) return;
    setValue("/" + fieldValue);
  }, [fieldValue]);

  useEffect(() => {
    setRows(props.rows);
  }, [props.rows]);

  useEffect(() => {
    if (!props.defaultValue) return;
    setValue(props.defaultValue);
  }, [props.defaultValue]);

  const getExpandedGroups = (rows: DropdownTreeRow[]): string[] | undefined => {
    let groups: string[] = [];
    rows.forEach((r) => {
      if (groups.includes(r.subject)) return;
      groups.push(r.subject);
    });
    return groups;
  };

  const handleOnChange = (row?: DropdownTreeRow) => {
    if (!row) return;
    const route = getRoute(props.rows, row);
    onSelected?.(row, route + "/");
    setValue(route);
    setAnchorEl(null);
  };

  /**
   * Gets the location of the selected row as a string
   * @param row - The current row
   * @param rows - The row array
   */
  const getRoute = (rows: DropdownTreeRow[], row: DropdownTreeRow): string => {
    const routeArray: string[] = [];
    let parentId = row.parent;
    while (parentId != null) {
      const rowParent = rows.filter((r) => r.id === parentId);
      (parentId as string | number | null) = rowParent[0].parent;
      routeArray.push(rowParent[0].title.replace(/\//g, ""));
    }

    return `/${routeArray.reverse().join("/")}/${row.title.replace(
      /\//g,
      ""
    )}`.replace("//", "/");
  };

  return (
    <FormControl className={classes.root} variant='outlined'>
      <InputLabel shrink className={classes.label} variant='outlined'>
        {label}
      </InputLabel>
      {title && <label className={classes.title}>{title}</label>}
      <ReactResizeDetector handleWidth>
        {({ width }: { width: number }) => (
          <Fragment>
            <Box
              className={classes.container}
              onClick={(e) => {
                setAnchorEl(e.currentTarget);
              }}
            >
              <Tooltip title={fieldValue ?? ""} placement='bottom'>
                <label
                  className={`${classes.tooltip} ${
                    fieldValue ? "" : classes.placeholder
                  }`}
                >
                  {fieldValue ?? placeholder ?? ""}
                </label>
              </Tooltip>
              {anchorEl ? (
                <ArrowDropUpIcon style={{ marginLeft: "auto" }} />
              ) : (
                <ArrowDropDownIcon style={{ marginLeft: "auto" }} />
              )}
            </Box>
            <CustomPopover
              anchorOrigin={{
                vertical: "top",
                horizontal: "center"
              }}
              transformOrigin={{
                vertical: "top",
                horizontal: "center"
              }}
              marginThreshold={0}
              anchorEl={anchorEl}
              onClose={() => {
                setAnchorEl(null);
              }}
              classes={{
                root: classes.popoverRoot,
                paper: classes.popoverPaper
              }}
            >
              <Box style={{ width: `calc(${width}px + 30px)` }}>
                <TreeView<DropdownTreeRow>
                  virtual
                  virtualTableProps={{ height: 200 }}
                  columns={columns}
                  rows={rows}
                  displayGroupInColumn='title'
                  onSelect={handleOnChange}
                  groupRowIndentColumnWidth={15}
                  treeIndentColumnWidth={32}
                  expandedGroups={getExpandedGroups(props.rows)}
                  classes={{
                    rowContainer: classes.rowContainer,
                    groupCell: classes.groupCell,
                    root: classes.tableRoot,
                    rootComponent: classes.treeRootComponent
                  }}
                  hideHeader
                  hideEye
                  hideSelectionHighlight
                  disableGrouping
                ></TreeView>
              </Box>
            </CustomPopover>
          </Fragment>
        )}
      </ReactResizeDetector>
    </FormControl>
  );
}

export default withStyles(useStyles)(DropDownTree);
