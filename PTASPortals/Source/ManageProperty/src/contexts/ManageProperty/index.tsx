// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { TaxAccount } from 'models/taxAccount';
import React, {
  createContext,
  Dispatch,
  FC,
  PropsWithChildren,
  SetStateAction,
  useState,
} from 'react';
import { ItemSuggestion } from '@ptas/react-public-ui-library';
import { useUpdateEffect } from 'react-use';
import { Task } from 'routes/models/Views/HomeDestroyedProperty/Task';
import { HomeImprovementApplication } from 'routes/models/Views/HomeImprovement/model/homeImprovementApplication';
import { apiService } from 'services/api/apiService';
import { addressService } from 'services/api/apiService/address';
import { taxAccountService } from 'services/api/apiService/taxaccount';
import { Country } from 'services/map/model/addresses';
import { PropertyDescription } from 'services/map/model/parcel';
import { TaxAccountAddress } from 'services/map/model/taxAccount';
import { CurrentUseApplication } from 'routes/models/Views/CurrentUse/models/currentUse';

interface OptionItem {
  id: string;
  name: string;
  abbr?: string;
}

export interface MailingAddress {
  parcelDetailId: string;
  taxAccountId?: string;
  name?: string;
  title?: string;
  country?: OptionItem;
  address?: string;
  addressCont: string;
  city?: OptionItem;
  state?: OptionItem;
  zip?: OptionItem;
}

