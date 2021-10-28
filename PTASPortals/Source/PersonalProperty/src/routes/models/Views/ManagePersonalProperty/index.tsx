// ManageBusinessPersonalProperty.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useEffect, useState, useContext } from 'react';
import {
  CustomCard,
  CustomTextButton,
  CustomSwitch,
  CustomButton,
} from '@ptas/react-public-ui-library';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatText';
import * as fmGeneral from '../../../../GeneralFormatMessage';
import useStyles from './styles';
import ViewTabs from './ViewTabs';
import HelpSection from 'components/HelpSection';
import ProfileOptionsButton from 'components/ProfileOptionsButton';
import { CLOSED_TAB_NAMES, MOVED_TAB_NAMES, SOLD_TAB_NAMES } from './constants';
import withChangedBusinessProvider, {
  ChangedBusinessContext,
} from 'contexts/ChangedBusiness';
import { useParams } from 'react-router';
import { businessApiService } from 'services/api/apiService/business';
import { apiService } from 'services/api/apiService';
import { useMount } from 'react-use';
import { dropdownOptionsMapping } from 'utils';
import { AppContext } from 'contexts/AppContext';
import { QuickCollect } from 'models/quickCollect';
import { PersonalPropertyHistory } from 'models/personalPropertyHistory';

export interface View {
  name: string;
  active: boolean;
}

interface RouteParams {
  businessId: string;
}

