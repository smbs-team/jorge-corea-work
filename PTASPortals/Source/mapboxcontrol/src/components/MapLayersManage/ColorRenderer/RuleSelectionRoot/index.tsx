// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext } from 'react';
import {
  makeStyles,
  Theme,
  createStyles,
  Box,
  useTheme,
} from '@material-ui/core';
import clsx from 'clsx';
import { SimpleDropDown } from '@ptas/react-ui-library';
import { PanelSection, PanelBody } from 'components/common/panel';
import { typesOfRenderer, useRuleSelectionRoot } from './useRuleSelectionRoot';
import BreakContent from './BreakContent';
import { useDataSetField } from './useDatasetField';
import DataSetFieldDropDown from './DataSetFieldDropDown';
import ColorRampContent from './ColorRampContent';
import { HomeContext } from 'contexts';
import { AppLayers } from 'appConstants';
import { RuleType } from 'services/map/model';
import { RuleContext } from '../Context';
import { useRuleType } from './useRuleType';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    panelSection: {
      marginTop: 0,
      marginLeft: 0,
    },
    flexRow: {
      display: 'flex',
    },
    firstPanelBodyRow: {
      marginBottom: 14,
    },
    panelBodyCol: {
      width: '230px',
      minWidth: '230px',
      marginRight: theme.spacing(10 / 8),
    },
    panelBodyColWide: {
      width: '400px',
    },
    horizontalSeparator: {
      marginTop: theme.spacing(2),
      marginBottom: theme.spacing(2),
    },
    datasetDropDown: {},
    dropDown: {
      width: '100%',
    },
  })
);

/**
 * Rule selection section, choose the rule type and the rule field
 */
function RuleSelectionRoot(): JSX.Element {
  useRuleType();
  const classes = useStyles(useTheme());
  const { currentLayer } = useContext(HomeContext);
  const ruleContext = useContext(RuleContext);
  const { datasetId, setDatasetId, setRuleType, ruleType } = ruleContext;
  const { dataSetsDropDownItems } = useRuleSelectionRoot();
  useDataSetField(ruleContext);

  return (
    <PanelSection classes={{ root: classes.panelSection }}>
      <PanelBody>
        <Box className={clsx(classes.flexRow, classes.firstPanelBodyRow)}>
          <Box className={classes.panelBodyCol}>
            <Box className={classes.flexRow}>
              <SimpleDropDown
                classes={{ root: classes.dropDown }}
                items={
                  currentLayer?.rendererRules.layer.id ===
                  AppLayers.PARCEL_LAYER
                    ? typesOfRenderer
                    : typesOfRenderer.filter((i) => i.value === 'Simple')
                }
                onSelected={(s): void => {
                  setRuleType(s.value as RuleType);
                }}
                label="Type of renderer"
                titleTop
                value={ruleType}
              />
            </Box>
          </Box>
          {currentLayer?.rendererRules.layer.id === AppLayers.PARCEL_LAYER && (
            <Box
              className={clsx(
                classes.panelBodyCol,
                classes.panelBodyColWide,
                classes.datasetDropDown
              )}
            >
              <Box className={classes.flexRow}>
                <SimpleDropDown
                  classes={{ root: classes.dropDown }}
                  items={dataSetsDropDownItems}
                  onSelected={(s): void => {
                    setDatasetId('' + s.value);
                  }}
                  label="Use Search Result"
                  value={
                    datasetId
                      ? datasetId
                      : dataSetsDropDownItems[0]?.value ?? ''
                  }
                />
              </Box>
            </Box>
          )}
          <Box className={clsx(classes.panelBodyCol, classes.panelBodyColWide)}>
            <Box className={classes.flexRow}>
              <DataSetFieldDropDown />
            </Box>
          </Box>
        </Box>
        {ruleType === 'Class' && (
          <Box className={classes.flexRow}>
            <BreakContent />
          </Box>
        )}
        {ruleType === 'Unique' && (
          <Box className={classes.flexRow}>
            <ColorRampContent />
          </Box>
        )}
        <hr className={classes.horizontalSeparator} />
      </PanelBody>
    </PanelSection>
  );
}

export default RuleSelectionRoot;
