// helpers.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { DropDownItem } from '@ptas/react-ui-library';

export const dropdownItemMap = (array: string[]): DropDownItem[] =>
  (array ?? []).map((i) => ({ label: i, value: i }));
