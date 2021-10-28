/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import mapboxgl from 'mapbox-gl';
import React, {
  createContext,
  PropsWithChildren,
  useContext,
  useEffect,
  useRef,
  useState,
} from 'react';
import axios from 'axios';
import { useAsync, useObservable, useUpdateEffect } from 'react-use';
import { BBox } from '@turf/turf';
import { ErrorMessageAlertCtx } from '@ptas/react-ui-library';
import { useSelectedDots } from 'hooks/map';
import {
  mapUtilService,
  GisMapDataFields,
  ParcelFeatureData,
} from 'services/map';
import { dotService } from 'services/map/dot';
import { useCurrentPageMarker } from './useCurrentPageMarker';
import selectionService from 'services/parcel/selection';
import { $onParcelClick, $onParcelStack } from 'services/map/mapServiceEvents';
import { getParcelDetail } from './service';
import { getErrorStr } from 'utils/getErrorStr';
import { parcelUtil } from 'utils';

const dsSource = dotService.store['dot-service-data-set'];

export type ParcelInfoCtxProps = {
  selectedParcelInfo: GisMapDataFields | undefined;
  setSelectedParcelInfo: React.Dispatch<
    React.SetStateAction<GisMapDataFields | undefined>
  >;
  parcelStack: {
    major: string;
    minor: string;
  }[];
  setSummaryState: React.Dispatch<
    React.SetStateAction<'loading' | 'error' | 'success' | undefined>
  >;
  summaryState: 'loading' | 'error' | 'success' | undefined;
  pin: string | undefined;
  setPin: React.Dispatch<React.SetStateAction<string | undefined>>;
};

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const ParcelInfoContext = createContext<ParcelInfoCtxProps>(null as any);

export const isGisMapDataFields = (
  data: ParcelFeatureData | GisMapDataFields | undefined
): data is GisMapDataFields => {
  if (!data) return false;
  return 'GeneralClassif' in data;
};

export const ParcelInfoProvider = (
  props: PropsWithChildren<{ map: mapboxgl.Map }>
): JSX.Element => {
  const { map } = props;
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const currentPageMarker = useCurrentPageMarker();
  const [selectedParcelInfo, setSelectedParcelInfo] = useState<
    GisMapDataFields
  >();
  const [summaryState, setSummaryState] = useState<
    'loading' | 'error' | 'success'
  >();

  const clickedParcel = useObservable($onParcelClick);
  const dots = useSelectedDots();
  const [pin, setPin] = useState<string>();
  const [bbox, setBbox] = useState<{ pin: string; bbox: BBox | undefined }>();
  const pinRef = useRef<string>();
  const parcelStack = useObservable($onParcelStack, []);
  useEffect(() => {
    pinRef.current = pin;
  }, [pin]);

  useUpdateEffect(() => {
    if (!pin) return setBbox(undefined);
    if (pin === bbox?.pin) return;
    const bboxVal = parcelUtil.getParcelBbox(pin);
    if (!bboxVal) {
      map.once('idle', () => {
        if (pinRef.current === pin) {
          setBbox({
            pin,
            bbox: parcelUtil.getParcelBbox(pin),
          });
        }
      });
    } else {
      setBbox({
        pin,
        bbox: bboxVal,
      });
    }
  }, [map, pin]);

  /**
   * On selection service dots clear
   */
  useEffect(() => {
    if (!dots.length && selectionService.isEnabled()) {
      setSelectedParcelInfo(undefined);
    }
  }, [dots]);

  useAsync(async () => {
    if (!pin) return;
    if (!map) return;
    if (!bbox?.bbox) return;
    try {
      setSummaryState('loading');
      const featureData = await getParcelDetail(pin, bbox?.bbox);
      if (!featureData) {
        setSelectedParcelInfo(undefined);
        showErrorMessage({
          detail: 'Parcel ' + pin + ' not found',
          message: 'Parcel ' + pin + ' not found',
        });
        return setSummaryState('error');
      }
      if (!featureData?.PropType) {
        setSelectedParcelInfo(undefined);
        return setSummaryState('error');
      }
      if (selectionService.isEnabled() && !selectionService.selectedDots.size)
        return;
      setSelectedParcelInfo(featureData);
      setSummaryState('success');
    } catch (e) {
      if (axios.isCancel(e)) {
        return;
      }
      setSummaryState('error');
      showErrorMessage({
        detail: getErrorStr(e),
      });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pin, bbox]);

  /**
   * Ctl + Click event
   */
  useEffect(() => {
    if (!clickedParcel) return;
    if (selectionService.isEnabled()) return;
    if (!mapUtilService.controlKeyPressed) return;

    const currentDot = dsSource?.get(currentPageMarker.id);
    if (currentDot?.properties.pin === clickedParcel.feature.properties.PIN) {
      setSelectedParcelInfo(undefined);
      return currentPageMarker.remove();
    }

    const center = parcelUtil.getPointFromFeatures([clickedParcel.feature]);
    if (!center?.geometry?.coordinates)
      return showErrorMessage({
        detail:
          'Cant calculate mid point for parcel ' +
          clickedParcel.feature.properties.PIN,
      });

    currentPageMarker.set(
      clickedParcel.feature.properties.PIN,
      center.geometry.coordinates as [number, number]
    );

    setPin(clickedParcel?.feature.properties.PIN);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [clickedParcel]);

  useUpdateEffect(() => {
    if (!parcelStack.length) return;
    const first = parcelStack[0];
    setPin(first.major + first.minor);
  }, [parcelStack]);

  //Clear selected parcel info for control click
  useUpdateEffect(() => {
    if (selectionService.isEnabled()) return;
    if (!pin) {
      currentPageMarker.remove();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pin]);

  useEffect(() => {
    if (!pin) return setSelectedParcelInfo(undefined);
    if (selectedParcelInfo) {
      if (pin !== selectedParcelInfo.Major + selectedParcelInfo.Minor)
        return setSelectedParcelInfo(undefined);
    }
  }, [pin, selectedParcelInfo]);

  return (
    <ParcelInfoContext.Provider
      value={{
        pin,
        setPin,
        setSummaryState,
        summaryState,
        selectedParcelInfo,
        setSelectedParcelInfo,
        parcelStack,
      }}
    >
      {props.children}
    </ParcelInfoContext.Provider>
  );
};
