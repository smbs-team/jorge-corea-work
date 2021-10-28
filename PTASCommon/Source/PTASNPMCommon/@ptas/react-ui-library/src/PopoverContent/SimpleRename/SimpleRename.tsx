// SimpleRename.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useCallback, useState, useEffect } from "react";
import {
  withStyles,
  WithStyles,
  Box,
  Typography,
  Theme,
  createStyles
} from "@material-ui/core";
import clsx from "clsx";
import CustomTextField from "../../CustomTextField";
import { CustomButton } from "../../CustomButton";

export interface RenameAcceptEvt {
  newName?: string;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles> {
  title: string;
  isConfirmDisabled?: boolean;
  okClick: (event: RenameAcceptEvt) => void;
  cancelClick?: () => void;
  textFieldOnChange?: (value: string) => void;
  buttonText?: string;
}
const styles = (theme: Theme) =>
  createStyles({
    title: {
      fontSize: theme.ptas.typography.bodyLarge.fontSize
    },
    header: {},
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
      top: theme.spacing(2),
      right: theme.spacing(2),
      cursor: "pointer"
    },
    body: {}
  });

/**
 * SimpleRename
 *
 * @param props - Component props
 * @returns A JSX element
 */
function SimpleRename(props: Props): JSX.Element {
  const {
    classes,
    title,
    okClick,
    textFieldOnChange,
    isConfirmDisabled,
    buttonText
  } = props;

  const [textFieldValue, setTextFieldValue] = useState<string>();

  const _textFieldOnChange = (val: string) => {
    setTextFieldValue(val);
  };

  const _onAcceptButtonClick = useCallback(
    (_e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
      okClick &&
        okClick({
          newName: textFieldValue
        });
    },
    [textFieldValue]
  );

  const _isConfirmDisabled =
    typeof isConfirmDisabled === "boolean"
      ? isConfirmDisabled
      : !textFieldValue;

  useEffect(() => {
    textFieldOnChange && textFieldOnChange(textFieldValue || "");
  }, [textFieldValue]);

  return (
    <div className={classes.container}>
      <Box className={classes.header}>
        <Typography variant='body1' className={classes.title}>
          {title}
        </Typography>
      </Box>
      <Box className={classes.body}>
        <CustomTextField
          onChange={(e) => _textFieldOnChange(e.target.value)}
          label='Name'
          classes={{
            root: clsx(classes.textField, classes.textFieldContainer)
          }}
        />
      </Box>
      <Box className={classes.footer}>
        <CustomButton
          onClick={_onAcceptButtonClick}
          classes={{ root: classes.okButton }}
          disabled={_isConfirmDisabled}
          fullyRounded
        >
          {buttonText ?? "Save"}
        </CustomButton>
      </Box>
    </div>
  );
}

export default withStyles(styles)(SimpleRename);
