/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { v4 as uuidV4 } from 'uuid';
import { ColorConfiguration } from '@ptas/react-ui-library';
import { useContext } from 'react';
import { apiService } from 'services/api';
import { ClassBreakRuleColorRange, RendererClassBreakRule } from 'services/map';
import { RuleContext } from '../../Context';
import { RGBA_BLACK } from 'appConstants';

type USeCalcQuantileRanges = (
  ruleId: string,
  initialClassRule: RendererClassBreakRule,
  fillRamp: ColorConfiguration | undefined
) => Promise<void>;

const genRuleId = (
  ds: string,
  colName: string,
  numberOfBreaks: number
): string => ['quantile', ds, colName, numberOfBreaks].join('-');

export const useCalcQuantileRanges = (): USeCalcQuantileRanges => {
  const { numberOfBreaks, datasetField, setRule, datasetId } = useContext(
    RuleContext
  );

  return async (
    ruleId: string,
    initialClassRule: RendererClassBreakRule,
    fillRamp: ColorConfiguration | undefined
  ): Promise<void> => {
    if (!datasetId) return;
    if (!datasetField) return;
    const newRuleId = genRuleId(
      datasetId,
      datasetField.columnName,
      numberOfBreaks
    );
    if (ruleId === newRuleId) return;
    const { data, errorMessage } = await apiService.getQuantileBreaks(
      datasetId,
      datasetField.columnName,
      numberOfBreaks
    );
    if (errorMessage) {
      throw new Error('Error getting quantile breaks');
    }

    if (data?.length) {
      setRule((prev) => {
        if (!(prev instanceof RendererClassBreakRule)) {
          prev = initialClassRule;
        }
        const newColorRanges = prev.colorRanges;
        if (prev.colorRanges.length < numberOfBreaks) {
          for (let i = prev.colorRanges.length; i < numberOfBreaks; i++) {
            newColorRanges.push(
              new ClassBreakRuleColorRange({
                id: uuidV4(),
                fill: fillRamp?.colors?.[i] ?? RGBA_BLACK,
                outline: RGBA_BLACK,
                from: 0,
                to: 0,
              })
            );
          }
        }

        newColorRanges.forEach((colorRange, i) => {
          if (data?.[i + 1] === undefined) {
            newColorRanges.length = i;
            return;
          }
          colorRange.from = data?.[i] ?? 0;
          colorRange.to = data?.[i + 1] ?? 0;
        });

        return new RendererClassBreakRule({
          ...prev,
          id: newRuleId,
          colorRanges: newColorRanges,
        });
      });
    }
  };
};
