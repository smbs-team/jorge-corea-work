// ViewMultipleRegression.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect, useState } from 'react';

import PlayArrowOutlined from '@material-ui/icons/PlayArrowOutlined';
import Loading from 'components/Loading';
import {
  GenericGridRowData,
  IdOnly,
  PostProcess,
  Project,
  ProjectDataset,
  ResultPayload,
  ResultDefinitionType,
  JobStateType,
} from 'services/map.typings';
import { Link, useLocation, useParams } from 'react-router-dom';
import {
  getDependentVars,
  LoadProject,
  getIndependentVars,
  getCalculatedVars,
  LoadPostProcess,
  getMultipleCustomExpressions,
  fetchMultipleRegressionsType,
  getHidenVars,
} from './common';
import { AxiosLoader } from 'services/AxiosLoader';

import '../../../../assets/time-trend-styles/styles.scss';
import { PanelHeader } from '@ptas/react-ui-library';
import GenericGrid from 'components/GenericGrid/GenericGrid';
import { InputLabel, Grid, makeStyles } from '@material-ui/core';
import moment from 'moment';
import { AppContext } from 'context/AppContext';
import AgGridService from 'services/AgGridService';
import useSignalR from 'components/common/useSignalR';
import { SalesDropdown } from './SalesDropdown';
import ErrorMessage, {
  ImportErrorMessageType,
} from 'components/ErrorMessage/ErrorMessage';
import { getPredictedEcuation } from '../Projects/Land/services/landServices';
import { EmptyGrid } from 'components/Grid/EmptyGrid';
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

const useStyles = makeStyles({
  ecuation: {
    fontSize: 16,
    fontWeight: 500,
    marginLeft: -16,
  },
  results: {
    display: 'flex',
    flexDirection: 'row',
  },
});

interface MultipleExpressionsType {
  rowData?: GenericGridRowData[];
  expressionPayload?: GenericGridRowData[];
}

let debounced: (() => Promise<void>) & _.Cancelable;

