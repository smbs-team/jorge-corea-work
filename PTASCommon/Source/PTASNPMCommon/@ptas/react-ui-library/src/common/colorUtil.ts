// colorUtil.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { range } from "lodash";
import chroma from "chroma-js";
import _range from "lodash/range";

/**
 * Converts HSV color to RGB. Conversion formula
 * adapted from http://en.wikipedia.org/wiki/HSV_color_space.
 * Assumes h, s, and v are contained in the set [0, 1] and
 * returns r, g, and b in the set [0, 255].
 *
 * @param h - The hue, from 0 to 359
 * @param s - The saturation, from 0,1
 * @param v  - The value, from 0,1
 * @returns  An array of the RGB representation
 */
export function hsvToRgb(
  h: number,
  s: number,
  v: number
): [number, number, number] {
  let r, g, b;

  const i = Math.floor(h * 6);
  const f = h * 6 - i;
  const p = v * (1 - s);
  const q = v * (1 - f * s);
  const t = v * (1 - (1 - f) * s);

  switch (i % 6) {
    case 0:
      r = v;
      g = t;
      b = p;
      break;
    case 1:
      r = q;
      g = v;
      b = p;
      break;
    case 2:
      r = p;
      g = v;
      b = t;
      break;
    case 3:
      r = p;
      g = q;
      b = v;
      break;
    case 4:
      r = t;
      g = p;
      b = v;
      break;
    case 5:
      r = v;
      g = p;
      b = q;
      break;
    default:
      throw new Error("Invalid i value");
  }

  return [Math.floor(r * 255), Math.floor(g * 255), Math.floor(b * 255)];
}

/**
 * Given a number of items, generates a an array  of rgb colors based on hsv model.
 *
 * @param numberOfItems - The array size
 * @remarks See: https://stackoverflow.com/questions/17242144/javascript-convert-hsb-hsv-color-to-rgb-accurately
 * @returns An array of rgb numbers
 */
export const getRandColorsFromHSV = (numberOfItems = 15): string[] => {
  const retVal: string[] = [];

  //See: https://codebeautify.org/hsv-to-hex-converter#
  const generateDeltaValues = () => range(0, 20).map((_v, k) => k / 20);

  let deltaValues = generateDeltaValues();

  const getRandDeltaValue = (): number => {
    const randIndex = randBetween(0, deltaValues.length - 1);
    const val = deltaValues[randIndex];
    deltaValues.splice(randIndex, 1);
    if (!deltaValues.length) {
      deltaValues = generateDeltaValues();
    }
    return val;
  };

  const convertedColor = hsvToRgb(
    getRandDeltaValue(),
    Math.random(),
    Math.random()
  );

  const rgbColor = `rgb(${convertedColor.join()})`;
  retVal.push(rgbColor);
  let i = 0;

  while (retVal.length < numberOfItems) {
    i += 1;
    if (i > 100000) {
      break;
    }

    let hue = getRandDeltaValue();

    const rgbColor = `rgb(${hsvToRgb(
      hue,
      Math.random(),
      Math.random()
    ).join()})`.replace(/\s/, "");

    if (!retVal.includes(rgbColor)) {
      retVal.push(rgbColor);
    }
  }
  return retVal;
};

/**
 * Generates a random number between min and max value
 * @param min - Minimum value
 * @param max - Maximum value
 * @returns - A random number between the 2 numbers. The generated value could be the min or the max value too
 */
export const randBetween = (min: number, max: number): number =>
  Math.floor(Math.random() * (max - min + 1) + min);

/**
 * Generate sequencial or diverging color palette
 *
 * @param colors - Colors to use
 * @param colors2 - Second array of colors to use
 * @param numColors - Number of colors to generate
 * @param diverging - If true, generates a diverging palette, colors2 array should have colors
 * @remarks See: https://github.com/gka/palettes
 * @returns An array colors
 */
export function generatePalette(
  colors: string[],
  colors2: string[],
  numColors: number,
  diverging?: boolean
): string[] {
  const autoGradient = (color: string, numColors: number) => {
    const lab = chroma(color).lab();
    const lRange = 100 * (0.95 - 1 / numColors);
    const lStep = lRange / (numColors - 1);
    let lStart = (100 - lRange) * 0.5;
    const range = _range(lStart, lStart + numColors * lStep, lStep);
    let offset = 0;
    if (!diverging) {
      offset = 9999;
      for (let i = 0; i < numColors; i++) {
        let diff = lab[0] - range[i];
        if (Math.abs(diff) < Math.abs(offset)) {
          offset = diff;
        }
      }
    }
    return range.map((l) => chroma.lab(l + offset, lab[1], lab[2]));
  };

  const autoColors = (color: string, numColors: number, reverse = false) => {
    if (diverging) {
      const colors = autoGradient(color, 3).concat(chroma("#f5f5f5"));
      if (reverse) colors.reverse();
      return colors;
    } else {
      return autoGradient(color, numColors);
    }
  };

  let even = numColors % 2 === 0;
  let numColorsLeft = diverging
    ? Math.ceil(numColors / 2) + (even ? 1 : 0)
    : numColors;
  let numColorsRight = diverging
    ? Math.ceil(numColors / 2) + (even ? 1 : 0)
    : 0;
  let genColors =
    colors.length !== 1 ? colors : autoColors(colors[0], numColorsLeft);
  let genColors2 =
    colors2.length !== 1
      ? colors2
      : autoColors(colors2[0], numColorsRight, true);
  let stepsLeft = colors.length
    ? chroma
        .scale(
          true && colors.length > 1
            ? (chroma.bezier(genColors as string[]) as any)
            : genColors
        )
        .correctLightness(true)
        .colors(numColorsLeft)
    : [];
  let stepsRight =
    diverging && colors2.length
      ? chroma
          .scale(
            true && colors2.length > 1
              ? (chroma.bezier(genColors2 as string[]) as any)
              : genColors2
          )
          .correctLightness(true)
          .colors(numColorsRight)
      : [];

  let steps = (even && diverging
    ? stepsLeft.slice(0, stepsLeft.length - 1)
    : stepsLeft
  ).concat(stepsRight.slice(1));

  return steps;
}

export const convertHexToRGBA = (
  hexColors: string[],
  opacity: number
): string[] => {
  return hexColors.map((color) => {
    let hex = color.replace("#", "");

    if (hex.length === 3) {
      hex = `${hex[0]}${hex[0]}${hex[1]}${hex[1]}${hex[2]}${hex[2]}`;
    }

    const r = parseInt(hex.substring(0, 2), 16);
    const g = parseInt(hex.substring(2, 4), 16);
    const b = parseInt(hex.substring(4, 6), 16);

    return `rgba(${r},${g},${b},${opacity})`;
  });
};

export const addAlphaToRgb = (
  rgbColors: string[],
  opacity: number
): string[] => {
  return rgbColors.map((rgb) => {
    const color = rgb
      .substring(4, rgb.length - 1)
      .replace(/ /g, "")
      .split(",");
    return `rgba(${color.join()}, ${opacity})`;
  });
};
