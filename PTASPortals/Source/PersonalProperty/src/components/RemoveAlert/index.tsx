// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC } from 'react';
import {
  createStyles,
  StyleRules,
  Theme,
  WithStyles,
  withStyles,
} from '@material-ui/core';
import {
  CustomPopover,
  Alert,
  GenericWithStyles,
} from '@ptas/react-public-ui-library';

interface Props extends WithStyles<typeof styles> {
  anchorEl: HTMLElement | null | undefined;
  alertContent: string | React.ReactNode;
  onClose: () => void;
  onButtonClick: () => void;
  buttonText?: string | React.ReactNode;
}

const styles = (theme: Theme): StyleRules<string, Props> =>
  createStyles({
    removePopover: {
      borderRadius: '9px',
    },
    removeAlert: {
      width: '304px',
      boxSizing: 'border-box',
      padding: '16px 32px 27px 32px',
      borderRadius: '9px',

      fontSize: theme.ptas.typography.bodyLargeBold.fontSize,
      fontWeight: theme.ptas.typography.bodyLargeBold.fontWeight,
      lineHeight: '24px',
    },
    removeAlertButton: {
      width: 'auto',
      height: 38,
      marginLeft: 'auto',
      marginRight: 'auto',
      display: 'block',
      fontSize: '18px',
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '18px',
      padding: '10px 32px 10px 32px',
    },
  });

function RemoveAlert(props: Props): JSX.Element {
  const { classes } = props;

  return (
    <CustomPopover
      classes={{ paper: classes.removePopover }}
      anchorEl={props.anchorEl}
      onClose={(): void => {
        props.onClose();
      }}
      ptasVariant="danger"
      showCloseButton
      tail
      tailPosition="end"
      anchorOrigin={{
        vertical: 'bottom',
        horizontal: 'right',
      }}
      transformOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
    >
      <Alert
        contentText={props.alertContent}
        ptasVariant="danger"
        classes={{
          root: classes.removeAlert,
          // text: classes.removeAlertText,
          buttons: classes.removeAlertButton,
        }}
        okShowButton
        okButtonText={props.buttonText}
        okButtonClick={(): void => {
          props.onButtonClick();
        }}
      />
    </CustomPopover>
  );
}

export default withStyles(styles)(RemoveAlert) as FC<GenericWithStyles<Props>>;
