// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import {
  PersonalProperty,
  PersonalPropertyEntity,
} from 'models/personalProperty';
import { ApiServiceResult, handleReq } from 'services/common/httpClient';
import { axiosPerPropInstance } from '../../axiosInstances';
import { NaicsCode } from 'models/naicsCode';
import {
  FileAttachmentMetadata,
  FileAttachmentMetadataEntityFields,
} from 'models/fileAttachmentMetadata';
import { isEmpty } from 'lodash';
import {
  Asset,
  AssetCategory,
  AssetCategoryEntity,
  AssetEntity,
  assetTypeCategory,
  YearEntity,
} from 'models/assets';
import {
  BUILDING_LAND_IMPS_4_CODE,
  BUILDING_LAND_IMPS_6_CODE,
  LEASED_LAND_CODE,
  LEASEHOLD_CODE,
  SIDE_IMPROVEMENT_CODE,
  STORED_LEASEHOLD_CODE,
} from 'routes/models/Views/NewBusiness/constants';
interface AssessedResponse {
  changes: {
    _ptas_personalpropertyid_value: string;
    ptas_yearid?: {
      ptas_yearid: string;
      ptas_name: string;
    };
  };
}

class BusinessApiService {
  getBusinessById = (
    id: string
  ): Promise<ApiServiceResult<PersonalProperty | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_personalproperty',
          query: `$filter=ptas_personalpropertyid eq '${id}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: {
            changes: PersonalPropertyEntity;
          }[];
        }>(url, data)
      ).data;

      const { data: assessedDates } = await this.getAssessedDates([id]);
      const assessedYearFound = assessedDates?.get(id)?.toString() ?? '-';

      let dataResp = undefined;

      if (res.items && res.items.length) {
        const businessFound = new PersonalProperty(res.items[0].changes);
        businessFound.assessedYear = assessedYearFound;
        dataResp = businessFound;
      }

      return new ApiServiceResult<PersonalProperty | undefined>({
        data: dataResp,
      });
    });
  };

  getBusinessByAccount = (
    accountNumber: string
  ): Promise<ApiServiceResult<PersonalProperty | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_personalproperty',
          query: `$filter=ptas_accountnumber eq '${accountNumber}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: {
            changes: PersonalPropertyEntity;
          }[];
        }>(url, data)
      ).data;

      let dataResp = undefined;

      if (res.items && res.items.length)
        dataResp = new PersonalProperty(res.items[0].changes);

      return new ApiServiceResult<PersonalProperty | undefined>({
        data: dataResp,
      });
    });
  };

  getBusinessByAccAndCode = (
    accountNumber: string,
    accessCode: string
  ): Promise<ApiServiceResult<PersonalProperty | undefined>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_personalproperty',
          query: `$filter=ptas_elistingaccesscode eq '${accessCode}' and ptas_accountnumber eq '${accountNumber}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: {
            changes: PersonalPropertyEntity;
          }[];
        }>(url, data)
      ).data;

      let dataResp = undefined;

      if (res.items && res.items.length)
        dataResp = new PersonalProperty(res.items[0].changes);

      return new ApiServiceResult<PersonalProperty | undefined>({
        data: dataResp,
      });
    });
  };

  /**
   * Creates or updates a personal property record
   * @param personalProperty - personal property model
   * @param omitFields - Entity fields to omit changes
   */
  addBusiness = (
    personalProperty: PersonalProperty
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = {
      /* eslint-disable @typescript-eslint/camelcase */
      ptas_personalpropertyid: personalProperty.id,
      ptas_businessname: personalProperty.businessName,
      ptas_ubi: personalProperty.ubi,
      _ptas_naicscodeid_value: personalProperty.naicsNumber,
      ptas_propertytype: personalProperty.propertyType,
      ptas_addr1_business: personalProperty.address,
      ptas_addr1_business_line2: personalProperty.addressCont,
      _ptas_taxpayercountryid_value: personalProperty.countryId,
      ptas_taxpayername: personalProperty.taxpayerName,
      _ptas_stateincorporatedid_value: personalProperty.stateOfIncorporationId,
      ptas_accountnumber: personalProperty.accountNumber,
      ptas_elistingaccesscode: personalProperty.accessCode,
      statecode: personalProperty.stateCode,
      statuscode: personalProperty.statusCode,
      ptas_preparer_cellphone1: personalProperty.preparerCellphone,
      ptas_addr1_preparer: personalProperty.preparerAddress,
      ptas_addr1_preparer_line2: personalProperty.preparerAddressLine2,
      _ptas_preparercountryid_value: personalProperty.preparerCountryId,
      _ptas_addr1_business_cityid_value: personalProperty.cityId,
      _ptas_addr1_business_stateid_value: personalProperty.stateId,
      _ptas_addr1_business_zipcodeid_value: personalProperty.zipId,
      ptas_preparer_email1: personalProperty.preparerEmail,
      ptas_naicsdescription: personalProperty.naicsDescription,
      _ptas_addr1_preparer_cityid_value: personalProperty.preparerCityId,
      _ptas_addr1_preparer_zipcodeid_value: personalProperty.preparerZipCodeId,
      _ptas_addr1_preparer_stateid_value: personalProperty.preparerStateId,
      ptas_preparer_attention: personalProperty.preparerAttention,
    };

    const data = {
      items: [
        {
          entityName: 'ptas_personalproperty',
          entityId: personalProperty.id,
          changes,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  getAssessedDates = (
    personalPropertyIds: string[]
  ): Promise<ApiServiceResult<Map<string, number>>> => {
    const requests = personalPropertyIds.map((personalPropertyId) => {
      return {
        entityName: 'ptas_personalpropertyhistory',
        query: `$filter=_ptas_personalpropertyid_value eq '${personalPropertyId}'&$select=_ptas_yearid_value,_ptas_personalpropertyid_value&$expand=ptas_yearid($select=ptas_name)`,
      };
    });
    const data = {
      requests,
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const res = (
        await axiosPerPropInstance.post<{
          items: AssessedResponse[];
        }>(url, data)
      ).data;

      let dataResp = undefined;

      if (res.items && res.items.length) {
        dataResp = res.items.reduce(
          (
            prev: Map<string, number>,
            current: AssessedResponse
          ): Map<string, number> => {
            const {
              // eslint-disable-next-line @typescript-eslint/camelcase
              changes: {
                ptas_yearid: yearId,
                _ptas_personalpropertyid_value: personalPropId,
              },
            } = current;
            try {
              if (!yearId?.ptas_name) return prev;
              const yearCompare = parseInt(yearId.ptas_name);
              const prevYearByPersonalProp = prev.get(personalPropId);
              if (
                !prevYearByPersonalProp ||
                yearCompare > prevYearByPersonalProp
              ) {
                prev.set(personalPropId, yearCompare);
              }
              return prev;
            } catch (error) {
              return prev;
            }
          },
          new Map()
        );
      }

      return new ApiServiceResult<Map<string, number>>({
        data: dataResp ?? new Map(),
      });
    });
  };

  /**
   * Get naics number suggestions
   * @param naicNumber - search param
   */
  getNaicNumberSuggestions = (
    naicNumber: number
  ): Promise<ApiServiceResult<NaicsCode[] | undefined>> => {
    const searchParam = naicNumber;

    const data = {
      requests: [
        {
          entityName: 'ptas_naicscode',
          query: `$top=4&$select=ptas_naicscodeid,ptas_description,ptas_code&$filter=ptas_code eq ${searchParam}`,
        },
      ],
    };

    return handleReq(async () => {
      const url = `/GenericDynamics/GetItems`;

      const res = await axiosPerPropInstance.post<{
        items: { changes: NaicsCode }[];
      }>(url, data);

      return new ApiServiceResult<NaicsCode[]>({
        data: res.data.items.map((i) => i.changes),
      });
    });
  };

  /**
   * get file attachment metadata for business
   * @param params - request, entityName field is not required
   */
  getBusinessFileAttachmentData = (
    personalPropertyId: string
  ): Promise<ApiServiceResult<FileAttachmentMetadata[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_fileattachmentmetadata',
          query: `$filter=_ptas_personalpropertyaccountid_value eq '${personalPropertyId}'`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: {
            changes: FileAttachmentMetadataEntityFields;
          }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<FileAttachmentMetadata[]>({
        data:
          result?.items.map(
            ({ changes }) =>
              new FileAttachmentMetadata(
                changes as FileAttachmentMetadataEntityFields
              )
          ) ?? ([] as FileAttachmentMetadata[]),
      });
    });
  };

  /**
   * Creates a file attachment metadata entity
   * @param fileAttachment - file attachment metadata model
   * @param personalPropertyId - personal property id
   */
  saveFileAttachmentData = (
    fileAttachment: FileAttachmentMetadata,
    personalPropertyId: string // INFO: this is account number too
  ): Promise<ApiServiceResult<unknown>> => {
    const data = {
      items: [
        {
          entityName: 'ptas_fileattachmentmetadata',
          entityId: fileAttachment.id,
          changes: {
            /* eslint-disable @typescript-eslint/camelcase */
            ptas_fileattachmentmetadataid: fileAttachment.id,
            ptas_name: fileAttachment.name,
            _ptas_parcelid_value: fileAttachment.parcelId,
            ptas_bloburl: fileAttachment.blobUrl,
            ptas_portaldocument: fileAttachment.document,
            ptas_portalsection: fileAttachment.section,
            ptas_icsdocumentid: fileAttachment.icsDocumentId,
            ptas_isblob: fileAttachment.isBlob,
            ptas_issharepoint: fileAttachment.isSharePoint,
            ptas_sharepointurl: fileAttachment.sharepointUrl,
            _ptas_personalpropertyaccountid_value: personalPropertyId,
            ptas_documenttype: fileAttachment.documentType,
            ptas_filingmethod: fileAttachment.filingMethod,
            ptas_listingstatus: fileAttachment.listingStatus,
            _ptas_yearid_value: fileAttachment.year,
            ptas_loaddate: fileAttachment.loadDate,
            _ptas_loadbyid_value: fileAttachment.loadBy,
            /* eslint-enable @typescript-eslint/camelcase */
          },
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  /**
   * update a business
   * @param oldBusiness - personal property model (before being updated)
   * @param businessUpdated - personal property model (with the updates)
   */
  updateBusiness = (
    oldBusiness: PersonalProperty,
    businessUpdated: PersonalProperty
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = this.getChanges(oldBusiness, businessUpdated);

    if (isEmpty(changes)) {
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
          entityName: 'ptas_personalproperty',
          entityId: businessUpdated.id,
          changes,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  /**
   * get years
   */
  getYears = (): Promise<ApiServiceResult<YearEntity[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_year',
          query: `$select=ptas_name, ptas_yearid`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: { changes: YearEntity }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<YearEntity[]>({
        data: result.items.map((i) => i.changes),
      });
    });
  };

  /**
   * Obtaining assets by business id
   */
  getAssetsByBusiness = (id: string): Promise<ApiServiceResult<Asset[]>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_personalpropertyasset',
          query: `$select=ptas_personalpropertyassetid, _ptas_yearacquiredid_value, ptas_originalcost, _ptas_categorycodeid_value,
          ptas_changereason,_ptas_personalpropertyid_value&$filter=_ptas_personalpropertyid_value eq ${id}`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: { changes: AssetEntity }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;

      const assetsMapping = result.items.map((a) => new Asset(a.changes));

      return new ApiServiceResult<Asset[]>({
        data: assetsMapping,
      });
    });
  };

  /**
   * get assets category
   */
  filterAssetCategory = (
    name: string,
    categoryGroup: assetTypeCategory
  ): Promise<ApiServiceResult<AssetCategory[]>> => {
    const categoryGroupFilter =
      categoryGroup !== 'All'
        ? `ptas_categorygrouph eq '${categoryGroup}' and`
        : '';

    /**
     * will create a filter to not include these codes in the request.
     *
     * example: and (not contains(ptas_categorycode, 980)) and (not contains(ptas_categorycode, 981))
     */
    const excludedCodes = [
      LEASED_LAND_CODE,
      LEASEHOLD_CODE,
      SIDE_IMPROVEMENT_CODE,
      STORED_LEASEHOLD_CODE,
      BUILDING_LAND_IMPS_4_CODE,
      BUILDING_LAND_IMPS_6_CODE,
    ].reduce(
      (acc, code) => `${acc} and (not contains(ptas_categorycode, '${code}'))`,
      ''
    );

    const query = `$select=ptas_categorycode,ptas_personalpropertycategoryid,
    ptas_perspropcategory,ptas_perspropcategory,ptas_legacycategorycode,ptas_categorygrouph,ptas_name
    &$filter=${categoryGroupFilter} contains(ptas_perspropcategory, '${name}') and statecode eq 0 ${excludedCodes}
    &$orderby=ptas_perspropcategory
    `;

    const data = {
      requests: [
        {
          entityName: 'ptas_personalpropertycategory',
          query,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: { changes: AssetCategoryEntity }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;

      const resultMapping = result.items.map(
        (c) => new AssetCategory(c.changes)
      );

      return new ApiServiceResult<AssetCategory[]>({
        data: resultMapping,
      });
    });
  };

  /**
   * get improvement type
   */
  getImprovementType = (): Promise<ApiServiceResult<AssetCategory[]>> => {
    /**
     * will create a filter to not include these codes in the request.
     *
     * example: and (not contains(ptas_categorycode, 980)) or (not contains(ptas_categorycode, 981))
     */
    const codesIncluded = [
      LEASED_LAND_CODE,
      LEASEHOLD_CODE,
      SIDE_IMPROVEMENT_CODE,
      STORED_LEASEHOLD_CODE,
      BUILDING_LAND_IMPS_4_CODE,
      BUILDING_LAND_IMPS_6_CODE,
    ].reduce(
      (acc, code, i) =>
        `${acc} ${i !== 0 ? 'or' : ''} contains(ptas_categorycode, '${code}')`,
      ''
    );

    const query = `$select=ptas_categorycode,ptas_personalpropertycategoryid,
      ptas_perspropcategory,ptas_perspropcategory,ptas_legacycategorycode,ptas_categorygrouph,ptas_name
      &$filter=statecode eq 0 and (${codesIncluded})
      &$orderby=ptas_perspropcategory
      `;

    const data = {
      requests: [
        {
          entityName: 'ptas_personalpropertycategory',
          query,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: { changes: AssetCategoryEntity }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;

      const resultMapping = result.items.map(
        (c) => new AssetCategory(c.changes)
      );

      return new ApiServiceResult<AssetCategory[]>({
        data: resultMapping,
      });
    });
  };

  /**
   * create assets
   */
  createAssetBusiness = (
    assets: Asset[]
  ): Promise<ApiServiceResult<unknown>> => {
    const assetsToSave = assets.map((a) => {
      const changes = {
        /* eslint-disable @typescript-eslint/camelcase */
        ptas_personalpropertyassetid: a.id,
        _ptas_yearacquiredid_value: a.yearAcquiredId,
        ptas_originalcost: a.cost,
        _ptas_categorycodeid_value: a.categoryCodeId,
        ptas_changereason: a.changeReason,
        _ptas_personalpropertyid_value: a.personalPropertyId,
      };

      return {
        entityName: 'ptas_personalpropertyasset',
        entityId: a.id,
        changes,
      };
    });

    const data = {
      items: assetsToSave,
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;

      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  /**
   * update assets
   */
  updateAssetBusiness = (
    oldAsset: Asset,
    assetUpdated: Asset
  ): Promise<ApiServiceResult<unknown>> => {
    const changes = this.getAssetsChanges(oldAsset, assetUpdated);

    if (isEmpty(changes)) {
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
          entityName: 'ptas_personalpropertyasset',
          entityId: assetUpdated.id,
          changes,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/UpdateItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: { changes: object }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;

      return new ApiServiceResult<unknown>({
        data: result,
      });
    });
  };

  getAssetsChanges = (oldAsset: Asset, newAsset: Asset): object => {
    const objectKeys = new Map<string, string>([
      ['yearAcquiredId', '_ptas_yearacquiredid_value'],
      ['cost', 'ptas_originalcost'],
      ['categoryCodeId', '_ptas_categorycodeid_value'],
      ['changeReason', 'ptas_changereason'],
    ]);

    let dataToEdit = {};

    objectKeys.forEach((value, key) => {
      const keyAsset = key as keyof Asset;

      if (oldAsset[keyAsset] !== newAsset[keyAsset]) {
        dataToEdit = {
          ...dataToEdit,
          [value]: newAsset[keyAsset],
        };
      }
    });

    return dataToEdit;
  };

  getChanges = (
    oldBusiness: PersonalProperty,
    businessUpdated: PersonalProperty
  ): object => {
    const objectKeys = new Map<string, string>([
      ['businessName', 'ptas_businessname'],
      ['ubi', 'ptas_ubi'],
      ['naicsNumber', '_ptas_naicscodeid_value'],
      ['propertyType', 'ptas_propertytype'],
      ['address', 'ptas_addr1_business'],
      ['addressCont', 'ptas_addr1_business_line2'],
      ['cityId', '_ptas_addr1_business_cityid_value'],
      ['stateId', '_ptas_addr1_business_stateid_value'],
      ['zipId', '_ptas_addr1_business_zipcodeid_value'],
      ['countryId', '_ptas_taxpayercountryid_value'],
      ['taxpayerName', 'ptas_taxpayername'],
      ['taxpayerAttention', 'ptas_taxpayer_attention'],
      ['stateOfIncorporationId', '_ptas_stateincorporatedid_value'],
      ['naicsDescription', 'ptas_naicsdescription'],
      ['accountNumber', 'ptas_accountnumber'],
      ['accessCode', 'ptas_elistingaccesscode'],
      ['filedDate', 'ptas_filingdate'],
      ['statecode', 'statecode'],
      ['statuscode', 'statuscode'],
      ['preparerAttention', 'ptas_preparer_attention'],
      ['preparerCellphone', 'ptas_preparer_cellphone1'],
      ['preparerAddress', 'ptas_addr1_preparer'],
      ['preparerAddressLine2', 'ptas_addr1_preparer_line2'],
      ['preparerCountryId', '_ptas_preparercountryid_value'],
      ['preparerCityId', '_ptas_addr1_preparer_cityid_value'],
      ['preparerStateId', '_ptas_addr1_preparer_stateid_value'],
      ['preparerZipCodeId', '_ptas_addr1_preparer_zipcodeid_value'],
      ['preparerEmail', 'ptas_preparer_email1'],
      ['preparerName', 'ptas_preparername'],
    ]);

    let dataToEdit = {};

    objectKeys.forEach((value, key) => {
      const keyPersonalProp = key as keyof PersonalProperty;

      if (oldBusiness[keyPersonalProp] !== businessUpdated[keyPersonalProp]) {
        dataToEdit = {
          ...dataToEdit,
          [value]: businessUpdated[keyPersonalProp],
        };
      }
    });

    return dataToEdit;
  };

  /**
   * get years
   */
  getYearsIdByYears = (year: number): Promise<ApiServiceResult<string>> => {
    const data = {
      requests: [
        {
          entityName: 'ptas_year',
          query: `$select=ptas_yearid &$filter=ptas_name eq '${year}' and statecode eq 0 and statuscode eq 1`,
        },
      ],
    };

    return handleReq(async () => {
      const url = '/GenericDynamics/GetItems';
      const result = (
        await axiosPerPropInstance.post<{
          items: {
            changes: {
              ptas_yearid: string;
            };
          }[];
        }>(url, data, {
          headers: {
            Authorization: 'Bearer ' + process.env.REACT_APP_MAGIC_TOKEN_TEMP,
          },
        })
      ).data;
      return new ApiServiceResult<string>({
        data: result.items.map((i) => i.changes)[0].ptas_yearid,
      });
    });
  };
}

export const businessApiService = new BusinessApiService();
