// context.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { unimplementedStateFn } from '@ptas/react-ui-library';
import { AppLayers, FONTS } from 'appConstants';
import { HomeContext } from 'contexts';
import { Anchor, Expression } from 'mapbox-gl';
import React, {
  createContext,
  PropsWithChildren,
  ComponentType,
  FC,
  useContext,
  useState,
  useEffect,
  Dispatch,
  SetStateAction,
} from 'react';
import { useDebounce } from 'react-use';
import { PtasLayer } from 'services/map';
import { RendererLabel } from 'services/map/renderer/textRenderer/textRenderersService';
import { isInSystem } from 'utils/userMap';
import { useUpdateCurrentLayerState } from '../useUpdateEditingMapLayersState';
import { ClassTagLabel } from './typing';

export const OPERATORS_BY_TYPE = [
  {
    label: '=',
    value: '==',
    type: ['number', 'string', 'date', 'boolean'],
  },
  {
    label: '<>',
    value: '!=',
    type: ['number', 'string', 'date'],
  },
  {
    label: '>',
    value: '>',
    type: ['number', 'date'],
  },
  {
    label: '<',
    value: '<',
    type: ['number', 'date'],
  },
  {
    label: '>=',
    value: '>=',
    type: ['number', 'date'],
  },
  {
    label: '<=',
    value: '<=',
    type: ['number', 'date'],
  },
  {
    label: 'Included',
    value: 'in',
    type: ['string'],
  },
  {
    label: 'Not included',
    value: '!in',
    type: ['string'],
  },
];

// eslint-disable-next-line @typescript-eslint/no-empty-interface
interface TagsManageContextProps {
  labelConfigs: ClassTagLabel[];
  setLabelConfigs: Dispatch<SetStateAction<ClassTagLabel[]>>;
  selectedLabel: ClassTagLabel | undefined;
  setSelectedLabel: Dispatch<SetStateAction<ClassTagLabel | undefined>>;
  addTextRendererLayer: () => void;
}

const vAlignLower = (text: string): string => (text ?? 'top').toLowerCase();

const hAlignLower = (text: string): string => (text ?? 'left').toLowerCase();

const secureParseInt = (n: string): number => {
  try {
    return parseInt(n);
  } catch (error) {
    return 12;
  }
};

const calculateTextAnchor = (textAlign: string): string => {
  let anchor;
  switch (textAlign.toLowerCase()) {
    case 'middle-left':
      anchor = 'left';
      break;
    case 'middle-right':
      anchor = 'right';
      break;
    case 'top-center':
      anchor = 'top';
      break;
    case 'bottom-center':
      anchor = 'bottom';
      break;
    case 'middle-center':
      anchor = 'center';
      break;
    default:
      anchor = textAlign.toLowerCase();
      break;
  }

  return anchor;
};

const interpolateConf = {
  start: 80,
  end: 140,
};
const interpolation: Record<'top' | 'bottom', Expression> = {
  top: [
    'interpolate',
    ['linear'],
    ['zoom'],
    20,
    ['literal', [0, interpolateConf.start]],
    22,
    ['literal', [0, interpolateConf.end]],
  ],
  bottom: [
    'interpolate',
    ['linear'],
    ['zoom'],
    16,
    ['literal', [0, -1 * interpolateConf.start]],
    22,
    ['literal', [0, -1 * interpolateConf.end]],
  ],
};

export const TagsManageContext = createContext<TagsManageContextProps>({
  addTextRendererLayer: unimplementedStateFn,
  labelConfigs: [],
  selectedLabel: undefined,
  setLabelConfigs: unimplementedStateFn,
  setSelectedLabel: unimplementedStateFn,
});

