// EmailExistsPopover.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC } from 'react';
import {
  createStyles,
  WithStyles,
  withStyles,
  StyleRules,
} from '@material-ui/core';
import { Theme } from '@material-ui/core/styles';
import {
  GenericWithStyles,
  CustomPopover,
  Alert,
} from '@ptas/react-public-ui-library';
import clsx from 'clsx';

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles> {
  anchorEl: HTMLElement | null | undefined;
  onClose: () => void;
  onClick: () => void;
  text?: string | React.ReactNode;
  buttonText?: string | React.ReactNode;
}

/**
 * Component styles
 */
const styles = (theme: Theme): StyleRules<string, object> =>
  createStyles({
    root: {},
    popover: {},
    popupColor: {
      backgroundColor: theme.ptas.colors.theme.accent,
      color: theme.ptas.colors.theme.white,
    },
    closePopupButton: {
      color: theme.ptas.colors.theme.white,
    },
    backdrop: {},
    alert: {
      padding: '24px 20px 20px 22px',
      borderRadius: '9px',
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '19px',
    },
    buttonContainer: {
      display: 'flex',
      justifyContent: 'center',
    },
    button: {
      width: 'auto',
      height: 23,
      padding: '3px 22px',
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '17px',
      marginTop: theme.spacing(2),
      color: theme.ptas.colors.theme.accent,
    },
  });

/**
 * EmailExistsPopover
 *
 * @param props - Component props
 * @returns A JSX element
 */
function EmailExistsPopover(props: Props): JSX.Element {
  const { classes, anchorEl, onClose, onClick } = props;

  return (
    <CustomPopover
      classes={{
        root: `${classes.root} ${props.classes?.root}`,
        paper: classes.popover,
        tail: classes.popupColor,
        closeButton: classes.closePopupButton,
        backdropRoot: classes.backdrop,
      }}
      anchorEl={anchorEl}
      onClose={(): void => {
        onClose();
      }}
      ptasVariant="info"
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
        classes={{
          root: clsx(classes.alert, classes.popupColor),
          buttons: classes.button,
          buttonContainer: classes.buttonContainer,
        }}
        contentText={props.text ?? 'This email already exists.'}
        ptasVariant="info"
        okShowButton
        okButtonText={props.buttonText ?? 'Use existing'}
        okButtonClick={(): void => {
          onClick();
        }}
      />
    </CustomPopover>
  );
}

export default withStyles(styles)(EmailExistsPopover) as FC<
  GenericWithStyles<Props>
>;
