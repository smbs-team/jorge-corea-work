// AssetsList.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  Box,
  makeStyles,
  Table,
  TableBody,
  TableRow,
  TableCell,
} from '@material-ui/core';
import { Theme } from '@material-ui/core/styles';
import { v4 as uuid } from 'uuid';
import clsx from 'clsx';
import { Asset } from 'models/assets';
import useUpdateBusiness from '../useUpdateBusiness';

export interface BusinessAsset {
  assetNumber: string;
  category: string;
  year: string;
  cost: string;
  reason: string;
}

const useStyles = makeStyles((theme: Theme) => ({
  table: {
    borderCollapse: 'separate',
    borderSpacing: '0 16px',
  },
  body: {},
  row: {},
  td: {
    border: 'none',
    padding: 0,
  },
  highlight: {
    backgroundColor: theme.ptas.colors.utility.changed,
    borderRadius: '3px',
  },
}));

function AssetsList(): JSX.Element {
  const classes = useStyles();
  const {
    yearsAcquired,
    allAssetsCategoryList,
    changeReason,
    improvementTypeList,
    assetsList,
    initialAssets,
  } = useUpdateBusiness();

  const getLabels = (
    row: Asset
  ): {
    categoryLabel: string;
    yearLabel: string;
    changeReasonLabel: string;
    code: string;
  } => {
    const category = [...allAssetsCategoryList, ...improvementTypeList].find(
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

  const getClassByChanged = (asset: Asset): string => {
    const oldAsset = initialAssets.find((oa) => oa.id === asset.id);

    if (JSON.stringify(oldAsset) === JSON.stringify(asset)) return classes.td;

    return clsx(classes.td, classes.highlight);
  };

  return (
    <Box>
      <Table className={classes.table} aria-label="caption table">
        <TableBody className={classes.body}>
          {assetsList.map((item, i) => {
            const labels = getLabels(item);

            return (
              <TableRow key={uuid()} className={classes.row}>
                <TableCell className={getClassByChanged(item)}>
                  <b>{`${labels.code} - `}</b>
                  {`${labels.categoryLabel} • ${labels.yearLabel} • ${item.cost} • ${labels.changeReasonLabel}`}
                </TableCell>
              </TableRow>
            );
          })}
        </TableBody>
      </Table>
    </Box>
  );
}

export default AssetsList;
