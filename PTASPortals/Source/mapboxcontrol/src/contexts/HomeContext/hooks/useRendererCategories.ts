/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { NewCategoryAcceptEvt } from '@ptas/react-ui-library';
import { ListItem } from '@ptas/react-ui-library/dist/ListSearch';
import { Dispatch, SetStateAction, useEffect, useState, Key } from 'react';
import { Subscription } from 'rxjs';
import { apiService } from 'services/api';
import { UserMap, UserMapCategory } from 'services/map';
import { $onError } from 'services/map/mapServiceEvents';
import {
  CategoryData,
  systemUserMapService,
} from 'services/map/systemUserMapService';

export type UseRendererCategories = {
  rendererCategories: UserMapCategory[];
  setRendererCategories: Dispatch<SetStateAction<UserMapCategory[]>>;
  categoriesData: ListItem[];
  setCategoriesData: Dispatch<SetStateAction<ListItem[]>>;
  selectedCategories: CategoryData[];
  setSelectedCategories: Dispatch<SetStateAction<CategoryData[]>>;
  loadUserMapsForAllCategories: (categories?: UserMapCategory[]) => void;
  onCategoryClick: (selectedItem: Key, isSelected: boolean) => void;
  createNewCategory: (data: NewCategoryAcceptEvt) => void;
  systemUserMaps: UserMap[];
};

export const useRendererCategories = (
  editingUserMap: UserMap | undefined
): UseRendererCategories => {
  //This state actually works with user map categories, but it's only used with system renderers
  const [rendererCategories, setRendererCategories] = useState<
    UserMapCategory[]
  >([]);
  const [categoriesData, setCategoriesData] = useState<ListItem[]>([]);
  const [selectedCategories, setSelectedCategories] = useState<CategoryData[]>(
    []
  );
  const [systemUserMaps, setSystemUserMaps] = useState<UserMap[]>([]);

  useEffect(() => {
    const subs: Subscription[] = [];
    subs.push(
      systemUserMapService.$onRendererCategoriesChange.subscribe((data) => {
        if (data.refreshUserMapsForCategories) {
          loadUserMapsForAllCategories(data.categories);
        } else {
          setRendererCategories(data.categories ?? []);
        }
      })
    );
    return (): void => {
      for (const sub of subs) {
        sub.unsubscribe();
      }
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    setCategoriesData(
      rendererCategories.map((cat) => ({
        Key: cat.categoryName,
        Value: cat.userMapCategoryId,
      }))
    );

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [rendererCategories]);

  useEffect(() => {
    if (rendererCategories.length) {
      //Search categories that contain the current editing system user map
      if (!editingUserMap) return;
      const _selectedCategories: CategoryData[] = [];
      rendererCategories.forEach((cat) => {
        if (
          cat.userMaps &&
          cat.userMaps.length &&
          cat.userMaps
            .map((map) => map.userMapId)
            .includes(editingUserMap.userMapId)
        ) {
          _selectedCategories.push({
            id: cat.userMapCategoryId,
            name: cat.categoryName,
          });
        }
      });
      setSelectedCategories(_selectedCategories);
    }
  }, [rendererCategories, editingUserMap]);

  const loadUserMapsForAllCategories = async (
    categories?: UserMapCategory[]
  ): Promise<void> => {
    const _categories = categories ?? [...rendererCategories];
    for (const cat of _categories) {
      if (!cat.userMaps) {
        const userMapsRes = await apiService.getUserMapsByCategory(
          cat.userMapCategoryId
        );
        if (userMapsRes.hasError) {
          $onError.next(
            'Error getting user maps for category ' + cat.categoryName
          );
          return;
        }

        cat.userMaps = userMapsRes.data || [];
      }
    }

    setRendererCategories(_categories);
  };

  const onCategoryClick = (selectedItem: Key, isSelected: boolean): void => {
    if (isSelected) {
      setSelectedCategories((prev) => {
        const newCategories = [...prev];
        newCategories.push({
          id: selectedItem as number,
          name:
            categoriesData.find((cat) => cat.Value === selectedItem)?.Key || '',
        });
        return newCategories;
      });
    } else {
      setSelectedCategories((prev) => {
        const newCategories = [...prev];
        return newCategories.filter(
          (cat) => cat.id !== (selectedItem as number)
        );
      });
    }
  };

  const createNewCategory = async (
    data: NewCategoryAcceptEvt
  ): Promise<void> => {
    if (!data.categoryName) return;
    if (categoriesData.find((cat) => cat.Key === data.categoryName)) {
      console.warn('Already exists a category with name:', data.categoryName);
      return;
    }

    setCategoriesData((prev) => {
      const newId = prev?.length
        ? Math.max(...prev.map((cat) => cat.Value as number)) + 1
        : 0;
      const newCategoriesData = [
        ...(prev || []),
        {
          Value: newId,
          Key: data.categoryName || '',
        },
      ];
      return newCategoriesData;
    });
  };

  useEffect(() => {
    setSystemUserMaps(rendererCategories.flatMap((val) => val.userMaps ?? []));
  }, [rendererCategories]);

  return {
    rendererCategories,
    setRendererCategories,
    categoriesData,
    setCategoriesData,
    selectedCategories,
    setSelectedCategories,
    loadUserMapsForAllCategories,
    onCategoryClick,
    createNewCategory,
    systemUserMaps,
  };
};
