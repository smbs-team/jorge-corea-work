// addresses.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import {
  AddressLookupEntity,
  CityEntity,
  CountryEntity,
  StateEntity,
  StateOrProvince,
  ZipCodeEntity,
} from 'models/addresses';
import { addressService } from 'services/api/apiService/address';

export const fetchAddressSuggestions = async (
  search: string
): Promise<AddressLookupEntity[]> => {
  const { data } = await addressService.getAddressSuggestions(search);
  return data ?? [];
};

export const fetchStateSuggestions = async (
  search: string
): Promise<StateEntity[]> => {
  const { data } = await addressService.getStateSuggestions(search);
  return data ?? [];
};

export const fetchCitySuggestions = async (
  search: string
): Promise<CityEntity[]> => {
  const { data } = await addressService.getCitySuggestions(search);
  return data ?? [];
};

export const fetchZipCodeSuggestions = async (
  search: string
): Promise<ZipCodeEntity[]> => {
  const { data } = await addressService.getZipCodeSuggestions(search);
  return data ?? [];
};

export const fetchCountries = async (): Promise<CountryEntity[]> => {
  const { data } = await addressService.getCountries();
  return data ?? [];
};

export const fetchStatesOrProvinces = async (): Promise<StateOrProvince[]> => {
  const { data } = await addressService.getStatesOrProvince();
  return data ?? [];
};

// By name or abbr
export const fetchCountryByName = async (
  name: string
): Promise<CountryEntity | undefined> => {
  const { data = [] } = await addressService.getCountryByName(name);
  return data[0];
};

export const fetchCityByName = async (
  name: string
): Promise<CityEntity | undefined> => {
  const { data = [] } = await addressService.getCityByName(name);
  return data[0];
};

export const fetchStateByAbbr = async (
  name: string
): Promise<StateEntity | undefined> => {
  const { data = [] } = await addressService.getStateByAbbr(name);
  return data[0];
};

export const fetchStateByName = async (
  name: string
): Promise<StateEntity | undefined> => {
  const { data = [] } = await addressService.getStateByName(name);
  return data[0];
};

export const fetchZipCodeByName = async (
  name: string
): Promise<ZipCodeEntity | undefined> => {
  const { data = [] } = await addressService.getZipCodeByName(name);
  return data[0];
};
// By name or abbr

// By id
export const fetchCountryById = async (
  id: string
): Promise<CountryEntity | undefined> => {
  const { data = [] } = await addressService.getCountryById(id);
  return data[0];
};
export const fetchStateById = async (
  id: string
): Promise<StateEntity | undefined> => {
  const { data } = await addressService.getStateById(id);
  return data;
};
export const fetchCityById = async (
  id: string
): Promise<CityEntity | undefined> => {
  const { data } = await addressService.getCityById(id);
  return data;
};
export const fetchZipCodeById = async (
  id: string
): Promise<ZipCodeEntity | undefined> => {
  const { data } = await addressService.getZipCodeById(id);
  return data;
};
// By id
