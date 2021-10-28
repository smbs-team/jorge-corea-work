// ProjectContext.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, {
  createContext,
  FC,
  PropsWithChildren,
  useState,
  useEffect,
  useContext,
} from 'react';

import {
  FileResult,
  IdValue,
  PostProcess,
  Project,
  ProjectDataset,
  RegressionDetails,
  UserInfo,
} from 'services/map.typings';
import {
  SyncDetails,
  Card,
} from 'routes/models/View/Projects/common/CustomSection';
import { useParams, useHistory } from 'react-router-dom';
import bar from '../assets/images/chart/bar.svg';
import scatter from '../assets/images/chart/scatter.svg';
import boxplot from '../assets/images/chart/boxplot.svg';
import population from '../assets/images/dataSource/population.svg';
import sales from '../assets/images/dataSource/sales.svg';
import pie from '../assets/images/chart/pie.svg';
import histogram from '../assets/images/chart/histogram.svg';
import reportSvg from '../assets/images/report/report.svg';
import {
  deleteDatasetPostProcess,
  getUserProject,
  setDatasetLockLevel,
} from 'services/common';
import { AppContext, PostProcessInProgressType } from './AppContext';
import { AxiosLoader } from 'services/AxiosLoader';

import { cloneDeep } from 'lodash';
import { Alert } from '@ptas/react-ui-library';
import AgGridService from 'services/AgGridService';

const images: { [id: string]: string } = {
  bar: bar,
  scatter: scatter,
  boxplot: boxplot,
  pie: pie,
  histogram: histogram,
};

interface Props {
  headerDetails: HeaderDetails;
  setHeaderDetails: React.Dispatch<React.SetStateAction<HeaderDetails>>;

  projectDetails: string;
  setProjectDetails: React.Dispatch<React.SetStateAction<string>>;

  projectType: string;
  setProjectType: React.Dispatch<React.SetStateAction<string>>;

  comments: string;
  setComments: React.Dispatch<React.SetStateAction<string>>;

  projectId: string;
  setProjectId: React.Dispatch<React.SetStateAction<string>>;

  projectName: string;
  setProjectName: React.Dispatch<React.SetStateAction<string>>;

  regression: PostProcess | null;
  setRegression: React.Dispatch<React.SetStateAction<PostProcess | null>>;

  regressionData: PostProcess[];
  setRegressionData: React.Dispatch<React.SetStateAction<PostProcess[]>>;

  regressionSyncDetails?: SyncDetails;
  setRegressionSyncDetails?: React.Dispatch<React.SetStateAction<SyncDetails>>;

  dataSourceSyncDetails?: SyncDetails;
  setDataSourceSyncDetails?: React.Dispatch<React.SetStateAction<SyncDetails>>;

  chartSyncDetails?: SyncDetails;
  setChartSyncDetails?: React.Dispatch<React.SetStateAction<SyncDetails>>;

  reportSyncDetails?: SyncDetails;
  setReportSyncDetails?: React.Dispatch<React.SetStateAction<SyncDetails>>;

  dataSourceCards?: Card[];
  setDataSourceCards?: React.Dispatch<React.SetStateAction<Card[]>>;

  chartCards?: Card[];
  setChartCards?: React.Dispatch<React.SetStateAction<Card[]>>;

  modelDetails: Project | null;

  timeCards?: Card[];
  multipleCards?: Card[];
  landCards?: Card[];
  suppCards?: Card[];
  appRatioCards?: Card[];
  annualCards?: Card[];
  salesDatasetId: string;

  userDetails: UserInfo[];
  setUserDetails: React.Dispatch<React.SetStateAction<UserInfo[]>>;

  getSyncDetails: (postProcessRole: string) => SyncDetails | undefined;

  jobIds?: JobInfo[];
  setJobIds?: (jobIds: JobInfo[]) => void;

  lockUI: boolean;

  bulkSuccess?: PostProcess;
  withNoBulkUpdate: boolean;
}

interface HeaderDetails {
  top: string;
  bottom: string;
}

interface JobInfo {
  jobId: number;
  datasetId: string;
}

// interface ErrorMessageType {
//   label: string;
//   message: ImportErrorMessageType;
// }

export const ProjectContext = createContext<Partial<Props>>({});

