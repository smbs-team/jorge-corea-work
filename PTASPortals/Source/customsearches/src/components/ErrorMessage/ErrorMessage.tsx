/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core';
import InfoIcon from '@material-ui/icons/Info';
import ErrorModal from 'routes/models/View/Projects/Land/ErrorModal';

const useStyles = makeStyles({
  errorMessage: {
    display: 'inline-block',
    color: 'red',
    marginTop: '5px',
    fontWeight: 500,
  },
  wrapper: {
    display: 'flex',
    flexDirection: 'column',
    marginLeft: '15px',
  },
  messageWrapper: {
    display:'flex',
    alignItems: 'center'
  },
  infoIcon: {
    height: '25px',
    marginLeft: '5px',
    color: 'red',
    cursor: 'pointer'
  }
});

export interface ValidationErrorType {
  executedStatement: string;
  success: boolean;
  validatedExpression: string;
  validatedExpressionRole: string;
  validatedExpressionType: string;
  validatedIndex: number;
  validationError: string;
  validationErrorDetails: string;
}

export interface ImportErrorMessageType {
  message: string;
  reason?: ValidationErrorType[];
}

interface ErrorMessageProps {
  message: ImportErrorMessageType | undefined;
}

export default function ErrorMessage(props: ErrorMessageProps): JSX.Element {
  const classes = useStyles();
  const [openModal, setOpenModal] = useState<boolean>(false);

  const handleModal = (): void => {
    setOpenModal(!openModal);
  };

  if (!props.message) return <></>;

  return (
    <div className={classes.wrapper}>
      <div className={classes.messageWrapper}>
        <span className={classes.errorMessage}>{props.message.message}</span>
        <span className={classes.infoIcon}>
          <InfoIcon onClick={handleModal} />
        </span>
      </div>
      {props.message?.reason?.map((val) => (
        <span className={classes.errorMessage}>
          {val?.validationError}: {val?.validationErrorDetails}
        </span>
      ))}
      <ErrorModal
        onClose={handleModal}
        isOpen={openModal}
        error={{
          message: props?.message?.message,
          validationErrors: props?.message?.reason ?? [],
        }}
      />
    </div>
  );
}
