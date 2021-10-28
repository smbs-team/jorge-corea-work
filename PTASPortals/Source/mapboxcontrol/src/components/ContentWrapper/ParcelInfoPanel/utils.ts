// utils.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export const parseDate = (date: string): string => {
  try {
    return new Date(date).toLocaleDateString('en-US');
  } catch (error) {
    return '';
  }
};
