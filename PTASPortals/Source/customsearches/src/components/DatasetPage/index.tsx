// DatasetPage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import {
  InsertDriveFile,
  // CloudDownload,
  // CloudUpload,
  Refresh,
  GetApp,
  Restore,
} from '@material-ui/icons';
import {
  CustomPopover,
  ErrorMessageContext,
  IconToolBarItem,
  NewFolderAcceptEvt,
  Save,
  SaveAcceptEvent,
  Banner,
} from '@ptas/react-ui-library';
import CustomHeader from 'components/common/CustomHeader';
import AgGrid from 'components/Grid';
import MessageDisplay from 'components/MessageDisplay';
import { AppContext } from 'context/AppContext';
import RestartApplyDialog from './RestartApplyDialog';
import ClearIcon from '@material-ui/icons/Clear';

import {
  useParams,
  useHistory,
  Link as LinkR,
  useLocation,
  RouteComponentProps,
} from 'react-router-dom';

import React, {
  ChangeEvent,
  Fragment,
  useContext,
  useEffect,
  useRef,
  useState,
} from 'react';
import { OverlayDisplay } from 'routes/models/View/Regression/elements';
import { DownloadFile, AxiosLoader } from 'services/AxiosLoader';
import {
  AgGridChildType,
  Dataset,
  DataSetInfo,
  GridDataMethods,
  IdOnly,
  JobItem,
} from 'services/map.typings';
import { makeStyles, Switch } from '@material-ui/core';
import { getUserProject } from 'services/common';
import { useAsync, useLifecycles } from 'react-use';
import useShowMapsMenu from 'components/common/useShowMapsMenu';
import Loading from 'components/Loading';
import moment from 'moment';

import ClearFilterModal from './ClearFilterModal';
import AgGridService from 'services/AgGridService';

const randStr = (): string | (() => string) => `${Math.random()}`;

const useStyles = makeStyles((theme) => ({
  saveSearch: {
    margin: 0,
    padding: theme.spacing(2, 4, 4, 4),
  },
  closeButton: {
    fontSize: 40,
  },
  saveSearchTitle: {
    fontSize: '1.375rem',
    fontFamily: theme.ptas.typography.bodyFontFamily,
  },
}));

interface FoldersToSave {
  folderPath: string;
  userId: string;
}

interface AssingData {
  folderPath: string;
}

interface RenameData {
  newName: string;
  newComments: string;
}

interface LinkInfo {
  linkTo: string;
  name: string;
}

interface DatasetTypeState {
  from: string;
  projectId?: number;
  postProcessId?: number;
  bulkUpdatePostProcessId: number;
  bulkUpdatePostProcessDate: string;
  withNoBulkUpdate: boolean;
}

interface JobInfo {
  jobId: number;
  datasetId: string;
}

