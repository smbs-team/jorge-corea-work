// Switch.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useState } from "react";
import { withStyles, WithStyles, createStyles, Theme } from "@material-ui/core";
import ReactResizeDetector from "react-resize-detector";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  items: [string, string];
  onSelected?: (selectedIndex: number) => void;
  selectedIndex?: 0 | 1;
  disabled?: boolean;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      width: 149,
      height: 22,
      backgroundColor: "#333333",
      display: "flex",
      alignItems: "center",
      justifyContent: "space-evenly",
      borderRadius: 8,
      position: "relative",
      cursor: "pointer",
      userSelect: "none"
    },
    thumb: {
      backgroundColor: "white",
      borderRadius: 8,
      transition:
        "background-color 0.25s ease 0s, transform 0.25s ease 0s, box-shadow 0.15s ease 0s",
      position: "absolute"
    },
    label: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontWeight: "bold",
      fontSize: "0.875rem",
      display: "flex",
      justifyContent: "center",
      zIndex: 2,
      transition: "all 300ms cubic-bezier(0.4, 0, 0.2, 1) 0ms"
    }
  });

/**
 * Switch
 *
 * @param props - Component props
 * @returns A JSX element
 */
function Switch(props: Props): JSX.Element {
  const { classes } = props;
  const [transform, setTransform] = useState<string>("translateX(-47%)");
  const [selectedIndex, setSelectedIndex] = useState<number>(
    props.selectedIndex ?? 0
  );

  useEffect(() => {
    if (props.selectedIndex === 1) setTransform("translateX(47%)");
  }, []);

  const handleClick = (): void => {
    if (props.disabled) return;
    if (selectedIndex === 0) {
      setTransform("translateX(47%)");
      setSelectedIndex(1);
      props.onSelected && props.onSelected(1);
    } else {
      setTransform("translateX(-47%)");
      setSelectedIndex(0);
      props.onSelected && props.onSelected(0);
    }
  };

  return (
    <ReactResizeDetector handleWidth handleHeight>
      {({ width = 149, height = 22 }: any) => (
        <div
          className={classes.root}
          style={{ cursor: props.disabled ? "not-allowed" : "" }}
          onClick={handleClick}
        >
          <div
            style={{
              width: width / 2,
              color: selectedIndex === 0 ? "#333333" : "white"
            }}
            className={classes.label}
          >
            {props.items[0]}
          </div>
          <div
            style={{
              width: width / 2,
              color: selectedIndex === 1 ? "#333333" : "white"
            }}
            className={classes.label}
          >
            {props.items[1]}
          </div>
          <div
            style={{
              height: height - 4,
              width: width / 2,
              transform: transform
            }}
            className={classes.thumb}
          />
        </div>
      )}
    </ReactResizeDetector>
  );
}

export default withStyles(useStyles)(Switch);
