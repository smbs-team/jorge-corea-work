// useUserStore.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { createTrackedSelector } from 'react-tracked';
import create from 'zustand';

interface Props {
  userId: string;
  setUserId: (userId: string) => void;
}

const useStore = create<Props>(set => ({
  userId: '',
  setUserId: userId => set({ userId }),
}));

const useUserStore = createTrackedSelector(useStore);

export default useUserStore;
