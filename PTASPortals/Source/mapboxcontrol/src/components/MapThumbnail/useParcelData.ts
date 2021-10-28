/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import axios from 'axios';
import { attempt } from 'lodash';
import { useContext, useEffect, useState } from 'react';
import { useAsync, usePrevious, useUpdateEffect } from 'react-use';
import {
  featureDataService,
  getParcelDetail,
} from 'contexts/ParcelInfoContext/service';
import { ErrorMessageAlertCtx } from '@ptas/react-ui-library';
import { apiService } from 'services/api';
import { GisMapDataFields } from 'services/map';
import { MapThumbnailService } from 'services/map/MapThumbnailService';
import { getErrorStr } from 'utils/getErrorStr';

type UseParcelData = {
  parcelDetails: GisMapDataFields | undefined;
  image: string | undefined;
  loading: boolean;
};

type Props = {
  pin: string | undefined;
  mediaToken: string | undefined;
  enableMapThumbnail: boolean;
  service: MapThumbnailService;
};

export const useThumbnailData = ({
  enableMapThumbnail,
  mediaToken,
  pin,
  service,
}: Props): UseParcelData => {
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const prevPin = usePrevious(pin);
  const [parcelDetails, setParcelDetails] = useState<GisMapDataFields>();
  const [loading, setLoading] = useState(false);
  const [image, setImage] = useState<string>();

  const clear = (options?: { image: boolean }): void => {
    setParcelDetails(undefined);
    options?.image !== false && setImage(undefined);
    setLoading(false);
    featureDataService.cancelRequest?.();
    apiService.parcelMedia.cancel?.();
  };

  useUpdateEffect(() => {
    if (!pin) {
      clear();
      service._popUp?.remove();
    }
  }, [pin]);

  useEffect(() => {
    service.enabled = enableMapThumbnail;
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [enableMapThumbnail]);

  useAsync(async () => {
    if (pin === prevPin) return;

    if (!pin) {
      clear();
      console.log('Pin is undefined');
      return;
    }

    if (!mediaToken) {
      return clear();
    }

    clear({ image: false });
    setLoading(true);

    (async (): Promise<void> => {
      try {
        const r = await (
          await apiService.parcelMedia.get(pin, mediaToken)
        ).shift();
        setImage(r);
      } catch (e) {
        return;
      }
    })();

    try {
      const res = await getParcelDetail(pin);
      if (!res) {
        clear();
        return showErrorMessage({
          detail: 'Parcel ' + pin + ' not found',
          message: 'Parcel ' + pin + ' not found',
        });
      }
      setLoading(false);
      setParcelDetails(res);
    } catch (e) {
      if (axios.isCancel(e)) return;
      showErrorMessage(getErrorStr(e));
      attempt(() => setLoading(false));
      console.error(e);
    }
  }, [pin, prevPin]);

  return { parcelDetails, image, loading };
};
