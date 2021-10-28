// AddRegression.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect, useState } from 'react';
import ModelDetailsHeader from 'routes/models/ModelDetailsHeader';

import PlayArrowOutlined from '@material-ui/icons/PlayArrowOutlined';
import { ProjectContext } from 'context/ProjectsContext';
import Loading from 'components/Loading';
import LinearRegression from './LinearRegression';
import {
  GenericGridRowData,
  Project,
  ProjectDataset,
  RegressionRoleType,
  SearchParameters,
} from 'services/map.typings';
import { useHistory, useParams } from 'react-router-dom';
import {
  runImportRScript,
  getTimeTrendCustomSearchExpressions,
} from './common';

import '../../../../assets/time-trend-styles/styles.scss';
import { AppContext } from 'context/AppContext';
import { SalesDropdown } from './SalesDropdown';
import { executeDatasetPostProcess } from '../Projects/Land/services/landServices';
import { getUserProject } from 'services/common';
import { ImportErrorMessageType } from 'components/ErrorMessage/ErrorMessage';
import { RunningRegression } from '../Projects/Land/RunningRegression';
import { RegressionGrid } from './RegressionGrid';

const AddRegression = (): JSX.Element => {
  const { id }: { id: string } = useParams();
  const context = useContext(ProjectContext);
  const appContext = useContext(AppContext);
  const [project, setProjectInfo] = useState<Project | null | undefined>(null);
  const [runningRegression, setRunningRegression] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<ImportErrorMessageType>();
  const [regression, setRegression] = useState<number>(0);
  const [gridData, setGridData] = useState<GenericGridRowData[]>([]);
  const [datasetId, setDatasetId] = useState<string>('');
  const [datasets, setDatasets] = useState<ProjectDataset[]>([]);
  const [secondaryDatasets, setSecondaryDatasets] = useState<string[]>([]);
  const [parameterState, setParameterState] = useState<
    SearchParameters[] | undefined
  >([]);
  const [expressionPayload, setExpressionsPayload] = useState<
    GenericGridRowData[]
  >([]);

  const history = useHistory();

  useEffect(() => {
    const fetchData = async (): Promise<void> => {
      try {
        const response = await getUserProject(id);
        setProjectInfo(response?.project);
        const d = response?.project?.projectDatasets.find(
          (ds: ProjectDataset) => ds.datasetRole === 'Sales'
        )?.datasetId;

        const datasets = response?.project?.projectDatasets || [];

        setDatasets(datasets);

        if (!d) return;

        setDatasetId(d);
        if (!datasets.length) return;
        setSecondaryDatasets(
          datasets
            .filter((d) => d.datasetRole === 'Population')
            .map((d) => d.datasetId)
        );
      } catch (error) {
        appContext.setSnackBar &&
          appContext.setSnackBar({
            severity: 'error',
            text: 'Getting user project failed',
          });
      }
    };
    fetchData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  useEffect(() => {
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
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [datasetId]);

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
      secondaryDatasets: secondaryDatasets,
      postProcessRole: RegressionRoleType.TimeTrendRegression,
    };
    try {
      const regression = await runImportRScript(payload);
      const executePost = await executeDatasetPostProcess(
        `${payload.datasetId}`,
        `${regression?.id}`
      );
      appContext.handleJobId?.(parseInt(`${executePost?.id}`));
      if (executePost?.id && regression?.id) {
        appContext?.handlePostProcessInProgress?.({
          jobId: parseInt(`${executePost?.id}`),
          postProcessId: parseInt(`${regression?.id}`),
          datasetId: datasetId,
          postProcessRole: RegressionRoleType.TimeTrendRegression.toLowerCase(),
        });
      }
      history.push({
        pathname: `/models/regression/${id}/${regression?.id}`,
        state: {
          jobId: executePost?.id,
        },
      });
    } catch (e) {
      setErrorMessage({
        message: e?.message ?? e,
        reason: e?.validationError ?? [],
      });
      setRunningRegression(false);
    }
  };

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

  const defaultIcons = [
    {
      icon: <PlayArrowOutlined />,
      text: 'Run Regression',
      onClick: (): Promise<void> => {
        return runRegression();
      },
    },
  ];

  const renderGrid = (): JSX.Element => {
    if (project && datasetId.length) {
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
    return <Fragment />;
  };

  const getDatasetId = (datasetId: string): void => {
    setDatasetId(datasetId);
  };
  if ((!project && !datasetId.length) || !context.modelDetails)
    return <Loading />;

  const getSecondaryDataset = (datasetIds: string[]): void => {
    setSecondaryDatasets(datasetIds);
  };

  return (
    <Fragment>
      <RunningRegression runningRegression={runningRegression} />
      <ModelDetailsHeader
        modelDetails={context.modelDetails}
        userDetails={context.userDetails}
        icons={defaultIcons}
        links={[<span>Time Trend</span>]}
      />
      {datasets.length && (
        <SalesDropdown
          datasetId={datasetId}
          selectedValues={secondaryDatasets}
          options={datasets}
          getDatasetId={getDatasetId}
          getSecondaryDataset={getSecondaryDataset}
        />
      )}
      <LinearRegression
        modelDetails={context.modelDetails}
        getFormData={getFormData}
        regressionType="Time Trend:"
        errorMessage={errorMessage}
        // useDropdown={true}
      />
      <div className="TimeTrend-maingrid">{renderGrid()}</div>
    </Fragment>
  );
};
export default AddRegression;
