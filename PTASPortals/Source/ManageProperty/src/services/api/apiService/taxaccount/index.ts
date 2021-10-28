// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { TaxAccount, TaxAccountEntityFields } from 'models/taxAccount';
import { ApiServiceResult, handleReq } from 'services/common';
import { TaxAccountAddress } from 'services/map/model/taxAccount';
import { axiosMpInstance } from '../../axiosInstances';

class TaxAccountService {
  /**
   * Get tax account
   * @param id - tax account id
   */
  getTaxAccount = (
    id: string
  ): Promise<ApiServiceResult<TaxAccount | undefined>> => {
    const data = {
      requests: [
        {
          entityId: id,
          entityName: 'ptas_taxaccount',
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: TaxAccountEntityFields }[];
      }>(url, data);

      let dataResp = undefined;
      if (res.data?.items && res.data.items[0]) {
        dataResp = new TaxAccount(res.data.items[0].changes);
      }

      return new ApiServiceResult<TaxAccount | undefined>({
        data: dataResp,
      });
    });
  };

  /**
   * update taxAccount paperless
   * @param taxAccount - tax account model
   */
  updateTaxAccountPaperless = (
    taxAccount: TaxAccount
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_taxaccount',
          entityId: taxAccount.id,
          changes: {
            /* eslint-disable @typescript-eslint/camelcase */
            ptas_email: taxAccount.email,
            ptas_paperless: taxAccount.paperless,
            /* eslint-enable @typescript-eslint/camelcase */
          },
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosMpInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization:
              'Bearer ' + process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  /**
   * Get tax account address
   * @param ids - tax account ids
   */
  getTaxAccountAddresses = (
    ids: string[]
  ): Promise<ApiServiceResult<TaxAccountAddress[] | undefined>> => {
    const data = {
      requests: ids.map(id => ({
        entityName: 'ptas_taxaccount',
        query: `$filter=ptas_taxaccountid eq '${id}'
        &$select=ptas_taxaccountid,ptas_isnonusaddress,ptas_addr1_street_intl_address,ptas_addr1_city,_ptas_addr1_stateid_value,
        _ptas_addr1_zipcodeid_value,_ptas_addr1_countryid_value,_ptas_addr1_cityid_value
        &$expand=ptas_addr1_countryid,ptas_addr1_stateid,ptas_addr1_zipcodeid,ptas_addr1_cityid`,
      })),
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosMpInstance.post<{
        items: { changes: TaxAccountAddress }[];
      }>(url, data);

      return new ApiServiceResult<TaxAccountAddress[]>({
        data: res.data.items.map(i => i.changes),
      });
    });
  };

  /**
   * Creates a file attachment metadata entity
   * @param taxAccountAddress - address
   */
  updateTaxAccountAddress = (
    taxAccountAddress: TaxAccountAddress
  ): Promise<ApiServiceResult<unknown>> => {
    const {
      // eslint-disable-next-line @typescript-eslint/camelcase
      _ptas_addr1_cityid_value,
      // eslint-disable-next-line @typescript-eslint/camelcase
      _ptas_addr1_countryid_value,
      // eslint-disable-next-line @typescript-eslint/camelcase
      _ptas_addr1_stateid_value,
      // eslint-disable-next-line @typescript-eslint/camelcase
      _ptas_addr1_zipcodeid_value,
      // eslint-disable-next-line @typescript-eslint/camelcase
      ptas_addr1_city,
      // eslint-disable-next-line @typescript-eslint/camelcase
      ptas_addr1_street_intl_address,
      // eslint-disable-next-line @typescript-eslint/camelcase
      ptas_isnonusaddress,
      // eslint-disable-next-line @typescript-eslint/camelcase
      ptas_taxpayername,
    } = taxAccountAddress;
    const data = {
      items: [
        {
          entityName: 'ptas_taxaccount',
          entityId: taxAccountAddress.ptas_taxaccountid,
          changes: {
            // eslint-disable-next-line @typescript-eslint/camelcase
            _ptas_addr1_cityid_value,
            // eslint-disable-next-line @typescript-eslint/camelcase
            _ptas_addr1_countryid_value,
            // eslint-disable-next-line @typescript-eslint/camelcase
            _ptas_addr1_stateid_value,
            // eslint-disable-next-line @typescript-eslint/camelcase
            _ptas_addr1_zipcodeid_value,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_addr1_city,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_addr1_street_intl_address,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_isnonusaddress,
            // eslint-disable-next-line @typescript-eslint/camelcase
            ptas_taxpayername,
          },
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosMpInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization:
              'Bearer ' + process.env.REACT_APP_HI_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };
}

export const taxAccountService = new TaxAccountService();
