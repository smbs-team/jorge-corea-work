// UniqueValues.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext } from 'react';
import ColorsRanges from '../common/ColorsRanges';
import { useUniqueValuesRule } from './useUniqueValuesRule';
import {
  RendererUniqueValuesRule,
  UniqueValuesRuleValueColor,
} from 'services/map/model';
import { RuleContext } from '../Context';
import PanelBody, { PanelBodyMainRow } from '../common/PanelBody';

const UniqueValuesSection = (): JSX.Element => {
  const { rule, setRule } = useContext(RuleContext);
  useUniqueValuesRule();

  const onColorsChange = (rows: UniqueValuesRuleValueColor[]): void => {
    setRule((prevRule) => {
      if (!(prevRule instanceof RendererUniqueValuesRule)) return;
      const newRule = new RendererUniqueValuesRule({ ...prevRule });
      newRule.colors = prevRule.colors.map((color, i) => {
        return new UniqueValuesRuleValueColor({
          ...color,
          tabIndex: rows[i].tabIndex,
          fill: rows[i].fill,
          outline: rows[i].outline,
          columnValue: rows[i].columnValue,
        });
      });
      return newRule;
    });
  };

  return (
    <Fragment>
      {rule instanceof RendererUniqueValuesRule && (
        <PanelBody>
          <PanelBodyMainRow>
            <ColorsRanges<UniqueValuesRuleValueColor>
              rows={rule.colors}
              onChange={onColorsChange}
            />
          </PanelBodyMainRow>
        </PanelBody>
      )}
    </Fragment>
  );
};

export default UniqueValuesSection;
