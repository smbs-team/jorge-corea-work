// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect, useState } from 'react';
import Project from './Projects';
import CustomHeader from 'components/common/CustomHeader';
import { Link, useHistory, useLocation, useParams } from 'react-router-dom';
import { ProjectContext } from 'context/ProjectsContext';
import { ErrorMessageContext, IconToolBarItem } from '@ptas/react-ui-library';
import SystemUpdateAltIcon from '@material-ui/icons/SystemUpdateAlt';
import { AppContext } from 'context/AppContext';
import useSignalR from 'components/common/useSignalR';
import RefreshIcon from '@material-ui/icons/Refresh';
import { AxiosLoader } from 'services/AxiosLoader';
import AgGridService from 'services/AgGridService';
import Loading from 'components/Loading';
import RestartApplyDialog from 'components/DatasetPage/RestartApplyDialog';
import _ from 'underscore';
import 'assets/model-styles.scss';

interface RefreshDatasetType {
  jobIds: number[];
  projectId: number;
  queuedDatasets: string[];
}

interface JobInfo {
  jobId: number;
  datasetId: string;
}

let debounced: (() => Promise<void>) & _.Cancelable;

/**
 * ViewProject
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ViewProject(): JSX.Element {
  const context = useContext(ProjectContext);
  const { setRouteFrom, jobId, setSnackBar } = useContext(AppContext);
  const location = useLocation();
  const history = useHistory();
  const { message } = useSignalR(parseInt(`${jobId}`));
  const { id }: { id: string } = useParams();
  const [runningRefresh, setRunningRefresh] = useState<boolean>(false);
  const [applyJobId, setApplyJobId] = useState<number>(0);
  const [applyDatasetId, setApplyDatasetId] = useState<string>('');
  const [runningApplyModel, setRunningApplyModel] = useState<boolean>(false);
  const [runningLandModel, setRunningLandModel] = useState<boolean>(false);
  const [openModal, setOpenModal] = useState<boolean>(false);
  const [versionType, setVersionType] = useState<string>('');
  const { showErrorMessage } = useContext(ErrorMessageContext);
  useEffect(() => {
    if (message?.jobStatus === 'Succeeded') {
      // window.location.reload();
      history.go(0);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [message]);

  useEffect(() => {
    if (!applyJobId && !applyDatasetId) return;
    getJobStatus();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [applyJobId, applyDatasetId]);

  useEffect(() => {
    setRouteFrom && setRouteFrom(location.pathname);
    return (): void => {
      debounced?.cancel();
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (!context.modelDetails) return;
    if (context.modelDetails.versionType === 'Frozen')
      return setVersionType('Frozen');
    setVersionType(context.modelDetails.versionType ?? '');
  }, [context.modelDetails]);

  const getBackground = (): string => {
    if (context.modelDetails?.versionType?.toLowerCase() === 'Frozen')
      return 'frozen';
    return context.modelDetails?.versionType?.toLowerCase() ?? '';
  };

  const refreshDataset = async (): Promise<RefreshDatasetType | null> => {
    const loader = new AxiosLoader<RefreshDatasetType, {}>();
    const url = `CustomSearches/RefreshProjectDatasets/${id}`;
    const result = await loader.PutInfo(url, {}, {});
    return result;
  };

  const handleRefresh = async (): Promise<void> => {
    try {
      setRunningRefresh(true);
      const value = await refreshDataset();
      const jobInfo = value?.jobIds.map<JobInfo>((id, index) => ({
        jobId: id,
        datasetId: value.queuedDatasets[index],
      }));
      context.setJobIds?.(jobInfo ?? []);
    } catch (error) {
      setSnackBar?.({
        severity: 'error',
        text: error ?? 'Refresh all datasets has been failed',
      });
    } finally {
      setRunningRefresh(false);
    }
  };

  const getJobStatus = async (): Promise<void> => {
    try {
      const jobStatus = await AgGridService.getJobStatus(applyJobId);
      const status = jobStatus?.jobStatus === 'Finished';
      // setRunningApplyModel(status);
      if (jobStatus?.jobStatus === 'Failed') {
        showErrorMessage(
          {
            errorDesc: JSON.stringify({
              reason: jobStatus.jobResult.Reason,
              status: 'Failed',
            }),
          },
          'CS'
        );
        if (applyDatasetId) {
          setTimeout(() => {
            history.push({
              pathname: `/models/results/${id}/${applyDatasetId}`,
              state: {
                from: 'applyModel',
                projectId: id,
              },
            });
          }, 1000);
        }
      } else {
        if (!status) {
          debounced = _.debounce(getJobStatus, 5000);
          debounced();
        } else {
          debounced?.cancel();
          if (jobStatus?.jobResult?.Status === 'Success') {
            if (applyDatasetId) {
              setTimeout(() => {
                history.push({
                  pathname: `/models/results/${id}/${applyDatasetId}`,
                  state: {
                    from: 'applyModel',
                    projectId: id,
                  },
                });
              }, 1000);
            }
          } else {
            setSnackBar?.({
              text: jobStatus?.jobResult?.Reason ?? '',
              severity: 'error',
            });
            setRunningApplyModel(false);
          }
        }
      }
    } catch (error) {
      setRunningApplyModel(false);
      console.log(error);
    }
  };

  const executeApplyModel = async (): Promise<void> => {
    try {
      const loader = new AxiosLoader<JobInfo, {}>();
      const url = `CustomSearches/ApplyModel/${id}`;
      const result = await loader.PutInfo(url, {}, {});
      if (result) {
        setApplyJobId(result.jobId);
        setApplyDatasetId(result.datasetId);
        setRunningApplyModel(true);
      }
    } catch (error) {
      setSnackBar?.({
        severity: 'error',
        text: error,
      });
      setRunningApplyModel(false);
    }
  };

  const handleApplyModel = (): void => {
    executeApplyModel();
  };

  const applyOrRefreshModel = (): void => {
    if (context.withNoBulkUpdate) {
      setOpenModal(true);
      return;
    }
    if (context.bulkSuccess) {
      setOpenModal(true);
      return;
    }
    handleApplyModel();
  };

  const icons: IconToolBarItem[] = [
    {
      icon: <SystemUpdateAltIcon />,
      text:
        context.bulkSuccess || context.withNoBulkUpdate
          ? 'Restart Apply'
          : 'Apply Model',
      disabled: context.modelDetails?.isLocked,
      onClick: applyOrRefreshModel,
    },
    {
      icon: <RefreshIcon />,
      text: 'Refresh All Datasets',
      disabled: context.modelDetails?.isLocked,
      onClick: handleRefresh,
    },
  ];

  if (runningApplyModel) return <Loading />;

  const handleRunningLandModel = (): void => {
    setRunningLandModel(true);
  };

  return (
    <Fragment>
      {!runningLandModel && (
        <CustomHeader
          route={[
            <Link to="/models" style={{ color: 'black' }}>
              Models
            </Link>,
            <span>{context.projectName}</span>,
          ]}
          key={`${context.bulkSuccess?.datasetPostProcessId}`}
          icons={icons}
          detailTop={context.headerDetails?.bottom}
          detailBottom={context.headerDetails?.top}
          classes={{
            root: `VersionTypeBg-${getBackground()}`,
          }}
        />
      )}
      <Project
        versionType={versionType}
        refreshLock={runningRefresh}
        isLocked={context?.modelDetails?.isLocked ?? false}
        runningLandModel={handleRunningLandModel}
      />
      <RestartApplyDialog
        title={
          context.withNoBulkUpdate
            ? 'If you reapply the model any current changes done to the apply model data set will be lost, are you sure you want to continue?'
            : ''
        }
        open={openModal}
        toggle={(): void => setOpenModal(false)}
        accept={(): void => {
          try {
            handleApplyModel();
            setOpenModal(false);
          } catch (error) {
            console.log(`error`, error);
          }
        }}
      />
    </Fragment>
  );
}

export default ViewProject;
