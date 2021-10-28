// ViewTabs.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  Fragment,
  useEffect,
  useState,
  useContext,
  useCallback,
} from 'react';
import { CustomTabs, CustomButton } from '@ptas/react-public-ui-library';
import useStyles from './styles';
import Move from '../Moved';
import Sale from '../Sold/Sale';
import NewContact from '../Sold/NewContact';
import NewInfo from '../Sold/NewInfo';
import Assets from '../Sold/Assets';
import OldInfo from '../Sold/OldInfo';
import Closure from '../Closed/Closure';
import AssetsClosed from '../Closed/Assets';
import * as fmGeneral from '../../../../../GeneralFormatMessage';
import * as fm from './formatText';
import clsx from 'clsx';
import { AppContext } from 'contexts/AppContext';
import { ChangedBusinessContext } from 'contexts/ChangedBusiness';
import { apiService } from 'services/api/apiService';
import { QuickCollect } from 'models/quickCollect';
import { PersonalPropertyHistory } from 'models/personalPropertyHistory';
import { debounce } from 'lodash';
import { useUpdateEffect } from 'react-use';
import { changedBusinessApiService } from 'services/api/apiService/changedBusiness';

interface Props {
  tabs: string[];
  handleChangeTab?: (tabName: string) => void;
}

function ViewTabs(props: Props): JSX.Element {
  const classes = useStyles();
  const { portalContact } = useContext(AppContext);
  const {
    currentBusiness,
    quickCollects,
    businessHistories,
    movedTabOn,
    soldTabOn,
    closedTabOn,
    initTabName,
    formIsInvalid,
  } = useContext(ChangedBusinessContext);
  const [currentTab, setCurrentTab] = useState<string>(props.tabs[0]);
  const [currentIndexTab, setCurrentIndexTab] = useState<number>(0);
  const [highestStepNumber, setHighestStepNumber] = useState<number>(0);

  const itemsTabs = props.tabs.map((tab, index) => {
    const isDisabled = index === 0 ? false : highestStepNumber < index;
    return {
      label: tab,
      disabled: isDisabled,
    };
  });

  const handleSelectedTab = (tab: number): void => {
    const tabName = props.tabs[tab];
    setCurrentTab(tabName);
    setCurrentIndexTab(tab);
  };

  useEffect(() => {
    if (initTabName) {
      const indexFound = props.tabs.findIndex(t => t === initTabName);
      setCurrentIndexTab(indexFound);
      setCurrentTab(initTabName);
    }
    // eslint-disable-next-line
  }, [initTabName, props.tabs]);

  useEffect(() => {
    props.handleChangeTab?.(currentTab ?? props.tabs[0]);
    // eslint-disable-next-line
  }, [currentTab]);

  useUpdateEffect(() => {
    handleSaveData(currentIndexTab);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [quickCollects, businessHistories]);

  const renderChildrenTabs = (): JSX.Element | void => {
    switch (currentTab) {
      case 'Move':
        return <Move />;
      case 'Sale':
        return <Sale />;
      case 'New contact':
        return <NewContact />;
      case 'New info':
        return <NewInfo />;
      case 'Assets':
        return <Assets />;
      case 'Old info':
        return <OldInfo />;
      case 'Closure':
        return <Closure />;
      case 'Assets/closed':
        return <AssetsClosed />;
      default:
        break;
    }
  };

  const handleContinue = (): void => {
    if (formIsInvalid) return;
    const tabsQty = props.tabs.length - 1;

    if (tabsQty === currentIndexTab) {
      handleEntitySave();
      return;
    }
    setCurrentTab(props.tabs[currentIndexTab + 1]);
    setCurrentIndexTab(prevIndex => prevIndex + 1);
  };

  const handleEntitySave = async (): Promise<void> => {
    const portalContactId = portalContact?.id ?? '';
    const businessId = currentBusiness?.id ?? '';
    if (!portalContactId || !businessId) return;
    const changedTypes = ['SOLD', 'MOVED', 'CLOSED'];
    const savingData = changedTypes.map(type => {
      let dataToReturn = {};
      const quickCollectData = quickCollects.get(type);
      const businessHistData = businessHistories.get(type);
      if (quickCollectData) {
        dataToReturn = {
          quickCollectSave: changedBusinessApiService.saveQuickCollect(
            quickCollectData as QuickCollect
          ),
        };
      }
      if (businessHistData) {
        dataToReturn = {
          ...dataToReturn,
          businessHistSave: changedBusinessApiService.savePersonalPropertyHistory(
            businessHistData as PersonalPropertyHistory
          ),
        };
      }
      //const quickCollectSave = changedBusinessApiService.saveQuickCollect()
      return dataToReturn;
    });
    Promise.all(savingData)
      .then(() => {
        apiService.deleteJson(
          `portals/${portalContactId}/${businessId}/businessChangedStep`
        );
      })
      .catch(error => {
        console.log(error);
      });
  };

  const handleSaveData = async (currenTabIndex: number): Promise<void> => {
    const portalContactId = portalContact?.id ?? '';
    const businessId = currentBusiness?.id ?? '';
    if (!portalContactId || !businessId) return;
    let quickCollectToSave: [string, QuickCollect][] = [];
    quickCollects.forEach((value, key) => {
      quickCollectToSave = [...quickCollectToSave, [key, value]];
    });
    let businessHistToSave: [string, PersonalPropertyHistory][] = [];
    businessHistories.forEach((value, key) => {
      businessHistToSave = [...businessHistToSave, [key, value]];
    });
    const businessChanged = {
      portalContactId,
      businessId,
      quickCollects: quickCollectToSave,
      businessHistories: businessHistToSave,
      currentTabName: props.tabs[currenTabIndex],
      movedTabOn,
      closedTabOn,
      soldTabOn,
    };

    saveJsonService(
      `portals/${portalContactId}/${businessId}/businessChangedStep`,
      businessChanged
    );
  };

  const saveJsonService = useCallback(
    debounce(async (url: string, json: object) => {
      await apiService.saveJson(url, json);
    }, 1000),
    [apiService]
  );

  useEffect(() => {
    setHighestStepNumber(
      currentIndexTab > highestStepNumber ? currentIndexTab : highestStepNumber
    );

    // eslint-disable-next-line
  }, [currentIndexTab]);

  const renderContinueButton = (): JSX.Element => {
    const isLastTab = props.tabs.length - 1 === currentIndexTab;
    const textButton = isLastTab
      ? fm.finishedReportSale
      : fmGeneral.continueTex;

    return (
      <CustomButton
        classes={{
          root: clsx(classes.continue, isLastTab && classes.lastTabButton),
        }}
        onClick={handleContinue}
      >
        {textButton}
      </CustomButton>
    );
  };

  return (
    <Fragment>
      <CustomTabs
        ptasVariant="Switch"
        items={itemsTabs}
        onSelected={handleSelectedTab}
        classes={{
          root: classes.tabs,
        }}
        selectedIndex={currentIndexTab}
      />
      {renderChildrenTabs()}
      {renderContinueButton()}
    </Fragment>
  );
}

export default ViewTabs;
