// Instruction.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomCard } from '@ptas/react-public-ui-library';
import { IconButton, makeStyles, Theme } from '@material-ui/core';
import CloseIcon from '@material-ui/icons/Close';
import MainLayout from '../../../../components/Layouts/MainLayout';
import { Link, useHistory } from 'react-router-dom';
import * as fm from './formatText';

const useStyles = makeStyles((theme: Theme) => ({
  root: {},
  card: {
    width: '100%',
    maxWidth: 1260,
    display: 'flex',
    justifyContent: 'center',
    position: 'relative',
    boxSizing: 'border-box',
    paddingTop: 24,
    marginLeft: 'auto',
    marginRight: 'auto',
    borderRadius: 0,

    [theme.breakpoints.up('sm')]: {
      borderRadius: 24,
      marginTop: 8,
    },
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    marginBottom: 8,
    display: 'block',
    margin: '0 auto',
  },
  title: {
    fontSize: theme.ptas.typography.h5.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontFamily: theme.ptas.typography.titleFontFamily,
    margin: '0 0 16px 0',
    textAlign: 'center',
    [theme.breakpoints.up('sm')]: {
      fontSize: theme.ptas.typography.h3.fontSize,
    },
  },
  closeButton: {
    position: 'absolute',
    [theme.breakpoints.up('sm')]: {
      right: 3,
      top: 3,
    },
    right: -3,
    top: -3,
  },
  closeIcon: {
    color: theme.ptas.colors.theme.black,
    fontSize: 34,
  },
  textWrapper: {
    width: '100%',
    maxWidth: 970,
    margin: '0 auto',
    padding: '21px 0px 42px',
  },
  heading: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontSize: theme.ptas.typography.h6.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    marginBottom: 6,
    marginTop: 0,
  },
  wrapper: {
    marginBottom: 32,
  },
  content: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    fontWeight: theme.ptas.typography.bodyLarge.fontWeight,

    '& > ul': {
      marginTop: 8,
      marginBottom: 8,
      listStyle: 'none',
      paddingLeft: 20,

      '& li > a': {
        color: theme.ptas.colors.theme.accent,
      },

      '& > li': {
        color: theme.ptas.colors.theme.black,
        fontSize: theme.ptas.typography.bodyLarge.fontSize,

        '&::before': {
          content: `'•'`,
          marginRight: 8,
        },
      },
    },

    '& > ul > li > ul': {
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: theme.ptas.typography.body.fontWeight,
      listStyle: 'none',
      paddingLeft: 20,
      marginTop: 7,
      marginBottom: 7,

      '& > li': {
        color: theme.ptas.colors.theme.black,
        fontSize: theme.ptas.typography.bodySmall.fontSize,
        margin: '8px 0',
        '&::before': {
          content: `'•'`,
          marginRight: 8,
        },
      },
    },
  },
  description: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    fontWeight: theme.ptas.typography.bodyLarge.fontWeight,
    marginTop: 32,
    '& > a': {
      color: theme.ptas.colors.theme.accent,
    },
  },
}));

function Instruction(): JSX.Element {
  const classes = useStyles();
  const history = useHistory();

  const handleClick = (): void => {
    history.goBack();
  };

  return (
    <MainLayout>
      <CustomCard
        variant="wrapper"
        classes={{
          rootWrap: classes.card,
        }}
      >
        <IconButton className={classes.closeButton} onClick={handleClick}>
          <CloseIcon className={classes.closeIcon} />
        </IconButton>

        <h2 className={classes.title}>{fm.title}</h2>
        <span className={classes.border}></span>
        <div className={classes.textWrapper}>
          <div className={classes.wrapper}>
            <h5 className={classes.heading}>{fm.signedInTitle}</h5>
            <div className={classes.content}>{fm.signingInContent}</div>
          </div>
          <div className={classes.wrapper}>
            <h5 className={classes.heading}>{fm.addPropertyTitle}</h5>
            <div className={classes.content}>{fm.addPropertyContent}</div>
          </div>
          <div className={classes.wrapper}>
            <h5 className={classes.heading}>{fm.uptMailingAddressTitle}</h5>
            <div className={classes.content}>{fm.uptMailingAddressDesc}</div>
          </div>
          <div className={classes.wrapper}>
            <h5 className={classes.heading}>{fm.paperlessTitle}</h5>
            <div className={classes.content}>{fm.paperlessContent}</div>
          </div>
          <div className={classes.wrapper}>
            <h5 className={classes.heading}>{fm.exemptionsTitle}</h5>
            <div className={classes.content}>{fm.exemptionContent}</div>
          </div>
          <div className={classes.wrapper}>
            <h5 className={classes.heading}>{fm.requestAdjustmentsTitle}</h5>
            <div className={classes.content}>
              {fm.requestAdjustmentsContent}
            </div>
          </div>
          <div className={classes.wrapper}>
            <h5 className={classes.heading}>{fm.bussinessPersonalProperty}</h5>
            <div className={classes.content}>
              {fm.bussinessPersonalPropertyContent}
            </div>
          </div>
          <span className={classes.description}>
            If you need more help, you can <Link to="/">contact us</Link>
          </span>
        </div>
      </CustomCard>
    </MainLayout>
  );
}

export default Instruction;
