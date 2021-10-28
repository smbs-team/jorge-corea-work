/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext } from 'react';
import { useTheme, makeStyles, Box } from '@material-ui/core';
import {
  Close as CloseIcon,
  RadioButtonUncheckedOutlined as CircleIcon,
  GestureOutlined as LassoIcon,
  Cancel as DeleteIcon,
  FileCopyOutlined as CopyIcon,
} from '@material-ui/icons';
import SelectToolBarOption from './SelectToolBarOption';
import { mapBoxDrawService } from 'services/map/mapboxDraw';
import { utilService } from 'services/common';
import { useMeasureAreaText, useHasMeasureFeatures } from './useDrawToolbar';
import { SubToolBar } from '@ptas/react-ui-library';
import selectionService from 'services/parcel/selection';
import { useMount, useObservable, useUnmount } from 'react-use';
import { DrawToolBarContext } from 'contexts/DrawToolbarContext';
import { ParcelInfoContext } from 'contexts/ParcelInfoContext';
import { useGlobalStyles } from 'hooks';
import { withMap } from 'hoc/withMap';

const useStyles = makeStyles(() => ({
  root: {
    justifyContent: 'space-between',
    background: 'rgba(68,68,68,0.8)',
    height: 44,
  },
  closeIcon: {
    cursor: 'pointer',
  },
}));

/**
 * A SubToolBar with some default props
 * @param props -SubToolBar props
 */

const DrawToolBar = withMap(function ({ map }): JSX.Element {
  const classes = { ...useStyles(useTheme()), ...useGlobalStyles() };
  const { actionMode, setActionMode } = useContext(DrawToolBarContext);
  const { setSelectedParcelInfo } = useContext(ParcelInfoContext);
  const areaText = useMeasureAreaText();
  const hasMeasureFeatures = useHasMeasureFeatures();
  const drawMode = useObservable(selectionService.$onChangeSelection);

  useMount(() => {
    if (actionMode === 'draw') {
      mapBoxDrawService.init(map);
      mapBoxDrawService.measure?.stop();
      selectionService.changeSelection('click');
    }
  });

  const clearSelection = (): void => {
    if (!setSelectedParcelInfo) return;
    selectionService.currentPagePin = undefined;
    selectionService.clearSelection();
    setSelectedParcelInfo(undefined);
    mapBoxDrawService.deleteAll();
  };

  useUnmount(() => {
    selectionService.changeSelection('disabled');
    mapBoxDrawService.measure?.stop();
    mapBoxDrawService.deleteAll();
  });

  return (
    <SubToolBar classes={{ root: classes.root }}>
      <Box className={classes.flexboxRow}>
        {actionMode === 'draw' && (
          <SelectToolBarOption
            title="Click"
            onClick={(): void => {
              drawMode !== 'click' && selectionService.changeSelection('click');
            }}
            isActive={drawMode === 'click'}
          />
        )}
        {actionMode === 'draw' && (
          <SelectToolBarOption
            title="Polygon"
            onClick={(): void => {
              drawMode !== 'polygon' &&
                selectionService.changeSelection('polygon');
            }}
            isActive={drawMode === 'polygon'}
          />
        )}
        {actionMode !== 'measure' && (
          <SelectToolBarOption
            title="Circle"
            onClick={(): void => {
              drawMode !== 'circle' &&
                selectionService.changeSelection('circle');
            }}
            isActive={drawMode === 'circle'}
          >
            <CircleIcon />
          </SelectToolBarOption>
        )}
        {actionMode === 'measure' && (
          <SelectToolBarOption
            title="Lasso"
            onClick={(): void => {
              // selectionService.changeSelection('line');
            }}
            isActive={!!mapBoxDrawService.measure?.enabled}
          >
            <LassoIcon />
          </SelectToolBarOption>
        )}

        {actionMode === 'measure' && areaText && (
          <SelectToolBarOption
            title="Copy value"
            onClick={(): void => {
              utilService.copyStringToClipboard(areaText ?? '0');
            }}
            isActive={false}
          >
            <CopyIcon />
          </SelectToolBarOption>
        )}
        {actionMode === 'measure' && hasMeasureFeatures ? (
          <SelectToolBarOption
            title={'Clear points'}
            onClick={(): void => {
              mapBoxDrawService.deleteAll();
            }}
            isActive={false}
          >
            <DeleteIcon />
          </SelectToolBarOption>
        ) : (
          actionMode === 'draw' && (
            <SelectToolBarOption
              title={'Clear selection'}
              onClick={clearSelection}
              isActive={false}
            >
              <DeleteIcon />
            </SelectToolBarOption>
          )
        )}
      </Box>
      <Box>
        <CloseIcon
          classes={{ root: classes.closeIcon }}
          onClick={(): void => {
            setActionMode(null);
          }}
        />
      </Box>
    </SubToolBar>
  );
});

export default DrawToolBar;
