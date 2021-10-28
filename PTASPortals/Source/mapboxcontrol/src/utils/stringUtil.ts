/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/**
 * Remove all white spaces
 *
 * @param str - The string parameter to replace white spaces
 * @returns A string with no white spaces
 */
export const removeAllWhiteSpaces = (str: string): string =>
  str.replace(/\s+/g, '');

export const capitalizeFirstLetter = (str: string): string =>
  str[0].toUpperCase() + str.slice(1);
