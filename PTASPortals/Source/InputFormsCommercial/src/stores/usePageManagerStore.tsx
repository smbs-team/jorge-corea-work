// usePageManagerStore.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import create from 'zustand';
import { createTrackedSelector } from 'react-tracked';
import {
  GenericPage,
  GenericListObject,
  DropDownItem,
} from '@ptas/react-ui-library';
import { SectionUseList } from './useSectionUseStore';
import { find } from 'lodash';
import {
  addAfter,
  applyAdjustmentFactor,
  applyFloatingHomeAdjustmentFactor,
  applyDollarOrPercentAdjustmentFactor,
} from 'utils';
import createLeaseRateGridSlice, {
  LeaseRateGridSlice,
} from './gridSlices/defaultArea/createLeaseRateGridSlice';
import createVacancyGridSlice, {
  VacancyGridSlice,
} from './gridSlices/defaultArea/createVacancyGridSlice';
import createOpExpPercentGridSlice, {
  OpExpPercentGridSlice,
} from './gridSlices/defaultArea/createOpExpPercentGridSlice';
import createOpExpDollarGridSlice, {
  OpExpDollarGridSlice,
} from './gridSlices/defaultArea/createOpExpDollarGridSlice';
import createCapRatesGridSlice, {
  CapRatesGridSlice,
} from './gridSlices/defaultArea/createCapRatesGridSlice';
import createValuesGridSlice, {
  ValuesGridSlice,
} from './gridSlices/defaultArea/createValuesGridSlice ';
import createPrevDollarGridSlice, {
  PrevDollarGridSlice,
} from './gridSlices/defaultArea/createPrevDollarGridSlice';
import createPrevPercentGridSlice, {
  PrevPercentGridSlice,
} from './gridSlices/defaultArea/createPrevPercentGridSlice';
import createMajorCapRatesGridSlice, {
  MajorCapRatesGridSlice,
} from './gridSlices/majorOffice/createMajorCapRatesGridSlice';
import createMajorLeaseRateGridSlice, {
  MajorLeaseRateGridSlice,
} from './gridSlices/majorOffice/createMajorLeaseRateGridSlice';
import createMajorOpExpDollarGridSlice, {
  MajorOpExpDollarGridSlice,
} from './gridSlices/majorOffice/createMajorOpExpDollarGridSlice';
import createMajorOpExpPercentGridSlice, {
  MajorOpExpPercentGridSlice,
} from './gridSlices/majorOffice/createMajorOpExpPercentGridSlice';
import createMajorPrevDollarGridSlice, {
  MajorPrevDollarGridSlice,
} from './gridSlices/majorOffice/createMajorPrevDollarGridSlice';
import createMajorPrevPercentGridSlice, {
  MajorPrevPercentGridSlice,
} from './gridSlices/majorOffice/createMajorPrevPercentGridSlice';
import createMajorVacancyGridSlice, {
  MajorVacancyGridSlice,
} from './gridSlices/majorOffice/createMajorVacancyGridSlice';
import createMajorValuesGridSlice, {
  MajorValuesGridSlice,
} from './gridSlices/majorOffice/createMajorValuesGridSlice ';
import createMajorBaseYearsSlice, {
  MajorBaseYearsSlice,
} from './gridSlices/majorOffice/createMajorBaseYearsGridSlice';
import createAverageRoomRateGridSlice, {
  AverageRoomRateGridSlice,
} from './gridSlices/hotels/createAverageRoomRateGridSlice';
import createOccupancyRateGridSlice, {
  OccupancyRateGridSlice,
} from './gridSlices/hotels/createOccupancyRateGridSlice';
import createHotelsCapRateGridSlice, {
  HotelsCapRateGridSlice,
} from './gridSlices/hotels/createHotelsCapRateGridSlice';
import createBaseInputsGridSlice, {
  BaseInputsGridSlice,
} from './gridSlices/hotels/createBaseInputsGridSlice';
import createHostsGridSlice, {
  HostsGridSlice,
} from './gridSlices/hotels/createHostsGridSlice';
import createSlipValuesGridSlice, {
  SlipValuesGridSlice,
} from './gridSlices/floatingHome/createSlipValuesGridSlice';
import createSlipPrevYearDollarSlice, {
  SlipPrevYearDollarSlice,
} from './gridSlices/floatingHome/createSlipPrevYearDollarSlice';
import createSlipPrevYearPercentSlice, {
  SlipPrevYearPercentSlice,
} from './gridSlices/floatingHome/createSlipPrevYearPercentSlice';
import createReplacementCostGridSlice, {
  ReplacementCostGridSlice,
} from './gridSlices/floatingHome/createReplacementCostGridSlice';
import createReplacementCostPrevYearDollarGridSlice, {
  ReplacementCostPrevYearDollarGridSlice,
} from './gridSlices/floatingHome/createReplacementCostPrevYearDollarGridSlice';
import createReplacementCostPrevYearPercentGridSlice, {
  ReplacementCostPrevYearPercentGridSlice,
} from './gridSlices/floatingHome/createReplacementCostPrevYearPercentGridSlice';
import { Area } from 'hooks/useGetArea';

