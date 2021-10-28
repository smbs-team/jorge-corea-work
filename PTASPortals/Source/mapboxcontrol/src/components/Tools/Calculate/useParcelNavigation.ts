// useParcelNavigation.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AnySourceData, LineLayer } from 'mapbox-gl';
import { Geometry } from 'geojson/index';
import { groupBy } from 'lodash';
import { Feature, Point } from '@turf/turf';
import { AutoCompleteRow, SnackContext } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import { useCursorMap } from 'hooks/map/useCursorMap';
import { useLocationFound } from 'hooks/map/useLocationFind';
import { useMapClick } from 'hooks/map/useMapClick';
import React, { useContext, useEffect, useRef, useState } from 'react';
import { mapService } from 'services/map';
import directionsService from 'services/map/directions/directionsService';
import selectionService from 'services/parcel/selection';
import {
  DrivingTimeRowData,
  MapboxDirections,
  WalkingDistanceRowData,
} from 'services/map/model/directions';
import { apiService } from 'services/api';
import { parcelUtil } from 'utils';
import { PinService } from 'services/map/pin/PinService';
import { useGlMap } from 'hooks/map/useGlMap';
import {
  CalculateContext,
  CalculateParcelsOp,
} from 'contexts/CalculateContext';

export type Order = 'asc' | 'desc';

export interface HeadCellData {
  id: string;
  label: string;
  width: string;
}

interface UseParcelNavigation<
  T extends WalkingDistanceRowData | DrivingTimeRowData
> extends ReturnType<typeof useLocationFound> {
  order: Order;
  setOrder: React.Dispatch<React.SetStateAction<Order>>;
  orderBy: keyof T;
  setOrderBy: React.Dispatch<React.SetStateAction<keyof T>>;
  destinationParcelRadioItems: {
    value: CalculateParcelsOp;
    label: string;
  }[];
  headCells: {
    id: string;
    label: string;
    width: string;
  }[];
  onClickSetStartingPoint: () => void;
  onClickClearData: () => void;
  onClickCalculate: () => void;
  onTextFieldValueChange: (text: string) => void;
  onSuggestionSelected: (val: AutoCompleteRow) => void;
  onIconClick: (text: string) => void;
  clickOnDirectionRow: (
    row: WalkingDistanceRowData | DrivingTimeRowData,
    onlyRemoveRoute?: boolean
  ) => void;
}

export const useParcelNavigation = <
  T extends WalkingDistanceRowData | DrivingTimeRowData
