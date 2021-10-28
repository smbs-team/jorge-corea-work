import React from 'react';
import './SideMenu.css';
import { renderIf } from '../../lib/helpers/util';
import {
  Hidden,
  List,
  ListItem,
  ListItemText,
  CssBaseline,
  Drawer,
} from '@material-ui/core';
import MenuIcon from '@material-ui/icons/Menu';
import { makeStyles } from '@material-ui/core/styles';
import { FormattedMessage } from 'react-intl';

const useStyles = makeStyles(theme => ({
  root: {
    position: 'sticky',
    top: 0,
    right: 0,
    width: 'calc(100vw - (100vw - 100%) / 2 - 9px)',
  },
  appBar: {},
  drawer: {
    flexShrink: 0,
    position: 'absolute',
    top: 0,
    right: 0,
    minWidth: 150,
    width: '15%',
  },
  drawerPaper: {
    position: 'sticky',
    color: '#6f1f62',
    borderLeft: 'none',
    backgroundColor: '#fafafa88',
  },
  toolbar: theme.mixins.toolbar,
  content: {
    flexGrow: 1,
    backgroundColor: theme.palette.background.default,
    padding: theme.spacing(3),
  },
  itemText: { backgroundColor: 'red' },
}));

const SideMenu = props => {
  const classes = useStyles();

  const drawer = (
    <List className="side-menu-list">
      <ListItem key="On this page" className="side-menu-item side-menu-header">
        On this page
      </ListItem>
      <ListItem
        button
        key="Overview"
        onClick={() => console.log('CLICKED')}
        className="side-menu-item"
      >
        <ListItemText
          primary={
            <FormattedMessage id="intro_overview" defaultMessage="Overview" />
          }
        />
      </ListItem>
      <ListItem
        button
        key="Watch help videos"
        onClick={() => console.log('CLICKED')}
        className="side-menu-item"
      >
        <ListItemText primary="Watch help videos" />
      </ListItem>
      <ListItem
        button
        key="Getting help"
        onClick={() => console.log('CLICKED')}
        className="side-menu-item"
      >
        <ListItemText primary="Getting help" />
      </ListItem>
      <ListItem
        button
        key="Qualification details"
        onClick={() => console.log('CLICKED')}
        className="side-menu-item"
      >
        <ListItemText primary="Qualification details" />
      </ListItem>
      <ListItem
        button
        key="Application details"
        onClick={() => console.log('CLICKED')}
        className="side-menu-item"
      >
        <ListItemText primary="Application details" />
      </ListItem>
      <ListItem
        button
        key="More info"
        onClick={() => console.log('CLICKED')}
        className="side-menu-item"
      >
        <ListItemText primary="More info" />
      </ListItem>
    </List>
  );

  return (
    <div className={classes.root}>
      <CssBaseline />
      <Hidden mdDown implementation="css">
        <Drawer
          className={classes.drawer}
          variant="permanent"
          classes={{
            paper: classes.drawerPaper,
          }}
          anchor="right"
        >
          {drawer}
        </Drawer>
      </Hidden>
    </div>
  );
};

export default SideMenu;
