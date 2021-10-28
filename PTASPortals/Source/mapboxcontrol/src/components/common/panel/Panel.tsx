/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  PropsWithChildren,
  forwardRef,
  FC,
  Fragment,
  useContext,
} from 'react';
import {
  withStyles,
  Theme,
  createStyles,
  WithStyles,
  Paper,
  PaperProps,
  StyleRules,
  Box,
} from '@material-ui/core';
import { Close as CloseIcon } from '@material-ui/icons';
import { GenericWithStyles } from '@ptas/react-ui-library';
import TitleRoute from './TitleRoute';
import Resizable, { ResizableProps } from 'libComponents/Resizable';
import { HomeContext } from 'contexts';

/**
 * Component props
 */
interface Props {
  route?: string[];
  toolbarItems?: React.ReactNode;
  onClose?: () => void;
  ref?: React.MutableRefObject<HTMLDivElement | null>;
  paper?: boolean;
  paperProps?: PaperProps;
  resizable?: boolean;
  resizableProps?: ResizableProps;
  disableScrollY?: boolean;
  subToolBar?: JSX.Element;
}

export type PanelProps = GenericWithStyles<
  Props & WithStyles<typeof useStyles>
>;

const useStyles = (theme: Theme): StyleRules<string, Props> =>
  createStyles({
    root: {
      padding: theme.spacing(1.25),
      height: 'calc(100% - 55px)',
    },
    // root: (props) => ({
    //   padding: theme.spacing(1.25),
    // }),
    paper: {
      position: 'relative',
      zIndex: 50,
      backgroundColor: theme.ptas.colors.theme.white,
    },
    header: {
      display: 'flex',
      margin: 0,
      backgroundColor: theme.ptas.colors.theme.grayLight,
    },
    titleTopDivider: {
      marginTop: '2px',
    },
    subToolBarWrapper: {
      position: 'absolute',
      width: '100%',
    },
    closeButton: {
      textTransform: 'none',
      alignItems: 'left',
      color: theme.ptas.colors.theme.black,
      fontSize: 20,
    },
    toolbarItemsContainer: {
      display: 'flex',
      backgroundColor: theme.ptas.colors.theme.grayLight,
      padding: theme.spacing(0, 2),
    },
    resizerToggle: {
      bottom: 10,
    },
    childrenWrap: (props: Props) => {
      return {
        overflowX: 'unset',
        overflowY: props.disableScrollY ? 'unset' : 'auto',
        paddingBottom: theme.spacing(1),
        height: 'inherit',
        display: 'flex',
        flexWrap: 'wrap',
      };
    },
    closeIconContainer: {
      position: 'absolute',
      top: 9,
      right: 19,
      zIndex: 1000,
    },
    closeIcon: {
      marginRight: theme.spacing(-1.25),
      color: theme.ptas.colors.theme.black,
      width: theme.spacing(35 / 8),
      height: theme.spacing(35 / 8),
      fontSize: theme.ptas.typography.h2.fontSize,
      '&:hover': {
        cursor: 'pointer',
      },
    },
  });

/**
 * Panel
 *
 * @param props - Component props
 */
const Panel = forwardRef<
  HTMLDivElement | null,
  Props & WithStyles<typeof useStyles>
>(
  (
    props: PropsWithChildren<Props & WithStyles<typeof useStyles>>,
    ref
  ): JSX.Element => {
    const { classes } = props;
    const wrapInPaper = props.paper === undefined ? true : props.paper;
    const resizable = !!props.resizable;
    const homeContext = useContext(HomeContext);

    const PanelHeader = (): JSX.Element => (
      <Box className={classes.header}>
        <div style={{ flexGrow: 1 }}>
          <TitleRoute route={props.route || []} />
        </div>
        <Box className={classes.closeIconContainer}>
          <CloseIcon
            classes={{ root: classes.closeIcon }}
            onClick={(): void => {
              props.onClose && props.onClose();
            }}
          />
        </Box>
      </Box>
    );

    const resizeWrapper = (el: JSX.Element): JSX.Element =>
      resizable ? (
        <Resizable
          classes={{
            resizerToggle: classes.resizerToggle,
          }}
          onResizeStop={(f): void => {
            homeContext.setPanelHeight(f.height);
          }}
          initialHeight={homeContext.panelHeight}
          {...(props.resizableProps || {})}
        >
          {el}
        </Resizable>
      ) : (
        el
      );

    const paperWrapper = (el: JSX.Element): JSX.Element =>
      wrapInPaper ? (
        <Paper className={props.classes.paper} {...(props.paperProps || {})}>
          {el}
        </Paper>
      ) : (
        el
      );

    return paperWrapper(
      <Fragment>
        {resizeWrapper(
          <div className={props.classes.root} ref={ref}>
            <PanelHeader />
            <Box className={classes.toolbarItemsContainer}>
              {props.toolbarItems}
            </Box>
            <Box className={props.classes.childrenWrap}>{props.children}</Box>
          </div>
        )}
        <Box className={props.classes.subToolBarWrapper}>
          {props.subToolBar}
        </Box>
      </Fragment>
    );
  }
);

export default withStyles(useStyles)(Panel) as FC<PanelProps>;
