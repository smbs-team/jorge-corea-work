// Closure.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomDatePicker } from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import useStyles from './styles';
import useManageData from '../useManageData';
import { formatDate } from 'utils/date';

function Closure(): JSX.Element {
  const classes = useStyles();
  const { quickCollect, setQuickCollect } = useManageData('CLOSED');

  const onChangeDate = (date: Date | null): void => {
    if (!date) return;
    const dateFormatted = formatDate(date);
    setQuickCollect(prev => {
      return {
        ...prev,
        closingDate: dateFormatted,
      };
    });
  };

  return (
    <div className={classes.closureRoot}>
      <CustomDatePicker
        value={quickCollect?.closingDate}
        onChange={onChangeDate}
        label={fm.closeDate}
        classes={{ root: classes.closeDate }}
        placeholder="mm/dd/yyyy"
      />
    </div>
  );
}

export default Closure;
