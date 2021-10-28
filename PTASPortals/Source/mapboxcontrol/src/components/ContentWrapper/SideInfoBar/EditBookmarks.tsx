// CreateBookmarks.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { Fragment, useContext, useEffect, useState } from 'react';
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
import {
  ChipBar,
  ChipDataType,
  CustomPopover,
  Alert,
} from '@ptas/react-ui-library';
import { BookmarkOptionType, BookmarkType } from './BookmarkItem';
import { SideInfoBarContext } from './SideInfoBarContext';
import Input from './InputBookmark';
import BookmarkOptionsType from './BookmarkOptionsType';

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  bookmark: BookmarkType;
  loadingTagsByBookmark: boolean;
  tagIds: string[];
  onSave?: () => void;
  onCancel?: () => void;
  onRemove?: () => void;
  onClose?: () => void;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      padding: '10px 0px',
    },
    switchContainer: {
      display: 'flex',
      flexDirection: 'row',
      padding: '5px 0',
    },
    btnSection: {
      display: 'flex',
      flexDirection: 'column',
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
    cancelBtn: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.buttonSmall.fontSize,
      fontWeight: theme.ptas.typography.buttonSmall.fontWeight,
      lineHeight: theme.ptas.typography.lineHeight,
      color: '#3C7893',
      padding: '3px 15px',
      marginTop: '5px',
    },
    removeBtn: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.finePrint.fontSize,
      fontWeight: theme.ptas.typography.finePrint.fontWeight,
      lineHeight: theme.ptas.typography.lineHeight,
      color: theme.ptas.colors.utility.danger,
      padding: '3px 15px',
      marginTop: '3px',
    },
    closeButton: {
      color: theme.ptas.colors.theme.white,
    },
  });

const EditBookmarks = (props: Props): JSX.Element => {
  const {
    classes,
    bookmark: bookmarkProp,
    loadingTagsByBookmark,
    tagIds: tagIdsProp,
  } = props;
  const {
    bookmarkTags = [],
    removeBookmark,
    bookmarkOptionTypes = [],
    handleEditBookmark,
  } = useContext(SideInfoBarContext);
  const [anchorElPop, setAnchorElPop] = useState<null | HTMLButtonElement>(
    null
  );
  const [bookmark, setBookmark] = useState<BookmarkType>(bookmarkProp ?? {});
  const [tagIds, setTagIds] = useState<string[]>(tagIdsProp ?? []);
  const [typeSelected, setTypeSelected] = useState<BookmarkOptionType>(
    bookmark.bookmarkOptionType as BookmarkOptionType
  );

  useEffect(() => {
    setTagIds(tagIdsProp ?? []);
  }, [tagIdsProp]);

  const onSaveEdition = (): void => {
    handleEditBookmark?.current?.({
      ...bookmark,
      tagsSelected: tagIds,
      bookmarkOptionType: typeSelected,
    });
    onCloseEdition();
  };

  const onCancelEdition = (): void => {
    onCloseEdition();
  };

  const onRemoveEdition = (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ): void => {
    setAnchorElPop(e.currentTarget);
  };

  const onCloseEdition = (): void => {
    props.onClose && props.onClose();
  };

  const onChangeName = (
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
    <Fragment>
      <Box className={classes.root}>
        <Grid container spacing={1}>
          <Grid item xs={9}>
            <Input
              value={bookmark.note}
              loadingTags={loadingTagsByBookmark}
              tagNames={tagNames}
              onChange={onChangeName}
            />
            <Box style={{ margin: '8px 0' }}>
              <ChipBar chipData={chipData} onChipClick={handleTagClick} />
            </Box>
          </Grid>
          <Grid item xs={3} className={classes.btnSection}>
            <Button
              className={classes.actionBtn}
              variant="contained"
              onClick={onSaveEdition}
            >
              Save
            </Button>
            <Button className={classes.cancelBtn} onClick={onCancelEdition}>
              Cancel
            </Button>
            <Button className={classes.removeBtn} onClick={onRemoveEdition}>
              Remove
            </Button>
          </Grid>
        </Grid>
        <Box className={classes.switchContainer}>
          <BookmarkOptionsType
            defaultSelected={bookmark.bookmarkOptionType}
            bookmarkOptionTypes={bookmarkOptionTypes}
            onChange={onChangeSwitch}
          />
        </Box>
      </Box>
      <CustomPopover
        showCloseButton
        anchorEl={anchorElPop}
        onClose={(): void => {
          setAnchorElPop(null);
        }}
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'left',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'left',
        }}
        classes={{
          closeButton: classes.closeButton,
        }}
      >
        <Alert
          contentText="Deleting permanently erases this bookmark"
          okButtonText="Delete this bookmark"
          okButtonClick={(): void => {
            removeBookmark && removeBookmark(bookmark.id);
            onCloseEdition();
          }}
        />
      </CustomPopover>
    </Fragment>
  );
};

export default withStyles(useStyles)(EditBookmarks);
