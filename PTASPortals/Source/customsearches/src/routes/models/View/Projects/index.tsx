// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect, useState } from 'react';
import CustomSection, { Card, SyncDetails } from './common/CustomSection';
import ProjectDetails from './common/ProjectDetails';
import { ProjectContext } from 'context/ProjectsContext';
import { useHistory, useParams } from 'react-router-dom';
import BlockUi from 'react-block-ui';
// import { useMount } from 'react-use';
// import HealthModal from './Land/HealthModal';
import { executeDatasetPostProcess } from './Land/services/landServices';
import AgGridService from 'services/AgGridService';
import _ from 'underscore';
import { useLocation } from 'react-router';
import ErrorMessage, {
  ImportErrorMessageType,
} from 'components/ErrorMessage/ErrorMessage';
import { ErrorMessageContext } from '@ptas/react-ui-library';
import { AppContext } from 'context/AppContext';
import { DatasetHealthDataType } from 'services/map.typings';

interface ProjectProps {
  refreshLock: boolean;
  isLocked: boolean;
  runningLandModel: () => void;
  versionType: string;
}
interface ErrorMessageType {
  label: string;
  message: ImportErrorMessageType;
}

/**
 * Project
 *
 * @param props - Component props
 * @returns A JSX element
 */

let debounced: (() => Promise<void>) & _.Cancelable;

