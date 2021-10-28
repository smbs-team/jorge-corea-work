// AddressEditor.tsx
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
  DropDownItem,
  ItemSuggestion,
} from '@ptas/react-public-ui-library';
import AddressInfoFields, { AddressInfoTextProps } from './AddressInfoFields';
import { PortalAddress } from 'models/portalContact';
import { AddressLookupEntity, BasicAddressData } from 'models/addresses';

export interface AddressEditorTextProps extends AddressInfoTextProps {
  removeButtonText?: string | React.ReactNode;
  removeAlertText?: string | React.ReactNode;
  removeAlertButtonText?: string | React.ReactNode;
}

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles>, AddressEditorTextProps {
  address: PortalAddress;
  addressSuggestions: AddressLookupEntity[];
  countryItems?: DropDownItem[];
  citySuggestions: ItemSuggestion[];
  stateSuggestions: ItemSuggestion[];
  zipCodeSuggestions: ItemSuggestion[];
  onRemove: (id: string) => void;
  onAddressSearchChange?: (value: string) => void;
  onCitySearchChange?: (value: string) => void;
  onStateSearchChange?: (value: string, countryId: string) => void;
  onZipCodeSearchChange?: (value: string) => void;
  onChange: (address: PortalAddress) => void; //Used for editing address on Contact (from modal)
  getAddressData: (
    addressSuggestion: AddressLookupEntity
  ) => Promise<BasicAddressData>;
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
    addressInfo: {
      marginTop: theme.spacing(3),
      marginBottom: theme.spacing(2.125),
    },
    line: {
      display: 'flex',
      justifyContent: 'space-between',
      marginTop: theme.spacing(1),
      marginBottom: theme.spacing(2.125),
    },
    addressTitle: {
      maxWidth: '198px',
    },
    countryDropdown: {
      maxWidth: '62px',
      minWidth: '62px',
    },
    countryDropdownInput: {
      height: '36px',
    },
    addressLine: {
      maxWidth: '270px',
    },
    city: {
      width: '107px',
    },
    state: {
      width: '45px',
    },
    zip: {
      width: '101px',
    },
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
 * AddressEditor
 *
 * @param props - Component props
 * @returns A JSX element
 */
function AddressEditor(props: Props): JSX.Element {
  const { classes, address, onRemove, onChange } = props;
  const [popoverAnchor, setPopoverAnchor] = useState<HTMLElement | null>(null);

  const onTitleChange = (title: string): void => {
    const newAddress = { ...address, title: title };
    onChange(newAddress);
  };

  const onLine1Change = async (value: string): Promise<void> => {
    const suggestion = props.addressSuggestions.find(
      el => el.formattedaddr === value
    );
    if (!suggestion) return;
    const addressData = await props.getAddressData(suggestion);

    const newAddress = {
      ...address,
      line1: suggestion.streetname,
      countryId: addressData.countryId,
      state: addressData.state,
      stateId: addressData.stateId,
      city: addressData.city,
      cityId: addressData.cityId,
      zipCode: addressData.zipCode,
      zipCodeId: addressData.zipCodeId,
    };
    onChange(newAddress);
  };

  const onLine2Change = (value: string): void => {
    const newAddress = { ...address, line2: value };
    onChange(newAddress);
  };

  const onCountryChange = (item: DropDownItem): void => {
    const newAddress = {
      ...address,
      countryId: item.value as string,
      //When country changes, clear state/city/zip
      state: '',
      stateId: '',
      city: '',
      cityId: '',
      zipCode: '',
      zipCodeId: '',
    };
    onChange(newAddress);
  };

  const onCityChange = (item: ItemSuggestion): void => {
    const newAddress = {
      ...address,
      cityId: item.id as string,
      city: item.title,
    };
    onChange(newAddress);
  };

  const onStateChange = (item: ItemSuggestion): void => {
    const newAddress = {
      ...address,
      stateId: item.id as string,
      state: item.title,
    };
    onChange(newAddress);
  };

  const onZipCodeChange = (item: ItemSuggestion): void => {
    const newAddress = {
      ...address,
      zipCodeId: item.id as string,
      zipCode: item.title,
    };
    onChange(newAddress);
  };

  return (
    <Box className={classes.root}>
      <AddressInfoFields
        address={address}
        addressSuggestions={props.addressSuggestions}
        countryItems={props.countryItems}
        citySuggestions={props.citySuggestions}
        stateSuggestions={props.stateSuggestions}
        zipCodeSuggestions={props.zipCodeSuggestions}
        onTitleChange={onTitleChange}
        onCountryChange={onCountryChange}
        onLine1Change={onLine1Change}
        onLine2Change={onLine2Change}
        onCityChange={onCityChange}
        onStateChange={onStateChange}
        onZipCodeChange={onZipCodeChange}
        onAddressSearchChange={props.onAddressSearchChange}
        onCitySearchChange={props.onCitySearchChange}
        onStateSearchChange={props.onStateSearchChange}
        onZipCodeSearchChange={props.onZipCodeSearchChange}
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
          ptasVariant="Text more"
          onClick={(evt: React.MouseEvent<HTMLButtonElement>): void =>
            setPopoverAnchor(evt.currentTarget)
          }
        >
          {props.removeButtonText ?? 'Remove address'}
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
          contentText={props.removeAlertText ?? 'Remove address'}
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
            onRemove(address.id);
          }}
        />
      </CustomPopover>
    </Box>
  );
}

export default withStyles(styles)(AddressEditor) as FC<
  GenericWithStyles<Props>
>;
