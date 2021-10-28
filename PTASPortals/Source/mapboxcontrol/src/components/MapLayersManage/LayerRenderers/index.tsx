/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext } from 'react';
import {
  Box,
  Theme,
  createStyles,
  useTheme,
  makeStyles,
} from '@material-ui/core';
import clsx from 'clsx';
import { HighlightButton } from '@ptas/react-ui-library';
import { v4 as uuidV4 } from 'uuid';

import { useManageLayerCommonStyles } from '../styles';
import { MapLayersManageContext } from '../Context';
import { PanelTitle } from 'components/common/panel';
import {
  layerService,
  MapRendererRules,
  RendererSimpleRule,
} from 'services/map';
import { HomeContext } from 'contexts/HomeContext';
import { useUpdateCurrentLayerState } from 'components/MapLayersManage/useUpdateEditingMapLayersState';
import { DEFAULT_MAX_ZOOM, DEFAULT_MIN_ZOOM } from 'appConstants';
import { isInSystem } from 'utils/userMap';

export const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      marginBottom: theme.spacing(1),
      fontFamily: theme.ptas.typography.bodyFontFamily,
    },
    rootChild: {
      flexBasis: '20%',
      marginBottom: theme.spacing(2),
    },
    rowFlexBox: {
      display: 'flex',
      alignItems: 'center',
    },
    columnFlexBox: {
      display: 'flex',
      flexDirection: 'column',
    },
    dropDownWrap: {
      maxWidth: '100px',
    },
    line: {
      width: 71,
    },
    scale: {
      width: 105,
    },
    and: {
      lineHeight: 2.7,
      padding: theme.spacing(0, 1),
    },
    marginRight: {
      marginRight: theme.spacing(1),
    },
    marginTop: {
      marginTop: theme.spacing(2),
    },
    colorField: {
      width: 105,
    },
  })
);

const LayerRenderers = (): JSX.Element => {
  const { editingUserMap, currentLayer, setColorRuleAction } = useContext(
    HomeContext
  );
  const { bottomSection, setBottomSection } = useContext(
    MapLayersManageContext
  );
  const theme = useTheme();
  const classes = {
    ...useManageLayerCommonStyles(theme),
    ...useStyles(theme),
  };
  const updateCurrentLayer = useUpdateCurrentLayerState();

  const hasColorRenderer = (editingUserMap?.mapRenderers ?? []).some((mr) => {
    return (
      mr.rendererRules.layer.id === currentLayer?.rendererRules.layer.id &&
      !!mr.rendererRules.colorRule
    );
  });

  const hasTextRenderer = (editingUserMap?.mapRenderers ?? []).some((mr) => {
    return (
      mr.rendererRules.layer.id === currentLayer?.rendererRules.layer.id &&
      !!mr.rendererRules.labels
    );
  });

  const addTextRenderer = (): void => {
    if (!editingUserMap || !editingUserMap.mapRenderers) return;
    if (!currentLayer) return;
    updateCurrentLayer((layer) => ({
      ...layer,
      rendererRules: {
        ...layer.rendererRules,
        labels: [],
      },
    }));
    setBottomSection('labelRenderer');
  };

  const onDeleteTextRenderer = (): void => {
    if (!editingUserMap || !editingUserMap.mapRenderers) return;
    if (!currentLayer) return;
    updateCurrentLayer((layer) => {
      delete layer.rendererRules.labels;
      return layer;
    });
  };
  return (
    <Box className={clsx(classes.root)}>
      {currentLayer?.rendererRules.layer &&
        ['fill', 'circle'].includes(currentLayer?.rendererRules.layer.type) && (
          <Box style={{ marginBottom: theme.spacing(3) }}>
            <PanelTitle topLine={false} bottomLine={false}>
              Color Renderer
            </PanelTitle>
            {!hasColorRenderer && (
              <Box>
                <HighlightButton
                  onClick={(): void => {
                    if (!editingUserMap || !editingUserMap.mapRenderers) return;
                    if (!currentLayer) return;
                    setColorRuleAction('create');
                    const id = 'color-renderer-' + uuidV4();
                    const sourceLayer =
                      layerService.layersConfiguration[
                        currentLayer.rendererRules.layer.id
                      ].defaultMapboxLayer;
                    const newColorRule: MapRendererRules['colorRule'] = {
                      layer: {
                        ...sourceLayer,
                        id,
                        minzoom: currentLayer.rendererRules.layer.minzoom,
                        maxzoom: currentLayer.rendererRules.layer.maxzoom,
                        metadata: {
                          isSystemRenderer: isInSystem(editingUserMap),
                          scaleMinZoom:
                            currentLayer.rendererRules.layer.metadata
                              ?.scaleMinZoom ?? DEFAULT_MIN_ZOOM,
                          scaleMaxZoom:
                            currentLayer.rendererRules.layer.metadata
                              ?.scaleMaxZoom ?? DEFAULT_MAX_ZOOM,
                        },
                      },
                      rule: new RendererSimpleRule(),
                    };
                    updateCurrentLayer((layer) => ({
                      ...layer,
                      rendererRules: {
                        ...layer.rendererRules,
                        colorRule: newColorRule,
                      },
                    }));
                    setBottomSection('colorRenderer');
                  }}
                >
                  Add Color Renderer
                </HighlightButton>
              </Box>
            )}
            {hasColorRenderer && (
              <Box>
                <HighlightButton
                  onClick={(): void => {
                    setColorRuleAction('update');
                    if (bottomSection === 'colorRenderer') {
                      setBottomSection(undefined);
                    } else {
                      setBottomSection('colorRenderer');
                    }
                  }}
                  isSelected={bottomSection === 'colorRenderer'}
                  style={{ marginRight: theme.spacing(3) }}
                >
                  Edit
                </HighlightButton>
                <HighlightButton
                  onClick={(): void => {
                    updateCurrentLayer((layer) => ({
                      ...layer,
                      datasetId: undefined,
                      rendererRules: {
                        ...layer.rendererRules,
                        colorRule: undefined,
                      },
                    }));
                    setBottomSection(undefined);
                  }}
                >
                  Delete
                </HighlightButton>
              </Box>
            )}
          </Box>
        )}

      {currentLayer?.rendererRules.layer &&
        ['fill', 'circle'].includes(currentLayer?.rendererRules.layer.type) && (
          <Box>
            <PanelTitle topLine={false} bottomLine={false}>
              Text Renderer
            </PanelTitle>
            {!hasTextRenderer && (
              <Box>
                <HighlightButton onClick={addTextRenderer}>
                  Add Text Renderer
                </HighlightButton>
              </Box>
            )}
            {hasTextRenderer && (
              <Box>
                <HighlightButton
                  onClick={(): void => {
                    if (bottomSection === 'labelRenderer') {
                      setBottomSection(undefined);
                    } else {
                      setBottomSection('labelRenderer');
                    }
                  }}
                  isSelected={bottomSection === 'labelRenderer'}
                  style={{ marginRight: theme.spacing(3) }}
                >
                  Edit
                </HighlightButton>
                <HighlightButton onClick={onDeleteTextRenderer}>
                  Delete
                </HighlightButton>
              </Box>
            )}
          </Box>
        )}
    </Box>
  );
};

export default LayerRenderers;
