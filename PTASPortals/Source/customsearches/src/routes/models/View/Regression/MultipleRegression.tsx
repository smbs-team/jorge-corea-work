// MultipleRegression.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect, useState } from 'react';
import ModelDetailsHeader from 'routes/models/ModelDetailsHeader';

import PlayArrowOutlined from '@material-ui/icons/PlayArrowOutlined';
import { ProjectContext } from 'context/ProjectsContext';
import Loading from 'components/Loading';
import {
  GenericGridRowData,
  IdOnly,
  Project,
  ProjectDataset,
  // RegressionRoleType,
} from 'services/map.typings';
import { useHistory, useParams } from 'react-router-dom';
import {
  getDependentVars,
  getIndependentVars,
  getCalculatedVars,
  fetchMultipleRegressionExpressions,
  getHidenVars,
} from './common';
import { AxiosLoader } from 'services/AxiosLoader';
import '../../../../assets/time-trend-styles/styles.scss';
import GenericGrid from 'components/GenericGrid/GenericGrid';
import { getUserProject } from 'services/common';
import { AppContext } from 'context/AppContext';
import { SalesDropdown } from './SalesDropdown';
import ErrorMessage, {
  ImportErrorMessageType,
} from 'components/ErrorMessage/ErrorMessage';
import { RegressionGrid } from './RegressionGrid';

