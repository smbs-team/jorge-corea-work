// PhoneEntity.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, useState } from 'react';
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
  Alert,
  CustomPopover,
} from '@ptas/react-public-ui-library';
import { PortalPhone } from 'models/portalContact';
import PhoneInfoFields, { PhoneInfoTextProps } from './PhoneInfoFields';

export interface PhoneEntityTextProps extends PhoneInfoTextProps {
  removeButtonText?: string | React.ReactNode;
  removeAlertText?: string | React.ReactNode;
  removeAlertButtonText?: string | React.ReactNode;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles>, PhoneEntityTextProps {
  phone: PortalPhone;
  phoneTypeList: { value: number; label: string }[];
  onRemove: (id: string) => void;
  onChange: (phone: PortalPhone) => void; //Used for editing phone on Contact (from modal)
}

/**
 * Component styles
 */
const styles = (theme: Theme): StyleRules<string, object> =>
  createStyles({
    root: {
      width: '100%',
      maxWidth: '270px',
    },
    phoneInfo: {
      marginTop: theme.spacing(3),
      marginBottom: theme.spacing(2.125),
    },
    line: {
      marginBottom: theme.spacing(3),
    },
    phoneNumber: {
      maxWidth: '196px',
    },
    phoneTypeTabs: {
      maxWidth: '267px',
    },
    tab: {
      padding: '0 11px',
    },
    textMessagesSwitch: {},
    removeLine: {
      display: 'flex',
      justifyContent: 'flex-end',
    },
    textButton: {
      color: theme.ptas.colors.utility.danger,
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '19px',
    },
    popoverRoot: {
      width: 304,
      padding: '16px 32px 23px 32px ',
      boxSizing: 'border-box',
    },
    alertText: {
      fontSize: theme.ptas.typography.bodyLarge.fontSize,
      color: theme.ptas.colors.theme.white,
    },
    button: {
      width: 128,
      height: 38,
      marginLeft: 'auto',
      marginRight: 'auto',
      display: 'block',
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
    },
  });

/**
 * PhoneEntity
 *
 * @param props - Component props
 * @returns A JSX element
 */
function PhoneEntity(props: Props): JSX.Element {
  const { classes, phone, onRemove, onChange } = props;
  const [popoverAnchor, setPopoverAnchor] = useState<HTMLElement | null>(null);

  const onPhoneNumberChange = (phoneNumber: string): void => {
    const newPhone = { ...phone, phoneNumber: phoneNumber };
    onChange(newPhone);
  };

  const onPhoneTypeSelect = (phoneType: number): void => {
    const newPhone = { ...phone, phoneTypeValue: phoneType };
    onChange(newPhone);
  };

  const onAcceptMessagesChange = (checked: boolean): void => {
    const newPhone = { ...phone, acceptsTextMessages: checked };
    onChange(newPhone);
  };

  return (
    <Box className={classes.root}>
      <PhoneInfoFields
        phone={props.phone}
        phoneTypeList={props.phoneTypeList}
        onPhoneNumberChange={onPhoneNumberChange}
        onPhoneTypeSelect={onPhoneTypeSelect}
        onAcceptMessagesChange={onAcceptMessagesChange}
        titleText={props.titleText}
        placeholderText={props.placeholderText}
        tabCellText={props.tabCellText}
        tabWorkText={props.tabWorkText}
        tabHomeText={props.tabHomeText}
        tabTollFreeText={props.tabTollFreeText}
        acceptMessagesText={props.acceptMessagesText}
      />
      <Box className={classes.removeLine}>
        <CustomTextButton
          classes={{ root: classes.textButton }}
          ptasVariant="Text more"
          onClick={(evt: React.MouseEvent<HTMLButtonElement>): void =>
            setPopoverAnchor(evt.currentTarget)
          }
        >
          {props.removeButtonText ?? 'Remove phone'}
        </CustomTextButton>
      </Box>
      <CustomPopover
        anchorEl={popoverAnchor}
        onClose={(): void => {
          setPopoverAnchor(null);
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
          contentText={props.removeAlertText ?? 'Remove phone'}
          ptasVariant="danger"
          okButtonText={props.removeAlertButtonText ?? 'Remove'}
          okShowButton
          classes={{
            root: classes.popoverRoot,
            text: classes.alertText,
            buttons: classes.button,
          }}
          okButtonClick={(): void => {
            setPopoverAnchor(null);
            onRemove(phone.id);
          }}
        />
      </CustomPopover>
    </Box>
  );
}

export default withStyles(styles)(PhoneEntity) as FC<GenericWithStyles<Props>>;
