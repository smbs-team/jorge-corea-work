/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { QUERY_PARAM } from 'appConstants';
import { uniqBy } from 'lodash';
import { utilService } from 'services/common';
import { rendererService } from 'services/map';
import { signalRService } from 'services/parcel/selection/signalRService';
import { apiService } from '../apiService';
import { axiosCsInstance } from '../axiosInstances';

export type UpdateParcelSelectionData = {
  Major: string;
  Minor: string;
  selection: boolean;
};

class DsSelection {
  queue: UpdateParcelSelectionData[] = [];
  loading = false;

  constructor() {
    setInterval(() => {
      if (this.queue.length) {
        console.log('=== Ds selection ===');
        console.log('Queue');
        console.log(this.queue);
        console.log('loading ' + this.loading);
      }
    }, 10000);
  }

  /**
   * Select and unselect parcels
   *
   * @param data - Selected or unselected parcels
   */
  update = async (data: UpdateParcelSelectionData[]): Promise<unknown> => {
    if (this.loading) {
      this.queue.push(...data);
      return;
    }
    if (data.length > 250) {
      this.queue.push(...data.slice(250, data.length));
    }
    const datasetId =
      utilService.getUrlSearchParam(QUERY_PARAM.DATASET_ID) ||
      rendererService.getDatasetId();
    if (!datasetId) return null;
    this.loading = true;
    const chunk = uniqBy(
      data.slice(0, 250).reverse(),
      (item) => item.Major + item.Minor
    );
    await axiosCsInstance.post<{ data: unknown }>(
      `/CustomSearches/UpdateDatasetData/${datasetId}`,
      chunk,
      {
        headers: {
          Authorization: 'Bearer ' + apiService.b2cToken,
        },
        params: {
          clientId: signalRService.clientId,
        },
      }
    );
    this.loading = false;
    if (this.queue.length) {
      const chunk = this.queue.splice(0, 250);
      return await this.update(chunk);
    }
  };
}

export default DsSelection;
