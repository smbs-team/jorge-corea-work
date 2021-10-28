/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { HomeContext } from 'contexts';
import React, { useContext } from 'react';
import { ComponentType, FC, PropsWithChildren } from 'react';

export const withAuth = <P extends object>(
  Component: ComponentType<P>
): FC<P> => (props: PropsWithChildren<P>): JSX.Element | null => {
  const { b2cToken } = useContext(HomeContext);
  return b2cToken ? <Component {...props}>{props.children}</Component> : null;
};
