// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { HTMLProps, useContext, useEffect, useState } from 'react';
import {
  withStyles,
  WithStyles,
  createStyles,
  Box,
  Theme,
  Menu,
  MenuItem,
  Badge,
  BadgeProps,
  ButtonBase,
  ButtonBaseProps,
  TextField,
  InputAdornment,
  Divider,
  StyleRules,
} from '@material-ui/core';
import clsx from 'clsx';
import { CustomIconButton, ChipDataType } from '@ptas/react-ui-library';
import CloseIcon from '@material-ui/icons/Close';
import ExpandIcon from '@material-ui/icons/ExpandMoreOutlined';
import CheckCircleOutlineIcon from '@material-ui/icons/CheckCircleOutline';
import KeyboardArrowLeftIcon from '@material-ui/icons/KeyboardArrowLeft';
import { useUpdateEffect } from 'react-use';
import { uniqueId } from 'lodash';
import { Search } from '@material-ui/icons';
import { SideInfoBarContext, withSideInfoBar } from './SideInfoBarContext';
import BookmarkList from './BookmarkList';
import CreateBookmarks from './CreateBookmarks';
import { BookmarkOptionType, BookmarkType } from './BookmarkItem';

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  parcelNumber: string;
  onClose?: () => void;
  images: React.ReactChild[];
  viewMenuItems: SideInfoBarMenuOption[];
  onViewOptionClick?: (action: SideInfoBarMenuOption) => void;
  BadgeProps?: BadgeProps;
  onChecklistButtonClick?: (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => void;
  numberOfBookmarks?: number;
  checkListName?: string;
  CheckListButtonProps?: ButtonBaseProps & HTMLProps<HTMLButtonElement>;
  bookmarkList?: BookmarkType[];
  removeCheckList?: boolean;
  bookmarkTags?: ChipDataType[];
  bookmarkOptionTypes?: BookmarkOptionType[];
  loadingBookmarkList?: boolean;
  onCreateBookmark?: (bookmark: BookmarkType) => void;
  fetchTagsByBookmark?: (bookmarkId: string) => Promise<string[]>;
  onEditBookmark?: (bookmark: BookmarkType) => void;
  onRemoveBookmark?: (bookmarkId: string) => void;
  stackedParcelsMenu?: JSX.Element;
  parcelSummary?: JSX.Element;
}

export type SideInfoBarParcelDetails = {
  name: string;
  value: string | number;
};

export type SideInfoBarMenuOption = {
  id: string | number;
  label: string;
};

/**
 * Component styles
 */
