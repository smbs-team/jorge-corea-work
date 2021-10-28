import React from 'react';
import PropTypes from 'prop-types';
import './CustomTab.css';
import { FormattedMessage } from 'react-intl';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import { makeStyles, useTheme, withStyles } from '@material-ui/core/styles';
import {
  Toolbar,
  IconButton,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Hidden,
  Drawer,
} from '@material-ui/core';
import OndemandVideo from '@material-ui/icons/OndemandVideo';
import Description from '@material-ui/icons/Description';
import CalendarToday from '@material-ui/icons/CalendarToday';
import { renderIf } from '../../lib/helpers/util';
import LeftMenu from '../common/LeftMenu';

function a11yProps(index) {
  return {
    id: `full-width-tab-${index}`,
    'aria-controls': `full-width-tabpanel-${index}`,
  };
}
const useStyles = makeStyles((theme) => ({
  root: {
    width: '100%',
    maxWidth: 722,
    flexGrow: 1,
    border: 'none',
  },
  drawerPaper: {
    top: 0,
    position: 'sticky',
    border: 'none',
    background: 'none',
  },
}));

const StyledTabs = withStyles({
  indicator: {
    backgroundColor: '#a5c727',
  },
})(Tabs);

const SideTabs = withStyles({
  indicator: {
    display: 'none',
    height: 4,
  },
})(Tabs);

const StyledTab = withStyles((theme) => ({
  root: {
    [theme.breakpoints.up('sm')]: {
      fontSize: '1.4rem',
    },
    [theme.breakpoints.down('xs')]: {
      fontSize: '0.9rem',
    },
    textTransform: 'none',
    fontWeight: 600,
    padding: 0,
  },
}))(Tab);

const listRef = React.createRef();

const CustomTab = (props) => {
  const classes = useStyles();

  const drawer = (
    <LeftMenu
      handleYearsPopUpOptions={props.handleYearsPopUpOptions}
      handleYearsPopUpDisplay={props.handleYearsPopUpDisplay}
      displayYearsPopUp={props.displayYearsPopUp}
      selectedYear={props.selectedYear}
      QRCode={props.QRCode}
      toggleDialog={props.toggleDialog}
      callHelpVideo={props.callHelpVideo}
      checkClientBrowser={props.checkClientBrowser}
      isIE={props.isIE}
    />
  );

  const display = props.display ? 'block' : 'none';
  console.log('LeftMenuprops CustomTab', props);
  return (
    <div className={`custom-tab ${classes.root}`}>
      <AppBar
        position="sticky"
        color="transparent"
        //style={{ height: 49 }}
        elevation={0}
        className="tabs-app-bar"
      >
        {!props.display && (
          <div className="text-center">
            <h1> {props.displayReplacementTitle}</h1>
          </div>
        )}

        <StyledTabs
          value={props.value}
          onChange={props.handleTabChange}
          textColor="primary"
          aria-label="Application navigation"
          variant="scrollable"
          scrollButtons="on"
          style={{
            visibility: `${props.display ? 'visible' : 'collapse'}`,
          }}
        >
          <StyledTab
            className="styled-tab no-focus"
            label={
              <FormattedMessage id="your_info" defaultMessage="Your info" />
            }
            {...a11yProps(0)}
          />
          <StyledTab
            className="styled-tab no-focus"
            label={
              <FormattedMessage
                id="property_info"
                defaultMessage="Property info"
              />
            }
            {...a11yProps(1)}
          />
          <StyledTab
            className="styled-tab no-focus"
            label={
              <FormattedMessage
                id="financial_info"
                defaultMessage="Financial info"
              />
            }
            {...a11yProps(2)}
          />
          <StyledTab
            className="styled-tab no-focus"
            label={<FormattedMessage id="summary" defaultMessage="Summary" />}
            {...a11yProps(3)}
          />
        </StyledTabs>
        {renderIf(
          props.value != 3,
          <Hidden lgUp implementation="css">
            <SideTabs
              value={props.value}
              onChange={props.handleTabChange}
              textColor="primary"
              aria-label="Application tools"
              variant="scrollable"
              scrollButtons="on"
            >
              {drawer}
            </SideTabs>
          </Hidden>
        )}
      </AppBar>
      {renderIf(
        props.value != 3,
        <div className="drawer">
          <Hidden mdDown implementation="css">
            <Drawer
              classes={{
                paper: classes.drawerPaper,
              }}
              variant="permanent"
              open
            >
              {drawer}
            </Drawer>
          </Hidden>
        </div>
      )}
    </div>
  );
};

export default CustomTab;

CustomTab.propTypes = {
  value: PropTypes.number.isRequired,
  handleTabChange: PropTypes.func.isRequired,
};
