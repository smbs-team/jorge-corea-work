// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, useEffect } from "react";

import { styles } from "./styles";
import { Box, WithStyles, withStyles } from "@material-ui/core";
import { GenericWithStyles } from "@ptas/react-ui-library";
import PDFObject from "pdfobject";

interface Props extends WithStyles<typeof styles> {
  elementId: string;
  file: File | string;
}

function PdfFilePreview(props: Props): JSX.Element {
  const { elementId, file, classes } = props;

  useEffect(() => {
    if (file && typeof file === "string") {
      PDFObject.embed(file, "#" + elementId, {
        pdfOpenParams: { suppressConsole: true }
      });
    } else if (file && typeof file === "object") {
      const fileUrl = URL.createObjectURL(file);
      PDFObject.embed(fileUrl, "#" + elementId, {
        pdfOpenParams: { suppressConsole: true }
      });
    }
  }, [file]);

  return <Box id={elementId} className={classes.root}></Box>;
}

export default withStyles(styles)(PdfFilePreview) as FC<
  GenericWithStyles<Props>
>;