const DatasetPage = ({
  location,
}: //eslint-disable-next-line
RouteComponentProps<{}, any, DatasetTypeState | any>): JSX.Element => {
  const childRef = React.useRef<AgGridChildType>(null);
  const locationDataset = useLocation<DatasetTypeState>();
  const context = useContext(AppContext);
  const classes = useStyles();
  const {
    id,
    datasetId,
    chartId,
    chartTitle,
  }: {
    id: string;
    datasetId: string;
    chartId: string;
    chartTitle: string;
  } = useParams();

  const [locationDatasetFrom, setLocationDatasetFrom] = useState<string>();
  const { showErrorMessage } = useContext(ErrorMessageContext);

  const [, setCountTotalRecords] = useState<number>(0);
  const [, setCountTotalSelection] = useState<number>(0);
  const [overlayMessage, setOverlayMessage] = useState('');
  const [datasetProjectRole, setDatasetProjectRole] = useState<string>('');
  const [errorMessage, setErrorMessage] = useState('');
  const [message, setMessage] = useState('');
  const [anchorElement, setAnchorElement] = useState<HTMLElement | null>(null);
  const [disableBulkUpdate, setDisableBulkUpdate] = useState<boolean>(false);
  const [bulkLastUpdate, setBulkLastUpdate] = useState<string>('');
  const [withNoBulkUpdate, setWithNoBulkUpdate] = useState<boolean>(true);

  const [dataset, setDataset] = useState<Dataset | null>();
  const [defaultName, setDefaultName] = useState<string>();
  const [defaultFolder, setDefaultFolder] = useState<string>();
  const [detailsTop, setDetailsTop] = useState<string>();
  const [loading, setLoading] = useState<boolean>(true);
  const [runningBulkUpdate, setRunningBulkUpdate] = useState<boolean>(false);
  const [openApply, setOpenApply] = useState<boolean>(false);

  const [postProcessId, setPostProcessId] = useState<number>(0);
  const [filterRowsCounter, setFilterRowsCounter] = useState<number>(0);
  const [openClearModal, setOpenClearModal] = useState<boolean>(false);

  const [projectName, setProjectName] = useState<string>();
  const [linkData, setLinkData] = useState<LinkInfo>({
    linkTo: '/',
    name: 'Home',
  });
  const history = useHistory();
  //const [comments, setComments] = useState<string>('');
  const [defaultComments, setDefaultComments] = useState<string>('');

  // const locationRouter = useLocation<GridLocationState>();

  const [lastKey, setLastKey] = useState(randStr());
  const [reloadKey, setReloadKey] = useState('');

  const reload = (): void => setLastKey(randStr());

  const showMapMenu = useShowMapsMenu();
  useLifecycles(
    () => showMapMenu(true),
    () => showMapMenu(false)
  );

  const getUserProjectLocalFn = async (): Promise<void> => {
    try {
      const data = await getUserProject(id);
      if (!data || !data.project) return;
      setProjectName(data.project.projectName);
    } catch (error) {
      showErrorMessage(JSON.stringify(error), 'CS', false);
    }
  };

  const getDataset = async (): Promise<void> => {
    try {
      const loader = new AxiosLoader<DataSetInfo, {}>();
      const data = await loader.GetInfo(
        `CustomSearches/GetDataset/${datasetId}`,
        {}
      );

      if (!data) return;
      setDefaultName('');
      setDatasetProjectRole(data.datasetProjectRole);
      setDefaultFolder('');
      setDataset(data.dataset);
      //setComments(data.dataset.comments);
      setDefaultComments(data.dataset.comments);
      const lastModifiedByUser =
        data.usersDetails &&
        data.dataset &&
        data.usersDetails.find((u) => u.id === data.dataset.lastModifiedBy);
      setDetailsTop(
        `Last sync on ${
          data.dataset.lastExecutionTimestamp
            ? new Date(
                `${data.dataset.lastExecutionTimestamp}Z`
              ).toLocaleString()
            : ''
        }, by ${
          lastModifiedByUser
            ? lastModifiedByUser.fullName
            : data.dataset.lastModifiedBy
        }`
      );

      if (!data?.dataset.folderPath.includes('/Unsaved')) {
        setDefaultFolder(data?.dataset.folderPath);
        setDefaultName(data?.dataset.datasetName);
      }
    } catch (error) {
      console.info(error);
    }
  };

  console.log(`dataset`, dataset);

  const inputFile = useRef(null);

  const cleanMessages = (): void => {
    setErrorMessage('');
    setMessage('');
  };

  const getDatasetMethods = (): GridDataMethods => {
    const data: GridDataMethods = {
      methods: {
        saveToFileClicked,
        refreshClicked,
        commitClicked,
        uploadFile,
      },
    };
    return data;
  };

  const saveToFileClicked = async (defaultFormat = 'xlsx'): Promise<void> => {
    let params = {};
    cleanMessages();
    if (defaultFormat === 'csv') {
      params = { format: 'csv' };
    }
    try {
      setOverlayMessage('Downloading file, this might take a few minutes...');
      const url =
        process.env.REACT_APP_CUSTOM_SEARCHES_URL ||
        'https://ptas-sbox-customsearchesfunctions.azurewebsites.net/v1.0/API/';
      await DownloadFile(
        `${url}CustomSearches/ExportDatasetDataToFile/${datasetId}`,
        `${datasetId}.${defaultFormat}`,
        params
      );
    } catch (error) {
      setErrorMessage(error.message);
    } finally {
      setOverlayMessage('');
    }
  };

  const refreshClicked = async (): Promise<void> => {
    cleanMessages();
    setOverlayMessage('Refreshing Dataset');
    try {
      const loader = new AxiosLoader<IdOnly, {}>();
      const url = `CustomSearches/RefreshDataset/${datasetId}`;
      const result = await loader.PutInfo(url, {}, {});
      const resultId = `${result?.id || 0}`;
      if (resultId === '-1') {
        setErrorMessage('Error: This dataset has no editable fields.');
        return;
      }
      if (resultId === '0') {
        setMessage('Sent refresh command');
        setOverlayMessage('');
        setTimeout(reload, 2000);
      } else startCheckingResults(resultId, 'Refresh');
    } catch (error) {
      setErrorMessage(error);
    } finally {
      setOverlayMessage('');
    }
  };

  const [isEnableRevert, setEnabledRevert] = useState(true);

  const commitClicked = async (): Promise<void> => {
    try {
      setOverlayMessage('Commiting Dataset');
      cleanMessages();
      const loader = new AxiosLoader<IdOnly, {}>();
      const url = `CustomSearches/ExecuteDatasetBackendUpdate/${datasetId}`;
      const result = await loader.PutInfo(url, {}, {});
      const resultId = `${result?.id}`;
      if (resultId === '-1') {
        setErrorMessage('Error: This dataset has no editable fields.');
        return;
      }
      startCheckingResults(resultId, 'Commited changes');
    } catch (error) {
      setErrorMessage(error);
    } finally {
      setOverlayMessage('');
    }
  };

  const uploadFile = (): void => {
    const f = inputFile?.current as null | { click: () => void };
    f?.click();
  };

  const populationIcons: IconToolBarItem[] = [
    {
      icon: <Refresh />,
      text: 'Refresh',
      onClick: refreshClicked,
    },
    {
      icon: <GetApp />,
      text: 'Commit changes',
      onClick: commitClicked,
    },
  ];

  const refreshApply = async (): Promise<void> => {
    setRunningBulkUpdate(true);
    const projectId = locationDataset.state?.projectId ?? id;
    try {
      const loader = new AxiosLoader<JobInfo, {}>();
      const url = `CustomSearches/ApplyModel/${projectId}`;

      const result = await loader.PutInfo(url, {}, {});
      if (result) {
        context?.handleJobId?.(result?.jobId);
        if (result.datasetId) {
          history.push(`/models/results/${id}/${result.datasetId}`);
        }
      }
      throw new Error('Error');
    } catch (error) {
      showErrorMessage(JSON.stringify(error), 'CS', false);
    } finally {
      setRunningBulkUpdate(false);
    }
  };

  const bulkUpdate = async (): Promise<void> => {
    try {
      setRunningBulkUpdate(true);
      //eslint-disable-next-line
      const loader = new AxiosLoader<any, {}>();
      const url = `CustomSearches/BulkUpdate/${datasetId}`;
      const result = await loader.PutInfo(url, {}, {});
      if (result) {
        context?.handleJobId?.(result?.id);
      }
      // setDisableBulkUpdate(true);
    } catch (error) {
      showErrorMessage(JSON.stringify(error), 'CS', false);
    } finally {
      setRunningBulkUpdate(false);
    }
  };

  const getChildren = (): JSX.Element => {
    if (!bulkLastUpdate.length) return <></>;
    return (
      <span style={{ color: 'green', fontWeight: 600 }}>
        Bulk Update successfully executed on{' '}
        {moment(bulkLastUpdate).format('MM/DD/YYYY')}{' '}
      </span>
    );
  };

  const applyModelIcons: IconToolBarItem[] = [
    {
      icon: <Refresh />,
      text: 'Restart Apply',
      onClick: (): void => {
        setOpenApply(true);
      },
    },
    {
      icon: <GetApp />,
      text: 'Bulk Update',
      onClick: bulkUpdate,
      disabled: disableBulkUpdate,
      node: getChildren(),
      nodePosition: 'right',
    },
  ];

  const defaultIcons: IconToolBarItem[] = [
    {
      icon: <InsertDriveFile />,
      text: 'Save dataset',
      onClick: (event): void => {
        setAnchorElement(event.currentTarget);
      },
    },
    {
      icon: <Refresh />,
      text: 'Refresh',
      onClick: refreshClicked,
    },
    {
      icon: <GetApp />,
      text: 'Commit Changes',
      onClick: commitClicked,
    },
    {
      icon: <Restore />,
      text: 'Revert All changes',
      disabled: isEnableRevert,
      onClick: (): void => {
        const f = childRef.current;
        f?.revertChanges();
        setEnabledRevert(true);
      },
    },
  ];

  const onAddTotalRecords = (x: number): void => {
    setCountTotalRecords(x);
  };
  const onAddTotalSelection = (x: number): void => {
    setCountTotalSelection(x);
  };

  const startCheckingResults = (id: string, message: string): void => {
    setOverlayMessage(`${message}, waiting for results...`);
    let tries = 0;
    const loader = new AxiosLoader<JobItem, {}>();

    const processStep = (): void => {
      setTimeout(async () => {
        try {
          const data = await loader.GetInfo(`Jobs/GetJobStatus/${id}`, {});
          if (data?.jobStatus === 'Finished') {
            setOverlayMessage('');
            reload();
            setMessage(
              `Status: ${data.jobStatus}. Result: ${data.jobResult.Status}`
            );
            return;
          } else {
            tries += 1;
            setOverlayMessage(`Try number ${tries}. Waiting for results.`);
            processStep();
          }
        } catch (error) {
          setErrorMessage(error.message);
          setOverlayMessage('');
        }
      }, 5000);
    };
    processStep();
  };

  useAsync(async () => {
    await context.getFolders?.();
  }, []);

  const fileSelected = async (event: ChangeEvent): Promise<void> => {
    try {
      setOverlayMessage('Uploading file, this might take a few minutes...');
      event.stopPropagation();
      event.preventDefault();
      const target = event.target as unknown as { files: File[] };
      const file = target.files[0];
      const loader = new AxiosLoader();
      const result = await loader.SaveFile(
        `CustomSearches/UpdateDatasetDataFromFile/${datasetId}`,
        file
      );
      // event.target.value = null;
      startCheckingResults(result.id, 'File uploaded');
    } catch (error) {
      setErrorMessage(error.message);
      setOverlayMessage('');
    }
  };

  const handleSave = async (event: SaveAcceptEvent): Promise<void> => {
    if (!event.folderName || !event.route) return;
    setAnchorElement(null);

    if (defaultFolder !== event.route) {
      await assignFolderToDataset(event.route);
      await renameDataset(event.folderName, event.comments as string);
      await getDataset();
      context.getDatasetsForUser && (await context.getDatasetsForUser());
      setMessage('Dataset successfully saved');
    } else {
      await renameDataset(event.folderName, event.comments as string);
      await getDataset();
      context.getDatasetsForUser && (await context.getDatasetsForUser());
      setMessage('Dataset successfully saved');
    }
  };

  const handleNewFolderSave = async (e: NewFolderAcceptEvt): Promise<void> => {
    await createDatasetFolder({
      folderPath: `${e?.route} ${e?.folderName}`,
      userId: context.currentUserId as string,
    });

    context.getFolders && (await context.getFolders());
  };

  const assignFolderToDataset = async (folderPath: string): Promise<void> => {
    if (!folderPath) return;
    try {
      const loader = new AxiosLoader<{}, AssingData>();
      await loader.PutInfo(
        `CustomSearches/AssignFolderToDataset/${datasetId}`,
        { folderPath: folderPath },
        {}
      );
    } catch (e) {
      setMessage(`${e}`);
      console.info(e);
    }
  };

  const renameDataset = async (
    name: string,
    newComments: string
  ): Promise<void> => {
    if (!name) return;
    try {
      const loader = new AxiosLoader<{}, RenameData>();
      await loader.PutInfo(
        `CustomSearches/UpdateDatasetAttributes/${datasetId}`,
        { newName: name, newComments: newComments },
        {}
      );
    } catch (e) {
      setMessage(`${e}`);
      console.error(e);
    }
  };

  const createDatasetFolder = async (folder: FoldersToSave): Promise<void> => {
    if (!folder) return;
    try {
      const loader = new AxiosLoader<{}, FoldersToSave>();

      await loader.PutInfo(`CustomSearches/CreateDatasetFolder/`, folder, {});
    } catch (error) {
      setMessage(`${error}`);
      console.info('Save folder error', error);
    }
  };

  const hasPath = (path: string): boolean =>
    !!location?.pathname.includes(path);

  const getLinkData = (): LinkInfo => {
    if (hasPath('/search/results'))
      return { linkTo: '/search/results', name: 'My search results' };
    if (hasPath('/new-search/'))
      return { linkTo: '/search/new-search/', name: 'New Search' };
    if (hasPath('/models/results/'))
      return { linkTo: '/models', name: 'Models' };
    return { linkTo: '/', name: 'Home' };
  };

  ///models/view-chart/201/09af3d2b-dca0-484a-94c1-8de3a71a54a7/245

  const breadCrumbs = [
    <LinkR to={linkData.linkTo} style={{ color: 'black' }}>
      {linkData.name}
    </LinkR>,
    projectName && <LinkR to={`/models/view/${id}`}>{projectName}</LinkR>,
    chartId && (
      <LinkR to={`/models/view-chart/${id}/${datasetId}/${chartId}`}>
        {chartTitle}
      </LinkR>
    ),
    <span>{dataset?.datasetName}</span>,
  ].filter((item) => !!item);

  const modelbreadCrumbs = [
    <LinkR
      to={(context?.routeFrom && context?.routeFrom) || '/'}
      style={{ color: 'black' }}
    >
      Model
    </LinkR>,
    projectName && <LinkR to={`/models/view/${id}`}>{projectName}</LinkR>,
    <span>{dataset?.datasetName}</span>,
  ].filter((item) => !!item);

  const callSetLinkData = (): void => {
    setLinkData(getLinkData);
    if (context.toogleDatasetMethods) {
      context.toogleDatasetMethods(getDatasetMethods());
    }
    return history.replace({ ...history.location, state: {} });
  };
  useEffect(callSetLinkData, []);

  useEffect(() => {
    context.setCurrentDatasetId && context.setCurrentDatasetId(datasetId);
  }, [context, datasetId]);

  useEffect(() => {
    if (context.setGridReference) {
      context.setGridReference(childRef);
    }
  }, [context, childRef]);

  const callSetLinkAndProject = (): void => {
    setLinkData(getLinkData);
    setProjectName(undefined);
    if (id) getUserProjectLocalFn();
    getDataset();
  };

  useEffect(callSetLinkAndProject, [datasetId, id]);

  useEffect(() => {
    setReloadKey(lastKey + '.' + datasetId);
  }, [datasetId, lastKey]);

  const onDatasetPageIsMounted = (): void => {
    updateDimensions();
    window.addEventListener('resize', updateDimensions);
    if (location?.pathname.includes('new-search')) {
      context.setRouteFrom && context.setRouteFrom('');
    }
    if (location?.pathname.includes('models')) {
      context.setRouteFrom && context.setRouteFrom(`/models/view/${id}`);
    }
    return window.removeEventListener('resize', updateDimensions);
  };

  useEffect(onDatasetPageIsMounted, []);

  const headerHeight = 150;

  const [height, setHeight] = useState(window.innerHeight - headerHeight);

  const updateDimensions = (): void => {
    const newHeight = window.innerHeight - headerHeight; //Combined height of header and options bar
    setHeight(newHeight);
  };

  useEffect(() => {
    setReloadKey(lastKey + '.' + datasetId);
  }, [datasetId, lastKey]);

  const handleEnabledRevert = (value = false): void => {
    setEnabledRevert(value);
  };

  const onClearFilter = async (value?: boolean): Promise<void> => {
    try {
      let params = {};
      if (value) {
        params = { preserveSelection: true };
      }
      await AgGridService.clearRowFilter(datasetId, params);
      reload();
    } catch (error) {
      showErrorMessage(JSON.stringify(error), 'CS', false);
    } finally {
      setOpenClearModal(false);
    }
  };

  const getIcons = (): IconToolBarItem[] => {
    let icons: IconToolBarItem[] = [...defaultIcons];
    let filteredIcons: IconToolBarItem[] = [];
    const modelName = ['Physical Inspection Model', 'Residential Bulk Update'];
    const isModelData = modelName.find((name) =>
      dataset?.datasetName.includes(name)
    );
    if (context.editGrid) {
      filteredIcons = [
        {
          icon: <InsertDriveFile />,
          text: 'Apply Filter',
          onClick: async (): Promise<void> => {
            await childRef.current?.applyFilter();
            reload();
          },
          disabled: !filterRowsCounter,
        },
        {
          icon: <ClearIcon />,
          text: 'Clear Filter',
          onClick: (): void => {
            setOpenClearModal(true);
          },
          disabled: !filterRowsCounter,
        },
      ];
    }
    if (
      ['population', 'sales', 'land'].includes(
        datasetProjectRole?.toLowerCase()
      )
    ) {
      icons = [...populationIcons];
    }

    if (isModelData) {
      icons = [...applyModelIcons];
    }
    icons = [
      ...icons,
      {
        icon: (
          <Switch
            checked={context.editGrid}
            name="editorGrid"
            className="FilterSwitch"
          />
        ),
        text: !context.editGrid
          ? 'Activate Filter Mode'
          : 'Desactivate Filter Mode',
        onClick: (): void => context?.handleEditGrid?.(),
      },
    ];
    return [...icons, ...filteredIcons];
  };

  useEffect(() => {
    if (locationDataset?.state?.from) {
      setLocationDatasetFrom(locationDataset?.state?.from);
    }
    if (locationDataset?.state?.postProcessId) {
      if (locationDataset.pathname.includes('duplicate')) {
        setPostProcessId(0);
      } else {
        setPostProcessId(locationDataset?.state?.postProcessId);
      }
    }
    if (locationDataset?.state?.bulkUpdatePostProcessId) {
      setDisableBulkUpdate(true);
    }
    if (locationDataset?.state?.bulkUpdatePostProcessDate) {
      setBulkLastUpdate(locationDataset?.state?.bulkUpdatePostProcessDate);
    }
    if (locationDataset?.state?.withNoBulkUpdate !== undefined) {
      setWithNoBulkUpdate(locationDataset?.state?.withNoBulkUpdate);
    }
  }, [locationDataset]);

  const renderBreadCrumbs = (): JSX.Element => {
    if (context?.routeFrom?.length) {
      return (
        <CustomHeader
          key={dataset?.datasetName ?? bulkLastUpdate}
          route={modelbreadCrumbs}
          icons={getIcons()}
          detailTop={detailsTop}
          detailBottom=""
        />
      );
    }

    return (
      <CustomHeader
        key={dataset?.datasetName}
        route={breadCrumbs}
        icons={getIcons()}
        detailTop={detailsTop}
        detailBottom=""
      />
    );
  };

  const getLoading = (value: boolean): void => {
    setLoading(value);
  };

  const handleFilteredRowsCounter = (value: number): void => {
    setFilterRowsCounter(value);
  };

  if (runningBulkUpdate) return <Loading />;

  const getPostProcessId = (): {} => {
    if (locationDataset?.pathname?.includes('duplicate')) return {};
    if (postProcessId) return { postProcessId: `${postProcessId}` };
    return {};
  };

  const renderBanner = (): JSX.Element => {
    // if (!dataset?.IsOutdated) {
    //   return <Fragment />;
    // }
    if (dataset?.IsOutdated && datasetProjectRole === null) {
      return (
        <Banner color="#F4EB49">
          <span style={{ color: '#000' }}>
            This dataset is outdated, re-execute the custom search.
          </span>
        </Banner>
      );
    }
    if (dataset?.IsOutdated && datasetProjectRole !== null) {
      return (
        <Banner color="#F4EB49">
          <span style={{ color: '#000' }}>
            This dataset is outdated, re-create the project.
          </span>
        </Banner>
      );
    }
    return <Fragment />;
  };

  return (
    <Fragment>
      {renderBanner()}
      {!loading && renderBreadCrumbs()}
      <RestartApplyDialog
        title={
          withNoBulkUpdate
            ? 'If you reapply the model any current changes done to the apply model data set will be lost, are you sure you want to continue?'
            : ''
        }
        open={openApply}
        toggle={(): void => setOpenApply(false)}
        accept={(): void => {
          try {
            refreshApply();
            setOpenApply(false);
          } catch (error) {
            console.log(`error`, error);
          }
        }}
      />
      <OverlayDisplay message={overlayMessage} />
      <MessageDisplay message={errorMessage} />
      <MessageDisplay message={message} color="green" />
      <AgGrid
        ref={childRef}
        datasetType={locationDatasetFrom}
        getLoading={getLoading}
        id={datasetId}
        {...getPostProcessId()}
        width="100%"
        height={`${height}px`}
        reloadGrid={reload}
        key={reloadKey}
        getTotalRecords={onAddTotalRecords}
        getTotalSelection={onAddTotalSelection}
        dataSet={dataset}
        handleFilteredRowsCounter={handleFilteredRowsCounter}
        setEnabledRevert={handleEnabledRevert}
      ></AgGrid>
      <input
        type="file"
        accept=".xlsx"
        ref={inputFile}
        style={{ display: 'none' }}
        onChange={async (event: ChangeEvent): Promise<void> => {
          setOverlayMessage('Uploading file, this might take a few minutes...');
          await fileSelected(event);
        }}
      />

      <CustomPopover
        border
        anchorEl={anchorElement}
        onClose={(): void => {
          setAnchorElement(null);
        }}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'right',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left',
        }}
        showCloseButton
      >
        <Save
          title="Save search"
          buttonText="Save"
          dropdownRows={context.folders ?? []}
          newFolderDropdownRows={context.folders ?? []}
          okClick={handleSave}
          newFolderOkClick={handleNewFolderSave}
          removeCheckBox
          defaultRoute={defaultFolder}
          defaultName={defaultName}
          classes={{
            root: classes.saveSearch,
            closeIcon: classes.closeButton,
            title: classes.saveSearchTitle,
          }}
          defaultComments={defaultComments}
          showComments
        />
      </CustomPopover>
      <ClearFilterModal
        isOpen={openClearModal}
        onSave={onClearFilter}
        onClose={(): void => setOpenClearModal(false)}
      />
    </Fragment>
  );
};

export default DatasetPage;
