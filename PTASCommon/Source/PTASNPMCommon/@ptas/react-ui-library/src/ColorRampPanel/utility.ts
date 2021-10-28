import { getRandColorsFromHSV } from "../common/colorUtil";
import { ColorConfiguration } from "./types";
import colors from "./defaultColors.json";

export const getDefaultRamp = (
  numColorsRequired?: number,
  id?: string
): ColorConfiguration | undefined => {
  if (
    id &&
    id !== "defaultRamp1" &&
    id !== "defaultRamp2" &&
    id !== "defaultRamp3"
  ) {
    return undefined;
  }
  return {
    id: id ?? "defaultRamp1",
    colors: getRandomColors(colors[id ?? "defaultRamp1"], numColorsRequired),
    type: "random"
  };
};

const getRandomColors = (
  colors: string[],
  numColorsRequired?: number
): string[] => {
  const clonedColors = [...colors];
  const totalColors = clonedColors.length;
  if (totalColors === (numColorsRequired ?? 23)) return clonedColors;
  if (totalColors < (numColorsRequired ?? 23)) {
    return [
      ...clonedColors,
      ...getRandColorsFromHSV((numColorsRequired ?? 23) - totalColors)
    ];
  }
  return clonedColors.splice(0, numColorsRequired ?? 23);
};