export type ValueModifier = 'dollar' | 'percent';
export type StratificationType = 'buildingQuality' | 'leasingClass';

export interface PtasYear {
  PtasYearid: string;
  PtasName: string;
  PtasIscurrentassessmentyear: boolean | null;
  PtasIspreviousassessmentyear: boolean | null;
  PtasIsnextassessmentyear: boolean | null;
}

export interface AssessmentYear extends DropDownItem {
  isCurrent: boolean | null;
  isPrevious: boolean | null;
  isNext: boolean | null;
}

interface Props<T> {
  selectedPage: GenericPage<T>;
  setSelectedPage: (page: GenericPage<T>) => void;
  pages: GenericPage<T>[];
  setPages: (pages: GenericPage<T>[]) => void;
  pageName: string;
  setPageName: (pageName: string) => void;
  currentPage: number;
  setCurrentPage: (currentPage: number) => void;
  isReady: boolean;
  setIsReady: (isReady: boolean) => void;
  selectedSectionUses: GenericListObject<SectionUseList>[];
  setSelectedSectionUses: (
    selectedSectionUses: GenericListObject<SectionUseList>[]
  ) => void;
  minSf: string;
  setMinSf: (minSf: string) => void;
  maxSf: string;
  setMaxSf: (maxSf: string) => void;
  searchErrorMessage: string | undefined;
  setSearchErrorMessage: (searchErrorMessage: string | undefined) => void;
  selectedYear: AssessmentYear | null;
  setSelectedYear: (selectedYear: AssessmentYear | null) => void;
  validated: boolean;
  setValidated: (validated: boolean) => void;
  operatingExpenses: ValueModifier;
  setOperatingExpenses: (operatingExpense: ValueModifier) => void;
  stratificationType: StratificationType;
  setStratificationType: (stratificationType: StratificationType) => void;
  years: AssessmentYear[];
  setYears: (years: AssessmentYear[]) => void;
  leaseRate: ValueModifier;
  setLeaseRate: (leaseRate: ValueModifier) => void;
  clonePages: GenericPage<T>[];
  currentPageBeforeFiltering: number;
  isFilterActive: boolean;
  message: string | null;
  pageCount: number;
  autoCalculate: boolean;
  setAutoCalculate: (autoCalculate: boolean) => void;
  copySectionUses: boolean;
  hotelNbhd: DropDownItem | null;
  leaseRateAf: string;
  setLeaseRateAf: (leaseRateAf: string) => void;
  vacancyAf: string;
  setVacancyAf: (vacancyAf: string, area: Area) => void;
  opExpensesAf: string;
  setOpExpensesAf: (opExpensesAf: string, area: Area) => void;
  capRate: string;
  setCapRate: (capRate: string, area: Area) => void;
  slipValueAf: string;
  setSlipValueAf: (slipValueAf: string) => void;
  replacementCostAf: string;
  setReplacementCostAf: (replacementCostAf: string) => void;
  setHotelNbhd: (hotelNbhd: DropDownItem | null) => void;
  setCopySectionUses: (copySectionUses: boolean) => void;
  addNewPage: () => void;
  addSectionUse: (sectionUse: GenericListObject<SectionUseList>) => void;
  deleteCurrentPage: () => void;
  filterBySectionUse: (filter: string) => void;
  refreshPage: (page: GenericPage<T>) => void;
  reset: () => void;
  updatePage: () => void;
  updateAndValidate: () => void;
  refresh: () => void;
  leaseRateGrid: LeaseRateGridSlice;
  vacancyGrid: VacancyGridSlice;
  opExpPercentGrid: OpExpPercentGridSlice;
  opExpDollarGrid: OpExpDollarGridSlice;
  capRatesGrid: CapRatesGridSlice;
  valuesGrid: ValuesGridSlice;
  prevDollarGrid: PrevDollarGridSlice;
  prevPercentGrid: PrevPercentGridSlice;
  majorCapRatesGrid: MajorCapRatesGridSlice;
  majorLeaseRateGrid: MajorLeaseRateGridSlice;
  majorOpExpDollarGrid: MajorOpExpDollarGridSlice;
  majorOpExpPercentGrid: MajorOpExpPercentGridSlice;
  majorPrevDollarGrid: MajorPrevDollarGridSlice;
  majorPrevPercentGrid: MajorPrevPercentGridSlice;
  majorVacancyGrid: MajorVacancyGridSlice;
  majorValuesGrid: MajorValuesGridSlice;
  majorBaseYearsGrid: MajorBaseYearsSlice;
  averageRoomRateGrid: AverageRoomRateGridSlice;
  occupancyRateGrid: OccupancyRateGridSlice;
  hotelsCapRateGrid: HotelsCapRateGridSlice;
  baseInputsGrid: BaseInputsGridSlice;
  hostsGrid: HostsGridSlice;
  slipValuesGrid: SlipValuesGridSlice;
  slipPrevYearDollarGrid: SlipPrevYearDollarSlice;
  slipPrevYearPercentGrid: SlipPrevYearPercentSlice;
  replacementCostGrid: ReplacementCostGridSlice;
  replacementCostPrevYearDollarGrid: ReplacementCostPrevYearDollarGridSlice;
  replacementCostPrevYearPercentGrid: ReplacementCostPrevYearPercentGridSlice;
}

