// PhoneInfoFields.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, ReactNode } from "react";
import {
  createStyles,
  WithStyles,
  withStyles,
  Box,
  useTheme
} from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { CustomPhoneTextField } from "../CustomPhoneTextField";
import { CustomTabs } from "../CustomTabs";
import { CustomSwitch } from "../CustomSwitch";
import { formatMessageStructure } from "../utils/formatMessage";

export type PhoneType = "cell" | "work" | "home" | "tollFree";
export interface PhoneInfo {
  id: string | number;
  phoneNumber: string;
  phoneType: PhoneType;
  textMessages: boolean;
  titleLabel?: string;
}

export interface PhoneInfoTextProps {
  titleText?: string | ReactNode;
  placeholderText?: string | formatMessageStructure;
  tabCellText?: string | ReactNode;
  tabWorkText?: string | ReactNode;
  tabHomeText?: string | ReactNode;
  tabTollFreeText?: string | ReactNode;
  acceptMessagesText?: string | ReactNode;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles>, PhoneInfoTextProps {
  phone: PhoneInfo;
  onPhoneNumberChange?: (phoneNumber: string) => void;
  onPhoneTypeSelect?: (id: string | number, phoneType: PhoneType) => void;
  onAcceptMessagesChange?: (checked: boolean) => void;
}

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    root: {
      width: "100%",
      maxWidth: "270px",
      marginTop: theme.spacing(3),
      marginBottom: theme.spacing(2.125)
    },
    line: {
      marginBottom: theme.spacing(3)
    },
    phoneNumber: {
      maxWidth: "196px"
    },
    phoneTypeTabs: {
      maxWidth: "267px"
    },
    tab: {
      padding: "0 11px"
    },
    textMessagesSwitch: {}
  });

/**
 * PhoneInfoFields
 *
 * @param props - Component props
 * @returns A JSX element
 */
function PhoneInfoFields(props: Props): JSX.Element {
  const { classes, phone, onPhoneTypeSelect } = props;
  const theme = useTheme();

  return (
    <Box className={classes.root}>
      <Box className={classes.line}>
        <CustomPhoneTextField
          classes={{ root: classes.phoneNumber }}
          label={props.titleText ?? "Phone"}
          placeholder={props.placeholderText ?? "Phone"}
          value={phone.phoneNumber}
          onChange={(e) => props.onPhoneNumberChange?.(e.currentTarget.value)}
        />
      </Box>
      <Box className={classes.line}>
        <CustomTabs
          classes={{ root: classes.phoneTypeTabs, item: classes.tab }}
          selectedIndex={
            phone.phoneType === "cell"
              ? 0
              : phone.phoneType === "work"
              ? 1
              : phone.phoneType === "home"
              ? 2
              : phone.phoneType === "tollFree"
              ? 3
              : 0
          }
          ptasVariant='SwitchMedium'
          items={[
            { label: props.tabCellText ?? "Cell", disabled: false },
            {
              label: props.tabWorkText ?? "Work",
              disabled: false
            },
            {
              label: props.tabHomeText ?? "Home",
              disabled: false
            },
            {
              label: props.tabTollFreeText ?? "Toll free",
              disabled: false
            }
          ]}
          onSelected={(tab: number): void => {
            const newPhoneType: PhoneType =
              tab === 0
                ? "cell"
                : tab === 1
                ? "work"
                : tab === 2
                ? "home"
                : tab === 3
                ? "tollFree"
                : "cell";
            onPhoneTypeSelect && onPhoneTypeSelect(phone.id, newPhoneType);
          }}
          indicatorBackgroundColor={theme.ptas.colors.theme.white}
          itemTextColor={theme.ptas.colors.theme.white}
          selectedItemTextColor={theme.ptas.colors.theme.black}
          tabsBackgroundColor={theme.ptas.colors.theme.gray}
        />
      </Box>
      <Box className={classes.line}>
        <CustomSwitch
          classes={{ root: classes.textMessagesSwitch }}
          label={props.acceptMessagesText ?? "Accepts text messages"}
          ptasVariant='normal'
          showOptions
          isChecked={(checked) => props.onAcceptMessagesChange?.(checked)}
        />
      </Box>
    </Box>
  );
}

export default withStyles(styles)(PhoneInfoFields) as FC<
  GenericWithStyles<Props>
>;
