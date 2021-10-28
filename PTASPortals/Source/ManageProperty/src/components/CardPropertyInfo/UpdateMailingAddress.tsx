// UpdateMailingAddress.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useCallback, useContext, useState } from 'react';
import {
  CustomTextField,
  SimpleDropDown,
  CustomButton,
  CustomTextButton,
  CustomSearchTextField,
  ItemSuggestion,
  DropDownItem,
  ErrorMessageContext,
  SnackContext,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core';
import * as fm from '../../routes/models/Views/Home/formatText';
import { MailingAddress, ManagePropertyContext } from 'contexts/ManageProperty';
import { debounce } from 'lodash';
import { addressService } from 'services/api/apiService/address';
import {
  AddressLookup,
  City,
  State,
  ZipCode,
} from 'services/map/model/addresses';

interface Props {
  test?: string;
}

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    width: '100%',
    maxWidth: 272,
    margin: '0 auto',
  },
  inputName: {
    marginBottom: 33,
    display: 'block',
    maxWidth: 260,
  },
  formGroup: {
    display: 'flex',
    marginBottom: 25,
  },
  addressButton: {
    marginBottom: 19,
    width: 197,
    display: 'block',
    marginLeft: 'auto',
    height: 23,
    padding: 0,
    fontSize: 14,
  },
  addressTitleInput: {
    maxWidth: 198,
    marginRight: 8,
  },
  countryDropdown: {
    minWidth: 62,
    background: theme.ptas.colors.theme.white,

    borderRadius: 3,

    '& .MuiSelect-select:focus': {
      background: theme.ptas.colors.theme.white,
    },
    '& .MuiOutlinedInput-root': {
      '& fieldset': {
        border: 'none',
        borderRadius: 3,
      },
      '&.Mui-focused fieldset': {
        borderBottom: `2px solid ${theme.ptas.colors.utility.selection}`,
      },
    },
  },
  dropdownLabel: {
    padding: '1px 3px',
    background: theme.ptas.colors.theme.white,
    left: -7,
    borderRadius: 3,
    fontSize: theme.ptas.typography.finePrint.fontSize,
  },
  inputWidth: {
    maxWidth: 270,
  },
  cityInput: {
    maxWidth: 107,
    marginRight: 8,
  },
  stateInput: {
    maxWidth: 50,
    marginRight: 8,
  },
  zipInput: {
    width: 101,
  },
  buttonWrapper: {
    width: '100%',
    maxWidth: 185,
    marginLeft: 'auto',
    marginRight: 'auto',
  },
  saveButton: {
    minWidth: 102,
    marginRight: 15,
  },
}));

