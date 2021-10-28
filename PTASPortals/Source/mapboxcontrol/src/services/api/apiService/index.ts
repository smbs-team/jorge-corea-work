// apiService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  UserMap,
  RendererCategory,
  RendererDataset,
  RendererDataSetColumn,
  ApiColumnType,
  UserMapCategory,
  MapsUserDetails,
} from 'services/map/model';
import { ApiServiceResult, handleReq } from 'services/common/httpClient';
import { utilService } from 'services/common';
import {
  CreateFolderReq,
  Folder,
  FolderItemType,
} from '../../map/model/folder';
import { ParcelMediaInfo } from '../../map/model/parcelMediaInfo';
import { round, uniqBy } from 'lodash';
import { UserInfo } from '../../user/userInfo';
import { tokenService } from 'services/common/tokenService';
import {
  AddressByLocationItem,
  AddressItem,
  FolderData,
  GetAddressByLocation,
  GetAddressSuggestions,
} from '../../map/types';
import { Subject } from 'rxjs';
import { RenameFolderRequest } from '../../map/types';
import { OverlapCalculatorRes } from '../../map/model/overlapCalculator';
import { ExcelSheet } from 'components/Tools/Calculate/common';
import axios, { Canceler } from 'axios';
import { userMapReqPipe } from '../dataPipes';
import { AnnotationLabelService } from 'services/map/annotationLabelService/annotationLabelService';
import { FillPaint } from 'mapbox-gl';
import {
  MAPBOX_SEARCH_API_PARAMS,
  RGBA_BLACK,
  RGBA_TRANSPARENT,
} from 'appConstants';
import {
  axiosCsInstance,
  axiosInstance,
  axiosMapTileInstance,
} from '../axiosInstances';
import * as metaStore from './metaStore';
import * as bookmark from './bookmark';
import { GetLayerDownloadUrl } from './types';
import { flatFolders } from 'utils/userMapFolder';
import { Key } from 'react';
import { LayerSources } from './layerSources';
export * from './types';

class ApiService {
  $onB2CTokenChanged = new Subject<string>();
  b2cToken = '';
  CancelToken = axios.CancelToken;
  constructor() {
    tokenService.$onB2CTokenInit.subscribe((token) => {
      this.b2cToken = token;
      this.$onB2CTokenChanged.next(this.b2cToken);
    });
  }

  metaStore = metaStore;
  bookmark = bookmark;
  layerSources = new LayerSources();

  /**
   * Gets one map for user
   * @param mapId -The map id
   */
  getUserMap = async (mapId: string | number): Promise<UserMap | undefined> => {
    const url = `/GIS/GetUserMap/${mapId}`;
    const userMap = (
      await axiosCsInstance.get<UserMap>(url, {
        transformResponse: (r) => {
          const res: { userMap: UserMap } = JSON.parse(r);
          for (const renderer of res.userMap.mapRenderers) {
            if (!renderer.rendererRules.nativeMapboxLayers) {
              renderer.rendererRules.nativeMapboxLayers = [];
            }
            if (
              AnnotationLabelService.instance?.isAnnoLayer(
                renderer.rendererRules.layer.id
              )
            ) {
              const showShape =
                process.env.REACT_APP_SHOW_ANNOTATIONS_SHAPE === 'true';
              (renderer.rendererRules.layer.paint as FillPaint)[
                'fill-outline-color'
              ] = showShape ? RGBA_BLACK : RGBA_TRANSPARENT;
            }
          }
          return res.userMap;
        },
      })
    ).data;
    return new UserMap(userMap);
  };

  /**
   * User maps
   * @param username - The user that maps belongs
   */
  getUserMaps = async (username: string): Promise<MapsUserDetails> => {
    const url = `/GIS/GetUserMapsForUser/${username}`;
    const response = (await axiosCsInstance.get<MapsUserDetails>(url)).data;
    return {
      userMaps: response.userMaps.map((item) => new UserMap(item)),
      usersDetails: response.usersDetails,
    };
  };

