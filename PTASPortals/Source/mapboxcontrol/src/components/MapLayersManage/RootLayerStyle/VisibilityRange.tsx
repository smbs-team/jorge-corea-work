/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { useContext, useEffect, useState } from 'react';
import { Box, Theme } from '@material-ui/core';
import { useTheme } from '@material-ui/core/styles';
import { CustomTextField } from '@ptas/react-ui-library';
import { useGlobalStyles } from 'hooks';
import { useManageLayerCommonStyles } from '../styles';
import { scaleToMapZoom } from 'utils/zoom';
import { makeStyles } from '@material-ui/core';
import { createStyles } from '@material-ui/core';
import { useUpdateCurrentLayerState } from 'components/MapLayersManage/useUpdateEditingMapLayersState';
import { HomeContext } from 'contexts';
import { useDebounce } from 'react-use';
import { DEFAULT_MAX_ZOOM, DEFAULT_MIN_ZOOM } from 'appConstants';
import { MapLayersManageContext } from '../Context';

const useStyles = makeStyles((theme: Theme) =>
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
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
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

const VisibilityRange = (): JSX.Element => {
  const { currentLayer } = useContext(HomeContext);
  const { isSystemRenderer } = useContext(MapLayersManageContext);
  const theme = useTheme();
  const classes = {
    ...useGlobalStyles(theme),
    ...useStyles(theme),
    ...useManageLayerCommonStyles(theme),
  };
  const updateCurrentLayer = useUpdateCurrentLayerState();

  const [min, setMin] = useState<number>(0);
  const [max, setMax] = useState<number>(0);

  useEffect(() => {
    setMin(
      currentLayer?.rendererRules.layer.metadata?.scaleMinZoom ??
        DEFAULT_MIN_ZOOM
    );
    setMax(
      currentLayer?.rendererRules.layer.metadata?.scaleMaxZoom ??
        DEFAULT_MAX_ZOOM
    );
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentLayer?.rendererRules.layer.id]);

  useDebounce(
    () => {
      updateCurrentLayer((layer) => {
        const convertedMin = scaleToMapZoom(min);
        const convertedMax = scaleToMapZoom(max);
        const minZoom =
          convertedMin >= 0 && convertedMin <= 22 ? convertedMin : 0;
        const maxZoom = convertedMax <= 22 && convertedMax ? convertedMax : 22;
        return {
          ...layer,
          rendererRules: {
            ...layer.rendererRules,
            layer: {
              ...layer.rendererRules.layer,
              minzoom: minZoom,
              maxzoom: maxZoom,
              metadata: {
                ...(layer.rendererRules.layer.metadata ?? {
                  isSystemRenderer,
                  checked: true,
                }),
                scaleMinZoom: min,
                scaleMaxZoom: max,
              },
            },
            colorRule: !layer.rendererRules.colorRule
              ? layer.rendererRules.colorRule
              : {
                  ...layer.rendererRules.colorRule,
                  layer: {
                    ...layer.rendererRules.colorRule.layer,
                    minzoom: minZoom,
                    maxzoom: maxZoom,
                    metadata: {
                      ...(layer.rendererRules.colorRule.layer.metadata ?? {
                        isSystemRenderer,
                        checked: true,
                      }),
                      scaleMinZoom: min,
                      scaleMaxZoom: max,
                    },
                  },
                },
            nativeMapboxLayers: layer.rendererRules.nativeMapboxLayers.map(
              (nativeLayer) => {
                return {
                  ...nativeLayer,
                  minzoom: minZoom,
                  maxzoom: maxZoom,
                };
              }
            ),
          },
        };
      });
      // eslint-disable-next-line react-hooks/exhaustive-deps
    },
    100,
    [min, max]
  );

  const handleMinChange = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const val = parseInt(e.target.value);
    setMin(val);
  };

  const handleMaxChange = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const val = parseInt(e.target.value);
    setMax(val);
  };

  return (
    <Box className={`${classes.root} ${classes.marginTop}`}>
      <Box className={classes.columnFlexBox}>
        <Box>
          <Box className={classes.flexboxRow}>
            <CustomTextField
              label="Min scale"
              type="number"
              value={max}
              className={classes.scale}
              onChange={handleMaxChange}
            />
            <span className={classes.and}>and</span>
            <CustomTextField
              label="Max scale"
              type="number"
              value={min}
              className={classes.scale}
              onChange={handleMinChange}
            />
          </Box>
        </Box>
      </Box>
    </Box>
  );
};

export default VisibilityRange;
