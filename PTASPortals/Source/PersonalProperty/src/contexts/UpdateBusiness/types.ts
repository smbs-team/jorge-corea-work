// updateBusinessType.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Dispatch, SetStateAction } from 'react';
// import { PersonalProperty as NewBusiness } from 'models/personalProperty';
import {
  DropDownItem,
  ItemSuggestion,
  // CustomFile,
} from '@ptas/react-public-ui-library';
import { PersonalProperty } from 'models/personalProperty';
import {
  PortalAddress,
  PortalContact,
  PortalEmail,
  PortalPhone,
} from 'models/portalContact';
import { AddressLookupEntity } from 'models/addresses';
import { Asset, AssetCategory } from 'models/assets';

export type PhoneList = { value: number; label: string };

export type InputName = 'zipCode' | 'state' | 'city' | 'address';

export interface UseUpdateBusiness {
  setCurrentStepIndex: Dispatch<SetStateAction<number>>;
  currentStepIndex: number;
  setViewChangesOpen: Dispatch<SetStateAction<boolean>>;
  viewChangesOpen: boolean;
  propertyType: DropDownItem[];
  setPropertyType: Dispatch<SetStateAction<DropDownItem[]>>;
  setInitialBusiness: Dispatch<SetStateAction<PersonalProperty>>;
  initialBusiness: PersonalProperty;
  setUpdatedBusiness: Dispatch<SetStateAction<PersonalProperty>>;
  updatedBusiness: PersonalProperty;
  setNaicsNumberSuggestionList: Dispatch<SetStateAction<ItemSuggestion[]>>;
  naicsnumberSuggestionList: ItemSuggestion[];
  setStateOrProvince: Dispatch<SetStateAction<DropDownItem[]>>;
  stateOrProvince: DropDownItem[];
  setOpenAssetForm: Dispatch<SetStateAction<boolean>>;
  openAssetForm: boolean;

  setRemoveAssetAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  removeAssetAnchor: HTMLElement | null;
  setOpenSuggestionAssetModal: Dispatch<SetStateAction<boolean>>;
  openSuggestionAssetModal: boolean;
  // manage address states
  setEditingContact: Dispatch<SetStateAction<PortalContact | undefined>>;
  editingContact: PortalContact | undefined;
  addressList: PortalAddress[];
  setAddressList: Dispatch<SetStateAction<PortalAddress[]>>;
  addressSuggestions: AddressLookupEntity[];
  setAddressSuggestions: Dispatch<SetStateAction<AddressLookupEntity[]>>;
  stateItems: ItemSuggestion[];
  setStateItems: Dispatch<SetStateAction<ItemSuggestion[]>>;
  cityItems: ItemSuggestion[];
  setCityItems: Dispatch<SetStateAction<ItemSuggestion[]>>;
  zipCodeItems: ItemSuggestion[];
  setZipCodeItems: Dispatch<SetStateAction<ItemSuggestion[]>>;
  countryItems: DropDownItem[];
  setCountryItems: Dispatch<SetStateAction<DropDownItem[]>>;
  phoneList: PortalPhone[];
  setPhoneList: Dispatch<SetStateAction<PortalPhone[]>>;
  emailList: PortalEmail[];
  setEmailList: Dispatch<SetStateAction<PortalEmail[]>>;
  phoneTypeList: {
    value: number;
    label: string;
  }[];
  setPhoneTypeList: Dispatch<
    SetStateAction<
      {
        value: number;
        label: string;
      }[]
    >
  >;
  phoneTypes: Map<string, number>;
  setPhoneTypes: Dispatch<SetStateAction<Map<string, number>>>;
  // assets state
  yearsAcquired: DropDownItem[];
  setYearsAcquired: Dispatch<SetStateAction<DropDownItem[]>>;
  changeReason: DropDownItem[];
  setChangeReason: Dispatch<SetStateAction<DropDownItem[]>>;
  assetsCategoryList: AssetCategory[];
  setAssetsCategoryList: Dispatch<SetStateAction<AssetCategory[]>>;
  setAllAssetsCategoryList: Dispatch<SetStateAction<AssetCategory[]>>;
  allAssetsCategoryList: AssetCategory[];
  setAssetsList: Dispatch<SetStateAction<Asset[]>>;
  assetsList: Asset[];
  setAssetToUpdate: Dispatch<React.SetStateAction<Asset | undefined>>;
  assetToUpdate: Asset | undefined;
  setInitialAssets: Dispatch<SetStateAction<Asset[]>>;
  initialAssets: Asset[];
  // improvement
  setImprovementToUpdate: Dispatch<React.SetStateAction<Asset | undefined>>;
  improvementToUpdate: Asset | undefined;
  improvementTypeList: AssetCategory[];
  setImprovementTypeList: Dispatch<SetStateAction<AssetCategory[]>>;
}
