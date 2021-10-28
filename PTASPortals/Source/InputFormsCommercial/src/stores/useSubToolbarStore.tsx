// useSubToolbarStore.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { IconToolBarItem } from '@ptas/react-ui-library';
import { createTrackedSelector } from 'react-tracked';
import create from 'zustand';


type FilteredSubToolbar = Pick<
  SubToolBarStore,
  'title' | 'setTitle' | 'setIcons' | 'icons'
>;

interface SubToolBarStore {
  route: React.ReactNode[];
  title: string;
  icons: IconToolBarItem[];
  miscContent: React.ReactNode;
  detailTop: string;
  detailBottom: string;
  setRoute: (route: React.ReactNode[]) => void;
  setTitle: (title: string) => void;
  setIcons: (icons: IconToolBarItem[]) => void;
  setMiscContent: (misc: React.ReactNode) => void;
  setDetailTop: (text: string) => void;
  setDetailBottom: (text: string) => void;
}

const useStore = create<FilteredSubToolbar>((set) => ({
  title: 'Default title',
  setTitle: (newTitle) => set((state) => ({ title: newTitle })),
  icons: [],
  setIcons: (newIcons) => set((state) => ({ icons: newIcons })),
}));

const useSubToolbarStore = createTrackedSelector(useStore);

export default useSubToolbarStore;
