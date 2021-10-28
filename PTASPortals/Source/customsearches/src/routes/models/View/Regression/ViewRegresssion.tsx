// AddRegression.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useEffect, useState } from 'react';

import PlayArrowOutlined from '@material-ui/icons/PlayArrowOutlined';
import Loading from 'components/Loading';
import LinearRegression from './LinearRegression';
import {
  GenericGridRowData,
  JobStateType,
  PostProcess,
  Project,
  ProjectDataset,
  ResultPayload,
  SearchParameters,
  RegressionRoleType,
} from 'services/map.typings';
import { Link, useParams, useLocation } from 'react-router-dom';
import {
  LoadProject,
  LoadPostProcess,
  runImportRScript,
  getTimeTrendCustomSearchExpressions,
} from './common';
import '../../../../assets/time-trend-styles/styles.scss';
import { PanelHeader } from '@ptas/react-ui-library';
import AgGridService from 'services/AgGridService';
import { ProjectContext } from 'context/ProjectsContext';
import useSignalR from 'components/common/useSignalR';
import { AppContext } from 'context/AppContext';
import { SalesDropdown } from './SalesDropdown';
import { executeDatasetPostProcess } from '../Projects/Land/services/landServices';
import { ImportErrorMessageType } from 'components/ErrorMessage/ErrorMessage';
import { EmptyGrid } from 'components/Grid/EmptyGrid';
import { RunningRegression } from '../Projects/Land/RunningRegression';
import { RegressionGrid } from './RegressionGrid';
import { v4 as uuidv4 } from 'uuid';
import _ from 'underscore';

interface UserInfo {
  email: string;
  fullName: string;
  id: string;
  roles: unknown;
  teams: unknown;
}

let debounced: (() => Promise<void>) & _.Cancelable;

