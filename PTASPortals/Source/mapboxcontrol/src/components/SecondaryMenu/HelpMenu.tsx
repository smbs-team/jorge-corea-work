// HelpMenu.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { MenuRootContent, CustomNestedItem } from '@ptas/react-ui-library';
import {
  MenuItem,
  Box,
  makeStyles,
  Theme,
  createStyles,
} from '@material-ui/core';
import { CustomMenuRoot } from 'components/common';
import HelpOutlineIcon from '@material-ui/icons/HelpOutline';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    item: {
      fontSize: '0.875rem',
      fontFamily: theme.ptas.typography.bodyFontFamily,
      height: 27,
      padding: theme.spacing(1),
    },
    icon: {
      color: 'gray',
    },
  })
);

/**
 * Help menu
 */
const HelpMenu = (): JSX.Element => {
  const classes = useStyles();
  return (
    <CustomMenuRoot
      icon={<HelpOutlineIcon className={classes.icon} />}
      disabled
    >
      {({ closeMenu, isMenuRootOpen, menuProps }): JSX.Element => (
        <MenuRootContent {...menuProps}>
          <CustomNestedItem
            label={
              <Box component="span" width="100%">
                Submenu
              </Box>
            }
            parentMenuOpen={isMenuRootOpen}
          >
            <MenuItem
              classes={{ root: classes.item }}
              onClick={(): void => {
                closeMenu();
              }}
            >
              Close
            </MenuItem>
            <MenuItem classes={{ root: classes.item }}>Option</MenuItem>
          </CustomNestedItem>
        </MenuRootContent>
      )}
    </CustomMenuRoot>
  );
};

export default HelpMenu;
