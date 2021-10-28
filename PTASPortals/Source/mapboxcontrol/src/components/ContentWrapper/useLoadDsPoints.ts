/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { useContext } from 'react';
import { MapCustomEvent, QUERY_PARAM } from 'appConstants';
import { HomeContext } from 'contexts';
import { useRef } from 'react';
import { utilService } from 'services/common';
import { DataSetParcelItem } from 'services/api';
import { fitAllBbox } from 'utils';
import { useMapEvent } from 'hooks/map';
import { useAsync } from 'react-use';
import { useGlMap } from 'hooks/map/useGlMap';

const filterDatasetId = utilService.getUrlSearchParam(QUERY_PARAM.DATASET_ID);

export const useLoadDsPoints = (): void => {
  const { dataSetPoints } = useContext(HomeContext);
  const { load } = dataSetPoints;
  const run = useRef(false);
  const loadDsPointsEvt = useMapEvent(MapCustomEvent.LOAD_DS_POINTS);
  const map = useGlMap();
  useAsync(async () => {
    if (!loadDsPointsEvt) return;
    if (!map) return;
    if (!filterDatasetId) return;
    if (run.current) return;
    run.current = true;
    const selectionFilter = utilService.getUrlSearchParam(
      QUERY_PARAM.SELECTION_FILTER
    );
    if (
      selectionFilter &&
      !['selected', 'notSelected', 'all', 'unknown'].includes(selectionFilter)
    )
      return;
    await load(selectionFilter as DataSetParcelItem['selected'] | undefined);
    if (!utilService.getUrlSearchParam(QUERY_PARAM.PARCELS_QUERY)) {
      fitAllBbox(map);
    }
  }, [load, loadDsPointsEvt, map]);
};
