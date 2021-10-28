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
import UpdateBusinessProvider from './provider';
import { UseUpdateBusiness } from './types';

export const UpdateBusinessContext = createContext<UseUpdateBusiness>(
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  null as any
);

const withUpdateBusinessProvider =
  <P extends object>(Component: ComponentType<P>): FC<P> =>
  (props: PropsWithChildren<P>): JSX.Element => {
    return (
      <UpdateBusinessProvider>
        <Component {...props} />
      </UpdateBusinessProvider>
    );
  };

export { withUpdateBusinessProvider };
