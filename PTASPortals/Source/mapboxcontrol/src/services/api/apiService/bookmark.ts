/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import axios, { Canceler } from 'axios';
import { ApiServiceResult, handleReq } from 'services/common';
import {
  BookmarkItemResp,
  BookmarkTagsItem,
  CreateUpdateBookmarkData,
  OptionSetItemResp,
} from 'services/map/types';
import { axiosDataServiceInstance } from '../axiosInstances';

let _cancelRequest: Canceler | undefined;
export const cancelRequest = (): void => _cancelRequest?.();

export const getBookmarkTags = async (): Promise<
  ApiServiceResult<BookmarkTagsItem[]>
> => {
  return handleReq(async () => {
    const { results } = (
      await axiosDataServiceInstance.get('/BookmarkTags')
    ).data;

    return new ApiServiceResult({
      data: results,
    });
  });
};

export const getOptionsSet = async (
  objectId: string,
  id: string
): Promise<ApiServiceResult<OptionSetItemResp[]>> => {
  return handleReq(async () => {
    const response = (
      await axiosDataServiceInstance.get(`/optionsets/${objectId}/${id}`)
    ).data;

    return new ApiServiceResult({
      data: response,
    });
  });
};

export const bookmarksByParcelId = async (
  parcelId: string
): Promise<{ count: number; results: BookmarkItemResp[] }> =>
  (
    await axiosDataServiceInstance.get(`/Bookmarks/${parcelId}`, {
      cancelToken: new axios.CancelToken((c) => {
        _cancelRequest = c;
      }),
    })
  ).data;

export const bookmarksByMajorMinorParcel = async (
  major: string,
  minor: string
): Promise<{ count: number; results: BookmarkItemResp[] }> =>
  (
    await axiosDataServiceInstance.get(`/Bookmarks/${major}/${minor}`, {
      cancelToken: new axios.CancelToken((c) => {
        _cancelRequest = c;
      }),
    })
  ).data;

export const getTagIdsByBookmark = async (
  entityId: string,
  cancelHandler: React.MutableRefObject<Canceler>
): Promise<ApiServiceResult<unknown>> =>
  handleReq(async () => {
    const response = (
      await axiosDataServiceInstance.post(
        '/GenericDynamics/GetItems',
        {
          requests: [
            {
              entityName: 'ptas_bookmark',
              entityId: entityId,
            },
          ],
        },
        {
          cancelToken: new axios.CancelToken(function executor(c) {
            cancelHandler.current = c;
          }),
        }
      )
    ).data;

    return new ApiServiceResult({
      data: response,
    });
  });

export const upsertBookmark = async (
  bookmarkData: CreateUpdateBookmarkData
): Promise<ApiServiceResult<unknown>> =>
  handleReq(async () => {
    const {
      id,
      date,
      type,
      note,
      tagNames,
      tag1Id,
      tag2Id,
      tag3Id,
      tag4Id,
      tag5Id,
      parcelDetailId,
    } = bookmarkData;
    const data = {
      items: [
        {
          changes: {
            /* eslint-disable @typescript-eslint/camelcase */
            ptas_bookmarkdate: date,
            ptas_tags: tagNames,
            _ptas_tag1_value: tag1Id,
            _ptas_tag2_value: tag2Id,
            _ptas_tag3_value: tag3Id,
            _ptas_tag4_value: tag4Id,
            _ptas_tag5_value: tag5Id,
            ptas_bookmarktype: type ?? 0,
            ptas_bookmarkid: id,
            ptas_bookmarknote: note,
            _ptas_parceldetailid_value: parcelDetailId,
            // eslint-enable @typescript-eslint/camelcase
          },
          entityName: 'ptas_bookmark',
          entityId: id,
        },
      ],
    };
    const response = (
      await axiosDataServiceInstance.post('/GenericDynamics/UpdateItems', data)
    ).data;

    return new ApiServiceResult({
      data: response,
    });
  });

export const deleteBookmarkData = async (
  bookmarkId: string
): Promise<ApiServiceResult<unknown>> => {
  const data = {
    items: [
      {
        entityName: 'ptas_bookmark',
        entityId: bookmarkId,
      },
    ],
  };
  return handleReq(async () => {
    const response = (
      await axiosDataServiceInstance.delete('/GenericDynamics/DeleteItems', {
        data,
      })
    ).data;

    return new ApiServiceResult({
      data: response,
    });
  });
};
