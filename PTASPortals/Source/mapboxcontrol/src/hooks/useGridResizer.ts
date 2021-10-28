// useGridResizer.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useRef, useState } from 'react';
import { debounce } from 'lodash';
import { OnResizeFields } from 'libComponents/Resizable';

interface UseGridResizer {
  gridHeight: number;
  resizeGrid: (fields: OnResizeFields) => void;
}

export const useGridResizer = (initialHeight = 220): UseGridResizer => {
  const [gridHeight, setGridHeight] = useState<number>(initialHeight);

  const resizeGrid = useRef(
    debounce((fields: OnResizeFields): void => {
      //if (fields.height < fields.initialRootRect.height) return;
      const newGridHeight = gridHeight + fields.diff.height;
      setGridHeight(newGridHeight);
    }, 40)
  );

  return {
    gridHeight,
    resizeGrid: resizeGrid.current,
  };
};
