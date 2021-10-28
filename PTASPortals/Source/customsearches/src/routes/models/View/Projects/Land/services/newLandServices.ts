import { ImportErrorMessageType } from './../../../../../../components/ErrorMessage/ErrorMessage';
import { ExpressionGridData } from './../ExpressionsGrid';
// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ColDef } from 'ag-grid-community';
import { LoadPostProcess } from './../../../Regression/common';
import {
  PostProcess,
  Project,
  LandGridData,
  LandVariableGridRowData,
} from 'services/map.typings';
import {
  buildNonWaterFrontGridData,
  buildPositiveAdjustment,
  buildWaterFrontGridData,
  buildNonWaterFrontExpressionGridData,
} from './methods';
import { AxiosLoader } from './../../../../../../services/AxiosLoader';
import { v4 as uuidv4 } from 'uuid';

interface PostProcessLandValues {
  mainPostProcess: PostProcess;
  regressionViewKey: string;
  nonWaterFront: PostProcess;
  adjustmentPostProcess: PostProcess;
  waterFront: PostProcess;
  disableApplyButton: boolean;
}

interface SetStatesTypes {
  setNonWaterfrontGridData: (data: LandGridData[]) => void;
  setNonWaterColDefs: (col: ColDef[]) => void;
  setWaterfrontGridData: (data: LandGridData[]) => void;
  setWaterColDefs: (col: ColDef[]) => void;
  setPositiveGridData: (data: LandVariableGridRowData[]) => void;
  setNonWaterFrontExpressionData: (data: ExpressionGridData[]) => void;
  setWaterFrontExpressionData: (data: ExpressionGridData[]) => void;
  setWaterFrontExpressionPostProcessId: (value: number) => void;
  setNonWaterFrontExpressionPostProcessId: (value: number) => void;
}

export interface UserInfo {
  email: string;
  fullName: string;
  id: string;
  roles: unknown;
  teams: unknown;
}

export interface ErrorMessageType {
  message: ImportErrorMessageType | undefined;
  section: string;
}

export const getBottom = (
  userInfo: UserInfo[] | undefined,
  project: Project
): string => {
  if (!userInfo) return '';
  if (!project) return '';

  const oldestProjectDataset = project.projectDatasets.reduce((r, o) =>
    o.dataset.lastExecutionTimestamp &&
    r.dataset.lastExecutionTimestamp &&
    new Date(o.dataset.lastExecutionTimestamp) <
      new Date(r.dataset.lastExecutionTimestamp)
      ? o
      : r
  );

  const oldestDsDate = new Date(
    oldestProjectDataset.dataset.lastExecutionTimestamp + 'Z' ||
      new Date().toLocaleString()
  ).toLocaleString();

  return `Last sync on ${oldestDsDate}, by ${
    userInfo.find((u) => u.id === project.userId)?.fullName ?? 'John Doe'
  }`;
};

