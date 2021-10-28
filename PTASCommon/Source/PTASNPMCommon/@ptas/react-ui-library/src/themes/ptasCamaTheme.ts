// PtasCamaTheme.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { createTheme, ThemeOptions } from "@material-ui/core/styles";
import { Opacity } from "./Typings/Opacity";
import { Typography } from "./Typings/Typography";
import { Spacing } from "./Typings/Spacing";
import { Colors } from "./Typings/Colors";

declare module "@material-ui/core" {
  interface Theme {
    ptas: {
      typography: Typography;
      spacing: Spacing;
      opacity: Opacity;
      colors: Colors;
    };
  }
}

const SPACING = 8;

const FONT_WEIGHT_1 = 400;
const FONT_WEIGHT_2 = 300;
const FONT_WEIGHT_3 = 700;
const FONT_WEIGHT_4 = 600;

export const ptasCamaTheme = createTheme({
  palette: {
    primary: {
      main: "#7EA000"
    }
  },
  spacing: SPACING,
  overrides: {
    MuiButton: {
      root: {
        textTransform: "unset"
      }
    },
    MuiIcon: {
      root: {
        fontSize: "1.2rem"
      }
    },
    MuiRadio: {
      root: {
        padding: SPACING
      }
    }
  },
  ptas: {
    typography: {
      lineHeight: 1.5,
      bodyFontFamily: ["Cabin", "helvetica", "sans-serif"].join(","),
      titleFontFamily: ["Roboto Condensed", "helvetica", "sans-serif"].join(
        ","
      ),
      h1: {
        fontSize: "3rem",
        fontWeight: FONT_WEIGHT_1
      },
      h2: {
        fontSize: "2.625rem",
        fontWeight: FONT_WEIGHT_1
      },
      h3: {
        fontSize: "2.5rem",
        fontWeight: FONT_WEIGHT_1
      },
      h4: {
        fontSize: "2.25rem",
        fontWeight: FONT_WEIGHT_1
      },
      h5: {
        fontSize: "1.875rem",
        fontWeight: FONT_WEIGHT_4
      },
      h6: {
        fontSize: "1.625rem",
        fontWeight: FONT_WEIGHT_1
      },
      buttonLarge: {
        fontSize: 16,
        fontWeight: FONT_WEIGHT_3
      },
      buttonSmall: {
        fontSize: "1rem",
        fontWeight: FONT_WEIGHT_1
      },
      formLabel: {
        fontSize: "1rem",
        fontWeight: FONT_WEIGHT_2
      },
      formField: {
        fontSize: "1.125rem",
        fontWeight: FONT_WEIGHT_2
      },
      body: {
        fontSize: "1.125rem",
        fontWeight: FONT_WEIGHT_2
      },
      bodyLarge: {
        fontSize: "1.25rem",
        fontWeight: FONT_WEIGHT_1
      },
      bodySmall: {
        fontSize: "1rem",
        fontWeight: FONT_WEIGHT_2
      },
      small: {
        fontSize: "0.875rem",
        fontWeight: "unset"
      },
      finePrint: {
        fontSize: "0.75rem",
        fontWeight: FONT_WEIGHT_2
      },
      bodyExtraBold: {
        fontSize: "1.125rem",
        fontWeight: FONT_WEIGHT_3
      }
    },
    spacing: {
      widths: {
        content: {
          minimum: 320,
          maximum: 920
        },
        form: {
          minimum: 230,
          maximum: 455
        },
        window: {
          maximum: 1078
        }
      }
    },
    opacity: {
      disabled: "30%",
      subdued: "50%",
      overlay1: "90%",
      overlay2: "70%"
    },
    colors: {
      theme: {
        black: "#000000",
        white: "#ffffff",
        gray: "#666666",
        grayDark: "#333333",
        grayMedium: "#7a7a7a",
        grayLight: "#ececec",
        grayLightest: "#f4f4f4",
        accentDark: "#0e4b5c",
        accent: "#187089",
        accentBright: "#1fabb3",
        accentLight: "#cdf4ff",
        redDark: "#840705"
      },
      utility: {
        success: "#43a047",
        sucessUltra: "#00ff22",
        danger: "#b70f0a",
        dangerInverse: "#f44336",
        dangerInverseLight: "#ff7167",
        error: "#d20000",
        warning: "#e3951f",
        changed: "#f4eb49",
        selection: "#7EA000",
        selectionInverse: "#a5c727",
        selectionLight: "#d4e693",
        link: "#1a6cff",
        disabled: "#aaaaaa"
      }
    }
  }
} as ThemeOptions);
