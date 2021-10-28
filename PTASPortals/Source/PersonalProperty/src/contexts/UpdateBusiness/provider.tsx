// NewBusiness.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren, useState } from 'react';

import { DropDownItem, ItemSuggestion } from '@ptas/react-public-ui-library';

import { UpdateBusinessContext } from './index';
import { PersonalProperty } from 'models/personalProperty';
import {
  PortalAddress,
  PortalContact,
  PortalEmail,
  PortalPhone,
} from 'models/portalContact';
import { AddressLookupEntity } from 'models/addresses';
import { Asset, AssetCategory } from 'models/assets';

const UpdateBusinessProvider = ({
  children,
}: PropsWithChildren<{}>): JSX.Element => {
  // index state
  const [currentStepIndex, setCurrentStepIndex] = useState<number>(0);
  const [viewChangesOpen, setViewChangesOpen] = useState(false);
  // basic info state
  const [propertyType, setPropertyType] = useState<DropDownItem[]>([]);
  const [initialBusiness, setInitialBusiness] = useState<PersonalProperty>(
    new PersonalProperty()
  );
  const [updatedBusiness, setUpdatedBusiness] = useState<PersonalProperty>(
    new PersonalProperty()
  );
  const [naicsnumberSuggestionList, setNaicsNumberSuggestionList] = useState<
    ItemSuggestion[]
  >([]);

  const [stateOrProvince, setStateOrProvince] = useState<DropDownItem[]>([]);
  const [openAssetForm, setOpenAssetForm] = useState<boolean>(false);
  const [assetToUpdate, setAssetToUpdate] =
    useState<Asset | undefined>(undefined);
  const [removeAssetAnchor, setRemoveAssetAnchor] =
    useState<HTMLElement | null>(null);
  const [openSuggestionAssetModal, setOpenSuggestionAssetModal] =
    useState<boolean>(false);
  const [editingContact, setEditingContact] =
    useState<PortalContact | undefined>();
  const [addressList, setAddressList] = useState<PortalAddress[]>([]);
  const [addressSuggestions, setAddressSuggestions] = useState<
    AddressLookupEntity[]
  >([]);
  const [stateItems, setStateItems] = useState<ItemSuggestion[]>([]);
  const [cityItems, setCityItems] = useState<ItemSuggestion[]>([]);
  const [zipCodeItems, setZipCodeItems] = useState<ItemSuggestion[]>([]);
  const [countryItems, setCountryItems] = useState<DropDownItem[]>([]);
  const [emailList, setEmailList] = useState<PortalEmail[]>([]);
  const [phoneList, setPhoneList] = useState<PortalPhone[]>([]);
  const [phoneTypeList, setPhoneTypeList] = useState<
    { value: number; label: string }[]
  >([]);
  const [phoneTypes, setPhoneTypes] = useState<Map<string, number>>(new Map());
  const [yearsAcquired, setYearsAcquired] = useState<DropDownItem[]>([]);
  const [changeReason, setChangeReason] = useState<DropDownItem[]>([]);
  const [assetsCategoryList, setAssetsCategoryList] = useState<AssetCategory[]>(
    []
  );

  const [assetsList, setAssetsList] = useState<Asset[]>([]);
  const [initialAssets, setInitialAssets] = useState<Asset[]>([]);

  const [allAssetsCategoryList, setAllAssetsCategoryList] = useState<
    AssetCategory[]
  >([]);

  const [improvementToUpdate, setImprovementToUpdate] =
    useState<Asset | undefined>(undefined);
  const [improvementTypeList, setImprovementTypeList] = useState<
    AssetCategory[]
  >([]);

  return (
    <UpdateBusinessContext.Provider
      value={{
        currentStepIndex,
        setCurrentStepIndex,
        setViewChangesOpen,
        viewChangesOpen,
        propertyType,
        setPropertyType,
        initialBusiness,
        setInitialBusiness,
        setUpdatedBusiness,
        updatedBusiness,
        naicsnumberSuggestionList,
        setNaicsNumberSuggestionList,
        stateOrProvince,
        setStateOrProvince,
        openAssetForm,
        setOpenAssetForm,
        assetToUpdate,
        setAssetToUpdate,
        removeAssetAnchor,
        setRemoveAssetAnchor,
        openSuggestionAssetModal,
        setOpenSuggestionAssetModal,
        editingContact,
        setEditingContact,
        addressList,
        setAddressList,
        addressSuggestions,
        setAddressSuggestions,
        setStateItems,
        stateItems,
        cityItems,
        setCityItems,
        setZipCodeItems,
        zipCodeItems,
        countryItems,
        setCountryItems,
        emailList,
        phoneList,
        setEmailList,
        setPhoneList,
        phoneTypeList,
        setPhoneTypeList,
        phoneTypes,
        setPhoneTypes,
        setYearsAcquired,
        yearsAcquired,
        changeReason,
        setChangeReason,
        assetsCategoryList,
        setAssetsCategoryList,
        assetsList,
        setAssetsList,
        allAssetsCategoryList,
        setAllAssetsCategoryList,
        initialAssets,
        setInitialAssets,
        improvementToUpdate,
        improvementTypeList,
        setImprovementToUpdate,
        setImprovementTypeList,
      }}
    >
      {children}
    </UpdateBusinessContext.Provider>
  );
};

export default UpdateBusinessProvider;
