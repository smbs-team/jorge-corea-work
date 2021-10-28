/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useEffectOnce, useInterval } from 'react-use';
import { AppealMessage } from './types';

const postAppealMessage = (message: AppealMessage): void =>
  window.parent.postMessage(message, process.env.REACT_APP_EAPPEALS_URL);

export const usePostMessage = (): void => {
  useInterval(() => {
    postAppealMessage({
      type: 'test-type1',
      payload: {
        name: 'Eduardo Carrillo',
      },
    });
  }, 10000);

  useEffectOnce(() => {
    window.addEventListener(
      'message',
      (event) => {
        if (event.origin !== process.env.REACT_APP_EAPPEALS_URL) return;
        if (event.data.type && event.data.payload === undefined) return;
        const data = event.data as AppealMessage;
        console.log('Appeal message received in mapbox control');
        console.log(data);
      },
      false
    );
  });
};
