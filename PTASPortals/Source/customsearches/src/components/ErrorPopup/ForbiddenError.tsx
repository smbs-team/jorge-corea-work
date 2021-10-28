/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, Modal } from '@material-ui/core';
import React from 'react';

const useStyles = makeStyles(theme => ({
  paper: {
    position: 'absolute',
    width: 400,
    backgroundColor: theme.palette.background.paper,
    border: '2px solid #000',
    boxShadow: theme.shadows[5],
    padding: theme.spacing(2, 4, 3),
  },
}));

function getModalStyle(): {} {
  return {
    top: `35%`,
    left: `35%`,
    // transform: `translate(+50%, +50%)`,
  };
}

interface ForbiddelProps {
  message: string;
  handleClose: () => void;
  openModal: boolean;
}

export const ForbiddenError = (props: ForbiddelProps): JSX.Element => {
  const classes = useStyles();
  const [modalStyle] = React.useState(getModalStyle);

  const handleOpen = (): void => {
    props.handleClose();
  };

  const body = (
    <div style={modalStyle} className={classes.paper}>
      <h2>Missing security requirements.</h2>
      <p>{props.message}</p>
      <p>
        Please contact your system administrator on Dynamics CE so they can
        assign the required permissions to your account
      </p>
      <button type="button" onClick={handleOpen}>
        Ok
      </button>
    </div>
  );

  return (
    <Modal open={props.openModal} onClose={props.handleClose}>
      {body}
    </Modal>
  );
};
