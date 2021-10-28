/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { HomeContext } from 'contexts';
import { CirclePaint } from 'mapbox-gl';
import {
  Dispatch,
  SetStateAction,
  useContext,
  useEffect,
  useState,
} from 'react';
import { useUpdateCurrentLayerState } from '../useUpdateEditingMapLayersState';

export const useCircleRadius = (): [
  number | undefined,
  Dispatch<SetStateAction<number | undefined>>
] => {
  const { currentLayer } = useContext(HomeContext);
  const updateCurrentLayerState = useUpdateCurrentLayerState();
  const [circleRadius, setCircleRadius] = useState<number | undefined>(0);

  useEffect(() => {
    switch (currentLayer?.rendererRules.layer.type) {
      case 'circle': {
        const r = currentLayer.rendererRules.layer.paint?.['circle-radius'];
        setCircleRadius(typeof r === 'number' ? r : 1);
        break;
      }
    }
  }, [currentLayer]);

  useEffect(() => {
    updateCurrentLayerState((renderer) => {
      switch (renderer.rendererRules.layer.type) {
        case 'circle': {
          if (renderer.rendererRules.layer.paint) {
            renderer.rendererRules.layer.paint['circle-radius'] = circleRadius;
          }
          if (renderer.rendererRules.colorRule?.layer.paint) {
            (renderer.rendererRules.colorRule.layer.paint as CirclePaint)[
              'circle-radius'
            ] = circleRadius;
          }
          break;
        }
      }

      return renderer;
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [circleRadius]);

  return [circleRadius, setCircleRadius];
};
