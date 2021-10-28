// useSectionUseStore.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { sortBy, startCase } from 'lodash';
import React from 'react';
import { createTrackedSelector } from 'react-tracked';
import create from 'zustand';
import { get as getData } from 'api/AxiosLoader';

export interface SectionUse {
  id: React.ReactText;
  name: string;
  description?: string;
}

interface GetSectionUses {
  PtasBuildingsectionuseid: string;
  PtasMarshallswiftdescription: string;
  PtasName: string;
  PtasItemid: string;
}

export type SectionUseList = Partial<Pick<SectionUse, 'description'>>;

interface Props {
  sectionUseList: SectionUse[];
  setSectionUseList: (sectionUseList: SectionUse[]) => void;
  getSectionUses: () => Promise<void>;
  isLoading: boolean;
}

const useStore = create<Props>((set) => ({
  isLoading: false,
  sectionUseList: [],
  setSectionUseList: (sectionUseList) => set({ sectionUseList }),
  getSectionUses: async () => {
    let data: GetSectionUses[] = [];
    const sectionUses: SectionUse[] = [];
    set({ isLoading: true });

    try {
      const response = await getData<{ value: GetSectionUses[] }>(
        `${process.env.REACT_APP_API_URL}/PtasBuildingsectionuse?$filter=statecode eq 0 and statuscode eq 1`
      );
      data = response.data.value;
    } catch (error) {
      console.error('Error getting section use list.');
      set({ isLoading: false });
      return;
    }

    data.forEach((item) => {
      sectionUses.push({
        id: item.PtasBuildingsectionuseid,
        name: `${item.PtasItemid} ${startCase(item.PtasName.toLowerCase()).replace(/[0-9]/g,'')}`,
        description: item.PtasMarshallswiftdescription,
      });
    });

    const sortedSectionUses = sortBy(sectionUses, 'name');
    set({
      sectionUseList: sortedSectionUses,
      isLoading: false,
    });
  },
}));

const useSectionUseStore = createTrackedSelector(useStore);

export default useSectionUseStore;
