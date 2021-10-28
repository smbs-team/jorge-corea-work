/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CirclePaint, FillPaint, LinePaint } from 'mapbox-gl';
import {
  Dispatch,
  SetStateAction,
  useContext,
  useEffect,
  useState,
} from 'react';
import { useUpdateEffect } from 'react-use';
import { RGBA_TRANSPARENT } from 'appConstants';
import { HomeContext } from 'contexts';
import { useUpdateCurrentLayerState } from 'components/MapLayersManage/useUpdateEditingMapLayersState';
import { getLayerFillColor, getLayerOutlineColor } from './utils';
import { lineTabItem, MapLayersManageContext } from '../Context';

type LayerColors = {
  fill: string;
  outline: string;
};

type UseRootLayerStyle = {
  applyLayerColors: (color: string) => void;
  getColorValue: () => string;
  tabIndex: number;
  setTabIndex: Dispatch<SetStateAction<number>>;
  tabItems: string[];
  applyPaintProps: <T extends CirclePaint | LinePaint | FillPaint>(
    props: T
  ) => void;
};

enum TabIndexes {
  Fill = 0,
  Outline = 1,
}

export const useRootLayerStyle = (): UseRootLayerStyle => {
  const updateCurrentLayerState = useUpdateCurrentLayerState();
  const { currentLayer } = useContext(HomeContext);
  const { tabIndex, setTabIndex, tabItems, setTabItems } = useContext(
    MapLayersManageContext
  );
  const [layerColors, setLayerColors] = useState<LayerColors>({
    fill: RGBA_TRANSPARENT,
    outline: RGBA_TRANSPARENT,
  });

  useEffect(() => {
    setLayerColors({
      fill: getLayerFillColor(currentLayer?.rendererRules.layer),
      outline: getLayerOutlineColor(currentLayer?.rendererRules.layer),
    });

    if (currentLayer?.rendererRules.layer.type === 'line') {
      setTabIndex(0);
      setTabItems([lineTabItem]);
    } else {
      setTabItems(['Fill', lineTabItem]);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentLayer?.mapRendererId]);

  const applyLayerColors = (color: string): void => {
    if (!currentLayer) return;
    switch (currentLayer.rendererRules.layer.type) {
      case 'fill':
      case 'circle': {
        setLayerColors((prev) => ({
          fill: tabIndex === TabIndexes.Fill ? color : prev.fill,
          outline: tabIndex === TabIndexes.Outline ? color : prev?.outline,
        }));
        break;
      }
      case 'line': {
        setLayerColors((prev) => ({
          fill: prev.fill,
          outline: color,
        }));
        break;
      }
    }
  };

  useUpdateEffect(() => {
    updateCurrentLayerState((renderer) => {
      switch (renderer.rendererRules.layer.type) {
        case 'fill': {
          (renderer.rendererRules.layer.paint as FillPaint)['fill-color'] =
            layerColors.fill;
          (renderer.rendererRules.layer.paint as FillPaint)[
            'fill-outline-color'
          ] = layerColors.outline;
          break;
        }
        case 'line': {
          (renderer.rendererRules.layer.paint as LinePaint)['line-color'] =
            layerColors.outline;
          break;
        }
        case 'circle': {
          (renderer.rendererRules.layer.paint as CirclePaint)['circle-color'] =
            layerColors.fill;
          (renderer.rendererRules.layer.paint as CirclePaint)[
            'circle-stroke-color'
          ] = layerColors.outline;
          break;
        }
      }

      return renderer;
    });

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [layerColors]);

  const getColorValue = (): string => {
    switch (currentLayer?.rendererRules.layer.type) {
      case 'fill':
      case 'circle': {
        return (
          (tabIndex === TabIndexes.Fill
            ? getLayerFillColor(currentLayer.rendererRules.layer)
            : getLayerOutlineColor(currentLayer.rendererRules.layer)) ??
          RGBA_TRANSPARENT
        );
      }
      case 'line': {
        return getLayerOutlineColor(currentLayer.rendererRules.layer);
      }
      default: {
        return RGBA_TRANSPARENT;
      }
    }
  };

  const applyPaintProps = <T extends CirclePaint | LinePaint | FillPaint>(
    props: T
  ): void => {
    updateCurrentLayerState((renderer) => {
      renderer.rendererRules.layer.paint = {
        ...renderer.rendererRules.layer.paint,
        ...props,
      };
      return renderer;
    });
  };

  return {
    applyLayerColors,
    getColorValue,
    setTabIndex,
    tabIndex,
    tabItems,
    applyPaintProps,
  };
};
