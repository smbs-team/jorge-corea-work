// AppContext.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  FC,
  useState,
  PropsWithChildren,
  useEffect,
  useContext,
} from 'react';
import { ThemeProvider, CssBaseline } from '@material-ui/core';
import { AxiosLoader } from 'services/AxiosLoader';
import * as signalR from '@microsoft/signalr';

import {
  CustomSearchCategoryResults,
  CustomSearchCategory,
  CustomSearch,
  AgGridChildType,
  GridDataMethods,
  CommonValue,
  GlobalVariables,
  GlobalVariablesData,
  SignalRResponseType,
  DatasetHealthDataType,
  Project,
} from 'services/map.typings';
import { IntlProvider } from 'react-intl';
import { FormValues, FormDefinition } from 'components/FormBuilder/FormBuilder';
import { TreeDataset } from 'routes/models/View/NewTimeTrend/typings';
import { Folder, Folders } from 'routes/search/NewSearch/typings';
import {
  DropdownTreeRow,
  SnackState,
  SnackContext,
  SnackProvider,
  ErrorMessageProvider,
  ptasCamaTheme,
  // ErrorMessageContext,
} from '@ptas/react-ui-library';
import { uniqueId } from 'lodash';
import { getMetadataStoreItem } from 'services/common';
import { useList } from 'react-use';
import { ListActions } from 'react-use/lib/useList';
import { ForbiddenError } from 'components/ErrorPopup/ForbiddenError';
import ErrorFallback from 'components/common/ErrorFallback';
import { ErrorBoundary } from 'react-error-boundary';
import { appInsights } from 'services/appInsights';
import _ from 'underscore';
import { getProjectHealth } from 'routes/models/View/Projects/Land/services/projectServices';
import AgGridService from 'services/AgGridService';

/**
 * Application context interface
 */

interface RolesTeamsType {
  id: string;
  name: string;
}

interface CurrentUserInfoType {
  email: string;
  fullName: string;
  id: string;
  roles: RolesTeamsType[];
  teams: RolesTeamsType[];
}

export interface PostProcessInProgressType {
  jobId: number;
  postProcessRole: string;
  postProcessId: number;
  datasetId?: string;
}

interface HealtInfo {
  datasetId: string;
  section: string;
  postProcessId: number;
  issue: string;
  message: string;
  isProcessing?: boolean;
}

interface SectionHealtFailedExecute {
  label: string;
  postProcessId: number;
  datasetId: string;
  reason?: string;
}

export interface AppContextTyping {
  messages: SignalRResponseType[];
  connection: signalR.HubConnection;
  jobId: number;
  handleJobId: (jobId: number) => void;
  currentUserId: string;
  selectedParcels: string[];
  setSelectedParcels: (selectedParcels: string[]) => void;
  customSearchesParams: {
    customSearchCategories: CustomSearchCategory[] | undefined;
    customSearch: CustomSearch | null;
    setCustomSearch: (c: CustomSearch) => void;
  };
  formValues: FormValues | null;
  setFormValues: React.Dispatch<React.SetStateAction<FormValues | null>>;

  formDefinition: FormDefinition | null;
  setFormDefinition: React.Dispatch<
    React.SetStateAction<FormDefinition | null>
  >;

  treeDatasets: TreeDataset | null;
  setTreeDatasets: React.Dispatch<React.SetStateAction<TreeDataset | null>>;

  currentDatasetId: string;
  setCurrentDatasetId: React.Dispatch<React.SetStateAction<string>>;
  getDatasetsForUser: () => Promise<void>;

  shouldDisplayVariablesGrid: boolean;
  toggleDisplayVariableGrid: () => void;

  selectAllRows: boolean;
  selectAllRowsAction: (state: boolean) => void;

  unselectAllRows: boolean;
  unselectAllRowsAction: (state: boolean) => void;

  shouldDisplayAutoFitGrid: boolean;
  toggleAutoFitColumns: () => void;

