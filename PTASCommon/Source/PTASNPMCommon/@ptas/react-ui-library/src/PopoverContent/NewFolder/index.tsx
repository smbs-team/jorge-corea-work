// NewFolder.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState } from "react";
import {
  withStyles,
  WithStyles,
  Box,
  Typography,
  createStyles,
  Theme
} from "@material-ui/core";
import clsx from "clsx";
import { useMount } from "react-use";
import CustomTextField from "./../../CustomTextField";
import { CustomButton } from "./../../CustomButton";
import DropDownTree, {
  DropDownTreeProps,
  DropdownTreeRow
} from "../../DropDownTree";

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

export interface NewFolderAcceptEvt {
  route: string;
  folderName: string;
  row?: DropdownTreeRow;
}

/**
 * Component props
 */
export interface NewFolderProps extends WithStyles<typeof styles> {
  title: string;
  okClick: (event: NewFolderAcceptEvt) => void;
  treeLabel?: string;
  treePlaceholder?: string;
  buttonText?: string;
  defaultName?: string;
  defaultRoute?: string;
  DropDownTreeProps: DropDownTreeProps;
}

/**
 * NewFolder
 *
 * @param props - Component props
 * @returns A JSX element
 */
function NewFolder(props: NewFolderProps): JSX.Element {
  const {
    classes,
    title,
    okClick,
    treeLabel,
    treePlaceholder,
    buttonText
  } = props;

  const [selectedRow, setSelectedRow] = useState<DropdownTreeRow>();
  const [name, setName] = useState<string>();
  const [route, setRoute] = useState<string>();

  useMount(() => {
    setName(props.defaultName);
    setRoute(props.defaultRoute);
  });

  const onAcceptButtonClick = (
    _e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    if (!name || !route) return;
    okClick({
      route:
        (route.startsWith("/") ? "" : "/") +
        route +
        (route.endsWith("/") ? "" : "/"),
      folderName: name,
      row: selectedRow
    });
  };

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
        <DropDownTree
          label={treeLabel ?? "Folder"}
          placeholder={treePlaceholder ?? "Select"}
          classes={{ container: classes.selectContainer }}
          onSelected={(val, route): void => {
            setSelectedRow(val);
            setRoute(route);
          }}
          defaultValue={props.defaultRoute}
          {...props.DropDownTreeProps}
        />
      </Box>
      <Box className={classes.footer}>
        <CustomButton
          onClick={onAcceptButtonClick}
          classes={{ root: classes.okButton }}
          disabled={!name || !route}
          fullyRounded
        >
          {buttonText ?? "Save"}
        </CustomButton>
      </Box>
    </div>
  );
}

export default withStyles(styles)(NewFolder);