function ManageBusinessPersonalProperty(): JSX.Element {
  const classes = useStyles();
  const { businessId } = useParams<RouteParams>();
  const { portalContact } = useContext(AppContext);
  const {
    currentBusiness,
    setCurrentBusiness,
    setCountryOptions,
    setQuickCollects,
    setBusinessHistories,
    setInitTabName,
    movedTabOn,
    setMovedTabOn,
    closedTabOn,
    setClosedTabOn,
    soldTabOn,
    setSoldTabOn,
  } = useContext(ChangedBusinessContext);
  const [tabs, setTabs] = useState<string[]>([]);
  const [viewMainElement, setViewMainElement] = useState<boolean>(true);
  const [showTabs, setShowTabs] = useState<boolean>(false);
  const [currentView, setCurrentView] = useState<string>('');

  useMount(() => {
    fetchCountries();
  });

  useEffect(() => {
    if (currentBusiness && portalContact) {
      fetchCurrentStep();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentBusiness, portalContact]);

  const fetchCurrentStep = async (): Promise<void> => {
    const portalContactId = portalContact?.id ?? '';
    const currentBusinessId = currentBusiness?.id ?? '';
    if (!portalContactId || !currentBusinessId) return;
    const { data } = await apiService.getJson(
      `portals/${portalContactId}/${currentBusinessId}/businessChangedStep/businessChangedStep`
    );
    if (data && data.currentTabName) {
      const quickCollectsState = new Map<string, QuickCollect>(
        (data.quickCollects as unknown) as [string, QuickCollect][]
      );
      const businessHistoriesState = new Map<string, PersonalPropertyHistory>(
        (data.businessHistories as unknown) as [
          string,
          PersonalPropertyHistory
        ][]
      );
      quickCollectsState.size && setQuickCollects(quickCollectsState);
      businessHistoriesState.size &&
        setBusinessHistories(businessHistoriesState);

      setInitTabName(data.currentTabName as string);
      setViewMainElement(false);
      setShowTabs(true);
      setMovedTabOn((data.movedTabOn as unknown) as boolean);
      setClosedTabOn((data.closedTabOn as unknown) as boolean);
      setSoldTabOn((data.soldTabOn as unknown) as boolean);
    }
  };

  const fetchCountries = async (): Promise<void> => {
    const { data } = await apiService.getCountries();
    if (!data) return;
    // from country entity to dropdown options
    const countriesFound = dropdownOptionsMapping(
      data,
      'ptas_name',
      'ptas_countryid'
    );
    setCountryOptions(countriesFound);
  };

  const fetchBusiness = async (): Promise<void> => {
    const { data } = await businessApiService.getBusinessById(businessId);
    setCurrentBusiness(data);
  };

  useEffect(() => {
    if (businessId) {
      fetchBusiness();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [businessId]);

  const handleSelect = (isChecked: boolean, name: string | undefined): void => {
    if (!name) return;
    switch (name) {
      case 'moved':
        setMovedTabOn(isChecked);
        break;
      case 'sold':
        setSoldTabOn(isChecked);
        break;
      case 'closed':
        setClosedTabOn(isChecked);
        break;
      default:
        break;
    }
  };

  const handleChangeTabName = (tabName: string): void => {
    const isMoved = MOVED_TAB_NAMES.includes(tabName);
    if (isMoved) return setCurrentView('moved');

    const isSold = SOLD_TAB_NAMES.includes(tabName);
    if (isSold) return setCurrentView('sold');

    const isClosed = CLOSED_TAB_NAMES.includes(tabName);
    if (isClosed) return setCurrentView('closed');
  };

  useEffect(() => {
    const movedTab = movedTabOn ? MOVED_TAB_NAMES : [];
    const soldTab = soldTabOn ? SOLD_TAB_NAMES : [];
    const closedTab = closedTabOn ? CLOSED_TAB_NAMES : [];

    setTabs([...movedTab, ...soldTab, ...closedTab] as string[]);
  }, [movedTabOn, soldTabOn, closedTabOn]);

  const handleContinue = (): void => {
    if (!tabs.length) return;

    setViewMainElement(false);

    setShowTabs(true);
  };

  const renderContinueButton = (): JSX.Element | void => {
    if (!viewMainElement) return;

    return (
      <CustomButton
        classes={{ root: classes.continue }}
        onClick={handleContinue}
      >
        {fmGeneral.continueTex}
      </CustomButton>
    );
  };

  const renderOptions = (): void | JSX.Element => {
    if (!viewMainElement) return;

    return (
      <div className={classes.wrapOpt}>
        <CustomSwitch
          label={fm.moved}
          offLabel={fmGeneral.no}
          onLabel={fmGeneral.yes}
          showOptions
          classes={{
            formControlRoot: classes.switchOption,
          }}
          isChecked={handleSelect}
          name="moved"
        />
        <CustomSwitch
          label={fm.sold}
          offLabel={fmGeneral.no}
          onLabel={fmGeneral.yes}
          showOptions
          classes={{
            formControlRoot: classes.switchOption,
          }}
          isChecked={handleSelect}
          name="sold"
        />
        <CustomSwitch
          label={fm.closed}
          offLabel={fmGeneral.no}
          onLabel={fmGeneral.yes}
          showOptions
          classes={{
            formControlRoot: classes.switchOption,
          }}
          isChecked={handleSelect}
          name="closed"
        />
      </div>
    );
  };

  const renderTitle = (): JSX.Element => {
    switch (currentView) {
      case 'moved':
        return (
          <span className={classes.title}>{fm.reportThisBusinessMove}</span>
        );
      case 'sold':
        return (
          <span className={classes.title}>{fm.reportThisBusinessSale}</span>
        );
      case 'closed':
        return (
          <span className={classes.title}>{fm.reportThisBusinessClosure}</span>
        );
      default:
        return (
          <Fragment>
            <span className={classes.title}>{fm.reportThisBusinessAs}:</span>
            <span className={classes.checkAllToApply}>
              {fm.checkAllThatApply}
            </span>
          </Fragment>
        );
    }
  };

  const renderTabs = (): JSX.Element | void => {
    console.log(tabs);
    if (!tabs.length || !showTabs) return;

    return <ViewTabs tabs={tabs} handleChangeTab={handleChangeTabName} />;
  };

  return (
    <MainLayout>
      <CustomCard
        variant="wrapper"
        classes={{
          rootWrap: classes.card,
          wrapperContent: classes.contentWrap,
        }}
      >
        <div className={classes.head}>
          <CustomTextButton ptasVariant="Clear">
            {fmGeneral.back}
          </CustomTextButton>
          <div className={classes.titlesWrap}>
            <span className={classes.description}>
              {fm.businessPersonalProperty}
            </span>
            <span className={classes.headTitle}>
              {currentBusiness?.businessName ?? ' - '}
            </span>
          </div>
          <ProfileOptionsButton />
        </div>
        <div className={classes.textWrap}>{renderTitle()}</div>
        {/* options */}
        {renderOptions()}
        {/* body */}
        {renderTabs()}
        {renderContinueButton()}
        {/* footer */}
        <HelpSection />
      </CustomCard>
    </MainLayout>
  );
}

export default withChangedBusinessProvider(ManageBusinessPersonalProperty);
