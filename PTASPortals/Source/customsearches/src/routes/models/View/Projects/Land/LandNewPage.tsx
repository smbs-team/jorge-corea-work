// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  Fragment,
  useContext,
  useEffect,
  useRef,
  useState,
} from 'react';
import GetAppIcon from '@material-ui/icons/GetApp';
import PublishIcon from '@material-ui/icons/Publish';
import FileCopyIcon from '@material-ui/icons/FileCopy';
import DeleteIcon from '@material-ui/icons/Delete';
import { CopyToClipboard } from 'react-copy-to-clipboard';
import CustomSection from '../common/CustomSection';
import CancelIcon from '@material-ui/icons/Cancel';
import SaveIcon from '@material-ui/icons/Save';
import {
  CustomIconButton,
  PanelHeader,
  SectionContainer,
} from '@ptas/react-ui-library';
import EditIcon from '@material-ui/icons/Edit';
import Loader from 'react-loader-spinner';
import {
  ExcelSheetJson,
  GenericGridRowData,
  LandGridData,
  LandVariableGridRowData,
  PostProcess,
  Project,
  ProjectDataset,
  SheetType,
  // WaterFrontType,
} from 'services/map.typings';
import LandVariableGrid from './LandVariablesGrid';
import { Link, useHistory, useParams } from 'react-router-dom';
import { LoadProject } from '../../Regression/common';
import Loading from 'components/Loading';
import {
  executeDatasetPostProcess,
  nonWaterFrontExpressionService,
  waterFrontExpressionService,
  runAdjustment,
  runNonWaterFrontFromExcel,
  runWaterFrontFromExcel,
  getPredictedEcuation,
  createSetupLandModel,
} from './services/landServices';
import ExpressionsGrid, { ExpressionGridData } from './ExpressionsGrid';
import { deleteDatasetPostProcess } from 'services/common';
import { AppContext } from 'context/AppContext';
import { ColDef } from 'ag-grid-community';
import useJsonTools from 'components/common/useJsonTools';
import { v4 as uuidv4 } from 'uuid';
import 'react-block-ui/style.css';
import '../../../../../assets/time-trend-styles/block-ui.scss';
import {
  getPostProcess,
  ErrorMessageType,
  UserInfo,
  getBottom,
} from './services/newLandServices';
import { useStyles } from './styles';
import { ScheduleGrid } from './ScheduleGrid';
// import useSignalR from 'components/common/useSignalR';
import ErrorMessage from 'components/ErrorMessage/ErrorMessage';
// import NewLandModal from './NewLandModal';
import LandCollapsableGrid from './LandCollapsableGrid';
import { SalesDropdown } from '../../Regression/SalesDropdown';
import AgGridService from 'services/AgGridService';
import { InsertDriveFile } from '@material-ui/icons';
import BlockUi from 'react-block-ui';
import _ from 'underscore';
import { ProjectContext } from 'context/ProjectsContext';

// interface LandRegressionModalType {
//   to: string;
//   default: string;
// }

const gridData: GenericGridRowData[] = [
  {
    name: 'NewLandValue',
    type: 'Calculated',
  },
  {
    name: 'WaterfrontValue',
    type: 'Calculated',
  },
  {
    name: 'Major',
    type: 'Independent',
  },
  {
    name: 'Minor',
    type: 'Independent',
  },
  {
    name: 'SqFtLot',
    type: 'Independent',
  },
  {
    name: 'WFtFoot',
    type: 'Independent',
  },
];

let debounced: (() => Promise<void>) & _.Cancelable;

