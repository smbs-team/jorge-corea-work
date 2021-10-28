import { ExpressionGridData } from './../ExpressionsGrid';
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AxiosLoader } from './../../../../../../services/AxiosLoader';
import {
  IdOnly,
  // LandExceptionTypes,
  AdjustmentParam,
  SheetType,
  LandVariableGridRowData,
} from 'services/map.typings';
import { deleteDatasetPostProcess } from 'services/common';

interface LandRegressionModalType {
  to: string;
  default: string;
}

interface WaterFrontType {
  datasetId: string;
  rules: LandRegressionModalType[];
  step: number;
}

interface PredictedEcuation {
  predictedEquation: string;
}

export const getPredictedEcuation = (
  postProcessId: number,
  precision = 4
): Promise<PredictedEcuation | null> => {
  const ad2 = new AxiosLoader<PredictedEcuation, {}>();
  return ad2.GetInfo(
    `CustomSearches/GetRScriptPredictedEquation/${postProcessId}?precision=${precision}`,
    {}
  );
};

export const createSetupLandModel = (
  datasetId: string,
  secondaryDatasets: string[]
  //eslint-disable-next-line
): Promise<any | null> => {
  //eslint-disable-next-line
  const ad2 = new AxiosLoader<any, {}>();
  return ad2.PutInfo(
    `CustomSearches/SetupLandModel/${datasetId}`,
    { secondaryDatasets },
    {}
  );
};

export const saveAdjustmentPostProcess =
  (payload: {}): Promise<IdOnly | null> => {
    const ad2 = new AxiosLoader<IdOnly, {}>();
    return ad2.PutInfo(
      `CustomSearches/ImportLandScheduleAdjustmentsPostProcess`,
      payload,
      {}
    );
  };

export const runNonWaterfrontSetup = (payload: {}): Promise<IdOnly | null> => {
  const ad2 = new AxiosLoader<IdOnly, {}>();
  return ad2.PutInfo(
    `CustomSearches/ImportNonWaterfrontSchedulePostProcess`,
    payload,
    {}
  );
};

export const runWaterfrontSetup = (payload: {}): Promise<IdOnly | null> => {
  const ad2 = new AxiosLoader<IdOnly, {}>();
  return ad2.PutInfo(
    `CustomSearches/ImportWaterfrontSchedulePostProcess`,
    payload,
    {}
  );
};

// //eslint-disable-next-line
// export const saveAdjustmentPostProcess = (
//   payload: any
// ): Promise<IdOnly | null> => {
//   const ad2 = new AxiosLoader<IdOnly, {}>();
//   return ad2.PutInfo(
//     `CustomSearches/ImportLandScheduleAdjustmentsPostProcess`,
//     payload,
//     {}
//   );
// };

// export const runNonWaterfrontSetup = (payload: {}): Promise<IdOnly | null> => {
//   const ad2 = new AxiosLoader<IdOnly, {}>();
//   return ad2.PutInfo(
//     `CustomSearches/ImportNonWaterfrontSchedulePostProcess`,
//     payload,
//     {}
//   );
// };

// export const runWaterfrontSetup = (payload: {}): Promise<IdOnly | null> => {
//   const ad2 = new AxiosLoader<IdOnly, {}>();
//   return ad2.PutInfo(
//     `CustomSearches/ImportWaterfrontSchedulePostProcess`,
//     payload,
//     {}
//   );
// };

export const executeDatasetPostProcess = (
  datasetId: string,
  postProcessId: number | string
): Promise<IdOnly | null> => {
  const ad2 = new AxiosLoader<IdOnly, {}>();
  return ad2.PutInfo(
    `CustomSearches/ExecuteDatasetPostProcess/${datasetId}/${postProcessId}`,
    [
      {
        Id: 0,
        Name: '',
        Value: '',
      },
    ],
    {}
  );
};

// payload is of type any since the internal data changes depending on the type of postprocess
const saveExceptionPostProcess = async (
  // eslint-disable-next-line
  payload: any
): Promise<IdOnly | null> => {
  const al1 = new AxiosLoader<IdOnly, {}>();
  return al1.PutInfo('CustomSearches/ImportExceptionPostProcess', payload, {});
};

