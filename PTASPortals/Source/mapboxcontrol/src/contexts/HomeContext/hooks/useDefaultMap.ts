/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState, useContext, useEffect } from 'react';
import { SnackContext, unimplementedStateFn } from '@ptas/react-ui-library';
import { apiService } from 'services/api';
import { mapService, UserMap } from 'services/map';
import { first } from 'rxjs/operators';

export type UseDefaultMap = {
  defaultMap: UserMap['userMapId'];
  saveDefaultMap: (id: UserMap['userMapId']) => Promise<void>;
};

export const defMapInitialState: UseDefaultMap = {
  defaultMap: 0,
  saveDefaultMap: unimplementedStateFn,
};

export const useDefaultMap = (): UseDefaultMap => {
  const [defaultMap, setDefaultMap] = useState<UserMap['userMapId']>(0);
  const { setSnackState } = useContext(SnackContext);

  const saveDefaultMap = async (id: UserMap['userMapId']): Promise<void> => {
    setDefaultMap(id);
    const res = await apiService.metaStore.saveUserStoreItem({
      storeType: 'default-map',
      value: {
        mapId: id,
      },
    });
    if (res.hasError) {
      setSnackState({
        severity: 'error',
        text: 'Error setting default map',
      });
    }
  };

  useEffect(() => {
    const sub = mapService.$onUserMapsChange
      .pipe(first())
      .subscribe(({ defaultUserMapId }) => {
        if (!defaultUserMapId) return;
        setDefaultMap(defaultUserMapId);
      });
    return (): void => {
      sub.unsubscribe();
    };
  }, []);

  return {
    defaultMap,
    saveDefaultMap,
  };
};
