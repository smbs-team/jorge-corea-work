// Home.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import {
  CustomButton,
  CustomCard,
  CustomTextButton,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import { Link, useHistory } from 'react-router-dom';
import * as fm from './formatText';
import * as generalFm from '../../../../GeneralFormatMessage';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    display: 'block',
    boxSizing: 'border-box',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    paddingTop: 32,
    height: 530,
    borderRadius: 0,
    marginBottom: 15,

    [theme.breakpoints.up('sm')]: {
      width: 486,
      borderRadius: 24,
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
    height: 330,
    marginLeft: 10,

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
    marginBottom: 27,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    margin: 0,

    [theme.breakpoints.up('sm')]: {
      fontSize: theme.ptas.typography.h1.fontSize,
    },
  },
  list: {
    margin: 0,
    marginBottom: 7,
    fontSize: theme.ptas.typography.body.fontSize,
    display: 'block',
    padding: 0,
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
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    marginBottom: 8,
    width: '100%',
    maxWidth: 320,
  },
  textHelp: {
    fontSize: theme.ptas.typography.body.fontSize,
    fontWeight: theme.ptas.typography.bodyBold.fontWeight,
    marginBottom: 7,
  },
  grayTitle: {
    fontSize: theme.ptas.typography.h4.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontFamily: theme.ptas.typography.titleFontFamily,
    margin: '0 0 10px 0',
  },
  subtitle: {
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.body.fontSize,

    display: 'block',
  },
  link: {
    display: 'block',
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    color: theme.ptas.colors.theme.gray,
    marginBottom: 4,

    '&:hover': {
      color: theme.ptas.colors.theme.gray,
    },
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
  lineHeight: {
    lineHeight: '19px',
  },
  button: {
    width: 176,
    borderRadius: 24,
    fontSize: 18,
    height: 48,
    fontWeight: theme.ptas.typography.buttonSmall.fontWeight,
  },
  item: {
    margin: 0,
    display: 'block',
    fontSize: '1rem',
    lineHeight: '6.5px',
    marginBottom: '32px',
    fontStyle: 'normal',
    fontWeight: 400,
    color: 'black',
    '&::before': {
      content: `'•'`,
      marginRight: '3px',
    },
  },
}));

function Home(): JSX.Element {
  const classes = useStyles();
  const history = useHistory();

  const handleClick = (): void => {
    history.push('/instruction');
  };

  const handleSignIn = (): void => {
    history.push('/home-manage-property/');
  };

  const renderList = (): JSX.Element => {
    return (
      <ul className={classes.list}>
        <li className={classes.item}>{fm.itemList1}</li>
        <li className={classes.item}>{fm.itemList2}</li>
        <li className={classes.item}>{fm.itemList3}</li>
        <li className={classes.item}>{fm.itemList4}</li>
        <li className={classes.item}>{fm.itemList5}</li>
      </ul>
    );
  };

  const renderLinks = (): JSX.Element => {
    return (
      <Fragment>
        <Link to="/" className={classes.link}>
          {fm.seniorExemption}
        </Link>
        <Link to="/home-improvement" className={classes.link}>
          {fm.homeImprovement}
        </Link>
        <Link to="/current-use" className={classes.link}>
          {fm.currentUse}
        </Link>
        <Link to="/home-destroyed-property/" className={classes.link}>
          {fm.destroyProperty}
        </Link>
      </Fragment>
    );
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
          {renderList()}
          <CustomButton
            classes={{
              root: classes.button,
            }}
            onClick={handleSignIn}
          >
            {fm.signIn}
          </CustomButton>
          <span className={classes.indication}>{fm.indication}</span>
          <span className={classes.border}></span>
          <span className={classes.textHelp}>{generalFm.help}</span>
          <CustomTextButton onClick={handleClick}>
            {generalFm.instruction}
          </CustomTextButton>
        </CustomCard>
        <CustomCard
          variant="wrapper"
          classes={{
            rootWrap: classes.smallCard,
          }}
        >
          <h2 className={classes.grayTitle}>{fm.titleNew}</h2>
          <span className={classes.subtitle}>{fm.newAccount}</span>
          <span
            className={`${classes.accountIndication} ${classes.lineHeight}`}
          >
            {fm.newAccountIndication}
          </span>
          <span className={classes.subtitle}>{fm.OnlineFilingLinks}</span>
          {renderLinks()}
        </CustomCard>
      </div>
    </MainLayout>
  );
}

export default Home;