function Project(props: ProjectProps): JSX.Element {
  const context = useContext(ProjectContext);
  const appContext = useContext(AppContext);
  const [errorDatasetId, setErrorDatasetId] = useState<string>('');
  const { showErrorMessage } = useContext(ErrorMessageContext);
  const { id }: { id: string } = useParams();
  //eslint-disable-next-line
  const [lockedPostProcess, setLockedPostProcess] = useState<string>('');
  const history = useHistory();
  const [isLocked, setIsLocked] = useState<boolean>(false);
  const [dataSourceCards, setDataSourceCards] = useState<Card[]>([]);
  const [healtInfo, setHealtInfo] = useState<
    DatasetHealthDataType[] | undefined
  >([]);
  const [disabledButtons, setDisabledButtons] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<ErrorMessageType>();
  const [jobId, setJobId] = useState<number>();
  const [isHealth, setIsHealth] = useState<boolean>(false);
  const location = useLocation();

  useEffect(() => {
    const isProcessing = healtInfo?.some((h) => h.isProcessing);
    setDisabledButtons(isProcessing ?? false);

    if (!context.modelDetails) return;
    healtInfo?.forEach((info) => {
      const dataset = context.modelDetails?.projectDatasets.find(
        (pd) => pd.datasetId === info.datasetId
      );
      dataset?.dataset.dependencies.postProcesses?.forEach((pp) => {
        const ppFound = info.postProcessHealthData.find(
          (pphd) => pphd.datasetPostProcessId === pp.datasetPostProcessId
        );
        if (ppFound) {
          setErrorMessage({
            label: `${pp.postProcessRole}`,
            message: {
              message:
                ppFound.postProcessHealthMessage ??
                JSON.parse(pp.resultPayload)?.Reason,
              reason: [],
            },
          });
          showErrorMessage(
            {
              errorDesc: pp.resultPayload,
              onClickReport: () => {
                executeHealth(info.datasetId);
              },
            },
            'CS'
          );
          setErrorDatasetId(info.datasetId);
        }
      });
    });
    //eslint-disable-next-line
  }, [healtInfo, context.modelDetails]);

  // useEffect(() => {
  //   if (sectionError && context.modelDetails) {
  //     const dataset = context.modelDetails?.projectDatasets.find(
  //       (dt) => dt.datasetId === sectionError.datasetId
  //     );
  //     const postProcesses = dataset?.dataset.dependencies.postProcesses;
  //     const pp = postProcesses?.find(
  //       (pp) => pp.datasetPostProcessId === sectionError.postProcessId
  //     );
  //     const payload = pp?.resultPayload;

  //     const reason =
  //       payload && JSON.parse(payload)?.Reason
  //         ? JSON.parse(payload)?.Reason
  //         : sectionError.reason;
  //     setErrorMessage({
  //       label: sectionError.label,
  //       message: {
  //         message: reason,
  //         reason: [],
  //       },
  //     });
  //     showErrorMessage(
  //       {
  //         errorDesc: payload,
  //         onClickReport: () => {
  //           executeHealth(sectionError.datasetId);
  //         },
  //       },
  //       'CS'
  //     );
  //   }
  //   //eslint-disable-next-line
  // }, [sectionError, context.modelDetails]);

  useEffect(() => {
    // setSectionError(appContext.sectionError);
    setHealtInfo(appContext.healtInfo);
    //eslint-disable-next-line
  }, [appContext.healtInfo]);

  const getSyncDetails = (postProcessRole: string): SyncDetails | undefined => {
    return context.getSyncDetails && context.getSyncDetails(postProcessRole);
  };

  const validateBeforePostProcessIsSuccess = (role: string): boolean => {
    let disabled = true;
    context.modelDetails?.projectDatasets?.forEach((pd) => {
      const postProcessFound = pd.dataset.dependencies?.postProcesses?.find(
        (pp) =>
          pp.postProcessRole === role && pp.primaryDatasetPostProcessId === null
      );
      if (postProcessFound) {
        const payload = JSON.parse(postProcessFound.resultPayload);
        if (payload?.Status === 'Success') {
          disabled = false;
        }
      }
    });
    return disabled;
  };

  const validateLandModelPostProcessIsSuccess = (): boolean => {
    let disabled = true;
    context.modelDetails?.projectDatasets?.forEach((pd) => {
      const timeTrendFound = pd.dataset.dependencies?.postProcesses?.find(
        (pp) =>
          pp.postProcessRole === 'TimeTrendRegression' &&
          pp.primaryDatasetPostProcessId === null
      );
      const adjustmentsFound = pd.dataset.dependencies?.postProcesses?.find(
        (pp) =>
          pp.postProcessRole === 'LandAdjustment' &&
          pp.primaryDatasetPostProcessId === null
      );
      if (timeTrendFound && adjustmentsFound) {
        const payloadTimeTrend = JSON.parse(timeTrendFound.resultPayload);
        const payloadAdjustments = JSON.parse(adjustmentsFound.resultPayload);
        if (
          payloadTimeTrend?.Status === 'Success' &&
          payloadAdjustments?.Status === 'Success'
        ) {
          disabled = false;
        }
      }
    });
    return disabled;
  };

  useEffect(() => {
    if (jobId) {
      getJobStatus();
    }
    //eslint-disable-next-line
  }, [jobId]);

  const executeHealth = async (datasetId?: string): Promise<void> => {
    const newDatasetId = datasetId || errorDatasetId;
    try {
      if (newDatasetId) {
        setIsHealth(true);
        const job = await executeDatasetPostProcess(newDatasetId, -1);
        if (job?.id) {
          setJobId(parseInt(job?.id));
        }
      }
    } catch (error) {
      setIsHealth(false);
    }
  };

  const getJobStatus = async (): Promise<void> => {
    try {
      const jobStatus = await AgGridService.getJobStatus(jobId ?? 0);
      const status = jobStatus?.jobStatus === 'Finished';
      if (!status) {
        debounced = _.debounce(getJobStatus, 5000);
        debounced();
      } else {
        setIsLocked(false);
        window.location.href = location.pathname;
      }
    } catch (error) {
      setIsLocked(false);
    }
  };

  useEffect(() => {
    setIsLocked(props.isLocked);
  }, [props.isLocked]);

  useEffect(() => {
    setDataSourceCards(context?.dataSourceCards ?? []);
    // setIsLocked(
    //   !!context.dataSourceCards?.find(
    //     (card) => card.status?.toLowerCase() === 'failed'
    //   )
    // );
    //eslint-disable-next-line
  }, [context.dataSourceCards]);

  useEffect(() => {
    const salesPostProcesses = context.modelDetails?.projectDatasets.find(
      (dataset) => dataset.datasetRole.toLowerCase() === 'sales'
    )?.dataset?.dependencies?.postProcesses;
    const salesPriority = salesPostProcesses?.find(
      (pp) => JSON.parse(pp.resultPayload)?.Status?.toLowerCase() === 'failed'
    )?.priority;
    const populationPostProcesses = context.modelDetails?.projectDatasets.find(
      (dataset) => dataset.datasetRole.toLowerCase() === 'population'
    )?.dataset?.dependencies?.postProcesses;
    const populationPriority = populationPostProcesses?.find(
      (pp) => JSON.parse(pp.resultPayload)?.Status?.toLowerCase() === 'failed'
    )?.priority;

    if (
      salesPriority &&
      [2000, 2100, 2200, 2300, 2400, 2500, 2600].includes(salesPriority)
    ) {
      setLockedPostProcess('LandSchedule');
    }

    switch (salesPriority) {
      case 1000:
        setLockedPostProcess('TimeTrend');
        break;
      case 3000:
        setLockedPostProcess('EMV');
        break;
      case 4000:
        setLockedPostProcess('AppraisalRatio');
        break;
      default:
        break;
    }
    switch (populationPriority) {
      case 2600:
        setLockedPostProcess('SupplementalAndException');
        break;
      case 3000:
        setLockedPostProcess('AnnualUpdateAdjustment');
        break;
      default:
        break;
    }
  }, [context.modelDetails]);

  const renderSection = (): JSX.Element => {
    if (context.projectType?.length === 0) return <Fragment></Fragment>;
    if (context.projectType === 'annual update')
      return (
        <BlockUi
          blocking={getBlocking('AnnualUpdateAdjustment')}
          loader={<></>}
        >
          <CustomSection
            title="Annual update adjustments"
            iconText="New annual update adjustments"
            lastSyncDetails={getSyncDetails('annualupdate')}
            iconOnClick={(): void =>
              history.push(`/models/annual-update/${id}`)
            }
            disableIcon={
              (context.annualCards && context.annualCards.length >= 1) ||
              props.refreshLock ||
              isLocked ||
              disabledButtons
            }
            cards={context.annualCards}
          />
        </BlockUi>
      );
    return (
      <BlockUi
        blocking={getBlocking('SupplementalAndException')}
        loader={<></>}
      >
        <CustomSection
          title="Supplementals"
          iconText="New supplemental"
          lastSyncDetails={getSyncDetails('supplemental')}
          iconOnClick={(): void => history.push(`/models/supplementals/${id}`)}
          disableIcon={
            validateBeforePostProcessIsSuccess('MultipleRegression') ||
            (context.suppCards && context.suppCards.length >= 1) ||
            props.refreshLock ||
            isLocked ||
            disabledButtons
          }
          cards={context.suppCards}
        />
        {errorMessage?.label === 'SupplementalAndException' && (
          <ErrorMessage message={errorMessage?.message} />
        )}
      </BlockUi>
    );
  };

  const getBlocking = (_label: string): boolean => {
    // if (lockedPostProcess) {
    //   return lockedPostProcess !== _label;
    // }
    return false;
  };

  const renderLandSection = (): JSX.Element => {
    if (context?.projectType === 'annual update') return <></>;
    return (
      <BlockUi blocking={getBlocking('LandSchedule')} loader={<></>}>
        <CustomSection
          title="Land model"
          iconText="New land model"
          lastSyncDetails={getSyncDetails('landregression')}
          iconOnClick={(): void => {
            history.push(`/create-land-model/${id}`);
          }}
          disableIcon={
            isLocked ||
            // validateBeforePostProcessIsSuccess('TimeTrendRegression') ||
            (context.landCards &&
              context.landCards.length >= 1 &&
              context.landCards?.some((l) => l.title === 'Land Schedule')) ||
            props.refreshLock ||
            disabledButtons
          }
          cards={context.landCards}
        />
        {errorMessage?.label &&
          [
            'LandSchedule',
            'LandAdjustment',
            'WaterfrontSchedule',
            'NonWaterfrontExpressions',
            'WaterfrontExpressions',
          ].includes(errorMessage?.label) && (
            <ErrorMessage message={errorMessage?.message} />
          )}
      </BlockUi>
    );
  };

  return (
    <Fragment>
      <BlockUi blocking={isHealth} message="In progress">
        <ProjectDetails versionType={props.versionType} />
        <BlockUi blocking={getBlocking('Datasets')} loader={<></>}>
          <CustomSection
            title="Data sources"
            lastSyncDetails={context.dataSourceSyncDetails}
            cards={dataSourceCards}
            noIcons
          />
        </BlockUi>
        <BlockUi blocking={getBlocking('Chart')} loader={<></>}>
          <CustomSection
            title="Analytics"
            iconText="New chart"
            lastSyncDetails={context.chartSyncDetails}
            iconOnClick={(): void => history.push(`/models/new-chart/${id}`)}
            cards={context.chartCards}
            disableIcon={isLocked || props.refreshLock || disabledButtons}
          />
        </BlockUi>
        <BlockUi blocking={getBlocking('TimeTrend')} loader={<></>}>
          <CustomSection
            title="Time trend"
            iconText="New time trend model"
            lastSyncDetails={getSyncDetails('timetrendregression')}
            iconOnClick={(): void => {
              return history.push(`/models/new-regression/${id}`);
            }}
            disableIcon={
              isLocked ||
              (context.timeCards && context.timeCards.length >= 1) ||
              props.refreshLock ||
              disabledButtons
            }
            cards={context.timeCards}
          />
          {errorMessage?.label === 'TimeTrendRegression' && (
            <ErrorMessage message={errorMessage?.message} />
          )}
        </BlockUi>
        {renderLandSection()}
        <BlockUi blocking={getBlocking('EMV')} loader={<></>}>
          <CustomSection
            title="Regression analysis"
            iconText="New EMV model"
            lastSyncDetails={getSyncDetails('multipleregression')}
            iconOnClick={(): void => {
              return history.push(`/models/estimated-market-regression/${id}`);
            }}
            disableIcon={
              isLocked ||
              validateLandModelPostProcessIsSuccess() ||
              (context.multipleCards && context.multipleCards.length >= 1) ||
              props.refreshLock ||
              disabledButtons
            }
            cards={context.multipleCards}
          />
          {errorMessage?.label === 'MultipleRegression' && (
            <ErrorMessage message={errorMessage?.message} />
          )}
        </BlockUi>
        <BlockUi blocking={getBlocking('AppraisalRatio')} loader={<></>}>
          <CustomSection
            title="Appraisal ratios"
            iconText="New appraisal ratios"
            lastSyncDetails={getSyncDetails('appraisalratioreport')}
            iconOnClick={(): void =>
              history.push(`/models/view/${id}/new-appraisal-report`)
            }
            cards={context.appRatioCards}
            disableIcon={props.refreshLock || isLocked || disabledButtons}
          />
        </BlockUi>
        {renderSection()}
      </BlockUi>
      {/* <HealthModal
        loading={runningReExecute}
        isOpen={openHealtModal}
        issue={healtInfo?.issue ?? ''}
        message={healtInfo?.message ?? ''}
        onClose={(): void => setOpenHealtModal(false)}
        execute={executeHealth}
      /> */}
    </Fragment>
  );
}

export default Project;
