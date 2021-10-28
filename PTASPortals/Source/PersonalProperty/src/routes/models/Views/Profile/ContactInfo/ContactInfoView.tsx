// ContactInfoView.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment } from 'react';
import {
  createStyles,
  WithStyles,
  withStyles,
  Box,
  StyleRules,
} from '@material-ui/core';
import { Theme } from '@material-ui/core/styles';
import {
  GenericWithStyles,
  CustomTextButton,
} from '@ptas/react-public-ui-library';

export interface ContactInfoViewTextProps {
  //Email
  useOtherEmailText?: string | React.ReactNode;
  //Address
  useOtherAddressText?: string | React.ReactNode;
  //Phone
  useOtherPhoneText?: string | React.ReactNode;
  phoneAcceptsMessagesText?: string | React.ReactNode;
  phoneAcceptsNoMessagesText?: string | React.ReactNode;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles>, ContactInfoViewTextProps {
  variant: 'email' | 'address' | 'phone';
  onButtonClick: (evt: React.MouseEvent<HTMLButtonElement, MouseEvent>) => void;
  //Email
  email?: string;
  //Address
  addressTitle?: string;
  addressLine1?: string;
  addressLine2?: string;
  //Phone
  phoneNumber?: string;
  acceptsTextMessages?: boolean;
}

/**
 * Component styles
 */
const styles = (theme: Theme): StyleRules<string, object> =>
  createStyles({
    root: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
    },
    textButtonContainer: {
      display: 'flex',
      justifyContent: 'flex-end',
      marginBottom: theme.spacing(0.5),
    },
    textButton: {
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '19px',
    },
    selectedEmail: {
      fontSize: theme.ptas.typography.bodyBold.fontSize,
      fontWeight: theme.ptas.typography.bodyBold.fontWeight,
      lineHeight: '22px',
    },
    addressLine1: {
      display: 'block',
      fontSize: theme.ptas.typography.bodyBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '22px',
      marginBottom: theme.spacing(0.5),
    },
    addressLine2: {
      display: 'block',
      fontSize: theme.ptas.typography.bodyBold.fontSize,
      fontWeight: theme.ptas.typography.bodyBold.fontWeight,
      lineHeight: '22px',
      marginBottom: theme.spacing(0.5),
    },
    phone: {
      display: 'block',
      fontSize: theme.ptas.typography.bodyBold.fontSize,
      fontWeight: theme.ptas.typography.bodyBold.fontWeight,
      lineHeight: '22px',
      marginBottom: theme.spacing(0.5),
    },
    textMessage: {
      // fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
      lineHeight: '17px',
      color: 'rgba(0, 0, 0, 0.54)',
    },
  });

/**
 * ContactInfoView
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ContactInfoView(props: Props): JSX.Element {
  const { classes, variant } = props;

  return (
    <Box className={classes.root}>
      <Box className={classes.textButtonContainer}>
        <CustomTextButton
          ptasVariant="Text more"
          classes={{ root: classes.textButton }}
          onClick={props.onButtonClick}
        >
          {variant === 'email'
            ? props.useOtherEmailText ?? 'Use other email'
            : variant === 'address'
            ? props.useOtherAddressText ?? 'Use other address'
            : variant === 'phone'
            ? props.useOtherPhoneText ?? 'Use other phone'
            : ''}
        </CustomTextButton>
      </Box>
      {variant === 'email' && (
        <label className={classes.selectedEmail}>{props.email}</label>
      )}
      {variant === 'address' && (
        <Fragment>
          <label className={classes.addressLine1}>{props.addressTitle}</label>
          <label className={classes.addressLine2}>{props.addressLine1}</label>
          <label className={classes.addressLine2}>{props.addressLine2}</label>
        </Fragment>
      )}
      {variant === 'phone' && (
        <Fragment>
          <label className={classes.phone}>{props.phoneNumber}</label>
          <label className={classes.textMessage}>
            {props.acceptsTextMessages
              ? props.phoneAcceptsMessagesText
              : props.phoneAcceptsNoMessagesText}
          </label>
        </Fragment>
      )}
    </Box>
  );
}

export default withStyles(styles)(ContactInfoView) as FC<
  GenericWithStyles<Props>
>;
