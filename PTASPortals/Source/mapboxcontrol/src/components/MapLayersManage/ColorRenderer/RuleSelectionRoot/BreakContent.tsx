// BreakElements
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
import {
  SimpleDropDown,
  DropDownItem,
  CustomTextField as TextField,
  ColorRampPanelProps,
  ColorConfiguration,
} from '@ptas/react-ui-library';
import ColorRampPanel from '../common/ColorRampPanel';
import { RuleContext } from '../Context';
import {
  BreakOptionKey,
  ClassBreakRuleColorRange,
  RendererClassBreakRule,
} from 'services/map/model';
import { useMount } from 'react-use';
import { HomeContext } from 'contexts';

type Props = { onSelectRamp?: ColorRampPanelProps['onSelect'] };

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      display: 'flex',
      flex: '1',
    },
    formControlsWrap: {
      display: 'flex',
      marginRight: theme.spacing(10 / 8),
    },
    breakOptionDropDown: {
      width: 140,
      marginRight: theme.spacing(10 / 8),
    },
    breaksTextField: {
      width: 80,
    },
  })
);

const breakOptionDropDownItems: DropDownItem[] = Object.entries(
  BreakOptionKey
).map(([, v]) => ({
  label: v,
  value: v,
}));

function BreakContent(): JSX.Element {
  const classes = useStyles(useTheme());
  const {
    numberOfBreaks,
    setNumberOfBreaks,
    breakOption,
    setBreakOption,
    setSelectedRampTab,
    setStDeviationInterval,
    stDeviationInterval,
    rule,
    setRule,
    selectedRampTab,
    setColorConfig,
  } = useContext(RuleContext);

  const { userColorRamps, setUserColorRamps } = useContext(HomeContext);

  useMount(() => {
    if (!breakOption) {
      setBreakOption(breakOptionDropDownItems[0].value as BreakOptionKey);
    }
  });

  const onColorRampUpdated = (newConfig: ColorConfiguration): void => {
    setUserColorRamps((prev) =>
      prev.map((colorConfig) => {
        if (colorConfig.id === newConfig.id) {
          return newConfig;
        }
        return colorConfig;
      })
    );

    if (
      newConfig.id === (rule as RendererClassBreakRule).selectedFillRampId ||
      newConfig.id === (rule as RendererClassBreakRule).selectedOutlineRampId
    ) {
      setRule((prev) => {
        if (prev instanceof RendererClassBreakRule) {
          const newRule = new RendererClassBreakRule({ ...prev });
          const newColorRanges: ClassBreakRuleColorRange[] = [];
          prev.colorRanges.forEach((colorRange, i) => {
            if (newConfig.colors && newConfig.colors.length > i) {
              const newColorRange: ClassBreakRuleColorRange = new ClassBreakRuleColorRange(
                {
                  ...colorRange,
                }
              );
              if (!selectedRampTab) {
                newColorRange.fill = newConfig.colors[i];
              } else {
                newColorRange.outline = newConfig.colors[i];
              }
              newColorRanges.push(newColorRange);
            }
          });
          newRule.colorRanges = newColorRanges;

          return newRule;
        }
        return prev;
      });
    }
  };

  return (
    <Fragment>
      {breakOption !== undefined && rule instanceof RendererClassBreakRule && (
        <Box className={classes.root}>
          <Box className={classes.formControlsWrap}>
            <SimpleDropDown
              classes={{
                root: classes.breakOptionDropDown,
              }}
              items={breakOptionDropDownItems}
              onSelected={(s): void => {
                setBreakOption(s.value as BreakOptionKey);
              }}
              label="Break option"
              titleTop
              value={breakOption}
            />

            {breakOption === BreakOptionKey.stDeviation &&
            stDeviationInterval !== undefined ? (
              <TextField
                key="st-deviation-text-field"
                type="number"
                label={'Interval'}
                classes={{ root: classes.breaksTextField }}
                value={stDeviationInterval > 0 ? stDeviationInterval : 0}
                onChange={(e): void => {
                  if (Number.isNaN(e.target.value)) {
                    setStDeviationInterval(0);
                    return;
                  }
                  setStDeviationInterval(+e.target.value);
                }}
              />
            ) : (
              <TextField
                key="break-option-text-field"
                type="number"
                label={'Breaks'}
                classes={{ root: classes.breaksTextField }}
                value={numberOfBreaks !== 0 ? numberOfBreaks : ''}
                onChange={(e): void => {
                  setNumberOfBreaks(+e.target.value);
                }}
              />
            )}
          </Box>
          <ColorRampPanel
            title="Automatic color ramp"
            colorsToGenerate={numberOfBreaks}
            opacity={(rule as RendererClassBreakRule).fillOpacity}
            switchContent={['Fill', 'Outline']}
            selectedFillRampId={
              (rule as RendererClassBreakRule).selectedFillRampId
            }
            selectedOutlineRampId={
              (rule as RendererClassBreakRule).selectedOutlineRampId
            }
            switchOnChange={(index): void => {
              setSelectedRampTab(index);
            }}
            onSelect={(colorConfig): void => {
              if (!selectedRampTab) {
                setColorConfig((prev) => ({
                  colorConfigFill: colorConfig,
                  colorConfigOutline: prev.colorConfigOutline,
                  count: prev.count++,
                }));
              } else {
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
}

export default BreakContent;
