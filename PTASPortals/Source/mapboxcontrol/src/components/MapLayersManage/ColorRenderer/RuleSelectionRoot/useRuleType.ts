// useRuleType.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { useContext, useEffect } from 'react';
import { HomeContext } from 'contexts';
import { RendererSimpleRule } from 'services/map/model';
import { RuleContext } from '../Context';

export const useRuleType = (): void => {
  const { currentLayer, colorRuleAction } = useContext(HomeContext);
  const { setRule, ruleType, cbInitColorRampRef } = useContext(RuleContext);

  useEffect(() => {
    if (colorRuleAction === 'update') {
      if (ruleType === 'Simple') {
        setRule((prev) => {
          if (prev instanceof RendererSimpleRule) return prev;
          return new RendererSimpleRule();
        });
      }
    }
  }, [colorRuleAction, ruleType, setRule]);

  useEffect(() => {
    if (!ruleType) return;
    if (!currentLayer?.rendererRules.colorRule) return;
    if (colorRuleAction === 'create') {
      //Only init if ruleType is simple, for Unique Values and Class Break rules,
      //that is being done on specific hooks for each type
      if (ruleType === 'Simple') {
        if (
          currentLayer.rendererRules.colorRule.rule instanceof
          RendererSimpleRule
        ) {
          setRule(currentLayer.rendererRules.colorRule.rule);
        } else {
          setRule(new RendererSimpleRule());
        }
      } else if (ruleType === 'Class') {
        if (cbInitColorRampRef) {
          cbInitColorRampRef.current = true;
        }
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [ruleType, colorRuleAction]);
};
