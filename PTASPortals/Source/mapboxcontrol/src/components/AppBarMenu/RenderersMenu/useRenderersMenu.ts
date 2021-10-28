/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useContext, useEffect } from 'react';
import { apiService } from 'services/api';
import { HomeContext } from 'contexts';
import { UserMapCategory } from 'services/map';
import { $onError } from 'services/map/mapServiceEvents';
import { systemUserMapService } from 'services/map/systemUserMapService';

interface UseRendererMenu {
  loadUserMapsByCategory: (category: UserMapCategory) => void;
}

export const useRenderersMenu = (): UseRendererMenu => {
  const { setRendererCategories } = useContext(HomeContext);

  const loadUserMapsByCategory = async (
    category: UserMapCategory
  ): Promise<void> => {
    const userMapsRes = await apiService.getUserMapsByCategory(
      category.userMapCategoryId
    );
    if (userMapsRes.hasError) {
      $onError.next(
        'Error getting user maps for category ' + category.categoryName
      );
      return;
    }
    const userMaps = userMapsRes.data || [];
    setRendererCategories((prev) => {
      const categories = [...prev];
      categories.forEach((cat) => {
        if (cat.userMapCategoryId === category.userMapCategoryId) {
          cat.userMaps = userMaps;
        }
      });
      return categories;
    });
  };

  useEffect(() => {
    systemUserMapService.loadCategories();
  }, []);

  return {
    loadUserMapsByCategory,
  };
};
