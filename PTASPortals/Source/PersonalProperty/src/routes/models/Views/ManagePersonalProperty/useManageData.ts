// useManageData.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { useContext } from 'react';
import { ChangedBusinessContext } from 'contexts/ChangedBusiness';
import { PersonalPropertyHistory } from 'models/personalPropertyHistory';
import { QuickCollect } from 'models/quickCollect';
import { setMapCollection } from 'utils/mapCollection';
//import { CHANGED_ACTION_MOVED, CHANGED_ACTION_CLOSED, CHANGED_ACTION_SOLD } from './constants';

enum ChangedType {
  SOLD = 'sold',
  MOVED = 'moved',
  CLOSED = 'closed',
}
type ChangedTypeStrings = keyof typeof ChangedType;

interface UseManageProps {
  businessHistory: PersonalPropertyHistory | undefined;
  quickCollect: QuickCollect | undefined;
  setBusinessHistory: (
    data:
      | PersonalPropertyHistory
      | ((prev: PersonalPropertyHistory) => PersonalPropertyHistory)
  ) => void;
  setQuickCollect: (
    data: QuickCollect | ((prev: QuickCollect) => QuickCollect)
  ) => void;
}

const useManageData = (changedType: ChangedTypeStrings): UseManageProps => {
  const {
    businessHistories,
    quickCollects,
    setBusinessHistories,
    setQuickCollects,
  } = useContext(ChangedBusinessContext);
  const businessHistory = businessHistories.get(changedType);
  const quickCollect = quickCollects.get(changedType);

  const setBusinessHistory = (
    data:
      | PersonalPropertyHistory
      | ((prev: PersonalPropertyHistory) => PersonalPropertyHistory)
  ): void => {
    if (!data) return;
    setBusinessHistories(prev => {
      return setMapCollection({
        id: changedType as string,
        map: prev,
        value: data,
      });
    });
  };

  const setQuickCollect = (
    data: QuickCollect | ((prev: QuickCollect) => QuickCollect)
  ): void => {
    if (!data) return;
    setQuickCollects(prev => {
      return setMapCollection({
        id: changedType as string,
        map: prev,
        value: data,
      });
    });
  };

  return {
    businessHistory,
    quickCollect,
    setBusinessHistory,
    setQuickCollect,
  };
};

export default useManageData;
