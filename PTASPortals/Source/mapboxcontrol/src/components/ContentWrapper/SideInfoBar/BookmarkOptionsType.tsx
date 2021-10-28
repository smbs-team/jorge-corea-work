// BookmarkoptionsType.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  Box,
  createStyles,
  Switch,
  withStyles,
  Theme,
  WithStyles,
  StyleRules,
} from '@material-ui/core';
import React, { ChangeEvent, useState } from 'react';
import { BookmarkOptionType } from './BookmarkItem';
import { useUpdateEffect } from 'react-use';

const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      display: 'flex',
      flexDirection: 'row',
    },
    switchBox: {
      marginRight: '18px',
    },
    text: {
      color: theme.ptas.colors.theme.black,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      lineHeight: theme.ptas.typography.lineHeight,
    },
  });

const AntSwitch = withStyles((theme) => ({
  root: {
    width: 40,
    height: 21,
    padding: 0,
    display: 'flex',
  },
  switchBase: {
    padding: 3,
    color: '#FFF',
    '&$checked': {
      transform: 'translateX(19px)',
      color: theme.palette.common.white,
      '& + $track': {
        opacity: 1,
        backgroundColor: theme.palette.primary.main,
        borderColor: theme.palette.primary.main,
      },
    },
  },
  thumb: {
    width: 15,
    height: 15,
    boxShadow: 'none',
  },
  track: {
    border: `1px solid #FFF`,
    borderRadius: '.9rem',
    opacity: 1,
    backgroundColor: '#000',
  },
  checked: {},
}))(Switch);

interface Props extends WithStyles<typeof useStyles> {
  bookmarkOptionTypes: BookmarkOptionType[];
  onChange: (option: BookmarkOptionType) => void;
  defaultSelected: BookmarkOptionType | undefined;
}

function BookmarkOptionsType(props: Props): JSX.Element {
  const bothOptionDefault = ['Office', 'Field'];
  const { classes, bookmarkOptionTypes, onChange, defaultSelected } = props;
  const bothOption = bookmarkOptionTypes.find((bo) => bo.label === 'Both');
  const [firstItem] = bookmarkOptionTypes;
  const [selected, setSelected] = useState<BookmarkOptionType[]>(
    firstItem ? [firstItem] : []
  );

  useUpdateEffect(() => {
    if (defaultSelected) {
      const selectedUpdated =
        defaultSelected.value === bothOption?.value
          ? bookmarkOptionTypes.filter((b) =>
              bothOptionDefault.includes(b.label)
            )
          : bookmarkOptionTypes.filter(
              (bo) => bo.value === defaultSelected.value
            );
      setSelected(selectedUpdated);
    } else {
      setSelected(firstItem ? [firstItem] : []);
    }
  }, [bookmarkOptionTypes]);

  useUpdateEffect(() => {
    if (selected.length > 1) {
      onChange(bothOption as BookmarkOptionType);
    } else {
      onChange(selected[0]);
    }
  }, [selected]);

  const handleChangeOptions = (
    e: ChangeEvent<HTMLInputElement>,
    checked: boolean
  ): void => {
    const { name, value } = e.currentTarget;
    if (!checked) {
      setSelected(() => {
        return bookmarkOptionTypes.filter((o) => o.label !== name);
      });
    } else {
      setSelected((prevState: BookmarkOptionType[]): BookmarkOptionType[] => {
        const val = parseInt(value);
        return [...prevState, { label: name, value: val }];
      });
    }
  };

  const renderItems = (): JSX.Element[] => {
    const options = bookmarkOptionTypes.filter((b) =>
      bothOptionDefault.includes(b.label)
    );
    return options.map((opt) => {
      const checked = selected.some((s) => s.label === opt.label);

      return (
        <span className={classes.switchBox} key={`switch-${opt.value}`}>
          <span className={classes.text}>{opt.label}</span>
          <AntSwitch
            key={opt.value}
            checked={checked}
            name={opt.label}
            value={opt.value}
            onChange={handleChangeOptions}
          />
        </span>
      );
    });
  };

  return <Box className={classes.root}>{renderItems()}</Box>;
}

export default withStyles(useStyles)(BookmarkOptionsType);
