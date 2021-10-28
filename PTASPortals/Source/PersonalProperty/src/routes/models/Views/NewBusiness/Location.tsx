// Location.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  SimpleDropDown,
  CustomTextField,
  CustomSearchTextField,
  ItemSuggestion,
  ContactInfoAlert,
  ListItem,
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import * as generalFm from '../../../../GeneralFormatMessage';
import { Box, makeStyles, Theme } from '@material-ui/core';
import clsx from 'clsx';
import useNewBusiness from './useNewBusiness';
import useAddress from 'hooks/useAddress';
import ContactInfoView from '../Profile/ContactInfo/ContactInfoView';
import * as fmProfile from '../Profile/formatText';
const useStyles = makeStyles((theme: Theme) => ({
  contentWrap: {
    width: 280,
    marginBottom: 25,
  },
  stateOfIncorporationRoot: {
    maxWidth: 270,
    marginTop: 57,
    marginBottom: 25,
    display: 'block',
  },
  dropdownStateOfIncorporation: {
    width: 202,
  },
  root: {
    width: '100%',
    maxWidth: '270px',
    marginTop: 8,
    marginBottom: theme.spacing(2.125),
  },
  line: {
    display: 'flex',
    justifyContent: 'space-between',
    marginBottom: theme.spacing(3.125),
    flexDirection: 'column',
    '& > div': {
      marginBottom: 10,
    },

    '& > div:last-child': {
      marginBottom: 0,
    },

    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
      '& > div': {
        marginBottom: 0,
      },
    },
  },
  lastLine: {
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
  statePadding: {
    padding: 0,

    '& > fieldset': {
      padding: 0,
    },
  },
  shrinkCity: {
    left: -6,
  },
  zip: {
    width: '101px',
  },
  addressInputWrap: {
    maxWidth: '100%',
    marginBottom: 25,
  },
  cityInputWrap: {
    maxWidth: 107,
  },
  stateInput: {
    maxWidth: 45,
  },
  zipInput: {
    maxWidth: 100,
  },
}));

