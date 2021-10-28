/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useEffect } from 'react';
import {
  Box,
  Theme,
  makeStyles,
  createStyles,
  useTheme,
} from '@material-ui/core';
import { AddCircleOutline } from '@material-ui/icons';
import { v4 as uuidV4 } from 'uuid';
import { IconToolBar } from '@ptas/react-ui-library';
import { PanelTitle, PanelBody, PanelSection } from 'components/common/panel';
import TagsLabelAssign from './TagsLabelAssign';
import LabelSettings from './LabelSettings';
import { TagsManageContext, withTagsManage } from './TagsManageContext';
import { ClassTagLabel } from './typing';
import { HomeContext } from 'contexts';
import { MapLayersManageContext } from 'components/MapLayersManage/Context';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    button: {
      color: theme.ptas.colors.theme.accent,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.buttonSmall.fontSize,
      fontWeight: theme.ptas.typography.buttonSmall.fontWeight,
    },
    titleButton: { marginLeft: theme.spacing(2) },
    section: {
      margin: theme.spacing(0, 4),
    },
    separator: {
      borderRight: '1px solid ' + theme.ptas.colors.theme.black,
    },
    popover: {
      borderRadius: 9,
      padding: theme.spacing(1),
    },
    routeButton: {
      cursor: 'pointer',
      textDecoration: 'underline',
    },
    sectionsContainer: {
      display: 'flex',
      flexDirection: 'row',
    },
  })
);

function TextRenderer(): JSX.Element {
  const theme = useTheme();
  const classes = useStyles(theme);
  const { currentLayer } = useContext(HomeContext);
  const { scrollToLabelRendererSection } = useContext(MapLayersManageContext);
  const { labelConfigs, setLabelConfigs, selectedLabel } = useContext(
    TagsManageContext
  );

  useEffect(() => {
    scrollToLabelRendererSection();
  }, [scrollToLabelRendererSection]);

  const findLabelName = (n: number): string => {
    const name = `Label${n}`;
    if (labelConfigs && labelConfigs.some((lc) => lc.labelName === name))
      return findLabelName(n + 1);
    return name;
  };

  const addLabel = (): void => {
    if (!currentLayer?.rendererRules.layer.id) return;
    if (!setLabelConfigs) return;
    const labelName = findLabelName(1);
    const newLabel = new ClassTagLabel({
      id: uuidV4(),
      labelName,
      refLayerId: currentLayer.rendererRules.layer.id,
      refLayerType: currentLayer.rendererRules.layer.type,
    });
    setLabelConfigs((prev) => [...prev, newLabel]);
  };

  return (
    <Box className={classes.sectionsContainer}>
      <PanelSection
        classes={{
          root: classes.separator,
        }}
      >
        <PanelTitle topLine={false} bottomLine={false}>
          <IconToolBar
            icons={[
              {
                text: 'Add label',
                icon: <AddCircleOutline />,
                onClick: addLabel,
              },
            ]}
            classes={{ root: classes.titleButton }}
          />
        </PanelTitle>
        <PanelBody>
          <TagsLabelAssign />
        </PanelBody>
      </PanelSection>

      {selectedLabel?.id && (
        <PanelSection>
          <PanelBody>
            <LabelSettings />
          </PanelBody>
        </PanelSection>
      )}
    </Box>
  );
}

export default withTagsManage(TextRenderer);
