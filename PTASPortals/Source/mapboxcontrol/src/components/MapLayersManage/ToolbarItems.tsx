/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import {
  InsertDriveFile as SaveIcon,
  Refresh as RefreshIcon,
} from '@material-ui/icons';
import { makeStyles } from '@material-ui/core';
import { PanelHeader, ErrorMessageAlertCtx } from '@ptas/react-ui-library';
import MapsTreeView from 'components/MapsTreeView';
import RenderersTreeView from 'components/RenderersTreeView';
import { HomeContext } from 'contexts';
import React, { useContext } from 'react';
import { mapService, UserMap } from 'services/map';
import { MapsLayersManageProps } from './types';
import { MapLayersManageContext } from './Context';
import { textRenderersService } from 'services/map/renderer/textRenderer/textRenderersService';

const useStyles = makeStyles(() => ({
  routeButton: {
    cursor: 'pointer',
    textDecoration: 'underline',
  },
}));

export default function ToolbarItems(
  props: MapsLayersManageProps
): JSX.Element | null {
  const classes = useStyles();
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const {
    editingUserMap,
    setPanelContent,
    setSelectedSystemUserMap,
    setSelectedUserMap,
  } = useContext(HomeContext);
  const { setSavePopoverAnchor } = useContext(MapLayersManageContext);

  const openMaps = (): void => {
    if (editingUserMap && !editingUserMap?.userMapId) {
      mapService.updateSelectedMap(undefined);
    }
    setPanelContent(<MapsTreeView />);
  };
  return (
    (editingUserMap && (
      <PanelHeader
        route={[
          <label
            key={1}
            onClick={(): void => {
              if (props.isSystemUserMap) {
                setPanelContent(<RenderersTreeView />);
              } else {
                openMaps();
              }
            }}
            className={classes.routeButton}
          >
            {props.isSystemUserMap ? 'Renderers' : 'Maps'}
          </label>,
          <label key={2}>{editingUserMap.userMapName}</label>,
        ]}
        icons={[
          {
            icon: <RefreshIcon />,
            text: 'Refresh preview',
            onClick: (): void => {
              if (!editingUserMap) return;
              const mapLabels = mapService.getMapLabels(editingUserMap);
              if (!textRenderersService.isValidLabelList(mapLabels)) {
                showErrorMessage({
                  detail: 'invalid text renderer labels list',
                });
                return;
              }
              if (props.isSystemUserMap) {
                setSelectedSystemUserMap(new UserMap(editingUserMap));
              } else {
                setSelectedUserMap(new UserMap(editingUserMap));
              }
            },
          },
          {
            text: props.isSystemUserMap ? 'Save renderer' : 'Save map',
            icon: <SaveIcon />,
            onClick: (e): void => {
              setSavePopoverAnchor(e.currentTarget);
            },
          },
        ]}
      />
    )) ??
    null
  );
}
