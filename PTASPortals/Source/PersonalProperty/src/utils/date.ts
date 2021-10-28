// date.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { format } from 'date-fns';

const removeTimeZone = (dt: Date): Date => {
  return new Date(dt.valueOf() + dt.getTimezoneOffset() * 60 * 1000);
};

export const formatDate = (
  date: Date | string | number | undefined
): string => {
  if (!date) return '';
  try {
    const dt = date instanceof Date ? date : new Date(date);
    const dateToFormat = removeTimeZone(dt);
    return format(dateToFormat, 'MM/dd/yyyy');
  } catch (e) {
    return '';
  }
};
