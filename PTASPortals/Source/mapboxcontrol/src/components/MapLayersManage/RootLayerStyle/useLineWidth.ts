/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { HomeContext } from 'contexts';
import { CirclePaint, LinePaint } from 'mapbox-gl';
import {
  Dispatch,
  SetStateAction,
  useState,
  useContext,
  useEffect,
} from 'react';
import { useUpdateCurrentLayerState } from '../useUpdateEditingMapLayersState';

export const useLineWidth = (): [
  number | undefined,
  Dispatch<SetStateAction<number | undefined>>
] => {
  const { currentLayer } = useContext(HomeContext);
  const updateCurrentLayerState = useUpdateCurrentLayerState();
  const [lineWidth, setLineWidth] = useState<number | undefined>(0);

  useEffect(() => {
    switch (currentLayer?.rendererRules.layer.type) {
      case 'line': {
        const w = currentLayer.rendererRules.layer.paint?.['line-width'];
        setLineWidth(typeof w === 'number' ? w : 1);
        break;
      }
      case 'circle': {
        const w =
          currentLayer.rendererRules.layer.paint?.['circle-stroke-width'];
        setLineWidth(typeof w === 'number' ? w : 1);
        break;
      }
    }
  }, [currentLayer]);

  useEffect(() => {
    updateCurrentLayerState((renderer) => {
      switch (renderer.rendererRules.layer.type) {
        case 'line': {
          if (renderer.rendererRules.layer.paint) {
            renderer.rendererRules.layer.paint['line-width'] = lineWidth;
          }
          if (renderer.rendererRules.colorRule?.layer.paint) {
            (renderer.rendererRules.colorRule.layer.paint as LinePaint)[
              'line-width'
            ] = lineWidth;
          }
          break;
        }
        case 'circle': {
          if (renderer.rendererRules.layer.paint) {
            renderer.rendererRules.layer.paint[
              'circle-stroke-width'
            ] = lineWidth;
          }
          if (renderer.rendererRules.colorRule?.layer.paint) {
            (renderer.rendererRules.colorRule.layer.paint as CirclePaint)[
              'circle-stroke-width'
            ] = lineWidth;
          }
          break;
        }
      }

      return renderer;
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [lineWidth]);

  return [lineWidth, setLineWidth];
};
