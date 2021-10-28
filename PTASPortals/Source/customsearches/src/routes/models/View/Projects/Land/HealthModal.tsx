// FILENAME
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import CloseIcon from '@material-ui/icons/Close';
import {
  Backdrop,
  Fade,
  Modal,
  createStyles,
  Theme,
  WithStyles,
  withStyles,
  StyleRules,
} from '@material-ui/core';
import { CustomButton, CustomIconButton } from '@ptas/react-ui-library';
import React, { Fragment, useEffect } from 'react';
import Loader from 'react-loader-spinner';

interface Props extends WithStyles<typeof useStyles> {
  isOpen?: boolean;
  message: string;
  onClose?: () => void;
  execute: () => void;
  loading: boolean;
  issue: string;
}

const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      borderRadius: 12,
      boxShadow: theme.shadows[5],
      width: 750,
      height: 414,
      backgroundColor: 'white',
      position: 'absolute',
      padding: theme.spacing(2.5, 5, 2.5, 5),
    },
    iconButton: {
      position: 'absolute',
      top: 13,
      right: 34,
      color: 'black',
    },
    closeIcon: {
      fontSize: 42,
    },
    label: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: '1.375rem',
      marginBottom: 14,
      display: 'block',
    },
    button: {
      display: 'flex',
      marginLeft: 'auto',
      marginTop: 60,
    },
    bold: {
      fontWeight: 700,
    },
    queryclass: {
      display: 'block',
      fontWeight: 700,
    },
    closeButton: {
      position: 'absolute',
      top: 15,
      right: 15,
    },
  });

const HealthModal = (props: Props): JSX.Element => {
  const { classes } = props;
  const [open, setOpen] = React.useState(false);

  useEffect(() => {
    if (props.isOpen === undefined) return;
    setOpen(props.isOpen);
  }, [props.isOpen]);

  const handleClose = (): void => {
    setOpen(false);
    props.onClose && props.onClose();
  };

  return (
    <Modal
      open={open}
      onClose={handleClose}
      closeAfterTransition
      BackdropComponent={Backdrop}
      BackdropProps={{
        timeout: 500,
      }}
      style={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
      }}
    >
      <Fade in={open}>
        <Fragment>
          <div className={classes.root} style={{ height: 'auto' }}>
            {props.loading ? (
              <Loader type="Oval" color="#00BFFF" height={80} width={80} />
            ) : (
              <label className={classes.label}>{props.message}</label>
            )}
            {props.issue !== 'FailedGeneration' && (
              <CustomButton
                classes={{ root: classes.button }}
                onClick={props.execute}
                fullyRounded
              >
                re-execute
              </CustomButton>
            )}
            <CustomIconButton
              icon={<CloseIcon className={classes.closeIcon} />}
              className={classes.iconButton}
              classes={{ root: classes.closeButton }}
              onClick={handleClose}
            />
          </div>
        </Fragment>
      </Fade>
    </Modal>
  );
};
export default withStyles(useStyles)(HealthModal);
