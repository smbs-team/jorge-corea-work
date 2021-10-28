// useAreaTreeStore.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import create from 'zustand';
import { createTrackedSelector } from 'react-tracked';
import { GenericRow } from '@ptas/react-ui-library';
import { sortBy, uniq } from 'lodash';
import React from 'react';
import { AreaTreeChanges } from 'types/areaTree.interface';
import { get as getData } from 'api/AxiosLoader';

interface CommArea {
  ptasName?: string;
  ptasNbhdNumber?: string;
  ptasAreaNumber?: string;
  entityName?: string;
  queryEntityName: string;
  commercialDistrict?: string;
  areaName?: string;
  description?: string;
  area?: GenericRow<CommArea>;
  nbhdDescription?: string;
}

interface Props {
  data: GenericRow<CommArea>[];
  getData: (ids: React.ReactText[]) => void;
  selectedItem: GenericRow<CommArea> | null;
  setSelectedItem: (selectedItem: GenericRow<CommArea>) => void;
  setData: (data: GenericRow<CommArea>[]) => void;
  isLoading: boolean;
  setIsLoading: (isLoading: boolean) => void;
  expandedRowIds: React.ReactText[];
  setExpandedRowIds: (expandedRowsIds: React.ReactText[]) => void;
  hotelNbhds: string[];
  setHodelNbhds: (hotelsNbhds: string[]) => void;
}

const createRow = (
  id: string,
  name: string,
  parentId: React.ReactText,
  isEmpty?: boolean,
  hasErrors?: boolean
): GenericRow<CommArea> => {
  return {
    id,
    hasItems: false,
    isDirectory: false,
    parentId,
    name,
    queryEntityName: '',
    hasErrors,
    isEmpty,
  };
};

export const getRootFriendlyName = (name: string | undefined): string => {
  if (!name) return 'Geo Area / Specialty Area:';
  return name === 'PtasSpecialtyarea' ? 'Specialty Area:' : 'Geo Area:';
};

const FILTER_QUERY = '$filter=statecode eq 0 and statuscode eq 1';

