// ColorLegend.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { forwardRef } from "react";
import {
  withStyles,
  WithStyles,
  Box,
  Popper,
  Theme,
  createStyles
} from "@material-ui/core";
import { Close } from "@material-ui/icons";

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles> {
  title?: string;
  onClose?: () => void;
  legends?: object[];
  anchorEl: ReferenceObject | (() => ReferenceObject) | null | undefined;
  open: boolean;
  options?: object;
}

interface Legends {
  fillColor: string;
  outlineColor: string;
  legendText: string;
}

interface ReferenceObject {
  clientHeight: number;
  clientWidth: number;
  getBoundingClientRect(): ClientRect;
}

const styles = (theme: Theme) =>
  createStyles({
    root: {
      backgroundColor: theme.ptas.colors.theme.black + "B3",
      color: theme.ptas.colors.theme.white,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      padding: theme.spacing(2, 1),
      bottom: 185,
      left: 0,
      height: "max-content",
      width: "max-content",
      minWidth: "170px",
      overflow: "auto",
      zIndex: 1300
    },
    header: {
      display: "flex"
    },
    title: {
      fontWeight: 700,
      fontFamily: theme.ptas.typography.titleFontFamily,
      marginBottom: theme.spacing(2),
      marginRight: theme.spacing(3)
    },
    legend: {
      display: "flex"
    },
    legendColor: {
      width: 60,
      height: 25,
      boxSizing: "border-box",
      borderRadius: 12,
      borderWidth: 6,
      borderStyle: "solid",
      margin: theme.spacing(0, 1, 1, 0)
    },
    legendColor2: {
      width: "100%",
      height: "100%",
      boxSizing: "border-box",
      borderRadius: 12
    },
    borderTransparent: {
      borderRadius: "12px",
      overflow: "hidden",
      width: "60px",
      height: "25px",
      padding: "5px",
      margin: theme.spacing(0, 1, 1, 0)
    },
    container: {
      margin: theme.spacing(2, 4)
    },
    textField: {},
    textFieldContainer: {
      margin: theme.spacing(4, 0)
    },
    selectContainer: {},
    footer: {},
    okButton: {
      margin: theme.spacing(4, 0),
      float: "right"
    },
    closeButton: {
      position: "absolute",
      top: theme.spacing(12 / 8),
      right: theme.spacing(3 / 4),
      cursor: "pointer"
    },
    body: {}
  });

const transparentBgBase64 =
  "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABkBAMAAACCzIhnAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAALVBMVEXKyszKyszQ0NL39/f9/f39/f3JycvQ0NL39/f+/v7Q0NLV1dby8vP39/f////EQ7taAAAAAWJLR0QOb70wTwAAAAlwSFlzAAALEwAACxMBAJqcGAAAAAd0SU1FB+QKHRA3ACXjZ5MAAAB/SURBVFjDY2BgVHZ1ViSBCGWgjxZBsYopM8pIIFwYlLT2XL6ziwTCmMHY9vSi1WdJIJQYXDzbE9M7SSAEGUJDjASETEggGMixhQy/kBFiZMTLaOyPxv5o7I/G/mjsj8b+aOyPxv5o7I/G/mjsj8b+aOyPxv5o7I/G/mjs00ULAHzvfQVn21lMAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDIwLTEwLTI5VDE2OjU1OjAwLTA0OjAwkxPuSgAAACV0RVh0ZGF0ZTptb2RpZnkAMjAyMC0xMC0yOVQxNjo1NTowMC0wNDowMOJOVvYAAAAASUVORK5CYII=";

/**
 * ColorLegend
 *
 * @param props - Component props
 * @returns A JSX element
 */
