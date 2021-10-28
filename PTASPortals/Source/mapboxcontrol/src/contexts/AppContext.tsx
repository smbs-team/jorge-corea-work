/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useAddWindowProps } from 'hooks/dev/useAddWindowProps';
import React, {
  createContext,
  Dispatch,
  PropsWithChildren,
  useState,
} from 'react';

interface AppContextProps {
  showBackdrop: boolean;
  setShowBackdrop: Dispatch<React.SetStateAction<boolean>>;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const AppContext = createContext<AppContextProps>(null as any);
export const AppProvider = (props: PropsWithChildren<unknown>): JSX.Element => {
  useAddWindowProps();
  const [showBackdrop, setShowBackdrop] = useState(false);
  return (
    <AppContext.Provider
      value={{
        showBackdrop,
        setShowBackdrop,
      }}
    >
      {props.children}
    </AppContext.Provider>
  );
};
