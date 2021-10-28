// useMapsMenu.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  useState,
  Dispatch,
  SetStateAction,
  useEffect,
  useContext,
  useCallback,
} from 'react';
import { UserMap } from 'services/map';
import { MenuToolBarOp } from '../common';
import { useUpdateEffect } from 'react-use';
import { HomeContext } from 'contexts';
import { sortBy } from 'lodash';

interface UseMapsMenu {
  activeTab: MenuToolBarOp;
  setActiveTab: Dispatch<SetStateAction<MenuToolBarOp>>;
  sharedMaps: UserMap[];
  personalMaps: UserMap[];
}

export const useMapsMenu = (): UseMapsMenu => {
  const { userMaps, selectedUserMap } = useContext(HomeContext);
  const [sharedMaps, setSharedMaps] = useState<UserMap[]>([]);
  const [personalMaps, setPersonalMaps] = useState<UserMap[]>([]);
  const [activeTab, setActiveTab] = useState<MenuToolBarOp>();
  const selectedUserMapId = selectedUserMap?.userMapId;

  const getRootFolder = useCallback(
    (mapId: number): MenuToolBarOp => {
      const map = userMaps.find((m) => m.userMapId === mapId);
      if (!map) return 'system';
      if (map.folderPath.indexOf('/System') === 0) return 'system';
      if (map.folderPath.indexOf('/Shared') === 0) return 'shared';
      if (map.folderPath.indexOf('/User') === 0) return 'user';
      return 'system';
    },
    [userMaps]
  );

  useEffect(() => {
    selectedUserMapId && setActiveTab(getRootFolder(selectedUserMapId));
  }, [getRootFolder, selectedUserMapId]);

  useUpdateEffect(() => {
    if (!userMaps.length) return;
    const sortedUserMaps = sortBy(userMaps, [
      (m): string => m.userMapName.toLowerCase(),
    ]);

    setSharedMaps(
      sortedUserMaps.filter((map) => map.folderPath.indexOf('/Shared') === 0) ??
        []
    );
    setPersonalMaps(
      sortedUserMaps.filter((map) => map.folderPath.indexOf('/User') === 0) ??
        []
    );
  }, [userMaps]);

  return {
    activeTab,
    setActiveTab,
    sharedMaps,
    personalMaps,
  };
};
