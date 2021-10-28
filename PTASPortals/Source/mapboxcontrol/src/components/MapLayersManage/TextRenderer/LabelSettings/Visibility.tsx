// Visibility.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useEffect, useState } from 'react';
import {
  createStyles,
  useTheme,
  Theme,
  makeStyles,
  Box,
} from '@material-ui/core';
import { useDebounce } from 'react-use';
import clsx from 'clsx';
import { CustomTextField, RadioButtonGroup } from '@ptas/react-ui-library';
import { TagsManageContext } from '../TagsManageContext';
import { ClassTagLabel } from '../typing';
import { mapZoomToScale, scaleToMapZoom } from 'utils/zoom';
import { DEFAULT_MAX_ZOOM, DEFAULT_MIN_ZOOM } from 'appConstants';
import { labelDisplayKeys } from 'services/map/renderer/textRenderer/textRenderersService';
import { HomeContext } from 'contexts';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      display: 'flex',
      justifyItems: 'flex-start',
    },
    leftBorder: {
      borderLeft: '1px solid ' + theme.ptas.colors.theme.black,
      paddingLeft: theme.spacing(1),
      marginLeft: theme.spacing(1),
    },
    radio: {
      marginBottom: theme.spacing(2),
    },
    scale: {
      width: 105,
    },
    and: {
      lineHeight: 2.7,
      padding: theme.spacing(0, 1),
    },
    scaleContainer: {
      display: 'flex',
      flexDirection: 'row',
      alignItems: 'flex-end',
      padding: 10,
    },
    panelRow: {
      display: 'flex',
    },
    groupTextFieldRoot: {
      width: 230,
    },
    ml0: {
      marginLeft: 0,
    },
  })
);

const typeZoomItems = [
  {
    value: 'all',
    label: 'Show label at all zoom levels',
  },
  {
    value: 'custom',
    label: 'Only show labels when zoomed between:',
  },
];

const labelShowItems: {
  label: string;
  value: typeof labelDisplayKeys[number];
}[] = [
  {
    value: 'one-label-per-shape',
    label: 'One label per shape',
  },
  {
    value: 'one-label-per-part',
    label: 'One label per part',
  },
];

const Visibility = (): JSX.Element => {
  const { currentLayer } = useContext(HomeContext);
  const classes = useStyles(useTheme());
  const { selectedLabel, setSelectedLabel } = useContext(TagsManageContext);
  const [typeZoom, setTypeZoom] = useState<string>(typeZoomItems[0].value);
  const [labelShow, setLabelShow] = useState<string>(labelShowItems[0].value);
  const [min, setMin] = useState<number>(DEFAULT_MIN_ZOOM);
  const [max, setMax] = useState<number>(DEFAULT_MAX_ZOOM);
  const [group, setGroup] = useState<string>();

  useEffect(() => {
    if (selectedLabel) {
      const { typeZoom: tz, typeLabelShow, minZoom, maxZoom } = selectedLabel;
      tz && setTypeZoom(tz);
      typeLabelShow && setLabelShow(typeLabelShow);
      setMin(mapZoomToScale(minZoom ?? 0));
      setMax(mapZoomToScale(maxZoom ?? 0));
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedLabel?.id]);

  const updateSelectedLabel = (properties: Partial<ClassTagLabel>): void => {
    if (!selectedLabel) return;
    setSelectedLabel((prev) => {
      if (!prev) return;
      return {
        ...prev,
        ...properties,
      };
    });
  };

  useEffect(() => {
    setGroup(selectedLabel?.group);
  }, [selectedLabel]);

  useDebounce(
    () => {
      updateSelectedLabel({
        group,
      });
    },
    250,
    [group]
  );

  const handleMinChange = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const val = parseInt(e.target.value);
    setMin(val);
    const convertedZoom = scaleToMapZoom(val);
    updateSelectedLabel({ minZoom: convertedZoom });
  };

  const handleMaxChange = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const val = parseInt(e.target.value);
    setMax(val);
    const convertedZoom = scaleToMapZoom(val);
    updateSelectedLabel({ maxZoom: convertedZoom });
  };

  const typeZoomChange = (v: string): void => {
    setTypeZoom(v);
    updateSelectedLabel({ typeZoom: v });
  };

  const labelShowChange = (v: string): void => {
    setLabelShow(v);
    const foundKey = labelDisplayKeys.find((item) => item === v);
    if (!foundKey) throw new Error('Invalid key ' + v);
    updateSelectedLabel({ typeLabelShow: foundKey });
  };

  return (
    <div className={classes.root}>
      <div>
        <RadioButtonGroup
          items={typeZoomItems}
          onChange={typeZoomChange}
          classes={{ formControlLabelRoot: classes.radio }}
          value={typeZoom}
        />
        {typeZoom === typeZoomItems[1].value && (
          <Box className={classes.scaleContainer}>
            <Box className={classes.panelRow}>
              <CustomTextField
                label="Min scale"
                type="number"
                value={max}
                className={classes.scale}
                onChange={handleMaxChange}
              />
              <span className={classes.and}>and</span>
              <CustomTextField
                label="Max scale"
                type="number"
                value={min}
                className={classes.scale}
                onChange={handleMinChange}
              />
            </Box>
          </Box>
        )}
      </div>
      {currentLayer?.rendererRules.layer.type === 'fill' && (
        <div className={classes.leftBorder}>
          <RadioButtonGroup
            items={labelShowItems}
            onChange={labelShowChange}
            classes={{ formControlLabelRoot: classes.radio }}
            value={labelShow}
          />
        </div>
      )}
      <CustomTextField
        value={group ?? ''}
        classes={{
          root: clsx(classes.groupTextFieldRoot, classes.leftBorder),
        }}
        label="Group"
        onChange={(e): void => {
          setGroup(e.currentTarget.value);
        }}
        helperText="Labels in the same group with same position doesn't reposition into the polygon"
        FormHelperTextProps={{
          classes: { contained: classes.ml0 },
        }}
      />
    </div>
  );
};

export default Visibility;
