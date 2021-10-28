// NewBusiness.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren, useState } from 'react';

import { PersonalProperty as NewBusiness } from 'models/personalProperty';
import {
  DropDownItem,
  ItemSuggestion,
  CustomFile,
} from '@ptas/react-public-ui-library';
import { CityEntity, AddressLookupEntity } from 'models/addresses';
import {
  PortalAddress,
  PortalContact,
  PortalEmail,
  PortalPhone,
} from 'models/portalContact';

import { PhoneList } from './types';

import { NewBusinessContext } from './index';
import { Asset, AssetCategory } from 'models/assets';

const NewBusinessProvider = ({
  children,
}: PropsWithChildren<{}>): JSX.Element => {
  const [currentStepIndex, setCurrentStepIndex] = useState<number>(0);
  const [showFormPurchasedMoved, setShowFormPurchasedMoved] =
    useState<boolean>(false);
  const [newBusiness, setNewBusiness] = useState<NewBusiness>(
    new NewBusiness()
  );
  const [propertyType, setPropertyType] = useState<DropDownItem[]>([]);
  const [naicsnumberSuggestionList, setNaicsNumberSuggestionList] = useState<
    ItemSuggestion[]
  >([]);
  // address state
  const [addressSugLoading, setAddressSugLoading] = useState<boolean>(false);
  const [addrSuggestions, setAddrSuggestions] = useState<AddressLookupEntity[]>(
    []
  );

  // tab contact step
  const [citySuggestionsContact, setCitySuggestionsContact] = useState<
    CityEntity[]
  >([]);
  const [addrSuggestionsContact, setAddrSuggestionsContact] = useState<
    AddressLookupEntity[]
  >([]);

  const [countryOpts, setCountryOpts] = useState<DropDownItem[]>([]);
  const [stateOrProvince, setStateOrProvince] = useState<DropDownItem[]>([]);

  const [filesAttach, setFilesAttach] = useState<CustomFile[]>([]);

  const [contactInfo, setContactInfo] = useState<PortalContact>();

  const [phoneTypeList, setPhoneTypeList] = useState<PhoneList[]>([]);

  const [statusCode, setStatusCode] = useState<Map<string, number>>(new Map());

  const [statusCodeAccAccess, setStatusCodeAccAccess] = useState<
    Map<string, number>
  >(new Map());
  const [levelOpts, setLevelOpts] = useState<Map<string, number>>(new Map());

  const [addrLocationError, setAddrLocationError] = useState<boolean>(false);
  const [addrContLocationError, setAddrContLocationError] =
    useState<boolean>(false);
  const [cityLocationError, setCityLocationError] = useState<boolean>(false);
  const [stateLocationError, setStateLocationError] = useState<boolean>(false);
  const [zipCodeLocationError, setZipCodeLocationError] =
    useState<boolean>(false);
  const [countryLocationError, setCountryLocationError] =
    useState<boolean>(false);

  const [addrContactError, setAddrContactError] = useState<boolean>(false);
  const [addrContContactError, setAddrContContactError] =
    useState<boolean>(false);
  const [cityContactError, setCityContactError] = useState<boolean>(false);
  const [stateContactError, setStateContactError] = useState<boolean>(false);
  const [zipCodeContactError, setZipCodeContactError] =
    useState<boolean>(false);
  const [countryContactError, setCountryContactError] =
    useState<boolean>(false);
  const [emailAnchor, setEmailAnchor] = useState<HTMLElement | null>(null);
  const [manageInfoAnchor, setManageInfoAnchor] =
    useState<HTMLElement | null>(null);
  const [addressAnchor, setAddressAnchor] = useState<HTMLElement | null>(null);
  const [phoneAnchor, setPhoneAnchor] = useState<HTMLElement | null>(null);
  const [editingContact, setEditingContact] =
    useState<PortalContact | undefined>();

  // assets state
  const [yearsAcquired, setYearsAcquired] = useState<DropDownItem[]>([]);
  const [changeReason, setChangeReason] = useState<DropDownItem[]>([]);
  const [assetsCategoryList, setAssetsCategoryList] = useState<AssetCategory[]>(
    []
  );

  const [assetsList, setAssetsList] = useState<Asset[]>([]);

  const [allAssetsCategoryList, setAllAssetsCategoryList] = useState<
    AssetCategory[]
  >([]);

  const [assetToUpdate, setAssetToUpdate] =
    useState<Asset | undefined>(undefined);

  const [improvementToUpdate, setImprovementToUpdate] =
    useState<Asset | undefined>(undefined);
  const [improvementTypeList, setImprovementTypeList] = useState<
    AssetCategory[]
  >([]);

  const [addressList, setAddressList] = useState<PortalAddress[]>([]);
  const [emailList, setEmailList] = useState<PortalEmail[]>([]);
  const [phoneList, setPhoneList] = useState<PortalPhone[]>([]);

  return (
    <NewBusinessContext.Provider
      value={{
        addrSuggestions,
        addrSuggestionsContact,
        addressSugLoading,
        citySuggestionsContact,
        contactInfo,
        countryOpts,
        currentStepIndex,
        filesAttach,
        naicsnumberSuggestionList,
        newBusiness,
        phoneTypeList,
        propertyType,
        setContactInfo,
        setCountryOpts,
        setCurrentStepIndex,
        setFilesAttach,
        setNaicsNumberSuggestionList,
        setNewBusiness,
        setPhoneTypeList,
        setPropertyType,
        setShowFormPurchasedMoved,
        setStateOrProvince,
        showFormPurchasedMoved,
        stateOrProvince,
        setAddrSuggestions,
        setAddressSugLoading,
        setCitySuggestionsContact,
        setAddrSuggestionsContact,
        setStatusCode,
        statusCode,
        setStatusCodeAccAccess,
        statusCodeAccAccess,
        levelOpts,
        setLevelOpts,
        addrContContactError,
        addrLocationError,
        cityLocationError,
        countryLocationError,
        setAddrContLocationError,
        setAddrLocationError,
        setCityLocationError,
        setCountryLocationError,
        setStateLocationError,
        setZipCodeLocationError,
        stateLocationError,
        zipCodeLocationError,
        addrContactError,
        cityContactError,
        countryContactError,
        setAddrContContactError,
        setAddrContactError,
        setCityContactError,
        setCountryContactError,
        setStateContactError,
        setZipCodeContactError,
        stateContactError,
        zipCodeContactError,
        addrContLocationError,
        addressAnchor,
        emailAnchor,
        manageInfoAnchor,
        phoneAnchor,
        setAddressAnchor,
        setEmailAnchor,
        setManageInfoAnchor,
        setPhoneAnchor,
        editingContact,
        setEditingContact,
        allAssetsCategoryList,
        assetsCategoryList,
        assetsList,
        changeReason,
        setAllAssetsCategoryList,
        setAssetsCategoryList,
        setAssetsList,
        setChangeReason,
        setYearsAcquired,
        yearsAcquired,
        assetToUpdate,
        setAssetToUpdate,
        improvementToUpdate,
        improvementTypeList,
        setImprovementToUpdate,
        setImprovementTypeList,
        addressList,
        setAddressList,
        setEmailList,
        emailList,
        phoneList,
        setPhoneList,
      }}
    >
      {children}
    </NewBusinessContext.Provider>
  );
};

export default NewBusinessProvider;
