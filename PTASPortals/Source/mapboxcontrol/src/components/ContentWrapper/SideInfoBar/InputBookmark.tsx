// InputBookmark.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  Chip,
  createStyles,
  TextField,
  WithStyles,
  withStyles,
  Theme,
  StyleRules,
} from '@material-ui/core';
import { v4 as uuidv4 } from 'uuid';

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  value: string;
  placeholder?: string;
  loadingTags?: boolean;
  tagNames: string[];
  onChange: (
    e: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => void;
  onEnter?: () => void;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      borderBottom: '1px solid #000',
      paddingBottom: '4px',
    },
    textField: {
      width: '100%',
    },
    input: {
      padding: 0,
      color: theme.ptas.colors.theme.black,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.formField.fontWeight,
      lineHeight: theme.ptas.typography.lineHeight,
      '&::placeholder': {
        color: theme.ptas.colors.theme.gray,
      },
    },
    noBorder: {
      border: 0,
    },
    chip: {
      color: theme.ptas.colors.theme.white,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.finePrint.fontSize,
      fontWeight: theme.ptas.typography.finePrint.fontWeight,
      lineHeight: theme.ptas.typography.lineHeight,
      backgroundColor: '#606060',
      height: '28px',
      marginRight: '3px',
    },
    chipLabel: {
      paddingLeft: '8px',
      paddingRight: '8px',
    },
  });

const InputBookmark = (props: Props): JSX.Element => {
  const {
    classes,
    placeholder,
    value,
    onChange,
    loadingTags,
    tagNames,
  } = props;

  const renderTags = (tags: string[]): JSX.Element[] | string => {
    if (loadingTags) return 'Loading...';
    return tags.map((t) => (
      <Chip
        key={uuidv4()}
        className={classes.chip}
        label={t}
        classes={{ label: classes.chipLabel }}
      />
    ));
  };

  return (
    <div className={classes.root}>
      <TextField
        classes={{
          root: classes.textField,
        }}
        placeholder={placeholder}
        value={value}
        InputProps={{
          className: classes.input,
          onKeyUp: (e): void => {
            if (e.key === 'Enter') {
              props.onEnter?.();
            }
          },
          onChange,
          classes: { input: classes.input, notchedOutline: classes.noBorder },
        }}
        variant="outlined"
      />
      {renderTags(tagNames)}
    </div>
  );
};

export default withStyles(useStyles)(InputBookmark);