export const withTagsManage = <P extends object>(
  Component: ComponentType<P>
): FC<P> => (props: PropsWithChildren<P>): JSX.Element => {
  const { currentLayer, editingUserMap } = useContext(HomeContext);
  const [labelConfigs, setLabelConfigs] = useState<ClassTagLabel[]>([]);
  const [selectedLabel, setSelectedLabel] = useState<ClassTagLabel>();
  const updateCurrentLayer = useUpdateCurrentLayerState();
  const refLayerId = currentLayer?.rendererRules.layer.id;

  useEffect(() => {
    if (selectedLabel && selectedLabel.refLayerId !== refLayerId)
      setSelectedLabel(undefined);

    loadTextRenderer();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [refLayerId]);

  const loadTextRenderer = (): void => {
    if (!currentLayer || !editingUserMap?.mapRenderers.length) return;
    const renderLabelLoaded = editingUserMap.mapRenderers.find(
      (mr) => mr.rendererRules.layer.id === refLayerId
    );
    if (!renderLabelLoaded) return;
    const labels = renderLabelLoaded.rendererRules?.labels ?? [];

    const labelConfigs = labels.map((l) => l.labelConfig);
    setLabelConfigs(labelConfigs as ClassTagLabel[]);
  };

  useEffect(() => {
    setLabelConfigs(
      (prev) =>
        prev.map((lc) => {
          return lc.id === selectedLabel?.id ? selectedLabel : lc;
        }) ?? []
    );
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [JSON.stringify(selectedLabel)]);

  const addTextRendererLayer = (): void => {
    if (!editingUserMap) return;
    const labels = labelConfigs ?? [];
    if (currentLayer?.rendererRules.layer.id) {
      const labelRenderList: RendererLabel[] = [];
      for (const lbl of labels) {
        const {
          content,
          fontSize,
          isBoldText,
          color,
          horizontalAlign,
          verticalAlign,
          padding,
          minZoom,
          maxZoom,
          typeZoom,
          id: layerId,
          refLayerId,
        } = lbl;

        const vAlign = vAlignLower(verticalAlign as string);
        const hAlign = hAlignLower(horizontalAlign as string);

        const textAlign = [vAlign, hAlign].join('-');
        const textAnchor =
          currentLayer.rendererRules.layer.type === 'fill'
            ? (calculateTextAnchor(textAlign) as Anchor)
            : undefined;

        const font = isBoldText
          ? FONTS.OPEN_SANS_BOLD
          : FONTS.OPEN_SANS_REGULAR;
        const textFont = [font, FONTS.ARIAL_UNICODE_MS_BOLD];
        const textField =
          refLayerId === AppLayers.PARCEL_LAYER
            ? '{text}'
            : (content ?? '').replaceAll('\\n', '\n');
        const filterControl =
          typeZoom === 'all' ? {} : { minzoom: minZoom, maxzoom: maxZoom };
        const textPadding = padding || padding === 0 ? padding : 2;

        const translateVal =
          vAlign === 'top'
            ? interpolation.top
            : vAlign === 'bottom'
            ? interpolation.bottom
            : undefined;

        const textLayer: PtasLayer = {
          metadata: {
            isSystemRenderer: isInSystem(editingUserMap),
          },
          type: 'symbol',
          id: layerId,
          source: layerId,
          ...filterControl,
          layout: {
            'icon-image': layerId,
            'text-field': textField,
            'text-anchor': textAnchor,
            'text-size': secureParseInt(fontSize as string),
            'text-font': textFont,
            'symbol-avoid-edges': true,
            'icon-text-fit-padding': [
              textPadding,
              textPadding,
              textPadding,
              textPadding,
            ], //top, right, bottom, left
            'icon-text-fit': 'both',
            visibility: 'visible',
          },
          paint: {
            'text-color': color,
            ...(translateVal
              ? {
                  'icon-translate': translateVal,
                  'text-translate': translateVal,
                }
              : {}),
          },
        };
        const newRenderLabel: RendererLabel = {
          layer: textLayer,
          labelConfig: lbl,
        };
        labelRenderList.push(newRenderLabel);
      }

      updateCurrentLayer((layer) => ({
        ...layer,
        rendererRules: {
          ...layer.rendererRules,
          labels: labelRenderList,
        },
      }));
    }
  };

  useDebounce(
    () => {
      addTextRendererLayer();
    },
    150,
    [JSON.stringify(labelConfigs)]
  );

  useEffect(() => {
    if (!labelConfigs.length) {
      setSelectedLabel(undefined);
    }
  }, [labelConfigs.length]);

  return (
    <TagsManageContext.Provider
      value={{
        setSelectedLabel,
        setLabelConfigs,
        selectedLabel,
        labelConfigs,
        addTextRendererLayer,
      }}
    >
      <Component {...props}>{props.children}</Component>
    </TagsManageContext.Provider>
  );
};
