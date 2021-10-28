//styles.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { createStyles, Theme } from "@material-ui/core/styles";

/**
 * Component styles
 */
export const useStyles = (theme: Theme) =>
  createStyles({
    table: {
      width: "100%",
      marginBottom: 10,
      borderCollapse: "collapse"
    },
    th: {
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      backgroundColor: "#ddd",
      padding: 10,
      border: "1px solid #ccc",
      fontFamily: theme.ptas.typography.titleFontFamily,
      position: "relative",
      color: theme.ptas.colors.theme.gray
    },
    td: {
      padding: 10,
      border: "1px solid #ccc",
      fontFamily: theme.ptas.typography.bodyFontFamily
    },
    wrapTable: {
      overflow: "auto",
      maxHeight: "94%"
    },
    arrowIcon: {
      transform: "rotate(47deg)",
      position: "absolute",
      right: -12,
      bottom: -12,
      fontSize: 40
    },
    sheetButton: {
      height: 26,
      cursor: "pointer",
      border: `1px solid ${theme.ptas.colors.theme.black}`,
      borderBottom: "none",
      marginRight: 4,
      "&.selected": {
        background: "#ddd"
      }
    }
  });
