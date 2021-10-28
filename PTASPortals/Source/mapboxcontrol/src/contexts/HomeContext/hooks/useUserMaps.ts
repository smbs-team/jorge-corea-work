/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  Dispatch,
  SetStateAction,
  useEffect,
  useState,
  useContext,
} from 'react';
import { Subscription } from 'rxjs';
import {
  SaveAcceptEvent,
  SnackContext,
  ErrorMessageAlertCtx,
} from '@ptas/react-ui-library';
import { mapService, UserDetails, UserMap } from 'services/map';
import {
  $onSelectedSystemUserMapChange,
  $onSelectedUserMapChange,
} from 'services/map/mapServiceEvents';
import {
  CategoryData,
  systemUserMapService,
} from 'services/map/systemUserMapService';
import { HomeContextProps } from '../types';

export type UseUserMaps = {
  userMaps: UserMap[];
  setUserMaps: Dispatch<SetStateAction<UserMap[]>>;
  selectedUserMap: UserMap | undefined;
  setSelectedUserMap: Dispatch<SetStateAction<UserMap | undefined>>;
  selectedSystemUserMap: UserMap | undefined;
  setSelectedSystemUserMap: Dispatch<SetStateAction<UserMap | undefined>>;
  editingUserMap: UserMap | undefined;
  setEditingUserMap: Dispatch<SetStateAction<UserMap | undefined>>;
  mapsUserDetails: UserDetails[];
  saveEditingUserMap(
    evt?: SaveAcceptEvent,
    isSystemUserMap?: boolean,
    categories?: CategoryData[]
  ): Promise<void>;
  saveUserMap(userMap: UserMap): void;
  colorRuleAction: 'create' | 'update' | undefined;
  setColorRuleAction: Dispatch<SetStateAction<'create' | 'update' | undefined>>;
  prevSelectedUserMap: UserMap | undefined;
  setPrevSelectedUserMap: Dispatch<SetStateAction<UserMap | undefined>>;
  prevSelectedSystemUserMap: UserMap | undefined;
  setPrevSelectedSystemUserMap: Dispatch<SetStateAction<UserMap | undefined>>;
  selectedUserMapId: UserMap['userMapId'] | undefined;
};

export const useUserMaps = ({
  currentLayer,
  setLinearProgress,
}: Pick<
  HomeContextProps,
  'currentLayer' | 'setLinearProgress'
>): UseUserMaps => {
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const { setSnackState } = useContext(SnackContext);
  const [userMaps, setUserMaps] = useState<UserMap[]>([]);
  const [prevSelectedUserMap, setPrevSelectedUserMap] = useState<UserMap>();
  const [selectedUserMap, setSelectedUserMap] = useState<UserMap | undefined>();
  const [selectedUserMapId, setSelectedUserMapId] = useState<
    UserMap['userMapId']
  >();

  const [editingUserMap, setEditingUserMap] = useState<UserMap | undefined>();
  const [selectedSystemUserMap, setSelectedSystemUserMap] = useState<
    UserMap | undefined
  >();
  const [prevSelectedSystemUserMap, setPrevSelectedSystemUserMap] = useState<
    UserMap
  >();
  const [mapsUserDetails, setMapsUserDetails] = useState<UserDetails[]>([]);
  const [colorRuleAction, setColorRuleAction] = useState<'create' | 'update'>();

  useEffect(() => {
    setSelectedUserMapId(selectedUserMap?.userMapId);
  }, [selectedUserMap]);

  useEffect(() => {
    const subs: Subscription[] = [];
    subs.push(
      mapService.$onUserMapsChange.subscribe(({ userMaps, userDetails }) => {
        if (userMaps) {
          setUserMaps(userMaps);
        } else {
          setUserMaps([]);
        }
        setMapsUserDetails(userDetails);
      })
    );
    subs.push(
      $onSelectedUserMapChange.subscribe((userMap) => {
        setSelectedUserMap(userMap);
      })
    );
    subs.push(
      $onSelectedSystemUserMapChange.subscribe((userMap) => {
        setSelectedSystemUserMap(userMap);
      })
    );
    return (): void => {
      for (const sub of subs) {
        sub.unsubscribe();
      }
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const saveEditingUserMap = async (
    evt?: SaveAcceptEvent,
    isSystemUserMap?: boolean,
    categories?: CategoryData[]
  ): Promise<void> => {
    if (!editingUserMap || !evt) return;
    const _editingUserMap = {
      ...editingUserMap,
    };
    if (!evt.folderName) {
      return showErrorMessage({
        message: 'Invalid user map name',
      });
    }
    if (!evt.route) {
      return showErrorMessage({
        message: 'Invalid folder path',
      });
    }
    evt.folderName = evt.folderName.trim();
    if (evt.folderName.length > 64) {
      evt.folderName = evt.folderName.substr(0, 64);
    }
    _editingUserMap.userMapName = evt.folderName;
    _editingUserMap.folderPath = evt.route;
    _editingUserMap.isLocked = evt.isChecked === true;

    const res = await mapService.saveUserMap(_editingUserMap);
    const msg =
      (isSystemUserMap ? 'System renderer ' : 'User map ') +
      (editingUserMap.userMapId === 0 ? 'saved' : 'updated');
    setSnackState({
      severity: 'success',
      text: msg,
    });

    if (!isSystemUserMap) {
      mapService.refreshUserMaps();
    }

    if (_editingUserMap.userMapId) {
      //Existing user map
      if (isSystemUserMap) {
        systemUserMapService.applySystemUserMap(
          _editingUserMap.userMapId,
          true
        );
        if (categories?.length) {
          await systemUserMapService.setCategories(
            _editingUserMap.userMapId,
            categories
          );
          systemUserMapService.loadCategories(true);
        }
      } else {
        mapService.updateSelectedMap(_editingUserMap.userMapId, true);
      }
    }
    //New user map
    if (isSystemUserMap) {
      systemUserMapService.applySystemUserMap(res, true);
      if (categories?.length) {
        await systemUserMapService.setCategories(res, categories);
        systemUserMapService.loadCategories(true);
      }
    } else {
      mapService.updateSelectedMap(res, true);
    }

    if (
      currentLayer &&
      currentLayer.rendererRules.colorRule &&
      colorRuleAction === 'create'
    ) {
      setColorRuleAction('update');
    }
  };

  const saveUserMap = (userMap: UserMap): void => {
    setLinearProgress(true);
    mapService
      .saveUserMap(userMap)
      .then(() => {
        setSnackState({
          severity: 'success',
          text: `User map ${userMap.userMapId === 0 ? 'saved' : 'updated'}`,
        });
      })
      .finally(() => {
        setLinearProgress(false);
      });
  };

  return {
    userMaps,
    setUserMaps,
    selectedUserMap,
    setSelectedUserMap,
    selectedSystemUserMap,
    setSelectedSystemUserMap,
    mapsUserDetails,
    editingUserMap,
    setEditingUserMap,
    saveEditingUserMap,
    saveUserMap,
    colorRuleAction,
    setColorRuleAction,
    prevSelectedUserMap,
    setPrevSelectedUserMap,
    prevSelectedSystemUserMap,
    setPrevSelectedSystemUserMap,
    selectedUserMapId,
  };
};