const useStore = create<Props>((set, get) => ({
  expandedRowIds: [],
  hotelNbhds: [],
  setHodelNbhds: (hotelNbhds) => set({ hotelNbhds }),
  setExpandedRowIds: (expandedRowIds) => set({ expandedRowIds }),
  data: [
    {
      id: '1',
      hasItems: true,
      isDirectory: true,
      name: 'Geo Areas',
      parentId: '',
      queryEntityName: 'PtasGeoarea',
    },
    {
      id: '0',
      hasItems: true,
      isDirectory: true,
      name: 'Specialty Areas',
      parentId: '',
      queryEntityName: 'PtasSpecialtyarea',
    },
  ],
  getData: async (ids) => {
    set({ isLoading: true });
    let data: AreaTreeChanges[] = [];

    const rowId = ids[0] as string;
    const row = get().data.find((row) => row.id === rowId);

    if (!row) return;

    const isRoot =
      row.queryEntityName === 'PtasSpecialtyarea' ||
      row.queryEntityName === 'PtasGeoarea';

    const isSpecialtyAreaRoot = row.queryEntityName === 'PtasSpecialtyarea';
    const isSpecialtyArea = row.queryEntityName === 'PtasSpecialtyneighborhood';

    const areaFilter = isSpecialtyArea
      ? `${FILTER_QUERY} and PtasSpecialtyareaidValue eq ${row.id}`
      : `${FILTER_QUERY} and PtasGeoareaidValue eq ${row.id}`;

    try {
      const response = await getData<{ value: AreaTreeChanges[] }>(
        `${process.env.REACT_APP_API_URL}/${row.queryEntityName}?${
          isRoot ? FILTER_QUERY : areaFilter
        }`
      );
      data = response.data.value;
    } catch (error) {
      set((state) => ({
        data: state.data.concat(
          createRow('e', 'Error fetching data', row.id, false, true)
        ),
      }));
      set({ isLoading: false });
      return;
    }

    if (!data.length) {
      set((state) => ({
        data: state.data.concat(
          createRow('x', 'No data found', row.id, true, false)
        ),
      }));
      return;
    }

    let toAdd: GenericRow<CommArea>[] = [];
    let isMajorOffice = false;
    if (isRoot) {
      data.forEach((item) => {
        toAdd.push({
          id: item.PtasSpecialtyareaid ?? item.PtasGeoareaid,
          name: 'Area ' + item.PtasAreanumber,
          parentId: isSpecialtyAreaRoot ? '0' : '1',
          hasItems: true,
          isDirectory: true,
          ptasName: item.PtasName,
          description: item.PtasDescription,
          ptasAreaNumber: item.PtasAreanumber,
          entityName: isSpecialtyAreaRoot ? 'PtasSpecialtyarea' : 'PtasGeoarea',
          commercialDistrict: item.PtasCommercialdistrict,
          queryEntityName: isSpecialtyAreaRoot
            ? 'PtasSpecialtyneighborhood'
            : 'PtasGeoneighborhood',
        });
      });
      if (!isSpecialtyAreaRoot) {
        toAdd = toAdd.filter(
          (a) =>
            a.ptasAreaNumber !== '012' &&
            a.ptasAreaNumber !== '015' &&
            a.ptasAreaNumber !== '034'
        );
      } else {
        let geoAreas: AreaTreeChanges[] = [];
        try {
          const response = await getData<{ value: AreaTreeChanges[] }>(
            `${process.env.REACT_APP_API_URL}/PtasGeoarea?$filter=statecode eq 0 and statuscode eq 1 and (PtasAreanumber eq '012' or PtasAreanumber eq '015' or PtasAreanumber eq '034')`
          );
          geoAreas = response.data.value;
        } catch (error) {
          set((state) => ({
            data: state.data.concat(
              createRow('e', 'Error fetching data', row.id, false, true)
            ),
          }));
          set({ isLoading: false });
          return;
        }

        geoAreas.forEach((item) => {
          toAdd.push({
            id: item.PtasSpecialtyareaid ?? item.PtasGeoareaid,
            name: 'Area ' + item.PtasAreanumber,
            parentId: '0',
            hasItems: true,
            isDirectory: true,
            ptasName: item.PtasName,
            description: item.PtasDescription,
            ptasAreaNumber: item.PtasAreanumber,
            entityName: 'PtasSpecialtyarea',
            commercialDistrict: item.PtasCommercialdistrict,
            queryEntityName: 'PtasGeoneighborhood',
          });
        });
      }
    } else {
      const hotelNbhds: string[] = [];
      data.forEach((item) => {
        const parentId =
          item.PtasSpecialtyareaidValue ?? item.PtasGeoareaidValue;
        const area = get().data.find((d) => d.id === parentId);

        //Floating homes or Hotels
        if (
          (!isSpecialtyArea && area?.ptasName === '015') ||
          (isSpecialtyArea && area?.ptasName === '160')
        ) {
          if (area?.ptasName === '160') {
            hotelNbhds.push(item.PtasName);
          }
          if (toAdd.length === 1) return;
          toAdd.push({
            id: item.PtasSpecialtyneighborhoodid ?? item.PtasGeoneighborhoodid,
            name: 'All Nbhds',
            parentId,
            hasItems: false,
            isDirectory: false,
            ptasName: item.PtasName,
            ptasNbhdNumber: item.PtasNbhdnumber,
            description: item.PtasDescription,
            entityName: 'PtasSpecialtyarea',
            queryEntityName: '',
            area,
            nbhdDescription: 'All',
          });
          return;
        }

        if (isSpecialtyArea && area?.ptasName === '280') {
          isMajorOffice = true;
        }

        toAdd.push({
          id: item.PtasSpecialtyneighborhoodid ?? item.PtasGeoneighborhoodid,
          name: 'Nbhd ' + item.PtasName,
          parentId,
          hasItems: false,
          isDirectory: false,
          ptasName: item.PtasName,
          ptasNbhdNumber: item.PtasNbhdnumber,
          description: item.PtasDescription,
          entityName: isSpecialtyAreaRoot ? 'PtasSpecialtyarea' : 'PtasGeoarea',
          queryEntityName: '',
          area,
          nbhdDescription: `${item.PtasName} - ${item.PtasDescription}`,
        });
      });
      if (get().hotelNbhds.length === 0) {
        set({ hotelNbhds });
      }
    }

    //Major Office
    const toAddMajor: GenericRow<CommArea>[] = [];
    if (isMajorOffice) {
      const regions = uniq(toAdd.flatMap((a) => a.ptasName?.charAt(0)));
      regions.forEach((r) => {
        if (!r) return;
        toAddMajor.push({
          id: `r${r}`,
          name: `Region ${r}`,
          parentId: toAdd[0].parentId,
          hasItems: true,
          isDirectory: true,
          ptasName: r,
          description: `Region ${r}`,
          ptasAreaNumber: toAdd[0].ptasAreaNumber,
          entityName: toAdd[0].entityName,
          queryEntityName: isSpecialtyAreaRoot
            ? 'PtasSpecialtyneighborhood'
            : 'PtasGeoneighborhood',
        });
      });

      toAdd.forEach((a) => {
        if (!a.area) return;
        toAddMajor.push({
          ...a,
          parentId: `r${a.ptasName?.charAt(0)}` ?? a.area.id,
        });
      });

      toAdd = toAddMajor;
    }

    set((state) => ({
      data: sortBy(state.data.concat(toAdd), 'name'),
      isLoading: false,
    }));
  },
  setData: (data) => set({ data }),
  selectedItem: null,
  setSelectedItem: (selectedItem) => set({ selectedItem }),
  isLoading: false,
  setIsLoading: (isLoading) => set({ isLoading }),
}));

const useAreaTreeStore = createTrackedSelector(useStore);

export default useAreaTreeStore;
