// SettingsMenu.tsx
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
  createStyles,
  Theme,
} from '@material-ui/core';
import SettingsIcon from '@material-ui/icons/Settings';
import { CustomMenuRoot } from 'components/common';

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
 * Settings menu
 */
const SettingsMenu = (): JSX.Element => {
  const classes = useStyles();
  return (
    <CustomMenuRoot icon={<SettingsIcon className={classes.icon} />} disabled>
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

export default SettingsMenu;
