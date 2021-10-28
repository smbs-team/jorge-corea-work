// useOverlapCalculator.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { DropDownItem,  SnackContext,ErrorMessageAlertCtx  } from '@ptas/react-ui-library';
import { AppLayers } from 'appConstants';
import { AppContext, HomeContext } from 'contexts';
import { groupBy } from 'lodash';
import React, { useContext, useEffect, useState } from 'react';
import { apiService } from 'services/api';
import { utilService } from 'services/common';
import { layerService } from 'services/map';
import { ParcelFeature } from 'services/map/model';
import selectionService from 'services/parcel/selection';
import { parcelUtil } from 'utils/parcelUtil';

export type Order = 'asc' | 'desc';

export interface OverlapCalculatorData {
  parcel: string;
  overlap: string;
  percentage: string;
  data: object;
}

export interface HeadCellData {
  id: string;
  label: string;
}

interface UseOverlapCalculator {
  order: Order;
  setOrder: React.Dispatch<React.SetStateAction<Order>>;
  orderBy: keyof OverlapCalculatorData;
  setOrderBy: React.Dispatch<React.SetStateAction<keyof OverlapCalculatorData>>;
  parcelRows: OverlapCalculatorData[];
  setParcelRows: React.Dispatch<React.SetStateAction<OverlapCalculatorData[]>>;
  fillLayers: DropDownItem[];
  setFillLayers: React.Dispatch<React.SetStateAction<DropDownItem[]>>;
  selectedFillLayer: string | number;
  setSelectedFillLayer: React.Dispatch<React.SetStateAction<string | number>>;
  increaseBy: number;
  setIncreaseBy: React.Dispatch<React.SetStateAction<number>>;
  destinationParcelOption: 'selected' | 'checked';
  setDestinationParcelOption: React.Dispatch<
    React.SetStateAction<'selected' | 'checked'>
  >;
  destinationParcelRadioItems: {
    value: 'selected' | 'checked';
    label: string;
  }[];
  headCells: {
    id: string;
    label: string;
  }[];
  onClickCalculate: () => void;
  numberWithCommas: (x: number) => string;
}

export const useOverlapCalculator = (): UseOverlapCalculator => {
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx );
  const { setSnackState } = useContext(SnackContext);
  const { setShowBackdrop}=useContext(AppContext)
  const { layerSources } = useContext(HomeContext);
  const [order, setOrder] = useState<Order>('asc');
  const [orderBy, setOrderBy] = useState<keyof OverlapCalculatorData>('parcel');
  const [parcelRows, setParcelRows] = useState<OverlapCalculatorData[]>([]);
  const [fillLayers, setFillLayers] = useState<DropDownItem[]>([]);
  const [selectedFillLayer, setSelectedFillLayer] = useState<string | number>(
    ''
  );
  const [increaseBy, setIncreaseBy] = useState<number>(10);
  const [destinationParcelOption, setDestinationParcelOption] = useState<
    'selected' | 'checked'
  >('selected');
  const destinationParcelRadioItems: {
    value: 'selected' | 'checked';
    label: string;
  }[] = [
    { value: 'selected', label: 'Selected parcels' },
    { value: 'checked', label: 'Visible parcels' },
  ];

  const headCells: HeadCellData[] = [
    { id: 'parcel', label: 'Parcel' },
    { id: 'overlap', label: 'Overlap sq ft' },
    { id: 'percentage', label: 'Overlap %' },
    { id: 'data', label: 'Environmental data' },
  ];

  useEffect(() => {
    const _fillLayers = layerSources.flatMap(layer => layer.defaultMapboxLayer.type === 'fill'
      && layer.defaultMapboxLayer.id != AppLayers.PARCEL_LAYER &&
      layer.defaultMapboxLayer.id!="parcel_geom_areaLayer"
      && layer.hasOverlapSupport ? {
      label: layer.layerSourceName,
      value: layer.layerSourceId,
    } : []);

    setFillLayers(_fillLayers);
    if (_fillLayers.length) {
      setSelectedFillLayer(37); //TEMP: wetlandsLayer
    }
  }, [layerSources]);

  const onClickCalculate = async (): Promise<void> => {
    try {
      if (!selectedFillLayer || !increaseBy) return;
      setShowBackdrop(true);
      let pins: string[] = [];
      if (destinationParcelOption === 'selected') {
        const featureList = selectionService.getSelectedDotsList();
        if (!featureList || !featureList.length) {
            setSnackState({
              severity: 'info',
              text: 'No parcel selected.',
            });
          return;
        }
        pins = featureList.map(feature => feature.properties.pin);
      } else {
        //destinationParcelOption is 'checked' (use visible dataset parcels)
        const renderedFeatures = parcelUtil.getRenderedFeatures();
        if (!renderedFeatures.length) {
            setSnackState({
              severity: 'info',
              text: 'No parcel rendered.',
            });
          return;
        }
        const groupedFeatures = groupBy(renderedFeatures, item => item.id);
        const featureList: ParcelFeature[] = [];
        for (const featureId in groupedFeatures) {
          const feature = groupedFeatures[featureId][0];
          featureList.push(feature);
        }
        pins = featureList.map(feature => feature.properties?.PIN);
      }
  
      const overlapCalcRes = await apiService.overlapCalculation(
        selectedFillLayer,
        increaseBy,
        pins
      );

      if (overlapCalcRes.length) {
        const convertedData: OverlapCalculatorData[] = overlapCalcRes.map(
          item => {
            return {
              parcel: parcelUtil.formatPin(item.parcelPIN),
              overlap:
                utilService.numberWithCommas(
                  utilService.roundToNearest10(item.overlapArea)
                ) + ' sq ft',
              percentage:
                utilService.numberWithCommas(
                  parseFloat((item.overlapPercentage * 100).toFixed(1))
                ) + '%',
              // //TEMP: calculate percentage because the value on the response is incorrect
              // percentage: utilService.numberWithCommas(parseFloat(((item.overlapArea / item.parcelArea)*100).toFixed(4))) + '%',
              data: item.additionalFields,
            };
          }
        );
        setParcelRows(convertedData);
      }
    }
    catch (e) {
      if (e instanceof Error) {
        showErrorMessage(e?.stack);
      } else {
        showErrorMessage(JSON.stringify(e));
      }
    }
    finally {
      setShowBackdrop(false);
    }
  };

  const numberWithCommas = (x: number): string =>
    utilService.numberWithCommas(x);

  return {
    order,
    setOrder,
    orderBy,
    setOrderBy,
    destinationParcelOption,
    setDestinationParcelOption,
    destinationParcelRadioItems,
    headCells,
    onClickCalculate,
    numberWithCommas,
    parcelRows,
    setParcelRows,
    fillLayers,
    setFillLayers,
    selectedFillLayer,
    setSelectedFillLayer,
    increaseBy,
    setIncreaseBy,
  };
};
