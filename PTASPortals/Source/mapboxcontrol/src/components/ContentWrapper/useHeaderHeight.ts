/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { DrawToolBarContext } from 'contexts/DrawToolbarContext';
import { useContext, useEffect, useState } from 'react';

export const useHeaderHeight = (): number => {
  const { actionMode } = useContext(DrawToolBarContext);
  const [headerHeight, setHeaderHeight] = useState<number>(48);
  useEffect(() => {
    if (actionMode === 'measure' || actionMode === 'draw') {
      setHeaderHeight(92);
    } else {
      setHeaderHeight(48);
    }
  }, [actionMode]);
  return headerHeight;
};
