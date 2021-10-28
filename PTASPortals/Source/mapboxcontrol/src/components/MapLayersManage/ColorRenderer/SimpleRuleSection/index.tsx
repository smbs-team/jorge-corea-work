// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext } from 'react';
import clsx from 'clsx';
import {
  withStyles,
  WithStyles,
  createStyles,
  Box,
  Theme,
  StyleRules,
} from '@material-ui/core';
import { ColorPicker, CustomTabs } from '@ptas/react-ui-library';
import { PanelBody, PanelSection } from 'components/common/panel';
import { RuleContext } from '../Context';
import OperatorElements from './OperatorElements';
import { useSimpleRule } from './useSimpleRule';
import { RendererSimpleRule } from 'services/map/model';

type Props = WithStyles<typeof useStyles>;

const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    panelSection: {
      marginTop: 0,
      marginLeft: 0,
    },
    bodyContainer: {
      display: 'flex',
      alignItems: 'center',
      flexWrap: 'wrap',
    },
    conditionContainer: {
      display: 'flex',
      flexDirection: 'column',
    },
    conditionDropDownsContainer: {
      display: 'flex',
      marginTop: theme.spacing(1.25),
      width: '100%',
    },
    rowContainer: {
      display: 'flex',
      marginTop: theme.spacing(1.25),
    },
    colorPickerContainer: {
      marginRight: theme.spacing(1.25),
    },
    colorPicker: {
      marginBottom: theme.spacing(1.25),
    },
    customTabs: {
      margin: 'auto',
    },
    verticalSeparator: {
      height: '90px',
      borderLeft: '1px solid',
      margin: theme.spacing(5 / 8, 2.5, 0, 2.5),
    },
    verticalSeparatorInvisible: {
      visibility: 'hidden',
    },
  });

/**
 * SimpleRuleSection
 *
 * @param props - Component props
 * @returns A JSX element
 */
function SimpleRuleSection(props: Props): JSX.Element | null {
  const { classes } = props;
  const { datasetField, rule, labelDefinitionVisible } = useContext(
    RuleContext
  );
  const simpleRule = useSimpleRule();
  const { editingColor, setEditingColor, updateRuleColor } = simpleRule;
  const selectedFieldName = datasetField ? `"${datasetField.columnName}"` : '';

  return rule instanceof RendererSimpleRule ? (
    <PanelSection classes={{ root: classes.panelSection }}>
      <PanelBody>
        <Box className={classes.bodyContainer}>
          <Box className={classes.conditionContainer}>
            <label>Color parcel if {selectedFieldName}</label>
            {rule.columnType !== 'boolean' && (
              <OperatorElements {...simpleRule} />
            )}
          </Box>
          <Box
            className={clsx(
              classes.verticalSeparator,
              labelDefinitionVisible ? classes.verticalSeparatorInvisible : ''
            )}
          ></Box>
          <Box className={classes.rowContainer}>
            <Box className={classes.colorPickerContainer}>
              <ColorPicker
                onChangeComplete={(c): void => {
                  updateRuleColor(c.rgbaStr, 'colorTrue');
                }}
                onInputChange={(c): void => {
                  updateRuleColor(c, 'colorTrue');
                }}
                rgbColor={
                  editingColor.fillColorTrue
                    ? rule.fillColorTrue
                    : rule.outlineColorTrue
                }
                showHexInput
                label="True color"
                classes={{ root: classes.colorPicker }}
              />

              <CustomTabs
                classes={{ root: classes.customTabs }}
                switchVariant
                items={['Fill', 'Outline']}
                onSelected={(index): void =>
                  setEditingColor({
                    ...editingColor,
                    fillColorTrue: index === 0,
                  })
                }
              />
            </Box>
            <Box className={classes.colorPickerContainer}>
              <ColorPicker
                onChangeComplete={(c): void => {
                  updateRuleColor(c.rgbaStr, 'colorFalse');
                }}
                onInputChange={(c): void => {
                  updateRuleColor(c, 'colorFalse');
                }}
                rgbColor={
                  editingColor.fillColorFalse
                    ? rule.fillColorFalse
                    : rule.outlineColorFalse
                }
                showHexInput
                label="False color"
                classes={{ root: classes.colorPicker }}
              />
              <CustomTabs
                classes={{ root: classes.customTabs }}
                switchVariant
                items={['Fill', 'Outline']}
                onSelected={(index): void =>
                  setEditingColor({
                    ...editingColor,
                    fillColorFalse: index === 0,
                  })
                }
              />
            </Box>
          </Box>
        </Box>
      </PanelBody>
    </PanelSection>
  ) : null;
}

export default withStyles(useStyles)(SimpleRuleSection);