  datasetMethods: GridDataMethods;
  toogleDatasetMethods: (params: GridDataMethods) => void;

  currentGridRef?: React.RefObject<AgGridChildType>;
  setGridReference?: (gridRef: React.RefObject<AgGridChildType>) => void;

  folders: DropdownTreeRow[];
  setFolders: React.Dispatch<React.SetStateAction<DropdownTreeRow[]>>;
  getFolders: () => Promise<void>;

  snackBar: SnackState;
  setSnackBar: React.Dispatch<React.SetStateAction<SnackState>>;

  postProcessName: string | null;
  setPostProcessName: React.Dispatch<React.SetStateAction<string | null>>;

  globalVariablesCategories: string[];
  setGlobalVariablesCategories: React.Dispatch<React.SetStateAction<string[]>>;

  globalVariables: GlobalVariables[];
  globalVariablesMethods: ListActions<GlobalVariables>;

  disableVariablesMenu: boolean;
  setDisableVariablesMenu: React.Dispatch<React.SetStateAction<boolean>>;

  globalVariablesToAdd: GlobalVariables[];
  setGlobalVariablesToAdd: React.Dispatch<
    React.SetStateAction<GlobalVariables[]>
  >;

  shouldHideSelectedRows: boolean;
  toggleHideSelectedRows: (value: boolean) => void;

  shouldShowSelectedRows: boolean;
  toggleShowSelectedRows: (value: boolean) => void;

  shouldDuplicateDataset: boolean;
  toggleDuplicateDataset: () => void;

  shouldDuplicateFullDataset: boolean;
  toggleDuplicateFullDataset: () => void;

  shouldDuplicateFilteredDataset: boolean;
  toggleDuplicateFilteredDataset: () => void;

  shouldDuplicateFilteredAndSelectedDataset: boolean;
  toggleDuplicateFilteredAndSelectedDataset: () => void;

  saveFilteredRows: boolean;
  toggleSaveFilteredRows: () => void;

  shouldDeleteSelectedRows: boolean;
  toggleDeleteSelectedRows: () => void;

  shouldShowColumns: boolean;
  toggleShowColumns: () => void;

  routeFrom: string;
  setRouteFrom: (route: string) => void;

  connnectionIsStarted: boolean;

  showMapMenu: boolean;
  setShowMapMenu: React.Dispatch<React.SetStateAction<boolean>>;

  countTotalRecords: number;
  toogleCountTotalRecords: (count: number) => void;

  shouldHideDataColumns: boolean;
  toggleHideDataColumns: () => void;

  shouldHideVariablesColumns: boolean;
  toggleHideVariablesColumns: () => void;

  postProcessId: number;
  setPostProcessId: (value: number) => void;

  postProcessInProgress?: PostProcessInProgressType[];
  setPostProcessInProgress?: (pp: PostProcessInProgressType[]) => void;
  handlePostProcessInProgress?: (
    postProcessesInProgress: PostProcessInProgressType
  ) => void;
  setCallFolders: (value: boolean) => void;

  healtInfo: DatasetHealthDataType[];
  sectionError: SectionHealtFailedExecute;
  setProjectId: (value: string) => void;

  editGrid: boolean;
  handleEditGrid: () => void;

  clearDebounceHealt: () => void;

  modelDetailsProject: Project | null;
  setModelDetailsProject: React.Dispatch<React.SetStateAction<Project | null>>;
}

let debounced: (() => Promise<void>) & _.Cancelable;
let debouncedHealth: (() => Promise<void>) & _.Cancelable;

/**
 * Initialize the app context
 */
export const AppContext = React.createContext<Partial<AppContextTyping>>({});

/**
 * A Context provider component
 *
 * @remarks
 * This component scope is private, because its exposed through the hoc
 * @param props -Arbitrary props
 */
