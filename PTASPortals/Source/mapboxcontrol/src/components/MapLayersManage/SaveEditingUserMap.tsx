/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  CustomPopover,
  ErrorMessageAlertCtx,
  Save,
  SaveAcceptEvent,
} from '@ptas/react-ui-library';
import { SYSTEM_RENDERER_FOLDER } from 'appConstants';
import { HomeContext } from 'contexts';
import { useCreateFolder } from 'hooks/api';
import React from 'react';
import { useContext } from 'react';
import { mapService, UserMap } from 'services/map';
import userService from 'services/user/userService';
import { getErrorStr } from 'utils/getErrorStr';
import { isInSystem } from 'utils/userMap';
import { MapLayersManageContext } from './Context';

type Props = {
  savedMapRef: React.MutableRefObject<UserMap | undefined>;
};

function SaveEditingUserMap({ savedMapRef }: Props): JSX.Element | null {
  const {
    saveEditingUserMap,
    setSelectedSystemUserMap,
    selectedCategories,
    setSelectedUserMap,
    categoriesData,
    onCategoryClick,
    mapsTreeView,
    createNewCategory,
    setLinearProgress,
    editingUserMap,
  } = useContext(HomeContext);
  const { setSavePopoverAnchor, savePopoverAnchor } = useContext(
    MapLayersManageContext
  );
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const { rows } = mapsTreeView;
  const createFolder = useCreateFolder();

  if (!editingUserMap) return null;

  const saveOkClick = async (evt: SaveAcceptEvent): Promise<void> => {
    try {
      setLinearProgress(true);
      setSavePopoverAnchor(null);
      if (isInSystem(editingUserMap)) {
        evt.route = SYSTEM_RENDERER_FOLDER.PATH;
        setSelectedSystemUserMap(new UserMap(editingUserMap));
        savedMapRef.current = new UserMap(editingUserMap);
        // Cannot update map categories here, because editingUserMap
        // might be out of date at this point, so it's being done
        // inside saveEditingUserMap
        await saveEditingUserMap(evt, true, selectedCategories);
      } else {
        setSelectedUserMap(new UserMap(editingUserMap));
        savedMapRef.current = new UserMap(editingUserMap);
        await saveEditingUserMap(evt);
      }
    } catch (e) {
      showErrorMessage(getErrorStr(e));
    } finally {
      setLinearProgress(false);
    }
  };

  return (
    <CustomPopover
      anchorEl={savePopoverAnchor}
      onClose={(): void => {
        setSavePopoverAnchor(null);
      }}
      border
      showCloseButton
    >
      <Save
        useCategories={isInSystem(editingUserMap)}
        categoriesData={categoriesData}
        selectedCategories={selectedCategories.map((cat) => cat.id)}
        onCategoryClick={onCategoryClick}
        isNewSave
        title={
          isInSystem(editingUserMap) ? 'Save system renderer' : 'Save user map'
        }
        okClick={(e: SaveAcceptEvent): void => {
          saveOkClick(e);
        }}
        buttonText="Save"
        popoverTitle={
          isInSystem(editingUserMap) ? 'New Category' : 'New Folder'
        }
        checkboxText="Lock"
        dropdownRows={rows.filter(
          (r) =>
            r.folder &&
            (userService.isAdminUser() ||
              r.title !== SYSTEM_RENDERER_FOLDER.NAME)
        )}
        newFolderDropdownRows={rows.filter((i) => i.folder)}
        newFolderOkClick={(e): void => {
          createFolder(e.route + e.folderName);
        }}
        newCategoryOkClick={createNewCategory}
        defaultRoute={
          mapService.isNewMap(editingUserMap)
            ? '/User'
            : editingUserMap.folderPath
        }
        defaultName={editingUserMap.userMapName}
        defaultChecked={editingUserMap.isLocked}
      />
    </CustomPopover>
  );
}

export default SaveEditingUserMap;
