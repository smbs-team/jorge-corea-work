// provider.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { OptionSet } from 'models/optionSet';
import { PersonalProperty } from 'models/personalProperty';
import { PersonalPropertyHistory } from 'models/personalPropertyHistory';
import { QuickCollect } from 'models/quickCollect';
import React, { PropsWithChildren, useState } from 'react';
import { ChangedBusinessContext } from '.';
import { DropDownItem } from '@ptas/react-public-ui-library';

const ChangedBusinessProvider = ({
  children,
}: PropsWithChildren<{}>): JSX.Element => {
  const [currentBusiness, setCurrentBusiness] = useState<
    PersonalProperty | undefined
  >();
  const [reasonForSaleOpts, setReasonForSaleOpts] = useState<OptionSet[]>([]);
  const [methodTransferOpts, setMethodTransferOpts] = useState<OptionSet[]>([]);
  const [dispositionAssetsOpts, setDispositionAssetsOpts] = useState<
    OptionSet[]
  >([]);
  const [quickCollects, setQuickCollects] = useState<Map<string, QuickCollect>>(
    new Map()
  );
  const [businessHistories, setBusinessHistories] = useState<
    Map<string, PersonalPropertyHistory>
  >(new Map());
  const [countryOptions, setCountryOptions] = useState<DropDownItem[]>([]);
  const [initTabName, setInitTabName] = useState<string>('');
  const [movedTabOn, setMovedTabOn] = useState<boolean>(false);
  const [soldTabOn, setSoldTabOn] = useState<boolean>(false);
  const [closedTabOn, setClosedTabOn] = useState<boolean>(false);
  const [businessTypes, setBusinessTypes] = useState<DropDownItem[]>([]);
  const [stateOfIncorps, setStateOfIncorps] = useState<DropDownItem[]>([]);
  const [formIsInvalid, setFormIsInvalid] = useState<boolean>(false);

  return (
    <ChangedBusinessContext.Provider
      value={{
        currentBusiness,
        setCurrentBusiness,
        reasonForSaleOpts,
        setReasonForSaleOpts,
        methodTransferOpts,
        setMethodTransferOpts,
        dispositionAssetsOpts,
        setDispositionAssetsOpts,
        quickCollects,
        setQuickCollects,
        businessHistories,
        setBusinessHistories,
        countryOptions,
        setCountryOptions,
        initTabName,
        setInitTabName,
        movedTabOn,
        setMovedTabOn,
        soldTabOn,
        setSoldTabOn,
        closedTabOn,
        setClosedTabOn,
        businessTypes,
        setBusinessTypes,
        stateOfIncorps,
        setStateOfIncorps,
        formIsInvalid,
        setFormIsInvalid,
      }}
    >
      {children}
    </ChangedBusinessContext.Provider>
  );
};

export default ChangedBusinessProvider;
