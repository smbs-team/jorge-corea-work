// Pagination.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, TextField } from '@material-ui/core';
import {
  Alert,
  CustomIconButton,
  CustomModalV2,
  CustomPopover,
  GenericPage,
  ObjectPagination,
} from '@ptas/react-ui-library';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import useContentHeaderStore from 'stores/useContentHeaderStore';
import { Page } from 'stores/usePageManagerStore';
import usePageManagerStore from 'stores/usePageManagerStore';
import DeleteIcon from '@material-ui/icons/Delete';
import CloseIcon from '@material-ui/icons/Close';
import SearchIcon from '@material-ui/icons/Search';
import { Fragment, useRef, useState } from 'react';
import { useEffect } from 'react';
import PageviewIcon from '@material-ui/icons/Pageview';
import ViewPages from 'components/home/viewPages';

const useStyles = makeStyles(theme => ({
  root: {
    display: 'flex',
  },
  iconButton: {
    marginRight: 16,
    height: 'fit-content',
    top: 7,
  },
  pagination: {
    marginRight: 26,
  },
  textField: {
    width: 162,
    marginRight: 26,
  },
  findMarginDense: {
    paddingBottom: 9.2,
    paddingTop: 9.2,
  },
  cancelFilterIcon: {
    opacity: 0.5,
    cursor: 'pointer',
  },
  searchIcon: {
    cursor: 'pointer',
  },
}));

function Pagination(): JSX.Element {
  const classes = useStyles();
  const pageManagerStore = usePageManagerStore();
  const { setSearchErrorMessage } = usePageManagerStore();
  const { hidePagination } = useContentHeaderStore();
  const [buttonAnchor, setButtonAnchor] = useState<HTMLButtonElement | null>(
    null
  );
  const searchEle = useRef<HTMLInputElement | null>(null);
  const [shrink, setShrink] = useState<boolean>(false);
  const [openViewPages, setOpenViewPages] = useState<boolean>(false);

  const handleOnPageChange = (page: GenericPage<Page>) => {
    pageManagerStore.updatePage();
    pageManagerStore.refreshPage(page);
  };

  const cleanInput = () => {
    if (searchEle.current) searchEle.current.value = '';
    setShrink(false);
  };

  useEffect(() => {
    if (!pageManagerStore.isFilterActive) {
      cleanInput();
    }
  }, [pageManagerStore.isFilterActive]);

  const handleOnFilterCancel = () => {
    pageManagerStore.filterBySectionUse('');
    pageManagerStore.setSearchErrorMessage(undefined);
    cleanInput();
  };

  const handleOnSearch = () => {
    pageManagerStore.filterBySectionUse(searchEle.current?.value ?? '');
    searchEle.current?.blur();
  };

  return (
    <Fragment>
      {!hidePagination && (
        <div className={classes.root}>
          <TextField
            error={pageManagerStore.searchErrorMessage ? true : false}
            variant="outlined"
            size="small"
            helperText={pageManagerStore.searchErrorMessage}
            className={classes.textField}
            label="Find section"
            InputProps={{
              endAdornment: pageManagerStore.isFilterActive ? (
                <CloseIcon
                  titleAccess="Remove filter"
                  onClick={handleOnFilterCancel}
                  className={classes.cancelFilterIcon}
                />
              ) : (
                <SearchIcon
                  className={classes.searchIcon}
                  onClick={e => {
                    e.preventDefault();
                    handleOnSearch();
                  }}
                />
              ),
              classes: {
                inputMarginDense: classes.findMarginDense,
              },
            }}
            InputLabelProps={{
              shrink: shrink,
            }}
            inputRef={searchEle}
            onKeyDown={e => {
              if (e.key === 'Enter') {
                handleOnSearch();
              }
            }}
            onFocus={() => {
              setShrink(true);
              if (pageManagerStore.searchErrorMessage)
                searchEle.current?.select();
            }}
            onBlur={e => {
              if (!e.currentTarget.value) {
                setShrink(false);
                setSearchErrorMessage(undefined);
              } else {
                setShrink(true);
              }
            }}
          />
          <ObjectPagination<Page>
            pages={pageManagerStore.pages}
            classes={{ ul: classes.pagination }}
            onClick={handleOnPageChange}
            UsePaginationProps={{
              onChange: (_e, pageNumber) => {
                pageManagerStore.setCurrentPage(pageNumber);
              },
              page: pageManagerStore.currentPage ?? 1,
            }}
            showStatusCircle
          />
          <CustomIconButton
            text="New Page"
            icon={<AddCircleOutlineIcon />}
            className={classes.iconButton}
            onClick={pageManagerStore.addNewPage}
          />
          <CustomIconButton
            text="Delete Page"
            icon={<DeleteIcon />}
            className={classes.iconButton}
            onClick={e => setButtonAnchor(e.currentTarget)}
          />
          <CustomIconButton
            text="View Pages"
            icon={<PageviewIcon />}
            className={classes.iconButton}
            onClick={() => {
              pageManagerStore.updatePage();
              setOpenViewPages(true);
            }}
          />
        </div>
      )}
      <CustomPopover
        anchorEl={buttonAnchor}
        onClose={() => {
          setButtonAnchor(null);
        }}
      >
        <Alert
          contentText={`Deleting permanently erases this page and its contents.`}
          okButtonText={`Delete page ${pageManagerStore.currentPage}`}
          okButtonClick={() => {
            pageManagerStore.deleteCurrentPage();
            setButtonAnchor(null);
          }}
        />
      </CustomPopover>
      <CustomModalV2
        open={openViewPages}
        onClose={() => setOpenViewPages(false)}
      >
        <ViewPages
          onSave={() => setOpenViewPages(false)}
          onCancel={() => setOpenViewPages(false)}
        />
      </CustomModalV2>
    </Fragment>
  );
}

export default Pagination;
