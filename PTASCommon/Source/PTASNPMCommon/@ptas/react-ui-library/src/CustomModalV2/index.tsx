// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren } from "react";
import { createStyles, withStyles, WithStyles } from "@material-ui/core";
import Modal from "@material-ui/core/Modal";
import Backdrop from "@material-ui/core/Backdrop";
import Fade, { FadeProps } from "@material-ui/core/Fade";

interface Props extends WithStyles<typeof useStyles> {
  open: boolean;
  onClose?: ((event: {}, reason: 'backdropClick' | 'escapeKeyDown') => void);
  FadeProps?: FadeProps;
}

const useStyles = () =>
  createStyles({
    root: {
      backgroundColor: "white",
      padding: 18,
      overflow: "auto"
    },
  });

function CustomModalV2(props: PropsWithChildren<Props>): JSX.Element {
  const { open, classes, FadeProps, children } = props;

  return (
    <Modal
      closeAfterTransition
      BackdropComponent={Backdrop}
      BackdropProps={{
        timeout: 500,
      }}
      style={{
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
      }}
      {...props}
    >
      <Fade in={open} {...FadeProps}>
        <div className={classes.root}>{children}</div>
      </Fade>
    </Modal>
  );
}

export default withStyles(useStyles)(CustomModalV2);