export const applyRegressionToSchedule = async (
  regressionId: number,
  exceptionId: number
): Promise<IdOnly | null> => {
  const al1 = new AxiosLoader<IdOnly, {}>();
  return al1.PutInfo(
    `CustomSearches/ApplyLandRegressionToSchedule/${regressionId}/${exceptionId}`,
    {},
    {}
  );
};

export const nonWaterFrontExpressionService = async (
  expressionData: ExpressionGridData[],
  datasetId: string,
  setPostProcessId: Function,
  secondaryDatasets: string[],
  postProcessId?: number
): Promise<IdOnly | null> => {
  const value = 'NewLandValue';
  const expreMessage = 'NonwaterfrontExpressions';
  //eslint-disable-next-line
  const regex = new RegExp("\'", 'g');
  const rules = expressionData.map((expre) => {
    return {
      description: `Plat Value: ${expre.filter?.trim()}`,
      customSearchExpressions: [
        {
          expressionType: 'TSQL',
          expressionRole: 'FilterExpression',
          script: expre.filter?.trim(),
          columnName: value,
          expressionExtensions: {
            Note: expre.note?.trim() || '',
            traceMessage: `'${expreMessage}: ${expre.filter.replaceAll(
              regex,
              `"`
            )} =>' {ColumnValue}`,
          },
        },
        {
          expressionType: 'TSQL',
          expressionRole: 'CalculatedColumn',
          script: expre.expression?.trim(),
          columnName: value,
        },
      ],
    };
  });

  const data = {
    datasetId: datasetId,
    postProcessName: 'Non waterfront expressions',
    postProcessRole: 'NonWaterfrontExpressions',
    priority: 2200,
    postProcessDefinition: 'Non waterfront expressions',
    PostProcessSubType: 'UniqueConditionSelector',
    traceEnabledFields: ['NewLandValue'],
    secondaryDatasets: secondaryDatasets,
    exceptionPostProcessRules: rules,
  };

  if (rules.length === 0) {
    if (postProcessId) {
      await deleteDatasetPostProcess(postProcessId);
      setPostProcessId(undefined);
    }
    return null;
  }
  return saveExceptionPostProcess(data);
};

export const waterFrontExpressionService = async (
  expressionData: ExpressionGridData[],
  datasetId: string,
  setPostProcessId: Function,
  secondaryDatasets: string[],
  postProcessId?: number
): Promise<IdOnly | null> => {
  const value = 'WaterfrontValue';
  const expreMessage = 'WaterfrontExpressions';
  const rules = expressionData.map((expre) => {
    return {
      description: `Plat Value: ${expre.filter}`,
      customSearchExpressions: [
        {
          expressionType: 'TSQL',
          expressionRole: 'FilterExpression',
          script: expre.filter?.trim(),
          columnName: value,
          expressionExtensions: {
            Note: expre.note?.trim() || '',
            traceMessage: `'${expreMessage}: ${expre.filter.replaceAll(
              "'",
              `"`
            )} =>' {ColumnValue}`,
          },
        },
        {
          expressionType: 'TSQL',
          expressionRole: 'CalculatedColumn',
          script: expre.expression?.trim(),
          columnName: value,
        },
      ],
    };
  });

  const data = {
    datasetId: datasetId,
    postProcessName: 'Waterfront expressions',
    postProcessRole: 'WaterfrontExpressions',
    priority: 2400,
    postProcessDefinition: 'Waterfront expressions',
    PostProcessSubType: 'UniqueConditionSelector',
    traceEnabledFields: ['WaterfrontValue'],
    secondaryDatasets: secondaryDatasets,
    exceptionPostProcessRules: rules,
  };

  if (rules.length === 0) {
    if (postProcessId) {
      await deleteDatasetPostProcess(postProcessId);
      setPostProcessId(undefined);
    }
    return null;
  }

  return saveExceptionPostProcess(data);
};

