// ProfileOptionsButton.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { makeStyles } from '@material-ui/core';
import { Theme } from '@material-ui/core/styles';
import { CustomTextButton } from '@ptas/react-public-ui-library';
import { useAppContext } from 'contexts/AppContext';
import useAuth from '../../auth/useAuth';

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
  const { contactProfile, showProfileOptions } = useAppContext();
  const { isAuthenticated } = useAuth();

  const renderButtonOptions = (): JSX.Element => {
    if (isAuthenticated && contactProfile) {
      return (
        <CustomTextButton
          variant={'text'}
          ptasVariant={'Text more'}
          onClick={(
            evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
          ): void => {
            showProfileOptions(evt.currentTarget);
          }}
          classes={{ root: classes.signInButton }}
        >
          {`${contactProfile?.firstName} ${contactProfile?.lastName}`}
        </CustomTextButton>
      );
    }

    return (
      <CustomTextButton
        variant={'text'}
        onClick={(): void => {
          console.log('Sign in button clicked');
        }}
        classes={{ root: classes.signInButton }}
      >
        Loading...
      </CustomTextButton>
    );
  };

  return renderButtonOptions();
}

export default ProfileOptionsButton;
