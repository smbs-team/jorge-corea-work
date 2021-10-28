// Constants.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { RGBA_BLACK, RGBA_WHITE } from 'appConstants';

export const FONT_STYLE_NAME = {
  normal: 'normal',
  italic: 'italic',
};
export const FONT_STYLES = [FONT_STYLE_NAME.normal, FONT_STYLE_NAME.italic];

export const FONT_SIZES = new Array(Math.floor(30 / 2))
  .fill(0)
  .map((v, k) => (k + 1) * 2 + 'pt');

export const OPTION_COLORS_NAME = {
  outline: 'Outline',
  fill: 'Fill',
};

export const OPTION_COLORS = [
  OPTION_COLORS_NAME.fill,
  OPTION_COLORS_NAME.outline,
];

export const INITIAL_BUBBLE_COLOR = {
  outline: RGBA_BLACK,
  fill: RGBA_WHITE,
};
