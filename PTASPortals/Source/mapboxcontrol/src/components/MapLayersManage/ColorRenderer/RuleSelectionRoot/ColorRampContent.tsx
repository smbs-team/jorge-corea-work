// ColorRampContent
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, Fragment } from 'react';
import {
  Box,
  makeStyles,
  createStyles,
  useTheme,
  Theme,
} from '@material-ui/core';
import ColorRampPanel from '../common/ColorRampPanel';
import { RuleContext } from '../Context';
import { ColorConfiguration } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import { RendererUniqueValuesRule } from 'services/map/model';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      display: 'flex',
      flex: '1',
    },
  })
);

const ColorRampContent = (): JSX.Element => {
  const classes = useStyles(useTheme());
  const {
    setSelectedRampTab,
    dataSetColumnValues,
    setColorConfig,
    rule,
    selectedRampTab,
  } = useContext(RuleContext);
  const { userColorRamps, setUserColorRamps } = useContext(HomeContext);

  const updateColorConfigOnUserColorRamps = (
    newConfig: ColorConfiguration
  ): void => {
    setUserColorRamps((prev) => {
      return prev.map((colorConfig) => {
        if (colorConfig.id === newConfig.id) {
          return newConfig;
        }
        return colorConfig;
      });
    });
  };

  const onColorRampUpdated = (newConfig: ColorConfiguration): void => {
    updateColorConfigOnUserColorRamps(newConfig);

    if (
      newConfig.id === (rule as RendererUniqueValuesRule).selectedFillRampId ||
      newConfig.id === (rule as RendererUniqueValuesRule).selectedOutlineRampId
    ) {
      setColorConfig &&
        setColorConfig((prev) => {
          let colorConfigFill = prev.colorConfigFill;
          let colorConfigOutline = prev.colorConfigOutline;

          if (
            newConfig.id ===
            (rule as RendererUniqueValuesRule).selectedFillRampId
          ) {
            colorConfigFill = newConfig;
          }
          if (
            newConfig.id ===
            (rule as RendererUniqueValuesRule).selectedOutlineRampId
          ) {
            colorConfigOutline = newConfig;
          }

          return {
            colorConfigFill,
            colorConfigOutline,
            count: prev.count++,
          };
        });
    }
  };

  return (
    <Fragment>
      {rule && (
        <Box className={classes.root}>
          <ColorRampPanel
            title="Automatic color ramp"
            colorsToGenerate={dataSetColumnValues?.length ?? 0}
            opacity={(rule as RendererUniqueValuesRule).fillOpacity}
            selectedFillRampId={
              (rule as RendererUniqueValuesRule).selectedFillRampId
            }
            selectedOutlineRampId={
              (rule as RendererUniqueValuesRule).selectedOutlineRampId
            }
            switchContent={['Fill', 'Outline']}
            switchOnChange={(index): void => {
              if (!setSelectedRampTab) return;
              setSelectedRampTab(index);
            }}
            onSelect={(colorConfig: ColorConfiguration): void => {
              updateColorConfigOnUserColorRamps(colorConfig);
              if (!selectedRampTab) {
                setColorConfig &&
                  setColorConfig((prev) => ({
                    colorConfigFill: colorConfig,
                    colorConfigOutline: prev.colorConfigOutline,
                    count: prev.count++,
                  }));
              } else {
                setColorConfig &&
                  setColorConfig((prev) => ({
                    colorConfigFill: prev.colorConfigFill,
                    colorConfigOutline: colorConfig,
                    count: prev.count++,
                  }));
              }
            }}
            onCreate={(newRamp: ColorConfiguration): void => {
              setUserColorRamps((prev) => {
                if (prev) return [...prev, newRamp];
                return [newRamp];
              });
              //Select created ramp
              if (!selectedRampTab) {
                setColorConfig &&
                  setColorConfig((prev) => ({
                    colorConfigFill: newRamp,
                    colorConfigOutline: prev.colorConfigOutline,
                    count: prev.count++,
                  }));
              } else {
                setColorConfig((prev) => ({
                  colorConfigFill: prev.colorConfigFill,
                  colorConfigOutline: newRamp,
                  count: prev.count++,
                }));
              }
            }}
            onDelete={(deletingRamp): void => {
              setUserColorRamps((prev) => {
                return prev.filter((ramp) => ramp.id !== deletingRamp.id);
              });
            }}
            onEdit={(newConfig): void => {
              onColorRampUpdated(newConfig);
            }}
            ramps={userColorRamps}
          />
        </Box>
      )}
    </Fragment>
  );
};

export default ColorRampContent;
