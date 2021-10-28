// Assets.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import { CustomButton, CustomTabs } from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import { makeStyles, Theme } from '@material-ui/core/styles';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import BusinessForm from '../../../../components/BusinessForm';
import clsx from 'clsx';
import BusinessTable from '../../../../components/AssetsTable';
import TableCell from '../../../../components/AssetsTable/TableCell';
import TableRow from '../../../../components/AssetsTable/TableRow';
import { v4 as uuid } from 'uuid';
import * as fmGeneral from '../../../../GeneralFormatMessage';
import useUpdateBusiness from './useUpdateBusiness';
import { Asset, assetTypeCategory } from 'models/assets';
import { businessApiService } from 'services/api/apiService/business';

const useStyles = makeStyles((theme: Theme) => ({
  assetsWrap: {
    width: '100%',
    maxWidth: 799,
    marginBottom: 32,
  },
  newAsset: {
    width: 128,
    height: 24,
  },
  addIcon: {
    color: theme.ptas.colors.theme.white,
    fontSize: theme.ptas.typography.body.fontSize,
    marginRight: 4,
  },
  sortWrap: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    width: '100%',
    maxWidth: 500,
    marginLeft: 'auto',
    flexDirection: 'column',
    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
    },
  },
  sortBy: {
    fontSize: 14,
    fontFamily: theme.ptas.typography.bodyFontFamily,
  },
  borderAssets: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    marginTop: 4,
    display: 'block',
    margin: '0 auto',
    width: '100%',
    maxWidth: 799,
    marginBottom: 5,
  },
  assetsDescription: {
    display: 'block',
    color: 'rgba(0, 0, 0, 0.54)',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: 14,
    textAlign: 'center',
    marginTop: 25,
  },

  hideElement: {
    visibility: 'hidden',
  },
  assetNumber: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    width: 31,
    paddingRight: 0,
  },
  yearCell: {
    width: 38,
  },
  editCell: {
    width: 80,
  },
  costCell: {
    width: 60,
  },
  category: {
    width: 356,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
  },
  tableWrap: {
    overflowX: 'auto',
    [theme.breakpoints.up('medium')]: {
      overflowX: 'unset',
    },
  },
}));

