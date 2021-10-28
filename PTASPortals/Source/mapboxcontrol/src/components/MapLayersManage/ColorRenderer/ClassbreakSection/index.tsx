// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext } from 'react';
import { Box, makeStyles, Theme, useTheme } from '@material-ui/core';
import { PanelBody } from 'components/common/panel';
import useClassBreak from './useClassbreak';
import ColorsRanges from '../common/ColorsRanges';
import { RuleContext } from '../Context';
import {
  RendererClassBreakRule,
  ClassBreakRuleColorRange,
} from 'services/map/model';
import { useSelectedRampTabChange } from './useSelectedRampTabChange';
import { useOnRangeChange } from './useOnRangeChange';

const useStyles = makeStyles((theme: Theme) => ({
  panelBody: {
    paddingLeft: theme.spacing(1),
    paddingRight: theme.spacing(1),
  },
  mainRow: {
    display: 'flex',
    flexWrap: 'wrap',
    '& > :nth-child(1)': {
      marginRight: 20,
    },
  },
  addButton: {
    color: '#0844a4',
  },
}));

function ClassBreakSection(): JSX.Element | null {
  const theme = useTheme();
  const classes = useStyles(theme);
  const ruleContext = useContext(RuleContext);
  const { rule } = ruleContext;
  useClassBreak(ruleContext);
  useSelectedRampTabChange();
  const onColorsRangesChange = useOnRangeChange();
  return rule instanceof RendererClassBreakRule &&
    !!rule?.colorRanges?.length ? (
    <PanelBody classes={{ root: classes.panelBody }}>
      <Box className={classes.mainRow}>
        <ColorsRanges<ClassBreakRuleColorRange>
          rows={rule.colorRanges}
          onChange={onColorsRangesChange}
        />
      </Box>
    </PanelBody>
  ) : null;
}

export default ClassBreakSection;
