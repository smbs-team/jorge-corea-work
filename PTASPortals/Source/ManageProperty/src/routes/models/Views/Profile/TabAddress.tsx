// TabAddress.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from 'react';
import {
  CustomTextButton,
  ContactInfoAlert,
  ListItem,
} from '@ptas/react-public-ui-library';
import ContactInfoModal from './ContactInfoModal';
import AddressInfoFields from './ContactInfoModal/AddressInfoFields';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { useProfile } from './ProfileContext';
import { useStyles } from './profileStyles';
import { ProfileStep } from './types';

interface Props {
  updateFormIsValid: (step: ProfileStep, valid: boolean) => void;
}

function TabAddress(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const {
    editingContact: profile,
    addressAnchor,
    setAddressAnchor,
    manageAddressAnchor,
    setManageAddressAnchor,
    addressList,
    addressSuggestions,
    countryItems,
    stateSuggestions,
    citySuggestions,
    zipCodeSuggestions,
    onAddressSearchChange,
    onCitySearchChange,
    onStateSearchChange,
    onZipCodeSearchChange,
    onTitleChange,
    onLine1Change,
    onLine2Change,
    onCountryChange,
    onCityChange,
    onStateChange,
    onZipCodeChange,
    onAddEmptyAddress,
    onChangeAddress,
    onRemoveAddress,
    getAddressData,
  } = useProfile();

  return (
    <Box className={classes.tab}>
      {profile?.address?.isSaved && (
        <Fragment>
          <Box className={classes.textButtonContainer}>
            <CustomTextButton
              ptasVariant="Text more"
              classes={{ root: classes.textButton }}
              onClick={(
                evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
              ): void => setAddressAnchor(evt.currentTarget)}
            >
              {fm.otherAddress}
            </CustomTextButton>
          </Box>
          <label className={classes.addressLine1}>
            {profile.address.title}
          </label>
          <label className={classes.addressLine2}>
            {profile.address.line1}
          </label>
          <label className={classes.addressLine2}>
            {profile.address.city}, {profile.address.state}
          </label>
        </Fragment>
      )}
      {profile?.address && !profile?.address?.isSaved && (
        <AddressInfoFields
          address={profile?.address}
          addressSuggestions={addressSuggestions.map.get(profile.address.id)}
          loadingAddressSuggestions={addressSuggestions.loading.get(
            profile.address.id
          )}
          countryItems={countryItems}
          stateSuggestions={stateSuggestions.map.get(profile.address.id)}
          loadingStateSuggestions={stateSuggestions.loading.get(
            profile.address.id
          )}
          citySuggestions={citySuggestions.map.get(profile.address.id)}
          loadingCitySuggestions={citySuggestions.loading.get(
            profile.address.id
          )}
          zipCodeSuggestions={zipCodeSuggestions.map.get(profile.address.id)}
          loadingZipCodeSuggestions={zipCodeSuggestions.loading.get(
            profile.address.id
          )}
          onTitleChange={onTitleChange}
          onLine1Change={onLine1Change}
          onLine2Change={onLine2Change}
          onCountryChange={onCountryChange}
          onCityChange={onCityChange}
          onStateChange={onStateChange}
          onZipCodeChange={onZipCodeChange}
          onAddressSearchChange={onAddressSearchChange}
          onCitySearchChange={onCitySearchChange}
          onStateSearchChange={onStateSearchChange}
          onZipCodeSearchChange={onZipCodeSearchChange}
          updateFormIsValid={updateFormIsValid}
          addressTexts={{
            titleLabel: fm.addressTitle,
            addressLine1Label: fm.addressLine1,
            addressLine2Label: fm.addressLine2,
            countryLabel: fm.addressCountry,
            cityLabel: fm.addressCity,
            stateLabel: fm.addressState,
            zipLabel: fm.addressZip,
            removeButtonText: fm.modalRemoveButtonAddress,
            removeAlertText: fm.modalRemoveAlertAddress,
            removeAlertButtonText: fm.modalRemoveAlertButtonAddress,
          }}
        />
      )}

      {addressAnchor && (
        <ContactInfoAlert
          anchorEl={addressAnchor}
          items={addressList.map((i) => ({
            value: i.id,
            label: i.title,
          }))}
          onItemClick={(item: ListItem): void =>
            console.log('Selected item:', item)
          }
          buttonText={fm.manageAddresses}
          onButtonClick={(): void => {
            setManageAddressAnchor(document.body);
          }}
          onClose={(): void => {
            setAddressAnchor(null);
          }}
        />
      )}

      {manageAddressAnchor && (
        <ContactInfoModal
          variant={'address'}
          anchorEl={manageAddressAnchor}
          onClose={(): void => setManageAddressAnchor(null)}
          onClickNewItem={onAddEmptyAddress}
          onRemoveItem={onRemoveAddress}
          onChangeItem={onChangeAddress}
          addressList={addressList}
          addressSuggestions={addressSuggestions}
          countryItems={countryItems}
          citySuggestions={citySuggestions}
          stateSuggestions={stateSuggestions}
          zipCodeSuggestions={zipCodeSuggestions}
          onAddressSearchChange={onAddressSearchChange}
          onCitySearchChange={onCitySearchChange}
          onStateSearchChange={onStateSearchChange}
          onZipCodeSearchChange={onZipCodeSearchChange}
          getAddressData={getAddressData}
          newItemText={fm.newAddress}
          addressTexts={{
            titleLabel: fm.addressTitle,
            addressLine1Label: fm.addressLine1,
            addressLine2Label: fm.addressLine2,
            countryLabel: fm.addressCountry,
            cityLabel: fm.addressCity,
            stateLabel: fm.addressState,
            zipLabel: fm.addressZip,
            removeButtonText: fm.modalRemoveButtonAddress,
            removeAlertText: fm.modalRemoveAlertAddress,
            removeAlertButtonText: fm.modalRemoveAlertButtonAddress,
          }}
        />
      )}
    </Box>
  );
}

export default TabAddress;
