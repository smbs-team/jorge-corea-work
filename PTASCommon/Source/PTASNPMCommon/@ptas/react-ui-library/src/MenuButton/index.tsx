// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from "react";
import {
  makeStyles,
  Menu,
  Button,
  MenuItem,
  MenuProps,
  ButtonProps,
  MenuItemProps
} from "@material-ui/core";
import NestedMenuItem from "../NestedMenuItem";

interface Props {
  content: React.ReactNode;
  menuItems?: Item[];
  onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
  ButtonProps?: Omit<ButtonProps, "onClick">;
  MenuProps?: Omit<MenuProps, "open">;
  NestedMenuProps?: Omit<MenuProps, "open" | "children">;
}

interface Item extends Omit<MenuItemProps, "onClick"> {
  onItemClick?: () => void;
  content: React.ReactNode;
  children?: Item[];
}

const useStyles = makeStyles({
  button: {
    color: "white",
    fontFamily: "cabin",
    fontSize: "1rem",
    "&:hover": {
      backgroundColor: "#0742ab"
    },
    height: 48,
    borderRadius: 0
  },
  paper: {
    borderRadius: 0
  },
  menuList: {
    padding: 0
  }
});

function MenuButton(props: Props): JSX.Element {
  const classes = useStyles();

  const [anchorEl, setAnchorEl] = React.useState<HTMLButtonElement | null>(
    null
  );

  const handleClick = (event: React.MouseEvent<HTMLButtonElement>): void => {
    props.onClick?.(event);
    if (!props.menuItems) return;
    setAnchorEl(event.currentTarget);
  };

  const handleClose = (item: Item): void => {
    item.onItemClick && item.onItemClick();
    setAnchorEl(null);
  };

  const renderItems = (items: Item[]): JSX.Element[] | undefined => {
    return items.map((item, index) => {
      if (item.children) {
        return (
          <NestedMenuItem
            parentMenuOpen={Boolean(anchorEl)}
            label={item.content}
            MenuProps={{
              classes: { paper: classes.paper, list: classes.menuList },
              ...props.NestedMenuProps
            }}
            key={index}
          >
            {renderItems(item.children)}
          </NestedMenuItem>
        );
      } else {
        return (
          <MenuItem
            onClick={() => {
              handleClose(item);
            }}
            key={index}
            {...delete item.onItemClick}
          >
            {item.content}
          </MenuItem>
        );
      }
    });
  };

  return (
    <Fragment>
      <Button
        onClick={handleClick}
        className={classes.button}
        style={{ color: anchorEl ? "#d4e693" : undefined }}
        {...props.ButtonProps}
      >
        {props.content}
      </Button>
      <Menu
        anchorEl={anchorEl}
        keepMounted
        open={Boolean(anchorEl)}
        onClose={handleClose}
        getContentAnchorEl={null}
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "center"
        }}
        transformOrigin={{
          vertical: "top",
          horizontal: "center"
        }}
        classes={{ paper: classes.paper, list: classes.menuList }}
        {...props.MenuProps}
      >
        {props.menuItems && renderItems(props.menuItems)}
      </Menu>
    </Fragment>
  );
}

export default MenuButton;
