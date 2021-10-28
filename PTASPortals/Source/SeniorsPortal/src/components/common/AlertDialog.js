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

function AlertDialog(props) {
  const [open, setOpen] = useState(false);
  const { text, isOpen, onClose } = props;

  useEffect(() => {
    setOpen(isOpen);
  }, [isOpen]);

  function handleClose() {
    setOpen(false);
    onClose();
  }

  function PaperComponent(props) {
    return (
      <Draggable cancel={'[class*="MuiDialogContent-root"]'}>
        <Paper {...props} />
      </Draggable>
    );
  }

  return (
    <Dialog
      open={open}
      onClose={handleClose}
      PaperComponent={PaperComponent}
      aria-describedby="alert-dialog-description"
      aria-labelledby="draggable-dialog-title"
    >
      <DialogTitle
        style={{ cursor: 'move', padding: 0 }}
        id="draggable-dialog-title"
      ></DialogTitle>
      <DialogActions style={{ padding: '0px' }}>
        <IconButton onClick={handleClose}>
          <ClearIcon style={{ width: '2em', height: '2em' }} />
        </IconButton>
      </DialogActions>
      <DialogContent style={{ marginTop: '-50px', marginBottom: '20px' }}>
        <div className="info-icon-alert" style={{ marginRight: '27px' }}>
          <InfoOutlinedIcon style={{ width: '2em', height: '2em' }} />
        </div>
        <div className="info-icon-alert" style={{ width: '75%' }}>
          <DialogContentText
            id="alert-dialog-description"
            style={{ color: 'black' }}
          >
            {text}
          </DialogContentText>
        </div>
      </DialogContent>
    </Dialog>
  );
}

export default AlertDialog;
