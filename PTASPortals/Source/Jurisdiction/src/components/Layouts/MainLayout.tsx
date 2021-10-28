// MainLayout.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren, Fragment } from 'react';
import { Header } from '@ptas/react-public-ui-library';
import { makeStyles } from '@material-ui/core';
import ProfileOptionsPopover from 'components/ProfileOptionsPopover';
import { useAppContext } from 'contexts/AppContext';
import * as fm from '../../GeneralFormatMessage';

interface Props {
  imageUrl?: string;
  anchorRef?:
    | React.RefObject<HTMLDivElement>
    | ((instance: HTMLDivElement) => void)
    | undefined;
}

const useStyles = makeStyles(() => ({
  root: {},
}));

function MainLayout(props: PropsWithChildren<Props>): JSX.Element {
  const classes = useStyles(props);
  const { children } = props;
  const {
    profileOptionsAnchor,
    setProfileOptionsAnchor,
    onClickProfileOption,
  } = useAppContext();

  const setNullProfileAnchor = (): void => setProfileOptionsAnchor(null);

  const renderProfileOption = (): JSX.Element => {
    if (!profileOptionsAnchor) return <Fragment />;

    return (
      <ProfileOptionsPopover
        anchorEl={profileOptionsAnchor}
        onClose={setNullProfileAnchor}
        onClick={onClickProfileOption}
        editProfileText={fm.editProfile}
        signOutText={fm.signOut}
      />
    );
  };

  return (
    <div className={classes.root} ref={props.anchorRef}>
      <Header
        ptasVariant="transparent"
        showDropdown
        showMenuIcon
        showSearchIcon
        defaultValue="en"
      />
      {children}
      {renderProfileOption()}
    </div>
  );
}

export default MainLayout;