function ProjectProvider(props: PropsWithChildren<{}>): JSX.Element {
  const appContext = useContext(AppContext);

  const [projectId, setProjectId] = useState<string>('');
  const [projectName, setProjectName] = useState<string>('');
  const [bulkSuccess, setBulkSuccess] = useState<PostProcess>();
  const [withNoBulkUpdate, setWithNoBulkUpdate] = useState<boolean>(false);
  const [projectType, setProjectType] = useState<string>('');
  const [projectDetails, setProjectDetails] = useState<string>('');
  const [jobIds, setJobIds] = useState<JobInfo[]>([]);
  //eslint-disable-next-line
  const [refreshDatasetStatus, setRefreshDatasetStatus] = useState<any>({});
  //eslint-disable-next-line
  const [postProcessStatus, setPostProcessStatus] = useState<any>({});
  //eslint-disable-next-line
  const [jobStatus, setJobStatus] = useState<any>({});
  //eslint-disable-next-line
  const [ppStatus, setPPStatus] = useState<any>({});

  const [headerDetails, setHeaderDetails] = useState<HeaderDetails>();

  const [regressionData, setRegressionData] = useState<PostProcess[]>();
  const [regression, setRegression] = useState<PostProcess | null>(null);
  const { id }: { id: string } = useParams();

  useEffect(() => {
    setProjectId(id);
    if (id) {
      appContext.setProjectId?.(id);
    }
    //eslint-disable-next-line
  }, [id]);

  useEffect(() => {
    return (): void => {
      appContext.clearDebounceHealt?.();
    };
    //eslint-disable-next-line
  }, []);

  useEffect(() => {
    jobIds?.forEach((jobInfo) => {
      debounceJobId(jobInfo);
    });
    //eslint-disable-next-line
  }, [jobIds]);

  useEffect(() => {
    appContext.postProcessInProgress?.forEach((p) => {
      debouncePostProcessJobId(p);
    });
    //eslint-disable-next-line
  }, [appContext.postProcessInProgress]);

  useEffect(() => {
    if (!jobStatus) return;
    setDataSourceCards(getDataSourceCards(jobStatus));
    //eslint-disable-next-line
  }, [jobStatus]);

  useEffect(() => {
    setTimeCards(getCards('timetrendregression'));
    setMultipleCards(getCards('multipleregression'));
    setLandCards(getCards('waterfrontschedule'));
    setAnnualCards(getCards('annualupdateadjustment', false));
    setSuppCards(getCards('supplementalandexception', false));
    setAppRatioCards(getCards('appraisalratioreport'));
    //eslint-disable-next-line
  }, [ppStatus]);

  useEffect(() => {
    setJobStatus({
      ...jobStatus,
      ...refreshDatasetStatus,
    });
    //eslint-disable-next-line
  }, [refreshDatasetStatus]);

  useEffect(() => {
    setPPStatus({
      ...ppStatus,
      ...postProcessStatus,
    });
    //eslint-disable-next-line
  }, [postProcessStatus]);

  const debounceJobId = async (job: JobInfo): Promise<void> => {
    const jobMessage = await AgGridService.getJobStatus(job.jobId ?? 0);
    setRefreshDatasetStatus({
      ...refreshDatasetStatus,
      [`${job.datasetId}`]:
        jobMessage?.jobResult?.Status === 'Failed'
          ? jobMessage?.jobResult?.Status
          : jobMessage?.jobStatus,
    });
    if (jobMessage?.jobStatus === 'Failed') {
      setRefreshDatasetStatus({});
      return;
    }
    if (jobMessage?.jobStatus !== 'Finished') {
      setTimeout(() => {
        debounceJobId(job);
      }, 5000);
    }
  };

  const debouncePostProcessJobId = async (
    p: PostProcessInProgressType
  ): Promise<void> => {
    const jobMessage = await AgGridService.getJobStatus(p.jobId ?? 0);
    if (jobMessage?.jobStatus === 'Failed') {
      setPostProcessStatus({});
      return;
    }
    setPostProcessStatus({
      ...postProcessStatus,
      [p.postProcessRole]:
        jobMessage?.jobResult?.Status === 'Failed'
          ? jobMessage?.jobResult?.Status
          : jobMessage?.jobStatus,
    });
    if (jobMessage?.jobStatus !== 'Finished') {
      setTimeout(() => {
        debouncePostProcessJobId(p);
      }, 5000);
    }
  };

  // const [heartbeat, setHeartbeat] = useState<number>(0);
  const [modelDetails, setModelDetails] = useState<Project | null>(null);
  const [comments, setComments] = useState<string>('');
  const history = useHistory();

  const [dataSourceSyncDetails, setDataSourceSyncDetails] = useState<
    SyncDetails | undefined
  >(undefined);

  const [chartSyncDetails, setChartSyncDetails] = useState<
    SyncDetails | undefined
  >(undefined);

  const [regressionSyncDetails, setRegressionSyncDetails] = useState<
    SyncDetails | undefined
  >(undefined);

  const [reportSyncDetails, setReportSyncDetails] = useState<
    SyncDetails | undefined
  >(undefined);

  const [dataSourceCards, setDataSourceCards] = useState<Card[]>([]);

  const [chartCards, setChartCards] = useState<Card[]>([]);

  const [userDetails, setUserDetails] = useState<UserInfo[]>([]);
  const [salesDatasetId, setSalesDatasetId] = useState<string>('');

  const [timeCards, setTimeCards] = useState<Card[] | undefined>([]);
  const [multipleCards, setMultipleCards] = useState<Card[] | undefined>([]);
  const [landCards, setLandCards] = useState<Card[] | undefined>([]);
  const [annualCards, setAnnualCards] = useState<Card[] | undefined>([]);
  const [suppCards, setSuppCards] = useState<Card[] | undefined>([]);
  const [appRatioCards, setAppRatioCards] = useState<Card[] | undefined>([]);
  const [lockUI, setLockUi] = useState<boolean>(false);

  useEffect(() => {
    let status = {};
    if (!modelDetails) return;
    if (appContext.healtInfo?.length === 0) {
      modelDetails?.projectDatasets.forEach((d) => {
        status = {
          ...status,
          [d.datasetId]: 'Finished',
        };
      });
      setDataSourceCards(getDataSourceCards(status));
      return;
    }
    appContext.healtInfo?.forEach((info) => {
      status = {
        ...status,
        [info.datasetId]: info.isProcessing ? 'InProgress' : 'Finished',
      };
    });
    setDataSourceCards(getDataSourceCards(status));
    //eslint-disable-next-line
  }, [appContext.healtInfo, modelDetails]);

  useEffect(() => {
    let timeOut: NodeJS.Timeout;
    if (!projectId) return;
    const fetchData = async (): Promise<void> => {
      await loadProject();
    };
    fetchData();
    return (): void => clearTimeout(timeOut);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [projectId /*, heartbeat*/]);

  const loadProject = async (): Promise<void> => {
    setModelDetails(null);
    try {
      const data = await getUserProject(projectId);
      if (!data) return;

      setUserDetails(data.usersDetails);
      // timeOut = setTimeout(() => setHeartbeat(heartbeat + 1), 10000);
      const projectInfo = data?.project || null;
      setModelDetails(projectInfo);
      appContext.setModelDetailsProject?.(projectInfo);
      setComments(data?.project?.comments || '');
      setSalesDatasetId(
        `${
          data.project?.projectDatasets.find(
            (dataset) => dataset.datasetRole.toLowerCase() === 'sales'
          )?.datasetId
        }`
      );
    } catch (error) {
      appContext.setSnackBar?.({
        text: 'Failed loading the project',
        severity: 'error',
      });
    }
  };

  useEffect(() => {
    if (!modelDetails) return;
    setProjectType(modelDetails.projectTypeName?.toLowerCase() as string);
    setHeaderDetails(getHeaderDetails());
    setProjectName(modelDetails.projectName);
    setDataSourceSyncDetails(getDataSourceSyncDetails());
    setDataSourceCards(getDataSourceCards());

    setTimeCards(getCards('timetrendregression'));
    setMultipleCards(getCards('multipleregression'));
    setLandCards(getCards('waterfrontschedule'));
    setAnnualCards(getCards('annualupdateadjustment', false));
    setSuppCards(getCards('supplementalandexception', false));
    setAppRatioCards(getCards('appraisalratioreport'));

    // setLandScheduleCards(getCards(""))
    // setlandScheduleSyncDetails(getSyncDetails(""));

    setChartSyncDetails(getNewestChart());
    setChartCards(getChartCards());
    setRegressionSyncDetails(getNewestRegression());

    setRegressionData(
      modelDetails.projectDatasets[0].dataset?.dependencies.postProcesses
    );

    const totalAreas = modelDetails.selectedAreas.length;
    const areas = modelDetails?.selectedAreas.join(', ');
    const from = modelDetails.assessmentDateFrom;
    const to = modelDetails.assessmentDateTo;

    setProjectDetails(
      `Project details - ${
        totalAreas > 1 ? 'Areas' : 'Area'
      } ${areas}, assessment year ${
        modelDetails.assessmentYear
      }, from ${new Date(from).toLocaleDateString()} to ${new Date(
        to
      ).toLocaleDateString()}`
    );

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [modelDetails]);

  useEffect(() => {
    if (!regression) return;
    setReportSyncDetails({
      lastSyncBy:
        userDetails.find((u) => u.id === regression.lastModifiedBy)?.fullName ??
        'John Doe',
      lastSyncOn: new Date(
        regression.lastModifiedTimestamp + 'Z'
      ).toLocaleString(),
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [regression]);

  const getHeaderDetails = (): HeaderDetails => {
    const headerOldestData = getHeaderSyncDetails();
    return {
      top: `Last sync on ${headerOldestData?.lastSyncOn}, by ${headerOldestData?.lastSyncBy}`,
      bottom: getHeaderNumberDetails(),
    };
  };

  const getHeaderSyncDetails = (): SyncDetails | undefined => {
    if (!modelDetails) return;

    const oldestProjectDataset = modelDetails.projectDatasets.reduce((r, o) =>
      o.dataset.lastExecutionTimestamp &&
      r.dataset.lastExecutionTimestamp &&
      new Date(o.dataset.lastExecutionTimestamp) <
        new Date(r.dataset.lastExecutionTimestamp)
        ? o
        : r
    );
    const oldestDsDate = new Date(
      oldestProjectDataset.dataset.lastExecutionTimestamp
        ? oldestProjectDataset.dataset.lastExecutionTimestamp + 'Z'
        : new Date().toLocaleString()
    ).toLocaleString();
    return {
      lastSyncOn: oldestDsDate,
      lastSyncBy:
        userDetails.find(
          (u) => u.id === oldestProjectDataset.dataset.lastModifiedBy
        )?.fullName ?? 'John Doe',
    };
  };

  const getHeaderNumberDetails = (): string => {
    if (!modelDetails) return `Sales: XX | Population: XX,XXX | Area(s): XX`;

    const population = modelDetails?.projectDatasets.find(
      (ds) => ds.datasetRole.toLowerCase() === 'population'
    );
    const sales = modelDetails?.projectDatasets.find(
      (ds) => ds.datasetRole.toLowerCase() === 'sales'
    );
    if (!sales || !population)
      return `Sales: XX | Population: XX,XXX | Area(s): XX`;

    return `Sales: ${sales.dataset.totalRows} | Population:  ${
      population.dataset.totalRows
    } | ${'Area: ' + modelDetails.modelArea}`;
  };

  const getDataSourceSyncDetails = (): SyncDetails | undefined => {
    if (!modelDetails) return;

    const newestProjectDataset = modelDetails.projectDatasets.reduce((r, o) =>
      o.dataset.lastModifiedTimestamp &&
      r.dataset.lastModifiedTimestamp &&
      new Date(o.dataset.lastModifiedTimestamp) >
        new Date(r.dataset.lastModifiedTimestamp)
        ? o
        : r
    );
    const newestDsDate = new Date(
      newestProjectDataset.dataset.lastModifiedTimestamp
        ? newestProjectDataset.dataset.lastModifiedTimestamp + 'Z'
        : new Date().toLocaleString()
    ).toLocaleString();
    return {
      lastSyncOn: newestDsDate,
      lastSyncBy:
        userDetails.find((u) => u.id === newestProjectDataset.dataset.userId)
          ?.fullName ?? 'John Doe',
    };
  };

  // const getLastPostProcessId = (label: string): number => {
  //   const postProcessSales = modelDetails?.projectDatasets?.find(
  //     d => d?.datasetRole?.toLocaleLowerCase() === label
  //   )?.dataset?.dependencies?.postProcesses;
  //   const prioritySales = postProcessSales?.map(pp => pp.priority);
  //   if (prioritySales) {
  //     const maxPriority = Math.max(...prioritySales);
  //     return (
  //       postProcessSales?.find(pp => pp.priority === maxPriority)
  //         ?.datasetPostProcessId ?? 0
  //     );
  //   }
  //   return 0;
  // };

  const capitalize = (s: string): string => {
    if (typeof s !== 'string') return '';
    return s.charAt(0).toUpperCase() + s.slice(1);
  };

  const getDataSourceCards = (datasetsStatus: IdValue = {}): Card[] => {
    const cards: Card[] = [];
    // let postProcessId = 0;
    let bulkUpdatePostProcess = 0;
    let bulkUpdatePostProcessDate = '';
    let bulkSuccess = false;
    let withNoBulkUpdate = true;
    const getStatus = (d: ProjectDataset): string => {
      if (Object.values(datasetsStatus).length)
        return `${datasetsStatus[d.datasetId]}`;
      if (d.dataset.datasetState === 'Processed') return 'Finished';
      if (
        d.dataset.datasetState === 'NotProcessed' ||
        d.dataset.datasetState === 'Failed'
      )
        return 'Error';
      return 'InProgress';
    };

    modelDetails?.projectDatasets
      .slice()
      .sort(
        (a, b) =>
          new Date(a.dataset.lastExecutionTimestamp ?? '').getUTCDate() -
          new Date(b.dataset.lastExecutionTimestamp ?? '').getUTCDate()
      )
      .forEach(async (d) => {
        if (d.datasetRole.toLowerCase() === 'applymodel') {
          const bulk = d.dataset.dependencies.postProcesses?.find(
            (pp) =>
              pp.postProcessRole === 'ApplyModelBulkUpdate' &&
              JSON.parse(pp.resultPayload)?.Status === 'Success'
          );
          withNoBulkUpdate = d.dataset.dependencies.postProcesses?.some(
            (pp) =>
              pp.postProcessRole === 'ApplyModelBulkUpdate' &&
              (pp.resultPayload === null ||
                JSON.parse(pp.resultPayload)?.Status === 'Failed')
          );
          setWithNoBulkUpdate(withNoBulkUpdate);
          if (bulk) {
            bulkSuccess = true;
          }
          setBulkSuccess(bulk);
          bulkUpdatePostProcess = bulk?.datasetPostProcessId ?? 0;
          bulkUpdatePostProcessDate = bulk?.lastExecutionTimestamp ?? '';
          // postProcessId = 0;
        } else {
          // postProcessId = getLastPostProcessId(d.datasetRole.toLowerCase());
        }
        // const isolatedId = { postId: postProcessId };

        const title = `${capitalize(d.dataset.datasetName)}`;
        const from = d.datasetRole.toLowerCase();

        cards.push({
          header: bulkSuccess ? 'Executed Bulk' : '',
          title: title,
          author:
            userDetails.find((u) => u.id === d.dataset.lastModifiedBy)
              ?.fullName ?? 'John Doe',
          date: d.dataset.lastExecutionTimestamp ?? '',
          image:
            d.datasetRole.toLowerCase() === 'population' ? population : sales,
          onClick: () => {
            setTimeout(() => {
              history.push({
                pathname: `/models/results/${projectId}/${d.datasetId}`,
                state: {
                  from: from,
                  postProcessId: 0,
                  bulkUpdatePostProcessId: bulkUpdatePostProcess,
                  bulkUpdatePostProcessDate: bulkUpdatePostProcessDate,
                  withNoBulkUpdate: withNoBulkUpdate,
                },
              });
            }, 1000);
          },
          onLockClick: (status: boolean) =>
            setDatasetLockStatus(d.datasetId, status),
          isLocked: d.dataset.isLocked,
          showLock: true,
          //eslint-disable-next-line
          onMenuOptionClick: async (option: any): Promise<void> => {
            if (option.id === 0) {
              history.push(`/models/results/${projectId}/${d.datasetId}`);
            } else {
              await setDatasetLockStatus(d.datasetId, !d.dataset.isLocked);
            }
          },
          menuItems: [{ id: 0, label: 'Edit' }],
          status: getStatus(d),
        });
      });
    return cards;
  };

  const setDatasetLockStatus = async (
    id: string,
    isLocked: boolean
  ): Promise<void> => {
    try {
      await setDatasetLockLevel(id, isLocked);
    } catch (error) {
      appContext.setSnackBar?.({
        text: 'Setting lock status failed',
        severity: 'error',
      });
      await loadProject();
    }
  };

  const deleteChart = async (
    chartId: number,
    datasetId: string
  ): Promise<void> => {
    const cloneModel = cloneDeep(modelDetails);

    const cloneModelDataset = cloneModel?.projectDatasets.find(
      (p) => p.datasetId === datasetId
    );

    if (!cloneModelDataset?.dataset || !cloneModel) return;
    cloneModelDataset.dataset.dependencies.interactiveCharts =
      cloneModelDataset?.dataset.dependencies.interactiveCharts?.filter(
        (c) => c.interactiveChartId !== chartId
      );

    cloneModel.projectDatasets = cloneModel?.projectDatasets.map((p) =>
      p.datasetId === datasetId ? cloneModelDataset : p
    );

    setModelDetails(cloneModel);

    try {
      const loader = new AxiosLoader<{}, {}>();
      await loader.PutInfo(
        `CustomSearches/DeleteInteractiveChart/${chartId}`,
        {},
        {}
      );
    } catch (error) {
      appContext.setSnackBar?.({
        text: 'Failed deleting chart',
        severity: 'error',
      });
      await loadProject();
    }
  };

  const getChartCards = (): Card[] => {
    const cards: Card[] = [];
    modelDetails?.projectDatasets.forEach((ds) =>
      ds?.dataset?.dependencies.interactiveCharts?.forEach((ic) =>
        cards.push({
          id: ic.interactiveChartId,
          title: ic.chartTitle,
          author:
            userDetails.find((u) => u.id === ds.dataset.createdBy)?.fullName ??
            'John Doe',
          date: `${ds.dataset.createdTimestamp}`,
          image: images[ic.chartType.toLowerCase()],
          onClick: (): void =>
            history.push(
              `/models/view-chart/${id}/${ic.datasetId}/${ic.interactiveChartId}`
            ),
          onMenuOptionClick: async (e): Promise<void> => {
            if (e.id === 1) {
              //Remame
            }
          },
          menuItems: [
            { id: 0, label: 'Edit Chart' },
            { id: 1, label: 'Rename', disabled: true },
            {
              id: 2,
              label: 'Delete',
              afterClickContent: DeleteAlert(
                () => deleteChart(ic.interactiveChartId, ds.datasetId),
                ic.chartTitle ?? 'chart'
              ),
              isAlert: true,
            },
          ],
        })
      )
    );
    return cards;
  };

  const getSyncDetails = (postProcessrole: string): SyncDetails | undefined => {
    if (!modelDetails || !modelDetails?.projectDatasets) return;
    const sales = modelDetails?.projectDatasets.find(
      (p) => p.datasetRole.toLowerCase() === 'sales'
    );

    if (
      !sales ||
      !sales.dataset ||
      !sales.dataset.dependencies ||
      !sales.dataset.dependencies.postProcesses
    )
      return;

    const postProcesses = sales?.dataset.dependencies.postProcesses.filter(
      (p) =>
        p.postProcessRole && p.postProcessRole.toLowerCase() === postProcessrole
    );

    const lastModiedDates = postProcesses.flatMap(
      (p) => p.lastModifiedTimestamp
    );

    if (!lastModiedDates || lastModiedDates.length === 0) return;

    const newestDate = lastModiedDates.reduce(function (a, b) {
      return a > b ? a : b;
    });

    const user = postProcesses.find(
      (p) => p.lastExecutionTimestamp === newestDate.toString()
    )?.lastModifiedBy;

    return {
      lastSyncBy:
        userDetails.find((u) => u.id === user)?.fullName ?? 'John Doe',
      lastSyncOn: new Date(newestDate + 'Z').toLocaleString(),
    };
  };

  const getLink = (
    postProcessRole: string,
    postProcess: PostProcess
  ): string => {
    if (postProcess.postProcessRole === 'LandRegression') {
      return `/models/view-land-model/${id}/edit/${postProcess.datasetPostProcessId}`;
    }
    switch (postProcessRole.toLowerCase()) {
      case 'timetrendregression':
        return `/models/regression/${id}/${postProcess.datasetPostProcessId}/`;
      case 'multipleregression':
        return `/models/estimated_market_regression/${id}/${postProcess.datasetPostProcessId}/`;
      case 'waterfrontschedule':
        return `/models/new-land-model/${id}`;
      case 'appraiserratiosreport':
        return '';
      case 'supplementalandexception':
        return `/models/supplementals_edit/${id}/${postProcess.datasetPostProcessId}`;
      case 'annualupdateadjustment':
        return `/models/annual_update/${id}/${postProcess.datasetPostProcessId}`;
    }
    return '';
  };

  const deleteLandModel = async (datasetId: string): Promise<void> => {
    const loader = new AxiosLoader<{}, {}>();
    const url = `CustomSearches/DeleteLandModel/${datasetId}`;
    await loader.PutInfo(url, {}, {});
  };

  const deletePostProcess = async (
    id: React.ReactText,
    datasetId: string,
    postProcessRole?: string
  ): Promise<void> => {
    const cloneModel = cloneDeep(modelDetails);

    const cloneModelDataset = cloneModel?.projectDatasets.find(
      (p) => p.datasetId === datasetId
    );

    if (!cloneModelDataset?.dataset || !cloneModel) return;
    cloneModelDataset.dataset.dependencies.postProcesses =
      cloneModelDataset?.dataset.dependencies.postProcesses?.filter(
        (p) => p.datasetPostProcessId !== id
      );

    cloneModel.projectDatasets = cloneModel?.projectDatasets.map((p) =>
      p.datasetId === datasetId ? cloneModelDataset : p
    );

    setModelDetails(cloneModel);

    if (postProcessRole?.toLowerCase() === 'waterfrontschedule') {
      try {
        return await deleteLandModel(datasetId);
      } catch (error) {
        appContext.setSnackBar?.({
          text: 'Deleting Land Model failed',
          severity: 'error',
        });
        return await loadProject();
      }
    }

    try {
      await deleteDatasetPostProcess(id);
    } catch (error) {
      appContext.setSnackBar?.({
        text: 'Deleting postprocess failed',
        severity: 'error',
      });
      await loadProject();
    }
  };

  const getCards = (
    postProcessRole: string,
    noPostProcesses?: boolean
  ): Card[] => {
    const cards: Card[] = [];
    if (!modelDetails || !modelDetails.projectDatasets) return [];

    const datasets = modelDetails?.projectDatasets;

    datasets.forEach((sales) => {
      if (
        !sales ||
        !sales.dataset ||
        !sales.dataset.dependencies ||
        !sales.dataset.dependencies.postProcesses
      )
        return [];

      let postProcess = sales.dataset.dependencies.postProcesses.filter((p) => {
        return (
          (p.postProcessRole && p.postProcessRole.toLowerCase()) ===
            postProcessRole.toLowerCase() &&
          p.primaryDatasetPostProcessId === null &&
          p.postProcessType.toLowerCase() ===
            (['supplementalandexception', 'annualupdateadjustment'].includes(
              postProcessRole.toLowerCase()
            ) || postProcessRole.toLowerCase() === 'waterfrontschedule'
              ? 'exceptionpostprocess'
              : 'rscriptpostprocess')
        );
      });

      if (!postProcess) return [];

      if (postProcessRole.toLowerCase() === 'waterfrontschedule') {
        postProcess = sales.dataset.dependencies.postProcesses.filter((p) =>
          ['WaterfrontSchedule', 'LandRegression'].includes(
            `${p.postProcessRole}`
          )
        );
      }

      const getCardTitle = (pp: PostProcess): string => {
        if (pp.postProcessRole?.toLowerCase() === 'waterfrontschedule')
          return 'Land Schedule';
        if (pp?.postProcessName === 'Land Schedule') return 'Land Regression';
        return pp?.postProcessName || '';
      };

      const getError = (payload: RegressionDetails): string | undefined => {
        if (postProcessRole.toLowerCase() === 'waterfrontschedule') {
          const failedLandPostProcess = sales.dataset.dependencies.postProcesses
            .filter((p) =>
              [
                'LandSchedule',
                'WaterfrontSchedule',
                'LandAdjustment',
                'LandRegression',
                'NonWaterfrontExpressions',
                'WaterfrontExpressions',
                'WaterfrontSchedule',
              ].includes(`${p.postProcessRole}`)
            )
            .find(
              (p) => JSON.parse(p.resultPayload)?.Reason === 'Failed'
            )?.resultPayload;
          if (!failedLandPostProcess) return undefined;
          const landPayload = JSON.parse(
            `${failedLandPostProcess}`
          ) as RegressionDetails;
          return `Status: Failed - Reason: ${
            landPayload.Reason ? landPayload.Reason : 'Unknown error'
          }`;
        }
        return payload?.Status.toLowerCase() === 'failed'
          ? `Status: Failed - Reason: ${
              payload.Reason ? payload.Reason : 'Unknown error'
            }`
          : '';
      };

      const getPostProcessStatus = (p: PostProcess): string => {
        if (!appContext.postProcessInProgress?.length) return '';
        const postProcessFound = appContext?.postProcessInProgress?.find(
          (pp) =>
            pp.postProcessRole?.toLowerCase() ===
            p.postProcessRole?.toLowerCase()
        );
        if (!Object.values(ppStatus).length) {
          setLockUi(false);
        } else {
          const postProcessStatus =
            ppStatus[`${p?.postProcessRole?.toLowerCase()}`]?.toLowerCase();
          if (
            postProcessStatus !== 'finished' &&
            postProcessStatus !== undefined
          ) {
            setLockUi(true);
          } else {
            setLockUi(false);
          }
        }

        if (postProcessFound) {
          return (
            (Object.values(ppStatus).length &&
              `${ppStatus[`${p?.postProcessRole?.toLowerCase()}`]}`) ||
            ''
          );
        }
        return '';
      };

      const getName = (role: string): string => {
        if (role === 'Land Schedule') {
          return 'Land Regression';
        }
        if (role === 'Waterfront schedule') {
          return 'Land Model';
        }
        return role;
      };

      postProcess
        .filter((p) => p.primaryDatasetPostProcessId === null)
        .forEach((p) => {
          let payload: RegressionDetails | undefined = undefined;
          try {
            payload = JSON.parse(p.resultPayload) as RegressionDetails;
          } catch (error) {
            appContext.setSnackBar?.({
              severity: 'error',
              text: 'Error getting suplemental regression',
            });
          }
          let errorCard = {};
          const error = getError(payload as RegressionDetails);
          if (error) {
            errorCard = { error };
          }
          if (!noPostProcesses) {
            cards.push({
              title: getCardTitle(p) ?? 'Card title',
              author:
                userDetails.find((u) => u.id === p.createdBy)?.fullName ??
                'John Doe',
              date: `${p.createdTimestamp}`,
              image: undefined,
              onClick: (): void => history.push(getLink(postProcessRole, p)),
              onMenuOptionClick: async (m): Promise<void> => {
                if (m.id === 'edit') {
                  history.push(getLink(postProcessRole, p));
                }
              },
              menuItems: [
                { id: 'edit', label: 'Edit' },
                {
                  id: 'delete',
                  label: 'Delete',
                  afterClickContent: DeleteAlert(
                    () =>
                      deletePostProcess(
                        p.datasetPostProcessId,
                        p.datasetId,
                        p.postProcessRole
                      ),
                    getName(`${p.postProcessName}`) ?? 'post process'
                  ),
                  isAlert: true,
                },
              ],
              ...errorCard,
              status: getPostProcessStatus(p),
            });
          }

          if (!payload || payload?.Status?.toLowerCase() === 'failed')
            return cards;

          let reports: FileResult[] = [];
          reports =
            payload?.FileResults?.map((fr: FileResult) => ({
              ...fr,
              regressionDetails: payload,
            })) || [];

          const filteredReports = reports.filter(
            (r) => r.FileName.includes('.html') || r.FileName.includes('.pdf')
          );

          if (!filteredReports) return cards;

          filteredReports.forEach((f) => {
            cards.push({
              title: f.FileName,
              author:
                userDetails.find((u) => u.id === p.createdBy)?.fullName ??
                'John Doe',
              date: `${p.createdTimestamp}`,
              image: reportSvg,
              onClick: (): void => {
                history.push(
                  `/models/reports/${id}/${p.datasetPostProcessId}/${f.FileName}`
                );
                appContext.setPostProcessName &&
                  appContext.setPostProcessName(null);
              },
              onMenuOptionClick: async (m): Promise<void> => {
                if (m.id === 0) {
                  history.push(
                    `/models/reports/${id}/${p.datasetPostProcessId}/${f.FileName}`
                  );
                  appContext.setPostProcessName &&
                    appContext.setPostProcessName(null);
                }
              },
              menuItems: [
                { id: 0, label: 'Open' },
                {
                  id: 1,
                  label: 'Delete',
                  disabled: postProcessRole !== 'appraisalratioreport',
                  afterClickContent: DeleteAlert(
                    () =>
                      deletePostProcess(p.datasetPostProcessId, p.datasetId),
                    p.postProcessName ?? 'report'
                  ),
                  isAlert: true,
                },
              ],
            });
          });
        });
    });

    return cards;
  };

  const DeleteAlert = (
    onConfirm: () => Promise<void>,
    type: string
  ): JSX.Element => (
    <Alert
      okButtonClick={async (): Promise<void> => {
        await onConfirm();
      }}
      contentText={`Delete will permanently erase ${type}`}
      okButtonText="Delete"
    />
  );

  const getNewestChart = (): SyncDetails | undefined => {
    const salesDataset = modelDetails?.projectDatasets.find(
      (ds) => ds.datasetRole.toLowerCase() === 'sales'
    );
    if (
      !salesDataset ||
      !salesDataset.dataset.dependencies.interactiveCharts ||
      salesDataset.dataset.dependencies.interactiveCharts.length === 0
    )
      return;

    const newestChart =
      salesDataset.dataset.dependencies.interactiveCharts.reduce((r, o) =>
        o.lastModifiedTimestamp > r.lastModifiedTimestamp ? o : r
      );

    return {
      lastSyncBy:
        userDetails.find((u) => u.id === newestChart.lastModifiedBy)
          ?.fullName ?? 'John Doe',
      lastSyncOn: new Date(
        newestChart.lastModifiedTimestamp + 'Z'
      ).toLocaleString(),
    };
  };

  const getNewestRegression = (): SyncDetails | undefined => {
    const dataset = modelDetails?.projectDatasets.find(
      (ds) => ds.dataset.dependencies.postProcesses !== null
    );
    if (
      !dataset ||
      !dataset.dataset.dependencies.postProcesses ||
      dataset.dataset.dependencies.postProcesses.length === 0
    )
      return;

    const newestRegression = dataset.dataset.dependencies.postProcesses.reduce(
      (r, o) => (o.lastModifiedTimestamp > r.lastModifiedTimestamp ? o : r)
    );

    return {
      lastSyncBy:
        userDetails.find((u) => u.id === newestRegression.lastModifiedBy)
          ?.fullName ?? 'John Doe',
      lastSyncOn: new Date(
        newestRegression.lastModifiedTimestamp + 'Z'
      ).toLocaleString(),
    };
  };

  return (
    <ProjectContext.Provider
      value={{
        projectId,
        projectType,
        headerDetails,
        projectName,
        regression,
        setRegression,
        regressionData,
        comments,
        setComments,
        dataSourceSyncDetails,
        chartSyncDetails,
        regressionSyncDetails,
        reportSyncDetails,
        dataSourceCards,
        projectDetails,
        chartCards,
        modelDetails,
        userDetails,
        getSyncDetails,
        timeCards,
        multipleCards,
        landCards,
        annualCards,
        suppCards,
        appRatioCards,
        salesDatasetId,
        setJobIds,
        lockUI,
        bulkSuccess,
        withNoBulkUpdate,
      }}
    >
      {props.children}
    </ProjectContext.Provider>
  );
}

export const withProjectProvider =
  (Component: FC) =>
  (props: object): JSX.Element =>
    (
      <ProjectProvider>
        <Component {...props} />
      </ProjectProvider>
    );
