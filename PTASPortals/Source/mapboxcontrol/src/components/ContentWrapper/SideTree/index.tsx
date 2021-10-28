/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  Fragment,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useRef,
  useState,
} from 'react';
import { makeStyles, Menu, MenuItem, PopoverPosition } from '@material-ui/core';
import { useAsync, useDebounce, useWindowSize } from 'react-use';
import {
  ResizableSideTree,
  YesNoBox,
  ErrorMessageAlertCtx,
} from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import { utilService } from 'services/common';
import { layerService } from 'services/map';
import { downloadUrl } from 'utils/downloadUrl';
import { isRootLayerRow, useSideTree } from './useSideTree';
import { apiService, GetLayerDownloadUrl } from 'services/api';
import { Popover } from 'components/common';
import { useGlMap } from 'hooks/map/useGlMap';
import { getErrorStr } from 'utils/getErrorStr';

const useStyles = makeStyles(() => ({
  root: {
    height: '93.8%',
  },
  popoverRoot: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
  },
}));

export default function SideTree(): JSX.Element {
  const classes = useStyles();
  const { sideRows, handleSideTreeSelection } = useSideTree();
  const [anchorPos, setAnchorPos] = useState<PopoverPosition>();
  const [showPopOver, setShowPopover] = useState(false);
  const { setLinearProgress } = useContext(HomeContext);
  const [layerId, setLayerId] = useState<string>();
  const [downloadInfo, setDownloadInfo] = useState<GetLayerDownloadUrl>();
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const popoverRef = useRef<HTMLDivElement>();
  const map = useGlMap();
  const sideTreeW = useRef(0);
  const windowSize = useWindowSize();

  const onResizableWidthChange = useCallback(
    (w: number): void => {
      if (!map) return;
      sideTreeW.current = w;
      const container = map.getContainer();
      const containerW = window.innerWidth - w;
      container.style.width = containerW + 'px';
      container.style.left = w + 'px';
      map.resize();
    },
    [map]
  );

  useDebounce(
    () => {
      onResizableWidthChange(sideTreeW.current);
    },
    150,
    [windowSize]
  );

  useAsync(async () => {
    if (!layerId) {
      return;
    }
    if (!showPopOver) {
      return;
    }
    try {
      const layerSource = layerService.layersConfiguration[layerId];
      setLinearProgress(true);
      const res = await apiService.getLayerDownloadUrl(
        layerSource.layerSourceId
      );
      setDownloadInfo(res.data);
    } catch (e) {
      showErrorMessage(getErrorStr(e));
    } finally {
      setLinearProgress(false);
    }
  }, [layerId, showPopOver]);

  useEffect(() => {
    if (!showPopOver) {
      setLayerId(undefined);
      setDownloadInfo(undefined);
    }
  }, [showPopOver]);

  return useMemo(
    () => (
      <Fragment>
        <ResizableSideTree
          onWidthChange={onResizableWidthChange}
          rows={sideRows}
          noDataMessage="No map selected"
          onChecked={handleSideTreeSelection}
          disableSelection
          SideTreeProps={{
            rows: sideRows,
            classes: { rootComponent: classes.root },
            onContextMenu: (e, row): void => {
              if (!isRootLayerRow(row)) return;
              setLayerId(row.id.toString().replace(/\d-/, ''));
              setAnchorPos({
                top: e.clientY,
                left: e.clientX,
              });
            },
          }}
        />
        {showPopOver && layerId && downloadInfo && (
          <Popover
            ref={popoverRef}
            open={true}
            anchorReference="none"
            onClose={(): void => {
              setShowPopover(false);
            }}
            anchorOrigin={{
              vertical: 'top',
              horizontal: 'left',
            }}
            transformOrigin={{
              vertical: 'top',
              horizontal: 'left',
            }}
            classes={{ root: classes.popoverRoot }}
          >
            <YesNoBox
              cancelClick={(): void => {
                console.log('Cancel click');
              }}
              clickNo={(): void => {
                setShowPopover(false);
              }}
              clickYes={async (): Promise<void> => {
                try {
                  setLinearProgress(true);
                  setShowPopover(false);
                  downloadUrl(downloadInfo.url);
                } finally {
                  setLinearProgress(false);
                }
              }}
              title={`${
                layerService.layersConfiguration[layerId].layerSourceName
              } size is ${utilService.bytesToMb(downloadInfo.fileSize)} mb.
          Are you sure you want to download ${
            layerService.layersConfiguration[layerId].layerSourceName
          }`}
            />
          </Popover>
        )}

        <Menu
          anchorReference="anchorPosition"
          anchorPosition={anchorPos}
          keepMounted
          open={!!anchorPos}
          onClose={(): void => {
            setAnchorPos(undefined);
          }}
          MenuListProps={{ disablePadding: true }}
          TransitionProps={{
            onExiting: (): void => {
              setAnchorPos(undefined);
            },
          }}
        >
          <MenuItem
            key={0}
            onClick={(): void => {
              setAnchorPos(undefined);
              setShowPopover(true);
            }}
          >
            Download layer
          </MenuItem>
        </Menu>
      </Fragment>
    ),
    [
      anchorPos,
      classes.popoverRoot,
      classes.root,
      downloadInfo,
      handleSideTreeSelection,
      layerId,
      onResizableWidthChange,
      setLinearProgress,
      showPopOver,
      sideRows,
    ]
  );
}
