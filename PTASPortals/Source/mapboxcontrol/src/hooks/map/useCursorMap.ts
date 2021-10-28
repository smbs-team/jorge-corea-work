/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useEffect, useState } from 'react';
import { useGlMap } from './useGlMap';

type CursorMap = {
  changeCursor: (cursor: string) => void;
};

export const useCursorMap = (): CursorMap => {
  const map = useGlMap();
  const [cursor, setCursor] = useState<string>('');
  useEffect(() => {
    if (!map) return;
    map.on('mousemove', placePinMove);
    return (): void => {
      map.off('mousemove', placePinMove);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [cursor]);

  const placePinMove = (): void => {
    if (!map) return;
    map.getCanvas().style.cursor = cursor;
  };

  const changeCursor = (c: string): void => {
    if (!map) return;
    setCursor(c);
  };

  return {
    changeCursor,
  };
};
