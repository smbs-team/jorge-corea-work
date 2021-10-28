/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ColDef } from 'ag-grid-community';
import {
  GrossLandGridRowData,
  LandCustomSearchExpressions,
  LandExceptionRuleType,
  LandGridData,
  LandVariableGridRowData,
} from 'services/map.typings';
import { ExpressionGridData } from '../ExpressionsGrid';

export const buildNonWaterFrontGridData = (
  rules: LandExceptionRuleType[],
  setState: (data: LandGridData[]) => void,
  setColDefs: (data: ColDef[]) => void
): void => {
  let counter = -1;
  let currentDesc = -1;
  const newRules: LandGridData[] = [];
  const gridDefs: ColDef[] = [];
  const rulesReverse = rules.reverse();
  for (const curr of rulesReverse) {
    if (
      currentDesc !==
      curr.customSearchExpressions[0].expressionExtensions?.ScheduleStep
    ) {
      currentDesc =
        curr.customSearchExpressions[0].expressionExtensions?.ScheduleStep ?? 0;
      counter++;
    }

    let value = '0';

    try {
      const stepValue = curr.customSearchExpressions[1]?.script;
      const numberValue = parseInt(stepValue);
      if (!isNaN(numberValue)) {
        value = `${parseFloat(curr.customSearchExpressions[1]?.script).toFixed(
          4
        )}`;
      } else {
        value = `${
          curr.customSearchExpressions[0]?.expressionExtensions?.StepValue ?? 0
        }`;
      }
    } catch (error) {
      console.log(`error`, error);
    }

    newRules[counter] = {
      ...newRules[counter],
      // from: `${curr.customSearchExpressions[0].expressionExtensions?.scheduleMin}`,
      to: `${curr.customSearchExpressions[0].expressionExtensions?.ScheduleStep}`,
      [`${
        curr.customSearchExpressions[0].expressionExtensions?.StepFilter
          ? curr.customSearchExpressions[0].expressionExtensions?.StepFilter
          : 'Baseline'
      }`]: value,
    };
  }
  if (newRules.length) {
    const keyNames = Object.keys(newRules[0]);
    keyNames.forEach((key) => {
      gridDefs.push({
        headerName: key,
        field: `${key}`,
        flex: 1,
        editable: !'to'.includes(key),
      });
    });
    setColDefs(gridDefs);
    setState(newRules.reverse() as LandGridData[]);
  }
};

export const buildWaterFrontGridData = (
  rules: LandExceptionRuleType[],
  setState: (data: LandGridData[]) => void,
  setColDefs: (data: ColDef[]) => void
): number => {
  let isAdded = false;
  let counter = -1;
  let currentDesc = -1;
  const newRules: LandGridData[] = [];
  const gridDefs: ColDef[] = [];
  const rulesReverse = rules.reverse();
  for (const curr of rulesReverse) {
    if (
      currentDesc !==
      curr.customSearchExpressions[0].expressionExtensions?.ScheduleStep
    ) {
      currentDesc =
        curr.customSearchExpressions[0].expressionExtensions?.ScheduleStep ?? 0;
      counter++;
    }
    if (counter === 0) {
      isAdded =
        curr.customSearchExpressions.find(
          (cs: LandCustomSearchExpressions) => cs.expressionExtensions
        )?.expressionExtensions?.addToNonWaterfrontValue || false;
    }

    let value = 0;
    if (
      !isNaN(
        parseInt(
          `${curr.customSearchExpressions[0]?.expressionExtensions?.StepValue}` ||
            '0'
        )
      )
    )
      value = parseInt(
        `${curr.customSearchExpressions[0]?.expressionExtensions?.StepValue}` ||
          '0'
      );

    newRules[counter] = {
      ...newRules[counter],
      // from: `${curr.customSearchExpressions[0].expressionExtensions?.scheduleMin}`,
      to: `${curr.customSearchExpressions[0].expressionExtensions?.ScheduleStep}`,
      [`${
        curr.customSearchExpressions[0].expressionExtensions?.StepFilter
          ? curr.customSearchExpressions[0].expressionExtensions?.StepFilter
          : 'Baseline'
      }`]: value,
    };
  }

  const getHeaderName = (key: string): string => {
    if (key === 'to') return 'Front Feet';
    if (key === 'WaterfrontValue') return '';
    return key;
  };

  if (newRules.length) {
    const keyNames = Object.keys(newRules[0]);
    keyNames.forEach((key) => {
      gridDefs.push({
        headerName: getHeaderName(key),
        field: `${key}`,
        flex: 1,
        editable: !'to'.includes(key),
      });
    });
    setState(newRules.reverse() as LandGridData[]);
    setColDefs(gridDefs);
  }
  return isAdded ? 1 : 2;
};

export const buildGrossGridData = (
  rules: LandExceptionRuleType[],
  setState: (data: GrossLandGridRowData[]) => void
): void => {
  const grossLandExpressions = rules?.map<GrossLandGridRowData>(
    (exp: LandExceptionRuleType) => {
      const data = exp.customSearchExpressions.find(
        (cs: LandCustomSearchExpressions) => cs.expressionExtensions
      );
      return {
        method: data?.expressionExtensions?.Method || '',
        filter: data?.script || '',
        expression:
          exp.customSearchExpressions.find(
            (cs: LandCustomSearchExpressions) =>
              cs.expressionRole === 'CalculatedColumn'
          )?.script || '',
      };
    }
  );
  if (grossLandExpressions?.length) {
    setState(grossLandExpressions.reverse());
  }
};

export const buildPositiveAdjustment = (
  rules: LandExceptionRuleType[],
  setState: (data: LandVariableGridRowData[]) => void
): void => {
  const gridData = rules?.map<LandVariableGridRowData>(
    (exp: LandExceptionRuleType) => {
      const data = exp.customSearchExpressions.find(
        (cs: LandCustomSearchExpressions) => cs.expressionExtensions
        //eslint-disable-next-line
      )?.expressionExtensions as any;
      return {
        // characteristicType: data?.characteristicType,
        // description: data?.description,
        // linearft: data?.linearft,
        // maxadjmoney: data?.maxadjmoney,
        // minadjmoney: data?.minadjmoney,
        // maxadjpercentaje: data?.maxadjpercentaje,
        // minadjpercentaje: data?.minadjpercentaje,
        // sqft: data?.sqft,
        // value: data?.value,
        ...data,
      };
    }
  );
  if (gridData?.length) {
    setState(gridData);
  }
};

export const buildNonWaterFrontExpressionGridData = (
  rules: LandExceptionRuleType[],
  setState: (data: ExpressionGridData[]) => void
): void => {
  const gridData = rules?.map<ExpressionGridData>(
    (exp: LandExceptionRuleType) => {
      const data = exp.customSearchExpressions;
      return {
        filter: data[0].script,
        expression: data[1].script || '',
        note: data[0].expressionExtensions?.Note || '',
      };
    }
  );
  if (gridData?.length) {
    setState(gridData);
  }
};
