// CreateBookmarks.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  createStyles,
  withStyles,
  Box,
  WithStyles,
  Button,
  Grid,
  Theme,
  StyleRules,
} from '@material-ui/core';
import React, { useContext, useState } from 'react';
import { ChipDataType, ChipBar } from '@ptas/react-ui-library';
import { BookmarkOptionType, BookmarkType } from './BookmarkItem';
import { SideInfoBarContext } from './SideInfoBarContext';
import { v4 as uuidv4 } from 'uuid';
import Input from './InputBookmark';
import BookmarkOptionsType from './BookmarkOptionsType';

type Props = WithStyles<typeof useStyles>;
type ClassKey =
  | 'root'
  | 'switchContainer'
  | 'actionBtn'
  | 'inputBtnGroup'
  | 'newItemButtonWrap';

/**
 * Component styles
 */
const useStyles = (theme: Theme): StyleRules =>
  createStyles<ClassKey, object>({
    root: {
      padding: '10px 0px',
    },
    switchContainer: {
      display: 'flex',
      flexDirection: 'row',
      padding: '4px 15px',
    },
    actionBtn: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.buttonSmall.fontSize,
      fontWeight: theme.ptas.typography.buttonSmall.fontWeight,
      lineHeight: theme.ptas.typography.lineHeight,
      backgroundColor: '#D3F5FF',
      borderRadius: '1rem',
      color: '#3C7893',
      padding: '3px 21px',
    },
    inputBtnGroup: {
      alignItems: 'flex-end',
      paddingLeft: '15px',
    },
    newItemButtonWrap: {
      textAlign: 'right',
      paddingRight: 15,
    },
  });

const generateInitialData = (): BookmarkType => {
  return {
    id: uuidv4(),
    note: '',
    date: new Date().toISOString(),
    bookmarkOptionType: undefined,
    tagsSelected: [],
    type: '',
  };
};

const CreateBookmarks = (props: Props): JSX.Element => {
  const { classes } = props;
  const {
    bookmarkTags = [],
    bookmarkOptionTypes = [],
    handleCreateBookmark,
  } = useContext(SideInfoBarContext);
  const [bookmark, setBookmark] = useState<BookmarkType>(generateInitialData());
  const [tagIds, setTagIds] = useState<string[]>([]);
  const [typeSelected, setTypeSelected] = useState<BookmarkOptionType>(
    bookmark.bookmarkOptionType as BookmarkOptionType
  );

  const onSaveData = (): void => {
    handleCreateBookmark?.current &&
      handleCreateBookmark.current({
        ...bookmark,
        tagsSelected: tagIds,
        bookmarkOptionType: typeSelected,
      });
    setBookmark(generateInitialData());
    setTagIds([]);
  };

  const onNameChange = (
    e: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    setBookmark((prev) => {
      return {
        ...prev,
        note: e.currentTarget.value,
      };
    });
  };

  const handleTagClick = (chip: ChipDataType): void => {
    if (chip.isSelected && tagIds.length === 5) return;

    if (chip.isSelected) {
      setTagIds((t: string[]): string[] => [...t, chip.id as string]);
    } else {
      setTagIds((t: string[]): string[] =>
        t.filter((item: string): boolean => item !== chip.id)
      );
    }
  };

  const onChangeSwitch = (option: BookmarkOptionType): void => {
    setTypeSelected(option);
  };

  const getBookmarkTagData = (): {
    chipData: ChipDataType[];
    tagNames: string[];
  } => {
    let tagNames: string[] = [];
    const chipData = bookmarkTags.map((bt) => {
      const isSelected = tagIds.includes(bt.id as string);
      if (isSelected) tagNames = [...tagNames, bt.label];

      return {
        ...bt,
        isSelected,
      };
    });

    return {
      chipData,
      tagNames,
    };
  };

  const { chipData, tagNames } = getBookmarkTagData();

  return (
    <Box className={classes.root}>
      <Grid className={classes.inputBtnGroup} container>
        <Grid item xs={9}>
          <Input
            value={bookmark.note}
            placeholder="New item text"
            tagNames={tagNames}
            onChange={onNameChange}
            onEnter={onSaveData}
          />
        </Grid>
        <Grid item xs={3} className={classes.newItemButtonWrap}>
          <Button
            className={classes.actionBtn}
            variant="contained"
            onClick={onSaveData}
          >
            Add
          </Button>
        </Grid>
      </Grid>
      <Box style={{ margin: '8px 0' }}>
        <ChipBar chipData={chipData} onChipClick={handleTagClick} />
      </Box>
      <Box className={classes.switchContainer}>
        <BookmarkOptionsType
          defaultSelected={bookmark.bookmarkOptionType}
          bookmarkOptionTypes={bookmarkOptionTypes}
          onChange={onChangeSwitch}
        />
      </Box>
    </Box>
  );
};

export default withStyles(useStyles)(CreateBookmarks);
