// MainLayout.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren, useContext } from 'react';
import { Header } from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core/styles';
import ProfileOptionsPopover from 'components/ProfileOptionsPopover';
import { AppContext } from 'contexts/AppContext';
import * as fm from '../../GeneralFormatMessage';

interface Props {
  imageUrl?: string;
  anchorRef?:
    | React.RefObject<HTMLDivElement>
    | ((instance: HTMLDivElement) => void)
    | undefined;
  hideHeader?: boolean;
}

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    height: '100vh',
  },
  header: {
    marginBottom: 0,
    [theme.breakpoints.up('sm')]: {
      marginBottom: 8,
    },
  },
}));

function MainLayout(props: PropsWithChildren<Props>): JSX.Element {
  const classes = useStyles(props);
  const { children } = props;
  const { profileOptionsAnchor, showProfileOptions, onClickProfileOption } =
    useContext(AppContext);

  return (
    <div className={classes.root} ref={props.anchorRef}>
      {!props.hideHeader && (
        <Header
          ptasVariant="transparent"
          showDropdown
          showMenuIcon
          showSearchIcon
          classes={{ root: classes.header }}
          defaultValue="en"
        />
      )}
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
