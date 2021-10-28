// Assets.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomTextField } from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import useStyles from './styles';
import useManageData from '../useManageData';

function Assets(): JSX.Element {
  const classes = useStyles();
  const { quickCollect, setQuickCollect } = useManageData('SOLD');

  const handleChangeCustomTextField = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    if (!e.currentTarget) return;
    const { name, value } = e.currentTarget;
    const parsedValue = parseInt(value || '0');
    if (isNaN(parsedValue)) return;
    setQuickCollect(prev => {
      return {
        ...prev,
        [name]: parsedValue,
      };
    });
  };

  return (
    <div className={classes.assetsTab}>
      <div className={classes.currencyWrap}>
        <CustomTextField
          ptasVariant="currency"
          label={fm.totalSalesPrice}
          classes={{ root: classes.currency }}
          placeholder="0"
          name="totalSalesPrice"
          value={quickCollect?.totalSalesPrice ?? 0}
          onChange={handleChangeCustomTextField}
          onChangeDelay={800}
        />
        <CustomTextField
          ptasVariant="currency"
          label={fm.equipment}
          classes={{ root: classes.currency }}
          placeholder="0"
          name="equipment"
          value={quickCollect?.equipment ?? 0}
          onChange={handleChangeCustomTextField}
          onChangeDelay={800}
        />
        <CustomTextField
          ptasVariant="currency"
          label={fm.improvements}
          classes={{ root: classes.currency }}
          placeholder="0"
          name="leaseHoldImprovements"
          value={quickCollect?.leaseHoldImprovements ?? 0}
          onChange={handleChangeCustomTextField}
          onChangeDelay={800}
        />
        <CustomTextField
          ptasVariant="currency"
          label={fm.intangibles}
          classes={{ root: classes.currency }}
          placeholder="0"
          name="intangibles"
          value={quickCollect?.intangibles ?? 0}
          onChange={handleChangeCustomTextField}
          onChangeDelay={800}
        />
        <CustomTextField
          ptasVariant="currency"
          label={fm.other}
          classes={{ root: classes.currency }}
          placeholder="0"
          name="other"
          value={quickCollect?.other ?? 0}
          onChange={handleChangeCustomTextField}
          onChangeDelay={800}
        />
      </div>
      <span className={classes.description}>{fm.theseFieldAreOptional}</span>
    </div>
  );
}

export default Assets;
