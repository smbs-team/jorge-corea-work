// ColorRampPanel.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { Fragment, MouseEvent } from 'react';
import {
  Button,
  makeStyles,
  Theme,
  createStyles,
  useTheme,
  Box,
} from '@material-ui/core';
import ColorLensIcon from '@material-ui/icons/ColorLens';
import {
  ColorRampPanel as PtasColorRampPanel,
  ColorRampPanelProps,
  CustomPopover,
} from '@ptas/react-ui-library';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    popoverChild: {
      padding: theme.spacing(2),
      overflow: 'hidden',
    },
    rampRoot: {
      boxSizing: 'unset',
    },
    buttonRoot: {
      color: theme.ptas.colors.theme.accent,
    },
    popoverTail: {
      right: 'unset',
      left: 32,
      backgroundColor: 'white',
      boxShadow:
        '0px 3px 3px -2px rgba(0,0,0,0.2),0px 3px 4px 0px rgba(0,0,0,0.14),0px 1px 8px 0px rgba(0,0,0,0.12)',
      clipPath: 'inset(-5px 0px 0px -5px)',
    },
  })
);

const ColorRampPanel = (props: ColorRampPanelProps): JSX.Element => {
  const classes = useStyles(useTheme());
  const [anchorEl, setAnchorEl] = React.useState<HTMLButtonElement | null>(
    null
  );

  const handleClick = (event: MouseEvent<HTMLButtonElement>): void => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = (): void => {
    setAnchorEl(null);
  };

  return (
    <Fragment>
      <Button
        classes={{
          text: classes.buttonRoot,
        }}
        startIcon={<ColorLensIcon />}
        onClick={handleClick}
      >
        Choose color ramp
      </Button>
      <CustomPopover
        anchorEl={anchorEl}
        onClose={handleClose}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'center',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left',
        }}
        classes={{ tail: classes.popoverTail }}
        tail
      >
        <Box className={classes.popoverChild}>
          <PtasColorRampPanel
            classes={{ rampContainer: classes.rampRoot }}
            title="Automatic color ramp"
            {...props}
          />
        </Box>
      </CustomPopover>
    </Fragment>
  );
};

export default ColorRampPanel;
