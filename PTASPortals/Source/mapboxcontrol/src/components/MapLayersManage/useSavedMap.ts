/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { HomeContext } from 'contexts';
import { MutableRefObject, useContext, useEffect, useRef } from 'react';
import { UserMap } from 'services/map';

export const useSavedMap = (
  isSystemUserMap: boolean
): MutableRefObject<UserMap | undefined> => {
  const { selectedUserMap, selectedSystemUserMap } = useContext(HomeContext);
  const savedMapRef = useRef<UserMap>();
  useEffect(() => {
    if (isSystemUserMap && selectedSystemUserMap) {
      savedMapRef.current = new UserMap(selectedSystemUserMap);
    } else if (selectedUserMap) {
      savedMapRef.current = new UserMap(selectedUserMap);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedUserMap, selectedSystemUserMap]);
  return savedMapRef;
};
