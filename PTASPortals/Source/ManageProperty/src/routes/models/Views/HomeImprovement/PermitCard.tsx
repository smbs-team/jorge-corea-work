// permitCard.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC } from 'react';
import { CustomCard, GenericWithStyles } from '@ptas/react-public-ui-library';
import {
  Box,
  createStyles,
  StyleRules,
  Theme,
  withStyles,
  WithStyles,
} from '@material-ui/core';
import utilService from 'services/utilService';
// import { GenericWithStyles } from '@ptas/react-ui-library';

interface Props extends WithStyles<typeof styles> {
  permitNumber: string | number;
  amount: number;
  date: number;
  description: string;
  onClick: (permitNumber: string | number) => void;
  isSelected: boolean;
}

const styles = (theme: Theme): StyleRules<string, Props> =>
  createStyles({
    card: {
      width: 234,
      height: 44,
      boxSizing: 'border-box',
      fontFamily: theme.ptas.typography.bodyFontFamily,
      // paddingTop: theme.spacing(3/8),
      padding: 0,
      border: '1px solid ' + theme.ptas.colors.theme.black,
      marginBottom: theme.spacing(1),
      backgroundColor: theme.ptas.colors.theme.white,
    },
    content: (props: Props) => ({
      display: 'flex',
      justifyContent: 'flex-start',
      flexDirection: 'column',
      padding: '3px 11px',
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
      lineHeight: '19px',
      cursor: 'pointer',
      background: props.isSelected
        ? theme.ptas.colors.utility.selectionLight
        : 'transparent',
    }),
    line1: {
      display: 'flex',
      justifyContent: 'space-between',
    },
  });

function PermitCard(props: Props): JSX.Element {
  const { classes } = props;

  return (
    <Box
      onClick={(): void => {
        props.onClick(props.permitNumber);
      }}
    >
      <CustomCard
        // variant="wrapper"
        classes={{
          root: classes.card,
          content: classes.content,
        }}
      >
        <Box className={classes.line1}>
          <label>#{props.permitNumber}</label>
          <label>•${utilService.numberWithCommas(props.amount)}</label>
          <label>•{new Date(props.date).toLocaleDateString()}</label>
        </Box>
        <Box>{props.description}</Box>
      </CustomCard>
    </Box>
  );
}

export default withStyles(styles)(PermitCard) as FC<GenericWithStyles<Props>>;