export const runWaterFrontFromExcel = async (
  excelData: SheetType | undefined,
  datasetId: string,
  secondaryDatasets: string[]
): Promise<IdOnly | null> => {
  if (!excelData) return null;
  const { rows, headers } = excelData;

  //eslint-disable-next-line
  let rules: any[] = [];
  rows.forEach((row, rowIndex) => {
    //eslint-disable-next-line
    let [, ...restHeaders] = headers;
    restHeaders = restHeaders.reverse();
    const [to, ...rest] = row;
    rest
      .filter((rule: RuleType) => rule)
      .reverse()
      .forEach((rule: string | undefined, index: number) => {
        if (rule === undefined) return;
        const stepFilter = ['baseline', 'default'].includes(
          restHeaders[index]?.toLowerCase()
        )
          ? ''
          : restHeaders[index];
        rules = [
          ...rules,
          {
            customSearchExpressions: [
              {
                expressionRole: 'FilterExpression',
                expressionExtensions: {
                  StepFilter: stepFilter,
                  ScheduleStep: to,
                  StepValue: rule,
                },
              },
              {
                expressionRole: 'CalculatedColumn',
              },
            ],
          },
        ];
      });
  });
  const data = {
    datasetId: datasetId,
    postProcessName: 'Waterfront schedule',
    postProcessRole: 'WaterfrontSchedule',
    priority: 2300,
    postProcessDefinition: 'Waterfront schedule No',
    PostProcessSubType: 'UniqueConditionSelector',
    exceptionPostProcessRules: rules,
    secondaryDatasets: secondaryDatasets,
  };
  return runWaterfrontSetup(data);
};

type RuleType = string | undefined;

export const runNonWaterFrontFromExcel = async (
  excelData: SheetType | undefined,
  datasetId: string,
  secondaryDatasets: string[]
): Promise<IdOnly | null> => {
  if (!excelData) return null;
  const { rows, headers } = excelData;
  //eslint-disable-next-line
  let rules: any[] = [];

  rows.forEach((row, index) => {
    let [, ...restHeaders] = headers;
    restHeaders = restHeaders.reverse();
    const [to, ...rest] = row;
    rest
      .filter((rule: RuleType) => rule)
      .reverse()
      .forEach((rule: string, indexRest: number) => {
        const stepFilter = ['baseline', 'default'].includes(
          restHeaders[indexRest]?.toLowerCase()
        )
          ? ''
          : restHeaders[indexRest];
        rules = [
          ...rules,
          {
            customSearchExpressions: [
              {
                expressionRole: 'FilterExpression',
                expressionExtensions: {
                  ScheduleStep: to,
                  StepValue: rule,
                  StepFilter: stepFilter,
                },
              },
              {
                expressionRole: 'CalculatedColumn',
              },
            ],
          },
        ];
      });
  });
  const data = {
    datasetId: datasetId,
    postProcessName: 'Nonwaterfront schedule',
    postProcessRole: 'LandSchedule',
    priority: 2100,
    postProcessDefinition: 'Nonwaterfront schedule',
    PostProcessSubType: 'UniqueConditionSelector',
    traceEnabledFields: ['NewLandValue'],
    exceptionPostProcessRules: rules,
    secondaryDatasets: secondaryDatasets,
  };
  return runNonWaterfrontSetup(data);
};

export const runNonWaterfront = (
  param: WaterFrontType,
  _exceptionId?: number
): Promise<IdOnly | null> => {
  const step = param.step;

  const data = {
    datasetId: param.datasetId,
    postProcessName: 'Nonwaterfront schedule',
    postProcessRole: 'LandSchedule',
    priority: 2100,
    postProcessDefinition: 'Nonwaterfront schedule',
    postProcessType: 'ExceptionPostProcess',
    PostProcessSubType: 'UniqueConditionSelector',
    traceEnabledFields: ['NewLandValue'],
    exceptionPostProcessRules:
      param.rules.map((rule, index) => {
        const to = index === 0 ? parseInt(rule.to) + step : parseInt(rule.to);
        return {
          customSearchExpressions: [
            {
              expressionRole: 'FilterExpression',
              expressionExtensions: {
                ScheduleStep: to,
                StepValue: 0,
              },
            },
            {
              expressionRole: 'CalculatedColumn',
            },
          ],
        };
      }) || [],
  };

  const payload = {
    ...data,
    exceptionPostProcessRules: [...data.exceptionPostProcessRules],
  };

  return runNonWaterfrontSetup(payload);
};

