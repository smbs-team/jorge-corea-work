/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  PropsWithChildren,
  useContext,
  useState,
  useEffect,
  useRef,
  Fragment,
} from 'react';
import {
  Box,
  Divider,
  Theme,
  makeStyles,
  createStyles,
  useTheme,
  CircularProgress,
} from '@material-ui/core';
import { useMount, useUnmount } from 'react-use';
import clsx from 'clsx';
import { AddCircleOutline } from '@material-ui/icons';
import { IconToolBar, ErrorMessageAlertCtx } from '@ptas/react-ui-library';
import { PanelTitle, PanelBody, PanelSection } from 'components/common/panel';
import Panel from 'components/BasePanel';
import RootLayerStyle from './RootLayerStyle';
import AppliedLayers from './AppliedLayers';
import { withLayerManage, MapLayersManageContext } from './Context';
import { HomeContext } from 'contexts';
import { UserMap, mapService } from 'services/map';
import userService from 'services/user/userService';
import ColorRenderer from './ColorRenderer';
import { ModalGrid } from 'components/common';
import LayerInfo from './LayerInfo';
import LayerRenderers from './LayerRenderers';
import TextRenderer from 'components/MapLayersManage/TextRenderer';
import { apiService, datasetService } from 'services/api';
import { systemUserMapService } from 'services/map/systemUserMapService';
import { SYSTEM_RENDERER_FOLDER } from 'appConstants';
import { useOnSelectedLayers } from 'hooks/map/useOnSelectedLayers';
import { MapsLayersManageProps } from './types';
import ToolbarItems from './ToolbarItems';
import { MapsTreeViewRow } from 'components/MapsTreeView/types';
import SaveEditingUserMap from './SaveEditingUserMap';
import { getErrorStr } from 'utils/getErrorStr';
import { useSavedMap } from './useSavedMap';
import { isInSystem } from 'utils/userMap';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    panelChild: {
      width: '100%',
    },
    loading: {
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
    },
    button: {
      color: theme.ptas.colors.theme.accent,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.buttonSmall.fontSize,
      fontWeight: theme.ptas.typography.buttonSmall.fontWeight,
    },
    titleButton: { marginLeft: theme.spacing(2) },
    section: {
      margin: theme.spacing(0, 4),
    },
    separator: {
      borderRight: '1px solid ' + theme.ptas.colors.theme.black,
    },
    popover: {
      borderRadius: 9,
      padding: theme.spacing(1),
    },
    routeButton: {
      cursor: 'pointer',
      textDecoration: 'underline',
    },
    layerInfoSection: {
      width: 800,
      [theme.breakpoints.down('sm')]: {
        width: 500,
      },
    },
    layerContentSection: {
      minWidth: '180px',
    },
    horizontalSeparator: {
      margin: theme.spacing(2, 4, 0, 4),
    },
    layersPanelRowRoot: {
      marginBottom: 0,
      display: 'inline-flex',
    },
    flexBox: {
      display: 'flex',
    },
    layersListSection: {
      minWidth: 208,
    },
  })
);

