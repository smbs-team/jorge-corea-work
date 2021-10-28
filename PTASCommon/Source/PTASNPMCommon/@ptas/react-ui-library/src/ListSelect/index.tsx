// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { ComponentProps } from "react";
import {
  Theme,
  WithStyles,
  createStyles,
  withStyles,
  ListItem,
  Button
} from "@material-ui/core";
import {
  FixedSizeList,
  FixedSizeListProps,
  ListChildComponentProps
} from "react-window";
import { useList } from "react-use";
import { ListActions } from "react-use/lib/useList";
import { CustomButton } from "../CustomButton";
import { GenericWithStyles } from "../common";
import OverflowToolTip from "../OverflowToolTip";

interface Props<T> extends WithStyles<typeof useStyles> {
  inputList: ListObject<T>[];
  selectedItems: ListObject<T>[];
  onDone?: (inputList: ListObject<T>[], selectedItems: ListObject<T>[]) => void;
  onCancel?: () => void;
  leftListTitle?: string;
  rightListTitle?: string;
  cancelButtonText?: string;
  confirmButtonText?: string;
  FixedSizeListProps?: Partial<FixedSizeListProps>;
  ListItemProps?: Omit<ComponentProps<typeof ListItem>, "button"> & {
    button?: true;
  };
}

interface Item {
  id: React.ReactText;
  name: string;
}

export type ListObject<T> = T & Item;
export type ListSelectProps<T> = GenericWithStyles<Props<T>>;

const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      flexDirection: "column",
      width: 892,
      border: "1px solid black",
      padding: theme.spacing(4.125),
      fontFamily: theme.ptas.typography.bodyFontFamily
    },
    listsContainer: {
      display: "flex",
      padding: theme.spacing(2, 2.5, 3, 2.5),
      justifyContent: "space-around",
      backgroundColor: "#c9f1fd",
      backgroundClip: "padding-box",
      width: 850,
      alignSelf: "center",
      border: "1px solid black"
    },
    list: {
      border: "1px solid black",
      borderRadius: 3,
      backgroundColor: "white"
    },
    button: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      color: "#8d8d8d",
      height: 42,
      width: 65,
      border: "none",
      backgroundColor: "#f5f5f5",
      borderBottom: "1px solid #c8c8c8",
      fontSize: "32px",
      borderRadius: 0,
      "&:last-child": {
        borderBottom: "none"
      },
      "&:hover": {
        backgroundColor: "#f5f5f5",
        color: "#169db3"
      },
      "&:active": {
        transform: "scale(0.98)"
      }
    },
    itemSelected: {
      backgroundColor: "#169db3 !important"
    },
    itemText: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      textTransform: "none",
      textOverflow: "ellipsis",
      overflow: "hidden",
      whiteSpace: "nowrap"
    },
    listLabel: {
      display: "block",
      fontWeight: "bold",
      fontSize: "1.125rem",
      marginBottom: theme.spacing(2),
      width: (props: Props<{}>) => props.FixedSizeListProps?.width ?? 360
    },
    actionButtonContainer: {
      display: "flex",
      flexDirection: "column",
      border: "1px solid #c8c8c8",
      height: "fit-content",
      alignSelf: "center"
    },
    buttonContainer: {
      display: "flex",
      alignSelf: "flex-end",
      paddingTop: 30
    }
  });

