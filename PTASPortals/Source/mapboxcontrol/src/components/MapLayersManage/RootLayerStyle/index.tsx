/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  Box,
  makeStyles,
  Theme,
  useTheme,
  createStyles,
} from '@material-ui/core';
import clsx from 'clsx';
import {
  ColorPicker,
  CustomTabs,
  CustomTextField,
} from '@ptas/react-ui-library';
import { useRootLayerStyle } from './useRootLayerStyle';
import { PanelBody, PanelTitle } from 'components/common/panel';
import VisibilityRange from './VisibilityRange';
import { MapRendererRules } from 'services/map';
import { useLineWidth } from './useLineWidth';
import { useCircleRadius } from './useCircleRadius';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      marginBottom: theme.spacing(1),
      fontFamily: theme.ptas.typography.bodyFontFamily,
      marginTop: theme.spacing(2),
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
      display: 'inline-block',
      marginRight: 12,
    },
    line: {
      width: 71,
    },
    marginRight: {
      marginRight: theme.spacing(1),
    },

    colorField: {
      width: 130,
    },
  })
);

type Props = {
  rendererRules: MapRendererRules;
};

function RootLayerStyle({ rendererRules }: Props): JSX.Element | null {
  const theme = useTheme();
  const classes = useStyles(theme);
  const { layer } = rendererRules;
  const {
    applyLayerColors,
    getColorValue,
    setTabIndex,
    tabIndex,
    tabItems,
  } = useRootLayerStyle();
  const [lineWidth, setLineWidth] = useLineWidth();
  const [circleRadius, setCircleRadius] = useCircleRadius();
  return (
    <PanelBody>
      <Box className={clsx(classes.root, classes.columnFlexBox)}>
        {layer.type !== 'raster' && (
          <Box className={classes.rootChild}>
            <Box className={classes.rowFlexBox}>
              <ColorPicker
                onChangeComplete={(color): void => {
                  applyLayerColors(color.rgbaStr);
                }}
                onInputChange={(rgbaStr): void => {
                  applyLayerColors(rgbaStr);
                }}
                rgbColor={getColorValue()}
                showHexInput
                label="Color"
                classes={{
                  root: classes.marginRight,
                  textField: classes.colorField,
                }}
              />
              <Box className={classes.rowFlexBox}>
                <CustomTabs
                  selectedIndex={tabIndex}
                  switchVariant
                  items={tabItems}
                  onSelected={(e: number): void => {
                    setTabIndex(e);
                  }}
                />
              </Box>
            </Box>
          </Box>
        )}
        {['line', 'circle'].includes(layer.type) && (
          <Box className={classes.rootChild}>
            <Box className={classes.dropDownWrap}>
              <CustomTextField
                label="Line"
                type="number"
                classes={{ root: classes.line }}
                value={lineWidth ?? ''}
                onChange={(e): void => {
                  const numValue = Number.parseFloat(e.target.value);
                  if (isNaN(numValue)) {
                    setLineWidth(undefined);
                  } else {
                    setLineWidth(numValue);
                  }
                }}
              />
            </Box>
            {layer.type === 'circle' && (
              <Box className={classes.dropDownWrap}>
                <CustomTextField
                  label="Radius"
                  type="number"
                  classes={{ root: classes.line }}
                  value={circleRadius ?? ''}
                  onChange={(e): void => {
                    const numValue = Number.parseFloat(e.target.value);
                    if (isNaN(numValue)) {
                      setCircleRadius(undefined);
                    } else {
                      setCircleRadius(numValue);
                    }
                  }}
                />
              </Box>
            )}
          </Box>
        )}
      </Box>
      <PanelTitle topLine={false} bottomLine={false}>
        Visible Range
      </PanelTitle>
      <PanelBody>
        <VisibilityRange />
      </PanelBody>
    </PanelBody>
  );
}

export default RootLayerStyle;
