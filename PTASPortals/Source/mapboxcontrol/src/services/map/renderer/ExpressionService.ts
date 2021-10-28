// ruleService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  RendererSimpleRule,
  RendererClassBreakRule,
  RendererUniqueValuesRule,
  PtasLayer,
  NumericOperatorId,
  StringOperatorId,
} from '../model';
import { FillPaint, Expression, CirclePaint } from 'mapbox-gl';
import { AppLayers, RGBA_BLACK, RGBA_TRANSPARENT } from 'appConstants';
import { numericOperators, stringOperators } from './utils';

const defaultFillPaint: FillPaint = {
  'fill-outline-color': RGBA_BLACK,
  'fill-color': RGBA_BLACK,
};

export default class ExpressionService {
  /**
   * Generates rule expression for Simple rule number field
   * @param rule -Simple rule class instance
   *
   * @remarks
   * See:
   * https://docs.mapbox.com/mapbox-gl-js/style-spec/expressions/
   * https://github.com/mapbox/mapbox-gl-js/issues/5761
   */
  genNumericSimpleRuleExpression = (
    rule: RendererSimpleRule,
    rootLayerId: string,
    layerType: Extract<PtasLayer['type'], 'circle' | 'fill'>
  ): FillPaint | CirclePaint => {
    const conditionOperator =
      rule.columnType === 'boolean'
        ? 'equals'
        : numericOperators[rule.conditionOperator as NumericOperatorId]
            .mapboxOperator;
    const conditionValue =
      rule.columnType === 'boolean' ? true : +rule.conditionValue;
    if (!(rule.conditionOperator in numericOperators)) return defaultFillPaint;

    const keyProperty =
      rootLayerId === AppLayers.PARCEL_LAYER ? 'feature-state' : 'get';
    const expression = (
      trueColor: string,
      falseColor: string,
      nullColor: string
    ): Expression => [
      'case',
      [
        'all',
        ['!=', [keyProperty, rule.columnName], null],
        [conditionOperator, [keyProperty, rule.columnName], conditionValue],
      ],
      trueColor,
      ['==', [keyProperty, rule.columnName], null],
      nullColor,
      falseColor,
    ];

    if (layerType === 'fill') {
      return {
        'fill-outline-color': expression(
          rule.outlineColorTrue,
          rule.outlineColorFalse,
          rule.outlineColorNull
        ),
        'fill-color': expression(
          rule.fillColorTrue,
          rule.fillColorFalse,
          rule.fillColorNull
        ),
      };
    }

    return {
      'circle-stroke-color': expression(
        rule.outlineColorTrue,
        rule.outlineColorFalse,
        rule.outlineColorNull
      ),
      'circle-color': expression(
        rule.fillColorTrue,
        rule.fillColorFalse,
        rule.fillColorNull
      ),
    };
  };

  /**
   * Generates rule expression for Simple rule string field
   * @param rule -Simple rule class instance
   */
  genStringSimpleRuleExpression = (
    rule: RendererSimpleRule,
    rootLayerId: string,
    layerType: Extract<PtasLayer['type'], 'circle' | 'fill'>
  ): FillPaint | CirclePaint => {
    if (!(rule.conditionOperator in stringOperators)) return defaultFillPaint;

    const conditionOperator = rule.conditionOperator as StringOperatorId;
    const keyProperty =
      rootLayerId === AppLayers.PARCEL_LAYER ? 'feature-state' : 'get';

    const notIncludesExpression = [
      '==',
      ['index-of', rule.conditionValue, [keyProperty, rule.columnName]],
      -1,
    ];

    const includesExpression = [
      '>=',
      ['index-of', rule.conditionValue, [keyProperty, rule.columnName]],
      0,
    ];

    const expression = (
      trueColor: string,
      falseColor: string,
      nullColor: string
    ): Expression => [
      'case',
      [
        'all',
        ['!=', [keyProperty, rule.columnName], null],
        conditionOperator === 'notIncludes'
          ? notIncludesExpression
          : conditionOperator === 'includes'
          ? includesExpression
          : [
              stringOperators[conditionOperator as StringOperatorId]
                .mapboxOperator,
              [keyProperty, rule.columnName],
              rule.conditionValue,
            ],
      ],
      trueColor,
      ['==', [keyProperty, rule.columnName], null],
      nullColor,
      falseColor,
    ];

    if (layerType === 'fill') {
      return {
        'fill-outline-color': expression(
          rule.outlineColorTrue,
          rule.outlineColorFalse,
          rule.outlineColorNull
        ),
        'fill-color': expression(
          rule.fillColorTrue,
          rule.fillColorFalse,
          rule.fillColorNull
        ),
      };
    }

    return {
      'circle-stroke-color': expression(
        rule.outlineColorTrue,
        rule.outlineColorFalse,
        rule.outlineColorNull
      ),
      'circle-color': expression(
        rule.fillColorTrue,
        rule.fillColorFalse,
        rule.fillColorNull
      ),
    };
  };

  genClassBreakRuleExpression = (rule: RendererClassBreakRule): FillPaint => {
    const expression = (isFill: boolean): Expression => {
      const retVal: unknown[] = ['case'];

      rule.colorRanges.forEach((range, i) => {
        retVal.push(
          [
            'all',
            ['==', ['typeof', ['feature-state', rule.columnName]], 'number'],
            [
              i === 0 ? '>=' : '>',
              ['feature-state', rule.columnName],
              range.from,
            ],
            ['<=', ['feature-state', rule.columnName], range.to],
          ],
          isFill ? range.fill : range.outline
        );
      });

      retVal.push(RGBA_TRANSPARENT);

      return retVal as Expression;
    };
    const fillPaint: FillPaint = {
      'fill-color': expression(true),
      'fill-outline-color': expression(false),
    };
    return fillPaint;
  };

  /**
   * Unique values rule
   * @param rule - Renderer unique values rule
   * @returns Fill paint
   */
  genUniqueValuesRuleExpression = (
    rule: RendererUniqueValuesRule
  ): FillPaint => {
    const expression = (isFill: boolean): Expression => {
      const retVal: unknown[] = ['case'];

      rule.colors.forEach((color) => {
        retVal.push(
          ['==', ['feature-state', rule.columnName], color.columnValue],
          isFill ? color.fill : color.outline
        );
      });

      retVal.push(RGBA_TRANSPARENT);

      return retVal as Expression;
    };
    const fillPaint: FillPaint = {
      'fill-color': expression(true),
      'fill-outline-color': expression(false),
    };
    return fillPaint;
  };
}
