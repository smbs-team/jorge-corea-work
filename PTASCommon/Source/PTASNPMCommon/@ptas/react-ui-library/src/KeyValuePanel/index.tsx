// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from "react";
import { createStyles, Theme, WithStyles, withStyles } from "@material-ui/core";
import OverflowToolTip from "../OverflowToolTip";

interface Props extends WithStyles<typeof styles> {
  data: KeyValueRow[];
}

export interface KeyValueRow {
  key: React.ReactNode;
  value: React.ReactNode;
}

const styles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      width: "fit-content",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.body.fontSize
    },
    key: {
      textAlign: "right",
      paddingRight: theme.spacing(0.625)
    },
    value: {
      textAlign: "left",
      paddingLeft: theme.spacing(0.625),
      fontWeight: "bold"
    }
  });

function KeyValuePanel(props: PropsWithChildren<Props>): JSX.Element {
  const { data, classes } = props;

  return (
    <table className={classes.root}>
      <tbody>
        {data.map(({ key, value }, index) => (
          <tr key={index}>
            <td className={classes.key}>{key}</td>
            <td>
              <OverflowToolTip classes={{ text: classes.value }}>
                {value}
              </OverflowToolTip>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}

export default withStyles(styles)(KeyValuePanel);
