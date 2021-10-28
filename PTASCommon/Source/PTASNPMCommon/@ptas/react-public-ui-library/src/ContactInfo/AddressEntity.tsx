// AddressEntity.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, useState } from "react";
import { createStyles, WithStyles, withStyles, Box } from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { CustomTextButton } from "../CustomTextButton";
import { Alert } from "../Alert";
import { CustomPopover } from "../CustomPopover";
import { DropDownItem } from "../SimpleDropDown";
import { AddressInfo, AddressInfoTextProps } from "./AddressInfoFields";
import { AddressInfoFields } from ".";

export interface AddressEntityTextProps extends AddressInfoTextProps {
  removeButtonText?: string | React.ReactNode;
  removeAlertText?: string | React.ReactNode;
  removeAlertButtonText?: string | React.ReactNode;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles>, AddressEntityTextProps {
  address: AddressInfo;
  countryItems?: DropDownItem[];
  onRemove: (id: string | number) => void;
  onTitleChange?: (value: string) => void;
  onCountryChange?: (value: DropDownItem) => void;
  onLine1Change?: (value: string) => void;
  onLine2Change?: (value: string) => void;
  onCityChange?: (value: string) => void;
  onStateChange?: (value: string) => void;
  onZipChange?: (value: string) => void;
}

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    root: {
      width: "100%",
      maxWidth: "270px"
    },
    addressInfo: {
      marginTop: theme.spacing(3),
      marginBottom: theme.spacing(2.125)
    },
    line: {
      display: "flex",
      justifyContent: "space-between",
      marginTop: theme.spacing(1),
      marginBottom: theme.spacing(2.125)
    },
    addressTitle: {
      maxWidth: "198px"
    },
    countryDropdown: {
      maxWidth: "62px",
      minWidth: "62px"
    },
    countryDropdownInput: {
      height: "36px"
    },
    addressLine: {
      maxWidth: "270px"
    },
    city: {
      width: "107px"
    },
    state: {
      width: "45px"
    },
    zip: {
      width: "101px"
    },
    removeLine: {
      display: "flex",
      justifyContent: "flex-end"
    },
    textButton: {
      color: theme.ptas.colors.utility.danger,
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: "19px"
    },
    popoverRoot: {
      width: 304,
      padding: "16px 32px 23px 32px ",
      boxSizing: "border-box"
    },
    alertText: {
      fontSize: theme.ptas.typography.bodyLarge.fontSize,
      color: theme.ptas.colors.theme.white
    },
    button: {
      width: 128,
      height: 38,
      marginLeft: "auto",
      marginRight: "auto",
      display: "block",
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: theme.ptas.typography.buttonLarge.fontWeight
    }
  });

/**
 * AddressEntity
 *
 * @param props - Component props
 * @returns A JSX element
 */
function AddressEntity(props: Props): JSX.Element {
  const { classes, address, onRemove } = props;
  const [popoverAnchor, setPopoverAnchor] = useState<HTMLElement | null>(null);

  return (
    <Box className={classes.root}>
      <AddressInfoFields
        address={address}
        countryItems={props.countryItems}
        onTitleChange={props.onTitleChange}
        onCountryChange={props.onCountryChange}
        onLine1Change={props.onLine1Change}
        onLine2Change={props.onLine2Change}
        onCityChange={props.onCityChange}
        onStateChange={props.onStateChange}
        onZipChange={props.onZipChange}
        titleLabel={props.titleLabel}
        addressLine1Label={props.addressLine1Label}
        addressLine2Label={props.addressLine2Label}
        countryLabel={props.countryLabel}
        cityLabel={props.cityLabel}
        stateLabel={props.stateLabel}
        zipLabel={props.zipLabel}
      />
      <Box className={classes.removeLine}>
        <CustomTextButton
          classes={{ root: classes.textButton }}
          ptasVariant='Text more'
          onClick={(evt): void => setPopoverAnchor(evt.currentTarget)}
        >
          {props.removeButtonText ?? "Remove address"}
        </CustomTextButton>
      </Box>
      <CustomPopover
        anchorEl={popoverAnchor}
        onClose={(): void => {
          setPopoverAnchor(null);
        }}
        ptasVariant='danger'
        showCloseButton
        tail
        tailPosition='end'
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "right"
        }}
        transformOrigin={{
          vertical: "top",
          horizontal: "right"
        }}
      >
        <Alert
          contentText={props.removeAlertText ?? "Remove address"}
          ptasVariant='danger'
          okButtonText={props.removeAlertButtonText ?? "Remove"}
          okShowButton
          classes={{
            root: classes.popoverRoot,
            text: classes.alertText,
            buttons: classes.button
          }}
          okButtonClick={(): void => {
            setPopoverAnchor(null);
            onRemove(address.id);
          }}
        />
      </CustomPopover>
    </Box>
  );
}

export default withStyles(styles)(AddressEntity) as FC<
  GenericWithStyles<Props>
>;
