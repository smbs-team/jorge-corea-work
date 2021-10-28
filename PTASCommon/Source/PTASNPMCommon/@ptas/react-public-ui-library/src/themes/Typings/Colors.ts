interface Theme {
  black: string;
  white: string;
  gray: string;
  grayDark: string;
  grayMedium: string;
  grayLight: string;
  grayLightest: string;
  accentDark: string;
  accent: string;
  accentBright: string;
  accentLight: string;
}

interface Utility {
  success: string;
  sucessUltra: string;
  danger: string;
  dangerInverse: string;
  dangerInverseLight: string;
  warning: string;
  changed: string;
  selection: string;
  selectionInverse: string;
  selectionLight: string;
  link: string;
  disabled: string;
}

export interface Colors {
  theme: Theme;
  utility: Utility;
}
