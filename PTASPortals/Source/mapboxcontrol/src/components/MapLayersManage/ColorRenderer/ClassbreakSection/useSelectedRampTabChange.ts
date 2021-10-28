/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useContext, useEffect } from 'react';
import { ClassBreakRuleColorRange, RendererClassBreakRule } from 'services/map';
import { RuleContext } from '../Context';

export const useSelectedRampTabChange = (): void => {
  const { selectedRampTab, setRule } = useContext(RuleContext);
  useEffect(() => {
    if (typeof selectedRampTab === 'undefined') return;
    setRule((prevRule) => {
      if (!(prevRule instanceof RendererClassBreakRule)) return;
      const newRule = new RendererClassBreakRule({ ...prevRule });
      newRule.colorRanges = prevRule.colorRanges.map((item, i) => {
        return new ClassBreakRuleColorRange({
          ...item,
          tabIndex: selectedRampTab,
        });
      });
      return newRule;
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedRampTab]);
};
