// useUniqueValuesRule.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { useUpdateEffect } from 'react-use';
import { ColorConfiguration, getDefaultRamp } from '@ptas/react-ui-library';
import { useContext, useEffect, useRef } from 'react';
import { RuleContext } from '../Context';
import { HomeContext } from 'contexts';
import {
  RendererUniqueValuesRule,
  UniqueValuesRuleValueColor,
} from 'services/map/model';

export const useUniqueValuesRule = (): void => {
  const {
    ruleType,
    datasetField,
    setRule,
    dataSetColumnValues,
    selectedRampTab,
    rule,
    colorConfig,
    setColorConfig,
  } = useContext(RuleContext);
  const { userColorRamps, colorRuleAction } = useContext(HomeContext);
  const ruleRef = useRef<RendererUniqueValuesRule | undefined>();
  const colorsHistory = useRef<Map<number, UniqueValuesRuleValueColor>>(
    new Map()
  );

  useEffect(() => {
    if (!dataSetColumnValues?.length) return;
    if (ruleType !== 'Unique') return;
    if (rule && rule instanceof RendererUniqueValuesRule) {
      ruleRef.current = rule;
    } else {
      ruleRef.current = new RendererUniqueValuesRule();
    }
    if (colorRuleAction === 'create') {
      if (
        !ruleRef.current?.selectedFillRampId &&
        !ruleRef.current?.selectedOutlineRampId
      ) {
        setColorConfig((prev) => ({
          colorConfigFill: getDefaultRamp(dataSetColumnValues?.length),
          colorConfigOutline: prev.colorConfigOutline,
          count: prev.count++,
        }));
        return;
      } else {
        setColorConfig((prev) => {
          //'Fill' color ramp
          let colorConfigFill: ColorConfiguration | undefined;
          if (ruleRef.current?.selectedFillRampId) {
            //Search in user color ramps
            let ramp = userColorRamps.find(
              (ramp) => ramp.id === ruleRef.current?.selectedFillRampId
            );
            if (!ramp) {
              //Search in default ramps
              ramp = getDefaultRamp(
                dataSetColumnValues?.length,
                ruleRef.current?.selectedFillRampId
              );
            }
            if (ramp) colorConfigFill = ramp;
          }

          //'Outline' color ramp
          let colorConfigOutline: ColorConfiguration | undefined;
          if (ruleRef.current?.selectedOutlineRampId) {
            //Search in user color ramps
            let ramp = userColorRamps.find(
              (ramp) => ramp.id === ruleRef.current?.selectedOutlineRampId
            );
            if (!ramp) {
              //Search in default ramps
              ramp = getDefaultRamp(
                dataSetColumnValues?.length,
                ruleRef.current?.selectedOutlineRampId
              );
            }
            if (ramp) colorConfigOutline = ramp;
          }

          return {
            colorConfigFill,
            colorConfigOutline,
            count: prev.count++,
          };
        });
        return;
      }
    } else {
      if (
        !ruleRef.current?.selectedFillRampId &&
        !ruleRef.current?.selectedOutlineRampId
      ) {
        setColorConfig((prev) => ({
          colorConfigFill: getDefaultRamp(dataSetColumnValues?.length),
          colorConfigOutline: undefined,
          count: prev.count++,
        }));
        return;
      } else {
        //Check if rule's ramp IDs exist in user ramps
        if (
          ruleRef.current.selectedFillRampId ||
          ruleRef.current.selectedOutlineRampId
        ) {
          setColorConfig((prev) => {
            //'Fill' color ramp
            let colorConfigFill: ColorConfiguration | undefined;
            if (ruleRef.current?.selectedFillRampId) {
              //Search in user color ramps
              let ramp = userColorRamps.find(
                (ramp) => ramp.id === ruleRef.current?.selectedFillRampId
              );
              if (!ramp) {
                //Search in default ramps
                ramp = getDefaultRamp(
                  dataSetColumnValues?.length,
                  ruleRef.current?.selectedFillRampId
                );
              }
              if (ramp) colorConfigFill = ramp;
            }

            //'Outline' color ramp
            let colorConfigOutline: ColorConfiguration | undefined;
            if (ruleRef.current?.selectedOutlineRampId) {
              //Search in user color ramps
              let ramp = userColorRamps.find(
                (ramp) => ramp.id === ruleRef.current?.selectedOutlineRampId
              );
              if (!ramp) {
                //Search in default ramps
                ramp = getDefaultRamp(
                  dataSetColumnValues?.length,
                  ruleRef.current?.selectedOutlineRampId
                );
              }
              if (ramp) colorConfigOutline = ramp;
            }

            return {
              colorConfigFill,
              colorConfigOutline,
              count: prev.count++,
            };
          });
        } else {
          setColorConfig((prev) => ({
            colorConfigFill: undefined,
            colorConfigOutline: undefined,
            count: prev.count++,
          }));
        }
        return;
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [colorRuleAction, dataSetColumnValues, ruleType]);

  useEffect(() => {
    if (ruleType !== 'Unique') return;
    if (!dataSetColumnValues?.length) return;
    if (typeof datasetField === 'undefined') return;

    let colors: UniqueValuesRuleValueColor[] = [];
    if (ruleRef.current instanceof RendererUniqueValuesRule) {
      //Set colors from 1) ramp, 2) history, 3) current rule, 4) initialize in transparent/black
      colors = dataSetColumnValues.map((value, i) => {
        const fillColor =
          colorConfig?.colorConfigFill?.colors &&
          colorConfig.colorConfigFill.colors.length > i
            ? colorConfig.colorConfigFill.colors[i]
            : colorsHistory.current.get(i)
            ? colorsHistory.current.get(i)?.fill
            : ruleRef.current && ruleRef.current.colors.length > i
            ? ruleRef.current.colors[i].fill
            : undefined;
        const outlineColor =
          colorConfig?.colorConfigOutline?.colors &&
          colorConfig.colorConfigOutline.colors.length > i
            ? colorConfig.colorConfigOutline.colors[i]
            : colorsHistory.current.get(i)
            ? colorsHistory.current.get(i)?.outline
            : ruleRef.current && ruleRef.current.colors.length > i
            ? ruleRef.current.colors[i].outline
            : undefined;

        return new UniqueValuesRuleValueColor({
          columnValue: value,
          columnDescription: '' + value,
          fill: fillColor,
          outline: outlineColor,
          tabIndex:
            ruleRef.current && ruleRef.current.colors.length > i
              ? ruleRef.current.colors[i].tabIndex
              : 0,
        });
      });
    } else {
      colors = dataSetColumnValues.map((value) => {
        return new UniqueValuesRuleValueColor({
          columnValue: value,
          columnDescription: '' + value,
          tabIndex: 0,
        });
      });
    }

    const newRule = new RendererUniqueValuesRule({
      datasetId: datasetField.datasetId,
      columnName: datasetField.columnName,
      columnType: datasetField.columnType,
      colors: colors,
      selectedFillRampId: colorConfig?.colorConfigFill
        ? colorConfig.colorConfigFill.id
        : // : rule instanceof RendererUniqueValuesRule //TODO: check if necessary (should set only if colorConfig received)
          // ? rule.selectedFillRampId
          undefined,
      fillOpacity: colorConfig?.colorConfigFill?.opacity ?? 1,
      selectedOutlineRampId: colorConfig?.colorConfigOutline
        ? colorConfig.colorConfigOutline.id
        : // : rule instanceof RendererUniqueValuesRule //TODO: check if necessary (should set only if colorConfig received)
          // ? rule.selectedOutlineRampId
          undefined,
      outlineOpacity: 1,
    });
    //Update colors history
    newRule.colors.forEach((item, index) => {
      colorsHistory.current.set(index, item);
    });

    setRule(newRule);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [colorConfig]);

  useUpdateEffect(() => {
    if (selectedRampTab === undefined) return;

    setRule((prevRule) => {
      if (prevRule instanceof RendererUniqueValuesRule) {
        const newColors = prevRule.colors.map((color) => {
          return new UniqueValuesRuleValueColor({
            ...color,
            tabIndex: selectedRampTab,
          });
        });
        const newRule = new RendererUniqueValuesRule({
          ...prevRule,
          colors: newColors,
        });
        return newRule;
      }
      return prevRule;
    });
  }, [selectedRampTab]);
};
