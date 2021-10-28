/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useContext } from 'react';
import { ClassBreakRuleColorRange, RendererClassBreakRule } from 'services/map';
import { RuleContext } from '../Context';

type UseOnRangeChange = (rows: ClassBreakRuleColorRange[]) => void;

export const useOnRangeChange = (): UseOnRangeChange => {
  const { setRule } = useContext(RuleContext);
  return (rows: ClassBreakRuleColorRange[]): void => {
    setRule((prev) => {
      if (!(prev instanceof RendererClassBreakRule)) return prev;
      const newRule = new RendererClassBreakRule({ ...prev });
      newRule.colorRanges = prev.colorRanges.map(
        (item, i) =>
          new ClassBreakRuleColorRange({
            ...item,
            tabIndex: rows[i].tabIndex,
            fill: rows[i].fill,
            outline: rows[i].outline,
            from: rows[i].from ?? 0,
            to: rows[i].to ?? 0,
          })
      );
      return newRule;
    });
  };
};
