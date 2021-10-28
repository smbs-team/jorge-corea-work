// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { ApiServiceResult, handleReq } from 'services/common/httpClient';
import { axiosHiInstance } from '../../axiosInstances';
import {
  HomeImprovementApplication,
  HomeImprovementApplicationEntity,
} from 'routes/models/Views/HomeImprovement/model/homeImprovementApplication';
import { utilService } from '@ptas/react-public-ui-library';

class HiApiService {
  /**
   * Gets home improvement applications
   * @param parcelId - Parcel ID
   */
  getHomeImprovementAppByParcel = (
    parcelId: string
  ): Promise<ApiServiceResult<HomeImprovementApplication[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_homeimprovement',
          query: `$filter=_ptas_parcelid_value eq '${parcelId}'
            &$expand=ptas_exemptionbeginyearid($select=ptas_name)`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosHiInstance.post<{
          items: { changes: HomeImprovementApplicationEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<HomeImprovementApplication[]>({
        data: res.items?.length
          ? res.items.map((i) => new HomeImprovementApplication(i.changes))
          : [],
      });
    });
  };

  /**
   * Gets home improvement applications
   * @param contactId - Portal contact ID
   */
  getHomeImprovementAppsByContact = (
    contactId: string
  ): Promise<ApiServiceResult<HomeImprovementApplication[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_homeimprovement',
          query: `$filter=_ptas_portalcontactid_value eq '${contactId}'
            &$expand=ptas_exemptionbeginyearid($select=ptas_name)`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosHiInstance.post<{
          items: { changes: HomeImprovementApplicationEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<HomeImprovementApplication[]>({
        data: res.items?.length
          ? res.items.map((i) => new HomeImprovementApplication(i.changes))
          : [],
      });
    });
  };

