// FILENAME
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { DropDownItem } from '@ptas/react-ui-library';
import { AxiosLoader } from 'services/AxiosLoader';

export interface IdLabel {
  id: string | number;
  label: string;
}

export type ValuesHolder = {
  [id: string]: IdLabel[];
};

export type StringMapType =
  | 'ptas_valuemethodcalculation'
  | 'ptas_nuisancetype'
  | 'ptas_viewtype'
  | 'ptas_quality'
  | 'ptas_noiselevel'
  | 'ptas_characteristictype';

export const StringMapProperties = [
  'ptas_valuemethodcalculation',
  'ptas_nuisancetype',
  'ptas_viewtype',
  'ptas_quality',
  'ptas_noiselevel',
  'ptas_characteristictype',
].map((s): { objectTypeCode: string; attributeName: string } => ({
  objectTypeCode: 'ptas_landvaluecalculation',
  attributeName: s,
}));

export function dropdownItemFromApi(
  viewTypes: IdLabel[]
): React.SetStateAction<DropDownItem[]> {
  return viewTypes.map(
    (lv): DropDownItem => ({ label: lv.label, value: lv.id })
  );
}

const getValues = async (): Promise<ValuesHolder> => {
  const loader = new AxiosLoader<
    {
      stringMapValues: {
        attributeValue: string;
        attributeName: string;
        value: string;
      }[];
    },
    {}
  >();
  const r = await loader.PutInfo(
    'CustomSearches/GetStringMapValues',
    { StringMapProperties },
    {}
  );
  if (!r) return {};
  const items = r.stringMapValues.map(v => ({
    id: v.attributeValue,
    label: v.value,
    key: v.attributeName,
  }));
  const results = items.reduce((prev: ValuesHolder, curr): ValuesHolder => {
    const { key, ...rest } = curr;
    prev[key] = [...(prev[key] ?? []), rest];
    return prev;
  }, {} as ValuesHolder);
  return results;
};

type getValueFunction = (key: StringMapType) => Promise<IdLabel[]>;

export const ValuesLoader: getValueFunction = ((): getValueFunction => {
  let values: ValuesHolder | null = null;
  return async (key: StringMapType): Promise<IdLabel[]> => {
    if (!values) values = await getValues();
    return values[key] || [];
  };
})();