/**
 * ListSelect
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ListSelect<T>(props: Props<T>): JSX.Element {
  const { classes, inputList } = props;
  const [selectedItems, selectedItemsProps] = useList<React.ReactText>([]);
  const [rightSelectedItems, rightSelectedItemsProps] = useList<
    React.ReactText
  >([]);

  const [leftList, leftListProps] = useList<ListObject<T>>(inputList);
  const [rightList, rightListProps] = useList<ListObject<T>>(
    props.selectedItems
  );

  const handleOnItemClick = (
    id: React.ReactText,
    selectedList: React.ReactText[],
    selectedListActions: ListActions<React.ReactText>
  ): void => {
    const index = selectedList.indexOf(id);
    const isSelected = selectedList.includes(id);
    if (isSelected) {
      selectedListActions.removeAt(index);
    } else {
      selectedListActions.push(id);
    }
  };

  const handleOnAdd = (): void => {
    const clone: ListObject<T>[] = [...leftList];

    selectedItems.forEach((itemId) => {
      const item = clone.find((item) => item.id === itemId);
      if (!item) return;
      const itemIndex = clone.indexOf(item);
      rightListProps.push(item);
      clone.splice(itemIndex, 1);
    });

    rightListProps.sort((a, b) => a.name.localeCompare(b.name));
    selectedItemsProps.clear();
    leftListProps.set(clone);
  };

  const handleOnRemove = (): void => {
    const clone: ListObject<T>[] = [...rightList];

    rightSelectedItems.forEach((itemId) => {
      const item = clone.find((item) => item.id === itemId);
      if (!item) return;
      const itemIndex = clone.indexOf(item);
      leftListProps.push(item);
      clone.splice(itemIndex, 1);
    });

    leftListProps.sort((a, b) => a.name.localeCompare(b.name));
    rightSelectedItemsProps.clear();
    rightListProps.set(clone);
  };

  const handleAddAll = () => {
    rightListProps.set([...rightList, ...leftList]);
    rightListProps.sort((a, b) => a.name.localeCompare(b.name));
    leftListProps.clear();
    selectedItemsProps.clear();
  };

  const handleRemoveAll = () => {
    leftListProps.set([...leftList, ...rightList]);
    leftListProps.sort((a, b) => a.name.localeCompare(b.name));
    rightListProps.clear();
    rightSelectedItemsProps.clear();
  };

  const handleOnDoneClick = () => {
    props.onDone && props.onDone(leftList, rightList);
  };

  const Row = (listChildProps: ListChildComponentProps) => {
    const { index, style, data } = listChildProps;

    const list = data.list as ListObject<T>[];
    const selectedIds = data.selectedItems as React.ReactText[];
    const selectedActions = data.selectedItemsActions;

    const rowId = list[index].id;

    return (
      <ListItem
        button
        style={style}
        key={index}
        onClick={() => handleOnItemClick(rowId, selectedIds, selectedActions)}
        selected={selectedIds.includes(rowId)}
        classes={{ selected: classes.itemSelected }}
        disableRipple
        {...props.ListItemProps}
      >
        <OverflowToolTip>{list[index].name}</OverflowToolTip>
      </ListItem>
    );
  };

  return (
    <div className={classes.root}>
      <div className={classes.listsContainer}>
        <div>
          <label className={classes.listLabel}>
            {props.leftListTitle ?? "Left List"}
          </label>
          <FixedSizeList
            className={classes.list}
            height={615}
            width={360}
            itemSize={20}
            itemCount={leftList.length}
            itemData={{
              list: leftList,
              selectedItems: selectedItems,
              selectedItemsActions: selectedItemsProps
            }}
            {...props.FixedSizeListProps}
          >
            {Row}
          </FixedSizeList>
        </div>
        <div className={classes.actionButtonContainer}>
          <Button
            onClick={handleAddAll}
            disabled={leftList.length === 0}
            className={classes.button}
            disableRipple
          >
            {">>"}
          </Button>
          <Button
            onClick={handleOnAdd}
            disabled={leftList.length === 0 || selectedItems.length === 0}
            className={classes.button}
            disableRipple
          >
            {">"}
          </Button>
          <Button
            onClick={handleOnRemove}
            disabled={rightList.length === 0 || rightSelectedItems.length === 0}
            className={classes.button}
            disableRipple
          >
            {"<"}
          </Button>
          <Button
            onClick={handleRemoveAll}
            disabled={rightList.length === 0}
            className={classes.button}
            disableRipple
          >
            {"<<"}
          </Button>
        </div>
        <div>
          <label className={classes.listLabel}>
            {props.rightListTitle ?? "Right List"}
          </label>
          <FixedSizeList
            className={classes.list}
            height={615}
            width={360}
            itemSize={20}
            itemCount={rightList.length}
            itemData={{
              list: rightList,
              selectedItems: rightSelectedItems,
              selectedItemsActions: rightSelectedItemsProps
            }}
            {...props.FixedSizeListProps}
          >
            {Row}
          </FixedSizeList>
        </div>
      </div>
      <div className={classes.buttonContainer}>
        <CustomButton
          ptasVariant='commercial'
          style={{ fontWeight: "normal", marginRight: 34, width: 95 }}
          onClick={props.onCancel}
        >
          {props.cancelButtonText ?? "Cancel"}
        </CustomButton>
        <CustomButton
          ptasVariant='commercial'
          style={{ fontWeight: "normal", width: 95 }}
          onClick={handleOnDoneClick}
        >
          {props.confirmButtonText ?? "Done"}
        </CustomButton>
      </div>
    </div>
  );
}

export default withStyles(useStyles, { withTheme: true })(ListSelect) as <T>(
  props: ListSelectProps<T>
) => React.ReactElement;
