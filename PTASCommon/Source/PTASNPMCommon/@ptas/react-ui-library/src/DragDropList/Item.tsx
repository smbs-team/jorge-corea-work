/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from "react";
import { withStyles, WithStyles, createStyles, Theme } from "@material-ui/core";
import { Draggable } from "react-beautiful-dnd";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  id: any;
  index: number;
  isSelected: number;
  onClick: () => void;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    item: {
      boxSizing: "border-box",
      border: `1px solid ${theme.ptas.colors.theme.black}`,
      borderRadius: theme.spacing(2),
      backgroundColor: theme.ptas.colors.theme.white,
      padding: theme.spacing(1 / 8, 12 / 8),
      "&:hover": {
        backgroundColor: theme.ptas.colors.utility.selectionLight
      },
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.body.fontSize,
      width: "fit-content",
      margin: theme.spacing(1, 0)
    }
  });

/**
 * Item
 *
 * @param props - Component props
 * @returns A JSX element
 */
function Item(props: PropsWithChildren<Props>): JSX.Element {
  const { classes, isSelected, onClick, children, id, index } = props;

  const getItemStyle = (
    isDragging: any,
    draggableStyle: any,
    isSelected: any
  ) => ({
    ...draggableStyle,
    ...(isDragging && {
      backgroundColor: "#d4e693"
    }),
    ...(isSelected && {
      backgroundColor: "#d4e693"
    })
  });

  return (
    <Draggable draggableId={id} index={index}>
      {(provided, snapshot) => (
        <div
          {...provided.draggableProps}
          {...provided.dragHandleProps}
          id={id}
          className={classes.item}
          onClick={onClick}
          ref={provided.innerRef}
          style={getItemStyle(
            snapshot.isDragging,
            provided.draggableProps.style,
            isSelected
          )}
        >
          {children}
        </div>
      )}
    </Draggable>
  );
}

export default withStyles(useStyles)(Item);
