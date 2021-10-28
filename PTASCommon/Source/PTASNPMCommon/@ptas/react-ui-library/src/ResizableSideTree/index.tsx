// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useState } from "react";
import CustomIconButton from "../CustomIconButton";
import SideTree, { SideTreeProps, SideTreeRow } from "../SideTree";
import { Resizable, ResizableProps } from "re-resizable";
import FastRewindIcon from "@material-ui/icons/FastRewind";
import FastForwardIcon from "@material-ui/icons/FastForward";
import UnfoldMoreIcon from "@material-ui/icons/UnfoldMore";
import UnfoldLessIcon from "@material-ui/icons/UnfoldLess";
import { createStyles, withStyles, WithStyles } from "@material-ui/core";

const OPTIONS_HEADER_HEIGHT = 18;

interface Props extends WithStyles<typeof useStyles> {
  rows: SideTreeRow[];
  minWidth?: number;
  defaultWidth?: number;
  SideTreeProps?: SideTreeProps;
  ResizableProps?: ResizableProps;
  isLoading?: boolean;
  noDataMessage?: string;
  disableSelection?: boolean;
  onChecked?: (row: SideTreeRow) => void;
  onSelected?: (row: SideTreeRow) => void;
  onWidthChange?: (width: number) => void;
}

const useStyles = () =>
  createStyles({
    resizable: {
      borderRight: "4px solid #444444"
    },
    optionsHeader: {
      height: OPTIONS_HEADER_HEIGHT,
      display: "flex",
      flexWrap: "wrap",
      marginBottom: 4
    },
    resizeDecoration: {
      height: 45,
      width: 20,
      backgroundColor: "#444444",
      position: "absolute",
      right: -12,
      bottom: "50%",
      borderRadius: 10
    },
    decorationWhiteSquare: {
      width: 4,
      height: 4,
      backgroundColor: "white",
      position: "absolute"
    }
  });

function ResizableSideTree(props: Props) {
  const { classes } = props;
  const minWidth = props.minWidth ?? 30;
  const defaultWidth = props.defaultWidth ?? 250;
  const [width, setWidth] = useState(defaultWidth);
  const [collapseAll, setCollapseAll] = useState<boolean>(false);
  const [firstRender, setFirstRender] = useState<boolean>(false);
  const [iconClicked, setIconClicked] = useState<boolean>(false);

  useEffect(() => {
    if (props.rows.length < 1) {
      setWidth(minWidth);
    } else {
      if (firstRender) return;
      setWidth(defaultWidth);
      setFirstRender(true);
    }
  }, [props.rows]);

  useEffect(() => {
    props.onWidthChange?.(width);
  }, [width]);

  return (
    <Resizable
      enable={{
        top: false,
        topLeft: false,
        topRight: false,
        bottom: false,
        bottomLeft: false,
        bottomRight: false,
        left: false,
        right: true
      }}
      size={{ width: width, height: "100%" }}
      onResizeStop={(_e, _direction, _ref, d) => {
        setWidth(width + d.width);
      }}
      className={classes.resizable}
      minWidth={minWidth}
      {...props.ResizableProps}
    >
      <div className={classes.optionsHeader}>
        {width > 55 && props.rows.length >= 1 && (
          <CustomIconButton
            icon={collapseAll ? <UnfoldMoreIcon /> : <UnfoldLessIcon />}
            style={{ color: "black" }}
            disableRipple={false}
            onClick={(): void => {
              setCollapseAll(!collapseAll);
              setIconClicked(true);
            }}
          />
        )}
        <CustomIconButton
          onClick={(): void =>
            width === minWidth ? setWidth(defaultWidth) : setWidth(minWidth)
          }
          icon={width === minWidth ? <FastForwardIcon /> : <FastRewindIcon />}
          style={{ marginLeft: "auto", marginRight: 8, color: "black" }}
          disableRipple={false}
        />
      </div>
      <div
        style={{
          display: width === minWidth ? "none" : "",
          height: `calc(100% - ${OPTIONS_HEADER_HEIGHT}px)`,
          paddingLeft: 5
        }}
      >
        <SideTree
          width={width}
          rows={props.rows}
          onChecked={props.onChecked}
          onSelected={props.onSelected}
          collapseAll={collapseAll}
          noDataMessage={width === minWidth ? " ‎ ‎" : props.noDataMessage}
          isLoading={width === minWidth ? undefined : props.isLoading}
          disableSelection={props.disableSelection}
          isCollapsed={(state) => setCollapseAll(state)}
          iconClicked={iconClicked}
          isIconClicked={(state) => setIconClicked(state)}
          {...props.SideTreeProps}
        />
      </div>
      <div className={classes.resizeDecoration}>
        <div
          className={classes.decorationWhiteSquare}
          style={{
            right: "42%",
            bottom: "32%"
          }}
        ></div>
        <div
          className={classes.decorationWhiteSquare}
          style={{
            right: "38%",
            top: "30%"
          }}
        ></div>
      </div>
    </Resizable>
  );
}

export default withStyles(useStyles)(ResizableSideTree);