//
export const runWaterfront = (
  param: WaterFrontType,
  _yes = false,
  _id?: number
): Promise<IdOnly | null> => {
  const step = param.step;

  // const getScriptFilter = (
  //   from: number,
  //   to: number,
  //   index: number,
  //   //eslint-disable-next-line
  //   rules: any[]
  // ): string => {
  //   if (rules.length === index + 1) return `[WftFoot] > ${from}`;
  //   return `WftFoot BETWEEN ${from} AND ${to}`;
  // };

  const rules = param.rules.map((rule, index) => {
    const from = index === 0 ? parseInt(rule.to) : parseInt(rule.to) - step;
    const to = index === 0 ? parseInt(rule.to) + step : parseInt(rule.to);
    return {
      description: `From ${from} to ${parseInt(rule.to)}`,
      customSearchExpressions: [
        {
          expressionRole: 'FilterExpression',
          expressionExtensions: {
            ScheduleStep: to,
            StepValue: 0,
          },
        },
        {
          expressionRole: 'CalculatedColumn',
        },
      ],
    };
  });

  const data = {
    datasetId: param.datasetId,
    postProcessName: 'Waterfront schedule',
    postProcessRole: 'WaterfrontSchedule',
    priority: 2300,
    postProcessType: 'ExceptionPostProcess',
    postProcessDefinition: 'Waterfront schedule No',
    PostProcessSubType: 'UniqueConditionSelector',
    traceEnabledFields: ['WaterfrontValue'],
    exceptionPostProcessRules: rules,
  };

  const payload = {
    ...data,
    exceptionPostProcessRules: [...data.exceptionPostProcessRules],
  };

  return runWaterfrontSetup(payload);
};

//Postive Adjustment services
export const runAdjustment = async (
  param: AdjustmentParam,
  postProcessId: number | undefined,
  secondaryDatasets: string[]
): Promise<IdOnly | null | void> => {
  const data = {
    datasetId: param.datasetId,
    postProcessName: 'Adjustments',
    postProcessRole: 'LandAdjustment',
    priority: 2600,
    postProcessDefinition: 'Adjustments',
    PostProcessSubType: 'MultipleConditionModifier',
    secondaryDatasets: secondaryDatasets,
    exceptionPostProcessRules:
      param.rules.map((rule: LandVariableGridRowData) => {
        return {
          description: rule.description,
          customSearchExpressions: [
            {
              expressionType: 'TSQL',
              expressionRole: 'FilterExpression',
              script: '0 = 0',
              columnName: process.env.REACT_APP_NONWATERFRONT_LAND_VALUE || '',
              expressionExtensions: {
                ...rule,
                objectTypeCode: 'ptas_landvaluecalculation',
              },
            },
            {
              expressionType: 'TSQL',
              expressionRole: 'CalculatedColumn',
              script: '0',
              columnName: process.env.REACT_APP_NONWATERFRONT_LAND_VALUE || '',
            },
          ],
        };
      }) || [],
  };

  if (data.exceptionPostProcessRules.length) {
    return saveAdjustmentPostProcess(data);
  }
  if (postProcessId) {
    return deleteDatasetPostProcess(`${postProcessId}`);
  }
  return null;
};

export const runWaterFrontToLandValue = async (
  datasetId: string
): Promise<IdOnly | null> => {
  const data = {
    datasetId: datasetId,
    postProcessName: 'Add Waterfront Value To Land Value',
    postProcessRole: 'AddWaterfrontValueToLandValue',
    priority: 2500,
    postProcessDefinition: 'Add Waterfront Value To Land Value',
    PostProcessSubType: 'UniqueConditionSelector',
    exceptionPostProcessRules: [
      {
        description: 'Add Waterfront Value To Land Value',
        customSearchExpressions: [
          {
            expressionType: 'TSQL',
            expressionRole: 'FilterExpression',
            script: '1 = 1',
            columnName: 'NewLandValue',
          },
          {
            expressionType: 'TSQL',
            expressionRole: 'CalculatedColumn',
            script:
              '(SELECT Max(v) FROM (VALUES (CAST([LandValue] as FLOAT) + CAST([WaterfrontValue] as FLOAT)), ({MinLandValue})) AS value(v))',
            columnName: 'NewLandValue',
          },
        ],
      },
      {
        description: 'Add Waterfront Value To Land Value Trace',
        customSearchExpressions: [
          {
            expressionType: 'TSQL',
            expressionRole: 'FilterExpression',
            script: '1 = 1',
            columnName: 'LandValue_RulesTrace',
          },
          {
            expressionType: 'TSQL',
            expressionRole: 'CalculatedColumn',
            script: '[LandValue_RulesTrace] + [WaterfrontValue_RulesTrace]',
            columnName: 'LandValue_RulesTrace',
          },
        ],
      },
    ],
  };
  return saveExceptionPostProcess(data);
};
