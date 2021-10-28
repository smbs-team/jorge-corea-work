// useNewBusiness.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Dispatch, SetStateAction } from 'react';
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
import { Asset, AssetCategory } from 'models/assets';

export type PhoneList = { value: number; label: string };

export type InputName = 'zipCode' | 'state' | 'city' | 'address';

export interface UseNewBusiness {
  setCurrentStepIndex: Dispatch<SetStateAction<number>>;
  currentStepIndex: number;
  setNewBusiness: Dispatch<SetStateAction<NewBusiness>>;
  newBusiness: NewBusiness;
  setShowFormPurchasedMoved: Dispatch<SetStateAction<boolean>>;
  showFormPurchasedMoved: boolean;
  setNaicsNumberSuggestionList: Dispatch<SetStateAction<ItemSuggestion[]>>;
  naicsnumberSuggestionList: ItemSuggestion[];
  countryOpts: DropDownItem[];
  addressSugLoading: boolean;
  setAddressSugLoading: Dispatch<SetStateAction<boolean>>;
  addrSuggestions: AddressLookupEntity[];
  setAddrSuggestions: Dispatch<React.SetStateAction<AddressLookupEntity[]>>;
  addrSuggestionsContact: AddressLookupEntity[];
  setAddrSuggestionsContact: Dispatch<
    React.SetStateAction<AddressLookupEntity[]>
  >;
  citySuggestionsContact: CityEntity[];
  setCitySuggestionsContact: Dispatch<React.SetStateAction<CityEntity[]>>;
  propertyType: DropDownItem[];
  setPropertyType: Dispatch<SetStateAction<DropDownItem[]>>;
  setCountryOpts: Dispatch<SetStateAction<DropDownItem[]>>;
  setStateOrProvince: Dispatch<SetStateAction<DropDownItem[]>>;
  stateOrProvince: DropDownItem[];
  setFilesAttach: Dispatch<SetStateAction<CustomFile[]>>;
  filesAttach: CustomFile[];
  setContactInfo: Dispatch<SetStateAction<PortalContact | undefined>>;
  contactInfo: PortalContact | undefined;
  setPhoneTypeList: Dispatch<SetStateAction<PhoneList[]>>;
  phoneTypeList: PhoneList[];
  statusCode: Map<string, number>;
  setStatusCode: Dispatch<SetStateAction<Map<string, number>>>;
  statusCodeAccAccess: Map<string, number>;
  setStatusCodeAccAccess: Dispatch<SetStateAction<Map<string, number>>>;
  levelOpts: Map<string, number>;
  setLevelOpts: Dispatch<SetStateAction<Map<string, number>>>;

  // state to validate fields from location
  addrLocationError: boolean;
  setAddrLocationError: Dispatch<SetStateAction<boolean>>;
  addrContLocationError: boolean;
  setAddrContLocationError: Dispatch<SetStateAction<boolean>>;
  cityLocationError: boolean;
  setCityLocationError: Dispatch<SetStateAction<boolean>>;
  stateLocationError: boolean;
  setStateLocationError: Dispatch<SetStateAction<boolean>>;
  zipCodeLocationError: boolean;
  setZipCodeLocationError: Dispatch<SetStateAction<boolean>>;
  countryLocationError: boolean;
  setCountryLocationError: Dispatch<SetStateAction<boolean>>;
  // state to validate fields from contact
  addrContactError: boolean;
  setAddrContactError: Dispatch<SetStateAction<boolean>>;
  addrContContactError: boolean;
  setAddrContContactError: Dispatch<SetStateAction<boolean>>;
  cityContactError: boolean;
  setCityContactError: Dispatch<SetStateAction<boolean>>;
  stateContactError: boolean;
  setStateContactError: Dispatch<SetStateAction<boolean>>;
  zipCodeContactError: boolean;
  setZipCodeContactError: Dispatch<SetStateAction<boolean>>;
  countryContactError: boolean;
  setCountryContactError: Dispatch<SetStateAction<boolean>>;
  // state to validate fields from basic info
  // anchor elements from contact tab
  setEmailAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  emailAnchor: HTMLElement | null;
  setManageInfoAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  manageInfoAnchor: HTMLElement | null;
  setAddressAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  addressAnchor: HTMLElement | null;
  setPhoneAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  phoneAnchor: HTMLElement | null;
  editingContact: PortalContact | undefined;
  setEditingContact: Dispatch<SetStateAction<PortalContact | undefined>>;
  // assets state
  yearsAcquired: DropDownItem[];
  setYearsAcquired: Dispatch<SetStateAction<DropDownItem[]>>;
  changeReason: DropDownItem[];
  setChangeReason: Dispatch<SetStateAction<DropDownItem[]>>;
  assetsCategoryList: AssetCategory[];
  setAssetsCategoryList: Dispatch<SetStateAction<AssetCategory[]>>;
  setAllAssetsCategoryList: Dispatch<SetStateAction<AssetCategory[]>>;
  allAssetsCategoryList: AssetCategory[];
  assetsList: Asset[];
  setAssetsList: Dispatch<SetStateAction<Asset[]>>;
  setAssetToUpdate: Dispatch<React.SetStateAction<Asset | undefined>>;
  assetToUpdate: Asset | undefined;
  // improvement states
  setImprovementToUpdate: Dispatch<React.SetStateAction<Asset | undefined>>;
  improvementToUpdate: Asset | undefined;
  improvementTypeList: AssetCategory[];
  setImprovementTypeList: Dispatch<SetStateAction<AssetCategory[]>>;

  addressList: PortalAddress[];
  setAddressList: Dispatch<SetStateAction<PortalAddress[]>>;

  phoneList: PortalPhone[];
  setPhoneList: Dispatch<SetStateAction<PortalPhone[]>>;
  emailList: PortalEmail[];
  setEmailList: Dispatch<SetStateAction<PortalEmail[]>>;
}
