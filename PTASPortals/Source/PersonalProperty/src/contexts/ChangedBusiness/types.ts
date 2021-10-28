// types.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { OptionSet } from 'models/optionSet';
import { ReactNode } from 'react';
import { PersonalPropertyHistory } from 'models/personalPropertyHistory';
import { QuickCollect } from 'models/quickCollect';
import { PersonalProperty } from 'models/personalProperty';
import { DropDownItem } from '@ptas/react-public-ui-library';

export interface ChangedBusinessContextProps {
  currentBusiness: PersonalProperty | undefined;
  setCurrentBusiness: React.Dispatch<
    React.SetStateAction<PersonalProperty | undefined>
  >;
  reasonForSaleOpts: OptionSet[];
  setReasonForSaleOpts: React.Dispatch<React.SetStateAction<OptionSet[]>>;
  methodTransferOpts: OptionSet[];
  setMethodTransferOpts: React.Dispatch<React.SetStateAction<OptionSet[]>>;
  dispositionAssetsOpts: OptionSet[];
  setDispositionAssetsOpts: React.Dispatch<React.SetStateAction<OptionSet[]>>;
  quickCollects: Map<string, QuickCollect>;
  setQuickCollects: React.Dispatch<
    React.SetStateAction<Map<string, QuickCollect>>
  >;
  businessHistories: Map<string, PersonalPropertyHistory>;
  setBusinessHistories: React.Dispatch<
    React.SetStateAction<Map<string, PersonalPropertyHistory>>
  >;
  countryOptions: DropDownItem[];
  setCountryOptions: React.Dispatch<React.SetStateAction<DropDownItem[]>>;
  initTabName: string;
  setInitTabName: React.Dispatch<React.SetStateAction<string>>;
  movedTabOn: boolean;
  setMovedTabOn: React.Dispatch<React.SetStateAction<boolean>>;
  soldTabOn: boolean;
  setSoldTabOn: React.Dispatch<React.SetStateAction<boolean>>;
  closedTabOn: boolean;
  setClosedTabOn: React.Dispatch<React.SetStateAction<boolean>>;
  businessTypes: DropDownItem[];
  setBusinessTypes: React.Dispatch<React.SetStateAction<DropDownItem[]>>;
  stateOfIncorps: DropDownItem[];
  setStateOfIncorps: React.Dispatch<React.SetStateAction<DropDownItem[]>>;
  formIsInvalid: boolean;
  setFormIsInvalid: React.Dispatch<React.SetStateAction<boolean>>;
}

export interface CustomTabOption {
  label: string | ReactNode;
  disabled?: boolean;
}
