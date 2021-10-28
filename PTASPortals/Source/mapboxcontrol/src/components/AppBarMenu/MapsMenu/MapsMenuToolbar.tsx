// MapsMenuToolbar.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { forwardRef, Dispatch, SetStateAction } from 'react';
import {
  BottomNavigation,
  Theme,
  makeStyles,
  createStyles,
} from '@material-ui/core';
import {
  Person as PersonIcon,
  People as PeopleIcon,
  Computer as ComputerIcon,
} from '@material-ui/icons';
import { rearg } from 'lodash';
import BottomNavigationAction from 'components/BottomNavigation';
import { MenuToolBarOp } from '../common';

interface Props {
  activeTab: MenuToolBarOp;
  setActiveTab: Dispatch<SetStateAction<MenuToolBarOp>>;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      height: 79,
      backgroundColor: theme.ptas.colors.theme.grayLight,
    },
  })
);

const MapsMenuToolbar = forwardRef<HTMLDivElement, Props>(
  (props: Props, ref) => {
    const { activeTab, setActiveTab } = props;
    const classes = useStyles();

    return (
      <BottomNavigation
        onChange={rearg(setActiveTab, [1])}
        value={activeTab}
        ref={ref}
        classes={{ root: classes.root }}
      >
        <BottomNavigationAction
          key={1}
          selected={activeTab === 'system'}
          label="System"
          value="system"
          icon={<ComputerIcon />}
        />
        <BottomNavigationAction
          key={2}
          selected={activeTab === 'shared'}
          label="Shared"
          value="shared"
          icon={<PeopleIcon />}
        />
        <BottomNavigationAction
          key={3}
          selected={activeTab === 'user'}
          label="User"
          value="user"
          icon={<PersonIcon />}
        />
      </BottomNavigation>
    );
  }
);

export default MapsMenuToolbar;
