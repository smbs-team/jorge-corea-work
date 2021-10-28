/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useObservable } from 'react-use';
import { DotFeature } from 'services/map/dot';
import selectionService from 'services/parcel/selection';

export const useSelectedDots = (): DotFeature[] =>
  useObservable(selectionService.$onDotsChange, []);
