// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC } from "react";

import { styles } from "./styles";
import { Box, WithStyles, withStyles } from "@material-ui/core";
import { GenericWithStyles } from "@ptas/react-ui-library";

interface Props extends WithStyles<typeof styles> {
  file?: File;
  fileUrl: string;
}

function OfficeFilePreview(props: Props): JSX.Element {
  const { fileUrl, classes /*, file*/ } = props;

  return (
    <Box className={classes.root}>
      <iframe
        height={"100%"}
        width={"100%"}
        // src='https://view.officeapps.live.com/op/view.aspx?src=https://www.amity.edu/aiit/icrito2016/ieee-format.doc'
        src={"https://view.officeapps.live.com/op/view.aspx?src=" + fileUrl}
      />
    </Box>
  );
}

export default withStyles(styles)(OfficeFilePreview) as FC<
  GenericWithStyles<Props>
>;
