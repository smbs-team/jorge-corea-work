// ProfileOptionsButton.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext } from 'react';
import { makeStyles } from '@material-ui/core';
import { Theme } from '@material-ui/core/styles';
import { CustomTextButton } from '@ptas/react-public-ui-library';
import { AppContext } from 'contexts/AppContext';
import * as fm from '../../GeneralFormatMessage';
import { useHistory } from 'react-router';

/**
 * Component styles
 */
const useStyles = makeStyles((theme: Theme) => ({
  root: {},
  signInButton: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
  },
}));

/**
 * ProfileOptionsButton
 *
 * @returns A JSX element
 */
function ProfileOptionsButton(): JSX.Element {
  const classes = useStyles();
  const history = useHistory();
  const {
    portalContact,
    isValidPortalContact,
    showProfileOptions,
  } = useContext(AppContext);

  const goToSignIn = (): void => {
    history.push('/signin');
  };

  return isValidPortalContact() ? (
    <CustomTextButton
      variant={'text'}
      ptasVariant={'Text more'}
      onClick={(evt: React.MouseEvent<HTMLButtonElement, MouseEvent>): void => {
        showProfileOptions(evt.currentTarget);
      }}
      classes={{ root: classes.signInButton }}
    >
      {`${portalContact?.firstName} ${portalContact?.lastName}`}
    </CustomTextButton>
  ) : (
    <CustomTextButton
      variant={'text'}
      onClick={goToSignIn}
      classes={{ root: classes.signInButton }}
    >
      {fm.signIn}
    </CustomTextButton>
  );
}

export default ProfileOptionsButton;
