// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, PropsWithChildren, useContext, useMemo } from 'react';
import { Panel as PtasPanel, PanelProps } from 'components/common/panel';
import { HomeContext } from 'contexts';
import { makeStyles, createStyles, useTheme } from '@material-ui/core/styles';
import { DrawToolBarContext } from 'contexts/DrawToolbarContext';
import DrawToolBar from 'components/DrawToolBar';

const useStyles = makeStyles(() =>
  createStyles({
    root: {
      padding: 0,
      zIndex: 3,
    },
  })
);

const BasePanel: FC<PanelProps> = (props: PropsWithChildren<PanelProps>) => {
  const { setPanelContent, setPanelHeight } = useContext(HomeContext);
  const { actionMode } = useContext(DrawToolBarContext);
  const theme = useTheme();
  const classes = useStyles(theme);
  const subToolBar = ['draw', 'measure'].includes(actionMode ?? '') ? (
    <DrawToolBar />
  ) : undefined;

  return useMemo(
    () => (
      <PtasPanel
        classes={classes}
        {...props}
        resizable
        paperProps={{
          elevation: 0,
          square: true,
        }}
        onClose={(): void => {
          props.onClose?.() || setPanelContent(null);
          setPanelHeight(450);
        }}
        subToolBar={subToolBar}
      >
        {props.children}
      </PtasPanel>
    ),
    [classes, props, setPanelContent, setPanelHeight, subToolBar]
  );
};

export default BasePanel;