const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      height: '100%',
      position: 'relative',
      overflow: 'hidden',
      display: 'flex',
      flexDirection: 'row',
      width: 'fit-content',
    },
    sidePopover: {
      backgroundColor: '#ffffff',
      borderRadius: '8px',
      display: 'flex',
      flexDirection: 'column',
      height: 'calc(100% - 10px)',
      margin: '5px 0',
      overflow: 'hidden',
      width: 0,
    },
    sidePopoverOpen: {
      width: '440px',
      border: '1px solid #AAA',
    },
    sidePopoverHeader: {
      display: 'flex',
      flexDirection: 'row',
      alignItems: 'flex-end',
      padding: '5px 15px',
    },
    sidePopOverHeaderInput: {
      width: '100%',
    },
    sidePopoverBody: {
      display: 'flex',
      flexDirection: 'column',
      flexGrow: 1,
      overflowY: 'auto',
    },
    sidePopoverTail: {
      backgroundColor: 'rgba(236,236,236, 0.9)',
      clipPath: 'polygon(0% 0%, 0% 100%, 100% 50%)',
      height: '0',
      overflow: 'hidden',
      width: '5px',
    },
    textHeader: {
      fontWeight: 'bold',
      marginRight: '25px',
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.body.fontSize,
      lineHeight: theme.ptas.typography.lineHeight,
    },
    mainContent: {
      border: '1px solid #AAA',
      backgroundColor: '#ffffff',
      flexFlow: 'column',
      height: '100%',
      overflow: 'hidden',
      display: 'flex',
      width: 290,
    },
    parcelNumber: {
      fontSize: theme.ptas.typography.bodyLarge.fontSize,
      color: theme.ptas.colors.theme.accentBright,
      fontWeight: 'bold',
    },
    label: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
    },
    address1: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      color: theme.ptas.colors.theme.accentBright,
      fontWeight: 'bold',
    },
    detailBox: {
      display: 'flex',
      alignItems: 'center',
    },
    // detailName: {
    //   fontSize: theme.ptas.typography.finePrint.fontSize,
    //   textAlign: 'right',
    //   paddingRight: theme.spacing(0.625),
    // },
    detailValue: {
      fontSize: theme.ptas.typography.bodyLarge.fontSize,
      // textAlign: 'left',
      // paddingLeft: theme.spacing(0.625),
      // '&:first-letter': {
      //   textTransform: 'capitalize',
      // },
    },
    closeButton: {
      marginLeft: 'auto',
      color: 'black',
    },
    menuPaper: {
      border: `1px solid ${theme.ptas.colors.theme.grayLight}`,
      borderRadius: 0,
      width: 'fit-content',
    },
    menuItem: {
      borderBottom: `1px solid ${theme.ptas.colors.theme.grayLight}`,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: '0.875rem',
      fontWeight: 'normal',
      '&:last-child': {
        borderBottom: 'none',
      },
    },
    checklistButton: {
      width: 'fit-content',
      marginTop: theme.spacing(1.625),
      height: 30,
      borderTopRightRadius: 12,
      borderBottomRightRadius: 12,
      backgroundColor: '#cecfd0',
      fontSize: '1rem',
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontWeight: 'bold',
      paddingRight: theme.spacing(2),
      alignItems: 'center',
      display: 'flex',
      '&:hover': {
        cursor: 'pointer',
      },
    },
    arrowLeftIcon: {
      marginLeft: theme.spacing(0.25),
      fontSize: '1rem',
    },
    checkIcon: {
      marginLeft: theme.spacing(1),
      marginRight: theme.spacing(1.25),
    },
    numBookmarks: {
      marginLeft: theme.spacing(1),
      fontSize: '0.75rem',
    },
    badge: {
      backgroundColor: '#ffa834',
      color: 'black',
      position: 'unset',
      transform: 'unset',
    },
    parcelDetailsTable: {
      tableLayout: 'fixed',
      width: '100%',
      marginTop: theme.spacing(2.5),
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
    imageRoot: {
      height: 212,
      textAlign: 'center',
      '& img': {
        height: '100%',
        width: '100%',
      },
    },
    titleContainer: {
      display: 'flex',
      paddingRight: theme.spacing(0.375),
      paddingLeft: theme.spacing(1),
    },
  });

/**
 * SideInfoBar
 *
 * @param props - Component props
 * @returns A JSX element
 */
