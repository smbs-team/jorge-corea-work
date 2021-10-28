// useUpdateBusiness.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useContext } from 'react';
import { UpdateBusinessContext } from 'contexts/UpdateBusiness';
import { UseUpdateBusiness } from 'contexts/UpdateBusiness/types';
import { useLocation } from 'react-router-dom';
import { businessApiService } from 'services/api/apiService/business';
import { PersonalProperty } from 'models/personalProperty';
import { apiService } from 'services/api/apiService';
import {
  DropDownItem,
  // ItemSuggestion,

  // CustomFile,
} from '@ptas/react-public-ui-library';
import { addressService } from 'services/api/apiService/address';

interface UpdateBusinessType extends UseUpdateBusiness {
  handleViewChangesClose: () => void;
  handleSelectedTab: (tab: number) => void;
  saveUpdateBusiness: (
    key: keyof PersonalProperty,
    value: string | number
  ) => void;
  loadBusiness: () => Promise<void>;
  loadPropertyType: () => Promise<void>;
  loadStateOrProvince: () => Promise<void>;
  loadCountryOpts: () => Promise<void>;
  loadPhoneTypes: () => Promise<void>;
  loadYearsAcquired: () => Promise<void>;
  loadChangeReason: () => Promise<void>;
  loadAssets: (id: string) => Promise<void>;
  loadCategoryAssets: () => Promise<void>;
  loadImprovementTypes: () => Promise<void>;
}

interface BusinessParams {
  businessId: string;
}