export interface Page {
  id: number;
  pageName?: string;
  selectedSectionUses: GenericListObject<SectionUseList>[];
  maxSf?: string;
  minSf?: string;
  operatingExpenses: ValueModifier;
  stratificationType: StratificationType;
  leaseRate: ValueModifier;
  autoCalculate: boolean;
  copySectionUses: boolean;
  hotelNbhd: DropDownItem | null;
  leaseRateAf?: string;
  vacancyAf?: string;
  opExpensesAf?: string;
  capRate?: string;
  slipValueAf?: string;
  replacementCostAf?: string;
}

const FIRST_PAGE: GenericPage<Page> = {
  id: 1,
  pageName: 'My page 1',
  isReady: false,
  selectedSectionUses: [],
  operatingExpenses: 'dollar',
  leaseRate: 'dollar',
  copySectionUses: false,
  autoCalculate: false,
  hotelNbhd: null,
  stratificationType: 'buildingQuality',
  leaseRateAf: '0',
  vacancyAf: '0',
  opExpensesAf: '0',
  capRate: '0',
  slipValueAf: '0',
  replacementCostAf: '0',
};

export type PageManagerStoreProps = Props<Page>;

const useStore = create<PageManagerStoreProps>((set, get) => ({
  selectedPage: FIRST_PAGE,
  pages: [FIRST_PAGE],
  clonePages: [],
  searchErrorMessage: undefined,
  autoCalculate: false,
  setAutoCalculate: (autoCalculate) => set({ autoCalculate }),
  copySectionUses: false,
  setCopySectionUses: (copySectionUses) => set({ copySectionUses }),
  pageName: 'My page 1',
  setPageName: (pageName) => set({ pageName }),
  years: [],
  setYears: (years) => set({ years }),
  setSearchErrorMessage: (searchErrorMessage) => set({ searchErrorMessage }),
  currentPageBeforeFiltering: 1,
  isFilterActive: false,
  setSelectedPage: (selectedPage) => set({ selectedPage }),
  setPages: (pages) => set({ pages }),
  currentPage: 1,
  setCurrentPage: (currentPage) => set({ currentPage }),
  addNewPage: () => {
    const newPage = {
      ...FIRST_PAGE,
      id: get().pageCount + 1,
      pageName: `My page ${get().pageCount + 1}`,
      selectedSectionUses: get().copySectionUses
        ? get().selectedSectionUses
        : [],
    };

    //If filter is active, get the clone pages selected pages index
    // to use for insertion into the original array (clonePages)
    let filterPageIndex = 0;
    if (get().isFilterActive) {
      filterPageIndex = get().clonePages.findIndex(
        (p) => p.id === get().selectedPage.id
      );
    }

    set((state) => ({
      pages: addAfter(state.pages, state.currentPage, newPage),
      pageCount: state.pageCount + 1,
      clonePages: addAfter(
        state.clonePages,
        state.isFilterActive ? filterPageIndex + 1 : state.currentPage,
        newPage
      ),
    }));

    set((state) => ({
      currentPage: state.currentPage + 1,
    }));

    get().updatePage();
    get().refreshPage(get().pages[get().currentPage - 1]);
  },
  updatePage: () => {
    if (!get().selectedPage) {
      console.error('Selected page', get().selectedPage);
      return;
    }

    const page: GenericPage<Page> = {
      id: get().selectedPage.id,
      pageName: get().pageName,
      selectedSectionUses: get().selectedSectionUses,
      isReady: get().isReady,
      maxSf: get().maxSf,
      minSf: get().minSf,
      operatingExpenses: get().operatingExpenses,
      leaseRate: get().leaseRate,
      copySectionUses: get().copySectionUses,
      autoCalculate: get().autoCalculate,
      hotelNbhd: get().hotelNbhd,
      stratificationType: get().stratificationType,
      leaseRateAf: get().leaseRateAf,
      vacancyAf: get().vacancyAf,
      opExpensesAf: get().opExpensesAf,
      capRate: get().capRate,
      slipValueAf: get().slipValueAf,
      replacementCostAf: get().replacementCostAf,
    };

    let updatedArray: GenericPage<Page>[] = [];
    let pagesWithFilter: GenericPage<Page>[] = [];

    if (get().isFilterActive) {
      updatedArray = get().clonePages.map((p) => (p.id === page.id ? page : p));
      pagesWithFilter = get().pages.map((p) => (p.id === page.id ? page : p));
    } else {
      updatedArray = get().pages.map((p) => (p.id === page.id ? page : p));
    }

    set({
      pages: get().isFilterActive ? pagesWithFilter : updatedArray,
      clonePages: updatedArray,
    });
  },
  updateAndValidate: () => {
    get().updatePage();

    const notReadyPages = get().pages.filter((p) => p.isReady === false);

    if (notReadyPages.length > 1) {
      set({
        message: `Pages ${notReadyPages
          .flatMap((p) => p.id)
          .join(',')} have not been marked 'ready for valuation'`,
        validated: true,
      });
      return;
    }

    if (notReadyPages.length === 1) {
      set({
        message: `Page ${notReadyPages[0].id} has not been marked 'ready for valuation'`,
        validated: true,
      });
      return;
    }

    set({ message: null, validated: true });
  },
  deleteCurrentPage: () => {
    const currentPage = get().currentPage;
    let filteredPages: GenericPage<Page>[] = [];
    let pagesWithFilter: GenericPage<Page>[] = [];

    if (!get().isFilterActive && get().pages.length === 1) {
      set({ pages: [FIRST_PAGE] });
      get().refreshPage(FIRST_PAGE);
      return;
    }

    if (get().isFilterActive) {
      filteredPages = get().clonePages.filter(
        (p) => p.id !== get().selectedPage.id
      );
      pagesWithFilter = get().pages.filter(
        (p) => p.id !== get().selectedPage.id
      );
    } else {
      filteredPages = get().pages.filter((p) => p.id !== get().selectedPage.id);
    }

    set({
      pages: get().isFilterActive
        ? pagesWithFilter.length
          ? pagesWithFilter
          : filteredPages
        : filteredPages,
      clonePages: filteredPages,
      currentPage: currentPage === 1 ? currentPage : currentPage - 1,
      isFilterActive: pagesWithFilter.length ? true : false,
    });
    const page = get().pages.find(
      (p) => p.id === get().pages[get().currentPage - 1].id
    );
    if (page) get().refreshPage(page);
  },
  refreshPage: (page) => {
    set({
      pageName: page.pageName ?? 'My page X',
      selectedPage: page,
      isReady: page.isReady,
      selectedSectionUses: page.selectedSectionUses,
      maxSf: page.maxSf ?? '',
      minSf: page.minSf ?? '',
      operatingExpenses: page.operatingExpenses,
      leaseRate: page.leaseRate,
      autoCalculate: page.autoCalculate,
      copySectionUses: page.copySectionUses,
      hotelNbhd: page.hotelNbhd,
      stratificationType: page.stratificationType,
      leaseRateAf: page.leaseRateAf ?? '0',
      vacancyAf: page.vacancyAf ?? '0',
      opExpensesAf: page.opExpensesAf ?? '0',
      capRate: page.capRate ?? '0',
      slipValueAf: page.slipValueAf ?? '0',
      replacementCostAf: page.replacementCostAf ?? '0',
    });
  },
  refresh: () => {
    set({ currentPage: 1, selectedPage: get().pages[0] });
    get().refreshPage(get().pages[0]);
    get().updatePage();
  },
  reset: () => {
    set(state => ({
      pages: [FIRST_PAGE],
      clonePages: [FIRST_PAGE],
      minSf: '',
      maxSf: '',
      selectedSectionUses: [],
      selectedYear: state.years.find(y => y.isCurrent) ?? null,
      isReady: false,
      operatingExpenses: 'dollar',
      leaseRate: 'dollar',
      currentPage: 1,
      pageName: 'My page 1',
      copySectionUses: false,
      autoCalculate: false,
      hotelNbhd: null,
      stratificationType: 'buildingQuality',
      leaseRateAf: '0',
      vacancyAf: '0',
      opExpensesAf: '0',
      capRate: '0',
      slipValueAf: '0',
      replacementCostAf: '0',
    }));
  },
  filterBySectionUse: (filter: string) => {
    get().updatePage();

    if (!filter && get().isFilterActive) {
      get().updatePage();
      const isPageAvailable =
        get().currentPageBeforeFiltering <= get().clonePages.length;
      set({
        pages: get().clonePages,
        isFilterActive: false,
        currentPage: isPageAvailable ? get().currentPageBeforeFiltering : 1,
      });
      get().refreshPage(
        get().pages[isPageAvailable ? get().currentPageBeforeFiltering - 1 : 0]
      );
      return;
    }

    const filterResult = get().pages.filter((p) =>
      p.selectedSectionUses.find((s) => {
        return s.value.toString().toLowerCase().includes(filter.toLowerCase());
      })
    );

    if (filterResult.length) {
      const currentPage = get().currentPage;
      set({
        pages: filterResult,
        isFilterActive: true,
        currentPage: 1,
        currentPageBeforeFiltering: currentPage,
        searchErrorMessage: undefined,
      });
      get().refreshPage(get().pages[0]);
    } else {
      set({ searchErrorMessage: 'Section not found' });
    }
  },
  pageCount: 1,
  isReady: false,
  setIsReady: (isReady) => set({ isReady }),
  selectedSectionUses: [],
  setSelectedSectionUses: (selectedSectionUses) => set({ selectedSectionUses }),
  addSectionUse: (sectionUse) => {
    if (find(get().selectedSectionUses, sectionUse)) {
      set({
        selectedSectionUses: get().selectedSectionUses.filter(
          (s) => s.key !== sectionUse.key
        ),
      });
    } else {
      set({ selectedSectionUses: [...get().selectedSectionUses, sectionUse] });
    }
  },
  minSf: '',
  setMinSf: (minSf) => set({ minSf }),
  maxSf: '',
  setMaxSf: (maxSf) => set({ maxSf }),
  operatingExpenses: 'dollar',
  setOperatingExpenses: (operatingExpenses) => set({ operatingExpenses, opExpensesAf: "0" }),
  opExpensesAf: '0',
  setOpExpensesAf: (opExpensesAf, area) => {
    if (area === 'commercial') {
      if (get().operatingExpenses === 'percent') {
        set((state) => ({
          opExpensesAf,
          opExpPercentGrid: {
            ...state.opExpPercentGrid,
            data: applyDollarOrPercentAdjustmentFactor(
              get().opExpPercentGrid.initialData,
              opExpensesAf,
              "percent",
              ['minEff', 'maxEff']
            ),
          },
        }));
      } else {
        set((state) => ({
          opExpensesAf,
          opExpDollarGrid: {
            ...state.opExpDollarGrid,
            data: applyDollarOrPercentAdjustmentFactor(
              get().opExpDollarGrid.initialData,
              opExpensesAf,
              "dollar",
              ['minEff', 'maxEff']
            ),
          },
        }));
      }
    }

    if (area === 'majorOffice') {
      if (get().operatingExpenses === 'percent') {
        set((state) => ({
          opExpensesAf,
          majorOpExpPercentGrid: {
            ...state.majorOpExpPercentGrid,
            data: applyDollarOrPercentAdjustmentFactor(
              get().majorOpExpPercentGrid.initialData,
              opExpensesAf,
              "percent",
              ['empty']
            ),
          },
        }));
      } else {
        set((state) => ({
          opExpensesAf,
          majorOpExpDollarGrid: {
            ...state.majorOpExpDollarGrid,
            data: applyDollarOrPercentAdjustmentFactor(
              get().majorOpExpDollarGrid.initialData,
              opExpensesAf,
              "dollar",
              ['empty']
            ),
          },
        }));
      }
    }
  },
  stratificationType: 'buildingQuality',
  setStratificationType: (stratificationType) => set({ stratificationType }),
  leaseRate: 'dollar',
  setLeaseRate: (leaseRate) => set({ leaseRate }),
  selectedYear: null,
  setSelectedYear: (selectedYear) => set({ selectedYear }),
  message: null,
  validated: false,
  setValidated: (validated) => set({ validated }),
  hotelNbhd: null,
  setHotelNbhd: (hotelNbhd) => set({ hotelNbhd }),
  leaseRateAf: '0',
  setLeaseRateAf: (leaseRateAf) => set({ leaseRateAf }),
  vacancyAf: '0',
  setVacancyAf: (vacancyAf, area) => {
    if (area === 'commercial') {
      set((state) => ({
        vacancyAf,
        vacancyGrid: {
          ...state.vacancyGrid,
          data: applyAdjustmentFactor(
            get().vacancyGrid.initialData,
            vacancyAf,
            ['minEff', 'maxEff']
          ),
        },
      }));
    }

    if (area === 'majorOffice') {
      set((state) => ({
        vacancyAf,
        majorVacancyGrid: {
          ...state.majorVacancyGrid,
          data: applyAdjustmentFactor(
            get().majorVacancyGrid.initialData,
            vacancyAf,
            ['empty']
          ),
        },
      }));
    }
  },
  capRate: '0',
  setCapRate: (capRate, area) => {
    if (area === 'commercial') {
      set((state) => ({
        capRate,
        capRatesGrid: {
          ...state.capRatesGrid,
          data: applyAdjustmentFactor(get().capRatesGrid.initialData, capRate, [
            'minEff',
            'maxEff',
          ]),
        },
      }));
    }

    if (area === 'majorOffice') {
      set((state) => ({
        capRate,
        majorCapRatesGrid: {
          ...state.majorCapRatesGrid,
          data: applyAdjustmentFactor(
            get().majorCapRatesGrid.initialData,
            capRate,
            ['minEff', 'maxEff']
          ),
        },
      }));
    }
  },
  slipValueAf: '0',
  setSlipValueAf: (slipValueAf) => {
    set((state) => ({
      slipValueAf,
      slipValuesGrid: {
        ...state.slipValuesGrid,
        data: applyFloatingHomeAdjustmentFactor(
          get().slipValuesGrid.initialData,
          slipValueAf,
          ['empty']
        ),
      },
    }));
  },
  replacementCostAf: '0',
  setReplacementCostAf: (replacementCostAf) => {
    set((state) => ({
      replacementCostAf,
      replacementCostGrid: {
        ...state.replacementCostGrid,
        data: applyFloatingHomeAdjustmentFactor(
          get().replacementCostGrid.initialData,
          replacementCostAf,
          ['empty']
        ),
      },
    }));
  },
  leaseRateGrid: { ...createLeaseRateGridSlice(set, get) },
  vacancyGrid: { ...createVacancyGridSlice(set, get) },
  opExpPercentGrid: { ...createOpExpPercentGridSlice(set, get) },
  opExpDollarGrid: { ...createOpExpDollarGridSlice(set, get) },
  capRatesGrid: { ...createCapRatesGridSlice(set, get) },
  valuesGrid: { ...createValuesGridSlice(set, get) },
  prevDollarGrid: { ...createPrevDollarGridSlice(set, get) },
  prevPercentGrid: { ...createPrevPercentGridSlice(set, get) },
  majorCapRatesGrid: { ...createMajorCapRatesGridSlice(set, get) },
  majorLeaseRateGrid: { ...createMajorLeaseRateGridSlice(set, get) },
  majorOpExpDollarGrid: { ...createMajorOpExpDollarGridSlice(set, get) },
  majorOpExpPercentGrid: { ...createMajorOpExpPercentGridSlice(set, get) },
  majorPrevDollarGrid: { ...createMajorPrevDollarGridSlice(set, get) },
  majorPrevPercentGrid: { ...createMajorPrevPercentGridSlice(set, get) },
  majorVacancyGrid: { ...createMajorVacancyGridSlice(set, get) },
  majorValuesGrid: { ...createMajorValuesGridSlice(set, get) },
  majorBaseYearsGrid: { ...createMajorBaseYearsSlice(set, get) },
  averageRoomRateGrid: { ...createAverageRoomRateGridSlice(set, get) },
  occupancyRateGrid: { ...createOccupancyRateGridSlice(set, get) },
  hotelsCapRateGrid: { ...createHotelsCapRateGridSlice(set, get) },
  baseInputsGrid: { ...createBaseInputsGridSlice(set, get) },
  hostsGrid: { ...createHostsGridSlice(set, get) },
  slipValuesGrid: { ...createSlipValuesGridSlice(set, get) },
  slipPrevYearDollarGrid: { ...createSlipPrevYearDollarSlice(set, get) },
  slipPrevYearPercentGrid: { ...createSlipPrevYearPercentSlice(set, get) },
  replacementCostGrid: { ...createReplacementCostGridSlice(set, get) },
  replacementCostPrevYearDollarGrid: {
    ...createReplacementCostPrevYearDollarGridSlice(set, get),
  },
  replacementCostPrevYearPercentGrid: {
    ...createReplacementCostPrevYearPercentGridSlice(set, get),
  },
}));

const usePageManagerStore = createTrackedSelector(useStore);

export default usePageManagerStore;
