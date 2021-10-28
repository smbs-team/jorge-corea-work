// IconToolBar.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren, useContext } from 'react';
import {
  withStyles,
  WithStyles,
  createStyles,
  Box,
  StyleRules,
} from '@material-ui/core';
import Icon from '@mdi/react';
import NearMeIcon from '@material-ui/icons/NearMe';
import StyledIconButton from './StyledIconButton';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import { mdiSelect } from '@mdi/js';
import { mdiRuler } from '@mdi/js';
import { withMap } from 'hoc/withMap';
import { DrawToolBarContext } from 'contexts/DrawToolbarContext';

type Props = WithStyles<typeof useStyles> & {
  infoOnClick?: () => void;
  locationOnClick: () => void;
  rulerOnClick: () => void;
  selectionOnClick: () => void;
};

const useStyles = (): StyleRules =>
  createStyles({
    root: {
      display: 'flex',
      flexDirection: 'column',
      width: 'fit-content',
      zIndex: 2,
      position: 'absolute',
      bottom: 8,
      right: -47,
    },
    icon: {
      fontSize: 32,
    },
    measureButton: {
      marginTop: 16,
      marginBottom: 16,
    },
    infoButton: {
      marginBottom: 16,
    },
  });

/**
 * IconToolBar
 *
 * @param props - Component props
 * @returns JSX element
 */
function IconToolBar(props: PropsWithChildren<Props>): JSX.Element {
  const {
    classes,
    locationOnClick,
    rulerOnClick,
    selectionOnClick,
    infoOnClick,
  } = props;
  const { actionMode } = useContext(DrawToolBarContext);

  return (
    <Box className={classes.root}>
      {infoOnClick && (
        <StyledIconButton
          classes={{ root: classes.infoButton }}
          onClick={infoOnClick}
          isSelected={actionMode === 'info-mode'}
        >
          <InfoOutlinedIcon className={classes.icon} />
        </StyledIconButton>
      )}
      <StyledIconButton onClick={locationOnClick}>
        <NearMeIcon className={classes.icon} />
      </StyledIconButton>
      <StyledIconButton
        classes={{ root: classes.measureButton }}
        onClick={rulerOnClick}
        isSelected={actionMode === 'measure'}
      >
        <Icon path={mdiRuler} size={2} />
      </StyledIconButton>
      <StyledIconButton
        onClick={selectionOnClick}
        isSelected={actionMode === 'draw'}
      >
        <Icon path={mdiSelect} size={2} />
      </StyledIconButton>
    </Box>
  );
}

export default withStyles(useStyles)(withMap(IconToolBar));
