/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Save, SaveAcceptEvent, SnackContext } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import React, { Key, useContext, useEffect, useState } from 'react';
import { UserMap } from 'services/map';
import {
  CategoryData,
  systemUserMapService,
} from 'services/map/systemUserMapService';

type Props = Pick<UserMap, 'userMapId' | 'userMapName'> & {
  onClose?: () => void;
};

function SetCategories({
  userMapId,
  userMapName,
  onClose,
}: Props): JSX.Element {
  const { setSnackState } = useContext(SnackContext);
  const {
    categoriesData,
    createNewCategory,
    rendererCategories,
    setLinearProgress,
  } = useContext(HomeContext);
  const [selectedCategories, setSelectedCategories] = useState<CategoryData[]>(
    []
  );

  useEffect(() => {
    setSelectedCategories(
      rendererCategories.flatMap((cat) =>
        cat.userMaps?.find((userMap) => userMap.userMapId === userMapId)
          ? {
              id: cat.userMapCategoryId,
              name: cat.categoryName,
            }
          : []
      )
    );
  }, [rendererCategories, userMapId]);

  const onCategoryClick = (catId: Key, selected: boolean): void => {
    if (typeof catId !== 'number') {
      return setSnackState({
        severity: 'error',
        text: 'Invalid category id ' + catId,
      });
    }
    if (selected) {
      setSelectedCategories((prev) => [
        ...prev,
        {
          id: catId,
          name: categoriesData.find((cat) => cat.Value === catId)?.Key || '',
        },
      ]);
    } else {
      setSelectedCategories((prev) => prev.filter((item) => item.id !== catId));
    }
  };

  const onSaveClick = async (_evt: SaveAcceptEvent): Promise<void> => {
    try {
      setLinearProgress(true);
      await systemUserMapService.setCategories(userMapId, selectedCategories);
      await systemUserMapService.loadCategories(true);
    } catch {
      setSnackState({
        severity: 'error',
        text: 'An error ocurred saving or reloading categories',
      });
    } finally {
      setLinearProgress(false);
      onClose?.();
    }
  };

  return (
    <Save
      isNewSave
      useCategories={true}
      categoriesData={categoriesData}
      selectedCategories={selectedCategories.map((c) => c.id)}
      onCategoryClick={onCategoryClick}
      title={'Categories for renderer ' + userMapName}
      okClick={onSaveClick}
      buttonText="Save"
      popoverTitle={'New Category'}
      newCategoryOkClick={(event): void => {
        createNewCategory(event);
        setSelectedCategories((prev) => [
          ...prev,
          {
            id:
              Math.max(
                ...categoriesData.map((val) =>
                  typeof val.Value === 'number' ? val.Value : 0
                )
              ) + 1,
            name: event.categoryName ?? '',
          },
        ]);
      }}
      dropdownRows={[]}
      disableRename
    />
  );
}

export default SetCategories;
