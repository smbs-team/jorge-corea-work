//-----------------------------------------------------------------------
// <copyright file="SignalRContext.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { AuthContext } from './AuthContext';
import { getSignalRKey, sendMessage } from '../services/signalrService';
import { cloneDeep } from 'lodash';
import { postValue } from '../services/jsonStoreService';

const SignalRContext = React.createContext();

/**
 * Will handle the connection to the SignalR service.
 *
 * @class SignalRProvider
 * @extends {React.Component}
 */
class SignalRProvider extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      url: process.env.REACT_APP_SIGNALR_API,
      token: null,
      hubConnection: null,
      connected: false,
      messages: [],
      gettingToken: false,
      uploadsData: null,
    };
  }

  componentDidMount() {
    this.connectToHub();
    window.addEventListener('beforeunload', this.onBrowserClosedMessage);
  }

  componentDidUpdate() {
    if (!this.state.connected && !this.state.hubConnection) {
      this.connectToHub();
    }
  }

  componentWillUnmount() {
    window.removeEventListener('beforeunload', this.onBrowserClosedMessage);
  }

  /**
   * Event handler that will send a SignalR Message when the browser/tab is closed to notify any client of the event.
   *
   * @memberof SignalRProvider
   */
  onBrowserClosedMessage = async () => {
    if (
      this.state.uploadsData &&
      this.state.uploadsData.contactId &&
      this.state.uploadsData.seniorAppId
    ) {
      let newUpload = cloneDeep(this.state.uploadsData);
      newUpload.refreshDate = new Date().toUTCString();
      newUpload.status = 'closed';
      this.setState({ uploadsData: newUpload });
      await postValue(
        `${newUpload.contactId}/${newUpload.seniorAppId}/uploads`,
        newUpload,
        null
      );
    }

    await this.sendMessage('closed');
  };

  /**
   * Creates a connection to the SignalR Hub.
   *
   * @memberof SignalRProvider
   */
  connectToHub = async () => {
    if (this.context.isLoggedIn()) {
      if (!this.state.token) {
        if (!this.state.gettingToken) {
          this.setState({ gettingToken: true });
          const token = await getSignalRKey();
          if (token) {
            this.setState({ token, gettingToken: false }, () => {
              this.connectToHub();
            });
          }
        }
      } else if (this.context.contact) {
        const hubConnection = new HubConnectionBuilder()
          .withUrl(this.state.url, {
            accessTokenFactory: () => this.state.token,
          })
          // .configureLogging(signalR.LogLevel.Trace)
          .withAutomaticReconnect()
          .build();

        hubConnection.on(this.context.contact.contactid, (name, message) => {
          // Ignore messages coming from web
          if (name !== 'web user') {
            let messages = cloneDeep(this.state.messages);
            messages.push({ from: name, message });
            this.setState({ messages });
          }
        });

        hubConnection.onclose(() => {
          this.setState({ connected: false });
        });

        hubConnection.onreconnecting(err =>
          console.log('err reconnecting  ', err)
        );

        hubConnection
          .start()
          .then(res => {
            this.setState({ connected: true });
          })
          .catch(console.error);

        this.setState({ hubConnection });
      }
    }
  };

  /**
   * This method will send a notification to inform the session is being closed (same as when the browser closes) and will also
   * disconnect from the SignalR hub and will call the logout function.
   * @memberof SignalRProvider
   */
  logOutAndDisconnectFromHub = async logOutFunction => {
    await this.onBrowserClosedMessage();
    await this.state.hubConnection?.stop();
    if (logOutFunction) {
      logOutFunction();
    }
  };

  /**
   * Send a notification to the contact id with the specified message.
   *
   * @memberof SignalRProvider
   */
  sendMessage = async message => {
    if (this.state.hubConnection) {
      let notification = {
        text: message,
        target: this.context.contact.contactid,
        name: 'web user',
      };

      await sendMessage(notification);
    }
  };

  /**
   *
   *
   * @memberof SignalRProvider
   */
  setUpload = uploadsData => {
    this.setState({ uploadsData });
  };

  render = () => {
    console.log('SignalRContext', this.state);
    return (
      <SignalRContext.Provider
        value={{
          sendMessage: this.sendMessage,
          setUpload: this.setUpload,
          messages: this.state.messages,
          uploadsData: this.state.uploadsData,
          logOutAndDisconnectFromHub: this.logOutAndDisconnectFromHub,
        }}
      >
        {this.props.children}
      </SignalRContext.Provider>
    );
  };
}

const SignalRConsumer = SignalRContext.Consumer;
SignalRProvider.contextType = AuthContext;
export { SignalRProvider, SignalRConsumer, SignalRContext };