const ColorLegend = forwardRef(function (props: Props, ref): JSX.Element {
  const { classes, title, onClose, legends, anchorEl, open, options } = props;

  return (
    <Popper
      anchorEl={anchorEl}
      popperOptions={options}
      className={classes.root}
      open={open}
    >
      <Box className={classes.header}>
        <Box className={classes.title}>{title}</Box>
        <Box
          onClick={onClose}
          className={classes.closeButton}
          title={ref ? "" : undefined}
        >
          <Close />
        </Box>
      </Box>
      {legends &&
        (legends as Legends[]).map((legend, index) => {
          let bgTransparent = false;
          let borderTransparent = false;
          const bgColorValues = getColorValues(legend.fillColor);
          if (bgColorValues.alpha === 0) {
            bgTransparent = true;
          }
          const borderColorValues = getColorValues(legend.outlineColor);
          if (borderColorValues.alpha === 0) {
            borderTransparent = true;
          }

          let style = {
            backgroundColor: legend.fillColor,
            borderColor: legend.outlineColor,
            backgroundImage: ""
          };
          if (bgTransparent) {
            style.backgroundImage = `url(${transparentBgBase64})`;
          }

          return (
            <Box key={index} className={classes.legend}>
              {!borderTransparent && (
                <Box className={classes.legendColor} style={style}></Box>
              )}
              {borderTransparent && (
                <Box
                  className={classes.borderTransparent}
                  style={{ backgroundImage: `url(${transparentBgBase64})` }}
                >
                  <Box
                    className={classes.legendColor2}
                    style={
                      bgTransparent
                        ? { backgroundImage: style.backgroundImage }
                        : { backgroundColor: style.backgroundColor }
                    }
                  ></Box>
                </Box>
              )}
              {legend.legendText}
            </Box>
          );
        })}
    </Popper>
  );
});

const getColorValues = (
  color: string
): { red: number; green: number; blue: number; alpha: number } => {
  let values = { red: 0, green: 0, blue: 0, alpha: 0 };
  if (typeof color == "string") {
    /* hex */
    if (color.indexOf("#") === 0) {
      color = color.substr(1);
      if (color.length === 3)
        values = {
          red: parseInt(color[0] + color[0], 16),
          green: parseInt(color[1] + color[1], 16),
          blue: parseInt(color[2] + color[2], 16),
          alpha: 1
        };
      else
        values = {
          red: parseInt(color.substr(0, 2), 16),
          green: parseInt(color.substr(2, 2), 16),
          blue: parseInt(color.substr(4, 2), 16),
          alpha: 1
        };
      /* rgb */
    } else if (color.indexOf("rgb(") === 0) {
      const pars = color.indexOf(",");
      values = {
        red: parseInt(color.substr(4, pars)),
        green: parseInt(color.substr(pars + 1, color.indexOf(",", pars))),
        blue: parseInt(
          color.substr(color.indexOf(",", pars + 1) + 1, color.indexOf(")"))
        ),
        alpha: 1
      };
      /* rgba */
    } else if (color.indexOf("rgba(") === 0) {
      const pars = color.indexOf(","),
        repars = color.indexOf(",", pars + 1);
      values = {
        red: parseInt(color.substr(5, pars)),
        green: parseInt(color.substr(pars + 1, repars)),
        blue: parseInt(
          color.substr(
            color.indexOf(",", pars + 1) + 1,
            color.indexOf(",", repars)
          )
        ),
        alpha: parseFloat(
          color.substr(color.indexOf(",", repars + 1) + 1, color.indexOf(")"))
        )
      };
      /* verbous */
    } else {
      const stdCol: { [key: string]: string } = {
        acqua: "#0ff",
        teal: "#008080",
        blue: "#00f",
        navy: "#000080",
        yellow: "#ff0",
        olive: "#808000",
        lime: "#0f0",
        green: "#008000",
        fuchsia: "#f0f",
        purple: "#800080",
        red: "#f00",
        maroon: "#800000",
        white: "#fff",
        gray: "#808080",
        silver: "#c0c0c0",
        black: "#000"
      };
      if (stdCol[color] !== undefined) {
        values = getColorValues(stdCol[color]);
      }
    }
  }
  return values;
};

export default withStyles(styles)(ColorLegend);
