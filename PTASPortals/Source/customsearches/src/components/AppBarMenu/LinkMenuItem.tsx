// LinkMenuItem.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { MenuRootItem } from '@ptas/react-ui-library';
import { useHistory } from 'react-router-dom';
import { makeStyles } from '@material-ui/core';
import clsx from 'clsx';

interface LinkMenuItemProps {
  display: string;
  link?: string;
  closeMenu?: () => void;
  redirect?: string;
  avoidNavigation?: boolean;
  disable?: boolean;
}

const useStyles = makeStyles((theme) => ({
  root: {
    fontSize: '0.875rem',
    transition: 'none !important',
    '&:hover': {
      backgroundColor: theme.ptas.colors.theme.grayLight,
    },
  },
  disabled: {
    opacity: 1 + '!important',
    backgroundColor: '#333333',
    color: 'gray',
  },
  gutters: {
    paddingLeft: 16,
    paddingRight: 16,
  },
}));

const LinkMenuItem = React.forwardRef(
  (
    {
      display,
      link,
      closeMenu,
      redirect,
      avoidNavigation,
      disable,
    }: LinkMenuItemProps,
    _ref
  ): JSX.Element => {
    const history = useHistory();
    const classes = useStyles();

    return (
      <MenuRootItem
        disabled={disable}
        onClick={(): void => {
          if (!avoidNavigation) {
            redirect
              ? window.open(redirect, '_blank')
              : history.push(link ?? '/');
          }

          closeMenu && closeMenu();
        }}
        classes={{
          root: disable ? clsx(classes.root, classes.disabled) : classes.root,
          gutters: classes.gutters,
        }}
      >
        {display}
      </MenuRootItem>
    );
  }
);

export default LinkMenuItem;
