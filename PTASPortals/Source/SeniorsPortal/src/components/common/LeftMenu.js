import React from 'react';
import './LeftMenu.css';

import { List, ListItem, ListItemText, Drawer } from '@material-ui/core';
import { FormattedMessage } from 'react-intl';
import OndemandVideo from '@material-ui/icons/OndemandVideo';
import Description from '@material-ui/icons/Description';
import CalendarToday from '@material-ui/icons/CalendarToday';
import QRCodeBlock from './QRCodeBlock';
import { renderIf } from '../../lib/helpers/util';

const LeftMenu = props => {
  console.log('LeftMenuprops', props);
  let showQR = process.env.REACT_APP_SHOW_QR === 'true' ? true : false;
  const drawer = (
    <List className="side-menu-list">
      <ListItem
        button
        key="Documents you'll need"
        onClick={props.toggleDialog}
        className="list-item"
        style={{
          margin: props.isIE == true ? '0px' : '',
          width: props.isIE == true ? '20% !important' : '',
        }}
      >
        <Description className="menu-icon" />
        <ListItemText
          style={{
            width: props.isIE == true ? '80% !important' : '',
          }}
          primary={
            <FormattedMessage
              id="documents_you_need"
              defaultMessage="Documents you'll need"
            />
          }
          className="menu-text"
        />
      </ListItem>
      <ListItem
        button
        key="Watch help videos"
        className="list-item"
        onClick={props.callHelpVideo}
        style={{
          maxHeight: props.isIE == true ? '100px' : '',
          minWidth: props.isIE == true ? '280px' : '',
        }}
      >
        <OndemandVideo className="menu-icon" />
        <ListItemText
          style={{
            margin: props.isIE == true ? '0px' : '',
            width: props.isIE == true ? '20% !important' : '',
          }}
          primary={
            <FormattedMessage
              id="watch_help_videos"
              defaultMessage="Watch help videos"
            />
          }
          className="menu-text"
        />
      </ListItem>

      {renderIf(
        props.selectedYear,
        <ListItem
          button
          key="First eligible year"
          onClick={() => props.handleYearsPopUpDisplay(true)}
          className="list-item"
          style={{
            width: props.isIE == true ? '80% !important' : '',
          }}
        >
          <CalendarToday className="menu-icon" />
          <ListItemText
            className="menu-text"
            style={{
              maxHeight: props.isIE == true ? '100px' : '',
              minWidth: props.isIE == true ? '280px' : '',
            }}
          >
            <FormattedMessage
              id="first_eligible_year"
              defaultMessage="Applying for: {year}"
              style={{
                margin: props.isIE == true ? '0px' : '',
                Width: props.isIE == true ? '20% !important' : '',
              }}
              values={{
                year: props.selectedYear,
              }}
            />
          </ListItemText>
        </ListItem>
      )}
      <ListItem
        className="list-item"
        style={{
          Width: props.isIE == true ? '80% !important' : '',
          visibility: showQR ? 'visible' : 'hidden',
        }}
      >
        <QRCodeBlock QRCode={props.QRCode} />
      </ListItem>
    </List>
  );

  return (
    <div>
      <List
        className="left-menu-list"
        style={{
          top: props.isIE == true ? '200px' : '0px',
          width: props.isIE == true ? '300px' : '',
          position: props.isIE == true ? 'relative' : '',
          flex: props.isIE ? '1 0 auto' : '',
        }}
      >
        {drawer}
      </List>
    </div>
  );
};

export default LeftMenu;
