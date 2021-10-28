/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React from 'react';
import { useEffectOnce } from 'react-use';
import { EappealsProvider } from 'contexts';
import { MapContext, MapProvider } from 'contexts/MapContext';
import { apiService } from 'services/api';
import { layerSourcesPipeFn } from 'utils/eAppeals/layerSourcesPipeFn';
import IconToolBar from 'components/IconToolBar';
import { useIconToolbar } from './useIconToolbar';
import { makeStyles } from '@material-ui/core';

const useStyles = makeStyles((_theme) => ({
  iconToolbarRoot: {
    right: 0,
    left: 12,
    bottom: 16,
  },
}));

//import { usePostMessage } from './usePostMessage';
export default function Appeals(): JSX.Element {
  const classes = useStyles();
  const { selectionOnClick, locationOnClick, rulerOnClick } = useIconToolbar();
  useEffectOnce(() => {
    apiService.layerSources.setPipe(layerSourcesPipeFn);
  });
  // usePostMessage();
  return (
    <MapProvider zoom={15}>
      <MapContext.Consumer>
        {({ map }): JSX.Element | undefined =>
          map ? (
            <EappealsProvider>
              <IconToolBar
                classes={{ root: classes.iconToolbarRoot }}
                selectionOnClick={selectionOnClick}
                locationOnClick={locationOnClick}
                rulerOnClick={rulerOnClick}
              />
            </EappealsProvider>
          ) : undefined
        }
      </MapContext.Consumer>
    </MapProvider>
  );
}
