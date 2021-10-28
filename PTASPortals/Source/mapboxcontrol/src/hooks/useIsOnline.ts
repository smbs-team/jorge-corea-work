/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useEffect, useState } from 'react';

export const useIsOnline = (): boolean | undefined => {
  const [isOnline, setIsOnline] = useState<boolean>();
  useEffect(() => {
    const fn = (e: Event): ReturnType<typeof window['addEventListener']> => {
      setIsOnline(e.type === 'online');
    };
    window.addEventListener('online', fn);
    window.addEventListener('offline', fn);
    return (): void => {
      window.removeEventListener('online', fn);
      window.removeEventListener('offline', fn);
    };
  }, []);
  return isOnline;
};
