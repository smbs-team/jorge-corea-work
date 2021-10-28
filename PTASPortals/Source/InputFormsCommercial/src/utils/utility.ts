// utility.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CustomRowData } from '@ptas/react-ui-library';
import { ValueModifier } from './../stores/usePageManagerStore';
import { AdjustmentFactorOverrides, IncomeRates } from 'components/home/areas/types';

interface UnknownObject {
  [key: string]: string;
}

export function addAfter<T>(array: T[], index: number, newItem: T): T[] {
  return [...array.slice(0, index), newItem, ...array.slice(index)];
}

export function reorder<T>(
  list: T[],
  startIndex: number,
  endIndex: number
): T[] {
  const result = Array.from(list);
  const [removed] = result.splice(startIndex, 1);
  result.splice(endIndex, 0, removed);

  return result;
}

export function forEachEntry<T>(
  node: T,
  callback: (value: string, key: string) => string,
  ignore?: string[]
): UnknownObject {
  if (!node) return {};
  const newNode: UnknownObject = {};
  Object.entries(node).forEach(([key, val]) => {
    if (ignore?.includes(key)) return (newNode[key] = val);
    return (newNode[key] = callback(val, key));
  });
  return newNode;
}

export const valueGridCalculation = (value: string): string => {
  const result =
    (parseFloat(value) * (1 - 6.0 * 0.01) * (1 - 7.5 * 0.01)) / (6.5 * 0.01);
  return result.toString();
};

export const roundBy = (roundBy: number, value: string): string => {
  return parseFloat(
    (Math.round(parseFloat(value) / roundBy) * roundBy).toFixed(2)
  ).toString();
};

export const percentChangeCalculation = (
  value: string,
  oldValue: string
): string => {
  if (!value || !oldValue) return '0';
  const current = parseFloat(value.split(',').join(''));
  const old = parseFloat(oldValue.split(',').join(''));
  if (!old || !current) return '0';
  const result = 100 * ((current - old) / old);
  return result.toFixed(2);
};

export function calculateValuesGridValues<T>(
  data: CustomRowData<T>[],
  leaseRate: ValueModifier
): CustomRowData<T>[] {
  return data.map((item) => {
    return {
      ...item,
      data: forEachEntry(
        item.data,
        (value) => {
          if (isNaN(parseInt(value))) return value;
          return roundBy(0.05, valueGridCalculation(value as string));
        },
        ['id', 'minEff', 'maxEff']
      ) as unknown as T,
    } as CustomRowData<T>;
  });
}

export function applyAdjustmentFactor<T>(
  data: CustomRowData<T>[],
  adjustment: string,
  propsToIgnore?: string[]
): CustomRowData<T>[] {
  return data.map((item) => {
    return {
      ...item,
      data: forEachEntry(
        item.data,
        (value) => {
          const originalValue = parseFloat(value);
          if (isNaN(originalValue)) return value;
          const adjustmentValue = parseFloat(adjustment);
          const valueToAdd = originalValue * (adjustmentValue / 100);
          return (originalValue + valueToAdd).toFixed(2);
        },
        propsToIgnore
      ) as unknown as T,
    } as CustomRowData<T>;
  });
}

const getDollarAdjustment = (value: string, adjustment: string): number => {
  const unformattedValue = parseFloat(value.split(',').join(''));
  const unformattedAdjustment = parseFloat(adjustment.split(',').join(''));
  if (isNaN(unformattedValue) || isNaN(unformattedAdjustment)) return 0;
  return unformattedValue + unformattedAdjustment;
};

const getPercentAdjustment = (value: string, adjustment: string): number => {
  const adjustmentFloat = parseFloat(adjustment);
  const valueNumber = parseFloat(value);
  if (isNaN(adjustmentFloat) || isNaN(valueNumber)) return 0;
  const valueToAdd = valueNumber * (adjustmentFloat / 100);
  return valueNumber + valueToAdd;
};

export function applyDollarOrPercentAdjustmentFactor<T>(
  data: CustomRowData<T>[],
  adjustment: string,
  type: ValueModifier,
  propsToIgnore?: string[]
): CustomRowData<T>[] {
  return data.map((item) => {
    return {
      ...item,
      data: forEachEntry(
        item.data,
        (value) => {
          if (type === 'dollar') {
            return getDollarAdjustment(value, adjustment).toLocaleString(
              'en-US',
              { minimumFractionDigits: 2 }
            );
          } else {
            return getPercentAdjustment(value, adjustment).toFixed(2);
          }
        },
        propsToIgnore
      ) as unknown as T,
    } as CustomRowData<T>;
  });
}