const AddRegression = (): JSX.Element => {
  const { id, regressionid }: { id: string; regressionid: string } =
    useParams();
  const location = useLocation<JobStateType>();
  const [project, setProjectInfo] = useState<Project | null | undefined>(null);

  const [postProcess, setPostProcess] = useState<
    PostProcess | null | undefined
  >(null);

  const context = useContext(ProjectContext);
  const appContext = useContext(AppContext);
  const [runningRegression, setRunningRegression] = useState<boolean>(false);
  const [isDatasetChanged, setChangeDataset] = useState<boolean>(false);
  const [secondaryDatasets, setSecondaryDatasets] = useState<string[]>([]);

  const [regression, setRegression] = useState<number>(0);
  const [gridData, setGridData] = useState<GenericGridRowData[]>([]);
  const [datasetId, setDatasetId] = useState<string>('');
  const [datasets, setDatasets] = useState<ProjectDataset[]>([]);
  // const [postProcessState, setPostProcessState] = useState<string>('');
  const [userInfo, setUserInfo] = useState<UserInfo[]>();
  const [postProcessResult, setPostProcessResult] = useState<ResultPayload>();
  const [expressionPayload, setExpressionsPayload] = useState<
    GenericGridRowData[]
  >([]);
  const [rowData, setRowData] = useState<GenericGridRowData[]>([]);
  const [parameterState, setParameterState] = useState<
    SearchParameters[] | undefined
  >([]);
  const [postProcessCorrect, setPostProcessCorrect] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<ImportErrorMessageType>();
  const [disableButton, setDisableButton] = useState<boolean>(true);
  const [formChanged, setFormChanged] = useState<boolean>(false);
  const [updateRegression, setUpdateRegression] = useState<boolean>(false);
  const [jobId, setJobId] = useState<number>(location?.state?.jobId | 0);
  const [gridKey, setGridKey] = useState<string>(uuidv4());

  const [isDirty, setIsDirty] = useState<boolean>(false);

  const { message } = useSignalR(jobId);

  const fetchPostProcess = async (): Promise<void> => {
    const pp = await LoadPostProcess(regressionid);
    setPostProcess(pp);
    // if (pp?.datasetId) setDatasetId(pp?.datasetId);
    // if (pp?.secondaryDatasets) {
    //   setSecondaryDatasets(pp.secondaryDatasets);
    // }
    if (pp?.isDirty) {
      setDisableButton(false);
      setIsDirty(true);
    }
    if (pp?.resultPayload?.length) {
      setPostProcessResult(JSON.parse(pp.resultPayload));
      setPostProcessCorrect(true);
    }
  };

  const callPostProcessData = (): void => {
    fetchPostProcess();
  };

  const fetchProjectData = async (): Promise<void> => {
    const project = await LoadProject(id, true);
    setProjectInfo(project.project);
    setUserInfo(project.usersDetails);
  };

  const callProjectData = (): void => {
    fetchProjectData();
  };

  //eslint-disable-next-line
  const callAllData = () => {
    getUserProject();
    callProjectData();
    callPostProcessData();
    return (): void => {
      debounced?.cancel();
    };
  };

  useEffect(callAllData, []);

  const validateMessage = async (): Promise<void> => {
    if (!message) {
      debounced = _.debounce(getJobStatus, 5000);
      debounced();
    }
    if (message?.jobStatus?.length) {
      setPostProcessCorrect(true);
      await getUserProject();
      if (message?.jobStatus === 'Succeeded') {
        await fetchPostProcess();
      }
      if (updateRegression) {
        await getUserProject();
        await callProjectData();
        await callPostProcessData();
      }
      setUpdateRegression(false);
    }
  };

  const callValidateMessage = (): void => {
    validateMessage();
  };

  useEffect(callValidateMessage, [message]);

  const getUserProject = async (): Promise<void> => {
    const project = await LoadProject(id, true);
    //eslint-disable-next-line
    let postProcessFound: PostProcess | any = {};
    project?.project?.projectDatasets?.forEach(
      (projectDataset: ProjectDataset) => {
        const ppf = projectDataset?.dataset?.dependencies?.postProcesses?.find(
          (pos: PostProcess) =>
            pos?.postProcessRole === 'TimeTrendRegression' &&
            pos.primaryDatasetPostProcessId === null
        );

        if (ppf) {
          postProcessFound = ppf;
          setDatasetId(ppf?.datasetId);
          setSecondaryDatasets(ppf?.secondaryDatasets ?? []);
        }
      }
    );

    const datasets = project?.project?.projectDatasets || [];

    setDatasets(datasets);
    let result = null;
    if (postProcessFound?.resultPayload) {
      result = JSON.parse(postProcessFound?.resultPayload);
    }
    if (result && result?.Status.length) {
      setPostProcessCorrect(true);
    }
    setRegression(postProcessFound?.rscriptModelId);
    setProjectInfo(project.project);
    if (result && result?.Status === 'Success') {
      setPostProcessResult(JSON.parse(postProcessFound?.resultPayload));
      // setPostProcessState('Processed');
    }
    if (result && result?.Status === 'Failed') {
      setErrorMessage({
        message: `${result.Reason}`,
      });
    }
  };

  const getJobStatus = async (): Promise<void> => {
    if (location?.state?.jobId && regression) {
      const jobStatus = await AgGridService.getJobStatus(location.state.jobId);
      const status =
        jobStatus?.jobStatus === 'Finished' ||
        jobStatus?.jobStatus === 'Failed';
      if (!status) {
        debounced = _.debounce(getJobStatus, 5000);
        debounced();
      } else {
        if (jobStatus?.jobResult?.Status === 'Failed') {
          setErrorMessage({
            message: `${jobStatus.jobResult.Reason}`,
          });
        }
        if (updateRegression) {
          await getUserProject();
          await callProjectData();
          await callPostProcessData();
        }
        setUpdateRegression(false);
        setPostProcessCorrect(status);
        getUserProject();
      }
    }
  };

  const addPostProcessUpdate = (jobId: string): void => {
    const postProcessId = parseInt(regressionid);
    const pp = appContext.postProcessInProgress ?? [];
    const index = pp?.findIndex((p) => p.postProcessId === postProcessId);
    if (index !== -1) {
      pp[index].jobId = parseInt(jobId);
      if (pp) appContext.setPostProcessInProgress?.([...pp]);
      return;
    }
    appContext?.handlePostProcessInProgress?.({
      jobId: parseInt(jobId),
      postProcessId: postProcessId,
      datasetId: datasetId,
      postProcessRole: RegressionRoleType.TimeTrendRegression.toLowerCase(),
    });
  };

  const runRegression = async (): Promise<void> => {
    if (runningRegression) return;

    if (!gridData) {
      appContext.setSnackBar?.({
        severity: 'error',
        text: 'No form data.',
      });
      return;
    }
    setRunningRegression(true);

    const customSearchExpressions = getTimeTrendCustomSearchExpressions(
      gridData,
      parameterState,
      expressionPayload
    );

    const payload = {
      datasetId: datasetId,
      postProcessName: 'Time Trend Regression',
      priority: 1000,
      rScriptModelId: regression,
      postProcessDefinition: 'RScript post process test',
      customSearchExpressions: customSearchExpressions,
      postProcessRole: RegressionRoleType.TimeTrendRegression,
      secondaryDatasets: secondaryDatasets,
    };

    try {
      if (formChanged) {
        await runImportRScript(payload);
        const executePost = await executeDatasetPostProcess(
          `${payload.datasetId}`,
          `${regressionid}`
        );
        setJobId(parseInt(`${executePost?.id}`));
        if (executePost?.id) addPostProcessUpdate(executePost?.id);
      }
      if (isDirty && !formChanged) {
        const executePost = await executeDatasetPostProcess(
          `${payload.datasetId}`,
          `${regressionid}`
        );
        setJobId(parseInt(`${executePost?.id}`));
        if (executePost?.id) addPostProcessUpdate(executePost?.id);
      }
      setErrorMessage(undefined);
      setUpdateRegression(true);
      setGridKey(uuidv4());
    } catch (e) {
      setErrorMessage({
        message: e?.message ?? e,
        reason: e?.validationError ?? [],
      });
      setRowData(gridData);
    } finally {
      setRunningRegression(false);
    }
  };

  const defaultIcons = [
    {
      icon: <PlayArrowOutlined />,
      text: 'Calculate Regression',
      onClick: (): Promise<void> => {
        return runRegression();
      },
      disabled: context.modelDetails?.isLocked || disableButton,
    },
  ];
  const getFormData = (
    regression: number,
    gridData: GenericGridRowData[],
    expressionPayload: GenericGridRowData[],
    params: SearchParameters[] | undefined
  ): void => {
    setRegression(regression);
    setGridData(gridData);
    setExpressionsPayload(expressionPayload);
    setParameterState(params);
  };

  const getBottom = (): string => {
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

  const changeGrid = (): void => {
    setDisableButton(false);
    setFormChanged(true);
  };

  const renderGrid = (): JSX.Element => {
    if (postProcess && postProcessCorrect && gridData.length) {
      return (
        <RegressionGrid
          key={gridKey}
          datasets={datasets?.filter((s) =>
            [datasetId, ...secondaryDatasets].includes(s.datasetId)
          )}
          project={project}
          postProcess={postProcess}
          gridData={gridData}
          edit
        />
      );
    }
    return <EmptyGrid />;
  };

  const getDatasetId = (id: string): void => {
    if (!isDatasetChanged) return;
    setDatasetId(id);
  };

  const getSecondaryDataset = (datasetIds: string[]): void => {
    setSecondaryDatasets(datasetIds);
  };

  const onChangeSecondary = (): void => {
    setDisableButton(false);
    // setFormChanged(true);
  };

  const changeDataset = (): void => {
    setChangeDataset(true);
  };

  //eslint-disable-next-line
  if (
    (!project && !postProcess) ||
    !context.modelDetails ||
    context.salesDatasetId?.length === 0 ||
    context.salesDatasetId === undefined ||
    !postProcessCorrect
  )
    return <Loading />;

  return (
    <div>
      <RunningRegression
        runningRegression={runningRegression || updateRegression}
      />
      <PanelHeader
        route={[
          <Link to="/models" style={{ color: 'black' }}>
            Models
          </Link>,
          <Link to={`/models/view/${id}`} style={{ color: 'black' }}>
            {project?.projectName}
          </Link>,
          <Link
            to={`/models/regression/${id}/${regressionid}`}
            style={{ color: 'black' }}
          >
            {postProcess?.postProcessName}
          </Link>,
        ]}
        icons={defaultIcons}
        // detailTop={getDetailTop()}
        detailBottom={getBottom()}
      />
      {datasets.length && (
        <SalesDropdown
          edit
          options={datasets}
          getDatasetId={getDatasetId}
          datasetId={datasetId}
          getSecondaryDataset={getSecondaryDataset}
          selectedValues={secondaryDatasets}
          onChange={onChangeSecondary}
          changeDataset={changeDataset}
        />
      )}
      <LinearRegression
        id={id}
        regression={regression}
        regressionid={regressionid}
        postProcessResult={postProcessResult}
        modelDetails={context.modelDetails}
        getFormData={getFormData}
        viewReport={true}
        variableGridData={postProcess?.customSearchExpressions}
        rowData={rowData}
        postProcess={postProcess}
        changeGrid={changeGrid}
        errorMessage={errorMessage}
        regressionType="Time Trend:"
        usePostProcessRegression={true}
        // useDropdown={true}
      />
      <div className="TimeTrend-maingrid">{renderGrid()}</div>
    </div>
  );
};
export default AddRegression;
