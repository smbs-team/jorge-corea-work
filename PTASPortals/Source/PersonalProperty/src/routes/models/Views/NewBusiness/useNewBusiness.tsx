// useNewBusiness.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  PersonalProperty as NewBusiness,
  PersonalProperty,
} from 'models/personalProperty';
import { addressService } from 'services/api/apiService/address';
import { useContext } from 'react';
import { NewBusinessContext } from 'contexts/NewBusiness';
import { UseNewBusiness } from 'contexts/NewBusiness/types';
import { PortalContact } from 'models/portalContact';
import { AppContext } from 'contexts/AppContext';
import { apiService } from 'services/api/apiService';
import { v4 as uuidV4 } from 'uuid';
import { isEmpty } from 'lodash';
import { DropDownItem, ItemSuggestion } from '@ptas/react-public-ui-library';
import { PersonalPropertyAccountAccess } from 'models/personalPropertyAccountAccess';
import { businessContactApiService } from 'services/api/apiService/businessContact';
import { LEVEL_EDIT, STATUS_CODE_ACTIVE } from '../Home/constants';
import { useHistory } from 'react-router-dom';
import { businessApiService } from 'services/api/apiService/business';
import { HomeState } from '.';
import { useLocation } from 'react-router-dom';
import { Asset } from 'models/assets';
import { formatDate } from 'utils/date';
import { getFileFromUrl } from 'services/common/getFileFromUrl';

interface NewBusinessType extends UseNewBusiness {
  saveNewBusiness: (key: keyof NewBusiness, value: string | number) => void;
  // index functions
  handleSelectedTab: (step: number) => void;
  persistDataOnJsonStore: () => void;
  loadNewBusinessInfo: () => Promise<void>;
  loadPropertyType: () => Promise<void>;
  loadCountryOpts: () => Promise<void>;
  loadStateOrProvince: () => Promise<void>;
  loadContactInfo: () => void;
  loadPhoneTypes: () => Promise<void>;
  loadStatusCode: () => Promise<void>;
  fetchStatusCodeOpt: () => Promise<void>;
  fetchAccessLevelOpt: () => Promise<void>;
  createRelationContact: () => Promise<void>;
  deleteInfo: () => Promise<void>;
  // basic info tab functions
  handleChangeCustomText: (e: React.ChangeEvent<HTMLInputElement>) => void;
  handleChangePropType: (item: DropDownItem) => void;
  handleSelectNaicsNumber: (item: ItemSuggestion) => void;
  handelShowForm: (isChecked: boolean) => void;
  // location tab functions
  handleChangeState: (item: DropDownItem) => void;
  onSelectZipCode: (item: ItemSuggestion) => void;
  handleChangeLine2: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onChangeCountry: (item: DropDownItem) => void;
  getSuggestionListAddr: () => ItemSuggestion[];
  // contact tab
  onSelectZipCodeContact: (item: ItemSuggestion) => void;
  onChangeCountryContact: (item: DropDownItem) => void;
  // assets function
  loadYearsAcquired: () => Promise<void>;
  loadChangeReason: () => Promise<void>;
  loadCategoryAssets: () => Promise<void>;
  saveAssetsInJsonStorage: () => void;
  // improvement
  loadImprovementTypes: () => Promise<void>;
}

