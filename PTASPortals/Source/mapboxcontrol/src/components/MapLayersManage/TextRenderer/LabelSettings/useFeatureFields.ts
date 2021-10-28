/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AppLayers, DEFAULT_DATASET_ID } from 'appConstants';
import { HomeContext } from 'contexts';
import { useContext, useState } from 'react';
import { apiService } from 'services/api';
import { layerService } from 'services/map';
import { PropertyItem } from '../typing';
import { useAsync } from 'react-use';
import { ErrorMessageAlertCtx } from '@ptas/react-ui-library';

export const useFeatureFields = (): PropertyItem[] => {
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const { currentLayer, setLinearProgress } = useContext(HomeContext);
  const [allProperties, setAllProperties] = useState<PropertyItem[]>([]);

  useAsync(async () => {
    try {
      if (currentLayer?.rendererRules.layer.id !== AppLayers.PARCEL_LAYER) {
        const layerId = currentLayer?.rendererRules.layer.id ?? '';
        const embeddedDataFields =
          layerService.layersConfiguration[layerId].embeddedDataFields ?? [];
        const properties = embeddedDataFields.map(
          (d) =>
            ({
              type: d.FieldType,
              name: d.FieldName,
            } as PropertyItem)
        );
        setAllProperties(properties);
        return;
      }
      setLinearProgress(true);
      const dsCols = await apiService.getDataSetColumns(DEFAULT_DATASET_ID);
      setLinearProgress(false);
      const properties = dsCols.map((c) => {
        return {
          type: c.columnType,
          name: c.columnName,
        };
      });
      setAllProperties(properties);
    } catch (e) {
      showErrorMessage(JSON.stringify(e));
      setLinearProgress(false);
    }
  }, [currentLayer?.rendererRules.layer.id]);

  return allProperties;
};
