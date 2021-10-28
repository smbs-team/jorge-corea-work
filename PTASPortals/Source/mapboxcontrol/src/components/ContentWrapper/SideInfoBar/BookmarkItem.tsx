// BookmarkItem.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  Box,
  createStyles,
  Switch,
  WithStyles,
  withStyles,
  Theme,
  StyleRules,
} from '@material-ui/core';
import React, { useContext } from 'react';
import MenuIcon from '@material-ui/icons/Menu';
import { SideInfoBarContext } from './SideInfoBarContext';
import clsx from 'clsx';

export type BookmarkOptionType = {
  value: number;
  label: string;
};

export type BookmarkType = {
  id: string;
  note: string;
  date: string;
  type?: string;
  tagsSelected?: string[];
  bookmarkOptionType?: BookmarkOptionType;
};

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  bookmark: BookmarkType;
  onDoubleClick?: (id: string) => void;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      display: 'flex',
      flexDirection: 'row',
      justifyContent: 'space-between',
      marginBottom: '5px',
    },
    name: {
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: theme.ptas.typography.lineHeight,
    },
    emptyName: {
      height: 4,
    },
    date: {
      fontWeight: theme.ptas.typography.finePrint.fontWeight,
      fontSize: '0.85rem',
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: theme.ptas.typography.lineHeight,
    },
    itemText: {
      display: 'flex',
      flexDirection: 'column',
    },
    icon: {
      fontSize: '1rem',
      marginRight: '2px',
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

export const parseDate = (date: string): string => {
  try {
    return new Date(date).toLocaleDateString('en-US');
  } catch (error) {
    return '';
  }
};

const BookmarkItem = (props: Props): JSX.Element => {
  const { classes, bookmark } = props;
  const { id, note, date, type } = bookmark;
  const { removeBookmark } = useContext(SideInfoBarContext);
  const formattedDate = parseDate(date);

  const onItemDoubleClick = (): void => {
    props.onDoubleClick && props.onDoubleClick(id);
  };

  const handleSwitchChange = (): void => {
    removeBookmark && removeBookmark(id);
  };

  return (
    <Box className={classes.root}>
      <Box style={{ display: 'flex' }} onDoubleClick={onItemDoubleClick}>
        <span style={{ paddingTop: '5px' }}>
          <MenuIcon className={classes.icon} />
        </span>
        <span className={classes.itemText}>
          <span className={clsx(classes.name, !note && classes.emptyName)}>
            {note}
          </span>
          <span className={classes.date}>{`${formattedDate} ${
            type ?? ''
          }`}</span>
        </span>
      </Box>
      <Box>
        <AntSwitch checked onChange={handleSwitchChange} />
      </Box>
    </Box>
  );
};

export default withStyles(useStyles)(BookmarkItem);
