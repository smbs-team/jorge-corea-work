/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Dispatch, SetStateAction, useContext, useState } from 'react';
import { useAsync } from 'react-use';
import { ErrorMessageAlertCtx } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import noImage from 'assets/img/no-image.png';
import { layerService } from 'services/map';
import { apiService } from 'services/api';
import { ParcelInfoContext } from 'contexts/ParcelInfoContext';
import { getErrorStr } from 'utils/getErrorStr';
import axios from 'axios';

type UseParcelImages = [string[], Dispatch<SetStateAction<string[]>>];

export const useParcelImages = (): UseParcelImages => {
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const { mediaToken } = useContext(HomeContext);
  const { pin } = useContext(ParcelInfoContext);
  const [imagesUrls, setImagesUrls] = useState<string[]>([]);

  useAsync(async () => {
    if (!mediaToken || !pin) {
      setImagesUrls([noImage]);
      return;
    }

    if (!layerService.hasParcelLayer()) return;
    try {
      const imageResponse = await apiService.parcelMedia.get(pin, mediaToken);
      if (imageResponse.length) {
        setImagesUrls(imageResponse);
      }
    } catch (e) {
      if (axios.isCancel(e)) return;
      showErrorMessage(getErrorStr(e));
    }
  }, [mediaToken, pin]);

  return [imagesUrls, setImagesUrls];
};
