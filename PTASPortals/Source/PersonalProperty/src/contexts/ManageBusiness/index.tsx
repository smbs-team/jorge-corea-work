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
import ManageBusinessProvider from './provider';
import { ManageBusinessContextProps } from './types';

export const ManageBusinessContext = createContext<ManageBusinessContextProps>(
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  null as any
);

const withManageBusinessProvider =
  <P extends object>(Component: ComponentType<P>): FC<P> =>
  (props: PropsWithChildren<P>): JSX.Element => {
    return (
      <ManageBusinessProvider>
        <Component {...props} />
      </ManageBusinessProvider>
    );
  };

export default withManageBusinessProvider;
