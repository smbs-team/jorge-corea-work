// ColorsRanges.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useCallback } from 'react';
import { Box, makeStyles, Theme, useTheme } from '@material-ui/core';
import clsx from 'clsx';
import {
  ColorPicker,
  CustomTextField,
  CustomTabs,
} from '@ptas/react-ui-library';
import { RuleContext } from '../Context';
import {
  ClassBreakRuleColorRange,
  UniqueValuesRuleValueColor,
} from 'services/map/model';

interface Props<T> {
  rows: T[];
  onChange?: (rows: T[]) => void;
}

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    display: 'grid',
    gridRowGap: '2.1em',
    gridColumnGap: '1.5em',
  },
  grid4Cols: {
    gridTemplateColumns: '1fr 1fr min-content min-content',
  },
  grid3Cols: {
    gridTemplateColumns: 'min-content min-content min-content',
  },
  grid2Cols: {
    gridTemplateColumns: '1fr min-content',
  },
  textFieldRoot: {
    width: '160px',
  },
  colorPickerWrap: {
    display: 'flex',
    '& > :nth-child(1)': {
      marginRight: theme.spacing(10 / 8),
    },
  },
  iconWrapper: {
    marginLeft: theme.spacing(1),
    marginRight: theme.spacing(1),
  },
  iconRoot: {
    marginLeft: theme.spacing(1),
    marginRight: theme.spacing(1),
    fontSize: '1.8rem',
    '&:hover': {
      cursor: 'pointer',
    },
  },
  tabsWrap: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
  },
}));

function ColorsRanges<
  T extends ClassBreakRuleColorRange | UniqueValuesRuleValueColor
>(props: Props<T>): JSX.Element {
  const { rows } = props;
  const theme = useTheme();
  const classes = useStyles(theme);
  const tmpColsClass = props.rows.length
    ? rows[0] instanceof ClassBreakRuleColorRange
      ? classes.grid4Cols
      : classes.grid3Cols
    : undefined;
  const { ruleType } = useContext(RuleContext);
  const onChange = (rows: T[]): void => {
    if (!props.onChange) return;
    props.onChange(rows);
  };


  const onColorChanged = useCallback(
    (
      rgbaStr: string,
      row: ClassBreakRuleColorRange | UniqueValuesRuleValueColor
    ) => {
      if (!props.onChange) return;
      props.onChange(
        rows.map((r) => {
          if (r.id === row.id) {
            r.fill = r.tabIndex === 0 ? rgbaStr : r.fill;
            r.outline = r.tabIndex === 1 ? rgbaStr : r.outline;
          }
          return r;
        })
      );
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [rows]
  );

  return (
    <Fragment>
      {!!props.rows.length && ruleType && (
        <Box className={clsx(classes.root, tmpColsClass)}>
          {rows.map((row, index) => (
            <Fragment key={row.id}>
              {row instanceof ClassBreakRuleColorRange && (
                <Fragment>
                  <CustomTextField
                    InputProps={{
                      classes: {
                        root: classes.textFieldRoot,
                      },
                    }}
                    label={`${index === 0 ? '>=' : '>'} Value`}
                    value={'' + row.from}
                    onChange={(e): void => {
                      row.from = +e.target.value;
                      if (index > 0) {
                        (rows[index - 1] as ClassBreakRuleColorRange).to = +e
                          .target.value;
                      }
                      onChange(rows);
                    }}
                  />
                  <CustomTextField
                    InputProps={{
                      classes: {
                        root: classes.textFieldRoot,
                      },
                    }}
                    label={`<= Value`}
                    value={row.to}
                    onChange={(e): void => {
                      row.to = +e.target.value;
                      if (index < rows.length - 1) {
                        (rows[index + 1] as ClassBreakRuleColorRange).from = +e
                          .target.value;
                      }
                      onChange(rows);
                    }}
                  />
                </Fragment>
              )}
              {row instanceof UniqueValuesRuleValueColor && (
                <CustomTextField
                  InputProps={{
                    classes: {
                      root: classes.textFieldRoot,
                    },
                  }}
                  disabled
                  value={row.columnDescription}
                  onChange={(e): void => {
                    row.columnValue = +e.target.value;
                    onChange(rows);
                  }}
                />
              )}
              <Box className={classes.colorPickerWrap}>
                <ColorPicker
                  showHexInput
                  label="True color"
                  rgbColor={row.tabIndex === 0 ? row.fill : row.outline}
                  onChangeComplete={(color): void => {
                    onColorChanged(color.rgbaStr, row);
                  }}
                  onInputChange={(rgbaStr): void => {
                    onColorChanged(rgbaStr, row);
                  }}
                />
              </Box>
              <Box className={classes.tabsWrap}>
                <CustomTabs
                  selectedIndex={row.tabIndex}
                  switchVariant
                  items={['Fill', 'Outline']}
                  onSelected={(val): void => {
                    if (!props.onChange) return;
                    props.onChange(
                      rows.map((r) => {
                        if (r.id === row.id) {
                          r.tabIndex = val;
                        }
                        return r;
                      })
                    );
                  }}
                />
              </Box>
            </Fragment>
          ))}
        </Box>
      )}
    </Fragment>
  );
}

export default ColorsRanges;
