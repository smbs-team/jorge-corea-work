/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { useEffect, useRef } from 'react';
import { useMount, useObservable } from 'react-use';
import { layerService, rendererService } from 'services/map';
import { dotService } from 'services/map/dot';
import { $onSelectedLayersChange } from 'services/map/mapServiceEvents';
import pJson from '../../../package.json';
import { useGlMap } from '../map/useGlMap';

/**
 * A hook just for development usage
 */
export const useAddWindowProps = (): void => {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const windowRef = useRef<any>(window);
  const map = useGlMap();
  const renderers = useObservable($onSelectedLayersChange);

  useMount(() => {
    windowRef.current.appVersion = pJson.version;
    windowRef.current.libVersion = pJson.dependencies['@ptas/react-ui-library'];
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    windowRef.current.dotStore = dotService.store;
    windowRef.current.layerService = layerService;
  });

  useEffect(() => {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    windowRef.current.glmap = map;
  }, [map]);

  useEffect(() => {
    windowRef.current.ds = rendererService.getDatasetId();
  }, [renderers]);
};
