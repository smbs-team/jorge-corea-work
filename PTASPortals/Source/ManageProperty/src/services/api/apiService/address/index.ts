// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ApiServiceResult, handleReq } from 'services/common';
import {
  AddressLookup,
  City,
  Country,
  State,
  ZipCode,
} from 'services/map/model/addresses';
import { axiosMpInstance } from '../../axiosInstances';

class AddressService {
  /**
   * Get country list
   */
  getCountries = (): Promise<ApiServiceResult<Country[] | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_country',
          query: `$top=230`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: Country }[];
      }>(url, data);

      const { items } = res.data;

      const countriesPriority = ['USA', 'CAN', 'MEX'];
      const countriesPriFound = countriesPriority.map(c => {
        return items.find(it => it.changes.ptas_abbreviation === c);
      });

      const restCountries = items.filter(
        i => !countriesPriority.includes(i?.changes?.ptas_abbreviation)
      );

      const allCountries = [...countriesPriFound, ...restCountries].filter(
        i => i
      );

      return new ApiServiceResult<Country[]>({
        data: allCountries.map(i => i?.changes as Country),
      });
    });
  };

  /**
   * Get country by name
   */
  getCountryByName = (
    name: string
  ): Promise<ApiServiceResult<Country[] | undefined>> => {
    const searchParam = name.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_country',
          query: `$filter=ptas_name eq '${searchParam}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: Country }[];
      }>(url, data);

      return new ApiServiceResult<Country[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get country by id
   */
  getCountryById = (
    id: string
  ): Promise<ApiServiceResult<Country[] | undefined>> => {
    const searchParam = id.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_country',
          query: `$filter=ptas_countryid eq '${searchParam}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: Country }[];
      }>(url, data);

      return new ApiServiceResult<Country[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get city suggestions
   * @param name - search param
   */
  getCitySuggestions = (
    name: string
  ): Promise<ApiServiceResult<City[] | undefined>> => {
    const searchParam = name.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_city',
          query: `$filter=contains(ptas_name, '${searchParam}')&$top=8`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: City }[];
      }>(url, data);

      return new ApiServiceResult<City[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get city by name
   * @param name - search param
   */
  getCityByName = (
    name: string
  ): Promise<ApiServiceResult<City[] | undefined>> => {
    const searchParam = name.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_city',
          query: `$filter=ptas_name eq '${searchParam}'&$top=1`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: City }[];
      }>(url, data);

      return new ApiServiceResult<City[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get city by id
   * @param id - search param
   */
  getCityById = (id: string): Promise<ApiServiceResult<City[] | undefined>> => {
    const searchParam = id.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_city',
          query: `$filter=ptas_cityid eq '${searchParam}'&$top=1`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: City }[];
      }>(url, data);

      return new ApiServiceResult<City[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get state suggestions
   * @param abbr - search param
   */
  getStateSuggestions = (
    abbr: string
  ): Promise<ApiServiceResult<State[] | undefined>> => {
    const searchParam = abbr.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_stateorprovince',
          query: `$filter=contains(ptas_abbreviation, '${searchParam}')&$top=8`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: State }[];
      }>(url, data);

      return new ApiServiceResult<State[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get state by abbr
   * @param abbr - search param
   */
  getStateByAbbr = (
    abbr: string
  ): Promise<ApiServiceResult<State[] | undefined>> => {
    const searchParam = abbr.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_stateorprovince',
          query: `$filter=ptas_abbreviation eq '${searchParam}'&$top=1`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: State }[];
      }>(url, data);

      return new ApiServiceResult<State[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get state by id
   * @param id - search param
   */
  getStateById = (
    id: string
  ): Promise<ApiServiceResult<State[] | undefined>> => {
    const searchParam = id.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_stateorprovince',
          query: `$filter=ptas_stateorprovinceid eq '${searchParam}'&$top=1`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: State }[];
      }>(url, data);

      return new ApiServiceResult<State[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get zipcode suggestions
   * @param name - search param
   */
  getZipCodeSuggestions = (
    name: string
  ): Promise<ApiServiceResult<ZipCode[] | undefined>> => {
    const searchParam = name.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_zipcode',
          query: `$filter=contains(ptas_name, '${searchParam}')&$top=8`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: ZipCode }[];
      }>(url, data);

      return new ApiServiceResult<ZipCode[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get zipcode by name
   * @param name - search param
   */
  getZipCodeByName = (
    name: string
  ): Promise<ApiServiceResult<ZipCode[] | undefined>> => {
    const searchParam = name.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_zipcode',
          query: `$filter=ptas_name eq '${searchParam}'&$top=1`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: ZipCode }[];
      }>(url, data);

      return new ApiServiceResult<ZipCode[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get zipcode by id
   * @param id - search param
   */
  getZipCodeById = (
    id: string
  ): Promise<ApiServiceResult<ZipCode[] | undefined>> => {
    const searchParam = id.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_zipcode',
          query: `$filter=ptas_zipcodeid eq '${searchParam}'&$top=1`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: ZipCode }[];
      }>(url, data);

      return new ApiServiceResult<ZipCode[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get addresses.
   * @param id - Partial parcel id, or account or address.
   */
  getAddressSuggestions = (
    search: string
  ): Promise<ApiServiceResult<AddressLookup[] | undefined>> =>
    handleReq(async () => {
      const searchWithoutSpace = search.trim();

      const url = `/addresslookup`;

      const parcel = await axiosMpInstance.post<AddressLookup[]>(url, {
        searchvalue: searchWithoutSpace,
      });

      return new ApiServiceResult<AddressLookup[]>({
        data: parcel.data,
      });
    });
}

export const addressService = new AddressService();
