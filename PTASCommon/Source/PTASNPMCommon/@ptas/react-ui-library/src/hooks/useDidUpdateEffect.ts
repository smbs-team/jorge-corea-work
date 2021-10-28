/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/explicit-function-return-type */
// useDidUpdateEffect.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useEffect, useRef } from "react";

/**
 * Ignores the initial render
 * @param fn - Function that will be executed after the initial render
 * @param inputs - Inputs for the useEffect to listen to
 */
export function useDidUpdateEffect(fn: () => void, inputs: any[]) {
  const didMountRef = useRef(false);
  useEffect(() => {
    if (didMountRef.current) fn();
    else didMountRef.current = true;
  }, inputs);
}