export function applyFloatingHomeAdjustmentFactor<T>(
  data: CustomRowData<T>[],
  adjustment: string,
  propsToIgnore?: string[]
): CustomRowData<T>[] {
  return data.map((item) => {
    return {
      ...item,
      data: forEachEntry(
        item.data,
        (value) => {
          const originalValue = parseFloat(value.split(',').join(''));
          if (isNaN(originalValue)) return value;
          const adjustmentValue = parseFloat(adjustment);
          const adjustmentPercent = adjustmentValue / 100;
          const totalToAdd = originalValue * adjustmentPercent;
          return (originalValue + totalToAdd).toLocaleString('en-US', {
            minimumFractionDigits: 2,
          });
        },
        propsToIgnore
      ) as unknown as T,
    } as CustomRowData<T>;
  });
}

export function calculatePercentChangeFromPrevious<T>(
  prevValuesdata: CustomRowData<T>[],
  currentValuesdata: CustomRowData<T>[],
  propsToIgnore?: string[]
): CustomRowData<T>[] {
  return prevValuesdata.map((item, index) => {
    return {
      ...item,
      data: forEachEntry(
        item.data,
        (value, key) => {
          const currentValue = currentValuesdata.find(
            (d) => d.rowIndex === item.rowIndex
          )?.data as unknown as UnknownObject;
          if (!currentValue || !Object.keys(currentValue).length) return '0';
          const result = percentChangeCalculation(value, currentValue[key]);
          return result;
        },
        propsToIgnore
      ) as unknown as T,
    } as CustomRowData<T>;
  });
}

export function createNewCommercialRow<T>(
  data: CustomRowData<T>[],
  data2: CustomRowData<T>[],
  propsToIgnore?: string[]
): CustomRowData<T>[] {
  return data.map((item, index) => {
    return {
      ...item,
      data: forEachEntry(
        item.data,
        (_value, key) => {
          const data2Object = data2.find((d) => d.rowIndex === item.rowIndex)
            ?.data as unknown as UnknownObject;
          if (!data2Object || !Object.keys(data2Object).length) return '0';

          return data2Object[key] ? data2Object[key] : '0';
        },
        propsToIgnore
      ) as unknown as T,
    } as CustomRowData<T>;
  });
}

const getAdjustedValueByType = (value: string, adjustment: string, type: ValueModifier): number => {
  if(type === "percent") return getPercentAdjustment(value, adjustment);
  return getDollarAdjustment(value, adjustment);
}

export function getGrades(
  initialData: CustomRowData<IncomeRates>[],
  data: CustomRowData<IncomeRates>[],
  adjustmentFactor: string,
  type: ValueModifier = "percent"
): CustomRowData<AdjustmentFactorOverrides>[] {
  const incomeData: CustomRowData<AdjustmentFactorOverrides>[] = [];

  initialData.forEach((initial) => {
    const current = data.find(d => d.id === initial.id);
    if(!current) return;
    const grade1 = getAdjustedValueByType(initial.data.grade1 ?? '0', adjustmentFactor, type);
    const grade2 = getAdjustedValueByType(initial.data.grade2 ?? '0', adjustmentFactor, type);
    const grade3 = getAdjustedValueByType(initial.data.grade3 ?? '0', adjustmentFactor, type);
    const grade4 = getAdjustedValueByType(initial.data.grade4 ?? '0', adjustmentFactor, type);
    const grade5 = getAdjustedValueByType(initial.data.grade5 ?? '0', adjustmentFactor, type);
    const grade6 = getAdjustedValueByType(initial.data.grade6 ?? '0', adjustmentFactor, type);
    const grade7 = getAdjustedValueByType(initial.data.grade7 ?? '0', adjustmentFactor, type);

    const gradeOverride1 = parseFloat(current.data.grade1 ?? '0');
    const gradeOverride2 = parseFloat(current.data.grade2 ?? '0');
    const gradeOverride3 = parseFloat(current.data.grade3 ?? '0');
    const gradeOverride4 = parseFloat(current.data.grade4 ?? '0');
    const gradeOverride5 = parseFloat(current.data.grade5 ?? '0');
    const gradeOverride6 = parseFloat(current.data.grade6 ?? '0');
    const gradeOverride7 = parseFloat(current.data.grade7 ?? '0');

    incomeData.push({
      rowIndex: initial.rowIndex,
      id: initial.id,
      data: {
        grade1,
        grade2,
        grade3,
        grade4,
        grade5,
        grade6,
        grade7,
        gradeOverride1: grade1 === gradeOverride1 ? 0 : gradeOverride1,
        gradeOverride2: grade2 === gradeOverride2 ? 0 : gradeOverride2,
        gradeOverride3: grade3 === gradeOverride3 ? 0 : gradeOverride3,
        gradeOverride4: grade4 === gradeOverride4 ? 0 : gradeOverride4,
        gradeOverride5: grade5 === gradeOverride5 ? 0 : gradeOverride5,
        gradeOverride6: grade6 === gradeOverride6 ? 0 : gradeOverride6,
        gradeOverride7: grade7 === gradeOverride7 ? 0 : gradeOverride7,
      },
    });
  })

  return incomeData;
}
