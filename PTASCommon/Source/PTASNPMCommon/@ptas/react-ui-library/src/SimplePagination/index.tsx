/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useEffect } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  Theme,
  Box
} from "@material-ui/core";
import IconButton from "@material-ui/core/IconButton";
import { KeyboardArrowRight, KeyboardArrowLeft } from "@material-ui/icons";
import clsx from "clsx";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  itemsCount: number;
  onPageChange?: (currentPage: number) => void;
  onArrowClick?: (page: number) => void;
  currentPage?: number;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      width: "160px",
      fontSize: theme.ptas.typography.bodySmall.fontSize
    },
    iconButton: {
      color: theme.ptas.colors.theme.white,
      padding: 0
    },
    arrow: {
      fontSize: 35
    },
    label: {
      color: theme.ptas.colors.theme.white
    },
    hidden: {
      visibility: "hidden"
    }
  });

/**
 * SimplePagination
 *
 * @param props - Component props
 * @returns A JSX element
 */
const SimplePagination = (props: Props): JSX.Element => {
  const [currentPage, setCurrentPage] = useState(0);
  const { onPageChange } = props;
  const { classes } = props;

  useEffect(() => {
    if (props.currentPage === undefined) return;
    setCurrentPage(props.currentPage);
  }, [props.currentPage]);

  useEffect(() => {
    if (!onPageChange) return;
    onPageChange(currentPage);
  }, [currentPage]);

  return (
    <Box className={classes.root}>
      <IconButton
        className={classes.iconButton}
        onClick={(): void => {
          props?.onArrowClick && props.onArrowClick(currentPage - 1);
          setCurrentPage(currentPage - 1);
        }}
        disabled={currentPage === 0}
      >
        {
          <KeyboardArrowLeft
            className={clsx(classes.arrow, currentPage === 0 && classes.hidden)}
          />
        }
      </IconButton>
      <label className={classes.label}>
        {currentPage + 1}&nbsp;of&nbsp;{props.itemsCount}
      </label>
      <IconButton
        className={classes.iconButton}
        onClick={(): void => {
          props?.onArrowClick && props.onArrowClick(currentPage + 1);
          setCurrentPage(currentPage + 1);
        }}
        disabled={currentPage === props.itemsCount - 1}
      >
        <KeyboardArrowRight
          className={clsx(
            classes.arrow,
            currentPage === props.itemsCount - 1 && classes.hidden
          )}
        />
      </IconButton>
    </Box>
  );
};

export default withStyles(useStyles)(SimplePagination);
