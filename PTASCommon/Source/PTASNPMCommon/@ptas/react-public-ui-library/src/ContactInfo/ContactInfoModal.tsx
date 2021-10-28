// ContactInfoAlert.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment, useEffect } from "react";
import { createStyles, WithStyles, withStyles, Box } from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { CustomPopover } from "./../CustomPopover";
import { CustomButton } from "../CustomButton";
import { AddCircleOutline as AddIcon } from "@material-ui/icons";
import { v4 as uuid } from "uuid";
import EmailEntity, { EmailEntityTextProps } from "./EmailEntity";
import AddressEntity, { AddressEntityTextProps } from "./AddressEntity";
import { AddressInfo } from "./AddressInfoFields";
import PhoneEntity, { PhoneEntityTextProps } from "./PhoneEntity";
import { PhoneInfo, PhoneType } from "./PhoneInfoFields";
import { DropDownItem } from "../SimpleDropDown";

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles> {
  variant: "email" | "address" | "phone";
  newItemText: string | React.ReactNode;
  anchorEl: HTMLElement | null | undefined;
  onClose: () => void;
  onClickNewItem?: () => void;
  onRemoveItem: (id: string | number) => void;
  onPhoneTypeSelect?: (id: string | number, phoneType: PhoneType) => void;
  countryItems?: DropDownItem[];
  emailList?: string[];
  addressList?: AddressInfo[];
  phoneList?: PhoneInfo[];
  emailTexts?: EmailEntityTextProps;
  addressTexts?: AddressEntityTextProps;
  phoneTexts?: PhoneEntityTextProps;
}

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    root: {},
    paper: {
      width: "380px",
      boxSizing: "border-box",
      padding: "42px 10px 16px 10px",
      borderRadius: "9px",
      marginTop: 0,

      [theme.breakpoints.up("sm")]: {
        marginTop: "60px",
        padding: "42px 30px 16px 30px"
      },

      overflowX: "auto"
    },
    backdrop: {
      backgroundColor: "rgba(0, 0, 0, 0.5) !important"
    },
    closeButton: {
      color: theme.ptas.colors.theme.black
    },
    buttonNew: {
      width: "auto",
      height: 24,
      padding: "3px 22px",
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: "17px"
    },
    addIcon: {
      width: "18px",
      height: "18px",
      marginRight: theme.spacing(0.5)
    },
    border: {
      background:
        "linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)",
      height: 1,
      width: "100%",
      display: "block",
      marginTop: theme.spacing(2),
      marginBottom: theme.spacing(1)
    },
    entities: {
      display: "flex",
      flexDirection: "column",
      alignItems: "center"
    }
  });

/**
 * ContactInfoModal
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ContactInfoModal(props: Props): JSX.Element {
  const {
    classes,
    variant,
    newItemText,
    anchorEl,
    onClose,
    onClickNewItem,
    onRemoveItem,
    onPhoneTypeSelect,
    countryItems,
    emailList,
    addressList,
    phoneList
  } = props;

  useEffect(() => {
    //Workaround for displaying correct width of indicator for tabs component (used in phone modal).
    //Issue https://github.com/mui-org/material-ui/issues/9337
    if (variant === "phone") {
      setTimeout(() => {
        window.dispatchEvent(new CustomEvent("resize"));
      }, 10);
    }
  });

  return (
    <Box>
      <CustomPopover
        classes={{
          root: `${classes.root} ${props.classes?.root}`,
          paper: `${classes.paper} ${props.classes?.paper}`,
          closeButton: `${classes.closeButton} ${props.classes?.closeButton}`,
          backdropRoot: classes.backdrop
        }}
        anchorEl={anchorEl}
        onClose={(): void => {
          onClose();
        }}
        ptasVariant='info'
        showCloseButton
        anchorOrigin={{
          vertical: "top",
          horizontal: "center"
        }}
        transformOrigin={{
          vertical: "top",
          horizontal: "center"
        }}
      >
        <Box>
          <CustomButton
            onClick={onClickNewItem}
            classes={{ root: classes.buttonNew }}
          >
            <AddIcon
              classes={{
                root: classes.addIcon
              }}
            />
            {newItemText ?? "New item"}
          </CustomButton>
          {variant === "email" && (
            <Box className={classes.entities}>
              {emailList?.length &&
                emailList.map((email, i) => (
                  <Fragment key={uuid()}>
                    <EmailEntity
                      email={email}
                      onRemove={() => onRemoveItem(email)}
                      {...props.emailTexts}
                    />
                    {i < emailList.length - 1 && (
                      <span className={classes.border}></span>
                    )}
                  </Fragment>
                ))}
            </Box>
          )}
          {variant === "address" && (
            <Box className={classes.entities}>
              {addressList &&
                addressList.length > 0 &&
                addressList.map((address, i) => (
                  <Fragment key={uuid()}>
                    <AddressEntity
                      address={address}
                      onRemove={() => onRemoveItem(address.id)}
                      countryItems={countryItems}
                      {...props.addressTexts}
                    />
                    {i < addressList.length - 1 && (
                      <span className={classes.border}></span>
                    )}
                  </Fragment>
                ))}
            </Box>
          )}
          {variant === "phone" && (
            <Box>
              <Box className={classes.entities}>
                {phoneList &&
                  phoneList.length > 0 &&
                  phoneList.map((phone, i) => (
                    <Fragment key={uuid()}>
                      <PhoneEntity
                        phone={phone}
                        onRemove={() => onRemoveItem(phone.id)}
                        onPhoneTypeSelect={onPhoneTypeSelect}
                        {...props.phoneTexts}
                      />
                      {i < phoneList.length - 1 && (
                        <span className={classes.border}></span>
                      )}
                    </Fragment>
                  ))}
              </Box>
            </Box>
          )}
        </Box>
      </CustomPopover>
    </Box>
  );
}

export default withStyles(styles)(ContactInfoModal) as FC<
  GenericWithStyles<Props>
>;
