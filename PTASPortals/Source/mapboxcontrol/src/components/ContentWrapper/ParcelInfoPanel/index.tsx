/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Box, makeStyles, useTheme } from '@material-ui/core';
import { ChipDataType, ErrorMessageAlertCtx } from '@ptas/react-ui-library';
import axios, { Canceler } from 'axios';
import { HomeContext, HomeContextProps } from 'contexts';
import { DrawToolBarContext } from 'contexts/DrawToolbarContext';
import { ParcelInfoContext } from 'contexts/ParcelInfoContext';
import React, {
  MutableRefObject,
  useContext,
  useEffect,
  useRef,
  useState,
} from 'react';
import { useMount, useObservable } from 'react-use';
import { apiService } from 'services/api';
import {
  BookmarkItemResp,
  mapUtilService,
  OptionSetItemResp,
} from 'services/map';
import selectionService from 'services/parcel/selection';
import { getErrorStr } from 'utils/getErrorStr';
import { parcelUtil } from 'utils/parcelUtil';
import SideInfoBar, { SideInfoBarMenuOption } from '../SideInfoBar';
import { BookmarkOptionType, BookmarkType } from '../SideInfoBar/BookmarkItem';
import ParcelSummary from '../SideInfoBar/ParcelSumary';
import StackedParcelsMenu from './StackedParcelsMenu';
import { useParcelImages } from './useParcelImages';
import { useViewOptionClick } from './useViewOptionClick';
import { parseDate } from './utils';

type Props = Partial<Pick<HomeContextProps, 'panelHeight'>> & {
  headerHeight: number;
};

const useStyles = makeStyles(() => ({
  root: {
    zIndex: 2,
  },
  barContainer: {
    position: 'absolute',
    right: 0,
  },
  bookmarksContent: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'space-between',
    height: '100%',
  },
  bookmarkItemsContainer: {
    overflowY: 'auto',
    height: '100%',
  },
  divider: {
    backgroundColor: '#000',
    height: '3px',
  },
}));

const parcelInfoViewMenuItems: SideInfoBarMenuOption[] = [
  {
    id: 'parcelDetail',
    label: 'Parcel detail',
  },
  {
    id: 'streetView',
    label: 'Street View',
  },
  {
    id: 'obliques',
    label: 'Obliques',
  },
  {
    id: 'bookmarks',
    label: 'Bookmarks',
  },
];

