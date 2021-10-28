// useClassbreak.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useContext, useEffect, useState } from 'react';
import { v4 as uuidV4 } from 'uuid';
import { RuleContextProps } from '../Context';
import { useDebounce } from 'react-use';
import {
  ColorConfiguration,
  ErrorMessageAlertCtx,
  getColors,
  getDefaultRamp,
} from '@ptas/react-ui-library';
import { RGBA_BLACK } from 'appConstants';
import { apiService } from 'services/api';
import {
  BreakOptionKey,
  ClassBreakRuleColorRange,
  RendererClassBreakRule,
} from 'services/map/model';
import { SnackContext } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import { useCalcQuantileRanges } from './calcRanges/useCalcQuantileRanges';
import { getErrorStr } from 'utils/getErrorStr';

const getEqIntervalRuleId = (
  ds: string,
  colName: string,
  breaks: number
): string => ['eq', 'interval', ds, colName, breaks].join('-');

const getStDeviationRuleId = (
  ds: string,
  colName: string,
  stDeviationInterval: number
): string => ['st', 'deviation', ds, colName, stDeviationInterval].join('-');

const useClassbreak = (ruleContext: RuleContextProps): void => {
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const { setLinearProgress, userColorRamps } = useContext(HomeContext);
  const {
    numberOfBreaks,
    ruleType,
    breakOption,
    datasetField,
    setRule,
    datasetId,
    stDeviationInterval,
    colorConfig,
    rule,
  } = ruleContext;
  const { setSnackState } = useContext(SnackContext);
  const calcQuantileRanges = useCalcQuantileRanges();
  const [classRuleId, setClassRuleId] = useState(
    rule instanceof RendererClassBreakRule ? rule.id : '0'
  );
  const fillRampId =
    rule instanceof RendererClassBreakRule
      ? rule.selectedFillRampId
      : undefined;

  const genColorRangeValues = (
    rule: RendererClassBreakRule,
    numberOfBreaks: number,
    min: number,
    max: number
  ): ClassBreakRuleColorRange[] => {
    const step = (max - min) / numberOfBreaks;
    if (step < 1) {
      setSnackState({
        severity: 'info',
        text: `Invalid number of breaks for selected field. Max: ${max - min}`,
      });
      return [];
    }
    return [...Array(numberOfBreaks).keys()].map((_, i) => {
      const rounded = Math.floor(step);
      return new ClassBreakRuleColorRange({
        id: rule.colorRanges[i]?.id ?? uuidV4(),
        fill: rule.colorRanges[i]?.fill,
        outline: rule.colorRanges[i]?.outline,
        tabIndex: rule.colorRanges[i]?.tabIndex ?? 0,
        from: i === 0 ? min : min + i * rounded,
        to: i === numberOfBreaks - 1 ? max : min + (1 + i) * rounded,
      });
    });
  };

  useDebounce(
    () => {
      if (ruleType !== 'Class') return;
      if (!numberOfBreaks) return;
      if (typeof breakOption === 'undefined') return;
      if (typeof datasetField === 'undefined') return;
      if (!datasetId) return;

      const getInitialClassRule = (): RendererClassBreakRule => {
        const defRamp = getDefaultRamp(numberOfBreaks);
        return new RendererClassBreakRule({
          id: classRuleId,
          breakOption,
          colorRanges: [...Array(numberOfBreaks).keys()].map(
            (_, i) =>
              new ClassBreakRuleColorRange({
                from: 0,
                to: 0,
                id: uuidV4(),
                fill: defRamp?.colors?.[i],
              })
          ),
          selectedFillRampId: defRamp?.id,
          stDeviationInterval,
          columnName: datasetField.columnName,
          columnType: datasetField.columnType,
          fillOpacity: defRamp?.opacity ?? 1,
          outlineOpacity: 1,
          datasetId: datasetField.datasetId,
        });
      };

      const getFillRamp = (
        fillRampId?: string
      ): ColorConfiguration | undefined =>
        userColorRamps.find((ramp) => ramp.id === fillRampId) ??
        getDefaultRamp(numberOfBreaks);

      const calculateRanges = async (): Promise<void> => {
        try {
          setLinearProgress(true);
          switch (breakOption) {
            /** Equal interval **/
            case BreakOptionKey.equalInterval: {
              const newRuleId = getEqIntervalRuleId(
                datasetId,
                datasetField.columnName,
                numberOfBreaks
              );
              if (classRuleId === newRuleId) return;
              const { data, errorMessage } = await apiService.getRangeValues(
                datasetId,
                datasetField.columnName
              );

              if (errorMessage) {
                throw new Error(
                  'There was an error while fetching range values'
                );
              }
              if (data?.length) {
                setRule((prev) => {
                  if (!(prev instanceof RendererClassBreakRule)) {
                    prev = getInitialClassRule();
                  }

                  const retVal = new RendererClassBreakRule({
                    ...prev,
                    id: newRuleId,
                    colorRanges: genColorRangeValues(
                      prev,
                      numberOfBreaks,
                      data[0] ?? 0,
                      data[1] ?? 0
                    ),
                  });
                  /**
                   * When the new color break size is greater thant previous
                   */
                  if (prev.colorRanges.length < retVal.colorRanges.length) {
                    const fillRamp = getFillRamp(retVal.selectedFillRampId);

                    for (
                      let i = prev.colorRanges.length;
                      i < retVal.colorRanges.length;
                      i++
                    ) {
                      if (fillRamp?.colors?.[i]) {
                        retVal.colorRanges[i].fill = fillRamp.colors[i];
                      }
                    }
                  }

                  return retVal;
                });
              }
              break;
            }
            /** Custom */
            case BreakOptionKey.custom: {
              setRule((prev) => {
                if (!(prev instanceof RendererClassBreakRule)) {
                  prev = getInitialClassRule();
                }
                const newColorRanges = prev.colorRanges;
                if (prev.colorRanges.length > numberOfBreaks) {
                  newColorRanges.length = numberOfBreaks;
                } else if (prev.colorRanges.length < numberOfBreaks) {
                  const defRamp = getDefaultRamp(numberOfBreaks);
                  //Add new color ranges
                  for (
                    let i = prev.colorRanges.length;
                    i < numberOfBreaks;
                    i++
                  ) {
                    newColorRanges.push(
                      new ClassBreakRuleColorRange({
                        id: uuidV4(),
                        fill: defRamp?.colors?.[i] ?? RGBA_BLACK,
                        outline: RGBA_BLACK,
                        from: 0,
                        to: 0,
                      })
                    );
                  }
                }
                return new RendererClassBreakRule({
                  ...prev,
                  id: [
                    'custom',
                    datasetField.datasetId,
                    datasetField.columnName,
                    numberOfBreaks,
                  ].join('-'),
                  colorRanges: newColorRanges,
                });
              });
              break;
            }
            /** Quantile */
            case BreakOptionKey.quantile: {
              await calcQuantileRanges(
                classRuleId,
                getInitialClassRule(),
                getFillRamp(fillRampId)
              );
              break;
            }
            /** St deviation */
            case BreakOptionKey.stDeviation: {
              const newRuleId = getStDeviationRuleId(
                datasetId,
                datasetField.columnName,
                stDeviationInterval
              );
              if (classRuleId === newRuleId) return;
              const { data, errorMessage } = await apiService.getStDeviation(
                datasetId,
                datasetField.columnName,
                stDeviationInterval
              );
              if (errorMessage) {
                throw new Error('Error getting st deviation');
              }
              if (data?.length) {
                setRule((prev) => {
                  const newRule =
                    prev instanceof RendererClassBreakRule
                      ? new RendererClassBreakRule({ ...prev })
                      : getInitialClassRule();
                  newRule.id = newRuleId;
                  const fillRamp =
                    userColorRamps.find(
                      (ramp) => ramp.id === newRule.selectedFillRampId
                    ) ?? getDefaultRamp(data.length);

                  data.forEach((item, index) => {
                    if (index === data.length - 1) return;
                    if (!newRule?.colorRanges) return;
                    const [from, to] = [item, data[index + 1]];
                    if (newRule?.colorRanges[index]) {
                      newRule.colorRanges[index].from = from;
                      newRule.colorRanges[index].to = to;
                    } else {
                      newRule.colorRanges[index] = new ClassBreakRuleColorRange(
                        {
                          id: `${datasetId}-${datasetField.columnName}-${breakOption}-${index}`,
                          from: from,
                          to: to,
                          fill: fillRamp?.colors?.[index],
                        }
                      );
                    }
                  });
                  newRule.stDeviationInterval = stDeviationInterval;
                  newRule.colorRanges = newRule.colorRanges.slice(
                    0,
                    data.length - 1
                  );

                  return newRule;
                });
              }
              break;
            }
            default: {
              throw new Error('Invalid break option ' + breakOption);
            }
          }
        } catch (e) {
          showErrorMessage(getErrorStr(e));
          setRule((prev) => {
            if (!(prev instanceof RendererClassBreakRule)) return prev;
            return new RendererClassBreakRule({
              ...prev,
              id: breakOption + '-error',
              colorRanges: [],
            });
          });
        } finally {
          setLinearProgress(false);
        }
      };

      calculateRanges();

      setRule((prev) => {
        if (prev instanceof RendererClassBreakRule) {
          return new RendererClassBreakRule({
            ...prev,
            stDeviationInterval,
            breakOption,
            datasetId,
            columnName: datasetField.columnName,
            columnType: datasetField.columnType,
          });
        }
        return getInitialClassRule();
      });
    },
    75,
    [
      numberOfBreaks,
      stDeviationInterval,
      breakOption,
      datasetField,
      ruleType,
      datasetId,
      classRuleId,
      fillRampId,
    ]
  );

  /** Set color ramp colors */
  useEffect(() => {
    if (!colorConfig) return;
    if (!colorConfig.colorConfigFill && !colorConfig.colorConfigOutline) return;
    setRule((prev) => {
      if (prev instanceof RendererClassBreakRule) {
        const fillColors = colorConfig?.colorConfigFill
          ? getColors(
              colorConfig.colorConfigFill,
              prev.colorRanges.length ?? 0,
              colorConfig.colorConfigFill.opacity
            )
          : undefined;

        const outlineColors = colorConfig?.colorConfigOutline
          ? getColors(
              colorConfig.colorConfigOutline,
              prev.colorRanges.length ?? 0,
              colorConfig.colorConfigOutline.opacity
            )
          : undefined;

        return new RendererClassBreakRule({
          ...prev,
          fillOpacity:
            colorConfig?.colorConfigFill?.opacity ?? prev.fillOpacity,
          outlineOpacity:
            colorConfig?.colorConfigOutline?.opacity ?? prev.outlineOpacity,
          selectedFillRampId:
            colorConfig?.colorConfigFill?.id ?? prev.selectedFillRampId,
          selectedOutlineRampId:
            colorConfig?.colorConfigOutline?.id ?? prev.selectedOutlineRampId,
          colorRanges: prev.colorRanges.map(
            (colorRange, i) =>
              new ClassBreakRuleColorRange({
                ...colorRange,
                fill: fillColors?.[i] ?? colorRange.fill,
                outline: outlineColors?.[i] ?? colorRange.outline,
              })
          ),
        });
      }
      return prev;
    });
  }, [colorConfig, setRule]);

  useEffect(() => {
    if (rule instanceof RendererClassBreakRule) {
      setClassRuleId(rule.id);
    }
  }, [rule]);
};

export default useClassbreak;
