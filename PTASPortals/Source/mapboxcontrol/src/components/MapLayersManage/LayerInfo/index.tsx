/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useEffect } from 'react';
import {
  Box,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
} from '@material-ui/core';
import { useTheme } from '@material-ui/core/styles';
import clsx from 'clsx';
import { useLayerInfoStyles } from './useLayerInfoStyles';
import { useManageLayerCommonStyles } from '../styles';
import { layerService } from 'services/map';
import { HomeContext } from 'contexts/HomeContext';
import { useMap } from 'react-use';

interface LayerObject {
  [key: string]: string;
}

const LayerInfo = (): JSX.Element => {
  const theme = useTheme();
  const homeContext = useContext(HomeContext);
  const classes = {
    ...useManageLayerCommonStyles(theme),
    ...useLayerInfoStyles(theme),
  };
  const { currentLayer } = homeContext;

  const [layerInfo, layerInfoMethods] = useMap<LayerObject>({});

  useEffect(() => {
    layerInfoMethods.setAll({});
    const layerConfig =
      layerService.layersConfiguration[
        currentLayer?.rendererRules.layer.id ?? ''
      ];
    if (!layerConfig) {
      layerInfoMethods.setAll({ Error: 'Getting layer configuration failed' });
      return;
    }

    layerInfoMethods.setAll({
      Org: layerConfig.organization,
      Name: layerConfig.layerSourceName,
      Abstract: layerConfig.metadata?.abstract ?? layerConfig.description,
      'Data Type':
        layerConfig.metadata?.dataType ?? layerConfig.layerSourceType,
      Subject: layerConfig.metadata?.subject ?? 'TODO',
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentLayer]);

  return (
    <Box className={clsx(classes.root)}>
      <TableContainer>
        <Table className={classes.table} aria-label="simple table">
          <TableBody>
            {Object.entries(layerInfo).map(([key, value]) => {
              return (
                <TableRow key={key}>
                  <TableCell
                    colSpan={1}
                    className={clsx(classes.tableCell, classes.tableCellKey)}
                    component="th"
                    scope="row"
                  >
                    {key}
                  </TableCell>
                  <TableCell
                    colSpan={2}
                    className={clsx(classes.tableCell, classes.tableCellValue)}
                    scope="row"
                  >
                    {value}
                  </TableCell>
                </TableRow>
              );
            })}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
};
export default LayerInfo;
