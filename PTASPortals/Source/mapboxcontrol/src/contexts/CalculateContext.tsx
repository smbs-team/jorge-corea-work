/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AppContext } from 'contexts';
import React, {
  createContext,
  Dispatch,
  PropsWithChildren,
  SetStateAction,
  useContext,
  useState,
} from 'react';
import { useUpdateEffect } from 'react-use';
import {
  DrivingTimeRowData,
  WalkingDistanceRowData,
} from 'services/map/model/directions';

export type CalculateParcelsOp = 'selected' | 'visible';

type Props = {
  walkingDistanceParcelRows: WalkingDistanceRowData[];
  setWalkingDistanceParcelRows: React.Dispatch<
    React.SetStateAction<WalkingDistanceRowData[]>
  >;
  drivingTimeParcelRows: DrivingTimeRowData[];
  setDrivingTimeParcelRows: React.Dispatch<
    React.SetStateAction<DrivingTimeRowData[]>
  >;
  walkingDistanceAddress: string | undefined;
  setWalkingDistanceAddress: React.Dispatch<React.SetStateAction<string>>;
  walkingDistanceCoords: { lat: number; lng: number } | undefined;
  setWalkingDistanceCoords: React.Dispatch<
    React.SetStateAction<{ lat: number; lng: number } | undefined>
  >;
  drivingTimeAddress: string | undefined;
  setDrivingTimeAddress: React.Dispatch<React.SetStateAction<string>>;
  drivingTimeCoords: { lat: number; lng: number } | undefined;
  setDrivingTimeCoords: React.Dispatch<
    React.SetStateAction<{ lat: number; lng: number } | undefined>
  >;
  wdDestinationParcelOption: CalculateParcelsOp | undefined;
  setWdDestinationParcelOption: React.Dispatch<
    React.SetStateAction<CalculateParcelsOp>
  >;
  dtDestinationParcelOption: CalculateParcelsOp | undefined;
  setDtDestinationParcelOption: React.Dispatch<
    React.SetStateAction<CalculateParcelsOp>
  >;
  wdStartPointOption: boolean;
  setWdStartPointOption: Dispatch<SetStateAction<boolean>>;
  dtStartPointOption: boolean;
  setDtStartPointOption: Dispatch<SetStateAction<boolean>>;
  fetching: boolean;
  setFetching: Dispatch<SetStateAction<boolean>>;
};
/**
 * A context for calculate walking distance and driving time
 */
// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const CalculateContext = createContext<Props>(null as any);

export const CalculateProvider = (
  props: PropsWithChildren<object>
): JSX.Element => {
  const { setShowBackdrop } = useContext(AppContext);
  const [wdStartPointOption, setWdStartPointOption] = useState(false);
  const [dtStartPointOption, setDtStartPointOption] = useState(false);
  const [wdDestinationParcelOption, setWdDestinationParcelOption] = useState<
    CalculateParcelsOp
  >('selected');
  const [dtDestinationParcelOption, setDtDestinationParcelOption] = useState<
    CalculateParcelsOp
  >('selected');
  const [walkingDistanceParcelRows, setWalkingDistanceParcelRows] = useState<
    WalkingDistanceRowData[]
  >([]);
  const [drivingTimeParcelRows, setDrivingTimeParcelRows] = useState<
    DrivingTimeRowData[]
  >([]);
  const [walkingDistanceAddress, setWalkingDistanceAddress] = useState<string>(
    ''
  );
  const [walkingDistanceCoords, setWalkingDistanceCoords] = useState<
    { lat: number; lng: number } | undefined
  >();
  const [drivingTimeAddress, setDrivingTimeAddress] = useState<string>('');
  const [drivingTimeCoords, setDrivingTimeCoords] = useState<
    { lat: number; lng: number } | undefined
  >();
  const [fetching, setFetching] = useState(false);

  useUpdateEffect(() => {
    setShowBackdrop(fetching);
  }, [fetching, setShowBackdrop]);

  return (
    <CalculateContext.Provider
      value={{
        fetching,
        setFetching,
        dtStartPointOption,
        setDtStartPointOption,
        wdStartPointOption,
        setWdStartPointOption,
        dtDestinationParcelOption,
        wdDestinationParcelOption,
        setWdDestinationParcelOption,
        setDtDestinationParcelOption,
        drivingTimeAddress,
        walkingDistanceParcelRows,
        walkingDistanceCoords,
        walkingDistanceAddress,
        setWalkingDistanceParcelRows,
        setWalkingDistanceCoords,
        drivingTimeCoords,
        setWalkingDistanceAddress,
        setDrivingTimeParcelRows,
        setDrivingTimeCoords,
        setDrivingTimeAddress,
        drivingTimeParcelRows,
      }}
    >
      {props.children}
    </CalculateContext.Provider>
  );
};
