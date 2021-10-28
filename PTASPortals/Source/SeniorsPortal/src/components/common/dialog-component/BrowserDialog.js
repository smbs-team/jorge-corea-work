import React from 'react';
import * as fm from './FormatTexts';

import { FormattedMessage } from 'react-intl';

import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import DialogActions from '@material-ui/core/DialogActions';

import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import WarningOutlinedIcon from '@material-ui/icons/WarningOutlined';
import LaptopChromebookIcon from '@material-ui/icons/LaptopChromebook';
import { renderIf } from '../../../lib/helpers/util';

class BrowserDialog extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div
        open={this.props.showDialog}
        onClose={this.props.handleCloseContinue}
        style={{
          width: '94%',
          margin: '9px 3% 0',
        }}
      >
        <div
          style={{
            maxWidth: 894,
            border: '1px solid black',
            margin: 'auto',
            boxShadow:
              '0 4px 8px 0 rgba(128, 128, 128, 0.1), 0 6px 20px 0 rgba(0, 0, 0, 0.09)',
          }}
        >
          <DialogActions style={{ padding: 0 }}>
            <IconButton onClick={this.props.handleCloseContinue}>
              <ClearIcon
                className="clear-icon"
                style={{
                  width: '1.5em',
                  height: '1.5em',
                  color: 'black',
                }}
              />
            </IconButton>
          </DialogActions>
          <DialogContent style={{ marginTop: '-60px' }}>
            <div
              className="info-icon-alert"
              style={{
                margin: '3% 27px 0 1%',
                transform: 'scale(2)',
              }}
            >
              {renderIf(
                this.props.icon == 'warning',
                <WarningOutlinedIcon
                  className="clear-icon"
                  style={{ height: '1.5em', width: '1.5em' }}
                />
              )}
            </div>
            <div
              className="info-icon-alert"
              style={{ width: this.props.icon == 'warning' ? '85%' : '100%' }}
            >
              {renderIf(
                this.props.icon == 'laptop',
                <LaptopChromebookIcon
                  className="clear-icon"
                  style={{ height: '1.5em', width: '100%' }}
                />
              )}
              <DialogContentText
                id="alert-dialog-description"
                style={{ color: 'black', fontSize: this.props.fontSize }}
              >
                <p>{fm[this.props.text]}</p>
              </DialogContentText>
            </div>
          </DialogContent>
        </div>
      </div>
    );
  }
}

export default BrowserDialog;
