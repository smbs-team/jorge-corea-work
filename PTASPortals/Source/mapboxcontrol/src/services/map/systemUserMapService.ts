/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import mapboxgl from 'mapbox-gl';
import { LayerSource, UserMap, UserMapCategory } from './model';
import { apiService, StoreItemValue } from '../api';
import {
  $onError,
  $onSelectedUserMapChange,
  $onSelectedSystemUserMapChange,
} from './mapServiceEvents';
import { ApiServiceResult } from 'services/common';
import { Subject } from 'rxjs';
import { sortBy } from 'lodash';

export interface CategoryData {
  id: number;
  name: string;
}

class SystemUserMapService {
  /**
   * Mapboxgl map
   */
  map: mapboxgl.Map | undefined;
  selectedUserMap: UserMap | undefined;
  selectedSystemUserMap: UserMap | undefined;
  selectedLayers: LayerSource['defaultMapboxLayer'][] = [];

  /**
   * Subject that indicates when the system renderer categories changed
   */
  $onRendererCategoriesChange = new Subject<{
    categories: UserMapCategory[] | undefined;
    refreshUserMapsForCategories: boolean;
  }>();

  /**
   * Subject that indicates when the editing system user map changes
   */
  $onEditingSystemUserMapChange = new Subject<UserMap | undefined>();

  /**
   * The main method to be called
   * @param _map - Mapboxgl map
   */
  init = async (_map: mapboxgl.Map): Promise<void> => {
    this.map = _map;

    const selectedSystemUserMapIdRes = await apiService.metaStore.getUserStoreItem<
      StoreItemValue['selected-system-user-map']
    >('selected-system-user-map');
    if (selectedSystemUserMapIdRes.hasError) {
      $onError.next('Error getting selected system renderer id');
    }
    const selectedSystemUserMapId =
      selectedSystemUserMapIdRes.data?.value.usermap ?? 0;

    if (selectedSystemUserMapId) {
      const selectedSystemUserMapRes:
        | UserMap
        | undefined = await apiService.getUserMap(selectedSystemUserMapId);
      if (selectedSystemUserMapRes) {
        this.selectedSystemUserMap = selectedSystemUserMapRes;
        $onSelectedSystemUserMapChange.next(selectedSystemUserMapRes);
      }
    }

    $onSelectedUserMapChange.subscribe((userMap) => {
      this.selectedUserMap = userMap;
    });
  };

  loadCategories = async (
    refreshUserMapsForCategories = false
  ): Promise<void> => {
    const r = await apiService.getUserMapCategories();
    if (r.hasError) {
      $onError.next(r.errorMessage);
      return;
    }
    if (!r.data) return;
    const sortedCategories = sortBy(r.data, [
      (r): string => r.categoryName.toLowerCase(),
    ]);
    this.$onRendererCategoriesChange.next({
      categories: sortedCategories,
      refreshUserMapsForCategories: refreshUserMapsForCategories,
    });
  };

  applySystemUserMap = async (
    systemUserMapId?: number,
    updateEditingSystemMap?: boolean
  ): Promise<void> => {
    if (!this.map) return;
    if (systemUserMapId === undefined) {
      this.selectedSystemUserMap = undefined;
      $onSelectedSystemUserMapChange.next(undefined);
      this.saveSelectedSystemUserMap(undefined);
    } else if (systemUserMapId === 0) {
      //Case for creating new system user map (empty map)
      this.selectedSystemUserMap = undefined;
    } else {
      //Apply system user map
      const systemUserMapRes = await apiService.getUserMap(systemUserMapId);
      if (systemUserMapRes) {
        this.selectedSystemUserMap = systemUserMapRes;
        $onSelectedSystemUserMapChange.next(systemUserMapRes);
        this.saveSelectedSystemUserMap(systemUserMapId);
        if (updateEditingSystemMap) {
          this.$onEditingSystemUserMapChange.next(systemUserMapRes);
        }
      }
    }
  };

  setCategories = async (
    systemUserMapId: number,
    categories: CategoryData[]
  ): Promise<void> => {
    if (categories.length) {
      const _categories: UserMapCategory[] = categories.map((cat) => {
        return {
          userMapCategoryId: cat.id,
          categoryName: cat.name,
          categoryDescription: '',
          userMaps: [],
        };
      });

      const res = await apiService.setUserMapCategories(
        systemUserMapId,
        _categories
      );
      if (res.hasError) {
        $onError.next('Error setting system renderer categories');
      }
    }
  };

  saveSelectedSystemUserMap = async (
    systemUserMapId?: number
  ): Promise<void> => {
    const r = await apiService.metaStore.saveUserStoreItem({
      storeType: 'selected-system-user-map',
      value: {
        usermap: systemUserMapId,
      },
    });
    if (r.hasError) {
      $onError.next('Error updating selected system user map id');
    }
  };

  deleteSystemUserMap = async (
    userMapId: number
  ): Promise<ApiServiceResult<unknown>> => {
    const response = await apiService.deleteUserMap(userMapId);
    if (response.hasError) {
      $onError.next('Error on delete system renderer');
      console.error(response.errorMessage);
    } else {
      if (userMapId === this.selectedSystemUserMap?.userMapId) {
        this.applySystemUserMap(undefined);
      }
    }
    return response;
  };
}

export const systemUserMapService = new SystemUserMapService();
