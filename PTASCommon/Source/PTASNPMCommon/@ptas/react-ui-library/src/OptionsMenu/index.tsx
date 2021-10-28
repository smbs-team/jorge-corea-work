// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  useState,
  PropsWithChildren,
  Fragment,
  useRef,
  useEffect,
  forwardRef,
  useImperativeHandle,
  ForwardedRef
} from "react";
import {
  createStyles,
  WithStyles,
  IconButton,
  Menu,
  MenuItem,
  Theme,
  withStyles
} from "@material-ui/core";

import { GenericWithStyles } from "../common/types";
import CustomPopover from "../CustomPopover";

interface Props<T> extends WithStyles<typeof useStyles> {
  row?: T;
  onItemClick: (action: MenuOption, row?: T) => void;
  items?: MenuOption[];
  closeAfterPopover?: boolean;
  customBtnIconMenu?: any;
  showTail?: boolean;
}

export type MenuOption = {
  id: string | number;
  label: string;
  disabled?: boolean;
  afterClickContent?: React.ReactNode;
  isAlert?: boolean;
  onClick?: (event: React.MouseEvent<HTMLLIElement>) => void;
};

const useStyles = (theme: Theme) =>
  createStyles({
    menuPaper: {
      border: `1px solid ${theme.ptas.colors.theme.grayLight}`,
      borderRadius: 0,
      width: "fit-content"
    },
    menuItem: {
      borderBottom: `1px solid ${theme.ptas.colors.theme.grayLight}`,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: "0.875rem",
      fontWeight: "normal",
      "&:last-child": {
        borderBottom: "none"
      }
    },
    iconButtonDots: {
      color: theme.ptas.colors.theme.black,
      marginLeft: "auto",
      marginRight: theme.spacing(2),
      padding: 0,
      fontSize: "1rem",
      "&:hover": {
        backgroundColor: "unset"
      }
    },
    iconButtonCustom: {
      color: theme.ptas.colors.theme.black,
      padding: 0,
      fontSize: "0.75rem",
      "&:hover": {
        backgroundColor: "unset"
      }
    },
    icon: {},
    tail: {},
    tailAlert: {
      backgroundColor: theme.ptas.colors.utility.danger
    },
    closeButton: {
      color: theme.ptas.colors.theme.white
    }
  });

export type OptionsMenuRefProps = {
  closePopup: () => void;
};

function OptionsMenu<T>(
  props: PropsWithChildren<Props<T>>,
  ref: ForwardedRef<OptionsMenuRefProps | undefined>
): JSX.Element {
  const { classes, customBtnIconMenu } = props;
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [anchorElPop, setAnchorElPop] = useState<null | HTMLElement>(null);
  const [content, setContent] = useState<React.ReactNode>(null);
  const iconButtonEl = useRef(null);
  const [isAlert, setIsAlert] = useState<boolean>();

  useImperativeHandle(ref, () => ({
    closePopup() {
      setAnchorElPop(null);
    }
  }));

  useEffect(() => {
    if (props.closeAfterPopover) {
      setAnchorElPop(null);
    }
  }, [props.closeAfterPopover]);

  const openMenu = (event: React.MouseEvent<HTMLElement>): void => {
    event.stopPropagation();
    setAnchorEl(event.currentTarget);
  };

  function handleContextMenuClose(event: React.MouseEvent<HTMLElement>): void {
    event.stopPropagation();
    setAnchorEl(null);
  }

  const handleClick = (
    event: React.MouseEvent<HTMLLIElement>,
    action: MenuOption,
    row: T | undefined
  ): void => {
    event.stopPropagation();
    setContent(action.afterClickContent);
    setIsAlert(action.isAlert);
    props.onItemClick(action, row);
    setAnchorEl(null);
    action.onClick?.(event);
  };

  return (
    <Fragment>
      {customBtnIconMenu ? (
        <IconButton
          disableRipple
          className={classes.iconButtonCustom}
          onClick={openMenu}
          ref={iconButtonEl}
        >
          {customBtnIconMenu}
        </IconButton>
      ) : (
        <IconButton
          disableRipple
          className={classes.iconButtonDots}
          onClick={openMenu}
          ref={iconButtonEl}
        >
          <span>...</span>
        </IconButton>
      )}
      <Menu
        anchorEl={anchorEl}
        keepMounted
        open={!!anchorEl}
        onClose={handleContextMenuClose}
        PaperProps={{ className: classes.menuPaper }}
        MenuListProps={{ disablePadding: true }}
        TransitionProps={{
          onExiting: (): void => {
            if (content) setAnchorElPop(iconButtonEl.current);
          }
        }}
      >
        {props.items?.map((item, index) => {
          return (
            <MenuItem
              key={index}
              onClick={(e): void => {
                handleClick(e, item, props.row);
              }}
              className={classes.menuItem}
              disabled={item.disabled}
            >
              {item.label}
            </MenuItem>
          );
        })}
      </Menu>
      <CustomPopover
        tail={props.showTail ?? true}
        showCloseButton
        anchorEl={anchorElPop}
        onClose={(): void => {
          setAnchorElPop(null);
        }}
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "left"
        }}
        transformOrigin={{
          vertical: "top",
          horizontal: "left"
        }}
        classes={{
          tail: isAlert ? classes.tailAlert : classes.tail,
          closeButton: isAlert ? classes.closeButton : ""
        }}
      >
        {content}
      </CustomPopover>
    </Fragment>
  );
}

export default withStyles(useStyles)(forwardRef(OptionsMenu)) as <T>(
  props: GenericWithStyles<Props<T>> & {
    ref?: ForwardedRef<OptionsMenuRefProps | undefined>;
  }
) => JSX.Element;
