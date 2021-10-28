// Improvements.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useEffect } from 'react';
import {
  CustomSwitch,
  CustomButton,
  CustomTabs,
  DropDownItem,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import * as fmGeneral from '../../../../GeneralFormatMessage';
import { makeStyles, Theme } from '@material-ui/core/styles';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import clsx from 'clsx';
import BusinessImprovementForm from '../../../../components/BusinessImprovementForm';
import TableCell from '../../../../components/AssetsTable/TableCell';
import TableRow from '../../../../components/AssetsTable/TableRow';
import BusinessTable from '../../../../components/AssetsTable';
import { Asset } from 'models/assets';
import useNewBusiness from './useNewBusiness';
import { v4 as uuid } from 'uuid';
import { formatDate } from 'utils/date';

import {
  BUILDING_LAND_IMPS_4_CODE,
  BUILDING_LAND_IMPS_6_CODE,
  LEASED_LAND_CODE,
  LEASEHOLD_CODE,
  SIDE_IMPROVEMENT_CODE,
  STORED_LEASEHOLD_CODE,
} from './constants';

const useStyles = makeStyles((theme: Theme) => ({
  improvementsWrap: {
    width: '100%',
    maxWidth: 802,
    marginBottom: 56,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
  },
  description: {
    marginBottom: 20,
    display: 'block',
  },
  sortWrap: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    width: '100%',
    maxWidth: 544,
    marginLeft: 'auto',
    flexDirection: 'column',
    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
    },

    '&.center': {
      justifyContent: 'center',
    },
  },
  sortBy: {
    fontSize: 14,
    fontFamily: theme.ptas.typography.bodyFontFamily,
  },
  hideElement: {
    visibility: 'hidden',
  },
  newItem: {
    width: 128,
    height: 24,
  },
  addIcon: {
    color: theme.ptas.colors.theme.white,
    fontSize: theme.ptas.typography.body.fontSize,
    marginRight: 4,
  },
  formControlRoot: {
    marginBottom: 34,
  },
  tableBody: {
    width: 570,
  },
  sort: {
    width: '100%',
    maxWidth: 570,
    margin: '0 auto',
  },
  sortItem: {
    padding: '0 15px',
  },
  editCell: {
    width: 79,
  },
  typeCell: {
    width: 99,
  },
  yearCell: {
    width: 37,
  },
  costCell: {
    width: 53,
  },
  modifiedDateCell: {
    textAlign: 'end',
    paddingRight: 25,
  },
  tableWrap: {
    overflowX: 'auto',
    [theme.breakpoints.up('medium')]: {
      overflowX: 'unset',
    },
  },
  hide: {
    display: 'none',
  },
}));

