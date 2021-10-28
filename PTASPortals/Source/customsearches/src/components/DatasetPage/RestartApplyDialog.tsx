/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import { DialogContent, DialogContentText } from '@material-ui/core';
import { CustomButton } from '@ptas/react-ui-library';

interface Props {
  open: boolean;
  toggle: () => void;
  accept: () => void;
  title?: string;
}

const RestartApplyDialog = (props: Props): JSX.Element => {
  return (
    <Dialog
      open={props.open}
      onClose={props.toggle}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description"
    >
      <DialogContent>
        <DialogContentText
          id="alert-dialog-description"
          style={{ color: 'black', fontWeight: 600 }}
        >
          {props.title?.length
            ? props.title
            : 'Are you sure you want to reply the model and lose an changes done to the current dataset?'}
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <CustomButton onClick={props.toggle}>
          No
        </CustomButton>
        <CustomButton onClick={props.accept}>
          Yes
        </CustomButton>
      </DialogActions>
    </Dialog>
  );
};

export default RestartApplyDialog;
