// Home.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomButton, CustomCard } from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core/styles';
import MainLayout from '../../../../components/Layouts/MainLayout';
import { useHistory } from 'react-router-dom';
import * as fm from './formatText';
import HelpSection from 'components/HelpSection';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    display: 'block',
    boxSizing: 'border-box',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    paddingTop: 32,
    paddingBottom: 22,
    borderRadius: 0,
    marginBottom: 15,
    height: 380,

    [theme.breakpoints.up('sm')]: {
      width: 591,
      borderRadius: 24,
      height: 343,
    },
  },
  smallCard: {
    width: 240,
    display: 'none',
    boxSizing: 'border-box',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    color: theme.ptas.colors.theme.gray,
    textAlign: 'center',
    paddingTop: 32,
    height: 428,
    marginLeft: 8,

    [theme.breakpoints.up('sm')]: {
      display: 'block',
    },
  },
  contentWrap: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
  },
  title: {
    fontSize: theme.ptas.typography.h5.fontSize,
    fontFamily: theme.ptas.typography.titleFontFamily,
    marginBottom: 31,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    margin: 0,
    textAlign: 'center',

    [theme.breakpoints.up('sm')]: {
      fontSize: theme.ptas.typography.h2.fontSize,
    },
  },
  accountIndication: {
    marginLeft: 'auto',
    marginRight: 'auto',
    marginBottom: 30,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    display: 'block',
    maxWidth: 157,
  },
  indication: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    marginTop: 8,
    marginBottom: 32,
    width: '100%',
    maxWidth: 228,
    textAlign: 'center',
    color: theme.ptas.colors.theme.black,
  },
  grayTitle: {
    fontSize: theme.ptas.typography.h4.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontFamily: theme.ptas.typography.titleFontFamily,
    margin: '0 0 21px 0',
  },
  subtitle: {
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    fontFamily: theme.ptas.typography.titleFontFamily,
    display: 'block',
    marginBottom: 32,
    textAlign: 'center',
  },
  cardWrapper: {
    width: '100%',
    maxWidth: '100%',
    display: 'flex',
    justifyContent: 'center',
    flexWrap: 'wrap',
    marginTop: 0,

    [theme.breakpoints.up('sm')]: {
      marginTop: 40,
      width: '100%',
      maxWidth: 967,
      marginLeft: 'auto',
      marginRight: 'auto',
      flexWrap: 'nowrap',
      display: 'flex',
      justifyContent: 'flex-end',
    },
  },
  button: {
    width: 176,
    borderRadius: 24,
    fontSize: 18,
    height: 48,
    fontWeight: theme.ptas.typography.buttonSmall.fontWeight,
    marginBottom: 32,
  },
  paragraphWrapper: {
    marginBottom: 31,
    '&:last-child': {
      marginBottom: 0,
    },
    fontFamily: theme.ptas.typography.bodyFontFamily,
    textAlign: 'center',
  },
  whatsNewTitle: {
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.body.fontSize,
    display: 'block',
    color: 'rgba(0, 0, 0, 0.54)',
    width: 142,
    margin: '0 auto',
  },
  whatsNewParagraph: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    display: 'block',
    '& > a': {
      color: theme.ptas.colors.theme.accent,
    },
    width: 168,
    margin: '0 auto',
  },
  newAccountTitle: {
    width: 155,
  },
}));

function Home(): JSX.Element {
  const classes = useStyles();
  const history = useHistory();

  const handleSignIn = (): void => {
    history.push('/home');
  };

  return (
    <MainLayout>
      <div className={classes.cardWrapper}>
        <CustomCard
          variant="wrapper"
          classes={{
            rootWrap: classes.card,
            wrapperContent: classes.contentWrap,
          }}
        >
          <h2 className={classes.title}>{fm.title}</h2>
          <span className={classes.subtitle}>{fm.description}</span>
          <CustomButton
            classes={{
              root: classes.button,
            }}
            onClick={handleSignIn}
          >
            {fm.signIn}
          </CustomButton>
          <HelpSection hideSeparator />
        </CustomCard>
        <CustomCard
          variant="wrapper"
          classes={{
            rootWrap: classes.smallCard,
          }}
        >
          <h2 className={classes.grayTitle}>{fm.whatsNew}</h2>
          <div className={classes.paragraphWrapper}>
            <span
              className={`${classes.whatsNewTitle} ${classes.newAccountTitle}`}
            >
              {fm.newAccount}
            </span>
            <span className={classes.whatsNewParagraph}>
              {fm.newAccountParagraph}
            </span>
          </div>
          <div className={classes.paragraphWrapper}>
            <span className={classes.whatsNewTitle}>{fm.easierEntry}</span>
            <span className={classes.whatsNewParagraph}>
              {fm.easierEntryParagraph}
            </span>
          </div>
          <div className={classes.paragraphWrapper}>
            <span className={classes.whatsNewTitle}>{fm.unlistedBusiness}</span>
            <span className={classes.whatsNewParagraph}>
              {fm.unlistedBusinessParagraph}
            </span>
          </div>
        </CustomCard>
      </div>
    </MainLayout>
  );
}

export default Home;
