//-----------------------------------------------------------------------
// <copyright file="Header.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React, { Component } from 'react';
import './Header.css';
import countyLogo from '../../assets/logo-kingcounty-inverse.svg';
import downArrow from '../../assets/downArrowWhite.png';
import { FormattedMessage } from 'react-intl';
import Menu from '@material-ui/core/Menu';
import MenuItem from '@material-ui/core/MenuItem';
import { AuthConsumer, AuthContext } from '../../contexts/AuthContext';
import { SignalRConsumer } from '../../contexts/SignalRContext';
import { renderIf, checkClientBrowser } from '../../lib/helpers/util';
import { Button, withStyles } from '@material-ui/core';
import BrowserDialog from './dialog-component/BrowserDialog';
import device from 'current-device';

class Header extends Component {
  state = {
    anchorEl: null,
    anchorOriginVertical: 'bottom',
    anchorOriginHorizontal: 'right',
    transformOriginVertical: 'top',
    transformOriginHorizontal: 'right',
    anchorReference: 'anchorEl',
    menuWidth: 236,
    isIE: checkClientBrowser(),
    mobile: device.mobile(),
  };

  componentDidMount = async () => {
    this.setState({
      menuWidth: this.refs.menuButton
        ? this.refs.menuButton.offsetWidth + 58
        : 236,
    });
  };

  handleClick = event => {
    this.setState({ anchorEl: event.currentTarget });
  };

  handleClose = () => {
    this.setState({ anchorEl: null });
  };

  handleCloseContinue = state => {
    this.setState({ [state]: !this.state[state] });
  };

  render = () => {
    const { anchorEl } = this.state;
    const open = Boolean(anchorEl);
    const menuButton = (
      <div className="menu-button body-small nogap" ref="menuButton">
        <Button id="sign-in" href="/home">
          <FormattedMessage id="signIn" defaultMessage="Sign in" />
        </Button>
      </div>
    );

    const StyledMenu = withStyles({
      paper: { width: this.state.menuWidth, borderRadius: 0 },
    })(Menu);

    const device = require('current-device').default;

    return (
      <AuthConsumer>
        {value => (
          <SignalRConsumer>
            {({ logOutAndDisconnectFromHub }) => (
              <React.Fragment>
                <header className="header">
                  <img src={countyLogo} alt="King County" id="logo" />
                  <nav>
                    {renderIf(
                      value.user,
                      <div className="menu-button body-small nogap">
                        <font
                          aria-controls="simple-menu"
                          aria-haspopup="true"
                          onClick={this.handleClick}
                          size="3"
                        >
                          {value.user ? `${value.user.email} ` : ''}
                          <img
                            src={downArrow}
                            alt="King County"
                            className="arrow-down"
                          />
                        </font>

                        <StyledMenu
                          className="dark"
                          id="simple-menu"
                          anchorEl={anchorEl}
                          getContentAnchorEl={null}
                          anchorOrigin={{
                            vertical: 'bottom',
                            horizontal: 'center',
                          }}
                          transformOrigin={{
                            vertical: 'top',
                            horizontal: 'center',
                          }}
                          open={open}
                          onClose={this.handleClose}
                        >
                          <MenuItem
                            onClick={() =>
                              logOutAndDisconnectFromHub(value.logout)
                            }
                            className="header-menu"
                          >
                            <FormattedMessage
                              id="signOut"
                              defaultMessage="Sign out"
                            />
                          </MenuItem>
                        </StyledMenu>
                      </div>,
                      menuButton
                    )}
                  </nav>
                </header>
                {renderIf(
                  this.state.isIE,
                  <BrowserDialog
                    showDialog={true}
                    handleCloseContinue={() => this.handleCloseContinue('isIE')}
                    text="updateBrowser"
                    icon="warning"
                    fontSize={24}
                  />
                )}
                {renderIf(
                  this.state.mobile,
                  <BrowserDialog
                    showDialog={true}
                    handleCloseContinue={() =>
                      this.handleCloseContinue('mobile')
                    }
                    text="useDesktop"
                    icon="laptop"
                    fontSize={16}
                  />
                )}
              </React.Fragment>
            )}
          </SignalRConsumer>
        )}
      </AuthConsumer>
    );
  };
}

Header.contextType = AuthContext;
export default Header;
