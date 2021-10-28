// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren, useEffect } from "react";
import { withStyles, WithStyles, createStyles, Theme } from "@material-ui/core";
import Modal from "@material-ui/core/Modal";
import Backdrop from "@material-ui/core/Backdrop";
import Fade from "@material-ui/core/Fade";
import CustomIconButton from "../CustomIconButton";
import CloseIcon from "@material-ui/icons/Close";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  isOpen: boolean;
  onClose: () => void;
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
 * CustomModal
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomModal(props: PropsWithChildren<Props>): JSX.Element {
  const { classes } = props;

  const [open, setOpen] = React.useState(false);

  useEffect(() => {
    if (props.isOpen === undefined) return;
    setOpen(props.isOpen);
  }, [props.isOpen]);

  const handleClose = () => {
    setOpen(false);
    props.onClose();
  };

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
          {props.children}
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

export default withStyles(useStyles)(CustomModal);
