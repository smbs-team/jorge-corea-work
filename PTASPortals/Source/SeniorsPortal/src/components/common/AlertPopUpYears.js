import React, { useState, useEffect } from 'react';

import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import IconButton from '@material-ui/core/IconButton';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import Draggable from 'react-draggable';
import ClearIcon from '@material-ui/icons/Clear';
import Paper from '@material-ui/core/Paper';
import Grid from '@material-ui/core/Grid';
import RadioInput from '../common/RadioInput';
import { withStyles } from '@material-ui/core/styles';

function AlertPopUpYears(props) {
  const {
    text,
    isOpen,
    yearOptionsArray,
    onChangeRadioButton,
    onClose,
    itemMsg,
  } = props;

  const RadioInputCss = withStyles({
    root: {
      marginTop: '19px',
    },
    label: {
      maxWidth: '310px',
      fontSize: '14px',
    },
  })(RadioInput);

  function PaperComponent(props) {
    return (
      <Draggable cancel={'[class*="MuiDialogContent-root"]'}>
        <Paper {...props} />
      </Draggable>
    );
  }

  return (
    <Dialog
      open={isOpen}
      onClose={() => onClose(false)}
      PaperComponent={PaperComponent}
      aria-describedby="alert-dialog-description"
      aria-labelledby="draggable-dialog-title"
    >
      <DialogTitle
        style={{ cursor: 'move', padding: 0 }}
        id="draggable-dialog-title"
      ></DialogTitle>
      <DialogActions style={{ padding: '0px' }}>
        <IconButton onClick={() => onClose(false)}>
          <ClearIcon style={{ width: '2em', height: '2em' }} />
        </IconButton>
      </DialogActions>
      <DialogContent style={{ marginTop: '-50px', marginBottom: '20px' }}>
        <div className="info-icon-alert" style={{ width: '75%' }}>
          <DialogContentText
            id="alert-dialog-description"
            style={{ color: 'black' }}
          >
            {text}
          </DialogContentText>
          <Grid container spacing={20} style={{ padding: '0 0px 0 27%' }}>
            <Grid item sm={12}>
              <RadioInputCss
                source={yearOptionsArray}
                onChange={onChangeRadioButton}
                itemMsg={itemMsg}
              />
            </Grid>
          </Grid>
        </div>
      </DialogContent>
    </Dialog>
  );
}

export default AlertPopUpYears;