function MapsLayersManage(
  props: PropsWithChildren<MapsLayersManageProps>
): JSX.Element {
  const theme = useTheme();
  const classes = useStyles(theme);
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const homeContext = useContext(HomeContext);
  const {
    bottomSection,
    labelRendererSectionRef,
    setIsSystemRenderer,
  } = useContext(MapLayersManageContext);
  const {
    selectedUserMap,
    selectedSystemUserMap,
    currentLayer,
    editingUserMap,
    setEditingUserMap,
    setIsEditingUserMap,
    setSelectedUserMap,
    setSelectedSystemUserMap,
    setSelectedCategories,
  } = homeContext;
  const onSelectedLayers = useOnSelectedLayers();
  const editingUserMapRef = useRef<UserMap | undefined>();
  const savedMapRef = useSavedMap(!!props.isSystemUserMap);
  //open or closes de layers modal tree
  const [open, setOpen] = useState<boolean>(false);
  const showRenderersOptions =
    currentLayer?.rendererRules.layer.type === 'fill';

  const hasColorRenderer = (editingUserMap?.mapRenderers ?? []).some((mr) => {
    return (
      mr.rendererRules.layer.id === currentLayer?.rendererRules.layer.id &&
      !!mr.rendererRules.colorRule
    );
  });
  const hasTextRenderer = (editingUserMap?.mapRenderers ?? []).some((mr) => {
    return (
      mr.rendererRules.layer.id === currentLayer?.rendererRules.layer.id &&
      !!mr.rendererRules.labels
    );
  });
  const [loading, setLoading] = useState(false);

  const loadNewMap = async (): Promise<void> => {
    const newMap = new UserMap({
      createdBy: userService.userInfo.id,
      createdTimestamp: '' + Date.now(),
      folderPath: props.isSystemUserMap ? SYSTEM_RENDERER_FOLDER.PATH : '',
      folderItemType: 'UserMap',
      isLocked: false,
      lastModifiedBy: userService.userInfo.id,
      lastModifiedTimestamp: '',
      parentFolderId: 0,
      userMapId: 0,
      userMapName: props.isSystemUserMap ? 'New Renderer' : 'New Map',
      mapRenderers: [],
    });

    setSelectedCategories([]);
    setEditingUserMap(new UserMap(newMap));
    if (props.isSystemUserMap) {
      setSelectedSystemUserMap(new UserMap(newMap));
    } else {
      setSelectedUserMap(new UserMap(newMap));
    }
  };

  const loadExistingMap = async (row: MapsTreeViewRow): Promise<void> => {
    try {
      setLoading(true);
      if (props.isSystemUserMap) {
        await systemUserMapService.applySystemUserMap(row.id as number, true);
      } else {
        await mapService.updateSelectedMap(row.id as number, true);
      }
      datasetService.getColorRendererData({ force: true });
    } catch (e) {
      showErrorMessage('Error loading user map');
    } finally {
      setLoading(false);
    }
  };

  const loadToDuplicateMap = async (id: number): Promise<void> => {
    if (props.isSystemUserMap) return;
    try {
      setLoading(true);
      const data = await apiService.getUserMap(id);
      if (data) {
        data.userMapName = 'Copy of ' + data.userMapName;
        data.userMapId = 0;
        setEditingUserMap(data);
        mapService.selectedUserMap = data;
        setSelectedUserMap(data);
      }
      datasetService.getColorRendererData({ force: true });
    } catch (e) {
      showErrorMessage('Error loading user map');
    } finally {
      setLoading(false);
    }
  };

  useMount(async () => {
    try {
      setEditingUserMap(undefined);
      homeContext.setPrevSelectedUserMap(homeContext.selectedUserMap);
      homeContext.setPrevSelectedSystemUserMap(
        homeContext.selectedSystemUserMap
      );
      setIsEditingUserMap(true);
      setIsSystemRenderer(!!props.isSystemUserMap);
      if (props.row?.id) {
        if (props.duplicate) {
          await loadToDuplicateMap(+props.row.id);
        } else {
          if (props.row.id === selectedUserMap?.userMapId) {
            return setEditingUserMap(selectedUserMap);
          }
          if (props.row.id === selectedSystemUserMap?.userMapId) {
            return setEditingUserMap(selectedSystemUserMap);
          }
          await loadExistingMap(props.row);
        }
      } else {
        loadNewMap();
      }
    } catch (e) {
      showErrorMessage(getErrorStr(e));
    }
  });

  useEffect(() => {
    const sub1 = mapService.$onEditingUserMapChange.subscribe(
      setEditingUserMap
    );
    const sub2 = systemUserMapService.$onEditingSystemUserMapChange.subscribe(
      setEditingUserMap
    );
    return (): void => {
      sub1.unsubscribe();
      sub2.unsubscribe();
    };
  }, [setEditingUserMap]);

  useEffect(() => {
    editingUserMapRef.current = editingUserMap;
  }, [editingUserMap]);

  useUnmount(() => {
    if (!savedMapRef.current || !editingUserMapRef.current) return;
    const savedMapStr = JSON.stringify(savedMapRef.current.mapRenderers);
    const editingMapStr = JSON.stringify(
      editingUserMapRef.current?.mapRenderers
    );

    if (
      savedMapStr !== editingMapStr ||
      savedMapRef.current.folderPath === ''
    ) {
      homeContext.setSavePendingChanges(true);
    }

    //Don't set editingUserMap to undefined, because its value is needed if the user
    //decides to save pending changes. Instead, use the state isEditingUserMap
    setIsEditingUserMap(false);
  });

  return (
    <Fragment>
      <Panel toolbarItems={<ToolbarItems {...props} />} {...props}>
        <Box className={clsx(classes.panelChild, loading && classes.loading)}>
          {loading ? (
            <CircularProgress color="inherit" />
          ) : (
            <Fragment>
              <Box className={classes.layersPanelRowRoot}>
                <PanelSection
                  classes={{
                    root: clsx(classes.separator, classes.layersListSection),
                  }}
                >
                  <PanelTitle topLine={false} bottomLine={false}>
                    Layers
                    <IconToolBar
                      icons={[
                        {
                          text: 'Add layer',
                          icon: <AddCircleOutline />,
                          onClick: (): void => {
                            setOpen(true);
                          },
                        },
                      ]}
                      classes={{ root: classes.titleButton }}
                    />
                  </PanelTitle>
                  <AppliedLayers shrink={!showRenderersOptions} />
                </PanelSection>

                {currentLayer?.rendererRules.layer && (
                  <PanelSection
                    classes={{
                      root: clsx(classes.layerInfoSection, classes.separator),
                    }}
                  >
                    <PanelTitle topLine={false} bottomLine={false}>
                      Layer Info
                    </PanelTitle>
                    <PanelBody key="last">
                      <LayerInfo />
                    </PanelBody>
                    {currentLayer?.rendererRules.layer.id &&
                      ['line', 'fill', 'circle'].includes(
                        currentLayer?.rendererRules.layer.type
                      ) && (
                        <Box className={classes.flexBox}>
                          <PanelBody>
                            <LayerRenderers />
                          </PanelBody>
                        </Box>
                      )}
                  </PanelSection>
                )}

                {currentLayer?.rendererRules.layer.id && (
                  <PanelSection classes={{ root: classes.separator }}>
                    <PanelTitle topLine={false} bottomLine={false}>
                      Layer Style
                    </PanelTitle>
                    <RootLayerStyle
                      rendererRules={currentLayer.rendererRules}
                    />
                  </PanelSection>
                )}
              </Box>

              <Divider className={classes.horizontalSeparator} />

              {bottomSection === 'colorRenderer' && hasColorRenderer && (
                <div className={classes.flexBox}>
                  <PanelSection>
                    <PanelTitle topLine={false} bottomLine={false}>
                      Color Renderer definition
                    </PanelTitle>
                    <PanelBody key="last">
                      <ColorRenderer />
                    </PanelBody>
                  </PanelSection>
                </div>
              )}

              {bottomSection === 'labelRenderer' && hasTextRenderer && (
                <div className={classes.flexBox} ref={labelRendererSectionRef}>
                  <PanelSection>
                    <PanelBody>
                      <TextRenderer />
                    </PanelBody>
                  </PanelSection>
                </div>
              )}
            </Fragment>
          )}
        </Box>
      </Panel>
      <ModalGrid
        isOpen={open}
        onClose={(): void => setOpen(false)}
        onButtonClick={(_selectedRows): void => {
          onSelectedLayers(_selectedRows, isInSystem(editingUserMap));
        }}
      />
      {editingUserMap && <SaveEditingUserMap savedMapRef={savedMapRef} />}
    </Fragment>
  );
}

export default withLayerManage(MapsLayersManage);
