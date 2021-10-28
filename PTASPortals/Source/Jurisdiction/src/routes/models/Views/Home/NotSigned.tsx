// Home.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomButton, CustomCard } from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import { useHistory } from 'react-router-dom';
import * as fm from './formatText';
import { useAppContext } from 'contexts/AppContext';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    display: 'block',
    boxSizing: 'border-box',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    paddingTop: 32,
    height: 255,
    borderRadius: 0,
    margin: '15px auto 0 auto',
    paddingLeft: 4,
    paddingRight: 4,
    marginTop: 0,

    [theme.breakpoints.up('sm')]: {
      width: 564,
      borderRadius: 24,
      marginTop: 40,
    },
  },
  contentWrap: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
  },
  title: {
    fontSize: theme.ptas.typography.h6.fontSize,
    fontFamily: theme.ptas.typography.titleFontFamily,
    marginBottom: 31,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    margin: 0,
    textAlign: 'center',

    [theme.breakpoints.up('sm')]: {
      fontSize: theme.ptas.typography.h1.fontSize,
    },

    [theme.breakpoints.up('minimum')]: {
      fontSize: theme.ptas.typography.h5.fontSize,
    },
  },
  subtitle: {
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.body.fontSize,
    display: 'block',
    width: '100%',
    marginBottom: 32,
    maxWidth: 230,
    textAlign: 'center',

    [theme.breakpoints.up('sm')]: {
      maxWidth: 270,
    },
  },
  button: {
    width: 176,
    borderRadius: 24,
    fontSize: 18,
    height: 48,
    fontWeight: theme.ptas.typography.buttonSmall.fontWeight,
  },
}));

function Home(): JSX.Element {
  const classes = useStyles();
  const history = useHistory();
  const { contactProfile } = useAppContext();

  const handleSignIn = (): void => {
    let route = '/levy';

    if (contactProfile?.type === 'jurisdiction') {
      route = '/permits';
    } else if (contactProfile?.type === 'taxDistrict') {
      route = '/levy';
    }

    history.push(route);
  };

  return (
    <MainLayout>
      <CustomCard
        variant="wrapper"
        classes={{
          rootWrap: classes.card,
          wrapperContent: classes.contentWrap,
        }}
      >
        <h2 className={classes.title}>{fm.jurisdictionAccess}</h2>
        <span className={classes.subtitle}>{fm.jurisdictionAccessDesc}</span>
        <CustomButton
          classes={{
            root: classes.button,
          }}
          onClick={handleSignIn}
        >
          {fm.signIn}
        </CustomButton>
      </CustomCard>
    </MainLayout>
  );
}

export default Home;
