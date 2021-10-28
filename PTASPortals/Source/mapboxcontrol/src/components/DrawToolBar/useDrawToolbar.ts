/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState, useEffect } from 'react';
import { useObservable } from 'react-use';
import { map } from 'rxjs/operators';
import { mapBoxDrawService } from 'services/map/mapboxDraw';
import { $areaTextChange } from 'services/map/mapboxDraw/measure';

export const useMeasureAreaText = (): string | undefined =>
  useObservable($areaTextChange.pipe(map((val) => val?.text)));

export const useHasMeasureFeatures = (): boolean => {
  const [val, setVal] = useState(false);
  useEffect(() => {
    const sub = mapBoxDrawService.measure?.$onChange.subscribe((store) => {
      setVal(!!store.size);
    });
    return (): void => {
      sub?.unsubscribe();
    };
  }, []);
  return val;
};