function SideInfoBar(props: Props): JSX.Element {
  const {
    classes,
    viewMenuItems,
    onViewOptionClick,
    bookmarkList,
    bookmarkTags,
    bookmarkOptionTypes,
    onCreateBookmark,
    onEditBookmark,
    onRemoveBookmark,
  } = props;
  const [viewMenuAnchor, setViewMenuAnchor] = useState<HTMLElement | null>(
    null
  );
  const [count, setCount] = useState<number>(props.numberOfBookmarks ?? 0);
  const [bookmarksPopoverAnchor, setBookmarksPopoverAnchor] = useState<
    HTMLElement | null | undefined
  >(null);
  const [openSidePopover, setOpenSidePopover] = useState<boolean>(false);
  const [tailPosition, setTailPosition] = useState<number | null>(null);
  const [bookmarkSearch, setBookmarkSearch] = useState<string>('');
  const {
    setBookmarkItems,
    setBookmarkTags,
    setBookmarkOptionTypes,
    handleCreateBookmark,
    handleEditBookmark,
    handleRemoveBookmark,
  } = useContext(SideInfoBarContext);
  handleCreateBookmark && (handleCreateBookmark.current = onCreateBookmark);
  handleEditBookmark && (handleEditBookmark.current = onEditBookmark);
  handleRemoveBookmark && (handleRemoveBookmark.current = onRemoveBookmark);

  const tailStyle = tailPosition
    ? { marginTop: tailPosition, height: '5px' }
    : { marginTop: 0, height: 0 };

  useUpdateEffect(() => {
    if (bookmarkList === undefined) return;
    setBookmarkItems?.(bookmarkList);
  }, [bookmarkList]);

  useUpdateEffect(() => {
    if (!bookmarkTags) return;
    if ((bookmarkTags ?? []).length && setBookmarkTags)
      setBookmarkTags(bookmarkTags);
  }, [bookmarkTags]);

  useEffect(() => {
    if (!bookmarkOptionTypes) return;

    if ((bookmarkTags ?? []).length && setBookmarkOptionTypes)
      setBookmarkOptionTypes(bookmarkOptionTypes);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [bookmarkOptionTypes]);

  const handleViewOptionClick = (
    event: React.MouseEvent<HTMLElement>,
    action: SideInfoBarMenuOption
  ): void => {
    event.stopPropagation();
    onViewOptionClick?.(action);
    setViewMenuAnchor(null);
  };

  useUpdateEffect(() => {
    setCount(props.numberOfBookmarks ?? 0);
  }, [props.numberOfBookmarks]);

  const handleBookmarksBtnClick = (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ): void => {
    const anchor = !bookmarksPopoverAnchor ? e.currentTarget : null;
    const boundingBtn = e.currentTarget.getBoundingClientRect();
    const newTailPosition = !openSidePopover ? boundingBtn.top : null;
    setTailPosition(newTailPosition);
    setOpenSidePopover((prevState) => !prevState);
    setBookmarksPopoverAnchor(anchor);
    props.onChecklistButtonClick?.(e);
  };

  return (
    <Box className={classes.root}>
      {props?.stackedParcelsMenu}
      <Box
        className={clsx(
          classes.sidePopover,
          openSidePopover && classes.sidePopoverOpen
        )}
      >
        <Box className={classes.sidePopoverHeader}>
          <Box className={classes.textHeader}>Bookmarks</Box>
          <TextField
            classes={{
              root: classes.sidePopOverHeaderInput,
            }}
            value={bookmarkSearch}
            onChange={(
              e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
            ): void => {
              setBookmarkSearch(e.currentTarget.value);
            }}
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <Search />
                </InputAdornment>
              ),
            }}
          />
        </Box>
        <Box className={classes.sidePopoverBody}>
          <Box className={classes.bookmarksContent}>
            <Box className={classes.bookmarkItemsContainer}>
              <BookmarkList
                loadingList={props.loadingBookmarkList}
                search={bookmarkSearch}
                fetchTagsByBookmark={props.fetchTagsByBookmark}
              />
            </Box>
            <Box>
              <Divider className={classes.divider} />
              <CreateBookmarks />
            </Box>
          </Box>
        </Box>
      </Box>
      <Box>
        <div className={classes.sidePopoverTail} style={tailStyle}></div>
      </Box>
      <Box className={classes.mainContent}>
        <Box className={classes.titleContainer}>
          <label className={clsx(classes.parcelNumber, classes.label)}>
            {props.parcelNumber}
          </label>
          <Box
            style={{ position: 'relative', cursor: 'pointer' }}
            onClick={(el): void => setViewMenuAnchor(el.currentTarget)}
          >
            <ExpandIcon style={{ position: 'absolute', bottom: 3 }} />
          </Box>
          <CustomIconButton
            icon={<CloseIcon />}
            className={classes.closeButton}
            onClick={props.onClose}
          />
          <Menu
            anchorEl={viewMenuAnchor}
            keepMounted
            open={!!viewMenuAnchor}
            onClose={(): void => setViewMenuAnchor(null)}
            PaperProps={{ className: classes.menuPaper }}
            MenuListProps={{ disablePadding: true }}
          >
            {viewMenuItems.map((item) => {
              return (
                <MenuItem
                  key={uniqueId('view-menu-item-')}
                  onClick={(e): void => {
                    handleViewOptionClick(e, item);
                  }}
                  className={classes.menuItem}
                >
                  {item.label}
                </MenuItem>
              );
            })}
          </Menu>
        </Box>
        <Box className={classes.imageRoot}>{props.images}</Box>
        {!props.removeCheckList && (
          <ButtonBase
            className={classes.checklistButton}
            focusRipple
            onClick={handleBookmarksBtnClick}
            {...props.CheckListButtonProps}
          >
            <KeyboardArrowLeftIcon className={classes.arrowLeftIcon} />
            <CheckCircleOutlineIcon className={classes.checkIcon} />
            {props.checkListName ?? 'Checklist'}
            <Badge
              badgeContent={count}
              classes={{ badge: classes.badge }}
              className={classes.numBookmarks}
              max={99}
              showZero
              {...props.BadgeProps}
            />
          </ButtonBase>
        )}
        {props.parcelSummary}
      </Box>
    </Box>
  );
}

export default withSideInfoBar(withStyles(useStyles)(SideInfoBar));
