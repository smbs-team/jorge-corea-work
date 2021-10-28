// AssetsSearcher.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { useEffect, useState } from 'react';
import { makeStyles, Theme } from '@material-ui/core/styles';
// import { v4 as uuid } from 'uuid';
// import clsx from 'clsx';
import Backdrop from '@material-ui/core/Backdrop';
import Modal from '@material-ui/core/Modal';
import {
  CustomSearchTextField,
  CustomTabs,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import { v4 as uuid } from 'uuid';
import { AssetCategory, assetTypeCategory } from 'models/assets';
import { useUpdateEffect } from 'react-use';

const useStyles = makeStyles((theme: Theme) => ({
  assetsSearcherWrap: {
    maxWidth: 469,
    width: '100%',
    background: theme.ptas.colors.theme.white,
    margin: '0 auto',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    padding: 16,
    boxSizing: 'border-box',
    boxShadow: '0px 2px 12px rgba(0, 0, 0, 0.25)',
    borderRadius: 9,
  },
  search: {
    maxWidth: 415,
    margin: '0 auto',
  },
  searchDesc: {
    fontSize: 12,
    paddingLeft: 28,
    marginBottom: 18,
    display: 'block',
  },
  suggestionWrap: {
    background: theme.ptas.colors.theme.grayLight,
    height: 428,
    width: '100%',
    maxWidth: 383,
    padding: '8px 0',
    boxSizing: 'border-box',
    borderRadius: 9,
    overflowY: 'auto',
    maxHeight: 211,
    '&::-webkit-scrollbar': {
      width: 8,
    },

    /* Track */
    '&::-webkit-scrollbar-track': {
      borderRadius: 10,
    },

    /* Handle */
    '&::-webkit-scrollbar-thumb': {
      background: '#666666',
      borderRadius: 4,
    },
    paddingLeft: 10,
    margin: '0 auto',
  },
  suggestionItem: {
    display: 'flex',
    alignItems: 'flex-start',
    color: 'rgba(0, 0, 0, 0.54)',
    fontSize: theme.ptas.typography.body.fontSize,
    cursor: 'pointer',
    marginBottom: 8,
  },
  suggestionItemContent: {
    marginLeft: 8,
  },
  suggestionItemTitle: {
    color: theme.ptas.colors.theme.black,
    display: 'block',
  },
  suggestionItemSubTitle: {
    fontSize: 12,
    display: 'block',
  },
  modal: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
  },
  switchSmall: {
    margin: '0 auto',
    marginBottom: '16px',
    maxWidth: 437,
    width: '100%',

    '& > div > div': {
      justifyContent: 'center',
    },
  },
  itemSwitchSmall: {
    fontSize: 14,
  },
}));

interface Props {
  open?: boolean;
  onClose?: () => void;
  onItemSuggestionSelected?: (itemSuggestion: AssetCategory) => void;
  assetsCategoryList: AssetCategory[];
  onChangeFilterParams?: (
    categoryName: string,
    categoryGroup: assetTypeCategory
  ) => void;
}

export interface ItemSuggestion {
  assetNumber: string | number;
  title: string;
  subtitle?: string;
}

export interface SelectedCategory {
  selectedTabIndex: number;
  name: string;
}

const AssetsSearcher = (props: Props): JSX.Element => {
  const classes = useStyles();
  const {
    onClose,
    open,
    onItemSuggestionSelected,
    assetsCategoryList,
    onChangeFilterParams,
  } = props;

  const [openModal, setOpenModal] = useState<boolean>(false);

  const [categoryAssetsGroup, setCategoryAssetGroup] =
    useState<assetTypeCategory>('All');
  const [categoryName, setCategoryName] = useState<string>('');

  useEffect(() => {
    setOpenModal(open ?? false);
  }, [open]);

  const handleSelectCategory = (tab: number): void => {
    const name = categoryItems[tab].label.props
      ?.defaultMessage as assetTypeCategory;

    setCategoryAssetGroup(name);
  };

  const handleSelectItemSuggestion = (item: AssetCategory) => (): void => {
    onItemSuggestionSelected?.(item);
  };

  const handleChangeCategory = async (
    e: React.ChangeEvent<HTMLInputElement>
  ): Promise<void> => {
    setCategoryName(e.target.value);
  };

  useUpdateEffect(() => {
    onChangeFilterParams?.(categoryName, categoryAssetsGroup);
  }, [categoryName, categoryAssetsGroup]);

  const Suggestion = (): JSX.Element | JSX.Element[] => {
    if (!assetsCategoryList.length) return <span>Nothing to display</span>;

    return assetsCategoryList.map((suggestionItem) => (
      <div
        key={uuid()}
        className={classes.suggestionItem}
        onClick={handleSelectItemSuggestion(suggestionItem)}
      >
        {suggestionItem.code}
        <div className={classes.suggestionItemContent}>
          <span className={classes.suggestionItemTitle}>
            #{suggestionItem.persPropCategory}
          </span>
          {suggestionItem.legancyCode && (
            <span className={classes.suggestionItemSubTitle}>
              {suggestionItem.legancyCode}
            </span>
          )}
        </div>
      </div>
    ));
  };

  const categoryItems = [
    {
      label: fm.all,
      disabled: false,
    },
    {
      label: fm.construction,
      disabled: false,
    },
    {
      label: fm.factory,
      disabled: false,
    },
    {
      label: fm.farm,
      disabled: false,
    },
    {
      label: fm.office,
      disabled: false,
    },
    {
      label: fm.retail,
      disabled: false,
    },
    {
      label: fm.service,
      disabled: false,
    },
  ];

  const handleClose = (): void => {
    setOpenModal((prevState: boolean) => !prevState);
    onClose?.();
  };

  return (
    <Modal
      open={openModal}
      onClose={handleClose}
      closeAfterTransition
      BackdropComponent={Backdrop}
      className={classes.modal}
      BackdropProps={{
        timeout: 500,
      }}
    >
      <div className={classes.assetsSearcherWrap}>
        <CustomSearchTextField
          ptasVariant="outline"
          label={fm.findAsset}
          classes={{ wrapper: classes.search }}
          onChange={handleChangeCategory}
          onChangeDelay={600}
        />
        <span className={classes.searchDesc}>{fm.searchByCategory}</span>
        <CustomTabs
          ptasVariant="SwitchSmall"
          items={categoryItems}
          onSelected={handleSelectCategory}
          classes={{
            root: classes.switchSmall,
            itemSwitchSmall: classes.itemSwitchSmall,
          }}
          tabsBackgroundColor="#666666"
          itemTextColor="#ffffff"
          selectedItemTextColor="#000000"
          indicatorBackgroundColor="#ffffff"
        />
        <div className={classes.suggestionWrap}>{Suggestion()}</div>
      </div>
    </Modal>
  );
};

export default AssetsSearcher;
