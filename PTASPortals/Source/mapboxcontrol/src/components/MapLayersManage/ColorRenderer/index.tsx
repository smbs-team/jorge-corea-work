//ColorRenderer.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext } from 'react';
import {
  withStyles,
  WithStyles,
  createStyles,
  Theme,
  Box,
  StyleRules,
} from '@material-ui/core';
import { RuleContext, withRuleProvider } from './Context';
import RuleSelectionRoot from './RuleSelectionRoot';
import SimpleRuleSection from './SimpleRuleSection';
import ClassbreakSection from './ClassbreakSection';
import UniqueValuesSection from './UniqueValuesSection';
import { HomeContext } from 'contexts';
import { useEffectOnce } from 'react-use';
import { RendererSimpleRule } from 'services/map';

/**
 * Component styles
 */
const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      display: 'flex',
      flexDirection: 'column',
      fontFamily: theme.ptas.typography.bodyFontFamily,
    },
    panel: {
      padding: 0,
    },
    panelChildrenWrap: {
      display: 'flex',
      padding: theme.spacing(0, 4),
      marginBottom: theme.spacing(3),
      flexWrap: 'nowrap',
    },
    rulesContainer: {
      width: '600px',
    },
    labelDefinitionContainer: {
      marginLeft: theme.spacing(3),
    },
    panelTitleContainer: {
      display: 'flex',
      justifyContent: 'space-between',
    },
    panelTitle: {
      marginTop: theme.spacing(2.75),
      marginBottom: theme.spacing(2),
      height: '26px',
    },
    verticalSeparator: {
      borderLeft: '1px solid gray',
      margin: '5px 15px 0 15px',
    },
    error: {
      marginBottom: theme.spacing(2 / 3),
      color: theme.ptas.colors.utility.dangerInverse,
    },
  });

/**
 * ColorRenderer
 *
 * @param props - Component props
 * @returns A JSX element
 */
const ColorRenderer = (props: WithStyles<typeof useStyles>): JSX.Element => {
  const { classes } = props;
  const {
    ruleType,
    datasetField,
    colorRendererSectionRef,
    rule,
    rendError,
  } = useContext(RuleContext);
  const { linearProgress } = useContext(HomeContext);

  useEffectOnce(() => {
    if (linearProgress) return;
    if (!colorRendererSectionRef?.current) return;
    setTimeout(() => {
      colorRendererSectionRef?.current?.scrollIntoView({
        block: 'start',
        behavior: 'smooth',
      });
    }, 500);
  });

  return (
    <Box key="color-rule-root" className={classes.root}>
      {rendError && <div className={classes.error}>{rendError}</div>}
      <RuleSelectionRoot />
      <div key="end" ref={colorRendererSectionRef} />
      {ruleType === 'Simple' && rule instanceof RendererSimpleRule && (
        <SimpleRuleSection />
      )}
      {ruleType === 'Class' &&
        (datasetField?.columnType === 'number' ||
          datasetField?.columnType === 'date') && <ClassbreakSection />}
      {ruleType === 'Unique' &&
        (datasetField?.columnType === 'number' ||
          datasetField?.columnType === 'string' ||
          datasetField?.columnType === 'date') && <UniqueValuesSection />}
    </Box>
  );
};

export default withStyles(useStyles)(withRuleProvider(ColorRenderer));
