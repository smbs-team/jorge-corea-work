// Supplies.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState } from 'react';
import { CustomTextField } from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import * as fmGeneral from '../../../../GeneralFormatMessage';
import { makeStyles, Theme } from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) => ({
  suppliesWrap: {
    width: '100%',
    maxWidth: 802,
    marginBottom: 39,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
  },
  title: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    marginBottom: 16,
    display: 'block',
  },
  description: {
    marginBottom: 20,
    display: 'block',
  },
  instruction: {
    display: 'block',
    '&::before': {
      content: `'-'`,
      marginRight: 8,
    },
  },
  item: {
    marginBottom: 20,
    '&:last-child': {
      marginBottom: 0,
    },
  },
  appliesDes: {
    marginBottom: 22,
  },
  currency: {
    maxWidth: 136,
  },
  currencySuppliesWrap: {
    width: '100%',
    maxWidth: 320,
    justifyContent: 'space-between',
    display: 'flex',
    alignItems: 'flex-end',
  },
  orText: {
    fontSize: theme.ptas.typography.body.fontSize,
  },
}));

function Supplies(): JSX.Element {
  const classes = useStyles();
  const [yearlyAverage, setYearlyAverage] = useState<string>('0');
  const [monthlyAverage, setMonthlyAverage] = useState<string>('0');

  const handleChangeYearlyAverage = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    const num = parseInt(e.target.value);

    if (num && num <= 0) return;

    setMonthlyAverage(`${num / 12}`);
  };

  const handleChangeMonthlyAverage = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    const num = parseInt(e.target.value);

    if (num && num <= 0) return;
    if (isNaN(num)) {
      return setYearlyAverage('0');
    }
    setYearlyAverage(`${num * 12}`);
  };

  return (
    <div className={classes.suppliesWrap}>
      <span className={classes.title}>{fm.suppliesMaterials}</span>
      <span className={classes.description}>{fm.provideTheCostOfItem}</span>
      <span>{fm.examples}</span>
      <span className={classes.instruction}>{fm.officeShopJanitorial}</span>
      <span className={classes.instruction}>
        {fm.brochuresPromotionalItems}
      </span>
      <span className={classes.instruction}>{fm.fuelSparePart}</span>
      <p className={classes.appliesDes}>{fm.researchCompanies}</p>
      <div className={classes.currencySuppliesWrap}>
        <CustomTextField
          ptasVariant="currency"
          label={fm.yearlyAverage}
          classes={{ root: classes.currency }}
          value={yearlyAverage}
          placeholder="0"
          onChange={handleChangeYearlyAverage}
        />
        <span className={classes.orText}>{fmGeneral.or}</span>
        <CustomTextField
          ptasVariant="currency"
          label={fm.monthlyAverage}
          classes={{ root: classes.currency }}
          placeholder="0"
          type="number"
          value={monthlyAverage}
          onChange={handleChangeMonthlyAverage}
        />
      </div>
    </div>
  );
}

export default Supplies;
