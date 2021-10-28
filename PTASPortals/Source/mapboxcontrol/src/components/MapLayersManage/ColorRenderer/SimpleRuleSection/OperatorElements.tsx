/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext } from 'react';
import {
  Box,
  createStyles,
  StyleRules,
  Theme,
  withStyles,
  WithStyles,
} from '@material-ui/core';
import {
  SimpleDropDown as PtasDropDown,
  CustomTextField,
  CustomDatePicker,
} from '@ptas/react-ui-library';
import { UseSimpleRule } from './useSimpleRule';
import { RuleContext } from '../Context';
import { utilService } from 'services/common';
import {
  RendererSimpleRule,
  NumericOperatorId,
  StringOperatorId,
} from 'services/map/model';

const styles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      display: 'flex',
      marginTop: theme.spacing(1.25),
      width: '100%',
      alignItems: 'center',
      height: 60,
    },
    conditionDropDown: {
      width: '140px',
      marginRight: theme.spacing(1.25),
    },
    conditionTextField: {
      maxWidth: '100%',
    },
    dropDown: {
      width: '100%',
    },
  });

const FieldValueElement = (
  props: UseSimpleRule & WithStyles<typeof styles>
): JSX.Element | null => {
  const { datasetField, rule } = useContext(RuleContext);
  const {
    datasetFieldValues,
    classes,
    conditionValue,
    setConditionValue,
  } = props;

  if (!(rule instanceof RendererSimpleRule)) return null;
  if (datasetField?.canBeUsedAsLookup)
    return (
      <PtasDropDown
        classes={{ root: classes.dropDown }}
        items={datasetFieldValues}
        onSelected={(x): void => {
          setConditionValue(x.value);
        }}
        label="Value"
        titleTop
        value={conditionValue}
      />
    );

  if (datasetField?.columnType === 'date')
    return (
      <CustomDatePicker
        onChange={(date): void => {
          setConditionValue(date ? '' + date.getTime() : '');
        }}
        label="Value"
        value={utilService.unixTimeToDate(conditionValue.toString())}
      />
    );

  return (
    <CustomTextField
      className={classes.conditionTextField}
      label="Value"
      onChange={(e): void => {
        setConditionValue((e.target as HTMLInputElement).value);
      }}
      value={conditionValue}
    />
  );
};

function OperatorElements(
  props: UseSimpleRule & WithStyles<typeof styles>
): JSX.Element {
  const {
    classes,
    operatorsItems,
    setConditionOperator,
    conditionOperator,
  } = props;

  return (
    <Box className={classes.root}>
      <Box className={classes.conditionDropDown}>
        <PtasDropDown
          classes={{ root: classes.dropDown }}
          items={operatorsItems}
          onSelected={(x): void => {
            setConditionOperator(
              x.value as NumericOperatorId | StringOperatorId
            );
          }}
          label="Operator"
          value={operatorsItems.length ? conditionOperator : ''}
        />
      </Box>
      <Box className={classes.conditionDropDown}>
        <FieldValueElement {...props} />
      </Box>
    </Box>
  );
}

export default withStyles(styles)(OperatorElements);
