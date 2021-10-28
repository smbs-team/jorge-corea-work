/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import * as signalR from '@microsoft/signalr';
import { QUERY_PARAM } from 'appConstants';
import { once } from 'lodash';
import { datasetService } from 'services/api';
import { utilService } from 'services/common';
import { rendererService } from 'services/map';
class SignalRService {
  readonly clientId = 'mapboxcontrol';
  connection: signalR.HubConnection | undefined;

  initSignalRConnection = once((token: string): void => {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env.REACT_APP_CUSTOM_SEARCHES_URL}`, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();
    this.startConnection();
  });

  datasetSelectionChange = (datasetId: string, clientId: string): void => {
    const ds =
      utilService.getUrlSearchParam(QUERY_PARAM.DATASET_ID) ??
      rendererService.getDatasetId();
    if (datasetId !== ds || clientId === this.clientId) return;
    console.log('SignalR');
    console.log('Client id =', clientId);
    console.log('Dataset Id =', datasetId);
    console.log('Current ds = ', ds);
    datasetService.getColorRendererData({
      force: true,
    });
  };

  startConnection = async (): Promise<void> => {
    try {
      await this.connection?.start();
      // this.connection?.on('JobProcessed', this.messageReceived);
      this.connection?.on(
        'DatasetSelectionChanged',
        this.datasetSelectionChange
      );
    } catch (error) {
      console.log(error);
      setTimeout(this.startConnection, 5000);
    }
  };
}

export const signalRService = new SignalRService();