const useNewBusiness = (): NewBusinessType => {
  const context = useContext(NewBusinessContext);
  const { portalContact } = useContext(AppContext);
  const history = useHistory();
  const { state } = useLocation<HomeState>();

  const {
    setNewBusiness,
    setCurrentStepIndex,
    newBusiness,
    statusCodeAccAccess,
    setPropertyType,
    setCountryOpts,
    setStateOrProvince,
    setContactInfo,
    setPhoneTypeList,
    setStatusCode,
    setStatusCodeAccAccess,
    setLevelOpts,
    levelOpts,
    setShowFormPurchasedMoved,
    addrSuggestions,
    setAddrContLocationError,
    setYearsAcquired,
    setChangeReason,
    setAssetsCategoryList,
    setAllAssetsCategoryList,
    setAssetsList,
    setImprovementTypeList,
    assetsList,
    setFilesAttach,
  } = context;

  const saveNewBusiness = (
    key: keyof NewBusiness,
    value: string | number
  ): void => {
    setNewBusiness((prev) => {
      return {
        ...prev,
        [key]: value,
      };
    });
  };

  //#region index
  const handleSelectedTab = (tab: number): void => {
    setCurrentStepIndex(tab);
  };

  const persistDataOnJsonStore = (): void => {
    let userContactID = '';

    if (portalContact && state?.createFromHome && portalContact?.id) {
      userContactID = portalContact.id;
    } else {
      let anonymousContactId = localStorage.getItem('contactId');

      if (!anonymousContactId) {
        const uuid = uuidV4();
        localStorage.setItem('contactId', uuid);

        anonymousContactId = uuid;
      }

      userContactID = anonymousContactId;
    }

    apiService.saveJson(`portals/${userContactID}/business`, newBusiness);
  };

  const loadAssetFromJsonStorage = async (
    businessId: string
  ): Promise<void> => {
    const { data } = await apiService.getJson(
      `portals/${businessId}/assets/assets`
    );

    if (isEmpty(data)) return;

    const assets = (data as unknown as Asset[]).map((a) => ({
      ...a,
      modifiedOn: formatDate(a.modifiedOn),
    }));

    setAssetsList(assets);
  };

  const loadNewBusinessInfo = async (): Promise<void> => {
    const contactId = state?.createFromHome
      ? portalContact?.id
      : localStorage.getItem('contactId');

    if (!contactId) return;

    const { data } = await apiService.getJson(
      `portals/${contactId}/business/business`
    );

    if (isEmpty(data)) return;

    const businessInfo = data as unknown as PersonalProperty;

    loadAssetFromJsonStorage(businessInfo.id);
    setNewBusiness(businessInfo);

    if (businessInfo.attachUrl) {
      setFilesAttach([
        {
          isUploading: false,
          content: businessInfo.attachUrl,
          fileName: businessInfo.attachName,
          id: businessInfo.attachId,
          contentType: 'publicUrl',
          file: await getFileFromUrl(businessInfo.attachUrl),
        },
      ]);
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

  const loadCountryOpts = async (): Promise<void> => {
    const { data, hasError } = await addressService.getCountries();

    if (hasError) {
      console.log('Error to get countries');

      return setCountryOpts([]);
    }

    const countriesOptions = (data || []).map((c) => ({
      value: c.ptas_countryid,
      label: c.ptas_name,
    }));

    setCountryOpts(countriesOptions);
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

  const loadContactInfo = (): void => {
    if (portalContact) return setContactInfo(portalContact);

    setContactInfo(new PortalContact());
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
    }
  };

  const loadStatusCode = async (): Promise<void> => {
    const { hasError, data } = await apiService.getOptionSet(
      'ptas_personalproperty',
      'statuscode'
    );

    if (hasError) {
      return console.log('Error getting option set mapping');
    }

    if (!data) return;

    const optionSetMap = new Map<string, number>(
      data.map((code) => [code.value, code.attributeValue])
    );

    setStatusCode(optionSetMap);
  };

  // status code from personal property account access
  const fetchStatusCodeOpt = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_personalpropertyaccountaccess',
      'statuscode'
    );

    if (!data) return;

    const newOpts = data.map(
      (opt) => [opt.value, opt.attributeValue] as [string, number]
    );
    setStatusCodeAccAccess(new Map(newOpts));
  };

  // access level from personal property account access
  const fetchAccessLevelOpt = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_personalpropertyaccountaccess',
      'ptas_accesslevel'
    );
    if (!data) return;
    const newOpts = data.map(
      (opt) => [opt.value, opt.attributeValue] as [string, number]
    );
    setLevelOpts(new Map(newOpts));
  };

  // create relation between contact and new business
  const createRelationContact = async (): Promise<void> => {
    const newBusinessContact = new PersonalPropertyAccountAccess();
    newBusinessContact.id = uuidV4();
    newBusinessContact.portalContactId = portalContact?.id ?? '';
    newBusinessContact.accessLevel = levelOpts.get(LEVEL_EDIT) as number;
    newBusinessContact.personalPropertyId = newBusiness.id;
    newBusinessContact.stateCode = 0;
    newBusinessContact.statusCode = statusCodeAccAccess.get(
      STATUS_CODE_ACTIVE
    ) as number;

    await businessContactApiService.addBusinessContact(newBusinessContact);

    history.push('/home', { createFromHome: true });
  };

  /**
   * Removes new business information from json storage and also removes the contact id from local storage.
   */
  const deleteInfo = async (): Promise<void> => {
    const contactId = portalContact?.id || localStorage.getItem('contactId');
    const deleteBusiness = apiService.deleteJson(
      `portals/${contactId}/business/business`
    );
    const deleteAssets = apiService.deleteJson(
      `portals/${newBusiness.id}/assets/assets`
    );

    Promise.all([deleteAssets, deleteBusiness])
      .catch(() => {
        console.log('Error removing from json storage');
      })
      .then(() => {
        localStorage.clear();
      });
  };
  //#endregion index

  //#region basic info functions
  const handleChangeCustomText = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    const value = e.target.value;
    const name = e.target.name as keyof PersonalProperty;

    saveNewBusiness(name, value);
  };

  const handleChangePropType = (item: DropDownItem): void => {
    saveNewBusiness('propertyType', parseInt(item.value.toString()));
  };

  const handleSelectNaicsNumber = (item: ItemSuggestion): void => {
    setNewBusiness((prev) => {
      return {
        ...prev,
        naicsNumber: (item.id as string) ?? '',
        naicsDescription: item.title ?? '',
      };
    });
  };

  const handelShowForm = (isChecked: boolean): void =>
    setShowFormPurchasedMoved(isChecked);
  //#endregion basic info functions

  //#region location
  const handleChangeState = (item: DropDownItem): void => {
    saveNewBusiness('stateOfIncorporationId', item.value);
  };

  const onSelectZipCode = (item: ItemSuggestion): void => {
    setNewBusiness((prev) => {
      return {
        ...prev,
        zipId: (item.id as string) ?? '',
        addrZipcodeLabel: item.title,
      };
    });
  };

  const handleChangeLine2 = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const value = e.target.value;
    setAddrContLocationError(!value);
    saveNewBusiness('addressCont', value);
  };

  const onChangeCountry = (item: DropDownItem): void => {
    saveNewBusiness('countryId', item.value);
  };

  const getSuggestionListAddr = (): ItemSuggestion[] => {
    return addrSuggestions.map((addr) => {
      return {
        title: addr.streetname,
        id: `${addr.laitude}-${addr.longitude}`,
        subtitle: addr.formattedaddr,
      };
    });
  };

  //#endregion location

  // #region contact tab functions

  const onSelectZipCodeContact = (item: ItemSuggestion): void => {
    setNewBusiness((prev) => {
      return {
        ...prev,
        preparerZipCodeId: (item.id as string) ?? '',
        preparerZipcodeLabel: item.title,
      };
    });
  };

  const onChangeCountryContact = (item: DropDownItem): void => {
    saveNewBusiness('preparerCountryId', item.value);
  };
  // #endregion contact tab functions

  //#region assets function
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

  const loadCategoryAssets = async (): Promise<void> => {
    const { data } = await businessApiService.filterAssetCategory('', 'All');

    if (data) {
      setAssetsCategoryList(data);
      setAllAssetsCategoryList(data);
    }
  };

  const saveAssetsInJsonStorage = (): void => {
    apiService.saveJson(`portals/${newBusiness?.id}/assets`, assetsList);
  };

  //#endregion assets function

  const loadImprovementTypes = async (): Promise<void> => {
    const { data } = await businessApiService.getImprovementType();

    setImprovementTypeList(data ?? []);
  };

  return {
    saveNewBusiness,
    persistDataOnJsonStore,
    handleSelectedTab,
    loadNewBusinessInfo,
    createRelationContact,
    deleteInfo,
    fetchAccessLevelOpt,
    fetchStatusCodeOpt,
    loadContactInfo,
    loadCountryOpts,
    loadPhoneTypes,
    loadPropertyType,
    loadStateOrProvince,
    loadStatusCode,
    handleChangeCustomText,
    handleChangePropType,
    handleSelectNaicsNumber,
    handelShowForm,
    getSuggestionListAddr,
    handleChangeLine2,
    handleChangeState,
    onChangeCountry,
    onSelectZipCode,
    onChangeCountryContact,
    onSelectZipCodeContact,
    loadCategoryAssets,
    loadChangeReason,
    loadYearsAcquired,
    saveAssetsInJsonStorage,
    loadImprovementTypes,
    ...context,
  };
};

export default useNewBusiness;
