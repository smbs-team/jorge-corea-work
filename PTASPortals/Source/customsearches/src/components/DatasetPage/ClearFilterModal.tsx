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
  withStyles,
  WithStyles,
  StyleRules,
} from '@material-ui/core';
import { CustomButton, CustomIconButton } from '@ptas/react-ui-library';
import React, { Fragment } from 'react';

interface Props extends WithStyles<typeof useStyles> {
  isOpen: boolean;
  onSave?: (remember?: boolean) => void;
  onClose?: () => void;
}

const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      borderRadius: 12,
      boxShadow: theme.shadows[5],
      width: 617,
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
    messageLabel: {
      fontSize: '18px',
      paddingBottom: '1em',
      display: 'block',
    },
    userInput: {
      maxWidth: '230px',
    },
    dropdown: {
      width: '100%',
      marginBottom: '1em',
      maxWidth: '230px',
    },
    content: {},
    numeric: {
      marginBottom: 16,
    },
    columnTitle: {
      fontFamily: theme.ptas.typography.titleFontFamily,
      fontSize: '1.25rem',
      marginBottom: 24,
      fontWeight: 'bolder',
    },
    buttonWrapper: {
      display: 'flex',
      justifyContent: 'flex-end',
    },
    button: {
      display: 'flex',
      marginLeft: 15,
      marginTop: 60,
    },
  });

const ClearFilterModal = (props: Props): JSX.Element => {
  const { classes } = props;

  return (
    <Modal
      open={props.isOpen}
      onClose={props.onClose}
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
      <Fade in={props.isOpen}>
        <Fragment>
          <div className={classes.root} style={{ height: 'auto' }}>
            <label className={classes.label}>Clear filter</label>
            <div className={classes.content}>
              Remember the selection when clean the filter?
            </div>
            <div className={classes.buttonWrapper}>
              <CustomButton
                classes={{ root: classes.button }}
                onClick={(): void => {
                  props.onSave?.(true);
                }}
                fullyRounded
              >
                Yes
              </CustomButton>
              <CustomButton
                classes={{ root: classes.button }}
                onClick={(): void => {
                  props.onSave?.();
                }}
                fullyRounded
              >
                No
              </CustomButton>
            </div>
            <CustomIconButton
              icon={<CloseIcon className={classes.closeIcon} />}
              className={classes.iconButton}
              onClick={props.onClose}
            />
          </div>
        </Fragment>
      </Fade>
    </Modal>
  );
};
export default withStyles(useStyles)(ClearFilterModal);
