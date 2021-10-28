// NewCategory.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useCallback, useEffect, useState } from "react";
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

export interface NewCategoryAcceptEvt {
  categoryName?: string;
}

/**
 * Component props
 */
export interface NewCategoryProps extends WithStyles<typeof styles> {
  title: string;
  okClick: (event: NewCategoryAcceptEvt) => void;
  buttonText?: string;
  defaultName?: string;
}

/**
 * NewCategory
 *
 * @param props - Component props
 * @returns A JSX element
 */
function NewCategory(props: NewCategoryProps): JSX.Element {
  const { classes, title, okClick, buttonText } = props;
  const [name, setName] = useState<string>();

  const _onAcceptButtonClick = useCallback(
    (_e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
      okClick &&
        okClick({
          categoryName: name
        });
    },
    [name]
  );

  useEffect(() => {
    setName(props.defaultName);
  }, []);

  return (
    <div className={classes.container}>
      <Box className={classes.header}>
        <Typography variant='body1' className={classes.title}>
          {title}
        </Typography>
      </Box>
      <Box className={classes.body}>
        <CustomTextField
          onChange={(e) => setName(e.target.value)}
          label='Name'
          classes={{
            root: clsx(classes.textField, classes.textFieldContainer)
          }}
          defaultValue={props.defaultName}
        />
      </Box>
      <Box className={classes.footer}>
        <CustomButton
          onClick={_onAcceptButtonClick}
          classes={{ root: classes.okButton }}
          disabled={!name}
          fullyRounded
        >
          {buttonText ?? "Save"}
        </CustomButton>
      </Box>
    </div>
  );
}

export default withStyles(styles)(NewCategory);
