// ZoomInContent.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { memo, useRef } from 'react';
import { Box, makeStyles, TextField, Theme } from '@material-ui/core';
import { CSSProperties } from '@material-ui/core/styles/withStyles';

interface ListItem {
  label: string;
  style?: CSSProperties;
  value?: string | number;
  disabled?: boolean;
  onClick?: (value?: string | number) => void;
}

/**
 * Component props
 */
interface Props {
  onSelected?: (selected: SelectedItem) => void;
  onManualScaleEnter?: (value: number) => void;
}

export type SelectedItem = string | number | undefined;

/**
 * Component styles
 */
const useStyles = makeStyles((theme: Theme) => ({
  root: {
    width: 160,
  },
  list: {
    listStyle: 'none',
    padding: theme.spacing(1),
    margin: 0,
    '& li': {
      cursor: 'pointer',
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontWeight: 'normal',
      lineHeight: theme.ptas.typography.lineHeight,
      '&:hover': {
        backgroundColor: theme.ptas.colors.theme.grayLight,
      },
    },
  },
  textField: {
    width: 60,
    height: 20,
    fontSize: '0.8125rem',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontWeight: 'normal',
    lineHeight: theme.ptas.typography.lineHeight,
  },
}));

const ListItem = ({
  label,
  style,
  value,
  disabled,
  onClick,
}: ListItem): JSX.Element => (
  <li
    onClick={(): void => {
      if (!disabled) onClick?.(value);
    }}
    style={style}
  >
    {label}
  </li>
);

/**
 * ZoomInContent
 *
 * @param props - Component props
 * @returns A JSX element
 */
export const ZoomMenu = memo(
  (props: Props): JSX.Element => {
    const classes = useStyles();
    const textFieldValue = useRef<string>('');
    const onMenuItemClick = (selected: SelectedItem): void => {
      props.onSelected && props.onSelected(selected);
    };

    return (
      <Box className={classes.root}>
        <ul className={classes.list}>
          <ListItem label="Show All" value="all" onClick={onMenuItemClick} />
          <ListItem
            label="Show Selected"
            value="selected"
            style={{ marginBottom: 5 }}
            onClick={onMenuItemClick}
          />
          <li style={{ marginBottom: 5 }}>
            <label>1: </label>
            <TextField
              InputProps={{
                classes: { root: classes.textField },
              }}
              onChange={(e): void => {
                textFieldValue.current = e.target.value;
              }}
              onKeyUp={(e): void => {
                if (e.key === 'Enter') {
                  if (
                    props.onManualScaleEnter &&
                    !isNaN(+textFieldValue.current)
                  ) {
                    props.onManualScaleEnter(+textFieldValue.current);
                  }
                }
              }}
            />
          </li>
          <ListItem label="1:250" value={250} onClick={onMenuItemClick} />
          <ListItem label="1:500" value={500} onClick={onMenuItemClick} />
          <ListItem label="1:1,000" value={1000} onClick={onMenuItemClick} />
          <ListItem label="1:2,500" value={2500} onClick={onMenuItemClick} />
          <ListItem label="1:5,000" value={5000} onClick={onMenuItemClick} />
          <ListItem label="1:10,000" value={10000} onClick={onMenuItemClick} />
          <ListItem label="1:25,000" value={25000} onClick={onMenuItemClick} />
          <ListItem label="1:50,000" value={50000} onClick={onMenuItemClick} />
          <ListItem
            label="1:100,000"
            value={100000}
            style={{ marginBottom: 5 }}
            onClick={onMenuItemClick}
          />
          <ListItem
            label="Show County"
            value="county"
            onClick={onMenuItemClick}
          />
        </ul>
      </Box>
    );
  }
);
