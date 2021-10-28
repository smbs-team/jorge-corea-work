/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { SnackContext } from '@ptas/react-ui-library';
import { ParcelInfoContext } from 'contexts/ParcelInfoContext';
import { useContext } from 'react';
import { parcelUtil } from 'utils';

export const useViewOptionClick = (): ((action: string | number) => void) => {
  const { selectedParcelInfo } = useContext(ParcelInfoContext);
  const { setSnackState } = useContext(SnackContext);

  const onParcelDetailClick = (): void => {
    if (
      selectedParcelInfo?.['ptas_parcelDetailId'] &&
      process.env.REACT_APP_DYNAMICS_PARCEL_DETAILS_URL
    ) {
      window.open(
        process.env.REACT_APP_DYNAMICS_PARCEL_DETAILS_URL +
          selectedParcelInfo['ptas_parcelDetailId']
      );
    }
  };

  const onObliquesClick = (): void => {
    if (!selectedParcelInfo?.Major) return;
    if (!selectedParcelInfo?.Minor) return;

    const features = parcelUtil.getFeaturesByPin(
      selectedParcelInfo.Major + selectedParcelInfo.Minor
    );
    const centerPoint = parcelUtil.getPointFromFeatures(features);
    if (centerPoint?.geometry?.coordinates.length) {
      const lng = centerPoint?.geometry?.coordinates[0];
      const lat = centerPoint?.geometry?.coordinates[1];
      window.open(`${process.env.REACT_APP_PICTOMETRY_URL}?y=${lat}&x=${lng}`);
    } else {
      setSnackState?.({
        severity: 'info',
        text: 'Parcel does not have sufficient data',
      });
    }
  };

  const onStreetViewClick = (): void => {
    if (!selectedParcelInfo?.Major) return;
    if (!selectedParcelInfo?.Minor) return;
    const features = parcelUtil.getFeaturesByPin(
      selectedParcelInfo.Major + selectedParcelInfo.Minor
    );
    const centerPoint = parcelUtil.getPointFromFeatures(features);
    if (centerPoint?.geometry?.coordinates.length) {
      const lng = centerPoint?.geometry?.coordinates[0];
      const lat = centerPoint?.geometry?.coordinates[1];
      window.open(
        `http://maps.google.com/maps?q=&layer=c&cbll=${lat},${lng}&cbp=11,0,0,0,0`,
        '_blank'
      );
    } else {
      setSnackState?.({
        severity: 'info',
        text: 'Parcel does not have sufficient data',
      });
    }
  };

  const onBookmarksClick = (): void => {
    if (!selectedParcelInfo?.Major) return;
    if (!selectedParcelInfo?.Minor) return;
    const features = parcelUtil.getFeaturesByPin(
      selectedParcelInfo.Major + selectedParcelInfo.Minor
    );
    const centerPoint = parcelUtil.getPointFromFeatures(features);
    const bookmarksUrl = process.env.REACT_APP_DYNAMICS_BOOKMARKS_URL;
    if (centerPoint?.geometry?.coordinates.length && bookmarksUrl) {
      window.open(bookmarksUrl);
    } else {
      setSnackState?.({
        severity: 'info',
        text: 'Parcel does not have sufficient data',
      });
    }
  };

  return (action: string | number): void => {
    switch (action) {
      case 'parcelDetail':
        onParcelDetailClick();
        break;
      case 'obliques':
        onObliquesClick();
        break;
      case 'streetView':
        onStreetViewClick();
        break;
      case 'bookmarks':
        onBookmarksClick();
        break;
    }
  };
};
