// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from "react";
import { createStyles, Theme, withStyles, WithStyles } from "@material-ui/core";
import OfficeFilePreview from "../OfficeFilePreview";
import { CustomButton } from "../CustomButton";
import KeyValuePanel from "../KeyValuePanel";
import { KeyValueRow } from "../KeyValuePanel";

interface Props extends WithStyles<typeof useStyles> {
  fileUrl: string;
  details: KeyValueRow[];
  title: string;
  onCancel?: () => void;
  onSave?: () => void;
  cancelButtonText?: string;
  confirmButtonText?: string;
}

const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      height: "100%",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      display: "flex",
      flexDirection: "column"
    },
    detailsContainer: {
      display: "flex",
      marginBottom: theme.spacing(1)
    },
    details: {
      width: "50%"
    },
    title: {
      width: "50%",
      fontSize: theme.ptas.typography.h6.fontSize,
      display: "flex",
      alignSelf: "center"
    },
    filePreview: {
      flex: "1 1 auto"
    },
    buttonContainer: {
      display: "flex",
      justifyContent: "flex-end",
      marginTop: 24
    }
  });

function ExcelViewer(props: Props): JSX.Element {
  const { fileUrl, classes, details, title } = props;

  return (
    <div className={classes.root}>
      <div className={classes.detailsContainer}>
        <KeyValuePanel data={details} classes={{ root: classes.details }} />
        <div className={classes.title}>{title}</div>
      </div>
      <div className={classes.filePreview}>
        <OfficeFilePreview fileUrl={fileUrl} />
      </div>
      <div className={classes.buttonContainer}>
        <CustomButton
          ptasVariant='commercial'
          style={{ fontWeight: "normal", marginRight: 34 }}
          onClick={props.onCancel}
        >
          {props.cancelButtonText ?? "Cancel"}
        </CustomButton>
        <CustomButton
          ptasVariant='commercial'
          style={{ fontWeight: "normal" }}
          onClick={props.onSave}
        >
          {props.confirmButtonText ?? "Save"}
        </CustomButton>
      </div>
    </div>
  );
}

export default withStyles(useStyles)(ExcelViewer);
