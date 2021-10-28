// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { Fragment, memo, useContext, useState, useEffect } from 'react';
import SearchMenu from './SearchMenu';
import ModelMenu from './ModelMenu';
import MapMenu from './MapMenu';
import {
  ErrorMessageContext,
  MenuRoot,
  MenuRootContent,
  RenderMenuProps,
} from '@ptas/react-ui-library';
import NestedMenuItem from 'material-ui-nested-menu-item';
import LinkMenuItem from './LinkMenuItem';
import { Box, Divider, makeStyles, Tooltip } from '@material-ui/core';
import { AppContext } from 'context/AppContext';
import { gridOptionsMenu } from './gridOptionsMenu';
import { useHistory } from 'react-router-dom';
import GlobalVariablesModal from 'components/common/GlobalVariablesModal';
import AddVariablesModal from 'components/common/AddVariablesModal';
import '../../assets/grid-styles/appbar.scss';
import { StringParam, useQueryParam } from 'use-query-params';
import LocalHospitalIcon from '@material-ui/icons/LocalHospital';
import { executeDatasetPostProcess } from 'routes/models/View/Projects/Land/services/landServices';
import NotificationTimeLine from '../NotificationTimeline';
import { freezeProject } from 'services/FreezeProjectService';

const useStyles = makeStyles((theme) => ({
  menu: {
    marginLeft: 'auto',
  },
  root: {
    fontSize: '0.875rem',
    transition: 'none !important',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    '&:hover': {
      backgroundColor: theme.ptas.colors.theme.grayLight,
    },
  },
}));

