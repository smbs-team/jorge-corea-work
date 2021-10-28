// HomeImprovementContext.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { ComponentType, FC, PropsWithChildren, useContext } from 'react';
import { createContext } from 'react';
import useHomeImprovement, { UseHomeImprovement } from './useHomeImprovement';

const HomeImprovementContext = createContext<UseHomeImprovement | undefined>(
  undefined
);

const HomeImprovementProvider = ({
  children,
}: PropsWithChildren<{}>): JSX.Element => {
  return (
    <HomeImprovementContext.Provider value={{ ...useHomeImprovement() }}>
      {children}
    </HomeImprovementContext.Provider>
  );
};

const useHI = (): UseHomeImprovement => {
  const context = useContext(HomeImprovementContext);
  if (context === undefined) {
    throw new Error('useHI must be used within a HomeImprovementProvider');
  }
  return context;
};

const withHomeImprovementProvider = <P extends object>(
  Component: ComponentType<P>
): FC<P> => (props: PropsWithChildren<P>): JSX.Element => {
  return (
    <HomeImprovementProvider>
      <Component {...props} />
    </HomeImprovementProvider>
  );
};

export { HomeImprovementProvider, useHI, withHomeImprovementProvider };
