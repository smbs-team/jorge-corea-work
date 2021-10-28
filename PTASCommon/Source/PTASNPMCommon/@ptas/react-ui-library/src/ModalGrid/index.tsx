// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useState } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  TextField,
  Theme,
  TextFieldProps
} from "@material-ui/core";
import CustomAgGrid from "../CustomAgGrid";
import SearchIcon from "@material-ui/icons/Search";
import { CustomButton } from "../CustomButton";
import CustomIconButton from "../CustomIconButton";
import Modal from "@material-ui/core/Modal";
import CloseIcon from "@material-ui/icons/Close";
import Backdrop from "@material-ui/core/Backdrop";
import Fade from "@material-ui/core/Fade";
import { GridOptions } from "ag-grid-community";
import { GenericWithStyles } from "../common";

/**
 * Component props
 */
interface Props<T> extends WithStyles<typeof useStyles> {
  rows: T[];
  buttonText?: string;
  isOpen: boolean;
  gridOptions?: GridOptions;
  onClose: () => void;
  onButtonClick?: (selectedRows: T[]) => void;
  TextFieldProps?: TextFieldProps;
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      width: "90%",
      height: 415,
      backgroundColor: "rgba(255,255,255, 0.9)",
      padding: theme.spacing(1, 3.75, 1, 3.75),
      borderRadius: 9,
      boxShadow: theme.shadows[5],
      position: "relative",
      outline: 0
    },
    button: {
      display: "flex",
      marginLeft: "auto",
      marginTop: 34
    },
    textField: {
      marginBottom: 16
    },
    iconButton: {
      position: "absolute",
      top: 12,
      right: 30,
      color: "black"
    },
    closeIcon: {
      fontSize: 32
    },
    backdrop: {
      backgroundColor: "unset"
    }
  });

/**
 * ModalGrid
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ModalGrid<T>(props: Props<T>): JSX.Element {
  const { classes } = props;

  const [filter, setFilterBy] = useState<string>();
  const [selectedRows, setSelectedRows] = useState<T[]>([]);
  const [open, setOpen] = React.useState(false);

  useEffect(() => {
    if (props.isOpen === undefined) return;
    setOpen(props.isOpen);
  }, [props.isOpen]);

  const handleClose = () => {
    setOpen(false);
    props.onClose();
  };

  useEffect(() => {
    if (!open) {
      setFilterBy(undefined);
    }
  }, [open]);

  return (
    <Modal
      open={open}
      onClose={handleClose}
      closeAfterTransition
      BackdropComponent={Backdrop}
      BackdropProps={{
        timeout: 500,
        classes: { root: classes.backdrop }
      }}
      style={{
        display: "flex",
        alignItems: "center",
        justifyContent: "center"
      }}
    >
      <Fade in={open}>
        <div className={classes.root}>
          <TextField
            onChange={(e) => setFilterBy(e.target.value)}
            placeholder='Filter...'
            className={classes.textField}
            InputProps={{
              endAdornment: (
                <i>
                  <SearchIcon />
                </i>
              )
            }}
            {...props.TextFieldProps}
          />
          <CustomAgGrid
            rows={props.rows}
            filterBy={filter}
            gridOptions={props.gridOptions}
            onRowSelected={(rows) => setSelectedRows(rows)}
          />
          <CustomButton
            classes={{ root: classes.button }}
            onClick={() =>
              props.onButtonClick && props.onButtonClick(selectedRows)
            }
            disabled={selectedRows.length < 1 ? true : false}
            fullyRounded
          >
            {props.buttonText ?? "Add selected layers"}
          </CustomButton>
          <CustomIconButton
            icon={<CloseIcon className={classes.closeIcon} />}
            className={classes.iconButton}
            onClick={handleClose}
          />
        </div>
      </Fade>
    </Modal>
  );
}

export default withStyles(useStyles)(ModalGrid) as <T>(
  props: GenericWithStyles<Props<T>>
) => React.ReactElement;
