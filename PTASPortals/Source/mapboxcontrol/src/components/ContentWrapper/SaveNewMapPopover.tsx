/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { createStyles, makeStyles, Theme } from '@material-ui/core';
import { Save, SaveAcceptEvent, YesNoBox } from '@ptas/react-ui-library';
import { SYSTEM_RENDERER_FOLDER } from 'appConstants';
import { HomeContext } from 'contexts';
import { useCreateFolder } from 'hooks/api';
import React, { Fragment, useContext, useRef } from 'react';
import { mapService, UserMap } from 'services/map';
import { systemUserMapService } from 'services/map/systemUserMapService';
import { Popover } from 'components/common';
import { isInSystem } from 'utils/userMap';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    popoverRoot: {
      //Old style
      // top: '100px',
      // left: '40%',
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
    },
  })
);

const SaveMapPopovers = (): JSX.Element => {
  const classes = useStyles();
  const homeContext = useContext(HomeContext);
  const {
    setSaveNewMapPopoverAnchor,
    setSaveNewRendererPopoverAnchor,
    editingUserMap,
    selectedUserMap,
    selectedSystemUserMap,
    setSelectedUserMap,
    setSelectedSystemUserMap,
    saveEditingUserMap,
    categoriesData,
    selectedCategories,
    onCategoryClick,
    createNewCategory,
    setSavePendingChanges,
    savePendingChanges,
  } = homeContext;
  const createFolder = useCreateFolder();
  const { rows } = homeContext.mapsTreeView;
  const popoverRef = useRef<HTMLDivElement>();
  const isSystemUserMap = editingUserMap?.folderPath.startsWith(
    SYSTEM_RENDERER_FOLDER.PATH
  );

  const cleanStateSaveMap = (): void => {
    setSaveNewMapPopoverAnchor(null);
    setSavePendingChanges(false);
    setSaveNewRendererPopoverAnchor(null);
    if (!editingUserMap?.userMapId) {
      if (!isInSystem(editingUserMap)) {
        if (
          homeContext.selectedUserMap?.userMapId === editingUserMap?.userMapId
        ) {
          setSelectedUserMap(homeContext.prevSelectedUserMap);
        }
      } else {
        if (
          homeContext.selectedSystemUserMap?.userMapId ===
          editingUserMap?.userMapId
        ) {
          setSelectedSystemUserMap(homeContext.prevSelectedSystemUserMap);
        }
      }
    }
    homeContext.setPrevSelectedSystemUserMap(undefined);
    homeContext.setPrevSelectedUserMap(undefined);
  };

  const saveOkClick = (evt: SaveAcceptEvent): void => {
    cleanStateSaveMap();
    if (!editingUserMap || !saveEditingUserMap) return;

    if (isSystemUserMap) {
      evt.route = SYSTEM_RENDERER_FOLDER.PATH;
      setSelectedSystemUserMap(new UserMap(editingUserMap));

      // Cannot update map categories here, because editingUserMap
      // might be out of date at this point, so it's being done
      // inside saveEditingUserMap
      saveEditingUserMap(evt, true, selectedCategories);
    } else {
      setSelectedUserMap(new UserMap(editingUserMap));
      saveEditingUserMap(evt);
    }
  };

  return (
    <Fragment>
      {!mapService.isNewMap(editingUserMap) && (
        <Popover
          open={savePendingChanges}
          ref={popoverRef}
          anchorReference="none"
          anchorOrigin={{
            vertical: 'top',
            horizontal: 'left',
          }}
          transformOrigin={{
            vertical: 'top',
            horizontal: 'left',
          }}
          classes={{ root: classes.popoverRoot }}
          onClose={cleanStateSaveMap}
        >
          <YesNoBox
            title="Do you want to save pending changes on the map?"
            clickYes={(): void => {
              cleanStateSaveMap();
              if (!editingUserMap) return;
              const isSystemUserMap = editingUserMap.folderPath.startsWith(
                SYSTEM_RENDERER_FOLDER.PATH
              );
              if (isSystemUserMap) {
                setSelectedSystemUserMap(new UserMap(editingUserMap));
              } else {
                setSelectedUserMap(new UserMap(editingUserMap));
              }
              const evt = {
                folderName: editingUserMap?.userMapName,
                isChecked: editingUserMap?.isLocked,
                route: editingUserMap?.folderPath,
              };
              saveEditingUserMap(evt, isSystemUserMap);
            }}
            clickNo={(): void => {
              cleanStateSaveMap();
              if (!editingUserMap) return;
              const isSystemUserMap = editingUserMap.folderPath.startsWith(
                SYSTEM_RENDERER_FOLDER.PATH
              );
              //Go back to version of map before modifications
              if (isSystemUserMap) {
                systemUserMapService.applySystemUserMap(
                  selectedSystemUserMap?.userMapId
                );
              } else {
                mapService.updateSelectedMap(selectedUserMap?.userMapId);
              }
            }}
            buttonTextYes="Yes"
            buttonTextNo="No"
            cancelClick={cleanStateSaveMap}
          ></YesNoBox>
        </Popover>
      )}
      {mapService.isNewMap(editingUserMap) && (
        <Popover
          open={savePendingChanges}
          ref={popoverRef}
          anchorReference="none"
          anchorOrigin={{
            vertical: 'top',
            horizontal: 'left',
          }}
          transformOrigin={{
            vertical: 'top',
            horizontal: 'left',
          }}
          classes={{ root: classes.popoverRoot }}
          onClose={cleanStateSaveMap}
        >
          <Save
            isNewSave
            useCategories={isSystemUserMap}
            categoriesData={categoriesData}
            selectedCategories={selectedCategories.map((cat) => cat.id)}
            onCategoryClick={onCategoryClick}
            title={isSystemUserMap ? 'Save system renderer' : 'Save user map'}
            okClick={saveOkClick}
            buttonText="Save"
            popoverTitle={isSystemUserMap ? 'New Category' : 'New Folder'}
            checkboxText="Lock"
            dropdownRows={rows.filter((row) => row.folder)}
            newFolderDropdownRows={rows.filter((r) => r.folder)}
            newFolderOkClick={(e): void => {
              createFolder(e.route + e.folderName);
            }}
            newCategoryOkClick={createNewCategory}
            defaultRoute={
              mapService.isNewMap(editingUserMap)
                ? '/User'
                : editingUserMap?.folderPath
            }
            defaultName={editingUserMap?.userMapName}
            defaultChecked={editingUserMap?.isLocked}
          />
        </Popover>
      )}
    </Fragment>
  );
};

export default SaveMapPopovers;