const AppProvider = (props: PropsWithChildren<{}>): JSX.Element => {
  const [currentUserId, setCurrentUserId] = useState<string>(''); //F639C0EE-8272-E911-A976-001DD800D5B0

  const selectedParcelsKey = 'selectedParcels';
  const [customSearchCategories, setCustomSearchCategories] = useState<
    CustomSearchCategory[] | undefined
  >([]);
  const [postProcessInProgress, setPostProcessInProgress] = useState<
    PostProcessInProgressType[]
  >([]);

  const [healtInfo, setHealtInfo] = useState<DatasetHealthDataType[]>([]);
  const [sectionError, setSectionError] = useState<SectionHealtFailedExecute>();
  const [countTotalRecords, setCountTotalRecords] = useState<number>(0);
  const [projectId, setProjectId] = useState<string>('');
  // const previousProjectId = usePrevious<string>(projectId);
  const [jobId, setJobId] = useState<number>(0);
  const [postProcessId, setPostProcessId] = useState<number>(0);
  const [connection, setConnection] = useState<signalR.HubConnection>();
  const [messages, setMessages] = useState<SignalRResponseType[]>([]);
  const [openForbiddenModal, setOpenForbiddenModal] = useState<string>('');
  const [editGrid, setEditGrid] = useState<boolean>(false);

  const [shouldDuplicateDataset, setDuplicateDataset] =
    useState<boolean>(false);
  const [shouldDuplicateFullDataset, setDuplicateFullDataset] =
    useState<boolean>(false);

  const [shouldHideDataColumns, setShouldHideDataColumns] =
    useState<boolean>(false);

  const [openModal, setOpenModal] = useState<boolean>(false);
  const [saveFilteredRows, setSaveFilteredRows] = useState<boolean>(false);
  const [modelDetailsProject, setModelDetailsProject] =
    useState<Project | null>(null);

  const handleEditGrid = (): void => {
    setEditGrid((prev) => !prev);
  };

  const toggleSaveFilteredRows = (): void => {
    setSaveFilteredRows(!saveFilteredRows);
  };

  const toggleHideDataColumns = (): void => {
    setShouldHideDataColumns(!shouldHideDataColumns);
  };

  const [shouldHideVariablesColumns, setShouldHideVariablesColumns] =
    useState<boolean>(false);

  const toggleHideVariablesColumns = (): void => {
    setShouldHideVariablesColumns(!shouldHideVariablesColumns);
  };

  const toogleCountTotalRecords = (count: number): void => {
    setCountTotalRecords(count);
  };

  const handleJobId = (jobId: number): void => {
    setJobId(jobId);
  };

  useEffect(() => {
    if (jobId) {
      getJobStatus();
    }
    //eslint-disable-next-line
  }, [jobId]);

  const getJobStatus = async (): Promise<void> => {
    try {
      const jobStatus = await AgGridService.getJobStatus(jobId ?? 0);
      const status = jobStatus?.jobStatus === 'Finished';
      if (!status) {
        debounced = _.debounce(getJobStatus, 5000);
        debounced();
      }
    } finally {
      handleJobId?.(0);
    }
  };

  const [shouldDuplicateFilteredDataset, setDuplicateFilteredDataset] =
    useState<boolean>(false);

  const [
    shouldDuplicateFilteredAndSelectedDataset,
    setDuplicateFilteredAndSelectedDataset,
  ] = useState<boolean>(false);

  const [shouldDeleteSelectedRows, setDeleteSelectedRows] =
    useState<boolean>(false);

  const [shouldShowColumns, setShowColumns] = useState<boolean>(false);

  const toggleShowColumns = (): void => {
    setShowColumns(!shouldShowColumns);
  };

  const callHealthInfo = async (): Promise<void> => {
    const healthInfo = await getProjectHealth(projectId);
    if (healthInfo) {
      setHealtInfo(healthInfo?.datasetHealthData);
    }
    healthInfo?.datasetHealthData.forEach((healtData) => {
      healtData?.postProcessHealthData?.forEach((pp) => {
        if (pp.postProcessHealthIssue === 'ExecutionFailed') {
          const sectionFailed = [
            'TimeTrendRegression',
            'MultipleRegression',
            'LandRegression',
            'SupplementalAndException',
          ].find((e) => pp.postProcessHealthMessage.includes(e));
          if (sectionFailed) {
            setSectionError({
              label: sectionFailed,
              postProcessId: pp.datasetPostProcessId,
              datasetId: healtData.datasetId,
              reason: pp.postProcessHealthMessage,
            });
          } else {
            const landModelSectionFailed = [
              'LandSchedule',
              'LandAdjustment',
              'WaterfrontSchedule',
              'NonWaterfrontExpressions',
              'WaterfrontExpressions',
            ].find((e) => pp.postProcessHealthMessage.includes(e));
            if (landModelSectionFailed) {
              setSectionError({
                label: landModelSectionFailed,
                postProcessId: pp.datasetPostProcessId,
                datasetId: healtData.datasetId,
              });
            }
          }
        }
      });
    });
  };

  const initCallHealtApi = async (): Promise<void> => {
    await callHealthInfo();
    debouncedHealth = _.debounce(initCallHealtApi, 30000);
    debouncedHealth();
  };

  useEffect(() => {
    if (projectId) {
      debouncedHealth?.cancel();
      initCallHealtApi();
    }
    // if (projectId !== previousProjectId && previousProjectId?.length) {
    //   debounced.cancel();
    // }
    //eslint-disable-next-line
  }, [projectId]);

  const [routeFrom, setRouteFrom] = useState<string>('');

  const [customSearch, setCustomSearch] = useState<CustomSearch | null>(null);
  const [currentDatasetId, setCurrentDatasetId] = useState<string>('');
  const [callFolders, setCallFolders] = useState<boolean>(false);
  const [formValues, setFormValues] = useState<FormValues | null>(null);
  const [formDefinition, setFormDefinition] = useState<FormDefinition | null>(
    null
  );
  const [treeDatasets, setTreeDatasets] = useState<TreeDataset | null>(null);

  const [shouldHideSelectedRows, setHideSelectedRows] =
    useState<boolean>(false);

  const [shouldShowSelectedRows, setShowSelectedRows] =
    useState<boolean>(false);

  const [selectedParcels, setSelectedParcels] = useState<string[]>(
    JSON.parse(localStorage.getItem(selectedParcelsKey) || '[]')
  );

  const toggleDuplicateFilteredAndSelectedDataset = (): void => {
    setDuplicateFilteredAndSelectedDataset(
      !shouldDuplicateFilteredAndSelectedDataset
    );
  };

  const toggleDuplicateDataset = (): void => {
    setDuplicateDataset(!shouldDuplicateDataset);
  };

  const toggleDuplicateFullDataset = (): void => {
    setDuplicateFullDataset(!shouldDuplicateFullDataset);
  };

  const toggleDuplicateFilteredDataset = (): void => {
    setDuplicateFilteredDataset(!shouldDuplicateFilteredDataset);
  };

  const [shouldDisplayVariablesGrid, setdisplayVariableGrid] =
    useState<boolean>(false);

  const [selectAllRows, setSelectAllRows] = useState<boolean>(false);
  const [unselectAllRows, setUnselectAllRows] = useState<boolean>(false);

  const [shouldDisplayAutoFitGrid, setdisplayAutoFitGrid] =
    useState<boolean>(false);

  const [folders, setFolders] = useState<DropdownTreeRow[]>([]);
  const { setSnackState: setSnackBar } = useContext(SnackContext);
  const [datasetMethods, setDatasetMethods] = useState<GridDataMethods>();
  const [postProcessName, setPostProcessName] = useState<string | null>(null);

  const [connnectionIsStarted, setConnnectionIsStarted] =
    useState<boolean>(false);

  const toggleAutoFitColumns = (): void => {
    setdisplayAutoFitGrid(!shouldDisplayAutoFitGrid);
  };
  const toggleDisplayVariableGrid = (): void => {
    setdisplayVariableGrid(!shouldDisplayVariablesGrid);
  };

  const toggleShowSelectedRows = (value: boolean): void => {
    setShowSelectedRows(value);
  };
  const selectAllRowsAction = (state: boolean): void => {
    setSelectAllRows(state);
  };

  const toggleDeleteSelectedRows = (): void => {
    setDeleteSelectedRows(!shouldDeleteSelectedRows);
  };

  const handlePostProcessInProgress = (pp: PostProcessInProgressType): void => {
    setPostProcessInProgress((prev) => [...prev, pp]);
  };

  const unselectAllRowsAction = (state: boolean): void => {
    setUnselectAllRows(state);
  };

  const toogleDatasetMethods = (methods: GridDataMethods): void => {
    setDatasetMethods(methods);
  };

  const [currentGridRef, setCurrentGridRef] =
    useState<React.RefObject<AgGridChildType>>();

  const [globalVariablesCategories, setGlobalVariablesCategories] = useState<
    string[]
  >([]);

  const [globalVariables, globalVariablesMethods] = useList<GlobalVariables>(
    []
  );

  const [disableVariablesMenu, setDisableVariablesMenu] =
    useState<boolean>(true);

  const [globalVariablesToAdd, setGlobalVariablesToAdd] = useState<
    GlobalVariables[]
  >([]);

  const [showMapMenu, setShowMapMenu] = useState<boolean>(false);

  const setGridReference = (
    gridRef: React.RefObject<AgGridChildType>
  ): void => {
    setCurrentGridRef(gridRef);
  };

  const setAndbroadcastSelectedParcels = (selectedParcels: string[]): void => {
    setSelectedParcels(selectedParcels);
    window.localStorage.setItem(
      selectedParcelsKey,
      JSON.stringify(selectedParcels)
    );
  };

  const toggleHideSelectedRows = (value: boolean): void => {
    setHideSelectedRows(value);
  };

  const initSignalRConnection = (): void => {
    const token = localStorage.getItem('magicToken');
    if (!token) return;
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env.REACT_APP_CUSTOM_SEARCHES_URL}`, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();
    setConnection(connection);
  };

  useEffect(() => {
    if (connection) startConnection();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [connection]);

  const startConnection = async (): Promise<void> => {
    try {
      await connection?.start();
      setConnnectionIsStarted(true);
      console.log('SignalR Connected.');
      connection?.on('JobProcessed', messageReceived);
      // connection?.on('DatasetSelectionChanged', messageReceived);
    } catch (error) {
      console.log(error);
      setTimeout(startConnection, 5000);
    }
  };

  const messageReceived = (
    arg1: unknown,
    arg2: unknown,
    arg3: unknown,
    arg4: unknown
  ): void => {
    const message = {
      jobType: arg1,
      jobId: arg2,
      jobStatus: arg3,
      payload: arg4,
    };
    const messageList = [...messages];
    if (messageList.length === 100) messageList.shift();
    setMessages([...messageList, message] as SignalRResponseType[]);
  };

  useEffect(() => {
    initSignalRConnection();
    window.addEventListener('storage', (e: StorageEvent) => {
      if (e.key === selectedParcelsKey) {
        setSelectedParcels(JSON.parse(e.newValue || '[]'));
      }
    });
  }, []);

  useEffect(() => {
    const fetchData = async (): Promise<void> => {
      try {
        const response = await getMetadataStoreItem(
          'globalVariables',
          'category'
        );
        if (!response || response.metadataStoreItems.length < 1) return;
        const value = response.metadataStoreItems[0].value as CommonValue;

        if (value.data.length < 1) return;

        setGlobalVariablesCategories(value.data.map((d) => d.toUpperCase()));
      } catch (error) {
        setSnackBar({
          text: 'Getting metadata failed: category',
          severity: 'error',
        });
      }
    };
    fetchData();
  }, [setSnackBar]);

  const fetchVariablesData = async (): Promise<void> => {
    try {
      const response = await getMetadataStoreItem(
        'globalVariables',
        'variables'
      );
      if (!response) return;
      if (response.metadataStoreItems.length < 1) return;

      const value = response.metadataStoreItems[0].value as GlobalVariablesData;
      globalVariablesMethods.push(...value.data);
    } catch (error) {
      if (openModal) return;
      if (error?.status === 403) {
        setOpenForbiddenModal(error.message);
        setOpenModal(true);
      }
      // setSnackBar({
      //   text: 'Getting metadata failed: variables',
      //   severity: 'error',
      // });
    }
  };

  useEffect(() => {
    fetchVariablesData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    const fetchData = async (): Promise<void> => {
      try {
        const categoryLoader = new AxiosLoader<
          CustomSearchCategoryResults,
          {}
        >();
        const info = await categoryLoader.GetInfo(
          'CustomSearches/GetCustomSearchCategories',
          {}
        );
        const categories =
          info?.customSearchCategories.sort((a, b) =>
            a.name > b.name ? 1 : -1
          ) || [];
        setCustomSearchCategories(categories);
        await getCurrentUserInfo();
      } catch (error) {
        console.log(error.description);
      }
    };
    fetchData();
  }, []);

  const getCurrentUserInfo = async (): Promise<string> => {
    try {
      const loader = new AxiosLoader<CurrentUserInfoType, {}>();
      const info = await loader.GetInfo('Auth/GetCurrentUserInfo', {});
      if (info) {
        setCurrentUserId(info.id);
        return info.id;
      }
    } catch (error) {
      console.log(error.description);
    }
    return '';
  };

  const getDatasetsForUser = async (): Promise<void> => {
    try {
      const loader = new AxiosLoader<TreeDataset, {}>();
      const data = await loader.GetInfo(
        `CustomSearches/GetDatasetsForUser/${currentUserId}`,
        {}
      );
      setTreeDatasets(data);
    } catch (error) {
      console.log(error.description);
    }
  };

  const getDataAndFolder = (): void => {
    if (currentUserId) {
      getDatasetsForUser();
    }
  };

  useEffect(getDataAndFolder, [currentUserId]);

  useEffect(() => {
    if (callFolders && currentUserId.length) {
      getFolders();
    }
    //eslint-disable-next-line
  }, [callFolders, currentUserId]);

  const getFolders = async (): Promise<void> => {
    let userId = '';
    if (!currentUserId) {
      userId = await getCurrentUserInfo();
    }

    try {
      const loader = new AxiosLoader<Folders, {}>();
      const data = await loader.GetInfo(
        `CustomSearches/GetDatasetFoldersForUser/${
          userId ? userId : currentUserId
        }`,
        {}
      );
      if (!data) return;
      let toAdd: DropdownTreeRow[] = [];
      flatFolderList(data.folders, toAdd);

      if (toAdd.find((f) => f.title === 'Unsaved')) {
        toAdd = toAdd.filter((f) => f.title !== 'Unsaved');
      }

      if (!toAdd.find((f) => f.title === 'User' && f.parent === null)) {
        toAdd.push({
          id: uniqueId(),
          parent: null,
          title: 'User',
          subject: '',
          folder: true,
        });
      }
      if (!toAdd.find((f) => f.title === 'Shared' && f.parent === null)) {
        toAdd.push({
          id: uniqueId(),
          parent: null,
          title: 'Shared',
          subject: '',
          folder: true,
        });
      }
      setFolders(toAdd);
    } catch (e) {
      console.log(e.description);
    } finally {
      setCallFolders(false);
    }
  };

  const clearDebounceHealt = (): void => {
    debounced?.cancel();
  };

  const flatFolderList = (
    folders: Folder[],
    flatList: DropdownTreeRow[]
  ): void => {
    folders.forEach((f) => {
      if (f.children) {
        flatFolderList(f.children, flatList);
      }
      flatList.push({
        id: f.folderId,
        parent: f.parentFolderId,
        title: f.folderName,
        subject: f.folderName,
        folder: true,
      });
    });
  };

  const handleModal = (): void => {
    setOpenModal(false);
  };

  //056fa904-c450-434d-9b3d-052fa01b5dcd
  //2BEBF379-5137-4E53-B08F-D0BF45F3C0C4
  return (
    <AppContext.Provider
      value={{
        currentUserId: currentUserId,
        selectedParcels: selectedParcels,
        customSearchesParams: {
          customSearchCategories,
          customSearch,
          setCustomSearch,
        },
        formValues,
        formDefinition,
        setFormDefinition,
        setFormValues,
        treeDatasets,
        currentDatasetId,
        setSelectedParcels: setAndbroadcastSelectedParcels,
        setCurrentDatasetId,
        getDatasetsForUser,
        shouldDisplayVariablesGrid,
        shouldDisplayAutoFitGrid,
        toggleDisplayVariableGrid,
        selectAllRowsAction,
        selectAllRows,
        unselectAllRows,
        unselectAllRowsAction,
        datasetMethods,
        toogleDatasetMethods,
        toggleAutoFitColumns,
        currentGridRef,
        setGridReference,
        folders,
        setFolders,
        getFolders,
        setSnackBar,
        postProcessName,
        setPostProcessName,
        globalVariablesCategories,
        globalVariables,
        globalVariablesMethods,
        disableVariablesMenu,
        setDisableVariablesMenu,
        globalVariablesToAdd,
        setGlobalVariablesToAdd,
        connection,
        messages,
        toggleHideSelectedRows,
        shouldHideSelectedRows,
        toggleShowSelectedRows,
        shouldShowSelectedRows,
        toggleDuplicateDataset,
        shouldDuplicateDataset,
        shouldDeleteSelectedRows,
        toggleDeleteSelectedRows,
        routeFrom,
        setRouteFrom,
        toggleDuplicateFilteredDataset,
        shouldDuplicateFilteredDataset,
        toggleDuplicateFilteredAndSelectedDataset,
        shouldDuplicateFilteredAndSelectedDataset,
        shouldShowColumns,
        toggleShowColumns,
        connnectionIsStarted,
        jobId,
        handleJobId,
        showMapMenu,
        setShowMapMenu,
        countTotalRecords,
        toogleCountTotalRecords,
        shouldHideDataColumns,
        toggleHideDataColumns,
        shouldHideVariablesColumns,
        toggleHideVariablesColumns,
        postProcessId,
        setPostProcessId,
        postProcessInProgress,
        setPostProcessInProgress,
        handlePostProcessInProgress,
        setCallFolders,
        shouldDuplicateFullDataset,
        toggleDuplicateFullDataset,
        toggleSaveFilteredRows,
        saveFilteredRows,
        healtInfo,
        sectionError,
        setProjectId,
        handleEditGrid,
        editGrid,
        clearDebounceHealt,
        modelDetailsProject,
        setModelDetailsProject,
      }}
    >
      <ForbiddenError
        openModal={openModal}
        message={openForbiddenModal}
        handleClose={handleModal}
      />
      {props.children}
    </AppContext.Provider>
  );
};

const lang = {};
export const withAppProvider =
  (Component: FC) =>
  (props: object): JSX.Element =>
    (
      <ThemeProvider theme={ptasCamaTheme}>
        <CssBaseline />
        <SnackProvider>
          <IntlProvider locale={'en'} messages={lang}>
            <ErrorBoundary FallbackComponent={ErrorFallback}>
              <ErrorMessageProvider
                onClickReport={(error): void => {
                  appInsights.trackException({
                    exception: new Error(error),
                  });
                }}
              >
                <AppProvider>
                  <Component {...props} />
                </AppProvider>
              </ErrorMessageProvider>
            </ErrorBoundary>
          </IntlProvider>
        </SnackProvider>
      </ThemeProvider>
    );
