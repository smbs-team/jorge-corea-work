//overlapCalcutator.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface OverlapCalculatorRes {
  parcelPIN: string;
  layerID: string;
  overlapPercentage: number;
  overlapArea: number;
  parcelArea: number;
  additionalFields: object;
}
