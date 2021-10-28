// Filter.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useEffect, useState } from 'react';
import { RadioButtonGroup } from '@ptas/react-ui-library';
import {
  createStyles,
  useTheme,
  Theme,
  makeStyles,
  Box,
  Typography,
} from '@material-ui/core';
import QueryControl from '../QueryControl';
import { TagsManageContext } from '../TagsManageContext';
import { AppLayers } from 'appConstants';
import { ClassTagLabel, FILTER_TYPES, PropertyItem } from '../typing';
import { HomeContext } from 'contexts';
import { useUpdateEffect } from 'react-use';
import { MapRenderer } from 'services/map';

interface RadioItem {
  value: string;
  label: string;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      display: 'flex',
    },
    radio: {
      marginBottom: theme.spacing(2),
    },
    columnFlexBoxTitle: {
      marginBottom: theme.spacing(2),
      fontFamily: theme.ptas.typography.titleFontFamily,
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
      lineHeight: theme.ptas.typography.lineHeight,
    },
    radioContainer: {
      paddingTop: '30px',
      paddingRight: '20px',
    },
    queryContainer: {
      paddingLeft: '15px',
      borderLeft: '1px solid ' + theme.ptas.colors.theme.black,
    },
  })
);

const getRadioItems = (currentLayer: MapRenderer | undefined): RadioItem[] => {
  let radioItemsToShow: RadioItem[] = [];
  if (
    currentLayer &&
    currentLayer.rendererRules.layer.id === AppLayers.PARCEL_LAYER
  ) {
    radioItemsToShow = Object.entries(FILTER_TYPES).map(([, v]) => v);
  } else {
    radioItemsToShow = [FILTER_TYPES.all, FILTER_TYPES.useQuery];
  }

  return radioItemsToShow;
};

interface FilterProps {
  properties: PropertyItem[];
}

const Filter = (props: FilterProps): JSX.Element => {
  const { properties } = props;
  const classes = useStyles(useTheme());
  const { currentLayer } = useContext(HomeContext);
  const { selectedLabel, setSelectedLabel } = useContext(TagsManageContext);
  const [radioItems, setRadioItems] = useState<RadioItem[]>([]);
  const [typeFilter, setTypeFilter] = useState<string>(FILTER_TYPES.all.value);

  useEffect(() => {
    if (selectedLabel) {
      const { typeFilter: tf } = selectedLabel;
      tf && tf !== typeFilter && setTypeFilter(tf);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedLabel?.id]);

  useEffect(() => {
    const radioItemsToShow: RadioItem[] = getRadioItems(currentLayer);

    radioItemsToShow.length !== radioItems.length &&
      setRadioItems(radioItemsToShow);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentLayer?.rendererRules.layer.id]);

  useUpdateEffect(() => {
    updateSelectedLabel();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [typeFilter]);

  const updateSelectedLabel = (): void => {
    if (!selectedLabel || selectedLabel.typeFilter === typeFilter) return;
    setSelectedLabel(
      (prev: ClassTagLabel | undefined): ClassTagLabel => {
        return {
          ...prev,
          typeFilter,
        } as ClassTagLabel;
      }
    );
  };

  const typeFilterChange = (selected: string): void => {
    setTypeFilter(selected);
  };

  return (
    <div className={classes.root}>
      <div className={classes.radioContainer}>
        <RadioButtonGroup
          items={radioItems}
          onChange={typeFilterChange}
          classes={{ formControlLabelRoot: classes.radio }}
          value={typeFilter}
        />
      </div>
      {typeFilter === FILTER_TYPES.useQuery.value && (
        <div className={classes.queryContainer}>
          <Box>
            <Typography className={classes.columnFlexBoxTitle}>
              Query
            </Typography>
          </Box>
          <QueryControl properties={properties ?? []} />
        </div>
      )}
    </div>
  );
};

export default Filter;
