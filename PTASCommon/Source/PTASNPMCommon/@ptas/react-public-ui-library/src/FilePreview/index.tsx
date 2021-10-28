// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC } from "react";

import { styles } from "./styles";
import { Box, withStyles, WithStyles } from "@material-ui/core";
import { GenericWithStyles } from "@ptas/react-ui-library";
import DocViewer from "react-doc-viewer";

interface Props extends WithStyles<typeof styles> {
  file?: File;
}

const docs = [
  {
    // uri:
    //   "http://www.dhs.state.il.us/OneNetLibrary/27897/documents/Initiatives/IITAA/Sample-Document.docx"
    uri: "http://www.iiswc.org/iiswc2012/sample.doc"
  }
];

function FilePreview(): JSX.Element {
  return (
    <Box>
      <DocViewer documents={docs} />
    </Box>
  );
}

export default withStyles(styles)(FilePreview) as FC<GenericWithStyles<Props>>;
