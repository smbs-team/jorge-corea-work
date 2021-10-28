// AddressInfoFields.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC } from "react";
import { createStyles, WithStyles, withStyles, Box } from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { CustomTextField } from "../CustomTextField";
import { DropDownItem, SimpleDropDown } from "../SimpleDropDown";
import clsx from "clsx";

export interface AddressInfo {
  id: string | number;
  title: string;
  country: string;
  addressLine1: string;
  addressLine2: string;
  city: string;
  state: string;
  zip: string;
}

export interface AddressInfoTextProps {
  titleLabel?: string | React.ReactNode;
  addressLine1Label?: string | React.ReactNode;
  addressLine2Label?: string | React.ReactNode;
  countryLabel?: string | React.ReactNode;
  cityLabel?: string | React.ReactNode;
  stateLabel?: string | React.ReactNode;
  zipLabel?: string | React.ReactNode;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles>, AddressInfoTextProps {
  address: AddressInfo;
  countryItems?: DropDownItem[];
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
      maxWidth: "270px",
      marginTop: theme.spacing(3),
      marginBottom: theme.spacing(2.125)
    },
    line: {
      display: "flex",
      justifyContent: "space-between",
      marginBottom: theme.spacing(3.125),
      flexDirection: "column",
      "& > div": {
        marginBottom: 10
      },

      "& > div:last-child": {
        marginBottom: 0
      },

      [theme.breakpoints.up("sm")]: {
        flexDirection: "row",
        "& > div": {
          marginBottom: 0
        }
      }
    },
    lastLine: {
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
    statePadding: {
      padding: 0,

      "& > fieldset": {
        padding: 0
      }
    },
    shrinkCity: {
      left: -6
    },
    zip: {
      width: "101px"
    }
  });

/**
 * AddressInfoFields
 *
 * @param props - Component props
 * @returns A JSX element
 */
function AddressInfoFields(props: Props): JSX.Element {
  const { classes, address } = props;
  return (
    <Box className={classes.root}>
      <Box className={classes.line}>
        <CustomTextField
          classes={{ root: classes.addressTitle }}
          ptasVariant='outline'
          label={props.titleLabel ?? "Address title"}
          value={address.title}
          onChange={(e) => props.onTitleChange?.(e.currentTarget.value)}
        />
        <SimpleDropDown
          items={props.countryItems ?? []}
          label={props.countryLabel ?? "Country"}
          onSelected={(item) => props.onCountryChange?.(item)}
          defaultValue={address.country}
          classes={{
            textFieldRoot: classes.countryDropdown,
            inputRoot: classes.countryDropdownInput
            // animated: classes.dropdownLabel
          }}
        />
      </Box>
      <Box className={classes.line}>
        <CustomTextField
          classes={{ root: classes.addressLine }}
          ptasVariant='outline'
          label={props.addressLine1Label ?? "Address"}
          value={address.addressLine1}
          onChange={(e) => props.onLine1Change?.(e.currentTarget.value)}
        />
      </Box>
      <Box className={classes.line}>
        <CustomTextField
          classes={{ root: classes.addressLine }}
          ptasVariant='outline'
          label={props.addressLine2Label ?? "Address (cont.)"}
          value={address.addressLine2}
          onChange={(e) => props.onLine2Change?.(e.currentTarget.value)}
        />
      </Box>
      <Box className={clsx(classes.line, classes.lastLine)}>
        <CustomTextField
          classes={{ root: classes.city }}
          ptasVariant='outline'
          label={props.cityLabel ?? "City"}
          value={address.city}
          onChange={(e) => props.onCityChange?.(e.currentTarget.value)}
        />
        <CustomTextField
          classes={{
            root: classes.state,
            fullWidth: classes.statePadding,
            shrinkRoot: classes.shrinkCity
          }}
          ptasVariant='outline'
          label={props.stateLabel ?? "State"}
          value={address.state}
          onChange={(e) => props.onStateChange?.(e.currentTarget.value)}
        />
        <CustomTextField
          classes={{ root: classes.zip }}
          ptasVariant='outline'
          label={props.zipLabel ?? "Zip"}
          value={address.zip}
          onChange={(e) => props.onZipChange?.(e.currentTarget.value)}
        />
      </Box>
    </Box>
  );
}

export default withStyles(styles)(AddressInfoFields) as FC<
  GenericWithStyles<Props>
>;