>(props: {
  navigationType: 'walking' | 'driving';
  component: JSX.Element;
}): UseParcelNavigation<T> => {
  const pinService = useRef<PinService>();
  const { setSnackState } = useContext(SnackContext);
  const { navigationType, component } = props;
  const [order, setOrder] = useState<Order>('asc');
  const [orderBy, setOrderBy] = useState<keyof T>('parcel');
  const {
    setWdStartPointOption,
    setDtStartPointOption: setDtWdStartPointOption,
  } = useContext(CalculateContext);
  const {
    autoCompleteData,
    searchByAddress,
    searchResult,
    runSearch,
  } = useLocationFound();
  const { setPanelContent } = useContext(HomeContext);
  const {setFetching }=useContext(CalculateContext)
  const {
    wdDestinationParcelOption,
    dtDestinationParcelOption,
    setWalkingDistanceParcelRows,
    setDrivingTimeParcelRows,
    setWalkingDistanceAddress,
    walkingDistanceCoords,
    setWalkingDistanceCoords,
    setDrivingTimeAddress,
    drivingTimeCoords,
    setDrivingTimeCoords,
  } = useContext(CalculateContext);
  const mapCursor = useCursorMap();
  const mapClick = useMapClick();
  const map = useGlMap();

  const destinationParcelRadioItems: {
    value: CalculateParcelsOp;
    label: string;
  }[] = [
    { value: 'selected', label: 'Selected parcels' },
    { value: 'visible', label: 'Visible parcels' },
  ];

  const headCells: HeadCellData[] = [
    { id: 'parcel', label: 'Parcel', width: '100px' },
    navigationType === 'walking'
      ? { id: 'distance', label: 'Distance', width: '100px' }
      : { id: 'time', label: 'Time', width: '100px' },
    { id: 'address', label: 'Address', width: '100%' },
  ];

  useEffect(() => {
    if (!map) return;
    pinService.current = new PinService(map);
  }, [map]);

  useEffect(() => {
    (async (): Promise<void> => {
      if (mapClick.event) {
        const {
          lngLat: { lng, lat },
        } = mapClick.event;
        const newAddressName = await getAddress(lng, lat);

        if (navigationType === 'walking') {
          setWalkingDistanceAddress(newAddressName);
          setWalkingDistanceCoords({ lat: lat, lng: lng });
          pinService.current?.setPinToMap(
            lat,
            lng,
            'walkingDistancePin',
            onPinClick,
            '#ffff00'
          );
        } else {
          setDrivingTimeAddress(newAddressName);
          setDrivingTimeCoords({ lat: lat, lng: lng });
          pinService.current?.setPinToMap(
            lat,
            lng,
            'drivingTimePin',
            onPinClick
          );
        }
        mapCursor.changeCursor('');
      }
    })();
  }, [mapClick.event]);

  useEffect(() => {
    if (searchResult) {
      const { lat, lng } = searchResult;
      if (navigationType === 'walking') {
        pinService.current?.setCenterWithPinToMap(
          lat,
          lng,
          'walkingDistancePin',
          onPinClick,
          '#ffff00'
        );
          setWalkingDistanceCoords({ lat: lat, lng: lng });
      } else {
        pinService.current?.setCenterWithPinToMap(
          lat,
          lng,
          'drivingTimePin',
          onPinClick
        );
        setDrivingTimeCoords({ lat: lat, lng: lng });
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [searchResult]);

  const onPinClick = (): void => {
    setPanelContent(component);
  };

  const clickOnDirectionRow = (
    row: WalkingDistanceRowData | DrivingTimeRowData,
    onlyRemoveRoute?: boolean
  ): void => {
    if (onlyRemoveRoute) {
      removeRouteLayerAndSource();
    } else {
      if (
        row.directionsData &&
        row.directionsData.routes.length &&
        row.directionsData.routes[0].geometry
      ) {
        addRouteLayer(row.directionsData.routes[0].geometry);
      }
    }
  };

  const addRouteLayer = (routeGeometry: Geometry): void => {
    if (!mapService.map) return;
    removeRouteLayerAndSource();
    addRouteSource(routeGeometry);
    const routeLayer: LineLayer = {
      id: navigationType === 'walking' ? 'wdRoute' : 'dtRoute',
      type: 'line',
      source: navigationType === 'walking' ? 'wdRouteSource' : 'dtRouteSource',
      layout: {
        'line-join': 'round',
        'line-cap': 'round',
      },
      paint: {
        'line-color': navigationType === 'walking' ? '#ffff00' : '#3887be',
        'line-width': 5,
        'line-opacity': 0.75,
      },
    };
    mapService.map.addLayer(routeLayer);
  };

  const removeRouteLayerAndSource = (): void => {
    if (!mapService.map) return;
    if (navigationType === 'walking') {
      if (mapService.map.getLayer('wdRoute')) {
        mapService.map.removeLayer('wdRoute');
      }
      if (mapService.map.getSource('wdRouteSource')) {
        mapService.map.removeSource('wdRouteSource');
      }
    } else if (navigationType === 'driving') {
      if (mapService.map.getLayer('dtRoute')) {
        mapService.map.removeLayer('dtRoute');
      }
      if (mapService.map.getSource('dtRouteSource')) {
        mapService.map.removeSource('dtRouteSource');
      }
    }
  };

  const addRouteSource = (routeGeometry: Geometry): void => {
    if (!mapService.map) return;
    const routeSource: AnySourceData = {
      type: 'geojson',
      data: {
        type: 'Feature',
        properties: {},
        geometry: routeGeometry,
      },
    };
    mapService.map.addSource(
      navigationType === 'walking' ? 'wdRouteSource' : 'dtRouteSource',
      routeSource
    );
  };

  const getAddress = async (lng: number, lat: number): Promise<string> => {
    const addressResp = await apiService.getAddressByLocation(lng, lat);
    const newAddressName = addressResp.data?.addressFull ?? '';
    return newAddressName;
  };

  const onClickSetStartingPoint = (): void => {
    PinService.removeMarker();
    mapClick.capture();
    mapCursor.changeCursor('crosshair');
    selectionService.changeSelection('disabled');
    if (navigationType === 'walking') {
      setWdStartPointOption(prev => {
        if (prev) {
          onClickClearData();
        }
        return !prev;
      });
    } else {
      setDtWdStartPointOption(prev => {
        if (prev) {
          onClickClearData();
        }
        return !prev;
      });
    }
  };

  const onClickClearData = (): void => {
    if (navigationType === 'walking') {
      setWdStartPointOption(false);
    } else {
      setDtWdStartPointOption(false);
    }
    PinService.removeMarker();
    if (navigationType === 'walking') {
      setWalkingDistanceParcelRows([]);
      setWalkingDistanceAddress('');
      setWalkingDistanceCoords(undefined);
    } else {
      setDrivingTimeParcelRows([]);
      setDrivingTimeAddress('');
      setDrivingTimeCoords(undefined);
    }
    removeRouteLayerAndSource();
  };

  const onClickCalculate = async (): Promise<void> => {
    if (navigationType === 'walking') {
      onClickCalculateWalkingDistance();
    } else {
      onClickCalculateDrivingTime();
    }
  };

  const onClickCalculateWalkingDistance = async (): Promise<void> => {
    if (
      !walkingDistanceCoords ||
      !walkingDistanceCoords.lat ||
      !walkingDistanceCoords.lng
    ) {
      setSnackState({
        severity: 'warning',
        text: 'Please set a starting point.',
      });
      return;
    }
    setWalkingDistanceParcelRows([]);
    setFetching(true);
    if (wdDestinationParcelOption === 'selected') {
      let featureList = selectionService.getSelectedDotsList();
      if (!featureList || !featureList.length) {
        setSnackState({
          severity: 'info',
          text: 'No parcel selected.',
        });
        setFetching(false);
        return;
      }
      if (featureList && featureList.length > 300) {
        featureList = featureList.slice(0, 300);

        setSnackState({
          severity: 'info',
          text:
            'Request exceeds limit of 300 parcels. Processing 300 distances...',
        });
      }

      for (const feature of featureList) {
        const _lng = feature.geometry?.coordinates[0];
        const _lat = feature.geometry?.coordinates[1];
        const _address = _lng && _lat ? await getAddress(_lng, _lat) : '';
        let _distance: number | undefined = 0;
        let _directionsData: MapboxDirections;
        if (_lat && _lng) {
          const res = await directionsService.getWalkingDistance(
            { lat: walkingDistanceCoords.lat, lng: walkingDistanceCoords.lng },
            { lat: _lat, lng: _lng }
          );
          if (res) {
            _distance = res.distance;
            _directionsData = res.directionsData;
          }
          if (_distance) _distance = Math.floor(_distance);
        }
        setWalkingDistanceParcelRows(prev => {
          const newRow = {
            parcel: parcelUtil.formatPin(feature.properties.pin),
            distance: _distance ?? 0,
            address: _address ? _address : '*insufficient data*',
            directionsData: _directionsData,
          };
          return [...prev, newRow];
        });
      }
    } else if (wdDestinationParcelOption === 'visible') {
      const renderedFeatures = parcelUtil.getRenderedFeatures();
      if (!renderedFeatures.length) {
        setSnackState({
          severity: 'info',
          text: 'No parcel rendered.',
        });
        setFetching(false);
        return;
      }

      const groupedFeatures = groupBy(renderedFeatures, item => item.id);
      const features: {
        id: string;
        pin: string;
        centerPoint: Feature<Point>;
      }[] = [];
      for (const featureId in groupedFeatures) {
        try {
          if (groupedFeatures[featureId].length) {
            const centerPoint = parcelUtil.getPointFromFeatures(
              groupedFeatures[featureId]
            );
            if (centerPoint) {
              features.push({
                id: featureId,
                pin: groupedFeatures[featureId][0].properties.PIN,
                centerPoint: centerPoint,
              });
            }
          }
        } catch (e) {
          console.error(e);
        }
      }

      if (features.length > 300) {
        features.length = 300;
        setSnackState({
          severity: 'info',
          text:
            'Request exceeds limit of 300 parcels. Processing 300 distances...',
        });
      }

      for (const feature of features) {
        let _distance: number | undefined = 0;
        let _directionsData: MapboxDirections;
        let _address = '';

        if (feature.centerPoint.geometry?.coordinates.length) {
          const _lng = feature.centerPoint.geometry.coordinates[0];
          const _lat = feature.centerPoint.geometry.coordinates[1];
          _address = _lng && _lat ? await getAddress(_lng, _lat) : '';
          if (_lat && _lng) {
            const res = await directionsService.getWalkingDistance(
              {
                lat: walkingDistanceCoords.lat,
                lng: walkingDistanceCoords.lng,
              },
              { lat: _lat, lng: _lng }
            );
            if (res) {
              _distance = res.distance;
              _directionsData = res.directionsData;
            }
            if (_distance) _distance = Math.floor(_distance);
          }
        }
        setWalkingDistanceParcelRows(prev => {
          return [
            ...prev,
            {
              parcel: parcelUtil.formatPin(feature.pin),
              distance: _distance ?? 0,
              address: _address ? _address : '*insufficient data*',
              directionsData: _directionsData,
            },
          ];
        });
      }
    }
    setFetching(false);
  };

  const onClickCalculateDrivingTime = async (): Promise<void> => {
    if (
      !drivingTimeCoords ||
      !drivingTimeCoords.lat ||
      !drivingTimeCoords.lng
    ) {
      setSnackState({
        severity: 'warning',
        text: 'Please set a starting point.',
      });
      return;
    }
    setDrivingTimeParcelRows([]);
    setFetching(true);
    if (dtDestinationParcelOption === 'selected') {
      let featureList = selectionService.getSelectedDotsList();
      if (!featureList || !featureList.length) {
        setSnackState({
          severity: 'info',
          text: 'No parcel selected.',
        });
        setFetching(false);
        return;
      }
      if (featureList && featureList.length > 300) {
        featureList = featureList.slice(0, 300);

        setSnackState({
          severity: 'info',
          text:
            'Request exceeds limit of 300 parcels. Processing 300 driving times...',
        });
      }

      for (const feature of featureList) {
        const _lng = feature.geometry?.coordinates[0];
        const _lat = feature.geometry?.coordinates[1];
        const _address = _lng && _lat ? await getAddress(_lng, _lat) : '';
        let _drivingTime: number | undefined = 0;
        let _directionsData: MapboxDirections;
        if (_lat && _lng) {
          const res = await directionsService.getDrivingTime(
            { lat: drivingTimeCoords.lat, lng: drivingTimeCoords.lng },
            { lat: _lat, lng: _lng }
          );
          if (res) {
            _drivingTime = res.duration;
            _directionsData = res.directionsData;
          }
          if (_drivingTime) _drivingTime = Math.ceil(_drivingTime);
        }
        setDrivingTimeParcelRows(prev => {
          return [
            ...prev,
            {
              parcel: parcelUtil.formatPin(feature.properties.pin),
              time: _drivingTime ?? 0,
              address: _address ? _address : '*insufficient data*',
              directionsData: _directionsData,
            },
          ];
        });
      }
    } else if (dtDestinationParcelOption === 'visible') {
      const renderedFeatures = parcelUtil.getRenderedFeatures();
      if (!renderedFeatures.length) {
        setSnackState({
          severity: 'info',
          text: 'No parcel rendered.',
        });
        setFetching(false);
        return;
      }

      const groupedFeatures = groupBy(renderedFeatures, item => item.id);
      let features: {
        id: string;
        pin: string;
        centerPoint: Feature<Point>;
      }[] = [];
      for (const featureId in groupedFeatures) {
        try {
          if (groupedFeatures[featureId].length) {
            const centerPoint = parcelUtil.getPointFromFeatures(
              groupedFeatures[featureId]
            );
            if (centerPoint) {
              features.push({
                id: featureId,
                pin: groupedFeatures[featureId][0].properties.PIN,
                centerPoint: centerPoint,
              });
            }
          }
        } catch (e) {
          console.error(e);
        }
      }

      if (features && features.length > 300) {
        features.length = 300;
        setSnackState({
          severity: 'info',
          text:
            'Request exceeds limit of 300 parcels. Processing 300 driving times...',
        });
      }

      for (const feature of features) {
        let _drivingTime: number | undefined = 0;
        let _directionsData: MapboxDirections;
        let _address = '';

        if (feature.centerPoint.geometry?.coordinates.length) {
          const _lng = feature.centerPoint.geometry.coordinates[0];
          const _lat = feature.centerPoint.geometry.coordinates[1];
          _address = _lng && _lat ? await getAddress(_lng, _lat) : '';
          if (_lat && _lng) {
            const res = await directionsService.getDrivingTime(
              { lat: drivingTimeCoords.lat, lng: drivingTimeCoords.lng },
              { lat: _lat, lng: _lng }
            );
            if (res) {
              _drivingTime = res.duration;
              _directionsData = res.directionsData;
            }
            if (_drivingTime) _drivingTime = Math.ceil(_drivingTime);
          }
        }

        setDrivingTimeParcelRows(prev => {
          return [
            ...prev,
            {
              parcel: parcelUtil.formatPin(feature.pin),
              time: _drivingTime ?? 0,
              address: _address ? _address : '*insufficient data*',
              directionsData: _directionsData,
            },
          ];
        });
      }
    }
    setFetching(false);
  };

  const onTextFieldValueChange = (text: string): void => {
    if (navigationType === 'walking') {
      setWalkingDistanceAddress(text);
    } else {
      setDrivingTimeAddress(text);
    }
    searchByAddress(text);
  };

  const onSuggestionSelected = (val: AutoCompleteRow): void => {
    const addressValue = val.title;
    if (navigationType === 'walking') {
      setWalkingDistanceAddress(addressValue);
    } else {
      setDrivingTimeAddress(addressValue);
    }
    runSearch(addressValue);
  };

  const onIconClick = (text: string): void => {
    runSearch(text);
  };

  return {
    order,
    setOrder,
    orderBy,
    setOrderBy,
    destinationParcelRadioItems,
    headCells,
    onClickSetStartingPoint,
    onClickClearData,
    onClickCalculate,
    onTextFieldValueChange,
    onSuggestionSelected,
    onIconClick,
    autoCompleteData,
    searchByAddress,
    searchResult,
    runSearch,
    clickOnDirectionRow,
  };
};