const AppBarMenu = (): JSX.Element => {
  const classes = useStyles();
  // const location = useLocation();
  const history = useHistory();
  const [openGlobalVariablesCategoty, setOpenGlobalVariablesCategory] =
    useState<boolean>(false);

  const [openAddVariableModal, setOpenAddVariableModal] =
    useState<boolean>(false);

  const [showHealthIcon, setShowHealthIcon] = useState<boolean>(false);

  const [view, setView] = useState<string | null>(localStorage.getItem('view'));
  const [viewParam] = useQueryParam('view', StringParam);

  const viewMode = localStorage.getItem('view');

  const {
    shouldDisplayVariablesGrid,
    toggleDisplayVariableGrid,
    toggleAutoFitColumns,
    currentGridRef,
    datasetMethods,
    disableVariablesMenu,
    toggleHideSelectedRows,
    toggleShowSelectedRows,
    selectAllRowsAction,
    unselectAllRowsAction,
    toggleDuplicateDataset,
    toggleDuplicateFullDataset,
    toggleDeleteSelectedRows,
    toggleDuplicateFilteredDataset,
    toggleShowColumns,
    showMapMenu,
    toggleHideDataColumns,
    shouldHideDataColumns,
    shouldHideVariablesColumns,
    toggleHideVariablesColumns,
    toggleDuplicateFilteredAndSelectedDataset,
    sectionError,
    healtInfo,
    toggleSaveFilteredRows,
    handleJobId,
    modelDetailsProject,
  } = useContext(AppContext);

  const { showErrorMessage } = useContext(ErrorMessageContext);

  console.log(`viewMode`, viewMode);

  useEffect(() => {
    if (viewParam) {
      localStorage.setItem('view', viewParam);
      setView(viewParam);
    }
  }, [viewParam]);

  useEffect(() => {
    const {
      location: { pathname },
    } = history;
    const isProjectListPage = pathname === '/models/' || pathname === '/models';
    const pathIncludes =
      !isProjectListPage &&
      ['/models/', '/datasets/', '/create-land-model/'].some((p) =>
        pathname.includes(p)
      );
    setShowHealthIcon(pathIncludes);
    //eslint-disable-next-line
  }, [history.location.pathname]);

  const getLabel = (label: string): string => {
    switch (label) {
      case 'Show/Hide data columns':
        if (shouldHideDataColumns) return 'Show data columns';
        return 'Hide data columns';
      case 'Show/Hide variables columns':
        if (shouldHideVariablesColumns) return 'Show variables columns';
        return 'Hide variables columns';
      default:
        return label;
    }
  };

  const menu = ({
    menuProps,
    isMenuRootOpen,
  }: RenderMenuProps): JSX.Element => {
    const onClickMenu = (label: string, parentLabel: string) => (): void => {
      if (parentLabel === 'Columns') {
        switch (label) {
          case 'Show all':
            if (toggleShowColumns) return toggleShowColumns();
            break;
          default:
            break;
        }
      }
      if (parentLabel === 'Group') {
        switch (label) {
          case 'Show/Hide data columns':
            toggleHideDataColumns && toggleHideDataColumns();
            break;
          case 'Show/Hide variables columns':
            toggleHideVariablesColumns && toggleHideVariablesColumns();
            break;
          default:
            break;
        }
      }
      switch (label) {
        case 'Autofit':
          if (toggleAutoFitColumns) {
            return toggleAutoFitColumns();
          }
          break;
        case 'Export to Excel...':
          if (datasetMethods?.methods.saveToFileClicked) {
            datasetMethods?.methods?.saveToFileClicked();
          }
          break;
        case 'Export to CSV...':
          if (datasetMethods?.methods.saveToFileClicked) {
            datasetMethods?.methods?.saveToFileClicked('csv');
          }
          break;
        case 'Import...':
          if (datasetMethods?.methods.uploadFile) {
            datasetMethods?.methods?.uploadFile();
          }
          break;
        case 'Commit changes...':
          if (datasetMethods?.methods.commitClicked) {
            datasetMethods?.methods?.commitClicked();
          }
          break;
        case 'Refresh data...':
          if (datasetMethods?.methods.refreshClicked) {
            datasetMethods?.methods?.refreshClicked();
          }
          break;
        case 'Reset to default':
          currentGridRef?.current?.revertChanges();
          break;
        case 'Hide selected':
          if (toggleHideSelectedRows) {
            toggleHideSelectedRows(true);
          }
          break;
        case 'Show all':
          if (toggleShowSelectedRows) {
            toggleShowSelectedRows(true);
          }
          break;
        case 'Select all':
          if (selectAllRowsAction && unselectAllRowsAction) {
            selectAllRowsAction(true);
            unselectAllRowsAction(false);
          }
          break;
        case 'Unselect all':
          if (unselectAllRowsAction && selectAllRowsAction) {
            unselectAllRowsAction(true);
            selectAllRowsAction(false);
          }
          break;
        case 'Create dataset from selected rows...':
          if (toggleDuplicateDataset) toggleDuplicateDataset();
          break;
        case 'Create dataset from current dataset':
          if (toggleDuplicateFullDataset) {
            toggleDuplicateFullDataset();
          }
          break;
        case 'Remove filtered rows...':
          if (toggleDeleteSelectedRows) toggleDeleteSelectedRows();
          break;
        case 'Create dataset from filtered rows...':
          if (toggleDuplicateFilteredDataset) toggleDuplicateFilteredDataset();
          break;
        case 'Create dataset from selected rows and filtered rows...':
          if (toggleDuplicateFilteredAndSelectedDataset)
            toggleDuplicateFilteredAndSelectedDataset();
          break;
        default:
          break;
      }
    };

    const toggleGrid = (): void => {
      if (toggleDisplayVariableGrid) {
        toggleDisplayVariableGrid();
      }
    };
    return (
      <MenuRootContent {...menuProps}>
        {gridOptionsMenu.map((m, i) => (
          <span key={i}>
            <NestedMenuItem
              label={
                <Box component="span" width="100%">
                  {m.label}
                </Box>
              }
              parentMenuOpen={isMenuRootOpen}
              className={classes.root}
              button
            >
              {m?.childrens?.map((ch, i) => (
                <span key={i}>
                  <LinkMenuItem
                    display={getLabel(ch.label)}
                    closeMenu={onClickMenu(ch.label, m.label)}
                    avoidNavigation={true}
                  />
                  {ch.divider && <Divider />}
                </span>
              ))}
            </NestedMenuItem>
            {m.divider && <Divider />}
          </span>
        ))}
        <LinkMenuItem
          display={'Save Filtered Dataset'}
          closeMenu={(): void => {
            if (toggleSaveFilteredRows) toggleSaveFilteredRows();
          }}
          avoidNavigation={true}
        />
        <LinkMenuItem
          display={
            shouldDisplayVariablesGrid ? 'Hide variables' : 'Show variables'
          }
          closeMenu={toggleGrid}
          avoidNavigation={true}
        />
      </MenuRootContent>
    );
  };

  const variablesMenu = ({
    menuProps,
    closeMenu,
  }: RenderMenuProps): JSX.Element => {
    const refreshGrid = async (): Promise<void> => {
      if (
        shouldDisplayVariablesGrid &&
        currentGridRef &&
        currentGridRef.current
      ) {
        await currentGridRef?.current?.saveCalculatedCols();
      }

      closeMenu();
    };

    const openGlobalVariablesCategory = (): void => {
      setOpenGlobalVariablesCategory(true);
      closeMenu();
    };

    const openAddVariableModal = (): void => {
      setOpenAddVariableModal(true);
      closeMenu();
    };

    return (
      <MenuRootContent {...menuProps}>
        <LinkMenuItem
          display="Update variables"
          closeMenu={refreshGrid}
          avoidNavigation={true}
        />
        <LinkMenuItem
          display="Add global variables category..."
          closeMenu={openGlobalVariablesCategory}
          avoidNavigation={true}
          disable={disableVariablesMenu}
        />
        <LinkMenuItem
          display="Add global variables..."
          closeMenu={openAddVariableModal}
          avoidNavigation={true}
          disable={disableVariablesMenu}
        />
        <LinkMenuItem
          display="Add variables from model..."
          closeMenu={closeMenu}
        />
      </MenuRootContent>
    );
  };

  // const getValid = (): boolean => {
  //   if (location.pathname === '/search/new-search' && view === 'model')
  //     return true;
  //   if (location.pathname === '/search/new-search' && view === 'search')
  //     return false;
  //   return true;
  // };

  const handleFreeze = async (id?: number): Promise<void> => {
    if (!id) return;

    try {
      await freezeProject(id);
      window.location.reload();
    } catch (error) {
      console.log(`error`, error);
    }
  };

  const getMenuOptions = (): JSX.Element => {
    if (view) {
      if ('model'.includes(view))
        return (
          <ModelMenu
            isFrozen={modelDetailsProject?.isFrozen}
            handleFreeze={handleFreeze}
            id={modelDetailsProject?.userProjectId}
          />
        );
      if ('search'.includes(view)) return <SearchMenu />;
    }
    return (
      <Fragment>
        <ModelMenu />
        <SearchMenu />
      </Fragment>
    );
  };

  const executeHealth = async (): Promise<void> => {
    const newDatasetId =
      sectionError?.datasetId ??
      healtInfo?.find((info) => info.postProcessHealthData.length)?.datasetId;
    try {
      if (newDatasetId) {
        const job = await executeDatasetPostProcess(newDatasetId, -1);
        if (job?.id) {
          handleJobId?.(parseInt(`${job?.id}`));
        }
      }
    } catch (error) {
      showErrorMessage(
        {
          errorDesc: JSON.stringify(sectionError?.reason),
          onClickReport: () => {
            executeHealth();
          },
        },
        'CS'
      );
    }
  };

  const getHealthColor = (): string => {
    const healtInfoProcessing = healtInfo?.some((h) => h.isProcessing);
    if (!healtInfo?.length || healtInfoProcessing) return 'green';
    return 'red';
  };

  const onHealtClick = (): void => {
    if (healtInfo?.length) {
      showErrorMessage(
        {
          errorDesc: JSON.stringify(sectionError?.reason),
          onClickReport: () => {
            executeHealth();
          },
        },
        'CS'
      );
    }
  };

  const getTitle = (): string => {
    let message = 'No problems with this project';
    if (!healtInfo?.length) return message;
    message =
      healtInfo?.find((info) => info.healthMessage)?.healthMessage ?? '';
    if (!message) {
      healtInfo?.forEach((info) => {
        const postProcessMessage = info.postProcessHealthData.find(
          (pp) => pp.postProcessHealthMessage.length
        )?.postProcessHealthMessage;
        if (postProcessMessage) {
          message = postProcessMessage;
        }
      });
    }
    return message;
  };

  const getMenuBySearch = (): JSX.Element => {
    return (
      <Fragment>
        {getMenuOptions()}
        <Fragment>
          {showMapMenu && <MenuRoot text="Grid">{menu}</MenuRoot>}
          {showMapMenu && <MapMenu />}
          {shouldDisplayVariablesGrid && (
            <MenuRoot text="Variables">{variablesMenu}</MenuRoot>
          )}
          <NotificationTimeLine />
          <MenuRoot
            text="Settings"
            onClick={(): void => history.push('/settings')}
          />
          <MenuRoot text="Help" disabled>
            {menu}
          </MenuRoot>
          {console.log(
            `  history.location.pathname`,
            history.location.pathname
          )}
          {viewMode === 'model' && showHealthIcon && (
            <MenuRoot
              text=""
              icon={
                <Tooltip title={getTitle()}>
                  <LocalHospitalIcon style={{ color: getHealthColor() }} />
                </Tooltip>
              }
              onClick={onHealtClick}
            />
          )}
          <GlobalVariablesModal
            isOpen={openGlobalVariablesCategoty}
            onClose={(): void => setOpenGlobalVariablesCategory(false)}
            onConfirm={(): void => {
              setOpenGlobalVariablesCategory(false);
            }}
          />
          <AddVariablesModal
            isOpen={openAddVariableModal}
            onClose={(): void => setOpenAddVariableModal(false)}
          />
        </Fragment>
      </Fragment>
    );
  };

  return <Fragment>{getMenuBySearch()}</Fragment>;
};

export default memo(AppBarMenu);
