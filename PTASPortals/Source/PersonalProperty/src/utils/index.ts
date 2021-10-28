// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { DropDownItem } from '@ptas/react-public-ui-library';

export function dropdownOptionsMapping<Type, Key extends keyof Type>(
  options: Type[],
  labelKey: Key,
  valueKey: Key
): DropDownItem[] {
  return options.map(o => ({
    label: (o[labelKey] as unknown) as string,
    value: (o[valueKey] as unknown) as number | string,
  }));
}
