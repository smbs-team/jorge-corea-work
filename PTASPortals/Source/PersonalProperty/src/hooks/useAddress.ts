// useAddress.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState } from 'react';
import {
  ItemSuggestion,
  useTextFieldValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import {
  AddressLookupEntity,
  CityEntity,
  CountryEntity,
  StateEntity,
  ZipCodeEntity,
} from 'models/addresses';
import {
  fetchAddressSuggestions,
  fetchCountryByName,
  fetchStateSuggestions,
  fetchStateById,
  fetchStateByName,
  fetchCitySuggestions,
  fetchCityById,
  fetchCityByName,
  fetchZipCodeSuggestions,
  fetchZipCodeById,
  fetchZipCodeByName,
} from 'utils/addresses';
import { useMount, useUpdateEffect } from 'react-use';

export interface HandleSelectAddressSuggestion {
  address: string;
  country: CountryEntity | undefined;
  city: CityEntity | undefined;
}

interface FetchDataParams<T> {
  value?: string;
  fetchByFn: (value: string) => Promise<T>;
  setStateFn: React.Dispatch<React.SetStateAction<T>>;
  ignoreSetDataUndefined?: boolean;
}

enum FetchBy {
  ID = 'ID',
  LABEL = 'LABEL',
}
type FetchByStrings = keyof typeof FetchBy;

interface AddressInputData {
  address?: {
    value: string;
  };
  state?: {
    value: string;
    valueType: FetchByStrings;
  };
  city?: {
    value: string;
    valueType: FetchByStrings;
  };
  zip?: {
    value: string;
    valueType: FetchByStrings;
  };
}

interface UseAddressProps {
  // update data to input fields
  updateInputsData: (data: AddressInputData) => void;
  // address ------------------------
  addrSuggestions: AddressLookupEntity[];
  addressSuggestionList: ItemSuggestion[];
  addressInputValue: string;
  addressDataSelected: string;
  addressIsValid: boolean;
  addressInputHasError: boolean;
  addressInputBlurHandler: () => void;
  handleAddressSuggestionSel: (item: ItemSuggestion) => Promise<void>;
  handleAddressSuggestions: (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => Promise<void>;
  getInfoFromAddressSuggestionSelected: (
    suggestionId: string
  ) => Promise<HandleSelectAddressSuggestion>;
  loadingAddressSug: boolean;
  // state ----------------------------
  stateSuggestionList: ItemSuggestion[];
  stateInputValue: string;
  stateDataSelected: StateEntity | undefined;
  stateIsValid: boolean;
  stateInputHasError: boolean;
  stateInputBlurHandler: () => void;
  handleStateSuggestionSel: (item: ItemSuggestion) => Promise<void>;
  handleStateSuggestions: (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement> | string
  ) => Promise<void>;
  getInfoFromStateSuggestionSelected: (
    suggestionId: string
  ) => Promise<StateEntity | undefined>;
  loadingStateSug: boolean;
  // city ------------------
  citySuggestionList: ItemSuggestion[];
  cityInputValue: string;
  cityDataSelected: CityEntity | undefined;
  cityIsValid: boolean;
  cityInputHasError: boolean;
  cityInputBlurHandler: () => void;
  handleCitySuggestionSel: (item: ItemSuggestion) => Promise<void>;
  handleCitySuggestions: (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => Promise<void>;
  getInfoFromCitySuggestionSelected: (
    suggestionId: string
  ) => Promise<CityEntity | undefined>;
  loadingCitySug: boolean;
  // zip ---------------
  zipCodeSuggestionList: ItemSuggestion[];
  zipInputValue: string;
  zipDataSelected: ZipCodeEntity | undefined;
  zipIsValid: boolean;
  zipInputHasError: boolean;
  zipInputBlurHandler: () => void;
  handleZipSuggestionSel: (item: ItemSuggestion) => Promise<void>;
  handleZipCodeSuggestions: (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => Promise<void>;
  getInfoFromZipSuggestionSelected: (
    suggestionId: string
  ) => Promise<ZipCodeEntity | undefined>;
  loadingZipSug: boolean;
}

const useAddress = (params?: AddressInputData): UseAddressProps => {
  const { isNotEmpty } = utilService;
  const [addrSuggestions, setAddrSuggestions] = useState<AddressLookupEntity[]>(
    []
  );
  const [loadingAddressSug, setLoadingAddressSug] = useState<boolean>(false);
  const [stateSuggestions, setStateSuggestions] = useState<StateEntity[]>([]);
  const [loadingStateSug, setLoadingStateSug] = useState<boolean>(false);
  const [citySuggestions, setCitySuggestions] = useState<CityEntity[]>([]);
  const [loadingCitySug, setLoadingCitySug] = useState<boolean>(false);
  const [zipSuggestions, setZipSuggestions] = useState<ZipCodeEntity[]>([]);
  const [loadingZipSug, setLoadingZipSug] = useState<boolean>(false);
  const [addressInputValue, setAddressInputValue] = useState<string>(
    params?.address?.value ?? ''
  );
  const [stateInputValue, setStateInputValue] = useState<string>('');
  const [cityInputValue, setCityInputValue] = useState<string>('');
  const [zipInputValue, setZipInputValue] = useState<string>('');
  const [addressDataSelected, setAddressDataSelected] = useState<string>(
    params?.address?.value ?? ''
  );
  const [stateDataSelected, setStateDataSelected] = useState<
    StateEntity | undefined
  >();
  const [cityDataSelected, setCityDataSelected] = useState<
    CityEntity | undefined
  >();
  const [zipDataSelected, setZipDataSelected] = useState<
    ZipCodeEntity | undefined
  >();

  useMount(() => {
    // set input data on init
    params && updateInputsData(params);
  });

  useUpdateEffect(() => {
    // input value updated
    setAddressInputValue(addressDataSelected ?? '');
  }, [addressDataSelected]);

  useUpdateEffect(() => {
    // input value updated
    stateDataSelected && setStateInputValue(stateDataSelected.ptas_name ?? '');
  }, [stateDataSelected]);

  useUpdateEffect(() => {
    // input value updated
    cityDataSelected && setCityInputValue(cityDataSelected.ptas_name ?? '');
  }, [cityDataSelected]);

  useUpdateEffect(() => {
    // input value updated
    zipDataSelected && setZipInputValue(zipDataSelected.ptas_name ?? '');
  }, [zipDataSelected]);

  const updateInputsData = (data: AddressInputData): void => {
    const { address, state, city, zip } = data;
    address && setAddressDataSelected(address.value);
    state &&
      fetchData<StateEntity | undefined>({
        value: state.value,
        fetchByFn:
          state.valueType === FetchBy.ID ? fetchStateById : fetchStateByName,
        setStateFn: setStateDataSelected,
      });
    city &&
      fetchData<CityEntity | undefined>({
        value: city.value,
        fetchByFn:
          city.valueType === FetchBy.ID ? fetchCityById : fetchCityByName,
        setStateFn: setCityDataSelected,
      });
    zip &&
      fetchData<ZipCodeEntity | undefined>({
        value: zip.value,
        fetchByFn:
          zip.valueType === FetchBy.ID ? fetchZipCodeById : fetchZipCodeByName,
        setStateFn: setZipDataSelected,
      });
  };

  const fetchData = async <T>(params: FetchDataParams<T>): Promise<void> => {
    const { value, fetchByFn, setStateFn, ignoreSetDataUndefined } = params;
    const dataFound = await fetchByFn(value ?? '');
    // Avoid set state if data found is undefined, if ignoreSetDataUndefined is true
    if (!dataFound && ignoreSetDataUndefined) return;
    setStateFn(dataFound);
  };

  const {
    isValid: addressIsValid,
    hasError: addressInputHasError,
    inputBlurHandler: addressInputBlurHandler,
  } = useTextFieldValidation(addressDataSelected ?? '', isNotEmpty);

  const {
    isValid: stateIsValid,
    hasError: stateInputHasError,
    inputBlurHandler: stateInputBlurHandler,
  } = useTextFieldValidation(stateDataSelected?.ptas_name ?? '', isNotEmpty);

  const {
    isValid: cityIsValid,
    hasError: cityInputHasError,
    inputBlurHandler: cityInputBlurHandler,
  } = useTextFieldValidation(cityDataSelected?.ptas_name ?? '', isNotEmpty);

  const {
    isValid: zipIsValid,
    hasError: zipInputHasError,
    inputBlurHandler: zipInputBlurHandler,
  } = useTextFieldValidation(zipDataSelected?.ptas_name ?? '', isNotEmpty);

  const handleAddressSuggestions = async (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement> | string
  ): Promise<void> => {
    const search = typeof e === 'string' ? e : e.currentTarget.value;

    setLoadingAddressSug(true);
    // reset only address field
    setAddressDataSelected(search);
    const addressEntity = await fetchAddressSuggestions(search);
    setAddrSuggestions(addressEntity);
    setLoadingAddressSug(false);
  };

  const getAddrSuggestionList = (): ItemSuggestion[] => {
    return addrSuggestions.map(addr => {
      return {
        title: addr.streetname,
        id: `${addr.laitude}-${addr.longitude}`,
        subtitle: addr.formattedaddr,
      };
    });
  };

  const handleAddressSuggestionSel = async (
    item: ItemSuggestion
  ): Promise<void> => {
    const data = await getInfoFromAddressSuggestionSelected(item.id as string);
    setAddressDataSelected(data.address);
    setCityDataSelected(data.city);
    setCityInputValue(data.city?.ptas_name ?? '');
  };

  const getInfoFromAddressSuggestionSelected = async (
    suggestionId: string
  ): Promise<HandleSelectAddressSuggestion> => {
    const addressData = addrSuggestions.find(
      as => `${as.laitude}-${as.longitude}` === suggestionId
    );

    const countryFetching = fetchCountryByName(addressData?.country ?? '');
    const cityFetching = fetchCityByName(addressData?.city ?? '');

    const [country, city] = await Promise.all([countryFetching, cityFetching]);

    return {
      address: addressData?.streetname ?? '',
      country,
      city,
    };
  };

  const handleStateSuggestions = async (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement> | string
  ): Promise<void> => {
    const search = typeof e === 'string' ? e : e.currentTarget.value;
    setLoadingStateSug(true);
    // reset
    setStateDataSelected(undefined);
    setStateInputValue(search);
    const states = await fetchStateSuggestions(search);
    setStateSuggestions(states);
    setLoadingStateSug(false);
  };

  const getStateSuggestionList = (): ItemSuggestion[] => {
    return stateSuggestions.map(addr => {
      return {
        title: addr.ptas_name,
        id: addr.ptas_stateorprovinceid,
        subtitle: addr.ptas_abbreviation,
      };
    });
  };

  const handleStateSuggestionSel = async (
    item: ItemSuggestion
  ): Promise<void> => {
    const data = await getInfoFromStateSuggestionSelected(item.id as string);
    setStateDataSelected(data);
  };

  const getInfoFromStateSuggestionSelected = async (
    suggestionId: string
  ): Promise<StateEntity | undefined> => {
    return stateSuggestions.find(
      as => as.ptas_stateorprovinceid === suggestionId
    );
  };

  const handleCitySuggestions = async (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement> | string
  ): Promise<void> => {
    const search = typeof e === 'string' ? e : e.currentTarget.value;
    setLoadingCitySug(true);
    // reset
    setCityDataSelected(undefined);
    setCityInputValue(search);
    const cities = await fetchCitySuggestions(search);
    setCitySuggestions(cities);
    setLoadingCitySug(false);
  };

  const getCitySuggestionList = (): ItemSuggestion[] => {
    return citySuggestions.map(addr => {
      return {
        title: addr.ptas_name,
        id: addr.ptas_cityid,
        subtitle: addr.ptas_name,
      };
    });
  };

  const handleCitySuggestionSel = async (
    item: ItemSuggestion
  ): Promise<void> => {
    const data = await getInfoFromCitySuggestionSelected(item.id as string);
    setCityDataSelected(data);
  };

  const getInfoFromCitySuggestionSelected = async (
    suggestionId: string
  ): Promise<CityEntity | undefined> => {
    return citySuggestions.find(c => c.ptas_cityid === suggestionId);
  };

  const handleZipCodeSuggestions = async (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement> | string
  ): Promise<void> => {
    const search = typeof e === 'string' ? e : e.currentTarget.value;
    setLoadingZipSug(true);
    // reset
    setZipDataSelected(undefined);
    setZipInputValue(search);
    const zipCodes = await fetchZipCodeSuggestions(search);
    setZipSuggestions(zipCodes);
    setLoadingZipSug(false);
  };

  const getZipCodeSuggestionList = (): ItemSuggestion[] => {
    return zipSuggestions.map(addr => {
      return {
        title: addr.ptas_name,
        id: addr.ptas_zipcodeid,
        subtitle: '',
      };
    });
  };

  const handleZipSuggestionSel = async (
    item: ItemSuggestion
  ): Promise<void> => {
    const data = await getInfoFromZipSuggestionSelected(item.id as string);
    setZipDataSelected(data);
  };

  const getInfoFromZipSuggestionSelected = async (
    suggestionId: string
  ): Promise<ZipCodeEntity | undefined> => {
    return zipSuggestions.find(z => z.ptas_zipcodeid === suggestionId);
  };

  return {
    // update data to input fields ---
    updateInputsData,
    // address ----------
    addrSuggestions,
    addressSuggestionList: getAddrSuggestionList(),
    addressInputValue,
    addressDataSelected,
    addressIsValid,
    addressInputHasError,
    addressInputBlurHandler,
    handleAddressSuggestionSel,
    handleAddressSuggestions,
    getInfoFromAddressSuggestionSelected,
    loadingAddressSug,
    // state -------------
    stateSuggestionList: getStateSuggestionList(),
    stateInputValue,
    stateDataSelected,
    stateIsValid,
    stateInputHasError,
    stateInputBlurHandler,
    handleStateSuggestionSel,
    handleStateSuggestions,
    getInfoFromStateSuggestionSelected,
    loadingStateSug,
    // city ----------------
    citySuggestionList: getCitySuggestionList(),
    cityInputValue,
    cityDataSelected,
    cityIsValid,
    cityInputHasError,
    cityInputBlurHandler,
    handleCitySuggestionSel,
    handleCitySuggestions,
    getInfoFromCitySuggestionSelected,
    loadingCitySug,
    // zip -------------
    zipCodeSuggestionList: getZipCodeSuggestionList(),
    zipInputValue,
    zipDataSelected,
    zipIsValid,
    zipInputHasError,
    zipInputBlurHandler,
    handleZipSuggestionSel,
    handleZipCodeSuggestions,
    getInfoFromZipSuggestionSelected,
    loadingZipSug,
  };
};

export default useAddress;
