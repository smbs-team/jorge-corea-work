// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ApiServiceResult, handleReq } from 'services/common';
import {
  CityEntity,
  CountryEntity,
  StateEntity,
  ZipCodeEntity,
  AddressLookupEntity,
  StateOrProvince,
} from 'models/addresses';
import { axiosPerPropInstance } from '../../axiosInstances';

class AddressService {
  /**
   * Get country list
   */
  getCountries = (): Promise<ApiServiceResult<CountryEntity[] | undefined>> => {
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

      const res = await axiosPerPropInstance.post<{
        items: { changes: CountryEntity }[];
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

      return new ApiServiceResult<CountryEntity[]>({
        data: allCountries.map(i => i?.changes as CountryEntity),
      });
    });
  };

  /**
   * Get province list
   */
  getStatesOrProvince = (): Promise<
    ApiServiceResult<StateOrProvince[] | undefined>
  > => {
    const data = {
      requests: [
        {
          entityName: 'ptas_stateorprovince',
          query: `$select=ptas_name,ptas_abbreviation,ptas_stateorprovinceid`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosPerPropInstance.post<{
        items: { changes: StateOrProvince }[];
      }>(url, data);

      return new ApiServiceResult<StateOrProvince[]>({
        data: res.data.items.map(pro => pro.changes) ?? [],
      });
    });
  };

  /**
   * Get country by name
   */
  getCountryByName = (
    name: string
  ): Promise<ApiServiceResult<CountryEntity[] | undefined>> => {
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

      const res = await axiosPerPropInstance.post<{
        items: { changes: CountryEntity }[];
      }>(url, data);

      return new ApiServiceResult<CountryEntity[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get country by id
   */
  getCountryById = (
    id: string
  ): Promise<ApiServiceResult<CountryEntity[] | undefined>> => {
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

      const res = await axiosPerPropInstance.post<{
        items: { changes: CountryEntity }[];
      }>(url, data);

      return new ApiServiceResult<CountryEntity[]>({
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
  ): Promise<ApiServiceResult<CityEntity[] | undefined>> => {
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

      const res = await axiosPerPropInstance.post<{
        items: { changes: CityEntity }[];
      }>(url, data);

      return new ApiServiceResult<CityEntity[]>({
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
  ): Promise<ApiServiceResult<CityEntity[] | undefined>> => {
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

      const res = await axiosPerPropInstance.post<{
        items: { changes: CityEntity }[];
      }>(url, data);

      return new ApiServiceResult<CityEntity[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get city by id
   * @param id - search param
   */
  getCityById = (
    id: string
  ): Promise<ApiServiceResult<CityEntity | undefined>> => {
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

      const res = await axiosPerPropInstance.post<{
        items: { changes: CityEntity }[];
      }>(url, data);

      return new ApiServiceResult<CityEntity>({
        data: res.data.items[0].changes,
      });
    });
  };

  /**
   * Get state suggestions
   * @param abbr - search param
   */
  getStateSuggestions = (
    abbr: string
  ): Promise<ApiServiceResult<StateEntity[] | undefined>> => {
    const searchParam = abbr.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_stateorprovince',
          query: `$filter=contains(ptas_name, '${searchParam}')&$top=8`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosPerPropInstance.post<{
        items: { changes: StateEntity }[];
      }>(url, data);

      return new ApiServiceResult<StateEntity[]>({
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
  ): Promise<ApiServiceResult<StateEntity[] | undefined>> => {
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

      const res = await axiosPerPropInstance.post<{
        items: { changes: StateEntity }[];
      }>(url, data);

      return new ApiServiceResult<StateEntity[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Get state by abbr
   * @param abbr - search param
   */
  getStateByName = (
    abbr: string
  ): Promise<ApiServiceResult<StateEntity[] | undefined>> => {
    const searchParam = abbr.trim();
    const data = {
      requests: [
        {
          entityName: 'ptas_stateorprovince',
          query: `$filter=ptas_name eq '${searchParam}'&$top=1`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosPerPropInstance.post<{
        items: { changes: StateEntity }[];
      }>(url, data);

      return new ApiServiceResult<StateEntity[]>({
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
  ): Promise<ApiServiceResult<StateEntity | undefined>> => {
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

      const res = await axiosPerPropInstance.post<{
        items: { changes: StateEntity }[];
      }>(url, data);

      return new ApiServiceResult<StateEntity>({
        data: res.data.items[0].changes,
      });
    });
  };

  /**
   * Get zipcode suggestions
   * @param name - search param
   */
  getZipCodeSuggestions = (
    name: string
  ): Promise<ApiServiceResult<ZipCodeEntity[] | undefined>> => {
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

      const res = await axiosPerPropInstance.post<{
        items: { changes: ZipCodeEntity }[];
      }>(url, data);

      return new ApiServiceResult<ZipCodeEntity[]>({
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
  ): Promise<ApiServiceResult<ZipCodeEntity[] | undefined>> => {
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

      const res = await axiosPerPropInstance.post<{
        items: { changes: ZipCodeEntity }[];
      }>(url, data);

      return new ApiServiceResult<ZipCodeEntity[]>({
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
  ): Promise<ApiServiceResult<ZipCodeEntity | undefined>> => {
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

      const res = await axiosPerPropInstance.post<{
        items: { changes: ZipCodeEntity }[];
      }>(url, data);

      return new ApiServiceResult<ZipCodeEntity>({
        data: res.data.items[0].changes,
      });
    });
  };

  /**
   * Get addresses.
   * @param id - Partial parcel id, or account or address.
   */
  getAddressSuggestions = (
    search: string
  ): Promise<ApiServiceResult<AddressLookupEntity[] | undefined>> =>
    handleReq(async () => {
      const searchWithoutSpace = search.trim();

      const url = `/addresslookup`;

      const parcel = await axiosPerPropInstance.post<AddressLookupEntity[]>(
        url,
        {
          searchvalue: searchWithoutSpace,
        }
      );

      return new ApiServiceResult<AddressLookupEntity[]>({
        data: parcel.data,
      });
    });
}

export const addressService = new AddressService();
