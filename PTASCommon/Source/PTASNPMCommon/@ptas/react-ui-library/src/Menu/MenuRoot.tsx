// MenuRoot
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useRef, PropsWithChildren, useEffect } from "react";
import {
  Button,
  MenuProps,
  withStyles,
  PopoverPosition,
  createStyles,
  Theme,
  WithStyles
} from "@material-ui/core";
import { omit } from "lodash";

const useMenuStyles = (theme: Theme) =>
  createStyles({
    root: {
      height: 48,
      backgroundColor: "inherit",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: "1rem",
      color: theme.palette.common.white,
      backdropFilter: "blur(16px)",
      padding: theme.spacing(6 / 8, 1),
      "&:hover": {
        backgroundColor: "#0742ab"
      }
    },
    containedSecondary: {
      backgroundColor: "inherit",
      color: theme.palette.common.white,
      borderRadius: 0
    },
    isOpen: {
      color: theme.ptas.colors.utility.selectionLight
    }
  });

export interface RenderMenuProps {
  menuProps: MenuProps;
  isMenuRootOpen: boolean;
  closeMenu: () => void;
}

export interface RenderMenu {
  (renderMenuProps: RenderMenuProps): JSX.Element;
}

export interface MenuRootProps extends WithStyles<typeof useMenuStyles> {
  text?: string;
  icon?: React.ReactNode;
  children?: RenderMenu;
  disabled?: boolean;
  onToggle?: (val: boolean) => void;
  onClick?: (e: React.MouseEvent) => void;
}

/**
 * Adds a button that will display a menu on click
 *
 * @remarks
 * @param props - Component props
 */

const MenuRoot = (props: PropsWithChildren<MenuRootProps>): JSX.Element => {
  const rootRef = useRef<HTMLButtonElement | null>(null);
  const [menuPosition, setMenuPosition] = useState<PopoverPosition | null>(
    null
  );
  const [isOpen, setIsOpen] = useState<boolean>(false);

  const { classes } = props;

  const handleRootClick = (event: React.MouseEvent): void => {
    if (menuPosition) return;
    event.preventDefault();
    event.stopPropagation();
    const btnBoundingClientRect = (rootRef.current as HTMLButtonElement).getBoundingClientRect();
    setMenuPosition({
      top: btnBoundingClientRect.bottom,
      left: btnBoundingClientRect.left
    });
    setIsOpen(true);
    props.onClick && props.onClick(event);
    if (!props.children) setMenuPosition(null);
  };

  const closeMenu = (): void => {
    setIsOpen(false);
    setMenuPosition(null);
  };

  const menuProps: MenuProps = {
    open: !!menuPosition,
    onClose: closeMenu,
    anchorReference: "anchorPosition",
    anchorPosition: menuPosition as PopoverPosition
  };

  useEffect(() => {
    props.onToggle && props.onToggle(isOpen);
  }, [isOpen]);

  return (
    <Button
      onClick={handleRootClick}
      ref={rootRef}
      classes={omit(classes, "isOpen")}
      className={props.children ? (isOpen ? classes.isOpen : "") : ""}
      variant={menuPosition ? "contained" : "text"}
      color='secondary'
      disableElevation
      disabled={props.disabled}
      style={{ color: props.disabled ? "rgba(255, 255, 255, 0.3)" : "" }}
    >
      {props.icon}
      {props.text}
      {isOpen &&
        props.children?.({
          menuProps,
          isMenuRootOpen: !!menuPosition,
          closeMenu: closeMenu
        })}
    </Button>
  );
};

export default withStyles(useMenuStyles)(MenuRoot);
