// NewBusiness.tsx
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
import NewBusinessProvider from './provider';
import { UseNewBusiness } from './types';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const NewBusinessContext = createContext<UseNewBusiness>(null as any);

const withNewBusinessProvider =
  <P extends object>(Component: ComponentType<P>): FC<P> =>
  (props: PropsWithChildren<P>): JSX.Element => {
    return (
      <NewBusinessProvider>
        <Component {...props} />
      </NewBusinessProvider>
    );
  };

export { withNewBusinessProvider };
