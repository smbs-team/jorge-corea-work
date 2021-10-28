/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { TreeViewRow } from '@ptas/react-ui-library';
import { Properties } from '@turf/turf';
import { AppLayers } from 'appConstants';
import { useMapEvent } from 'hooks/map';
import { uniqBy } from 'lodash';
import {
  Dispatch,
  SetStateAction,
  useEffect,
  useRef,
  useState,
} from 'react';
import { layerService, mapService } from 'services/map';
import { parcelUtil } from 'utils/parcelUtil';

export type LayerTreeItem = Pick<TreeViewRow, 'id' | 'parent'> & {
  name: string;
  layer: string;
  featureProps?: Properties;
};

interface RightTreeRow {
  id: React.ReactText;
  parent: React.ReactText | null;
  field: string;
  value: string;
  layerId: string | number | null;
}
export interface UseFeaturesTree {
  layersTree: LayerTreeItem[];
  setLayersTree: (value: SetStateAction<LayerTreeItem[]>) => void;
  dropdownOption: InfoModeDropDownKey;
  setDropDownOption: Dispatch<SetStateAction<InfoModeDropDownKey>>;
  rightTreeData: RightTreeRow[];
  setRightTreeData: Dispatch<SetStateAction<RightTreeRow[]>>;
}

export type InfoModeDropDownKey = 'visible' | 'top' | 'all';

export const useFeaturesTree = (): UseFeaturesTree => {
  const TOP_MOST = +(process.env.REACT_APP_INFO_MODE_TOP_MOST ?? '2');
  const [layersTree, setLayersTree] = useState<LayerTreeItem[]>([]);
  const [rightTreeData, setRightTreeData] = useState<RightTreeRow[]>([]);
  const clickEvtData = useMapEvent('click');
  const [dropdownOption, setDropDownOption] = useState<InfoModeDropDownKey>(
    'all'
  );
  const allLayers = useRef<LayerTreeItem[]>([]);
  useEffect(() => {
    if (!mapService.map) return;
    const fn = (): void => {
      if (!mapService.map) return;
      mapService.map.getCanvas().style.cursor = 'crosshair';
    };
    mapService.map.on('mousemove', fn);
    return (): void => {
      if (!mapService.map) return;
      mapService.map.off('mousemove', fn);
      mapService.map.getCanvas().style.cursor = '';
    };
  }, []);

  useEffect(() => {
    setLayersTree((prev) => {
      if (dropdownOption === 'top') {
        let cnt = 0;
        const removedItems: LayerTreeItem[] = [];
        const newArr: LayerTreeItem[] = [];
        prev.forEach((item1) => {
          if (item1.parent === null) {
            if (cnt < TOP_MOST) {
              newArr.push(item1);
              cnt++;
            } else {
              removedItems.push(item1);
            }
          } else {
            if (removedItems.every((item2) => item2.id === item1.parent)) {
              newArr.push(item1);
            } else {
              removedItems.push(item1);
            }
          }
        });
        return newArr;
      }
      return allLayers.current;
    });
  }, [TOP_MOST, dropdownOption]);

  useEffect(() => {
    if (!clickEvtData) return;
    const renderers =
      layerService.renderers?.slice(
        0,
        dropdownOption === 'top'
          ? TOP_MOST
          :  layerService.renderers.length
      ) ?? [];
    allLayers.current =
      renderers.reduce<LayerTreeItem[]>((prev, current) => {
        if (!mapService.map) return [];
        const layerFeatures = uniqBy(
          mapService.map.queryRenderedFeatures(clickEvtData.point, {
            layers: [current.rendererRules.layer.id],
            validate: false,
          }),
          (f) => f.id
        );
        if (!layerFeatures.length) return prev;
        const retVal = [
          ...prev,
          {
            id: current.mapRendererId,
            name: current.rendererRules.layer.id,
            parent: null,
            layer: current.rendererRules.layer.id,
          },
        ];

        layerFeatures.forEach((_feature, i) => {
          retVal.push({
            id: current.rendererRules.layer.id + _feature.id + i,
            name:
              current.rendererRules.layer.id === AppLayers.PARCEL_LAYER
                ? parcelUtil.formatPin(_feature.properties?.PIN)
                : '' + _feature.id,
            parent: current.mapRendererId,
            layer: current.rendererRules.layer.id,
            featureProps: _feature.properties,
          });
        });

        return retVal;
      }, []) ?? [];
    setLayersTree(allLayers.current);
  }, [
    clickEvtData,
    layerService.renderers,
  ]);

  return {
    layersTree,
    setLayersTree,
    dropdownOption,
    setDropDownOption,
    rightTreeData,
    setRightTreeData,
  };
};
