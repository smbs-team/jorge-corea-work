// Home.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles } from '@material-ui/core';
import React, { Fragment, useState } from 'react';
import AreaTree from './AreaTree';
import { useEffectOnce } from 'react-use';
import PageDetails from './PageDetails';
import Ribbon from './Ribbon';
import useAreaTreeStore from 'stores/useAreaTreeStore';
import useSubToolbarStore from 'stores/useSubToolbarStore';
import useGetNbhd from 'hooks/useGetArea';
import useRibbonStore from 'stores/useRibbonStore';
import { Resizable } from 're-resizable';
import { CustomIconButton } from '@ptas/react-ui-library';
import FastRewindIcon from '@material-ui/icons/FastRewind';
import FastForwardIcon from '@material-ui/icons/FastForward';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

const useStyles = makeStyles({
  contentContainer: {
    display: 'flex',
    marginTop: 8,
    height: 'calc(100vh - 168.25px)',
    position: 'relative',
  },
  childrenContainer: {
    overflow: 'auto',
    padding: '16px 16px 8px 16px',
    width: '100%',
  },
  innerHeader: {
    display: 'flex',
    backgroundColor: '#fafafa',
    zIndex: 2,
    minWidth: 1198,
    paddingBottom: 8,
    paddingLeft: 16,
    boxShadow: '9px 10px 6px -6px #00000008',
    width: '100%',
  },
  childrenContentContainer: {
    width: '100%',
    display: 'flex',
    flexFlow: 'column wrap',
    gap: 32,
  },
  optionsHeader: {
    height: 18,
    display: 'flex',
    overflow: 'hidden',
    marginBottom: 4,
    alignContent: 'center',
  },
});

const MIN_WIDTH = 30;
const DEFAULT_WIDTH = 200;

function Home(): JSX.Element {
  const classes = useStyles();
  const areaTreeStore = useAreaTreeStore();
  const subToolBarStore = useSubToolbarStore();
  const { NbhdComponent } = useGetNbhd();
  const ribbonStore = useRibbonStore();
  const [treeWidth, setTreeWidth] = useState<number>(DEFAULT_WIDTH);
  const [transition, setTransition] = useState<string>();
  
  useEffectOnce(() => {
    subToolBarStore.setTitle(`Income Tables`);
  });

  return (
    <Fragment>
      {areaTreeStore.selectedItem && <PageDetails />}
      <div
        className={classes.contentContainer}
        style={{
          height: !areaTreeStore.selectedItem
            ? 'calc(100vh - 136px)'
            : undefined,
        }}
      >
        <Resizable
          enable={{
            top: false,
            topLeft: false,
            topRight: false,
            bottom: false,
            bottomLeft: false,
            bottomRight: false,
            left: false,
            right: true,
          }}
          size={{ width: treeWidth, height: '100%' }}
          onResizeStop={(_e, _direction, _ref, d) => {
            setTreeWidth(treeWidth + d.width);
          }}
          style={{
            borderRight: '1px solid #c0c0c0',
            transition: transition,
          }}
          minWidth={MIN_WIDTH}
          onResizeStart={() => setTransition(undefined)}
        >
          <div className={classes.optionsHeader}>
            {treeWidth > MIN_WIDTH && (
              <CustomIconButton
                onClick={() => areaTreeStore.setExpandedRowIds([])}
                icon={
                  <ExpandMoreIcon
                    style={{ transform: 'rotate(180deg)', color: 'black' }}
                    titleAccess="Collapse all"
                  />
                }
              />
            )}
            <CustomIconButton
              onClickCapture={() => setTransition('0.2s ease-in-out')}
              onClick={(): void => {
                treeWidth === MIN_WIDTH
                  ? setTreeWidth(DEFAULT_WIDTH)
                  : setTreeWidth(MIN_WIDTH);
              }}
              icon={
                treeWidth === MIN_WIDTH ? (
                  <FastForwardIcon titleAccess="Open panel" />
                ) : (
                  <FastRewindIcon titleAccess="Collapse panel" />
                )
              }
              style={{ marginLeft: 'auto', marginRight: 8, color: 'black' }}
              disableRipple
            />
          </div>
          <div
            style={{
              display: treeWidth === MIN_WIDTH ? 'none' : undefined,
              height: 'calc(100% - 22px)',
            }}
          >
            <AreaTree />
          </div>
        </Resizable>
        {areaTreeStore.selectedItem && (
          <div
            style={{
              display: 'flex',
              flexDirection: 'column',
              overflow: 'auto',
              width: '100%',
            }}
          >
            {!ribbonStore.hideAll && (
              <div className={classes.innerHeader}>
                <Ribbon />
              </div>
            )}
            <div className={classes.childrenContainer}>{NbhdComponent}</div>
          </div>
        )}
        {!areaTreeStore.selectedItem && (
          <div className={classes.childrenContainer}>
            <h2 style={{ margin: 0 }}>Select a neighborhood</h2>
          </div>
        )}
      </div>
    </Fragment>
  );
}

export default Home;
