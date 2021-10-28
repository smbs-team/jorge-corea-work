/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { HomeContext } from 'contexts';
import { Dispatch, SetStateAction, useContext, useMemo } from 'react';
import { MapRenderer, UserMap } from 'services/map';

type UseUpdateMapRenderersState = {
  userMap: (cb: (renderer: MapRenderer) => MapRenderer) => void;
  systemUserMap: (cb: (renderer: MapRenderer) => MapRenderer) => void;
};

export const useUpdateMapRenderersState = (): UseUpdateMapRenderersState => {
  const { setSelectedUserMap, setSelectedSystemUserMap } = useContext(
    HomeContext
  );

  return useMemo(() => {
    const updateRend = (
      setFn: Dispatch<SetStateAction<UserMap | undefined>> | undefined
    ) => (cb: (renderer: MapRenderer) => MapRenderer): void => {
      setFn &&
        setFn((prev) => {
          if (!prev) return;
          return {
            ...prev,
            mapRenderers:
              prev.mapRenderers?.map((renderer) => cb(renderer)) ?? [],
          };
        });
    };
    return {
      userMap: updateRend(setSelectedUserMap),
      systemUserMap: updateRend(setSelectedSystemUserMap),
    };
  }, [setSelectedSystemUserMap, setSelectedUserMap]);
};
