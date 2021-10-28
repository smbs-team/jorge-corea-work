// types.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface ColorConfiguration {
  id: string;
  colorSet1?: string[];
  colorSet2?: string[];
  type?: "random" | "sequential" | "diverging" | string;
  colors?: string[];
  selectedTab?: number;
  opacity?: number;
}

export interface SelectedRamp {
  id: string;
  colors: string[];
  selectedTab?: number;
}
