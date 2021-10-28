// TabPassword.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomPassword } from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { Box } from '@material-ui/core';
import * as fm from './formatText';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    width: 448,
    padding: theme.spacing(4, 0, 4, 0),
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
  },
  title: {
    fontSize: theme.ptas.typography.bodyBold.fontSize,
    fontWeight: theme.ptas.typography.bodyBold.fontWeight,
    lineHeight: '22px',
    paddingBottom: theme.spacing(2),
  },
  password: {
    height: '36px',
    paddingBottom: theme.spacing(2),
  },
}));

function TabPassword(): JSX.Element {
  const classes = useStyles();

  return (
    <Box className={classes.root}>
      <Box className={classes.title}>{fm.passwordReviewUpdate}</Box>
      <CustomPassword
        classes={{ root: classes.password }}
        label={fm.passwordLabel}
      />
    </Box>
  );
}

export default TabPassword;