const NewLandPage = (): JSX.Element => {
  const { id }: { id: string } = useParams();
  const appContext = useContext(AppContext);
  const history = useHistory();
  const [project, setProjectInfo] = useState<Project | null | undefined>(null);
  const [datasetId, setDatasetId] = useState<string>('');
  const [datasets, setDatasets] = useState<ProjectDataset[]>([]);
  const [predictedEcuation, setPredictedEcuation] = useState<string>('');
  const [reloadGridKey, setReloadGridKey] = useState<string>(uuidv4());
  const [excelKey, setExcelKey] = useState<string>(uuidv4());
  const classes = useStyles();
  const [userInfo, setUserInfo] = useState<UserInfo[]>();
  const [nonWaterGridData, setNonWaterGridData] = useState<LandGridData[]>([]);
  const [nonWaterfrontGridData, setNonWaterfrontGridData] = useState<
    LandGridData[]
  >([]);
  const [postProcess, setPostProcess] = useState<PostProcess | null>();
  const [waterfrontGridData, setWaterfrontGridData] = useState<LandGridData[]>(
    []
  );
  const [nonWaterFrontExpressionData, setNonWaterFrontExpressionData] =
    useState<ExpressionGridData[]>([]);
  const [waterFrontExpressionData, setWaterFrontExpressionData] = useState<
    ExpressionGridData[]
  >([]);
  const [positiveGridData, setPositiveGridData] = useState<
    LandVariableGridRowData[]
  >([]);

  const [secondaryDatasets, setSecondaryDatasets] = useState<string[]>([]);
  const [positiveUpdateData, setPositiveUpdateData] = useState<
    LandVariableGridRowData[]
  >([]);
  const [loadingPostProcess, setLoadingPostProcess] = useState<boolean>(false);
  const [runningSetup, setRunningSetup] = useState<boolean>(false);

  const [waterfront, setWaterfront] = useState<PostProcess>();
  const [nonWaterfront, setNonWaterfront] = useState<PostProcess>();
  const [adjustmentPostProcess, setAdjustmentPostProcess] =
    useState<PostProcess>();
  const [defaultPostProcess, setDefaultPostProcess] = useState<PostProcess>();
  const [nonWaterColDefs, setNonWaterColDefs] = useState<ColDef[]>();
  const [waterColDefs, setWaterColDefs] = useState<ColDef[]>();
  const [blocked, setBlocked] = useState<string>('');
  const [selectedSection, setSelectedSection] = useState<
    'nonWater' | 'water'
  >();
  const [loadingRegression, setLoadingRegression] = useState<string>('');
  const [regressionViewKey, setRegressionViewKey] = useState<string>(uuidv4());
  const [isChangeDataset, setIsChangeDataset] = useState(false);
  const [disableApllyDatasetSelection, setDisableApllyDatasetSelection] =
    useState(true);

  const [
    waterFrontExpressionPostProcessId,
    setWaterFrontExpressionPostProcessId,
  ] = useState<number>();
  const [
    nonWaterFrontExpressionPostProcessId,
    setNonWaterFrontExpressionPostProcessId,
  ] = useState<number>();
  const [datasetIsDirty, setDatasetIsDirty] = useState<boolean>(false);

  const [jobId, setJobId] = useState<number>(0);
  const projectContext = useContext(ProjectContext);

  // the error variable is of any type since any structure can come from the backend
  //eslint-disable-next-line
  const getErrorMessage = (error: any, label: string): void => {
    setLoadingRegression('');
    if (error?.message) {
      setErrorMessage({
        message: {
          message: error?.message,
          reason: error?.validationError,
        },
        section: label,
      });
    } else {
      setErrorMessage({
        message: {
          message: error,
          reason: [],
        },
        section: label,
      });
    }
  };

  useEffect(() => {
    const validateDatasetSelected = (): void => {
      if (!datasetId.length) return;
      if (!isChangeDataset) return;
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
  }, [datasetId, isChangeDataset]);

  const getJobStatus = async (): Promise<void> => {
    if (jobId) {
      appContext?.handlePostProcessInProgress?.({
        jobId: jobId,
        postProcessId: waterfront?.datasetPostProcessId ?? 0,
        datasetId: datasetId,
        postProcessRole: 'waterfrontschedule',
      });
      const jobStatus = await AgGridService.getJobStatus(jobId);
      const status = jobStatus?.jobStatus === 'Finished';
      console.log(`status`, status);
      if (!status) {
        debounced = _.debounce(getJobStatus, 5000);
        debounced();
      } else {
        setRunningSetup(false);
        setLoadingRegression('');
        setBlocked('');
        setReloadGridKey(uuidv4());
      }
    }
  };

  const callValidateMessage = (): void => {
    getJobStatus();
  };

  //eslint-disable-next-line
  useEffect(() => {
    return (): void => {
      debounced?.cancel();
    };
  }, []);

  useEffect(callValidateMessage, [jobId]);

  const inputFileNonWaterRef = useRef<HTMLInputElement>(null);
  const inputFileWaterRef = useRef<HTMLInputElement>(null);
  const [files, setFiles] = useState<FileList | null>(null);
  const {
    getExcelFromJson,
    isLoading,
    convertFromExcelToJson,
    fromExcelLoading,
  } = useJsonTools();

  const [selectedVariableGrid, setSelectedVariableGrid] = useState<
    'water' | 'nonWater'
  >();

  const [errorMessage, setErrorMessage] = useState<ErrorMessageType>();

  const callFetch = (): void => {
    fetchData();
  };
  useEffect(callFetch, [id]);

  const fetchData = async (): Promise<void> => {
    const projectInfo = await LoadProject(id, true);
    if (projectInfo) {
      setProjectInfo(projectInfo.project);
      setUserInfo(projectInfo.usersDetails);
      const projectI = projectInfo.project as Project;
      const datasets = projectI.projectDatasets;
      let datasetId = datasets[0].datasetId;
      let secondaryDatasets: string[] = [];
      setDatasets(datasets);
      datasets.forEach((d) => {
        const pp = d.dataset.dependencies?.postProcesses?.find(
          (p) => p.priority === 2600 && p.primaryDatasetPostProcessId === null
        );
        if (pp) {
          console.log(`pp`, pp);
          datasetId = pp.datasetId;
          if (pp?.secondaryDatasets?.length)
            secondaryDatasets = [...pp?.secondaryDatasets];
        }
      });
      setDatasetId(datasetId);
      setSecondaryDatasets(secondaryDatasets);
    }
  };

  const callLoadLandData = (): void => {
    loadLandData();
  };
  useEffect(callLoadLandData, [project]);

  const loadLandData = async (viewSpinner = true): Promise<void> => {
    if (project) {
      if (viewSpinner) setLoadingPostProcess(true);
      const data = await getPostProcess(project, {
        setNonWaterfrontGridData,
        setNonWaterColDefs,
        setWaterfrontGridData,
        setWaterColDefs,
        setPositiveGridData,
        setNonWaterFrontExpressionData,
        setWaterFrontExpressionData,
        setWaterFrontExpressionPostProcessId,
        setNonWaterFrontExpressionPostProcessId,
      });
      if (data.mainPostProcess) {
        setPostProcess(data.mainPostProcess);
        if (data.mainPostProcess.resultPayload?.length) {
          const result = JSON.parse(data.mainPostProcess.resultPayload);
          if (result?.Status === 'Success') {
            const ecuation = await getPredictedEcuation(
              data.mainPostProcess.datasetPostProcessId
            );
            if (ecuation?.predictedEquation)
              setPredictedEcuation(ecuation?.predictedEquation);
          }
        }
      }
      if (data.nonWaterFront) setNonWaterfront(data.nonWaterFront);
      if (data.regressionViewKey) setRegressionViewKey(data.regressionViewKey);
      if (data.waterFront) setWaterfront(data.waterFront);
      if (data.adjustmentPostProcess) {
        setAdjustmentPostProcess(data.adjustmentPostProcess);
        setDefaultPostProcess(data.adjustmentPostProcess);
      }
      setLoadingPostProcess(false);
    }
  };

  const disableContent = (label: string) => async (): Promise<void> => {
    if (datasetIsDirty) {
      appContext.setSnackBar?.({
        severity: 'warning',
        text: 'Another post process is happening right now.',
      });
      return;
    }
    if (blocked.length && blocked !== label) return;
    if (blocked === label) {
      switch (label) {
        case 'nonWater':
          await runNonWaterFrontRegression();
          break;
        case 'nonWaterExpression':
          await runNonWaterFrontExpression();
          break;
        case 'water':
          await updateWaterFront();
          break;
        case 'waterExpression':
          await runWaterFrontExpression();
          break;
        case 'adjustment':
          await runAdjustments();
          break;
        default:
          break;
      }
      setBlocked('');
      return;
    }
    setBlocked(label);
  };

  const runWaterFrontExpression = async (): Promise<void> => {
    try {
      setLoadingRegression('waterExpression');
      setDatasetIsDirty(true);
      const postProcess = await waterFrontExpressionService(
        waterFrontExpressionData,
        datasetId,
        setWaterFrontExpressionPostProcessId,
        secondaryDatasets,
        waterFrontExpressionPostProcessId
      );
      if (!postProcess) setLoadingRegression('');
      if (postProcess) {
        const execute = await executeDatasetPostProcess(
          datasetId,
          postProcess?.id
        );
        if (postProcess?.id) {
          setWaterFrontExpressionPostProcessId(parseInt(postProcess?.id));
        }
        if (execute) setJobId(parseInt(`${execute.id}`));
      }
      setErrorMessage(undefined);
    } catch (error) {
      getErrorMessage(error, 'waterExpression');
    } finally {
      setDatasetIsDirty(false);
      setBlocked('');
    }
  };

  const runNonWaterFrontExpression = async (): Promise<void> => {
    try {
      setLoadingRegression('nonWaterExpression');
      setDatasetIsDirty(true);
      const postProcess = await nonWaterFrontExpressionService(
        nonWaterFrontExpressionData,
        datasetId,
        setNonWaterFrontExpressionPostProcessId,
        secondaryDatasets,
        nonWaterFrontExpressionPostProcessId
      );
      if (!postProcess) setLoadingRegression('');
      if (postProcess) {
        const execute = await executeDatasetPostProcess(
          datasetId,
          postProcess?.id
        );
        if (postProcess?.id) {
          setNonWaterFrontExpressionPostProcessId(parseInt(postProcess?.id));
          // const pp = await LoadPostProcess(postProcess?.id);
          // if (pp) setDefaultPostProcess(pp);
          // setReloadGridKey(uuidv4());
        }
        if (execute) setJobId(parseInt(`${execute.id}`));
      }
      setErrorMessage(undefined);
    } catch (error) {
      getErrorMessage(error, 'nonWaterExpression');
    } finally {
      setDatasetIsDirty(false);
      setBlocked('');
    }
  };

  const runNonWaterFrontRegression = async (): Promise<void> => {
    if (nonWaterfront) {
      const sheet: SheetType = { headers: [], rows: [] };
      if (nonWaterColDefs?.length) {
        sheet.headers = nonWaterColDefs?.map<string>((col) => `${col?.field}`);
      }
      if (nonWaterGridData.length) {
        sheet.rows = nonWaterGridData.map((gridData) =>
          Object.values(gridData)
        );
      }
      try {
        setLoadingRegression('nonWater');
        setDatasetIsDirty(true);
        const exeception = await runNonWaterFrontFromExcel(
          sheet,
          datasetId,
          secondaryDatasets
        );
        // setDefaultPostProcess(nonWaterfront)
        // setReloadGridKey(uuidv4());
        if (exeception) {
          setJobId(parseInt(`${exeception.id}`));
        }
        setErrorMessage(undefined);
      } catch (error) {
        getErrorMessage(error, 'nonWater');
      } finally {
        setDatasetIsDirty(false);
        setBlocked('');
      }
    }
  };

  const createRegression = (): void => {
    if (postProcess) {
      history.push(
        `/models/view-land-model/${id}/edit/${postProcess?.datasetPostProcessId}`
      );
    } else {
      history.push(`/models/view-land-model/${id}/create`);
    }
  };

  const importFromExcel = (from: 'water' | 'nonWater'): void => {
    switch (from) {
      case 'water':
        if (inputFileNonWaterRef.current) {
          inputFileNonWaterRef.current.click();
        }
        break;
      case 'nonWater':
        if (inputFileWaterRef.current) {
          inputFileWaterRef.current.click();
        }
        break;
      default:
        break;
    }
    setSelectedSection(from);
  };

  const callJson = (): void => {
    files && getJsonFromExcel(files[0]);
  };

  useEffect(callJson, [files]);

  const getField = (header: string): string => {
    if (header === 'SqFt') return 'to';
    if (header === 'default') return 'default';
    return header;
  };

  const getEditable = (header: string): boolean => {
    if (header !== 'SqFt') return true;
    return false;
  };

  const getJsonFromExcel = async (file: File): Promise<void> => {
    try {
      const json = await convertFromExcelToJson(file);
      if (selectedSection === 'nonWater') {
        const colDefs = json?.Sheet1.headers.map<ColDef>((header) => {
          return {
            headerName: header,
            field: getField(header),
            flex: 1,
            editable: getEditable(header),
          };
        });
        setNonWaterColDefs(colDefs);
        const data = json?.Sheet1.rows.map((row: unknown[]) => {
          let result = {};
          // if (Array.isArray(row))
          row?.forEach((value: unknown, index: number) => {
            result = {
              ...result,
              [getField(json?.Sheet1.headers[index])]: value,
            };
          });
          return result;
        });
        if (data) {
          setNonWaterfrontGridData(data as LandGridData[]);
          if (nonWaterfront) {
            setLoadingRegression('nonWater');
            try {
              const pp = await runNonWaterFrontFromExcel(
                json?.Sheet1,
                datasetId,
                secondaryDatasets
              );
              if (pp) {
                // setDefaultPostProcess(nonWaterfront)
                // setReloadGridKey(uuidv4());
                setJobId(parseInt(pp.id));
              }
              // await loadLandData(false);
              setErrorMessage(undefined);
            } catch (error) {
              getErrorMessage(error, 'nonWater');
            } finally {
              setFiles(null);
              setExcelKey(uuidv4());
            }
          }
        }
      }
      if (selectedSection === 'water') {
        const colDefs = json?.Sheet1.headers.map<ColDef>((header) => {
          return {
            headerName: header,
            field: getField(header),
            flex: 1,
            editable: getEditable(header),
          };
        });
        setWaterColDefs(colDefs);
        const data = json?.Sheet1.rows.map((row: unknown[]) => {
          let result = {};
          row?.forEach((value: unknown, index: number) => {
            result = {
              ...result,
              [getField(json?.Sheet1.headers[index])]: value,
            };
          });
          return result;
        });
        if (data) {
          setWaterfrontGridData(data as LandGridData[]);
          if (waterfront) {
            setLoadingRegression('water');
            try {
              const pp = await runWaterFrontFromExcel(
                json?.Sheet1,
                datasetId,
                secondaryDatasets
              );
              if (pp) {
                // setDefaultPostProcess(waterfront)
                // setReloadGridKey(uuidv4());
                setJobId(parseInt(pp.id));
              }
              // await loadLandData(false);
              setFiles(null);
              setErrorMessage(undefined);
            } catch (error) {
              getErrorMessage(error, 'water');
            } finally {
              setFiles(null);
              setExcelKey(uuidv4());
            }
          }
        }
      }
      //TODO Jorge: implement imported data - https://kingcounty.visualstudio.com/PTAS/_workitems/edit/140293
    } catch (error) {
      appContext.setSnackBar?.({
        severity: 'error',
        text: `Something failed ${error}`,
      });
    }
  };

  const getColumnNames = (from: string): string[] => {
    if (from === 'water')
      return waterColDefs?.map((col) => `${col.headerName}`) || [];
    return (
      nonWaterColDefs?.map((col) => {
        if (`${col.headerName}` === 'to') return 'SqFt';
        if (`${col.headerName}` === 'LandValue') return '';
        return `${col.headerName}`;
      }) || []
    );
  };

  const toExcel = async (from: 'water' | 'nonWater'): Promise<void> => {
    const fileName =
      from === 'water'
        ? 'Waterfront schedule.xlsx'
        : 'Nonwaterfront schedule.xlsx';
    setSelectedVariableGrid(from);
    const columns: string[] = getColumnNames(from);

    const data = from === 'water' ? waterfrontGridData : nonWaterGridData;

    const rows = data.map((data) => Object.values(data));

    const toSend: ExcelSheetJson = {
      Sheet1: {
        headers: columns,
        rows: rows,
      },
    };
    console.log({ toSend });
    // return;
    await getExcelFromJson(toSend, fileName);
  };

  const extraIcons = [
    {
      icon: <DeleteIcon />,
      text: 'Delete',
      onClick: (): Promise<void> => {
        return deleteRegression();
      },
      disabled: !predictedEcuation.length,
    },
  ];

  const deleteRegression = async (): Promise<void> => {
    try {
      if (postProcess) {
        setLoadingRegression('deleteRegression');
        await deleteDatasetPostProcess(postProcess?.datasetPostProcessId);
        setPredictedEcuation('');
        setPostProcess(null);
        setRegressionViewKey(uuidv4());
      }
    } catch (error) {
      appContext.setSnackBar?.({
        text: `Delete regression failed. Message: ${error}`,
        severity: 'error',
      });
    } finally {
      setLoadingRegression('');
    }
  };

  // const getBlockedFromExcel = (label: string): boolean => {
  //   if (!blocked.length) return false;
  //   return blocked !== label;
  // };

  const renderWaterfrontIcons = (): JSX.Element => {
    return (
      <Fragment>
        <CustomIconButton
          text="To Excel"
          icon={<GetAppIcon />}
          onClick={(): void => {
            toExcel('water');
          }}
          className={classes.firstIconButton}
        />
        <CustomIconButton
          text="From Excel"
          icon={<PublishIcon />}
          onClick={(): void => {
            if (blocked.length && blocked !== 'water') return;
            importFromExcel('water');
          }}
          disabled={projectContext.modelDetails?.isLocked}
          className={classes.iconButton}
        />
      </Fragment>
    );
  };

  const renderAdjustmentIcons = (): JSX.Element => {
    if (isEditing('adjustment')) {
      return (
        <Fragment>
          <CustomIconButton
            text="Save"
            icon={<SaveIcon />}
            onClick={disableContent('adjustment')}
            className={classes.iconButton}
          />
          <CustomIconButton
            text="Cancel"
            icon={<CancelIcon />}
            onClick={cleanBlock}
            className={classes.iconButton}
          />
        </Fragment>
      );
    }
    return (
      <CustomIconButton
        text="Edit"
        icon={<EditIcon />}
        onClick={disableContent('adjustment')}
        className={classes.iconButton}
        disabled={projectContext.modelDetails?.isLocked}
      />
    );
  };

  const applyDatasetSelection = async (): Promise<void> => {
    try {
      setRunningSetup(true);
      if (datasetId) {
        const landModelSetup = await createSetupLandModel(
          datasetId,
          secondaryDatasets
        );
        if (landModelSetup?.jobId) {
          setJobId(landModelSetup.jobId);
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

  const modelIcons = [
    {
      icon: <InsertDriveFile />,
      text: 'Apply Data set selection',
      onClick: (): Promise<void> => {
        return applyDatasetSelection();
      },
      disabled: disableApllyDatasetSelection,
    },
  ];

  const renderNonWaterfrontIcons = (): JSX.Element => {
    return (
      <Fragment>
        <CustomIconButton
          text="To Excel"
          icon={<GetAppIcon />}
          onClick={(): void => {
            toExcel('nonWater');
          }}
          className={classes.firstIconButton}
        />
        <CustomIconButton
          text="From Excel"
          icon={<PublishIcon />}
          onClick={(): void => {
            if (blocked.length && blocked !== 'water') return;
            importFromExcel('nonWater');
          }}
          disabled={projectContext.modelDetails?.isLocked}
          className={classes.iconButton}
        />
      </Fragment>
    );
  };

  const updateNonWaterColData = (
    newData: LandGridData[],
    change: boolean
  ): void => {
    if (change) {
      //With change transformation
    }
    setNonWaterGridData(newData);
  };

  const updateWaterFront = async (): Promise<void> => {
    try {
      setLoadingRegression('water');
      setDatasetIsDirty(true);
      const sheet: SheetType = { headers: [], rows: [] };
      if (waterColDefs?.length) {
        sheet.headers = waterColDefs?.map<string>((col) => `${col?.field}`);
      }
      if (waterfrontGridData.length) {
        sheet.rows = waterfrontGridData.map((gridData) =>
          Object.values(gridData)
        );
      }
      const postProcess = await runWaterFrontFromExcel(
        sheet,
        datasetId,
        secondaryDatasets
      );
      if (postProcess) {
        // setDefaultPostProcess(waterfront)
        // setReloadGridKey(uuidv4());
        setJobId(parseInt(`${postProcess.id}`));
      }
      setErrorMessage(undefined);
    } catch (error) {
      getErrorMessage(error, 'water');
    } finally {
      setDatasetIsDirty(false);
      setBlocked('');
    }
  };

  const updateWaterColData = (
    newData: LandGridData[],
    change: boolean
  ): void => {
    //With change transformation
    if (change) {
      setWaterfrontGridData(newData);
    }
  };

  const runAdjustments = async (): Promise<void> => {
    try {
      setLoadingRegression('adjustment');
      const postProcess = await runAdjustment(
        {
          rules: positiveUpdateData,
          datasetId: datasetId,
        },
        adjustmentPostProcess?.datasetPostProcessId,
        secondaryDatasets
      );
      if (postProcess) {
        // setDefaultPostProcess(adjustmentPostProcess)
        // setReloadGridKey(uuidv4());
        setJobId(parseInt(postProcess.id));
      }
      setErrorMessage(undefined);
    } catch (error) {
      getErrorMessage(error, 'adjustment');
    } finally {
      setBlocked('');
      setPositiveGridData(positiveUpdateData);
    }
  };

  const updatePositiveAdjustentGrid = (
    data: LandVariableGridRowData[]
  ): void => {
    setPositiveUpdateData(data);
  };

  const getBlokedStatus = (label: string): boolean => {
    return !!blocked && blocked !== label;
  };

  const isEditing = (label: string): boolean => {
    return label === blocked;
  };

  const updateGridData =
    (label: string) =>
    (data: ExpressionGridData[]): void => {
      if (label === 'nonWater') setNonWaterFrontExpressionData(data);
      if (label === 'water') setWaterFrontExpressionData(data);
    };

  const cleanBlock = (): void => setBlocked('');

  const renderEcuation = (): JSX.Element => {
    if (predictedEcuation.length) {
      return (
        <span className={classes.ecuation}>
          {predictedEcuation}
          <CopyToClipboard
            text={predictedEcuation}
            // onCopy={() => setState({ copied: true })}
          >
            <FileCopyIcon
              style={{ color: 'black', cursor: 'pointer', marginLeft: '10px' }}
            />
          </CopyToClipboard>
        </span>
      );
    }
    return <></>;
  };

  const getDatasetId = (datasetId: string): void => {
    setDatasetId(datasetId);
  };

  const changeDataset = (): void => {
    setIsChangeDataset(true);
  };

  const getSecondaryDataset = (datasets: string[]): void => {
    setSecondaryDatasets(datasets);
  };

  if (loadingPostProcess || !project) return <Loading />;

  console.log(`datasets`, datasets);

  return (
    <Fragment>
      <PanelHeader
        route={[
          <Link to="/models" style={{ color: 'black' }}>
            Models
          </Link>,
          <Link to={`/models/view/${id}`} style={{ color: 'black' }}>
            {project?.projectName}
          </Link>,
          <span>Land Model</span>,
        ]}
        icons={modelIcons}
        detailBottom={getBottom(userInfo, project)}
      />
      {datasets.length && (
        <SalesDropdown
          edit
          options={datasets}
          datasetId={datasetId}
          getDatasetId={getDatasetId}
          selectedValues={secondaryDatasets}
          getSecondaryDataset={getSecondaryDataset}
          changeDataset={changeDataset}
          onChange={(): void => setDisableApllyDatasetSelection(false)}
          role={'TimeTrendRegression'}
          previousModel="Time Trend Model"
          actualModel="Land Model"
        />
      )}
      <CustomSection
        title="Regression analysis"
        iconText="Use Regression"
        iconOnClick={createRegression}
        key={regressionViewKey}
        extraIcons={extraIcons}
        disableIcon={projectContext.modelDetails?.isLocked}
      >
        {loadingRegression === 'deleteRegression' ? (
          <div className={classes.sectionLoader} style={{ height: '50px' }}>
            <Loader type="Oval" color="#00BFFF" height={80} width={80} />
          </div>
        ) : (
          renderEcuation()
        )}
      </CustomSection>
      <BlockUi blocking={runningSetup} loader={<></>}>
        <ScheduleGrid
          sectionTitle="Non-Waterfront schedule"
          isLoading={
            (isLoading && selectedVariableGrid === 'nonWater') ||
            (selectedSection === 'nonWater' && fromExcelLoading)
          }
          loadingRegression={loadingRegression}
          classes={classes}
          blockUI={getBlokedStatus('nonWater')}
          editing={
            projectContext.modelDetails?.isLocked || isEditing('nonWater')
          }
          colDefs={nonWaterColDefs}
          updateGridData={updateNonWaterColData}
          gridData={nonWaterfrontGridData}
          errorMessage={errorMessage}
          type="nonWater"
        >
          <div className={classes.icons}>{renderNonWaterfrontIcons()}</div>
        </ScheduleGrid>
        <SectionContainer title="Non-Waterfront Expressions">
          {loadingRegression === 'nonWaterExpression' ? (
            <div className={classes.sectionLoader}>
              <Loader type="Oval" color="#00BFFF" height={80} width={80} />
            </div>
          ) : (
            <Fragment>
              <ExpressionsGrid
                rowData={nonWaterFrontExpressionData}
                updateGridData={updateGridData('nonWater')}
                title="New non-waterfront expression"
                blocking={getBlokedStatus('nonWaterExpression')}
                editing={isEditing('nonWaterExpression')}
                editOrSave={disableContent('nonWaterExpression')}
                cancel={cleanBlock}
                isLocked={projectContext.modelDetails?.isLocked}
              />
              {errorMessage?.section === 'nonWaterExpression' &&
                errorMessage?.message && (
                  <ErrorMessage message={errorMessage.message} />
                )}
            </Fragment>
          )}
        </SectionContainer>
        <ScheduleGrid
          sectionTitle="Waterfront schedule"
          isLoading={
            (isLoading && selectedVariableGrid === 'water') ||
            (selectedSection === 'water' && fromExcelLoading)
          }
          loadingRegression={loadingRegression}
          classes={classes}
          blockUI={getBlokedStatus('water')}
          editing={projectContext.modelDetails?.isLocked || isEditing('water')}
          colDefs={waterColDefs}
          updateGridData={updateWaterColData}
          gridData={waterfrontGridData}
          errorMessage={errorMessage}
          type="water"
        >
          <div className={classes.icons}>{renderWaterfrontIcons()}</div>
        </ScheduleGrid>
        <SectionContainer title="Waterfront Expressions">
          {loadingRegression === 'waterExpression' ? (
            <div className={classes.sectionLoader}>
              <Loader type="Oval" color="#00BFFF" height={80} width={80} />
            </div>
          ) : (
            <Fragment>
              <ExpressionsGrid
                rowData={waterFrontExpressionData}
                updateGridData={updateGridData('water')}
                title="New waterfront expression"
                editing={isEditing('waterExpression')}
                blocking={getBlokedStatus('waterExpression')}
                cancel={cleanBlock}
                editOrSave={disableContent('waterExpression')}
                isLocked={projectContext.modelDetails?.isLocked}
              />
              {errorMessage?.section === 'waterExpression' &&
                errorMessage?.message && (
                  <ErrorMessage message={errorMessage.message} />
                )}
            </Fragment>
          )}
        </SectionContainer>
        <SectionContainer title="Adjustments">
          {loadingRegression === 'adjustment' ? (
            <div className={classes.sectionLoader}>
              <Loader type="Oval" color="#00BFFF" height={80} width={80} />
            </div>
          ) : (
            <Fragment>
              <LandVariableGrid
                rowData={positiveGridData}
                updateColData={updatePositiveAdjustentGrid}
                datasetId={datasetId}
                postProcessId={''}
                editing={isEditing('adjustment')}
                classes={classes}
              >
                {renderAdjustmentIcons()}
              </LandVariableGrid>
              {errorMessage?.section === 'adjustment' &&
                errorMessage?.message && (
                  <ErrorMessage message={errorMessage.message} />
                )}
            </Fragment>
          )}
        </SectionContainer>
        {defaultPostProcess && (
          <LandCollapsableGrid
            datasetId={datasetId}
            gridData={gridData}
            reloadGrid={reloadGridKey}
            datasets={datasets?.filter((s) =>
              [datasetId, ...secondaryDatasets].includes(s.datasetId)
            )}
            postProcess={defaultPostProcess}
            project={project}
          />
        )}
        <input
          type="file"
          key={excelKey}
          ref={inputFileNonWaterRef}
          onChange={(event): void => setFiles(event.target.files)}
          accept=".xlsx"
          multiple={false}
          style={{ display: 'none' }}
        />
        <input
          type="file"
          key={excelKey}
          ref={inputFileWaterRef}
          onChange={(event): void => setFiles(event.target.files)}
          accept=".xlsx"
          multiple={false}
          style={{ display: 'none' }}
        />
      </BlockUi>
    </Fragment>
  );
};

export default NewLandPage;
