// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect, useState } from 'react';
import Loading from 'components/Loading';
import { PanelHeader } from '@ptas/react-ui-library';
import {
  GenericGridRowData,
  Project,
  ProjectDataset,
  SearchParameters,
  IdOnly,
} from 'services/map.typings';
import { Link, useHistory, useParams } from 'react-router-dom';
import {
  getCalculatedVars,
  getDependentVars,
  getExpressionVars,
  getIndependentVars,
  LoadProject,
} from '../../Regression/common';
import LinearRegression from '../../Regression/LinearRegression';
import { AxiosLoader } from 'services/AxiosLoader';
import { PlayArrowOutlined } from '@material-ui/icons';
import { AppContext } from 'context/AppContext';
import { SalesDropdown } from '../../Regression/SalesDropdown';
import { ImportErrorMessageType } from 'components/ErrorMessage/ErrorMessage';
import { RunningRegression } from './RunningRegression';
import { RegressionGrid } from '../../Regression/RegressionGrid';

/**
 * Land
 *
 * @param props - Component props
 * @returns A JSX element
 */

interface UserInfo {
  email: string;
  fullName: string;
  id: string;
  roles: unknown;
  teams: unknown;
}

const ViewLandPage = (): JSX.Element => {
  const { id }: { id: string } = useParams();
  const history = useHistory();
  const appContext = useContext(AppContext);
  const [project, setProjectInfo] = useState<Project | null>(null);
  const [userInfo, setUserInfo] = useState<UserInfo[]>();
  const [datasetId, setDatasetId] = useState<string>('');
  const [regression, setRegression] = useState<number>(0);
  const [gridData, setGridData] = useState<GenericGridRowData[]>([]);
  const [errorMessage, setErrorMessage] = useState<ImportErrorMessageType>();
  const [isDatasetChanged, setChangeDataset] = useState<boolean>(false);
  const [expressionPayload, setExpressionsPayload] = useState<
    GenericGridRowData[]
  >([]);
  const [datasets, setDatasets] = useState<ProjectDataset[]>([]);
  const [secondaryDatasets, setSecondaryDatasets] = useState<string[]>([]);

  const [loading, setLoading] = useState<boolean>(false);
  const [runningRegression, setRunningRegression] = useState<boolean>(false);

  useEffect(() => {
    const fetchData = async (): Promise<void> => {
      const project = await LoadProject(id, true);
      if (project) {
        setProjectInfo(project.project);
        const datasets = project.project.projectDatasets;
        setDatasets(project.project.projectDatasets);
        const dataset = datasets.find(
          (d: ProjectDataset) => d.datasetRole.toLowerCase() === 'sales'
        );
        if (!dataset) return;
        setDatasetId(dataset.datasetId);
        setSecondaryDatasets([]);
        setUserInfo(project.usersDetails);
      }
    };
    fetchData();
  }, [id]);

  useEffect(() => {
    const validateDatasetSelected = (): void => {
      setSecondaryDatasets([]);
    };
    validateDatasetSelected();
    //eslint-disable-next-line
  }, [datasetId, isDatasetChanged]);

  const getDatasetId = (id: string): void => {
    setDatasetId(id);
  };

  const runRegression = async (): Promise<void> => {
    if (runningRegression) return;
    if (!gridData) {
      appContext.setSnackBar &&
        appContext.setSnackBar({
          text: 'No form data.',
          severity: 'warning',
        });
      return;
    }

    setRunningRegression(true);

    const priorVars = getDependentVars(gridData);
    const postVars = getIndependentVars(gridData);
    const calculatedVars = getCalculatedVars(gridData);
    const expressionVars = getExpressionVars(expressionPayload);

    const customSearchExpressions = [
      ...priorVars,
      ...postVars,
      ...calculatedVars,
      ...expressionVars,
    ];

    const payload = {
      datasetId: datasetId,
      postProcessName: 'Land Schedule',
      priority: 2000,
      rScriptModelId: regression,
      postProcessDefinition: 'RScript post process test',
      customSearchExpressions: customSearchExpressions,
      postProcessRole: 'LandRegression',
      secondaryDatasets: secondaryDatasets,
    };
    try {
      setLoading(true);
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
      appContext.handleJobId &&
        appContext.handleJobId(parseInt(`${executePost?.id}`));
      appContext?.handlePostProcessInProgress?.({
        jobId: parseInt(`${executePost?.id}`),
        postProcessId: parseInt(postProcessInfo?.id ?? '0'),
        datasetId: datasetId,
        postProcessRole: 'landregression',
      });
      history.push({
        pathname: `/models/view-land-model/${id}/edit/${postProcessInfo?.id}`,
        state: {
          jobId: executePost?.id,
          from: 'create',
        },
      });
      // history.push(`/models/new-land-model/${id}`);
    } catch (e) {
      setErrorMessage({
        message: e.message ?? e,
        reason: e.validationError ?? [],
      });
    } finally {
      setRunningRegression(false);
      setLoading(false);
    }
  };

  const defaultIcons = [
    {
      icon: <PlayArrowOutlined />,
      text: 'Run Regression',
      onClick: (): Promise<void> => {
        return runRegression();
      },
    },
  ];

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

  const getFormData = (
    regression: number,
    gridData: GenericGridRowData[],
    expressionPayload: GenericGridRowData[],
    _params: SearchParameters[] | undefined
  ): void => {
    setRegression(regression);
    setGridData(gridData);
    setExpressionsPayload(expressionPayload);
  };

  const renderGrid = (): JSX.Element => {
    if (project && datasetId.length) {
      return (
        <RegressionGrid
          datasets={datasets}
          datasetId={datasetId}
          // reloadGrid={datasetId}
        />
      );
    }
    return <Fragment></Fragment>;
  };

  if (!project || loading) return <Loading />;

  const getSecondaryDataset = (datasets: string[]): void => {
    setSecondaryDatasets(datasets);
  };

  const onChange = (): void => {
    setChangeDataset(true);
  };

  return (
    <Fragment>
      <RunningRegression runningRegression={loading} />
      <PanelHeader
        route={[
          <Link to="/models" style={{ color: 'black' }}>
            Models
          </Link>,
          <Link to={`/models/view/${id}`} style={{ color: 'black' }}>
            {project?.projectName}
          </Link>,
          <Link to={`/models/new-land-model/${id}`} style={{ color: 'black' }}>
            Land Model
          </Link>,
          <span>Land Regression</span>,
        ]}
        icons={defaultIcons}
        // detailTop={getDetailTop()}
        detailBottom={getBottom()}
      />
      {datasets.length && (
        <SalesDropdown
          unlockPopulation
          datasetId={datasetId}
          options={datasets}
          getDatasetId={getDatasetId}
          getSecondaryDataset={getSecondaryDataset}
          changeDataset={onChange}
          selectedValues={secondaryDatasets}
        />
      )}
      <LinearRegression
        modelDetails={project}
        getFormData={getFormData}
        errorMessage={errorMessage}
        useLandRegression
      />
      <div className="TimeTrend-maingrid">{renderGrid()}</div>
    </Fragment>
  );
};

export default ViewLandPage;