function Location(): JSX.Element {
  const classes = useStyles();

  const {
    countryOpts,
    stateOrProvince,
    newBusiness,
    addrLocationError,
    cityLocationError,
    stateLocationError,
    zipCodeLocationError,
    addrContLocationError,
    onChangeCountry,
    handleChangeLine2,
    onSelectZipCode,
    handleChangeState,
    setNewBusiness,
    contactInfo,
    addressAnchor,
    setAddressAnchor,
    addressList,
    setManageInfoAnchor,
  } = useNewBusiness();

  const {
    handleAddressSuggestions,
    getInfoFromAddressSuggestionSelected,
    addressSuggestionList,
    loadingAddressSug,
    handleCitySuggestions,
    citySuggestionList,
    loadingCitySug,
    handleStateSuggestions,
    stateSuggestionList,
    handleZipCodeSuggestions,
    zipCodeSuggestionList,
    loadingZipSug,
    loadingStateSug,
  } = useAddress();

  const {
    hasError: stateOfIncorporationInputHasError,
    inputBlurHandler: stateOfIncorporationInputBlurHandler,
  } = useTextFieldValidation(
    newBusiness.stateOfIncorporationId ?? '',
    utilService.isNotEmpty
  );

  const onSelectAddressSuggestion = async (
    item: ItemSuggestion
  ): Promise<void> => {
    const addressData = await getInfoFromAddressSuggestionSelected(
      (item.id as string) ?? ''
    );

    setNewBusiness((prev) => {
      return {
        ...prev,
        address: addressData?.address ?? '',
        countryId: addressData.country?.ptas_countryid ?? '',
        cityId: addressData.city?.ptas_cityid ?? '',
        addrCityLabel: addressData.city?.ptas_name ?? '',
      };
    });
  };

  const onSelectCity = async (item: ItemSuggestion): Promise<void> => {
    setNewBusiness((prev) => {
      return {
        ...prev,
        cityId: (item.id as string) ?? '',
        addrCityLabel: item.title,
      };
    });
  };

  const onSelectState = (item: ItemSuggestion): void => {
    setNewBusiness((prev) => {
      return {
        ...prev,
        stateId: (item.id as string) ?? '',
        addrStateLabel: item.title,
      };
    });
  };

  const mappingAddressList = addressList.map((i) => ({
    value: i.id,
    label: i.title,
  }));

  const handleSelectInfoAddress = async (item: ListItem): Promise<void> => {
    const itemFound = addressList.find((i) => i.id === item.value);

    if (!itemFound) return;

    setNewBusiness((prev) => {
      return {
        ...prev,
        addrCityLabel: itemFound.city,
        addrZipcodeLabel: itemFound.zipCode,
        addrStateLabel: itemFound.state,
        cityId: itemFound.cityId,
        zipId: itemFound.zipCodeId,
        stateId: itemFound.stateId,
        address: itemFound.line1,
        addrBusinessTitle: itemFound.title,
      };
    });
  };

  const AddressPopup = (
    <ContactInfoAlert
      anchorEl={addressAnchor}
      items={mappingAddressList}
      onItemClick={handleSelectInfoAddress}
      buttonText={fmProfile.manageAddresses}
      onButtonClick={(): void => {
        setManageInfoAnchor(document.body);
      }}
      onClose={(): void => {
        setManageInfoAnchor(null);
        setAddressAnchor(null);
      }}
    />
  );

  const renderInfoPopup = (): JSX.Element | void => {
    if (addressAnchor) return AddressPopup;
  };

  const renderLocationForm = (): JSX.Element => {
    if (contactInfo?.address?.isSaved) {
      return (
        <ContactInfoView
          variant="address"
          onButtonClick={(
            evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
          ): void => setAddressAnchor(evt.currentTarget)}
          addressTitle={
            newBusiness.addrBusinessTitle || contactInfo.address.title
          }
          addressLine1={newBusiness.address || contactInfo.address.line1}
          addressLine2={
            `${newBusiness.addrCityLabel}, ${newBusiness.addrStateLabel} ${newBusiness.addrZipcodeLabel}` ||
            `${contactInfo.address.city}, ${contactInfo.address.state}`
          }
          useOtherAddressText={fmProfile.otherAddress}
        />
      );
    }

    return (
      <Box className={classes.root}>
        <Box className={classes.line}>
          <CustomTextField
            classes={{ root: classes.addressTitle }}
            ptasVariant="outline"
            label={'Address title'}
          />
          <SimpleDropDown
            items={countryOpts}
            label={'Country'}
            classes={{
              textFieldRoot: classes.countryDropdown,
              inputRoot: classes.countryDropdownInput,
            }}
            onSelected={onChangeCountry}
            value={newBusiness.countryId}
          />
        </Box>
        <CustomSearchTextField
          ptasVariant="squared outline"
          label="Address"
          onChangeDelay={500}
          hideSearchIcon={true}
          value={newBusiness.address}
          onChange={handleAddressSuggestions}
          suggestion={{
            List: addressSuggestionList,
            onSelected: onSelectAddressSuggestion,
            loading: loadingAddressSug,
          }}
          error={addrLocationError}
          helperText={addrLocationError ? generalFm.fieldRequired : ''}
          classes={{
            wrapper: classes.addressInputWrap,
          }}
        />
        <Box className={classes.line}>
          <CustomTextField
            classes={{ root: classes.addressLine }}
            ptasVariant="outline"
            label="Address (cont.)"
            value={newBusiness.addressCont}
            name="addressCont"
            onChange={handleChangeLine2}
            error={addrContLocationError}
            helperText={addrContLocationError ? generalFm.fieldRequired : ''}
            onChangeDelay={500}
          />
        </Box>
        <Box className={clsx(classes.line, classes.lastLine)}>
          <CustomSearchTextField
            ptasVariant="squared outline"
            label="City"
            onChangeDelay={500}
            hideSearchIcon={true}
            value={newBusiness.addrCityLabel}
            onChange={handleCitySuggestions}
            suggestion={{
              List: citySuggestionList,
              onSelected: onSelectCity,
              loading: loadingCitySug,
            }}
            error={cityLocationError}
            helperText={cityLocationError ? generalFm.fieldRequired : ''}
            classes={{
              wrapper: classes.cityInputWrap,
            }}
          />
          <CustomSearchTextField
            ptasVariant="squared outline"
            label="State"
            onChangeDelay={500}
            hideSearchIcon={true}
            value={newBusiness.addrStateLabel}
            onChange={handleStateSuggestions}
            suggestion={{
              List: stateSuggestionList,
              onSelected: onSelectState,
              loading: loadingStateSug,
            }}
            error={stateLocationError}
            helperText={stateLocationError ? generalFm.fieldRequired : ''}
            classes={{
              wrapper: classes.stateInput,
            }}
          />
          <CustomSearchTextField
            ptasVariant="squared outline"
            onChangeDelay={500}
            hideSearchIcon={true}
            label="Zip"
            value={newBusiness.addrZipcodeLabel}
            onChange={handleZipCodeSuggestions}
            suggestion={{
              List: zipCodeSuggestionList,
              onSelected: onSelectZipCode,
              loading: loadingZipSug,
            }}
            error={zipCodeLocationError}
            helperText={zipCodeLocationError ? generalFm.fieldRequired : ''}
            classes={{
              wrapper: classes.zipInput,
            }}
          />
        </Box>
      </Box>
    );
  };

  return (
    <div className={classes.contentWrap}>
      {renderLocationForm()}
      {renderInfoPopup()}
      <SimpleDropDown
        items={stateOrProvince}
        label={fm.stateOfIncorporation}
        value={newBusiness.stateOfIncorporationId}
        onBlur={stateOfIncorporationInputBlurHandler}
        error={stateOfIncorporationInputHasError}
        helperText={stateOfIncorporationInputHasError ? 'field required' : ''}
        classes={{
          root: classes.stateOfIncorporationRoot,
          textFieldRoot: classes.dropdownStateOfIncorporation,
        }}
        onSelected={handleChangeState}
      />
    </div>
  );
}

export default Location;
