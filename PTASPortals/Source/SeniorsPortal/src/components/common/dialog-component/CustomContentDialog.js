//-----------------------------------------------------------------------
// <copyright file="CustomContentDialog.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React, { useRef, useEffect } from 'react';
import * as fm from './FormatTexts';
import './CustomDialog.css';
import {
  renderIf,
  arrayNullOrEmpty,
  checkClientBrowser,
} from '../../../lib/helpers/util';

import { makeStyles, withStyles } from '@material-ui/core/styles';

import { withRouter } from 'react-router-dom';
import { FormattedMessage } from 'react-intl';

import CustomButton from '../CustomButton';
import BorderColorIcon from '@material-ui/icons/BorderColor';

import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';

import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';

import Grid from '@material-ui/core/Grid';
import FormControl from '@material-ui/core/FormControl';

import ReactToPrint from 'react-to-print';
import IncomeHelp from '../../income-help/IncomeHelp';
import Faq from '../../faq/Faq';

import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import WarningOutlinedIcon from '@material-ui/icons/WarningOutlined';
import ErrorOutlinedIcon from '@material-ui/icons/ErrorOutlined';
const CustomButtonCss = withStyles({
  button: {
    marginTop: '14px',
    backgroundColor: 'white',
    color: '#6f1f62',
    borderStyle: 'solid',
    borderWidth: '2px',
    borderColor: '#6f1f62',
    fontSize: '16px',
  },
})(CustomButton);

const DialogB = withStyles({
  root: {
    '& .MuiDialog-paper': {
      overflowY: 'scroll !important',
      height: '90vh !important',

      minWidth: '600px !important',
    },
    '.MuiDialog-paperScrollPaper': {
      maxHeight: '600px !important',
      height: '90vh !important',
    },
  },
})(Dialog);
const DialogSmall = withStyles({
  root: {
    width: '88%',
    marginLeft: 50,
    marginTop: 40,
    '& .MuiDialog-paper': {
      border: '3px solid black',
      width: '630px !important',
    },
  },
})(Dialog);
function CustomContentDialog(props) {
  const renderSwitch = param => {
    switch (param) {
      case 1:
        return <WarningOutlinedIcon className="clear-icon" />;
      case 2:
        return (
          <ErrorOutlinedIcon style={{ color: 'red' }} className="clear-icon" />
        );

      default:
        return <InfoOutlinedIcon className="clear-icon" />;
    }
  };

  const ITEM_HEIGHT = 48;

  const ITEM_PADDING_TOP = 8;
  const MenuProps = {
    PaperProps: {
      style: {
        maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
        width: 250,
      },
    },
  };

  const componentRef = useRef();
  return (
    <React.Fragment>
      {renderIf(
        props.small && props.showContentDialog,

        <DialogSmall
          maxWidth={true}
          onClose={props.toggleDialog}
          aria-labelledby="customized-dialog-title"
          scroll={'paper'}
          open={props.showContentDialog}
          style={{ top: -20 }}
          className={'no-focus'}
        >
          {!props.noActions ? (
            <div className={'custom-dialog-content-header'}>
              <DialogActions>
                {props.dialogTitle && props.dialogTitle !== '' ? (
                  <DialogTitle
                    id="scroll-dialog-title"
                    style={{ position: 'absolute', left: 0 }}
                  >
                    {props.dialogTitle !== '' && (
                      <React.Fragment>
                        <InfoOutlinedIcon className="clear-icon" />{' '}
                        {props.dialogTitle}
                      </React.Fragment>
                    )}
                  </DialogTitle>
                ) : (
                  ''
                )}

                {!props.hidePrint && (
                  <ReactToPrint
                    trigger={() => (
                      <CustomButtonCss secondaryWhite label={fm.print} />
                    )}
                    content={() => componentRef.current}
                  />
                )}
                <IconButton onClick={props.toggleDialog}>
                  <ClearIcon style={{ width: '2em', height: '2em' }} />
                </IconButton>
              </DialogActions>
            </div>
          ) : (
            <React.Fragment>
              <BorderColorIcon
                style={{
                  color: 'black',
                  width: '2em',
                  height: '2em',
                  marginTop: 24,
                  left: '6%',
                  position: 'absolute',
                }}
              />
              <IconButton
                onClick={props.toggleDialog}
                style={{ right: '0%', width: '50px', position: 'absolute' }}
              >
                <ClearIcon
                  style={{ color: 'black', width: '2em', height: '2em' }}
                />
              </IconButton>
            </React.Fragment>
          )}

          <DialogContent className="no-focus">
            <DialogContentText
              id="scroll-dialog-description"
              ref={componentRef}
              tabIndex={-1}
            >
              {props.children}
            </DialogContentText>
          </DialogContent>
        </DialogSmall>
      )}
      {props.showContentDialog && !props.small ? (
        <DialogB
          maxWidth={true}
          onClose={props.toggleDialog}
          aria-labelledby="customized-dialog-title"
          scroll={'paper'}
          open={props.showContentDialog}
          style={{ top: -20, overflowY: 'scroll' }}
          className={'no-focus'}
        >
          <div className={'custom-dialog-content-header'}>
            <DialogActions
              style={{
                padding: '0',
                position: checkClientBrowser() ? 'absolute' : 'inherit',
                right: 0,
              }}
            >
              {!props.hidePrint && (
                <ReactToPrint
                  trigger={() => <CustomButtonCss label={fm.print} />}
                  content={() => componentRef.current}
                />
              )}
              <IconButton onClick={props.toggleDialog}>
                <ClearIcon style={{ width: '2em', height: '2em' }} />
              </IconButton>
            </DialogActions>

            <DialogTitle id="scroll-dialog-title">
              {props.dialogTitle}
            </DialogTitle>
          </div>
          <DialogContent
            className="no-focus"
            dividers={true}
            style={{ marginTop: checkClientBrowser() ? 70 : 0 }}
          >
            <DialogContentText
              id="scroll-dialog-description"
              ref={componentRef}
              tabIndex={-1}
            >
              {props.children}
            </DialogContentText>
          </DialogContent>
        </DialogB>
      ) : (
        ''
      )}
    </React.Fragment>
  );
}
export default withRouter(CustomContentDialog);