const MultipleRegression = (): JSX.Element => {
  const { id }: { id: string } = useParams();
  const context = useContext(ProjectContext);
  const appContext = useContext(AppContext);
  const [project, setProjectInfo] = useState<Project | null | undefined>(null);
  const [runningRegression, setRunningRegression] = useState<boolean>(false);
  const [regression, setRegression] = useState<number>(0);
  const [gridData, setGridData] = useState<GenericGridRowData[]>([]);
  const [datasetId, setDatasetId] = useState<string>('');
  const [datasets, setDatasets] = useState<ProjectDataset[]>([]);
  const [rowData, setRowData] = useState<GenericGridRowData[]>([]);
  const [errorMessage, setErrorMessage] = useState<ImportErrorMessageType>();
  const [secondaryDatasets, setSecondaryDatasets] = useState<string[]>([]);
  const [isDatasetChanged, setChangeDataset] = useState<boolean>(false);
  const [lockPrecommitExpressions, setLockPrecommitExpressions] =
    useState(false);
  // const [expressionPayload, setExpressionPayload] = useState<
  //   GenericGridRowData[]
  // >([]);

  const history = useHistory();

  const fetchExpressions = (): void => {
    getData();
  };

  useEffect(fetchExpressions, []);

  const fetchProject = (): void => {
    fetchUserProject();
  };

  useEffect(fetchProject, [id]);

  const fetchUserProject = async (): Promise<void> => {
    try {
      const response = await getUserProject(id);
      setProjectInfo(response?.project);
      let d = '';
      let secondaryDatasets: string[] = [];
      response?.project?.projectDatasets.forEach((ds: ProjectDataset) => {
        const pp = ds.dataset.dependencies.postProcesses?.find(
          (pp) =>
            2600 === pp.priority && pp.primaryDatasetPostProcessId === null
        );
        if (pp) {
          d = pp.datasetId;
          if (pp.secondaryDatasets)
            secondaryDatasets = [...pp.secondaryDatasets];
        }
      });

      const datasets = response?.project?.projectDatasets || [];

      setDatasets(datasets);
      setSecondaryDatasets(secondaryDatasets);
      if (!d) {
        appContext.setSnackBar &&
          appContext.setSnackBar({
            severity: 'error',
            text: 'This model does not have a sales dataset.',
          });
        return;
      }
      setDatasetId(d);
    } catch (error) {
      appContext.setSnackBar &&
        appContext.setSnackBar({
          severity: 'error',
          text: 'Getting user project failed',
        });
    }
  };

  useEffect(() => {
    const validateDatasetSelected = (): void => {
      if (!datasetId.length) return;
      if (!isDatasetChanged) return;
      if (
        datasets.some(
          (d) => d.datasetRole === 'Population' && d.datasetId === datasetId
        )
      ) {
        setSecondaryDatasets([]);
      } else {
        setSecondaryDatasets(
          datasets
            .filter((d) => d.datasetRole === 'Population')
            .map((d) => d.datasetId)
        );
      }
    };
    validateDatasetSelected();
    //eslint-disable-next-line
  }, [datasetId, isDatasetChanged]);

  const getDatasetId = (id: string): void => {
    if (!isDatasetChanged) return;
    setDatasetId(id);
  };

  const getData = async (): Promise<void> => {
    const regressionInfo = await fetchMultipleRegressionExpressions();
    if (regressionInfo?.lockPrecommitExpressions !== undefined) {
      setLockPrecommitExpressions(regressionInfo?.lockPrecommitExpressions);
    }
    if (regressionInfo?.data?.rowData) {
      setRowData(regressionInfo?.data?.rowData);
    }
    // if (regressionInfo?.data?.expressionPayload) {
    //   setExpressionPayload(regressionInfo?.data?.expressionPayload);
    // }
    if (regressionInfo?.regression) {
      setRegression(regressionInfo?.regression);
    }
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

    const priorVars = getDependentVars(gridData);
    const postVars = getIndependentVars(gridData);
    const calculatedVars = getCalculatedVars(gridData);
    const expressionVars = getHidenVars(priorVars, postVars);
    setRowData(gridData);
    // const expressionVars = getExpressionVars(expressionPayload);

    if (priorVars.length === 0) {
      appContext.setSnackBar &&
        appContext.setSnackBar({
          text: 'There need to be 1 dependent variable only to run regression',
          severity: 'warning',
        });
      return;
    }

    if (postVars.length === 0) {
      appContext.setSnackBar &&
        appContext.setSnackBar({
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
    setRunningRegression(true);

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
      appContext.handleJobId?.(parseInt(`${executePost?.id}`));
      appContext?.handlePostProcessInProgress?.({
        jobId: parseInt(`${executePost?.id}`),
        postProcessId: parseInt(postProcessInfo?.id ?? '0'),
        datasetId: datasetId,
        postProcessRole: 'multipleregression',
      });
      history.push({
        pathname: `/models/estimated_market_regression/${id}/${postProcessInfo?.id}`,
        state: {
          jobId: executePost?.id,
        },
      });
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
      text: 'Run Regression',
      onClick: (): Promise<void> => {
        return runRegression();
      },
    },
  ];

  if (
    !context.modelDetails ||
    (!project && !datasetId.length) ||
    runningRegression
  )
    return <Loading />;

  const saveColData = (): boolean => {
    return true;
  };

  const updateColData = (newData: GenericGridRowData[]): void => {
    setGridData(newData);
  };

  const renderGrid = (): JSX.Element => {
    if (project) {
      if (datasetId.length) {
        return (
          <RegressionGrid
            datasets={datasets?.filter((s) =>
              [datasetId, ...secondaryDatasets].includes(s.datasetId)
            )}
            datasetId={datasetId}
            // reloadGrid={datasetId}
          />
        );
      }
    }
    return <Fragment></Fragment>;
  };

  const onChange = (): void => {
    setChangeDataset(true);
  };

  const getSecondaryDataset = (datasetIds: string[]): void => {
    setSecondaryDatasets(datasetIds);
  };

  return (
    <div>
      <ModelDetailsHeader
        modelDetails={context.modelDetails}
        userDetails={context.userDetails}
        icons={defaultIcons}
        links={[<span>New Regression</span>]}
      />
      {datasets.length && (
        <SalesDropdown
          datasetId={datasetId}
          options={datasets}
          selectedValues={secondaryDatasets}
          getDatasetId={getDatasetId}
          getSecondaryDataset={getSecondaryDataset}
          changeDataset={onChange}
          role={'LandSchedule'}
          previousModel="Land Model"
          actualModel="EMV Model"
        />
      )}
      <div className="TimeTrend-grid withBorder mt-0">
        <GenericGrid
          rowData={rowData}
          saveColData={saveColData}
          updateColData={updateColData}
          useDropdown={true}
          badExpressions={errorMessage?.reason}
          lockPrecommitExpressions={lockPrecommitExpressions}
        />
        <ErrorMessage message={errorMessage} />
      </div>
      <div className="TimeTrend-maingrid">{renderGrid()}</div>
    </div>
  );
};
export default MultipleRegression;
