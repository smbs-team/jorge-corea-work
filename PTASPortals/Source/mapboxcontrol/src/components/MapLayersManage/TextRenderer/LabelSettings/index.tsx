// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, memo, useState } from 'react';
import {
  makeStyles,
  createStyles,
  Theme,
  useTheme,
} from '@material-ui/core/styles';
import { Box, Typography } from '@material-ui/core';
import { CustomTabs } from '@ptas/react-ui-library';
import TabContent from './TabContent';
import Visibility from './Visibility';
import Filter from './Filter';
import Content from './Content';
import Format from './Format';
import { useFeatureFields } from './useFeatureFields';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    columnFlexBoxTitle: {
      marginBottom: theme.spacing(2),
      fontFamily: theme.ptas.typography.titleFontFamily,
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
    },
  })
);

const Title = memo(
  (): JSX.Element => {
    const theme = useTheme();
    const classes = useStyles(theme);
    return (
      <Box key="tabs-title">
        <Typography className={classes.columnFlexBoxTitle}>
          Label definition
        </Typography>
      </Box>
    );
  }
);

const LabelSettings = (): JSX.Element => {
  const [tabSelected, setTabSelected] = useState<number>(0);
  const featureFields = useFeatureFields();

  return (
    <Fragment>
      <Title />
      <CustomTabs
        selectedIndex={tabSelected}
        items={['Visibility', 'Filter', 'Content', 'Format']}
        onSelected={(e: number): void => {
          setTabSelected(e);
        }}
      />
      <TabContent value={tabSelected} index={0}>
        <Visibility />
      </TabContent>
      <TabContent value={tabSelected} index={1}>
        <Filter properties={featureFields} />
      </TabContent>
      <TabContent value={tabSelected} index={2}>
        <Content allProperties={featureFields} />
      </TabContent>
      <TabContent value={tabSelected} index={3}>
        <Format />
      </TabContent>
    </Fragment>
  );
};

export default LabelSettings;
