// useSimpleRule.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import {
  StringOperatorId,
  NumericOperatorId,
  RendererSimpleRule,
} from 'services/map/model';
import { DropDownItem } from '@ptas/react-ui-library';
import {
  useState,
  SetStateAction,
  Dispatch,
  useMemo,
  useCallback,
  useEffect,
} from 'react';
import { DataSetColumnType } from 'services/map';
import { useContext } from 'react';
import { RuleContext } from '../Context';
import { HomeContext } from 'contexts';
import { AppLayers } from 'appConstants';
import { numericOperators, stringOperators } from 'services/map/renderer/utils';

interface EditingColor {
  fillColorTrue: boolean;
  fillColorFalse: boolean;
  fillColorNull: boolean;
}

export interface UseSimpleRule {
  datasetFieldValues: DropDownItem[];
  operatorsItems: DropDownItem[];
  editingColor: EditingColor;
  setEditingColor: Dispatch<SetStateAction<EditingColor>>;
  conditionValue: number | string;
  setConditionValue: Dispatch<SetStateAction<number | string>>;
  conditionOperator: StringOperatorId | NumericOperatorId;
  setConditionOperator: Dispatch<
    SetStateAction<StringOperatorId | NumericOperatorId>
  >;
  updateRuleColor: (
    rgbaColor: string,
    colorCategory: 'colorTrue' | 'colorFalse'
  ) => void;
}

export const getOperators = (
  columnType: DataSetColumnType = 'unknown'
): DropDownItem[] =>
  Object.entries(
    ['number', 'date'].includes(columnType)
      ? numericOperators
      : columnType === 'string'
      ? stringOperators
      : {}
  ).map(([k, v]) => ({
    value: k,
    label: v.name,
  }));

export const useSimpleRule = (): UseSimpleRule => {
  const { currentLayer } = useContext(HomeContext);
  const currentLayerId = currentLayer?.rendererRules.layer.id;
  const {
    dataSetColumnValues,
    datasetField,
    setRule,
    embeddedDataField,
    rule,
  } = useContext(RuleContext);
  const [editingColor, setEditingColor] = useState<EditingColor>({
    fillColorTrue: true,
    fillColorFalse: true,
    fillColorNull: true,
  });
  const [conditionValue, setConditionValue] = useState<number | string>('');
  const [conditionOperator, setConditionOperator] = useState<
    StringOperatorId | NumericOperatorId
  >('equals');
  const ruleCondVal =
    rule instanceof RendererSimpleRule ? rule.conditionValue : undefined;
  const ruleCondOperator =
    rule instanceof RendererSimpleRule ? rule.conditionOperator : undefined;
  const datasetFieldValues = useMemo<DropDownItem[]>(
    () =>
      dataSetColumnValues?.map((fieldValue) => {
        return {
          label: '' + fieldValue,
          value: fieldValue,
        };
      }) ?? [],
    [dataSetColumnValues]
  );

  const operatorsItems = useMemo<DropDownItem[]>(() => {
    if (currentLayerId === AppLayers.PARCEL_LAYER) {
      //Parcel layer
      return getOperators(datasetField?.columnType);
    } else {
      //Non-parcel layers
      return getOperators(embeddedDataField?.FieldType);
    }
  }, [currentLayerId, datasetField, embeddedDataField]);

  useEffect(() => {
    if (!ruleCondOperator) return;
    if (!ruleCondVal) return;
    setConditionOperator(
      ruleCondOperator as StringOperatorId | NumericOperatorId
    );
    setConditionValue(ruleCondVal);
  }, [ruleCondVal, ruleCondOperator]);

  /**
   * Condition operator,condition value
   */
  useEffect(() => {
    setRule((prev) => {
      if (!(prev instanceof RendererSimpleRule))
        return new RendererSimpleRule();
      return new RendererSimpleRule({
        ...prev,
        conditionOperator: conditionOperator,
        conditionValue: conditionValue,
      });
    });
  }, [conditionOperator, conditionValue, setRule]);

  const updateRuleColor = useCallback(
    (rgbaColor: string, colorCategory: 'colorTrue' | 'colorFalse'): void => {
      if (colorCategory === 'colorTrue') {
        setRule((prev) => {
          if (!(prev instanceof RendererSimpleRule))
            return new RendererSimpleRule();
          const updatedRule = new RendererSimpleRule({
            ...prev,
          });
          if (editingColor.fillColorTrue) {
            updatedRule.fillColorTrue = rgbaColor;
          } else {
            updatedRule.outlineColorTrue = rgbaColor;
          }
          return updatedRule;
        });
      } else if (colorCategory === 'colorFalse') {
        setRule((prevRule) => {
          if (!(prevRule instanceof RendererSimpleRule))
            return new RendererSimpleRule();
          const updatedRule = new RendererSimpleRule({
            ...prevRule,
          });
          if (editingColor.fillColorFalse) {
            updatedRule.fillColorFalse = rgbaColor;
          } else {
            updatedRule.outlineColorFalse = rgbaColor;
          }
          return updatedRule;
        });
      }
    },
    [editingColor.fillColorFalse, editingColor.fillColorTrue, setRule]
  );

  return {
    operatorsItems,
    datasetFieldValues,
    editingColor,
    setEditingColor,
    conditionValue,
    setConditionValue,
    conditionOperator,
    setConditionOperator,
    updateRuleColor,
  };
};