export const runExecutePostProcess = (
  datasetId: string,
  postProcessId: number
): Promise<unknown> => {
  const ad2 = new AxiosLoader<unknown, unknown>();
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

export const getPostProcess = async (
  project: Project,
  {
    setNonWaterfrontGridData,
    setNonWaterColDefs,
    setWaterfrontGridData,
    setWaterColDefs,
    setPositiveGridData,
    setNonWaterFrontExpressionData,
    setWaterFrontExpressionData,
    setWaterFrontExpressionPostProcessId,
    setNonWaterFrontExpressionPostProcessId,
  }: SetStatesTypes
): Promise<PostProcessLandValues> => {
  //eslint-disable-next-line
  let data: any = {};
  //eslint-disable-next-line
  const postProcessesList: any[] = [];
  if (project) {
    const datasets = project?.projectDatasets;
    let postProcesses: PostProcess[] = [];
    let mainPostProcess = null;
    let postProcessesSchedule: PostProcess | undefined;

    datasets.forEach((d) => {
      if (
        d.dataset.dependencies.postProcesses?.some(
          (pp) => pp.postProcessRole === 'LandSchedule'
        )
      ) {
        postProcesses = d.dataset.dependencies.postProcesses;
      }
      mainPostProcess = d?.dataset.dependencies.postProcesses?.find(
        (pp) => pp.priority === 2000 && pp.primaryDatasetPostProcessId === null
      );
      if (mainPostProcess) {
        data = { ...data, mainPostProcess, regressionViewKey: uuidv4() };
      }
      postProcessesSchedule = d?.dataset.dependencies.postProcesses?.find(
        (pp) => pp.postProcessRole === 'WaterfrontSchedule' && pp.primaryDatasetPostProcessId === null
      );

      if (postProcesses) {
        const nonWaterFromPostProcess = postProcesses?.find(
          (pp) => pp.postProcessDefinition === 'Nonwaterfront schedule' && pp.primaryDatasetPostProcessId === null
        );
        if (nonWaterFromPostProcess) {
          data = { ...data, nonWaterFront: nonWaterFromPostProcess };
        }
        if (nonWaterFromPostProcess) {
          postProcessesList?.push(
            LoadPostProcess(
              nonWaterFromPostProcess.datasetPostProcessId?.toString()
            )
          );
        }
      }
      const adjustmentPostProcess =
        d?.dataset.dependencies.postProcesses?.find(
          (pp) => pp.postProcessRole === 'LandAdjustment' && pp.primaryDatasetPostProcessId === null
        );

      const nonWaterExpressionsPostProcess =
        d?.dataset.dependencies.postProcesses?.find(
          (pp) => pp.postProcessRole === 'NonWaterfrontExpressions' && pp.primaryDatasetPostProcessId === null
        );

      const waterExpressionsPostProcess =
        d?.dataset.dependencies.postProcesses?.find(
          (pp) => pp.postProcessRole === 'WaterfrontExpressions' && pp.primaryDatasetPostProcessId === null
        );

      const waterFromPostProcessId =
        postProcessesSchedule?.datasetPostProcessId;

      if (adjustmentPostProcess) {
        data = { ...data, adjustmentPostProcess };
      }
      if (adjustmentPostProcess) {
        postProcessesList?.push(
          LoadPostProcess(adjustmentPostProcess.datasetPostProcessId.toString())
        );
      }
      if (postProcessesSchedule) {
        data = { ...data, waterFront: postProcessesSchedule };
      }
      if (waterFromPostProcessId) {
        postProcessesList?.push(
          LoadPostProcess(waterFromPostProcessId?.toString())
        );
      }
      if (nonWaterExpressionsPostProcess) {
        postProcessesList.push(
          LoadPostProcess(
            nonWaterExpressionsPostProcess.datasetPostProcessId?.toString()
          )
        );
      }

      if (waterExpressionsPostProcess) {
        postProcessesList.push(
          LoadPostProcess(
            waterExpressionsPostProcess.datasetPostProcessId?.toString()
          )
        );
      }
    });

    const postProcessesResult = await Promise.all(postProcessesList);

    const waterExpressionsRules = postProcessesResult.find(
      (po) => po?.postProcessRole === 'WaterfrontExpressions'
    );
    if (
      waterExpressionsRules &&
      Array.isArray(waterExpressionsRules?.exceptionPostProcessRules)
    ) {
      buildNonWaterFrontExpressionGridData(
        waterExpressionsRules?.exceptionPostProcessRules || [],
        setWaterFrontExpressionData
      );
      setWaterFrontExpressionPostProcessId(
        waterExpressionsRules?.datasetPostProcessId
      );
    }

    if (mainPostProcess) {
      data = { ...data, disableApplyButton: false };
    }

    const nonWaterExpressionsRules = postProcessesResult.find(
      (po) => po?.postProcessRole === 'NonWaterfrontExpressions'
    );

    if (
      nonWaterExpressionsRules &&
      Array.isArray(nonWaterExpressionsRules?.exceptionPostProcessRules)
    ) {
      buildNonWaterFrontExpressionGridData(
        nonWaterExpressionsRules?.exceptionPostProcessRules || [],
        setNonWaterFrontExpressionData
      );
      setNonWaterFrontExpressionPostProcessId(
        nonWaterExpressionsRules?.datasetPostProcessId
      );
    }

    const nonWater = postProcessesResult.find(
      (po) => po?.postProcessRole === 'LandSchedule'
    )?.exceptionPostProcessRules;

    if (Array.isArray(nonWater)) {
      buildNonWaterFrontGridData(
        nonWater || [],
        setNonWaterfrontGridData,
        setNonWaterColDefs
      );
    }

    const water = postProcessesResult.find(
      (po) => po?.postProcessRole === 'WaterfrontSchedule'
    )?.exceptionPostProcessRules;

    if (Array.isArray(water)) {
      buildWaterFrontGridData(
        water || [],
        setWaterfrontGridData,
        setWaterColDefs
      );
    }

    const adjustment = postProcessesResult.find(
      (po) => po?.postProcessRole === 'LandAdjustment'
    )?.exceptionPostProcessRules;

    if (Array.isArray(adjustment)) {
      buildPositiveAdjustment(adjustment || [], setPositiveGridData);
    }
  }
  return data as PostProcessLandValues;
};
