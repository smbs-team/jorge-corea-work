/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { PlayArrowOutlined } from '@material-ui/icons';
import Loading from 'components/Loading';
import { ProjectContext } from 'context/ProjectsContext';
import React, { Fragment, useContext, useEffect, useState } from 'react';
import { useHistory, useParams } from 'react-router-dom';
import ModelDetailsHeader from 'routes/models/ModelDetailsHeader';
import { ProjectDataset } from 'services/map.typings';
import { createSetupLandModel } from '../Projects/Land/services/landServices';
import { LoadProject } from './common';
import { SalesDropdown } from './SalesDropdown';
import _ from 'underscore';
import AgGridService from 'services/AgGridService';
import { AppContext } from 'context/AppContext';

let debounced: (() => Promise<void>) & _.Cancelable;

const SetupLandModelPage = (): JSX.Element => {
  const { id }: { id: string } = useParams();
  const [datasetId, setDatasetId] = useState<string>('');
  const [datasets, setDatasets] = useState<ProjectDataset[]>([]);
  const [secondaryDatasets, setSecondaryDatasets] = useState<string[]>([]);
  const [jobId, setJobId] = useState<number>(0);
  const [runningSetup, setRunningSetup] = useState<boolean>(false);
  const [manualChange, setManualChange] = useState<boolean>(false);
  const context = useContext(ProjectContext);
  const appContext = useContext(AppContext);
  const history = useHistory();

  //eslint-disable-next-line
  const callFetch = () => {
    fetchData();
    return (): void => {
      debounced?.cancel();
    };
  };
  useEffect(callFetch, []);

  useEffect(() => {
    if (jobId) {
      validateJobId();
    }
    //eslint-disable-next-line
  }, [jobId]);

  const validateJobId = async (): Promise<void> => {
    try {
      const jobStatus = await AgGridService.getJobStatus(jobId);
      const status = jobStatus?.jobStatus === 'Finished';
      if (!status) {
        debounced = _.debounce(validateJobId, 5000);
        debounced();
      } else {
        history.push(`/models/new-land-model/${id}`);
      }
    } catch (error) {
      console.log(error);
      setRunningSetup(false);
    }
  };

  useEffect(() => {
    const validateDatasetSelected = (): void => {
      if (!datasetId.length) return;
      if (!manualChange) return;
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
  }, [datasetId, manualChange]);

  const fetchData = async (): Promise<void> => {
    const project = await LoadProject(id);
    if (project) {
      const datasets = (project.projectDatasets ?? []) as ProjectDataset[];
      if (datasets) {
        setDatasets(datasets);
        let datasetId = datasets.find(
          (dt) => dt.datasetRole === 'Sales'
        )?.datasetId;
        let secondaryDatasets: string[] = [];
        datasets.forEach((d: ProjectDataset) => {
          const pp = d.dataset.dependencies?.postProcesses?.find(
            (p) =>
              p.postProcessRole === 'TimeTrendRegression' &&
              p.primaryDatasetPostProcessId === null
          );
          if (pp) {
            datasetId = pp.datasetId;
            secondaryDatasets = pp?.secondaryDatasets ?? [];
          }
        });
        if (datasetId) {
          setDatasetId(datasetId);
        }
        if (secondaryDatasets.length) {
          setSecondaryDatasets(secondaryDatasets);
        } else {
          if (
            datasets.find(
              (dt) =>
                dt.datasetRole === 'Population' && dt.datasetId === datasetId
            )
          ) {
            setSecondaryDatasets([]);
          } else {
            const secondaryDatasetsDefault = datasets
              .filter((dt) => dt.datasetRole === 'Population')
              .map((dt) => dt.datasetId) as string[];
            setSecondaryDatasets(secondaryDatasetsDefault);
          }
        }
      }
    }
  };

  const createLandModel = async (): Promise<void> => {
    try {
      setRunningSetup(true);
      if (datasetId) {
        const landModelSetup = await createSetupLandModel(
          datasetId,
          secondaryDatasets
        );
        if (landModelSetup) {
          if (landModelSetup?.jobId) {
            setJobId(landModelSetup.jobId);
          } else {
            history.push(`/models/new-land-model/${id}`);
          }
        }
      }
    } catch (error) {
      setRunningSetup(false);
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
    }
  };

  const defaultIcons = [
    {
      icon: <PlayArrowOutlined />,
      text: 'Create land model',
      onClick: (): Promise<void> => {
        return createLandModel();
      },
    },
  ];

  const getDatasetId = (datasetId: string): void => {
    setDatasetId(datasetId);
  };

  const getSecondaryDataset = (datasets: string[]): void => {
    setSecondaryDatasets(datasets);
  };

  const changePrimaryDataset = (): void => {
    setManualChange(true);
  };

  if (!datasets.length || !context.modelDetails || runningSetup)
    return <Loading />;

  return (
    <Fragment>
      <ModelDetailsHeader
        modelDetails={context.modelDetails}
        userDetails={context.userDetails}
        icons={defaultIcons}
        links={[<span>Land Model</span>]}
      />
      {datasets.length && (
        <SalesDropdown
          options={datasets}
          changeDataset={changePrimaryDataset}
          datasetId={datasetId}
          getDatasetId={getDatasetId}
          selectedValues={secondaryDatasets}
          getSecondaryDataset={getSecondaryDataset}
          role={'TimeTrendRegression'}
          previousModel="Time Trend Model"
          actualModel="Land Model"
        />
      )}
    </Fragment>
  );
};

export default SetupLandModelPage;