function Improvements(): JSX.Element {
  const classes = useStyles();
  const [openForm, setOpenForm] = useState<boolean>(false);
  const [improvements, setImprovements] = useState<Asset[]>([]);

  const {
    yearsAcquired,
    changeReason,
    improvementTypeList,
    improvementToUpdate,
    setImprovementToUpdate,
    setAssetsList,
    assetsList,
  } = useNewBusiness();

  const sortItems = [
    {
      label: fm.type,
    },
    {
      label: fm.year,
    },
    {
      label: fm.cost,
    },
    {
      label: fm.reason,
    },
    {
      label: fm.modified,
    },
  ];

  const handleSubmit = (improvement: Asset): void => {
    const modifiedOn = formatDate(new Date());

    if (improvement.id) {
      const newData = assetsList.map((al) => {
        if (al.id === improvement.id) {
          return { ...improvement, modifiedOn };
        }

        return al;
      });

      if (!newData.length) return;

      setAssetsList(newData);
    } else {
      const id = uuid();

      const dataToSave: Asset = {
        ...improvement,
        /**
         * this id will be replaced by the
         * personal property id at the time of saving business
         */
        modifiedOn,
        id,
      };

      setAssetsList((prev) => [...prev, dataToSave]);
    }
  };

  const handleRemove = (id: string | number): void => {
    console.log('id element remove', id);
  };

  const handleOpenForm = (): void => {
    setOpenForm((prevState) => !prevState);
    if (improvementToUpdate) setImprovementToUpdate(undefined);
  };

  const handleNewAsset = (): void => {
    handleOpenForm();
  };

  const handleEdit = (item: Asset): void => {
    setOpenForm(true);
    setImprovementToUpdate(item);
  };

  const SortComponent = (
    <div className={`${classes.sortWrap} ${!improvements.length && 'center'}`}>
      <CustomButton
        ptasVariant="Slim"
        classes={{
          root: clsx(
            classes.newItem,
            !improvementToUpdate?.id && openForm && classes.hideElement
          ),
        }}
        onClick={handleNewAsset}
      >
        <AddCircleOutlineIcon className={classes.addIcon} />
        {fm.newAssets}
      </CustomButton>
      <span
        className={clsx(classes.sortBy, !improvements.length && classes.hide)}
      >
        {fm.sortBy}
      </span>
      <CustomTabs
        ptasVariant="SwitchMedium"
        items={sortItems}
        onSelected={(tab: number): void => {
          console.log('tab selected', tab);
        }}
        classes={{
          itemSwitchMedium: classes.sortItem,
          root: !improvements.length && classes.hide,
        }}
        tabsBackgroundColor="#666666"
        itemTextColor="#ffffff"
        selectedItemTextColor="#000000"
        indicatorBackgroundColor="#ffffff"
      />
    </div>
  );

  const disableButton = (asset: Asset): boolean => {
    const improvementCategory = improvementTypeList.find(
      (i) => i.id === asset.categoryCodeId
    );

    if (!improvementCategory) return true;

    const nonEditableCodes = [
      LEASED_LAND_CODE,
      SIDE_IMPROVEMENT_CODE,
      STORED_LEASEHOLD_CODE,
      BUILDING_LAND_IMPS_4_CODE,
      BUILDING_LAND_IMPS_6_CODE,
    ];

    return nonEditableCodes.includes(improvementCategory.code);
  };

  const getLabels = (
    row: Asset
  ): {
    categoryLabel: string;
    yearLabel: string;
    changeReasonLabel: string;
  } => {
    const categoryLabel =
      improvementTypeList.find((ac) => ac.id === row.categoryCodeId)
        ?.persPropCategory ?? '';

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
    };
  };

  useEffect(() => {
    const improvementTypesIds = improvementTypeList.map((i) => i.id);

    const improvementsList = assetsList.filter((a) =>
      improvementTypesIds.includes(a.categoryCodeId)
    );

    setImprovements(improvementsList);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [improvementTypeList, assetsList]);

  const renderRows = (): JSX.Element[] | JSX.Element => {
    return improvements.map((row) => {
      const labels = getLabels(row);
      const nonEditable = disableButton(row);

      return (
        <TableRow key={uuid()}>
          <TableCell>
            <CustomButton
              onClick={(): void => handleEdit(row)}
              ptasVariant="Slim outline"
              disabled={nonEditable}
            >
              {fmGeneral.edit}
            </CustomButton>
          </TableCell>
          <TableCell classes={{ td: classes.typeCell }}>
            {labels.categoryLabel}
          </TableCell>
          <TableCell classes={{ td: classes.yearCell }}>
            {labels.yearLabel}
          </TableCell>
          <TableCell classes={{ td: classes.costCell }}>${row.cost}</TableCell>
          <TableCell>{labels.changeReasonLabel}</TableCell>
          <TableCell classes={{ td: classes.modifiedDateCell }}>
            {row.modifiedOn}
          </TableCell>
        </TableRow>
      );
    });
  };

  const improvementTypeMapping = (): DropDownItem[] => {
    const editableCodes = [LEASEHOLD_CODE].map((c) => c.toString());

    const categoryEditableCodes = improvementTypeList.filter((i) =>
      editableCodes.includes(i.code)
    );

    return (categoryEditableCodes || []).map((ce) => ({
      label: ce?.persPropCategory ?? '',
      value: ce?.id ?? '',
    }));
  };

  return (
    <div className={classes.improvementsWrap}>
      <span className={classes.description}>{fm.recordAnyLeaseHold}</span>
      <CustomSwitch
        onLabel={fmGeneral.yes}
        offLabel={fmGeneral.no}
        label={fm.personalPropertyTaxpayer}
        ptasVariant="small"
        showOptions
        classes={{
          formControlRoot: classes.formControlRoot,
        }}
      />
      <BusinessImprovementForm
        isOpen={openForm}
        onClose={handleOpenForm}
        onSubmit={handleSubmit}
        onRemove={handleRemove}
        changeReason={changeReason}
        improvementType={improvementTypeMapping()}
        yearsAcquired={yearsAcquired}
        dataToUpdate={improvementToUpdate}
      />
      <div className={classes.sort}>{SortComponent}</div>
      <div className={classes.tableWrap}>
        <BusinessTable classes={{ body: classes.tableBody }}>
          {renderRows()}
        </BusinessTable>
      </div>
    </div>
  );
}

export default Improvements;
