// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from "react";
import { createStyles, WithStyles, withStyles } from "@material-ui/core";
import IframeResizer from "iframe-resizer-react";

interface Props extends WithStyles<typeof styles> {
  fileUrl: string;
}

const styles = () =>
  createStyles({
    root: {
      width: "100%",
      height: "100%"
    },
  });

function OfficeFilePreview(props: Props): JSX.Element {
  const { fileUrl, classes } = props;

  return (
      <IframeResizer
        title="Document viewer"
        frameBorder={0}
        className={classes.root}
        src={"https://view.officeapps.live.com/op/view.aspx?src=" + fileUrl}
      />
  );
}

export default withStyles(styles)(OfficeFilePreview);
