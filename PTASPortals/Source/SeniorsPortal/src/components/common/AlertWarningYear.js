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
import Button from '@material-ui/core/Button';
import { FormattedMessage } from 'react-intl';
import CustomButton from '../common/CustomButton';

function AlertWarningYear(props) {
  const [open, setOpen] = useState(false);
  const { text, isOpen, handleDeleteFinancialForms, onClose } = props;

  useEffect(() => {
    setOpen(isOpen);
  }, [isOpen]);

  function handleClose() {
    setOpen(false);
    onClose();
  }

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
        <div className="info-icon-alert" style={{ width: '75%' }}>
          <DialogContentText
            id="alert-dialog-description"
            style={{ color: 'black' }}
          >
            {text}
          </DialogContentText>
          <Grid
            container
            direction="row"
            spacing={20}
            style={{ padding: '0 0px 0 10%' }}
          >
            <Grid item sm={4}>
              <CustomButton
                testId={'changeYearYes'}
                style={{ marginBottom: 30 }}
                onClick={handleDeleteFinancialForms}
                label={
                  <FormattedMessage id="changeYearYes" defaultMessage="Yes" />
                }
              />
            </Grid>
            <Grid item sm={4}></Grid>
            <Grid item sm={4}>
              <CustomButton
                style={{
                  margin: '10px 0',
                }}
                secondary={true}
                btnBigLabel={true}
                onClick={handleClose}
                label={
                  <FormattedMessage id="changeYearNo" defaultMessage="No" />
                }
              />
            </Grid>
          </Grid>
        </div>
      </DialogContent>
    </Dialog>
  );
}

export default AlertWarningYear;