  /**
   * Gets home improvement applications
   * @param parcelId - Parcel ID
   * @param contactId - Portal contact ID
   */
  getHomeImprovementAppByParcelAndContact = (
    parcelId: string,
    contactId: string
  ): Promise<ApiServiceResult<HomeImprovementApplication[] | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_homeimprovement',
          query: `$filter=_ptas_parcelid_value eq '${parcelId}' 
                    and _ptas_portalcontactid_value eq '${contactId}'
                    &$expand=ptas_exemptionbeginyearid($select=ptas_name)`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosHiInstance.post<{
          items: { changes: HomeImprovementApplicationEntity }[];
        }>(url, data)
      ).data;
      return new ApiServiceResult<HomeImprovementApplication[]>({
        data: res.items?.length
          ? res.items.map((i) => new HomeImprovementApplication(i.changes))
          : [],
      });
    });
  };

  /**
   * Creates or updates a home improvement application entity
   * @param newContact - contact model
   */
  saveHiApplication = (
    newHiApplication: HomeImprovementApplication,
    oldHiApplication?: HomeImprovementApplication
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = this.getHiApplicationChanges(
      newHiApplication,
      oldHiApplication
    );
    //If "changes" is empty, don't send update request
    if (Object.keys(changes).length === 0) {
      return new Promise((res) =>
        res(
          new ApiServiceResult<unknown>({
            data: {},
          })
        )
      );
    }

    const data = {
      items: [
        {
          entityName: 'ptas_homeimprovement',
          entityId: newHiApplication.homeImprovementId,
          changes: changes,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosHiInstance.post<{
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

  getHiApplicationChanges = (
    newHiApplication: HomeImprovementApplication,
    oldHiApplication?: HomeImprovementApplication
  ): object => {
    const changes: { [k: string]: string | number | boolean | null } = {};
    if (oldHiApplication) {
      if (
        oldHiApplication.signedByTaxpayer !== newHiApplication.signedByTaxpayer
      ) {
        changes['ptas_applicationsignedbytaxpayer'] =
          newHiApplication.signedByTaxpayer;
      }
      if (oldHiApplication.stateCode !== newHiApplication.stateCode) {
        changes['statecode'] = newHiApplication.stateCode;
      }
      if (oldHiApplication.statusCode !== newHiApplication.statusCode) {
        changes['statuscode'] = newHiApplication.statusCode;
      }
      if (oldHiApplication.parcelId !== newHiApplication.parcelId) {
        changes['_ptas_parcelid_value'] = newHiApplication.parcelId;
      }
      if (
        oldHiApplication.homeImprovementId !==
        newHiApplication.homeImprovementId
      ) {
        changes['ptas_homeimprovementid'] = newHiApplication.homeImprovementId;
      }
      if (oldHiApplication.permitId !== newHiApplication.permitId) {
        changes['_ptas_permitid_value'] = newHiApplication.permitId;
      }
      if (
        oldHiApplication.estimatedConstructionCost !==
        newHiApplication.estimatedConstructionCost
      ) {
        changes['ptas_estimatedconstructioncost'] =
          newHiApplication.estimatedConstructionCost;
      }
      if (
        oldHiApplication.constructionBeginDate !==
        newHiApplication.constructionBeginDate
      ) {
        if (utilService.validDate(newHiApplication.constructionBeginDate)) {
          changes[
            'ptas_constructionbegindate'
          ] = newHiApplication.constructionBeginDate.toISOString();
        }
      }
      if (
        oldHiApplication.estimatedCompletionDate !==
        newHiApplication.estimatedCompletionDate
      ) {
        if (utilService.validDate(newHiApplication.estimatedCompletionDate)) {
          changes[
            'ptas_estimatedcompletiondate'
          ] = newHiApplication.estimatedCompletionDate.toISOString();
        }
      }
      if (oldHiApplication.phoneNumber !== newHiApplication.phoneNumber) {
        changes['ptas_phonenumber'] = newHiApplication.phoneNumber;
      }
      if (oldHiApplication.emailAddress !== newHiApplication.emailAddress) {
        changes['ptas_emailaddress'] = newHiApplication.emailAddress;
      }
      if (
        oldHiApplication.permitJurisdictionId !==
        newHiApplication.permitJurisdictionId
      ) {
        changes['_ptas_permitjurisdictionid_value'] =
          newHiApplication.permitJurisdictionId;
      }
      if (
        oldHiApplication.descriptionOfTheImprovement !==
        newHiApplication.descriptionOfTheImprovement
      ) {
        changes['ptas_descriptionoftheimprovement'] =
          newHiApplication.descriptionOfTheImprovement;
      }
      if (
        oldHiApplication.portalContactId !== newHiApplication.portalContactId
      ) {
        changes['_ptas_portalcontactid_value'] =
          newHiApplication.portalContactId;
      }
      if (
        oldHiApplication.dateApplicationReceived !==
        newHiApplication.dateApplicationReceived
      ) {
        changes[
          'ptas_dateapplicationreceived'
        ] = newHiApplication.dateApplicationReceived.toISOString();
      }
      if (
        oldHiApplication.constructionPropertyAddress !==
        newHiApplication.constructionPropertyAddress
      ) {
        changes['ptas_constructionpropertyaddress'] =
          newHiApplication.constructionPropertyAddress;
      }
      if (oldHiApplication.taxAccountId !== newHiApplication.taxAccountId) {
        changes['_ptas_taxaccountid_value'] = newHiApplication.taxAccountId;
      }
      if (
        oldHiApplication.datePermitIssued !== newHiApplication.datePermitIssued
      ) {
        if (utilService.validDate(newHiApplication.datePermitIssued)) {
          changes[
            'ptas_datepermitissued'
          ] = newHiApplication.datePermitIssued.toISOString();
        }
      }
      if (
        oldHiApplication.applicationSource !==
        newHiApplication.applicationSource
      ) {
        changes['ptas_applicationsource'] = newHiApplication.applicationSource;
      }
      if (
        oldHiApplication.exemptionBeginYearId !==
        newHiApplication.exemptionBeginYearId
      ) {
        changes['_ptas_exemptionbeginyearid_value'] =
          newHiApplication.exemptionBeginYearId;
      }
    } else {
      changes['ptas_applicationsignedbytaxpayer'] =
        newHiApplication.signedByTaxpayer;
      changes['statecode'] = newHiApplication.stateCode;
      changes['statuscode'] = newHiApplication.statusCode;
      changes['_ptas_parcelid_value'] = newHiApplication.parcelId;
      changes['ptas_homeimprovementid'] = newHiApplication.homeImprovementId;
      changes['_ptas_permitid_value'] = newHiApplication.permitId;
      changes['ptas_estimatedconstructioncost'] =
        newHiApplication.estimatedConstructionCost;
      if (utilService.validDate(newHiApplication.constructionBeginDate)) {
        changes[
          'ptas_constructionbegindate'
        ] = newHiApplication.constructionBeginDate.toISOString();
      }
      if (utilService.validDate(newHiApplication.estimatedCompletionDate)) {
        changes[
          'ptas_estimatedcompletiondate'
        ] = newHiApplication.estimatedCompletionDate.toISOString();
      }
      changes['ptas_phonenumber'] = newHiApplication.phoneNumber;
      changes['ptas_emailaddress'] = newHiApplication.emailAddress;
      changes['_ptas_permitjurisdictionid_value'] =
        newHiApplication.permitJurisdictionId;
      changes['ptas_descriptionoftheimprovement'] =
        newHiApplication.descriptionOfTheImprovement;
      changes['_ptas_portalcontactid_value'] = newHiApplication.portalContactId;
      changes[
        'ptas_dateapplicationreceived'
      ] = newHiApplication.dateApplicationReceived.toISOString();
      changes['ptas_constructionpropertyaddress'] =
        newHiApplication.constructionPropertyAddress;
      changes['_ptas_taxaccountid_value'] = newHiApplication.taxAccountId;
      if (utilService.validDate(newHiApplication.datePermitIssued)) {
        changes[
          'ptas_datepermitissued'
        ] = newHiApplication.datePermitIssued.toISOString();
      }
      changes['ptas_applicationsource'] = newHiApplication.applicationSource;
      changes['_ptas_exemptionbeginyearid_value'] =
        newHiApplication.exemptionBeginYearId;
    }
    return changes;
  };
}

export const hiApiService = new HiApiService();
