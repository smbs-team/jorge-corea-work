// MenuToolBar.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, forwardRef, useEffect } from 'react';
import {
  Person as PersonIcon,
  People as PeopleIcon,
  Computer as ComputerIcon,
} from '@material-ui/icons';
import { BottomNavigation } from '@ptas/react-ui-library';
import BottomNavigationAction from 'components/BottomNavigation';
import { makeStyles, createStyles, Theme } from '@material-ui/core';
import { MenuToolBarOp } from '../common';

interface Props {
  activeTab: MenuToolBarOp;
  onChanged: (val: MenuToolBarOp) => void;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      height: 79,
      backgroundColor: theme.ptas.colors.theme.grayLight,
    },
  })
);

const MenuToolBar = forwardRef<HTMLDivElement, Props>((props: Props, ref) => {
  const classes = useStyles();
  const [activeTab, setActiveTab] = useState<MenuToolBarOp>(props.activeTab);
  const { onChanged } = props;

  const handleChange = (
    _: React.ChangeEvent<{}>,
    newValue: MenuToolBarOp
  ): void => {
    setActiveTab(newValue);
  };

  useEffect(() => {
    onChanged(activeTab);
  }, [activeTab, onChanged]);

  return (
    <BottomNavigation
      classes={{ root: classes.root }}
      onChange={handleChange}
      value={activeTab}
      ref={ref}
      style={{ height: 79 }}
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
});

export default MenuToolBar;
