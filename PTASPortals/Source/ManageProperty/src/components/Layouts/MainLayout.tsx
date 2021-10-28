// MainLayout.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren, useContext } from 'react';
import { Header } from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core';
import ProfileOptionsPopover from 'components/ProfileOptionsPopover';
import { AppContext } from 'contexts/AppContext';
import * as fm from '../../GeneralFormatMessage';

interface Props {
  imageUrl?: string;
}

const useStyles = makeStyles((theme: Theme) => ({
  root: {},
}));

function MainLayout(props: PropsWithChildren<Props>): JSX.Element {
  const classes = useStyles(props);
  const { children } = props;
  const {
    profileOptionsAnchor,
    showProfileOptions,
    onClickProfileOption,
  } = useContext(AppContext);

  return (
    <div className={classes.root}>
      <Header
        ptasVariant="transparent"
        showDropdown
        showMenuIcon
        showSearchIcon
        defaultValue="en"
      />
      {children}
      {profileOptionsAnchor && (
        <ProfileOptionsPopover
          anchorEl={profileOptionsAnchor}
          onClose={(): void => showProfileOptions(null)}
          onClick={onClickProfileOption}
          editProfileText={fm.editProfile}
          signOutText={fm.signOut}
        />
      )}
    </div>
  );
}

export default MainLayout;
