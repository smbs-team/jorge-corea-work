// BookmarkItem.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  WithStyles,
  withStyles,
  createStyles,
  Box,
  StyleRules,
} from '@material-ui/core';
import React, { useContext, useState } from 'react';
import { useUpdateEffect } from 'react-use';
import BookmarkItem, { BookmarkType } from './BookmarkItem';
import EditBookmarks from './EditBookmarks';
import { SideInfoBarContext } from './SideInfoBarContext';

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  search?: string;
  loadingList?: boolean;
  fetchTagsByBookmark?: (BookmarkId: string) => Promise<string[]>;
}

/**
 * Component styles
 */
const useStyles = (): StyleRules =>
  createStyles({
    root: {
      overflowY: 'auto',
      padding: '10px 15px',
    },
  });

const BookmarkList = (props: Props): JSX.Element => {
  const { classes, fetchTagsByBookmark, search, loadingList } = props;
  const { bookmarkItems = [] } = useContext(SideInfoBarContext);
  const [itemIdEdit, setItemIdEdit] = useState<string>('');
  const [loadingTagsByBookmark, setLoadingTagsByBookmark] = useState<boolean>(
    false
  );
  const [tagIds, setTagIds] = useState<string[]>([]);

  useUpdateEffect(() => {
    if (itemIdEdit) {
      setLoadingTagsByBookmark(true);
      handleFetchTagByBookmarks();
    }
  }, [itemIdEdit]);

  const handleFetchTagByBookmarks = async (): Promise<void> => {
    if (!fetchTagsByBookmark) return;
    const resp = await fetchTagsByBookmark(itemIdEdit);
    const tIds = resp ?? [];
    setTagIds(tIds);
    setLoadingTagsByBookmark(false);
  };

  const handleCloseEdition = (): void => setItemIdEdit('');

  const renderItems = (): JSX.Element[] => {
    const bookmarkFilter = search
      ? bookmarkItems.filter((b: BookmarkType): boolean =>
          b.note.includes(search)
        )
      : bookmarkItems;
    return bookmarkFilter.map((item: BookmarkType) => {
      if (item.id === itemIdEdit)
        return (
          <EditBookmarks
            key={`bookmarkEdit-${item.id}`}
            loadingTagsByBookmark={loadingTagsByBookmark}
            tagIds={tagIds}
            bookmark={item}
            onClose={handleCloseEdition}
          />
        );

      return (
        <BookmarkItem
          key={`bookmarkItem-${item.id}`}
          bookmark={item}
          onDoubleClick={setItemIdEdit}
        />
      );
    });
  };

  return (
    <Box className={classes.root}>
      {renderItems()}
      {loadingList && 'loading ...'}
    </Box>
  );
};

export default withStyles(useStyles)(BookmarkList);