const useUpdateBusiness = (): UpdateBusinessType => {
  const context = useContext(UpdateBusinessContext);
  const {
    setCurrentStepIndex,
    setViewChangesOpen,
    setPropertyType,
    setInitialBusiness,
    setUpdatedBusiness,
    setStateOrProvince,
    setCountryItems,
    setPhoneTypeList,
    setPhoneTypes,
    setYearsAcquired,
    setChangeReason,
    setAssetsCategoryList,
    setAllAssetsCategoryList,
    setInitialAssets,
    setAssetsList,
    setImprovementTypeList,
  } = context;

  const { state } = useLocation<BusinessParams>();

  const saveUpdateBusiness = (
    key: keyof PersonalProperty,
    value: string | number
  ): void => {
    setUpdatedBusiness((prev) => {
      return {
        ...prev,
        [key]: value,
      };
    });
  };

  const loadBusiness = async (): Promise<void> => {
    if (!state.businessId) return;

    const { data } = await businessApiService.getBusinessById(state.businessId);

    if (!data) return;

    let business = data;

    // location info
    const city = addressService.getCityById(business.cityId);

    const zip = addressService.getZipCodeById(business.zipId);

    const stateAddr = addressService.getStateById(business.stateId);

    // contact info (tab contact)
    // contact information is stored in the preparer fields
    const cityPreparer = addressService.getCityById(business.preparerCityId);

    const zipPreparer = addressService.getZipCodeById(
      business.preparerZipCodeId
    );

    const statePreparer = addressService.getStateById(business.preparerStateId);

    // get assets
    loadAssets(business.id);

    const result = await Promise.all([
      city,
      zip,
      stateAddr,
      cityPreparer,
      zipPreparer,
      statePreparer,
    ]);

    const [
      { data: cityInfo },
      { data: zipInfo },
      { data: stateInfo },
      { data: cityPreparerInfo },
      { data: zipPreparerInfo },
      { data: statePreparerInfo },
    ] = result;

    business = {
      ...business,
      addrCityLabel: cityInfo?.ptas_name || '',
      addrZipcodeLabel: zipInfo?.ptas_name || '',
      addrStateLabel: stateInfo?.ptas_name || '',
      preparerCityLabel: cityPreparerInfo?.ptas_name ?? '',
      preparerZipcodeLabel: zipPreparerInfo?.ptas_name ?? '',
      preparerStateLabel: statePreparerInfo?.ptas_name ?? '',
    };

    // set an initial value of new business to compare the old fields with the new ones, in order to know which fields have been updated.
    setInitialBusiness(business);
    // this status stores the personal property data with updated fields
    setUpdatedBusiness(business);
  };

  const loadPhoneTypes = async (): Promise<void> => {
    const { data: _phoneTypes } = await apiService.getOptionSet(
      'ptas_phonenumber',
      'ptas_phonetype'
    );
    if (_phoneTypes) {
      setPhoneTypeList(
        _phoneTypes.map((el) => ({ value: el.attributeValue, label: el.value }))
      );
      const map = new Map<string, number>(
        _phoneTypes?.map((c) => [c.value, c.attributeValue])
      );
      setPhoneTypes(map);
    }
  };

  const loadPropertyType = async (): Promise<void> => {
    const { hasError, data: propType } = await apiService.getOptionSet(
      'ptas_personalproperty',
      'ptas_propertytype'
    );

    if (hasError) {
      console.log('error to get property type');

      return setPropertyType([]);
    }

    const propTypeMapping: DropDownItem[] =
      propType?.map((pt) => {
        return {
          label: pt.value,
          value: `${pt.attributeValue}`,
        };
      }) ?? [];

    setPropertyType(propTypeMapping);
  };

  const loadStateOrProvince = async (): Promise<void> => {
    const { data, hasError } = await addressService.getStatesOrProvince();

    if (hasError) {
      console.log('Error to get states or provinces');

      return setStateOrProvince([]);
    }

    const countriesOptions = (data || []).map((c) => ({
      value: c.ptas_stateorprovinceid,
      label: c.ptas_name, // c.ptas_abbreviation
    }));

    setStateOrProvince(countriesOptions);
  };

  const handleViewChangesClose = (): void => {
    setViewChangesOpen(false);
  };

  const handleSelectedTab = (tab: number): void => {
    setCurrentStepIndex(tab);
  };

  const loadCountryOpts = async (): Promise<void> => {
    const { data, hasError } = await addressService.getCountries();

    if (hasError) {
      console.log('Error to get countries');

      return setCountryItems([]);
    }

    const countriesOptions = (data || []).map((c) => ({
      value: c.ptas_countryid,
      label: c.ptas_name, // c.ptas_abbreviation
    }));

    setCountryItems(countriesOptions);
  };

  const loadYearsAcquired = async (): Promise<void> => {
    const { data } = await businessApiService.getYears();

    const yearsOptions = (data || []).map((y) => ({
      value: y.ptas_yearid,
      label: y.ptas_name,
    }));

    setYearsAcquired(yearsOptions);
  };

  const loadChangeReason = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_personalpropertyasset',
      'ptas_changereason'
    );

    const changeReason = (data || []).map((c) => ({
      value: c.attributeValue,
      label: c.value,
    }));

    setChangeReason(changeReason);
  };

  const loadAssets = async (id: string): Promise<void> => {
    const { data } = await businessApiService.getAssetsByBusiness(id);

    setInitialAssets(data ?? []);
    setAssetsList(data ?? []);
  };

  const loadCategoryAssets = async (): Promise<void> => {
    const { data } = await businessApiService.filterAssetCategory('', 'All');

    if (data) {
      setAssetsCategoryList(data);
      setAllAssetsCategoryList(data);
    }
  };

  const loadImprovementTypes = async (): Promise<void> => {
    const { data } = await businessApiService.getImprovementType();

    setImprovementTypeList(data ?? []);
  };

  return {
    handleSelectedTab,
    handleViewChangesClose,
    saveUpdateBusiness,
    loadBusiness,
    loadPropertyType,
    loadStateOrProvince,
    loadCountryOpts,
    loadPhoneTypes,
    loadYearsAcquired,
    loadChangeReason,
    loadAssets,
    loadCategoryAssets,
    loadImprovementTypes,
    ...context,
  };
};

export default useUpdateBusiness;
