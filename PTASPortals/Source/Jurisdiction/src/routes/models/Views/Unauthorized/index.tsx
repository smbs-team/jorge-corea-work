// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React from 'react';
import {
  CustomCard,
  CustomButton,
  Banner,
} from '@ptas/react-public-ui-library';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as generalFm from '../../../../GeneralFormatMessage';
import { Theme } from '@material-ui/core/styles';
import { makeStyles } from '@material-ui/core';
import useAuth from '../../../../auth/useAuth';

/**
 * Component styles
 */
const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    maxWidth: 640,
    paddingTop: 16,
    marginLeft: 'auto',
    marginRight: 'auto',
    boxSizing: 'border-box',
    marginTop: 22,
  },
  returnSigningButton: {
    maxWidth: 190,
    margin: '0 auto',
    display: 'block',
    width: '100%',
    paddingTop: 4,
  },
}));

const Unauthorized = (): JSX.Element => {
  const classes = useStyles();
  const { signOut } = useAuth();

  const handleReturn = (): void => {
    signOut();
    // history.replace('/intro');
  };

  return (
    <MainLayout>
      <Banner message={generalFm.notAuthorized} open={true} />
      <CustomCard
        variant="wrapper"
        classes={{
          rootWrap: classes.card,
        }}
      >
        <CustomButton
          classes={{ root: classes.returnSigningButton }}
          onClick={handleReturn}
        >
          {generalFm.returnToSignIn}
        </CustomButton>
      </CustomCard>
    </MainLayout>
  );
};

export default Unauthorized;
