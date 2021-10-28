// currentUseCard.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { ReactNode } from 'react';
import { CustomCard } from '@ptas/react-public-ui-library';
import { Box, makeStyles, Theme } from '@material-ui/core';

interface Props {
  headerText: string | ReactNode;
  children: string | ReactNode;
}

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    maxWidth: 470,
    boxSizing: 'border-box',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    // paddingTop: theme.spacing(3/8),
    padding: 0,
    // border: '1px solid ' + theme.ptas.colors.theme.black,
    // marginBottom: theme.spacing(6),
    backgroundColor: theme.ptas.colors.theme.white,
  },
  content: {
    display: 'flex',
    justifyContent: 'flex-start',
    flexDirection: 'column',
    padding: theme.spacing(1),
    '&:last-child': {
      paddingBottom: '8px !important',
    },
  },
  header: {
    backgroundColor: theme.ptas.colors.theme.grayLightest,
    padding: theme.spacing(2, 2.75),
    marginBottom: theme.spacing(1),
    borderRadius: 9,
    fontSize: theme.ptas.typography.bodySmallBold.fontSize,
    fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
    lineHeight: '19px',
  },
  children: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
  },
}));

function CurrentUseCard(props: Props): JSX.Element {
  const classes = useStyles();

  return (
    <CustomCard
      // variant="wrapper"
      classes={{
        root: classes.card,
        content: classes.content,
      }}
    >
      <Box className={classes.header}>{props.headerText}</Box>
      <Box className={classes.children}>{props.children}</Box>
    </CustomCard>
  );
}

export default CurrentUseCard;