export default function ParcelInfoPanel({
  panelHeight,
  headerHeight,
}: Props): JSX.Element | null {
  const classes = useStyles(useTheme());
  const { selectedParcelInfo, setPin, parcelStack, pin } = useContext(
    ParcelInfoContext
  );
  const [imagesUrls] = useParcelImages();
  const onViewOptionClick = useViewOptionClick();
  const [closedByUser, setClosedByUser] = useState(false);
  const {
    bookmarkTags = [],
    setBookmarkTags,
    bookmarks = [],
    setBookmarks,
    bookmarkOptionSet = [],
    setBookmarkOptionSet,
    panelContent,
    setLinearProgress,
  } = useContext(HomeContext);
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const { actionMode } = useContext(DrawToolBarContext);
  const [open, setOpen] = useState(false);
  const selection = useObservable(selectionService.$onChangeSelection);
  const [bookmarksPopoverAnchor, setBookmarksPopoverAnchor] = useState<
    HTMLElement | null | undefined
  >(null);
  const [bookmarkCount, setBookmarkCount] = useState<number>(0);
  const cancelGetTagsByBookmarkRef = useRef<Canceler>();
  const [loadingBookmarkList, setLoadingBookmarkList] = useState(false);

  useMount(() => {
    (async (): Promise<void> => {
      const { data } = await apiService.bookmark.getBookmarkTags();
      const tags = (data ?? []).map((t) => {
        return {
          id: t.ptas_bookmarktagid,
          label: t.ptas_name,
          isSelected: false,
        };
      });
      setBookmarkTags(tags);
    })();
    (async (): Promise<void> => {
      const { data = [] } = await apiService.bookmark.getOptionsSet(
        'ptas_bookmark',
        'ptas_bookmarktype'
      );
      setBookmarkOptionSet(data);
    })();
  });

  useEffect(() => {
    if (actionMode === 'draw') {
      setClosedByUser(false);
    }
  }, [actionMode]);

  useEffect(() => {
    fetchBookmarks();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedParcelInfo]);

  useEffect(() => {
    setOpen(
      !!pin &&
        ((selectionService.isEnabled() && !closedByUser) ||
          mapUtilService.controlKeyPressed ||
          !!parcelStack.length)
    );
  }, [closedByUser, pin, selection, parcelStack]);

  const fetchBookmarks = async (): Promise<void> => {
    try {
      if (selectedParcelInfo) {
        const {
          bookmarksByParcelId,
          bookmarksByMajorMinorParcel,
        } = apiService.bookmark;
        apiService.bookmark.cancelRequest();
        const parcelId = selectedParcelInfo['ParcelID'];
        setLoadingBookmarkList(true);
        const bookmarksRes = parcelId
          ? await bookmarksByParcelId(parcelId)
          : selectedParcelInfo.Major && selectedParcelInfo.Minor
          ? await bookmarksByMajorMinorParcel(
              selectedParcelInfo.Major,
              selectedParcelInfo.Minor
            )
          : undefined;
        setLoadingBookmarkList(false);
        const updatedBookmark = (bookmarksRes?.results ?? []).map(
          (r: BookmarkItemResp) => {
            const optionSet =
              bookmarkOptionSet.find(
                (os: OptionSetItemResp) =>
                  os.attributeValue === r.ptas_bookmarktype
              ) ?? ({} as OptionSetItemResp);
            const type = optionSet.value ?? '';
            return {
              id: r.ptas_bookmarkid,
              note: r.ptas_bookmarknote ?? '',
              date: parseDate(r.ptas_bookmarkdate),
              type,
              tagsSelected: [],
              bookmarkOptionType: {
                value: r.ptas_bookmarktype,
                label: type,
              },
            } as BookmarkType;
          }
        );
        setBookmarkCount(bookmarksRes?.count ?? 0);
        setBookmarks(updatedBookmark);
      }
    } catch (e) {
      if (axios.isCancel(e)) return;
      showErrorMessage(getErrorStr(e));
    }
  };

  const handleFetchTagByBookmark = async (
    bookmarkId: string
  ): Promise<string[]> => {
    const { current: cancelRequest } = cancelGetTagsByBookmarkRef;
    cancelRequest && cancelRequest();
    const resp = await apiService.bookmark.getTagIdsByBookmark(
      bookmarkId,
      cancelGetTagsByBookmarkRef as MutableRefObject<Canceler>
    );
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const data = resp.data as any;
    if (!data || !data.items) return [];

    const {
      _ptas_tag1_value: tag1,
      _ptas_tag2_value: tag2,
      _ptas_tag3_value: tag3,
      _ptas_tag4_value: tag4,
      _ptas_tag5_value: tag5,
    } = data.items[0]?.changes ?? {};

    return [tag1, tag2, tag3, tag4, tag5].filter((t) => t);
  };

  const handleUpsertBookmark = async (
    bookmark: BookmarkType
  ): Promise<void> => {
    const {
      id,
      date,
      note,
      bookmarkOptionType = {},
      tagsSelected = [],
    } = bookmark;
    const option = bookmarkOptionType as BookmarkOptionType;
    const type: number | null = option.value ?? null;
    const tagNames = bookmarkTags
      .filter((bt: ChipDataType) => tagsSelected.includes(bt.id as string))
      .map((bt: ChipDataType): string => bt.label)
      .join(',');

    const newBookmark = {
      id,
      date,
      note,
      type,
      tagNames,
      tag1Id: tagsSelected[0] ?? null,
      tag2Id: tagsSelected[1] ?? null,
      tag3Id: tagsSelected[2] ?? null,
      tag4Id: tagsSelected[3] ?? null,
      tag5Id: tagsSelected[4] ?? null,
      parcelDetailId: selectedParcelInfo?.ptas_parcelDetailId ?? '',
    };
    setBookmarks((prev) => {
      // let isNewItem = true;
      const retVal = prev.map((item) => {
        if (item.id === newBookmark.id) {
          //  isNewItem = false;
          return {
            ...item,
            note,
          };
        }
        return item;
      });
      return retVal;
    });
    setLoadingBookmarkList(true);
    await apiService.bookmark.upsertBookmark(newBookmark);
    fetchBookmarks();
  };

  const handleBookmarkDelete = async (bookmarkId: string): Promise<void> => {
    setLoadingBookmarkList(true);
    setBookmarkCount((prev) => prev - 1);
    setBookmarks((prev) => prev.filter((item) => item.id !== bookmarkId));
    await apiService.bookmark.deleteBookmarkData(bookmarkId);
    fetchBookmarks();
  };

  const handleBookmarksBtnClick = (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ): void => {
    const anchor = !bookmarksPopoverAnchor ? e.currentTarget : null;
    setBookmarksPopoverAnchor(anchor);
  };

  return open ? (
    <Box
      className={classes.barContainer}
      style={{
        top:
          panelContent && panelHeight
            ? `${panelHeight + headerHeight}px`
            : headerHeight,
        height:
          panelContent && panelHeight
            ? `calc(100% - (${panelHeight + headerHeight}px))`
            : `calc(100% - ${headerHeight}px)`,
      }}
    >
      <SideInfoBar
        parcelSummary={<ParcelSummary />}
        stackedParcelsMenu={<StackedParcelsMenu />}
        parcelNumber={parcelUtil.formatPin(pin ?? '')}
        onClose={(): void => {
          setClosedByUser(true);
          setPin(undefined);
          setLinearProgress(false);
        }}
        images={imagesUrls.map((url) => (
          <img alt="" key={url} src={url} />
        ))}
        classes={{ root: classes.root }}
        viewMenuItems={parcelInfoViewMenuItems}
        onViewOptionClick={(action): void => onViewOptionClick(action.id)}
        onChecklistButtonClick={handleBookmarksBtnClick}
        numberOfBookmarks={bookmarkCount}
        loadingBookmarkList={loadingBookmarkList}
        bookmarkList={bookmarks}
        bookmarkTags={bookmarkTags}
        fetchTagsByBookmark={handleFetchTagByBookmark}
        bookmarkOptionTypes={bookmarkOptionSet.map((o) => ({
          value: o.attributeValue,
          label: o.value,
        }))}
        onCreateBookmark={handleUpsertBookmark}
        onEditBookmark={handleUpsertBookmark}
        onRemoveBookmark={handleBookmarkDelete}
      />
    </Box>
  ) : null;
}