function UpdateMailingAddress(props: Props): JSX.Element {
  const { setSnackState } = useContext(SnackContext);
  const { setErrorMessageState } = useContext(ErrorMessageContext);
  const {
    mailingAddress,
    setMailingAddress,
    updateTaxAccount,
    countries,
    setAddressFromParcel,
  } = useContext(ManagePropertyContext);
  const [addressSuggestions, setAddressSuggestions] = useState<AddressLookup[]>(
    []
  );
  const [addressSugLoading, setAddressSugLoading] = useState<boolean>(false);
  const [citySuggestions, setCitySuggestions] = useState<City[]>([]);
  const [citySugLoading, setCitySugLoading] = useState<boolean>(false);
  const [stateSuggestions, setStateSuggestions] = useState<State[]>([]);
  const [stateSugLoading, setStateSugLoading] = useState<boolean>(false);
  const [zipSuggestions, setZipSuggestions] = useState<ZipCode[]>([]);
  const [zipSugLoading, setZipSugLoading] = useState<boolean>(false);
  const classes = useStyles(props);
  const { name, title, address, addressCont, country, city, state, zip } =
    mailingAddress ?? {};

  const countriesOptions = (countries || []).map(c => ({
    value: c.ptas_countryid,
    label: c.ptas_abbreviation,
  }));

  const getAddressSuggestion = useCallback(
    debounce(async (valueData: string) => {
      const { data, hasError } = await addressService.getAddressSuggestions(
        valueData
      );
      setAddressSugLoading(false);
      setAddressSuggestions(data ?? []);
      if (!setMailingAddress) return;

      if (hasError) {
        setMailingAddress(prevState => {
          if (!prevState) return;
          return {
            ...prevState,
            address: '',
            addressCont: '',
          } as MailingAddress;
        });

        return;
      }

      setMailingAddress(prevState => {
        if (!prevState) return;
        return {
          ...prevState,
          address: valueData,
        } as MailingAddress;
      });
    }, 800),
    []
  );

  const getStateSuggestion = useCallback(
    debounce(async (valueData: string) => {
      const { data } = await addressService.getStateSuggestions(valueData);
      setStateSugLoading(false);
      setStateSuggestions(data ?? []);
      updateStateField(valueData ?? '');
    }, 800),
    []
  );

  const getCitySuggestion = useCallback(
    debounce(async (valueData: string) => {
      const { data } = await addressService.getCitySuggestions(valueData);
      setCitySugLoading(false);
      setCitySuggestions(data ?? []);
      updateCityField(valueData ?? '');
    }, 800),
    []
  );

  const getZipSuggestion = useCallback(
    debounce(async (valueData: string) => {
      const { data } = await addressService.getZipCodeSuggestions(valueData);
      setZipSugLoading(false);
      setZipSuggestions(data ?? []);
      updateZipField(valueData ?? '');
    }, 800),
    []
  );

  const handleSuggestionChange = (inputName: string) => (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ): void => {
    const inputValue = e.currentTarget.value;

    switch (inputName) {
      case 'address':
        setAddressSugLoading(true);
        getAddressSuggestion(inputValue);
        break;
      case 'city':
        setCitySugLoading(true);
        getCitySuggestion(inputValue);
        break;
      case 'state':
        setStateSugLoading(true);
        getStateSuggestion(inputValue);
        break;
      case 'zipCode':
        setZipSugLoading(true);
        getZipSuggestion(inputValue);
        break;
      default:
        break;
    }
  };

  const updateCityField = (value: string): void => {
    if (!setMailingAddress) return;

    setMailingAddress(prevState => {
      if (!prevState) return;

      return {
        ...prevState,
        city: {
          id: '',
          abbr: '',
          name: value ?? '',
        },
      } as MailingAddress;
    });
  };

  const updateStateField = (value: string): void => {
    if (!setMailingAddress) return;

    setMailingAddress(prevState => {
      if (!prevState) return;

      return {
        ...prevState,
        state: {
          id: '',
          abbr: value ?? '',
          name: '',
        },
      } as MailingAddress;
    });
  };

  const updateZipField = (value: string): void => {
    if (!setMailingAddress) return;

    setMailingAddress(prevState => {
      if (!prevState) return;

      return {
        ...prevState,
        zip: {
          id: '',
          abbr: '',
          name: value ?? '',
        },
      } as MailingAddress;
    });
  };

  const checkStateCityZipFields = async (): Promise<string[]> => {
    const [cityFound] =
      (await (await addressService.getCityByName(city?.name ?? '')).data) ?? [];
    const [stateFound] =
      (await (await addressService.getStateByAbbr(state?.abbr ?? '')).data) ??
      [];
    const [zipFound] =
      (await (await addressService.getZipCodeByName(zip?.name ?? '')).data) ??
      [];

    setMailingAddress &&
      setMailingAddress(prevState => {
        if (!prevState) return;
        return {
          ...prevState,
          city: {
            id: cityFound?.ptas_cityid ?? '',
            abbr: '',
            name: cityFound?.ptas_name ?? '',
          },
          state: {
            id: stateFound?.ptas_stateorprovinceid ?? '',
            abbr: stateFound?.ptas_abbreviation ?? '',
            name: stateFound?.ptas_name ?? '',
          },
          zip: {
            id: zipFound?.ptas_zipcodeid ?? '',
            abbr: '',
            name: zipFound?.ptas_name ?? '',
          },
        } as MailingAddress;
      });

    return [
      !stateFound ? 'State' : '',
      !cityFound ? 'City' : '',
      !zipFound ? 'ZipCode' : '',
    ].filter(f => f);
  };

  const handleSaveAction = async (): Promise<void> => {
    if (!updateTaxAccount) return;
    const invalidFields = await checkStateCityZipFields();
    if (invalidFields.length === 0) {
      await updateTaxAccount();
      setSnackState &&
        setSnackState({
          severity: 'success',
          text: 'Tax account updated!',
        });
    } else {
      setErrorMessageState({
        open: true,
        errorHead: `No valid fields ${invalidFields.join(',')}`,
      });
    }
  };

  const onSelectAddressSuggestion = async (
    item: ItemSuggestion
  ): Promise<void> => {
    const addressData = addressSuggestions.find(
      as => `${as.laitude}-${as.longitude}` === item.id
    );
    const [countryFound] =
      (await (await addressService.getCountryByName(addressData?.country ?? ''))
        .data) ?? [];
    setMailingAddress &&
      setMailingAddress(prevState => {
        if (!prevState) return;
        return {
          ...prevState,
          address: item.title,
          state: { name: addressData?.state },
          country: {
            id: countryFound?.ptas_countryid ?? '',
            abbr: countryFound?.ptas_abbreviation ?? '',
            name: countryFound?.ptas_name ?? '',
          },
        } as MailingAddress;
      });
  };

  const handleSelectCountry = (item: DropDownItem): void => {
    const countryFound = countries?.find(c => c.ptas_countryid === item.value);
    setMailingAddress &&
      setMailingAddress(prevState => {
        if (!prevState) return;
        return {
          ...prevState,
          country: {
            id: countryFound?.ptas_countryid ?? '',
            abbr: countryFound?.ptas_abbreviation ?? '',
            name: countryFound?.ptas_name ?? '',
          },
        };
      });
  };

  return (
    <div className={classes.root}>
      <CustomTextField
        ptasVariant="overlay"
        label="Name(s)"
        helperText={fm.nameHelperText}
        classes={{
          root: classes.inputName,
        }}
        value={name}
      />
      <CustomButton
        ptasVariant="Inverse"
        classes={{ root: classes.addressButton }}
        onClick={setAddressFromParcel}
      >
        {fm.address}
      </CustomButton>
      <div className={classes.formGroup}>
        <CustomTextField
          ptasVariant="overlay"
          label={fm.addressLabel}
          name="addressTitle"
          classes={{
            root: classes.addressTitleInput,
          }}
          value={title}
        />
        <SimpleDropDown
          items={countriesOptions}
          label={'Country'}
          value={country?.id ?? ''}
          onSelected={handleSelectCountry}
          classes={{
            textFieldRoot: classes.countryDropdown,
            animated: classes.dropdownLabel,
          }}
        />
      </div>
      <div className={classes.formGroup}>
        <CustomSearchTextField
          ptasVariant="squared outline"
          label="Address"
          value={address ?? ''}
          onChange={handleSuggestionChange('address')}
          suggestion={{
            List: addressSuggestions.map(a => {
              return {
                title: a.streetname,
                id: `${a.laitude}-${a.longitude}`,
                subtitle: a.formattedaddr,
              };
            }),
            onSelected: onSelectAddressSuggestion,
            loading: addressSugLoading,
          }}
        />
      </div>
      <div className={classes.formGroup}>
        <CustomTextField
          ptasVariant="overlay"
          label="Address (Cont.)"
          name="addressCont"
          classes={{ root: classes.inputWidth }}
          value={addressCont ?? ''}
        />
      </div>
      <div className={classes.formGroup}>
        <CustomSearchTextField
          ptasVariant="squared outline"
          label="City"
          value={city?.name ?? ''}
          onChange={handleSuggestionChange('city')}
          suggestion={{
            List: citySuggestions.map(c => {
              return {
                title: c.ptas_name,
                id: c.ptas_cityid,
                subtitle: c.ptas_name,
              };
            }),
            onSelected: (item: ItemSuggestion): void => {
              updateCityField(item.title);
            },
            loading: citySugLoading,
          }}
        />
        <CustomSearchTextField
          ptasVariant="squared outline"
          label="State"
          value={state?.abbr ?? ''}
          onChange={handleSuggestionChange('state')}
          suggestion={{
            List: stateSuggestions.map(ss => {
              return {
                title: ss.ptas_name,
                id: ss.ptas_stateorprovinceid,
                subtitle: ss.ptas_abbreviation,
              };
            }),
            onSelected: (item: ItemSuggestion): void => {
              updateStateField(item.subtitle);
            },
            loading: stateSugLoading,
          }}
        />
        <CustomSearchTextField
          ptasVariant="squared outline"
          label="Zip"
          value={zip?.name ?? ''}
          onChange={handleSuggestionChange('zipCode')}
          suggestion={{
            List: zipSuggestions.map(zs => {
              return {
                title: zs.ptas_name,
                id: zs.ptas_zipcodeid,
                subtitle: '',
              };
            }),
            onSelected: (item: ItemSuggestion): void => {
              updateZipField(item.title);
            },
            loading: zipSugLoading,
          }}
        />
      </div>
      <div className={classes.buttonWrapper}>
        <CustomButton
          classes={{ root: classes.saveButton }}
          onClick={handleSaveAction}
        >
          Save
        </CustomButton>
        <CustomTextButton ptasVariant="Text">Cancel</CustomTextButton>
      </div>
    </div>
  );
}

export default UpdateMailingAddress;