export interface Props {
  mailingAddress: MailingAddress | undefined;
  setMailingAddress: Dispatch<SetStateAction<MailingAddress | undefined>>;
  parcelDetails: PropertyDescription[];
  setParcelDetails: Dispatch<SetStateAction<PropertyDescription[]>>;
  selectedParcels: string;
  setSelectedParcels: Dispatch<SetStateAction<string>>;
  updateTaxAccount: () => void;
  countries: Country[];
  fetchCountries: () => void;
  setAddressFromParcel: () => void;
  getOptionSetMapping: (
    entityName: string,
    fieldName: string
  ) => Promise<Map<string, number>>;
  taxAccountByParcelSel: TaxAccount | undefined;
  destroyedPropertyApp: Task;
  setDestroyedPropertyApp: Dispatch<SetStateAction<Task>>;
  currentUseApp: CurrentUseApplication | undefined;
  setCurrentUseApp: Dispatch<SetStateAction<CurrentUseApplication | undefined>>;
  stateCodeDestroyProperty: Map<string, number>;
  setStateCodeDestroyedProperty: Dispatch<SetStateAction<Map<string, number>>>;
  statusCodeDestroyedProperty: Map<string, number>;
  setStatusCodeDestroyProperty: Dispatch<SetStateAction<Map<string, number>>>;
  homeImprovementApp: HomeImprovementApplication;
  setHomeImprovementApp: Dispatch<SetStateAction<HomeImprovementApplication>>;
  stateCodeHI: Map<string, number>;
  setStateCodeHI: Dispatch<SetStateAction<Map<string, number>>>;
  statusCodeHI: Map<string, number>;
  setStatusCodeHI: Dispatch<SetStateAction<Map<string, number>>>;
  currentUseAppState: Map<string, number>;
  setCurrentUseAppState: Dispatch<SetStateAction<Map<string, number>>>;
  parcelSuggestions: ItemSuggestion[];
  setParcelSuggestions: Dispatch<React.SetStateAction<ItemSuggestion[]>>;
  valueParcelSearch: string;
  setValueParcelSearch: Dispatch<SetStateAction<string>>;
  loadingParcel: boolean;
  setLoadingParcel: Dispatch<SetStateAction<boolean>>;
  parcelItemSelected: PropertyDescription[];
  setParcelItemSelected: Dispatch<SetStateAction<PropertyDescription[]>>;
  propertyInfoTabsValue: number;
  setPropertyInfoTabsValue: Dispatch<SetStateAction<number>>;
  deletePopoverAnchor: HTMLElement | null;
  setDeletePopoverAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  parcelInfoSelected: PropertyDescription | undefined;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const ManagePropertyContext = createContext<Props>(null as any);

function ManagePropertyProvider(props: PropsWithChildren<{}>): JSX.Element {
  const [selectedParcels, setSelectedParcels] = useState<string>('');
  const [parcelDetails, setParcelDetails] = useState<PropertyDescription[]>([]);
  const [mailingAddress, setMailingAddress] = useState<
    MailingAddress | undefined
  >();
  const [mailingAddresses, setMailingAddresses] = useState<MailingAddress[]>(
    []
  );
  const [countries, setCountries] = useState<Country[]>([]);
  const [taxAccountByParcelSel, setTaxAccountByParcelSel] = useState<
    TaxAccount | undefined
  >();
  // tab more state
  const [destroyedPropertyApp, setDestroyedPropertyApp] = useState(new Task());
  const [stateCodeDestroyProperty, setStateCodeDestroyedProperty] = useState<
    Map<string, number>
  >(new Map());
  const [statusCodeDestroyedProperty, setStatusCodeDestroyProperty] = useState<
    Map<string, number>
  >(new Map());
  const [homeImprovementApp, setHomeImprovementApp] = useState(
    new HomeImprovementApplication()
  );
  const [stateCodeHI, setStateCodeHI] = useState<Map<string, number>>(
    new Map()
  );
  const [statusCodeHI, setStatusCodeHI] = useState<Map<string, number>>(
    new Map()
  );
  const [currentUseAppState, setCurrentUseAppState] = useState<
    Map<string, number>
  >(new Map());
  const [parcelSuggestions, setParcelSuggestions] = useState<ItemSuggestion[]>(
    []
  );
  const [valueParcelSearch, setValueParcelSearch] = useState<string>('');
  const [loadingParcel, setLoadingParcel] = useState<boolean>(true);
  const [parcelItemSelected, setParcelItemSelected] = useState<
    PropertyDescription[]
  >([]);
  const [propertyInfoTabsValue, setPropertyInfoTabsValue] = useState<number>(
    -1
  );
  const [currentUseApp, setCurrentUseApp] = useState<
    CurrentUseApplication | undefined
  >();
  const [
    deletePopoverAnchor,
    setDeletePopoverAnchor,
  ] = useState<HTMLElement | null>(null);

  useUpdateEffect(() => {
    fetchMailingAddress();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [parcelDetails]);

  useUpdateEffect(() => {
    if (selectedParcels) initMailingAddressData();
  }, [selectedParcels, mailingAddresses]);

  useUpdateEffect(() => {
    if (selectedParcels) fetchTaxAccountFromParcelSelected();
  }, [selectedParcels]);

  const getParcelInfoSelected = (): PropertyDescription | undefined => {
    return parcelDetails.find(pd => pd.ptas_parceldetailid === selectedParcels);
  };

  const findTaxAccountIdFromParcelSelected = (): string | undefined => {
    return getParcelInfoSelected()?._ptas_taxaccountid_value;
  };

  const fetchTaxAccountFromParcelSelected = async (): Promise<void> => {
    const taxAccId = findTaxAccountIdFromParcelSelected();
    if (!taxAccId) return;
    const { data } = await taxAccountService.getTaxAccount(taxAccId);
    setTaxAccountByParcelSel(data);
  };

  const initMailingAddressData = (): void => {
    const taxAccId = findTaxAccountIdFromParcelSelected();
    if (taxAccId) {
      const mailingAddressFound = mailingAddresses.find(
        ma => ma.taxAccountId === taxAccId
      );
      setMailingAddress(mailingAddressFound);
    } else {
      setMailingAddress(undefined);
    }
  };

  const fetchCountries = async (): Promise<void> => {
    const { data } = await addressService.getCountries();
    setCountries(data ?? []);
  };

  const fetchMailingAddress = async (): Promise<void> => {
    const taxAccountIds = parcelDetails
      .filter(pd => pd._ptas_taxaccountid_value)
      .map(pd => pd._ptas_taxaccountid_value);
    if (taxAccountIds.length === 0) return;
    const { data } = await taxAccountService.getTaxAccountAddresses(
      taxAccountIds as string[]
    );
    if (!data) return;
    const newMailingAddresses = data.map(
      (d): MailingAddress => {
        const beforeAddress = d.ptas_addr1_street_intl_address ?? '';
        const commaIndex = beforeAddress.indexOf(',');
        const address =
          commaIndex >= 0
            ? beforeAddress.substring(0, commaIndex)
            : beforeAddress;
        const addressCont =
          commaIndex >= 0 ? beforeAddress.substring(commaIndex + 1) : '';
        const city = {
          id: d.ptas_addr1_cityid?.ptas_cityid ?? '',
          name: d.ptas_addr1_cityid?.ptas_name ?? '',
        };
        const country = {
          id: d.ptas_addr1_countryid?.ptas_countryid ?? '',
          abbr: d.ptas_addr1_countryid?.ptas_abbreviation,
          name: d.ptas_addr1_countryid?.ptas_name ?? '',
        };

        const state = {
          id: d.ptas_addr1_stateid?.ptas_stateorprovinceid ?? '',
          abbr: d.ptas_addr1_stateid?.ptas_abbreviation,
          name: d.ptas_addr1_stateid?.ptas_name ?? '',
        };

        const zip = {
          id: d.ptas_addr1_zipcodeid?.ptas_zipcodeid ?? '',
          name: d.ptas_addr1_zipcodeid?.ptas_name ?? '',
        };

        return {
          parcelDetailId: selectedParcels,
          taxAccountId: d.ptas_taxaccountid,
          name: d.ptas_taxpayername,
          title: '', // (LF) TODO: It will apply later
          address,
          addressCont,
          city,
          country,
          state,
          zip,
        };
      }
    );
    setMailingAddresses(newMailingAddresses);
  };

  const setAddressFromParcel = async (): Promise<void> => {
    const parcelDetFound = parcelDetails.find(
      pd => pd.ptas_parceldetailid === selectedParcels
    );
    if (!parcelDetFound) return;
    const [cityFound] =
      (await (
        await addressService.getCityById(
          parcelDetFound._ptas_addr1_cityid_value ?? ''
        )
      ).data) ?? [];
    const [stateFound] =
      (await (
        await addressService.getStateById(
          parcelDetFound._ptas_addr1_stateid_value ?? ''
        )
      ).data) ?? [];
    const [zipFound] =
      (await (
        await addressService.getZipCodeById(
          parcelDetFound._ptas_addr1_zipcodeid_value ?? ''
        )
      ).data) ?? [];
    const [countryFound] =
      (await (
        await addressService.getCountryById(
          parcelDetFound._ptas_addr1_countryid_value ?? ''
        )
      ).data) ?? [];
    const beforeAddress = parcelDetFound.ptas_address ?? '';
    const commaIndex = beforeAddress.indexOf(',');
    const address =
      commaIndex >= 0 ? beforeAddress.substring(0, commaIndex) : beforeAddress;
    const addressCont =
      commaIndex >= 0 ? beforeAddress.substring(commaIndex + 1) : '';
    const city = {
      id: cityFound?.ptas_cityid ?? '',
      name: cityFound?.ptas_name ?? '',
    };
    const country = {
      id: countryFound?.ptas_countryid ?? '',
      abbr: countryFound?.ptas_abbreviation,
      name: countryFound?.ptas_name ?? '',
    };

    const state = {
      id: stateFound?.ptas_stateorprovinceid ?? '',
      abbr: stateFound?.ptas_abbreviation,
      name: stateFound?.ptas_name ?? '',
    };

    const zip = {
      id: zipFound?.ptas_zipcodeid ?? '',
      name: zipFound?.ptas_name ?? '',
    };
    setMailingAddress({
      parcelDetailId: selectedParcels,
      taxAccountId: parcelDetFound._ptas_taxaccountid_value ?? '',
      address,
      addressCont,
      name: parcelDetFound.ptas_namesonaccount,
      city,
      country,
      state,
      zip,
    });
  };

  const updateTaxAccount = async (): Promise<unknown> => {
    const {
      taxAccountId,
      name,
      address,
      addressCont,
      country,
      city,
      state,
      zip,
    } = mailingAddress as MailingAddress;

    const taxAccountUpdated: TaxAccountAddress = {
      /* eslint-disable @typescript-eslint/camelcase */
      _ptas_addr1_cityid_value: city?.id ?? '',
      _ptas_addr1_countryid_value: country?.id ?? '',
      _ptas_addr1_stateid_value: state?.id ?? '',
      _ptas_addr1_zipcodeid_value: zip?.id ?? '',
      ptas_addr1_city: city?.name ?? '',
      ptas_addr1_street_intl_address: `${address}${
        addressCont ? `, ${addressCont}` : ''
      }`,
      ptas_isnonusaddress: country?.abbr !== 'USA',
      ptas_taxpayername: name ?? '',
      ptas_taxaccountid: taxAccountId ?? '',
      /* eslint-enable @typescript-eslint/camelcase */
    };
    const { data } = await taxAccountService.updateTaxAccountAddress(
      taxAccountUpdated
    );
    return data;
  };

  // TODO: gl move to useManagerProperty hook
  const getOptionSetMapping = async (
    entityName: string,
    fieldName: string
  ): Promise<Map<string, number>> => {
    const { hasError, data } = await apiService.getOptionSet(
      entityName,
      fieldName
    );

    if (hasError || !data) {
      console.log('Error getting option set mapping');
      return new Map<string, number>();
    }

    const optionSetMap = new Map<string, number>(
      data.map(code => [code.value, code.attributeValue])
    );

    return optionSetMap;
  };

  return (
    <ManagePropertyContext.Provider
      value={{
        mailingAddress,
        setMailingAddress,
        parcelDetails,
        setParcelDetails,
        selectedParcels,
        setSelectedParcels,
        updateTaxAccount,
        countries,
        fetchCountries,
        setAddressFromParcel,
        getOptionSetMapping,
        taxAccountByParcelSel,
        destroyedPropertyApp,
        setDestroyedPropertyApp,
        stateCodeDestroyProperty,
        setStateCodeDestroyedProperty,
        setStatusCodeDestroyProperty,
        statusCodeDestroyedProperty,
        homeImprovementApp,
        setHomeImprovementApp,
        stateCodeHI,
        setStateCodeHI,
        statusCodeHI,
        setStatusCodeHI,
        loadingParcel,
        setLoadingParcel,
        parcelItemSelected,
        setParcelItemSelected,
        parcelSuggestions,
        setParcelSuggestions,
        valueParcelSearch,
        setValueParcelSearch,
        propertyInfoTabsValue,
        setPropertyInfoTabsValue,
        deletePopoverAnchor,
        setDeletePopoverAnchor,
        parcelInfoSelected: getParcelInfoSelected(),
        currentUseApp,
        setCurrentUseApp,
        currentUseAppState,
        setCurrentUseAppState,
      }}
    >
      {props.children}
    </ManagePropertyContext.Provider>
  );
}

export const withManagePropertyProvider = (Component: FC) => (
  props: object
): JSX.Element => {
  return (
    <ManagePropertyProvider>
      <Component {...props} />
    </ManagePropertyProvider>
  );
};
