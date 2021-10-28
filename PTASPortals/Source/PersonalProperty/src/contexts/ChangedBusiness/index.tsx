// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  FC,
  PropsWithChildren,
  createContext,
  ComponentType,
} from 'react';
import ChangedBusinessProvider from './provider';
import { ChangedBusinessContextProps } from './types';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const ChangedBusinessContext = createContext<
  ChangedBusinessContextProps
>(null as any);

const withChangedBusinessProvider = <P extends object>(
  Component: ComponentType<P>
): FC<P> => (props: PropsWithChildren<P>): JSX.Element => {
  return (
    <ChangedBusinessProvider>
      <Component {...props} />
    </ChangedBusinessProvider>
  );
};

export default withChangedBusinessProvider;
