/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { HomeContext } from 'contexts';
import { useGridResizer } from 'hooks';
import { OnResizeFields } from 'libComponents/Resizable';
import { groupBy } from 'lodash';
import React, {
  createContext,
  PropsWithChildren,
  ComponentType,
  FC,
  useState,
  useContext,
  useRef,
  Dispatch,
  SetStateAction,
  MutableRefObject,
  useEffect,
} from 'react';
import { useDebounce, useObservable } from 'react-use';
import { mapService } from 'services/map';
import { AnnotationLabelService } from 'services/map/annotationLabelService/annotationLabelService';
import { $onSelectedLayersChange } from 'services/map/mapServiceEvents';
import { unimplementedStateFn } from '@ptas/react-ui-library';

interface ContextProps {
  tabIndex: number;
  setTabIndex: React.Dispatch<React.SetStateAction<number>>;
  tabItems: string[];
  setTabItems: React.Dispatch<React.SetStateAction<string[]>>;
  setSavePopoverAnchor: React.Dispatch<
    React.SetStateAction<HTMLElement | null>
  >;
  savePopoverAnchor: HTMLElement | null;
  gridHeight: number;
  resizeGrid: (fields: OnResizeFields) => void;
  bottomSection: 'colorRenderer' | 'labelRenderer' | undefined;
  setBottomSection: Dispatch<
    SetStateAction<'colorRenderer' | 'labelRenderer' | undefined>
  >;
  scrollToLabelRendererSection: () => void;
  labelRendererSectionRef: MutableRefObject<HTMLDivElement | null> | undefined;
  isSystemRenderer: boolean;
  setIsSystemRenderer: (val: boolean) => void;
}

export const lineTabItem = 'Outline';

export const MapLayersManageContext = createContext<ContextProps>({
  gridHeight: 0,
  resizeGrid: unimplementedStateFn,
  bottomSection: undefined,
  setBottomSection: unimplementedStateFn,
  scrollToLabelRendererSection: unimplementedStateFn,
  isSystemRenderer: false,
  setIsSystemRenderer: unimplementedStateFn,
  labelRendererSectionRef: undefined,
  savePopoverAnchor: null,
  setSavePopoverAnchor: unimplementedStateFn,
  setTabIndex: unimplementedStateFn,
  tabIndex: 0,
  setTabItems: unimplementedStateFn,
  tabItems: [],
});

export const withLayerManage = <P extends object>(
  Component: ComponentType<P>
): FC<P> => (props: PropsWithChildren<P>): JSX.Element => {
  const { gridHeight, resizeGrid } = useGridResizer(175);
  const { currentLayer, setCurrentLayer, editingUserMap } = useContext(
    HomeContext
  );
  const labelRendererSectionRef = useRef<HTMLDivElement | null>(null);
  const [bottomSection, setBottomSection] = useState<
    undefined | 'colorRenderer' | 'labelRenderer'
  >();
  const renderers = useObservable($onSelectedLayersChange, []);
  const [isSystemRenderer, setIsSystemRenderer] = useState(false);
  const [
    savePopoverAnchor,
    setSavePopoverAnchor,
  ] = useState<HTMLElement | null>(null);
  const [tabIndex, setTabIndex] = useState<number>(0);
  const [tabItems, setTabItems] = useState<string[]>([lineTabItem]);

  useEffect(() => {
    if (editingUserMap && currentLayer) {
      const layer = editingUserMap.mapRenderers.find(
        (map) => map.layerSourceId === currentLayer.layerSourceId
      );
      if (layer) {
        setCurrentLayer(layer);
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [editingUserMap]);

  const scrollToLabelRendererSection = (): void => {
    if (labelRendererSectionRef.current) {
      labelRendererSectionRef.current.scrollIntoView({
        behavior: 'smooth',
        block: 'nearest',
        inline: 'start',
      });
    }
  };

  useDebounce(
    () => {
      if (
        renderers.some((val) =>
          AnnotationLabelService.instance?.isAnnoLayer(
            val.rendererRules.layer.id
          )
        )
      ) {
        const featureGroups = groupBy(
          AnnotationLabelService.instance?.features ?? [],
          (val) => val.properties?.refLayerId
        );
        for (const [, v] of Object.entries(featureGroups)) {
          const first = v.shift();
          mapService.map?.removeLayer(first?.properties?.layerId ?? '');
        }
        AnnotationLabelService.instance?.onLayerAdded();
      }
    },
    1000,
    [renderers]
  );

  return (
    <MapLayersManageContext.Provider
      value={{
        isSystemRenderer,
        setIsSystemRenderer,
        gridHeight,
        resizeGrid,
        bottomSection,
        setBottomSection,
        labelRendererSectionRef,
        scrollToLabelRendererSection,
        savePopoverAnchor,
        setSavePopoverAnchor,
        setTabIndex,
        tabIndex,
        setTabItems,
        tabItems,
      }}
    >
      <Component {...props}>{props.children}</Component>
    </MapLayersManageContext.Provider>
  );
};