const ViewMultipleRegression = (): JSX.Element => {
  const { id, regressionid }: { id: string; regressionid: string } =
    useParams();
  const classes = useStyles();
  const location = useLocation<JobStateType>();
  const [project, setProjectInfo] = useState<Project | null>(null);
  const [disableButton, setDisableButton] = useState<boolean>(true);
  const [runningRegression, setRunningRegression] = useState<boolean>(false);
  const [updateRegression, setUpdateRegression] = useState<boolean>(false);
  const [jobId, setJobId] = useState<number>(location.state?.jobId | 0);

  const [userInfo, setUserInfo] = useState<UserInfo[]>();
  const [gridData, setGridData] = useState<GenericGridRowData[]>([]);
  const [datasetId, setDatasetId] = useState<string>('');
  const [datasets, setDatasets] = useState<ProjectDataset[]>([]);
  const [secondaryDatasets, setSecondaryDatasets] = useState<string[]>([]);
  const [postProcessCorrect, setPostProcessCorrect] = useState<boolean>(false);
  const [rowData, setRowData] = useState<GenericGridRowData[]>([]);
  const [postProcess, setPostProcess] = useState<PostProcess | null>(null);
  const [errorMessage, setErrorMessage] = useState<ImportErrorMessageType>();
  const [predictedEcuation, setPredictedEcuation] = useState<string>('');
  const [gridKey, setGridKey] = useState<string>(uuidv4());
  const [lockPrecommitExpressions, setLockPrecommitExpressions] =
    useState(false);
  // const [expressionPayload, setExpressionPayload] = useState<
  //   GenericGridRowData[]
  // >([]);
  const [postProcessResult, setPostProcessResult] = useState<ResultPayload>();
  const [regression, setRegression] = useState<number>(0);
  const appContext = useContext(AppContext);
  const [regressionType, setRegressionType] =
    useState<ResultDefinitionType[]>();

  const { message } = useSignalR(jobId);

  //eslint-disable-next-line
  const fetchProjectData = () => {
    getUserProject();
    return (): void => {
      debounced?.cancel();
    };
  };

  useEffect(fetchProjectData, []);

  const verifySignalRMessage = async (): Promise<void> => {
    if (!message) {
      debounced = _.debounce(getJobStatus, 5000);
      debounced();
    }
    if (message?.jobStatus?.length) {
      await getUserProject();
      setPostProcessCorrect(true);
      setUpdateRegression(false);
    }
  };

  const callVerifySignalRMessage = (): void => {
    verifySignalRMessage();
  };

  useEffect(callVerifySignalRMessage, [message]);

  const getJobStatus = async (): Promise<void> => {
    if (location?.state?.jobId) {
      const jobStatus = await AgGridService.getJobStatus(location.state.jobId);
      const status =
        jobStatus?.jobStatus === 'Finished' ||
        jobStatus?.jobStatus === 'Failed';
      // const status = false;
      if (!status) {
        debounced = _.debounce(getJobStatus, 5000);
        debounced();
      } else {
        if (jobStatus?.jobResult?.Status === 'Failed') {
          setErrorMessage({
            message: `${jobStatus?.jobResult.Reason}`,
          });
        }
        await getUserProject();
        setPostProcessCorrect(status);
        setUpdateRegression(false);
      }
    }
  };

  const getUserProject = async (): Promise<void> => {
    const project = await LoadProject(id, true);
    if (!project) return;
    setUserInfo(project.usersDetails);
    //eslint-disable-next-line
    let pp: ProjectDataset | any = {};
    project?.project?.projectDatasets?.forEach(
      (projectDataset: ProjectDataset) => {
        const postP = projectDataset.dataset?.dependencies?.postProcesses?.find(
          (pos: PostProcess) =>
            pos?.datasetPostProcessId === parseInt(regressionid)
        );
        if (postP) {
          pp = postP;
        }
      }
    );
    let result = null;
    if (pp) {
      if (pp.resultPayload) {
        result = JSON.parse(pp.resultPayload);
      }
      if (result && result?.Status === 'Success') {
        setPostProcessResult(JSON.parse(pp.resultPayload));
        setErrorMessage(undefined);
        const ecuation = await getPredictedEcuation(parseInt(regressionid));
        if (ecuation?.predictedEquation)
          setPredictedEcuation(ecuation?.predictedEquation);
      }
      if (result && result?.Status.length) {
        setPostProcessCorrect(true);
      }
      if (result && result?.Status === 'Failed') {
        setErrorMessage({
          message: `${result.Reason}`,
        });
      }
      if (pp.rscriptModelId) {
        setRegression(pp.rscriptModelId);
      }
      setProjectInfo(project.project);
    }
  };

  const fetchPostProcessData = async (): Promise<void> => {
    const pp = await LoadPostProcess(regressionid);
    const regressionInfo = await fetchMultipleRegressionsType();
    setRegressionType(regressionInfo?.resultDefinitions);
    setLockPrecommitExpressions(
      regressionInfo?.lockPrecommitExpressions ?? false
    );
    if (pp?.secondaryDatasets) {
      setSecondaryDatasets(pp.secondaryDatasets);
    }
    setPostProcess(pp);
    let data: MultipleExpressionsType | null = null;
    if (pp?.customSearchExpressions?.length) {
      data = getMultipleCustomExpressions(pp?.customSearchExpressions);
    }
    if (data) {
      setRowData(data?.rowData || []);
      // setExpressionPayload(data?.expressionPayload || []);
    }
  };

  const fetchCallPostProcess = (): void => {
    if (regressionid) {
      fetchPostProcessData();
    }
  };

  useEffect(fetchCallPostProcess, [regressionid]);

  const fetchData = async (): Promise<void> => {
    const project = await LoadProject(id);
    const datasets = project?.projectDatasets || [];

    let datasetId = datasets[0].datasetId ?? '';
    datasets.forEach((d: ProjectDataset) => {
      const pp = d.dataset?.dependencies?.postProcesses?.find(
        (p) => p.primaryDatasetPostProcessId === null && p.priority === 3000
      );
      if (pp?.datasetId) datasetId = pp.datasetId;
    });
    setDatasetId(datasetId);
    setDatasets(datasets);
  };

  const fetchCall = (): void => {
    fetchData();
  };

  useEffect(fetchCall, [id]);

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
      postProcessRole: 'multipleregression',
    });
  };

  const runRegression = async (): Promise<void> => {
    if (runningRegression) return;

    if (!gridData) {
      appContext.setSnackBar &&
        appContext.setSnackBar({
          severity: 'error',
          text: 'No form data.',
        });
      return;
    }
    const priorVars = getDependentVars(gridData);
    const postVars = getIndependentVars(gridData);
    const calculatedVars = getCalculatedVars(gridData);
    // const expressionVars = getExpressionVars(expressionPayload);
    const expressionVars = getHidenVars(priorVars, postVars);

    if (priorVars.length === 0) {
      appContext.setSnackBar?.({
        text: 'There need to be 1 dependent variable only to run regression',
        severity: 'warning',
      });
      return;
    }

    if (postVars.length === 0) {
      appContext.setSnackBar?.({
        text: 'There needs to be at least 1 independent variable to run regression',
        severity: 'warning',
      });
      return;
    }

    const customSearchExpressions = [
      ...priorVars,
      ...postVars,
      ...calculatedVars,
      ...expressionVars,
    ];

    const payload = {
      datasetId: datasetId,
      postProcessName: 'Estimated market value regression',
      priority: 3000,
      rScriptModelId: regression,
      postProcessDefinition: 'RScript post process test',
      customSearchExpressions: customSearchExpressions,
      postProcessRole: 'MultipleRegression',
      secondaryDatasets: secondaryDatasets,
    };
    try {
      setRunningRegression(true);
      const al1 = new AxiosLoader<IdOnly, {}>();
      const postProcessInfo = await al1.PutInfo(
        'CustomSearches/ImportRScriptPostProcess',
        payload,
        {}
      );
      const ad2 = new AxiosLoader<IdOnly, {}>();
      const executePost = await ad2.PutInfo(
        `CustomSearches/ExecuteDatasetPostProcess/${payload.datasetId}/${postProcessInfo?.id}`,
        [
          {
            Id: 0,
            Name: '',
            Value: '',
          },
        ],
        {}
      );

      setJobId(parseInt(`${executePost?.id}`));
      addPostProcessUpdate(executePost?.id ?? '');
      setErrorMessage(undefined);
      setUpdateRegression(true);
      fetchCallPostProcess();
      fetchProjectData();
      setGridKey(uuidv4());
    } catch (error) {
      if (error?.length) {
        const findDataset = datasets.find((d) => error?.includes(d.datasetId));
        const message = error?.replace(
          findDataset?.datasetId,
          findDataset?.dataset.datasetName
        );
        appContext.setSnackBar?.({
          severity: 'error',
          text: message,
        });
      }
      setErrorMessage({
        message: error?.message ?? error,
        reason: error?.validationError || [],
      });
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
      disabled: disableButton,
    },
  ];

  const saveColData = (): boolean => {
    return true;
  };

  const updateColData = (newData: GenericGridRowData[]): void => {
    setGridData(newData);
    setDisableButton(false);
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
      userInfo.find((u) => u.id === project.userId)?.fullName ?? project.userId
    }`;
  };

  const renderGrid = (): JSX.Element => {
    if (postProcess && postProcessCorrect && gridData.length) {
      return (
        <RegressionGrid
          key={gridKey}
          project={project}
          datasets={datasets?.filter((s) =>
            [datasetId, ...secondaryDatasets].includes(s.datasetId)
          )}
          postProcess={postProcess}
          gridData={gridData}
          edit
        />
      );
    }
    return <EmptyGrid />;
  };

  const renderResult = (name: string): string => {
    let resultString = 'NA';
    if (postProcessResult?.Results?.length) {
      const result = postProcessResult?.Results[0];
      Object.values(result).forEach((re, index) => {
        if (Object.keys(result)[index] === name) {
          resultString = re;
        }
      });
    }
    return resultString;
  };

  const renderVariablesResult = (): JSX.Element => {
    const columns: ResultDefinitionType[][] = [];
    let counter = 0;
    let rdtemp: ResultDefinitionType[] = [];
    if (regressionType?.length)
      for (const rd of regressionType) {
        rdtemp.push(rd);
        counter++;
        if (counter === 4) {
          counter = 0;
          columns.push(rdtemp);
          rdtemp = [];
        }
      }
    return (
      <Fragment>
        {columns?.map((rd) => (
          <Grid sm={4}>
            {rd?.map((r: ResultDefinitionType) => (
              <InputLabel className="" id="label-for-dd">
                <strong>{r.name}:</strong>{' '}
                <span className="Result-strong">{renderResult(r.name)}</span>
              </InputLabel>
            ))}
          </Grid>
        ))}
      </Fragment>
    );
  };

  const renderReportSection = (): JSX.Element => {
    if (postProcessResult?.FileResults?.length) {
      return (
        <Grid sm={3} className="TimeTrend-results report">
          <InputLabel className="" id="label-for-dd">
            {postProcessResult?.FileResults?.filter(
              (file) =>
                file.FileName.includes('.html') && file.Type === 'Report'
            ).map((fileResult) => (
              <Link
                to={`/models/reports/${id}/${regressionid}/${fileResult.FileName}`}
                onClick={(): void =>
                  appContext.setPostProcessName &&
                  appContext.setPostProcessName(
                    postProcess?.postProcessName ?? null
                  )
                }
              >
                <span className="TimeTrend-report">{fileResult.Title}</span>
              </Link>
            ))}
          </InputLabel>
        </Grid>
      );
    }
    return <Fragment></Fragment>;
  };

  const getDatasetId = (id: string): void => {
    if (disableButton) return;
    setDatasetId(id);
  };

  const renderEcuation = (): JSX.Element => {
    if (predictedEcuation.length) {
      return (
        <Grid sm={2}>
          <span className={classes.ecuation}>
            Predicted = {predictedEcuation}
          </span>
        </Grid>
      );
    }
    return <></>;
  };

  const renderRsQuaredResult = (name: string[]): string => {
    let resultString = 'To Be Calculated';
    if (postProcessResult?.Results?.length) {
      const result = postProcessResult?.Results[1];
      Object.values(result).forEach((re, index) => {
        if (name.includes(Object.keys(result)[index])) {
          resultString = re;
        }
      });
    }
    return resultString;
  };

  const getSecondaryDataset = (datasetIds: string[]): void => {
    setSecondaryDatasets(datasetIds);
  };

  const onChange = (): void => {
    setDisableButton(false);
  };

  if (
    (!project && !postProcess) ||
    runningRegression ||
    updateRegression ||
    !postProcessCorrect
  )
    return <Loading />;

  return (
    <div>
      <PanelHeader
        route={[
          <Link to="/models" style={{ color: 'black' }}>
            Models
          </Link>,
          <Link to={`/models/view/${id}`} style={{ color: 'black' }}>
            {project?.projectName || 'Model'}
          </Link>,
          <Link
            to={`/models/estimated-market-regression/${id}/${regressionid}`}
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
          changeDataset={onChange}
          role={'LandSchedule'}
          previousModel="Land Model"
          actualModel="EMV Model"
        />
      )}
      <Grid container className="TimeTrend-formWrapper mt-0">
        {renderEcuation()}
        <Grid sm={5} className="TimeTrend-results">
          <InputLabel className="" id="label-for-dd">
            <strong>
              ValuationDate:{' '}
              {moment(project?.assessmentDateTo).format('MM-DD-YYYY')}
            </strong>
          </InputLabel>
          <div className={classes.results}>{renderVariablesResult()}</div>
        </Grid>
        <Grid sm={2} className="TimeTrend-results">
          <InputLabel className="" id="label-for-dd">
            <strong>R-squared = </strong>{' '}
            <span className={'Result-strong'}>
              {renderRsQuaredResult(['RSquared', 'R-Squared'])}
            </span>
          </InputLabel>
          <InputLabel className="" id="label-for-dd">
            <strong>Adjusted R-squared = </strong>{' '}
            <span className={'Result-strong'}>
              {renderRsQuaredResult(['AdjustedRSquared', 'AdjustedR-Squared'])}
            </span>
          </InputLabel>
        </Grid>
        {renderReportSection()}
      </Grid>
      <div className="TimeTrend-grid withBorder">
        <GenericGrid
          rowData={rowData}
          saveColData={saveColData}
          useDropdown={true}
          updateColData={updateColData}
          badExpressions={errorMessage?.reason}
          lockPrecommitExpressions={lockPrecommitExpressions}
        />
        <ErrorMessage message={errorMessage} />
      </div>
      <div className="TimeTrend-maingrid">{renderGrid()}</div>
    </div>
  );
};
export default ViewMultipleRegression;
