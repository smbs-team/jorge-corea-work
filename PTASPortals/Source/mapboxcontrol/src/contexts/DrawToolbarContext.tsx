/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  createContext,
  Dispatch,
  PropsWithChildren,
  SetStateAction,
  useState,
} from 'react';
export type ActionMode = 'location' | 'measure' | 'draw' | 'info-mode' | null;

type Props = {
  actionMode: ActionMode | null;
  setActionMode: Dispatch<SetStateAction<ActionMode | null>>;
};

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const DrawToolBarContext = createContext<Props>(null as any);

export const DrawToolbarProvider = (
  props: PropsWithChildren<unknown>
): JSX.Element => {
  const [actionMode, setActionMode] = useState<ActionMode>(null);
  return (
    <DrawToolBarContext.Provider
      value={{
        actionMode,
        setActionMode,
      }}
    >
      {props.children}
    </DrawToolBarContext.Provider>
  );
};