  /**
   * Get user map categories
   */
  getUserMapCategories = (): Promise<ApiServiceResult<UserMapCategory[]>> =>
    handleReq(async () => {
      const url = `/GIS/GetUserMapCategories`;
      const { userMapCategories } = (
        await axiosCsInstance.get<{
          userMapCategories: UserMapCategory[];
        }>(url)
      ).data;
      return new ApiServiceResult({
        data: userMapCategories.map(
          (category) => new UserMapCategory(category)
        ),
      });
    });

  /**
   * Set categories of a user map
   * @param userMapId -The user map to be created
   */
  setUserMapCategories = async (
    userMapId: number,
    userMapCategories: UserMapCategory[]
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      UserMapCategories: userMapCategories.map((cat) => ({
        UserMapCategoryId: cat.userMapCategoryId,
        CategoryName: cat.categoryName,
        CategoryDescription: cat.categoryDescription,
      })),
    };
    return handleReq(async () => {
      const url = `/GIS/SetUserMapCategories/${userMapId}`;
      const result = (await axiosCsInstance.post(url, data)).data;
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  /**
   * Get user maps by category
   */
  getUserMapsByCategory = (
    categoryId: number
  ): Promise<ApiServiceResult<UserMap[]>> =>
    handleReq(async () => {
      const url = `/GIS/GetUserMapsByCategory/${categoryId}`;
      const { userMaps } = (
        await axiosCsInstance.get<{
          userMaps: UserMap[];
        }>(url, {
          transformResponse: (res) => {
            const json = JSON.parse(res);
            if (!json.userMaps) {
              json.userMaps = [];
            }
            return json;
          },
        })
      ).data;
      return new ApiServiceResult({
        data: userMaps.map((userMap) => new UserMap(userMap)),
      });
    });

  /**
   * Create user map
   * @param userMap -The user map to be created
   */
  createUserMap = async (userMap: UserMap): Promise<number> => {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const data: any = {
      ...userMap,
      createdTimestamp: utilService.unixTimeToDate(userMap.createdTimestamp),
      lastModifiedTimestamp: utilService.unixTimeToDate(
        userMap.lastModifiedTimestamp
      ),
    };
    const url = `/GIS/CreateUserMap`;
    return (await axiosCsInstance.post<number>(url, userMapReqPipe(data))).data;
  };

  /**
   * Update user map
   * @param userMap -The user map to be updated
   */
  updateUserMap = async (
    userMapId: UserMap['userMapId'],
    updatedFields: Partial<Omit<UserMap, 'userMapId'>>
  ): Promise<ApiServiceResult<UserMap>> =>
    handleReq(async () => {
      const url = `/GIS/UpdateUserMap`;
      const userMap = await apiService.getUserMap(userMapId);
      if (!userMap)
        return new ApiServiceResult({
          errorMessage: 'Error getting user map ' + userMapId,
        });

      const newMap: UserMap = userMapReqPipe({
        ...userMap,
        ...updatedFields,
      }) as UserMap;

      await axiosCsInstance.post(url, newMap, {
        headers: {
          Authorization: 'Bearer ' + this.b2cToken,
        },
      });

      return new ApiServiceResult({
        data: newMap,
      });
    });

  deleteUserMap = async (
    userMapId: number
  ): Promise<ApiServiceResult<unknown>> =>
    handleReq(async () => {
      const url = `/GIS/DeleteUserMap/` + userMapId;
      const result = (await axiosCsInstance.post(url, {})).data;

      return new ApiServiceResult({
        data: result,
      });
    });

  /**
   * Sets selected renderer for user
   * @param rendererId -The map id
   */
  setUserRendererSelection = async (
    rendererId: number
  ): Promise<ApiServiceResult<unknown>> => {
    return handleReq(async () => {
      const url = `/GIS/SetMapRendererSelection/${rendererId}`;
      const result = (await axiosCsInstance.post(url, {})).data;

      return new ApiServiceResult({
        data: result,
      });
    });
  };

  /**
   * Get renderer categories
   */
  getRendererCategories = (): Promise<ApiServiceResult<RendererCategory[]>> => {
    return handleReq(async () => {
      const url = `/GIS/GetMapRendererCategories`;
      const { mapRendererCategories } = (
        await axiosCsInstance.get<{
          mapRendererCategories: RendererCategory[];
        }>(url)
      ).data;

      return new ApiServiceResult({
        data: mapRendererCategories.map(
          (category) => new RendererCategory(category)
        ),
      });
    });
  };

  /**
   * Get folders for user
   */
  getUserFolders = async (
    userId: string,
    folderItemType: FolderItemType
  ): Promise<Folder[]> => {
    const url = `/GIS/GetFoldersForUser/${userId}/${folderItemType}`;
    const { folders } = (
      await axiosCsInstance.get<{
        folders: Folder[];
      }>(url)
    ).data;

    const defFolders = [
      {
        folderId: -2,
        parentFolderId: null,
        folderName: 'User',
        children: null,
      },
      {
        folderId: -1,
        parentFolderId: null,
        folderName: 'Shared',
        children: null,
      },
      {
        folderId: 0,
        parentFolderId: null,
        folderName: 'System',
        children: null,
      },
    ];

    return uniqBy(
      [...flatFolders(folders), ...defFolders],
      (folder) => folder.folderName
    );
  };

  /**
   * Create folder
   * @param folder -The folder to be created
   */
  createFolder = async (
    folder: CreateFolderReq
  ): Promise<ApiServiceResult<number>> => {
    return handleReq(async () => {
      const url = `/GIS/CreateFolder`;
      const result = (await axiosCsInstance.post<number>(url, folder)).data;

      return new ApiServiceResult<number>({
        data: result,
      });
    });
  };

  /**
   * Rename GIS Folder
   * @param request - New folder request
   */
  renameFolder = async (
    request: RenameFolderRequest
  ): Promise<ApiServiceResult<unknown>> => {
    return handleReq(async () => {
      const url = `/GIS/RenameGISFolder`;
      const result = (await axiosCsInstance.post(url, request)).data;

      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  /**
   * Renderer Datasets for user
   * @param userId - The user id that the renderers belongs
   */
  getUserRendererDataSets = async (
    userId: string
  ): Promise<RendererDataset[]> => {
    const url = `/CustomSearches/GetDataSetsForUser/${userId}`;
    const { datasets } = (
      await axiosCsInstance.get<{
        datasets: RendererDataset[];
      }>(url)
    ).data;
    return datasets
      .map((dataset) => new RendererDataset(dataset))
      .sort((a, b) =>
        a.datasetName.toLowerCase() < b.datasetName.toLowerCase() ? -1 : 1
      );
  };
  /**
   * Get a specific Renderer Datasets
   * @param datasetId - The dataset id
   */
  getRendererDataSet = (
    datasetId: string
  ): Promise<ApiServiceResult<RendererDataset | undefined>> =>
    handleReq(async () => {
      const url = `/CustomSearches/GetDataSet/${datasetId}`;
      const { dataset } = (
        await axiosCsInstance.get<{
          dataset: RendererDataset;
        }>(url, {
          params: {
            cache: true,
          },
        })
      ).data;
      return new ApiServiceResult({
        data: new RendererDataset(dataset),
      });
    });

  /**
   * Dataset Columns
   * @param datasetId - The user id that the renderers belongs
   */
  getDataSetColumns = async (
    datasetId: string
  ): Promise<RendererDataSetColumn[]> => {
    const url = `/CustomSearches/GetDatasetColumns/${datasetId}`;
    const { datasetColumns } = (
      await axiosCsInstance.get<{
        datasetColumns: RendererDataSetColumn[];
      }>(url, {
        timeout: 90000,
        params: {
          includeDependencies: true,
          cache: true,
          'is-static': true,
        },
      })
    ).data;

    return datasetColumns
      .map(
        (dataset) =>
          new RendererDataSetColumn(
            { ...dataset, columnType: dataset.columnType as ApiColumnType },
            datasetId
          )
      )
      .sort((a, b) =>
        a.columnName.toLowerCase() < b.columnName.toLowerCase() ? -1 : 1
      );
  };

  /**
   * Dataset Column Values
   * @param datasetId - The dataset id that the column belongs to
   * @param columnName - Name of column whose values are to be fetched
   * @param lookupValue - Whether the column can be used as lookup
   */
  getDataSetLookUpColumnValues = async (
    datasetId: string,
    columnName: string
  ): Promise<Key[]> => {
    const url = `/CustomSearches/GetDatasetColumnLookupValues/${datasetId}/${columnName}`;
    const { results } = (
      await axiosCsInstance.get<{
        results: Key[];
      }>(url, {
        params: {
          cache: true,
        },
      })
    ).data;

    return results.sort((a, b) => (a < b ? -1 : 1));
  };
  /**
   * Dataset Column Values
   * @param datasetId - The user id that the dataset belongs to
   * @param columnName - Name of column whose values are to be fetched
   * @param lookupValue - Whether the column can be used as lookup
   */
  getRangeValues = (
    datasetId: string,
    columnName: string
  ): Promise<ApiServiceResult<[number, number]>> =>
    handleReq(async () => {
      const { results } = (
        await axiosCsInstance.get<{
          results: [number, number];
        }>(
          `/CustomSearches/GetDatasetColumnRangeValues/${datasetId}/${columnName}`,
          {
            timeout: 90 * 1000,
            params: {
              cache: true,
            },
          }
        )
      ).data;

      return new ApiServiceResult({
        data: results,
      });
    });

  getQuantileBreaks = (
    datasetId: string,
    columnName: string,
    numberOfBreaks: number
  ): Promise<ApiServiceResult<number[]>> =>
    handleReq(async () => {
      const { results } = (
        await axiosCsInstance.post<{
          results: number[];
        }>(
          `/CustomSearches/GetDatasetColumnBreaksQuantile/${datasetId}/${columnName}`,
          {
            ClassBreakCount: numberOfBreaks,
            FilterEmptyValuesExpression: {
              ExpressionRole: 'FilterExpression',
              Script: `${columnName} IS NOT NULL`,
            },
          }
        )
      ).data;

      return new ApiServiceResult({
        data: results,
      });
    });

  getStDeviation = (
    datasetId: string,
    columnName: string,
    interval: number
  ): Promise<ApiServiceResult<number[]>> =>
    handleReq(async () => {
      const { results } = (
        await axiosCsInstance.post<{
          results: number[];
        }>(
          `/CustomSearches/GetDatasetColumnBreaksStdDeviation/${datasetId}/${columnName}`,
          {
            Interval: interval,
            FilterEmptyValuesExpression: {
              ExpressionRole: 'FilterExpression',
              Script: `${columnName} IS NOT NULL AND ${columnName} > 0`,
            },
          }
        )
      ).data;

      return new ApiServiceResult({
        data: results.map((i) => round(i, 2)),
      });
    });

  /**
   * Get valid File Storage SAS token to read media files.
   * @returns SAS Token
   */
  getMediaToken = (): Promise<ApiServiceResult<string>> =>
    handleReq(async () => {
      const r = (
        await axiosInstance.get<string>(
          `${process.env.REACT_APP_MEDIA_API}/GetSAS`,
          {
            headers: { Authorization: 'Bearer ' + this.b2cToken },
          }
        )
      ).data;

      return new ApiServiceResult({
        data: r,
      });
    });

  /**
   * Gets a collection of media images associated with an specific parcel.
   * @param parcelPIN - the parcel ping number.
   * @returns A Collection of ParcelMediaInfo objects.
   */

  parcelMedia: {
    get: (
      parcelPIN: string,
      mediaToken: string | undefined
    ) => Promise<string[]>;
    cancel: Canceler | undefined;
  } = {
    cancel: undefined,
    get: async (
      parcelPIN: string,
      mediaToken: string | undefined
    ): Promise<string[]> => {
      this.parcelMedia.cancel?.();
      // Clean parameter, if parcel # is missing the separator, include it.
      const parcelNumber = !parcelPIN.includes('-')
        ? `${parcelPIN.substring(0, 6)}-${parcelPIN.substring(6)}`
        : parcelPIN;
      const results = (
        await axiosCsInstance.get<ParcelMediaInfo[]>(
          `/GIS/GetParcelMediaInfo/${parcelNumber}`,
          {
            cancelToken: new axios.CancelToken((c) => {
              this.parcelMedia.cancel = c;
            }),
          }
        )
      ).data;

      const createUrl = (item: ParcelMediaInfo): string => {
        const url = new URL(
          `${process.env.REACT_APP_MEDIA_BLOB_URL}${item.MediaPath}${mediaToken}`
        );
        return url.href;
      };

      return results.map(createUrl);
    },
  };

  /**
   * Gets the logged user information.
   */
  getCurrentUserInfo = (): Promise<ApiServiceResult<UserInfo>> =>
    handleReq(async () => {
      const r = new ApiServiceResult({
        data: (await axiosCsInstance.get<UserInfo>('/Auth/GetCurrentUserInfo'))
          .data,
      });
      return r;
    });

  /**
   * Get address by location
   *
   * @param lng - Longitude
   * @param lat - Latitude
   */
  getAddressByLocation = async (
    lng: number,
    lat: number
  ): Promise<ApiServiceResult<AddressByLocationItem>> =>
    handleReq(async () => {
      const results = (
        await axiosInstance.get<GetAddressByLocation>(
          `${process.env.REACT_APP_MAPBOX_SEARCH_API_URL}${lng},${lat}.json`,
          {
            params: MAPBOX_SEARCH_API_PARAMS,
          }
        )
      ).data;

      const featureFound = results?.features[0];
      return new ApiServiceResult({
        data: {
          lng: featureFound?.center[0],
          lat: featureFound?.center[1],
          addressFull: featureFound?.place_name ?? '',
        },
      });
    });

  getAddressSuggestions = async (
    address: string
  ): Promise<ApiServiceResult<AddressItem[]>> =>
    handleReq(async () => {
      const results = (
        await axiosInstance.get<GetAddressSuggestions>(
          `${process.env.REACT_APP_MAPBOX_SEARCH_API_URL}${address}.json`,
          {
            params: MAPBOX_SEARCH_API_PARAMS,
          }
        )
      ).data;

      const featureFound = results?.features ?? [];
      const data = featureFound.map((ff) => {
        return {
          lng: ff?.center[0],
          lat: ff?.center[1],
          addressFull: ff?.place_name ?? '',
        };
      });
      return new ApiServiceResult({ data });
    });

  deleteFolder = async (
    folderData: FolderData
  ): Promise<ApiServiceResult<unknown>> =>
    handleReq(async () => {
      const url = `/GIS/DeleteFolder/`;
      const result = (await axiosCsInstance.post(url, folderData)).data;
      return new ApiServiceResult({
        data: result,
      });
    });

  overlapCalculation = async (
    layerId: string | number,
    buffer: number,
    pins: string[]
  ): Promise<OverlapCalculatorRes[]> => {
    const url = `/overlapCalculation/${layerId}`;
    return (
      await axiosMapTileInstance.post<OverlapCalculatorRes[]>(url, {
        buffer,
        pins,
      })
    ).data;
  };

  convertFromJsonToExcel = async (json: ExcelSheet): Promise<Blob> => {
    return (
      await axiosCsInstance.post(`/Shared/ConvertFromJsonToExcel`, json, {
        responseType: 'blob',
      })
    ).data;
  };

  getLayerDownloadUrl = async (
    layerId: number
  ): Promise<ApiServiceResult<GetLayerDownloadUrl>> => {
    const url = `/GIS/GetGisLayerDownloadUrl/${layerId}`;
    const { data } = await axiosCsInstance.get<GetLayerDownloadUrl>(url);
    return new ApiServiceResult<GetLayerDownloadUrl>({
      data,
    });
  };
}

export const apiService = new ApiService();
