// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useEffect, useRef, useState } from "react";
import { withStyles, WithStyles, createStyles, Theme } from "@material-ui/core";
import { DragDropContext, Droppable } from "react-beautiful-dnd";
import { useUpdateEffect } from "react-use";
import MenuIcon from "@material-ui/icons/Menu";
import Item from "./Item";
import OptionsMenu, { MenuOption } from "../OptionsMenu";

export type DragDropItem = {
  id: string;
  content: string;
};

/**
 * Component props
 */
export interface DragDropListProps extends WithStyles<typeof useStyles> {
  data: DragDropItem[];
  onItemsChange?: (item: DragDropItem[]) => void;
  onSelectionChange?: (item: DragDropItem | undefined) => void;
  useContextMenu?: boolean;
  selected?: DragDropItem;
}

interface ContextMenuProps {
  item: DragDropItem;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      padding: theme.spacing(1),
      width: "250px",
      maxHeight: "50vh",
      overflow: "auto"
    },
    endButton: {
      padding: theme.spacing(0.8),
      cursor: "pointer"
    },
    inputRename: {
      background: "transparent",
      borderWidth: "0",
      borderBottom: "1px solid #000",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.formLabel.fontSize,
      outline: "none",
      width: "100%"
    },
    menuIcon: {
      fontSize: "16px",
      paddingBottom: "3px"
    }
  });

/**
 * DragDropList
 *
 * @param props - Component props
 * @returns A JSX element
 */
function DragDropList(props: DragDropListProps): JSX.Element {
  const {
    onItemsChange,
    onSelectionChange,
    classes,
    useContextMenu,
    selected
  } = props;
  const [items, setItems] = useState<DragDropItem[]>([]);
  const [selectedItem, setSelectedItem] = useState<DragDropItem>();
  const [itemIdEdit, setItemIdEdit] = useState<string>("");
  const [itemLabelEdit, setItemLabelEdit] = useState<string>("");
  const renameInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    setSelectedItem(selected);
  }, [selected]);

  useEffect(() => {
    onSelectionChange?.(selectedItem);
  }, [selectedItem]);

  useEffect(() => {
    setItems(props.data);
  }, [JSON.stringify(props.data)]);

  useUpdateEffect(() => {
    onItemsChange?.(items);
  }, [JSON.stringify(items)]);

  const onMenuItemClick = (
    _action: MenuOption,
    row: DragDropItem | undefined
  ): void => {
    if (!row) return;
    switch (_action.id) {
      case "Rename": {
        setItemIdEdit(row.id);
        setItemLabelEdit(row.content);
        break;
      }
      case "Delete":
        setItems((prev) => prev.filter((curr) => curr.id !== row.id));
        break;
    }
  };

  const ContextMenu = (props: ContextMenuProps): JSX.Element => {
    const { item } = props;
    return (
      <OptionsMenu<DragDropItem>
        row={item}
        onItemClick={onMenuItemClick}
        customBtnIconMenu={<MenuIcon className={classes.menuIcon} />}
        items={[
          {
            label: "Rename",
            id: "Rename"
          },
          {
            label: "Delete",
            id: "Delete",
            isAlert: true
          }
        ]}
      />
    );
  };

  const onBlurInputRename = (): void => {
    const contentExist = (itemLabelEdit || "").replace(
      new RegExp("s", "g"),
      ""
    );
    if (contentExist) {
      const itemsUpdated = items.map((item) =>
        item.id === itemIdEdit ? { ...item, content: itemLabelEdit } : item
      );
      setItems(itemsUpdated);
    }
    setItemIdEdit("");
  };

  const onKeyDownRename = (e: React.KeyboardEvent<HTMLInputElement>): void => {
    if (e.key === "Escape") {
      setItemIdEdit("");
    } else if (e.key === "Enter") {
      renameInputRef.current?.blur();
    }
  };

  const renderInputRename = (): JSX.Element => {
    return (
      <input
        ref={renameInputRef}
        autoFocus
        className={classes.inputRename}
        value={itemLabelEdit}
        onChange={(e) => setItemLabelEdit(e.currentTarget.value)}
        onBlur={onBlurInputRename}
        onKeyDown={onKeyDownRename}
      />
    );
  };

  function ItemRow({ data, index, onClick, isSelected }: any) {
    const editMode = !!itemIdEdit && itemIdEdit === data.id;
    return (
      <Item
        id={data.id}
        index={index}
        onClick={onClick}
        isSelected={isSelected}
      >
        {editMode ? renderInputRename() : data.content}
        {useContextMenu ? (
          !editMode && (
            <span className={classes.endButton}>
              <ContextMenu item={data} />
            </span>
          )
        ) : (
          <span
            className={classes.endButton}
            onClick={(event): void => {
              event.stopPropagation();
              setItems((prev) => prev.filter((curr) => curr.id !== data.id));
            }}
          >
            x
          </span>
        )}
      </Item>
    );
  }

  const reorder = (
    list: DragDropItem[],
    startIndex: number,
    endIndex: number
  ): DragDropItem[] => {
    const result = Array.from(list);
    const [removed] = result.splice(startIndex, 1);
    result.splice(endIndex, 0, removed);

    return result;
  };

  const onDragEnd = (result: any) => {
    if (!result.destination) {
      return;
    }

    const newItems: DragDropItem[] = reorder(
      items,
      result.source.index,
      result.destination.index
    );
    setItems(newItems);
  };

  const handleClick = (clickedItem: DragDropItem) => {
    if (!!itemIdEdit) return; // if renamed is active the onclick has no effect
    if (clickedItem.id === selectedItem?.id) {
      setSelectedItem(undefined);
      onSelectionChange?.(undefined);
    } else {
      setSelectedItem(clickedItem);
      onSelectionChange?.(clickedItem);
    }
  };

  const ItemList = ({ items }: { items: DragDropItem[] }): JSX.Element => (
    <Fragment>
      {items.map((item: DragDropItem, index: number) => (
        <ItemRow
          data={item}
          index={index}
          key={item.id}
          onClick={() => handleClick(item)}
          isSelected={item.id === selectedItem?.id}
        />
      ))}
    </Fragment>
  );

  return (
    <Fragment>
      <DragDropContext onDragEnd={onDragEnd}>
        <Droppable droppableId='list'>
          {(provided) => (
            <div
              ref={provided.innerRef}
              {...provided.droppableProps}
              className={classes.root}
            >
              <ItemList items={items} />
              {provided.placeholder}
            </div>
          )}
        </Droppable>
      </DragDropContext>
    </Fragment>
  );
}

export default withStyles(useStyles)(DragDropList);
