/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { point } from '@turf/turf';
import {
  Dispatch,
  SetStateAction,
  useState,
  useContext,
  useCallback,
  useEffect,
} from 'react';
import {
  ErrorMessageAlertCtx,
  unimplementedStateFn,
} from '@ptas/react-ui-library';
import { DataSetParcelItem, datasetService } from 'services/api';
import { DotDsFeature, dotService } from 'services/map/dot';
import { utilService } from 'services/common';
import { QUERY_PARAM } from 'appConstants';
import { rendererService } from 'services/map';
import selectionService from 'services/parcel/selection';
import { AppContext } from 'contexts/AppContext';
import { getErrorStr } from 'utils/getErrorStr';

type UseDataSetPoints = {
  load: (selected?: DataSetParcelItem['selected']) => Promise<number>;
  hasResults: boolean;
  clear: () => void;
  showingSelected: boolean;
  setShowingSelected: Dispatch<SetStateAction<boolean>>;
  showingNotSelected: boolean;
  setShowingNotSelected: Dispatch<SetStateAction<boolean>>;
  showingAll: boolean;
  setShowingAll: Dispatch<SetStateAction<boolean>>;
  loadingDataSetParcels: boolean;
};

export const dsPointsInitialState: UseDataSetPoints = {
  clear: unimplementedStateFn,
  hasResults: false,
  load: unimplementedStateFn,
  setShowingAll: unimplementedStateFn,
  setShowingNotSelected: unimplementedStateFn,
  setShowingSelected: unimplementedStateFn,
  showingAll: false,
  showingNotSelected: false,
  showingSelected: false,
  loadingDataSetParcels: false,
};

export const useDataSetPoints = (): UseDataSetPoints => {
  const dotSource = dotService.store['dot-service-data-set'];
  const { setShowBackdrop } = useContext(AppContext);
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const [hasResults, setHasResults] = useState(false);
  const [showingSelected, setShowingSelected] = useState(false);
  const [showingNotSelected, setShowingNotSelected] = useState(false);
  const [showingAll, setShowingAll] = useState(false);
  const [loadingDataSetParcels, setLoadingDataSetParcels] = useState(false);

  useEffect(() => {
    setShowBackdrop(loadingDataSetParcels);
  }, [loadingDataSetParcels, setShowBackdrop]);

  const load = useCallback(
    async (
      selected?: DataSetParcelItem['selected']
    ): ReturnType<UseDataSetPoints['load']> => {
      try {
        setLoadingDataSetParcels(true);
        const val = datasetService.getSelectedValue(selected);
        const ds =
          utilService.getUrlSearchParam(QUERY_PARAM.DATASET_ID) ??
          rendererService.getDatasetId();
        if (!ds) return 0;
        const r = await datasetService
          .getDataSetParcels(ds, selected)
          .finally(() => {
            setLoadingDataSetParcels(false);
          });

        setHasResults(!!r?.length);
        if (!r.length) return 0;
        if (selected !== 'selected') {
          for (const item of r) {
            const dot: DotDsFeature = {
              ...point(item.centroid),
              id: item.pin,
              properties: {
                major: item.major,
                minor: item.minor,
                selected: item.selected === 'selected',
                pin: item.pin,
                circleColor:
                  item.selected === 'selected'
                    ? dotService.dotColors.red
                    : dotService.dotColors.blue,
                dotType: 'dataset-dot',
              },
            };
            if (process.env.REACT_APP_SHOW_IN_SURFACE_DOT === 'true') {
              const inSurfaceDot: DotDsFeature = {
                ...point(item.inSurfacePointCoords),
                id: 'in-surface-dot-' + item.pin,
                properties: {
                  major: item.major,
                  minor: item.minor,
                  selected: item.selected === 'selected',
                  pin: item.pin,
                  circleColor: '#F5F749',
                  dotType: 'dataset-dot',
                },
              };
              dotSource.set(inSurfaceDot.id, inSurfaceDot);
            }

            dotSource?.set(item.pin, dot);
          }
          dotService?.render('dot-service-data-set');
        } else {
          for (const item of r) {
            const _point = point(item.centroid);
            const dot: DotDsFeature = {
              ..._point,
              id: 'selection-point-' + item.pin,
              properties: {
                circleColor: dotService.dotColors.red,
                'icon-image': dotService.images.locationRed,
                layer: 'dot-service-selected-circle',
                selected: true,
                selectionType: 'feature-data',
                pin: item.pin,
                dotType: 'dataset-dot',
                major: item.major,
                minor: item.minor,
              },
            };
            selectionService.selectedDots.set(item.pin, dot);
          }
          selectionService.render({
            doApiRequest: false,
          });
        }

        if (val === 'selected') {
          setShowingSelected(true);
        } else if (val === 'notSelected') {
          setShowingNotSelected(true);
        } else if (val === 'unknown') {
          setShowingAll(true);
        }
        return r.length;
      } catch (e) {
        showErrorMessage({
          detail: getErrorStr(e),
        });
        return 0;
      }
    },
    [dotSource, showErrorMessage]
  );

  const clear = (): void => {
    dotSource.clear();
    dotService?.render();
    setTimeout(() => {
      setHasResults(false);
    }, 1000);
  };
  return {
    load,
    hasResults,
    clear,
    setShowingAll,
    setShowingNotSelected,
    setShowingSelected,
    showingAll,
    showingNotSelected,
    showingSelected,
    loadingDataSetParcels,
  };
};
