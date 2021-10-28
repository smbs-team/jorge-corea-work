/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { isEmpty, isEqual, xorWith } from 'lodash';

/**
 * Gets the folder full path
 */
export const getFolderPath = <
  T extends {
    id: React.ReactText;
    title: string;
    parent: string | number | null;
  }
>(
  row: T,
  rows: T[],
  isFolder?: boolean
): string => {
  let currentRow: T | undefined = row;
  const route: string[] = [];
  while (currentRow !== undefined) {
    route.push(currentRow.title);
    // eslint-disable-next-line no-loop-func
    currentRow = rows.find((r) => r.id === currentRow?.parent);
  }
  if (isFolder && route.length > 1) route.shift();
  return '/' + route.reverse().join('/');
};

/**
 * Gets all parent indexes
 */
export const getAllIndexes = <
  T extends {
    id: React.ReactText;
    title: string;
    parent: string | number | null;
  }
>(
  row: T,
  rows: T[]
): number[] => {
  let currentRow: T | undefined = row;
  const indexes: number[] = [];
  while (currentRow !== undefined) {
    // eslint-disable-next-line no-loop-func
    indexes.push(rows.findIndex((r) => r.id === currentRow?.id));
    // eslint-disable-next-line no-loop-func
    currentRow = rows.find((r) => r.id === currentRow?.parent);
  }
  return indexes;
};

/**
 * Gets all child rows from a given row
 */
export const getChildRows = <
  T extends { id: React.ReactText; parent: React.ReactText | null }
>(
  row: T,
  rows: T[]
): T[] => {
  return rows.filter((r) => r.parent === (row ? row.id : null));
};

export const isArrayEqual = (x: unknown[], y: unknown[]): boolean =>
  isEmpty(xorWith(x, y, isEqual));