function Assets(): JSX.Element {
  const classes = useStyles();
  const {
    setOpenAssetForm,
    openAssetForm,
    assetToUpdate,
    setAssetToUpdate,
    assetsList,
    assetsCategoryList,
    yearsAcquired,
    changeReason,
    setAssetsCategoryList,
    allAssetsCategoryList,
    setAssetsList,
    updatedBusiness,
    improvementTypeList,
  } = useUpdateBusiness();

  const sortItems = [
    {
      label: fm.year,
      disabled: false,
    },
    {
      label: fm.cost,
      disabled: false,
    },
    {
      label: fm.category,
      disabled: false,
    },
    {
      label: fm.reason,
      disabled: false,
    },
  ];

  const handleOpenForm = (): void => {
    setOpenAssetForm((prevState) => !prevState);

    if (!assetToUpdate) return;

    setAssetToUpdate(undefined);
  };

  const handleNewAsset = (): void => {
    setAssetToUpdate(undefined);
    setOpenAssetForm(true);
  };

  const handleEdit = (item: Asset): void => {
    setOpenAssetForm(true);
    setAssetToUpdate(item);
  };

  const getLabels = (
    row: Asset
  ): {
    categoryLabel: string;
    yearLabel: string;
    changeReasonLabel: string;
    code: string;
  } => {
    const category = allAssetsCategoryList.find(
      (ac) => ac.id === row.categoryCodeId
    );

    const categoryLabel = category?.persPropCategory ?? '';

    const code = category?.code ?? '';

    const yearLabel =
      yearsAcquired.find((yl) => (yl.value as string) === row.yearAcquiredId)
        ?.label ?? '';

    const changeReasonLabel =
      changeReason.find((cr) => (cr.value as number) === row.changeReason)
        ?.label ?? '';

    return {
      categoryLabel,
      yearLabel,
      changeReasonLabel,
      code,
    };
  };

  const renderDescription = (): JSX.Element => {
    if (getAssets().length) return <Fragment />;

    return (
      <span className={classes.assetsDescription}>{fm.addOneOrMoreAssets}</span>
    );
  };

  const getAssets = (): Asset[] => {
    const improvementTypesIds = improvementTypeList.map((i) => i.id);

    return assetsList.filter(
      (a) => !improvementTypesIds.includes(a.categoryCodeId)
    );
  };

  const renderRows = (): JSX.Element[] | JSX.Element => {
    const filterAssets = getAssets();

    if (!filterAssets?.length) return <Fragment />;

    return filterAssets.map((row) => {
      const labels = getLabels(row);

      return (
        <TableRow key={uuid()}>
          <TableCell classes={{ td: classes.editCell }}>
            <CustomButton
              onClick={(): void => handleEdit(row)}
              ptasVariant="Slim outline"
            >
              {fmGeneral.edit}
            </CustomButton>
          </TableCell>
          <TableCell classes={{ td: classes.assetNumber }}>
            {labels.code}
          </TableCell>
          <TableCell classes={{ td: classes.category }}>
            {labels.categoryLabel}
          </TableCell>
          <TableCell classes={{ td: classes.yearCell }}>
            {labels.yearLabel}
          </TableCell>
          <TableCell classes={{ td: classes.costCell }}>${row.cost}</TableCell>
          <TableCell>{labels.changeReasonLabel}</TableCell>
        </TableRow>
      );
    });
  };

  const handleSubmitAsset = async (asset: Asset): Promise<void> => {
    if (asset.id) {
      const newData = assetsList.map((al) => {
        if (al.id === asset.id) {
          return asset;
        }

        return al;
      });

      if (!newData.length) return;

      setAssetsList(newData);
    } else {
      const id = uuid();

      const dataToSave: Asset = {
        ...asset,
        personalPropertyId: updatedBusiness.id,
        id,
      };

      setAssetsList((prev) => [...prev, dataToSave]);
    }
  };

  const handleChangeCategoryFilterParams = async (
    categoryName: string,
    categoryGroup: assetTypeCategory
  ): Promise<void> => {
    const { data } = await businessApiService.filterAssetCategory(
      categoryName,
      categoryGroup
    );

    if (data) setAssetsCategoryList(data);
  };

  return (
    <div className={classes.assetsWrap}>
      <BusinessForm
        isOpen={openAssetForm}
        onClose={handleOpenForm}
        changeReason={changeReason}
        assetsCategoryList={assetsCategoryList}
        yearsAcquired={yearsAcquired}
        assetToUpdate={assetToUpdate}
        onSubmit={handleSubmitAsset}
        onChangeFilterParams={handleChangeCategoryFilterParams}
        allAssetsCategory={allAssetsCategoryList}
      />

      <div className={classes.sortWrap}>
        <CustomButton
          ptasVariant="Slim"
          classes={{
            root: clsx(
              classes.newAsset,
              !assetToUpdate?.id && openAssetForm && classes.hideElement
            ),
          }}
          onClick={handleNewAsset}
        >
          <AddCircleOutlineIcon className={classes.addIcon} />
          {fm.newAssets}
        </CustomButton>
        <span className={classes.sortBy}>{fm.sortBy}</span>
        <CustomTabs
          ptasVariant="SwitchMedium"
          items={sortItems}
          onSelected={(tab: number): void => {
            console.log('tab selected', tab);
          }}
          tabsBackgroundColor="#666666"
          itemTextColor="#ffffff"
          selectedItemTextColor="#000000"
          indicatorBackgroundColor="#ffffff"
        />
      </div>
      <span className={classes.borderAssets}></span>
      <div className={classes.tableWrap}>
        <BusinessTable>{renderRows()}</BusinessTable>
      </div>
      {renderDescription()}
    </div>
  );
}

export default Assets;
