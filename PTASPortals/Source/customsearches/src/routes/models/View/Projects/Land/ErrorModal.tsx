// FILENAME
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import CloseIcon from '@material-ui/icons/Close';
import lodash from 'lodash';
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

interface ValidationErrorType {
  executedStatement: string;
  success: boolean;
  validatedExpression: string;
  validatedExpressionRole: string;
  validatedExpressionType: string;
  validatedIndex: number;
  validationError: string;
  validationErrorDetails: string;
}

interface ErrorWithValidation {
  message: string;
  validationErrors: ValidationErrorType[];
}

interface Props extends WithStyles<typeof useStyles> {
  isOpen?: boolean;
  error?: ErrorWithValidation;
  onClose?: () => void;
}

const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      borderRadius: 12,
      boxShadow: theme.shadows[5],
      width: 550,
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
        fontWeight: 700
    },
    closeButton: {
      position: 'absolute',
      top: 15,
      right: 15,
    }
  });

const ErrorModal = (props: Props): JSX.Element => {
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

  const getValidationMessage = (
    value: string | number,
    index: number,
    names: string[]
  ): JSX.Element => {
    if (typeof value === 'string' && value.length > 50) {
      return (
        <div>
          <span className={classes.queryclass}>
            {lodash.startCase(names[index])}:{' '}
          </span>
          <textarea
            cols={64}
            rows={10}
            value={value}
            disabled={true}
          ></textarea>
        </div>
      );
    }
    return (
      <div>
        <span className={classes.bold}>{lodash.startCase(names[index])}:</span>
        <span>{value}</span>
      </div>
    );
  };

  //eslint-disable-next-line
  const renderValidationErrors = (): any => {
    return props?.error?.validationErrors?.map((validationError) => {
      const { success, ...rest } = validationError;
      const names = Object.keys(rest);
      return Object.values(rest).map((value, index) =>
        getValidationMessage(value, index, names)
      );
    });
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
            <label className={classes.label}>{props.error?.message}</label>
            <div className={classes.content}>{renderValidationErrors()}</div>
            <CustomButton
              classes={{ root: classes.button }}
              onClick={(): void => {
                handleClose();
              }}
              fullyRounded
            >
              Ok
            </CustomButton>
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
export default withStyles(useStyles)(ErrorModal);
