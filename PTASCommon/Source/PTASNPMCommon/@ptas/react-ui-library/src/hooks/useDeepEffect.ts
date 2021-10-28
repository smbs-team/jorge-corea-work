// useDeepEffect.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useRef, useEffect } from "react";
import { isEqual } from "lodash";

/**
 * See: https://stackoverflow.com/questions/55808749/use-object-in-useeffect-2nd-param-without-having-to-stringify-it-to-json
 * @param fn - The function to be executed
 * @param deps - The depending object
 */
// eslint-disable-next-line @typescript-eslint/no-explicit-any
export function useDeepEffect(fn: any, deps: any[]): void {
  const isFirst = useRef(true);
  const prevDeps = useRef(deps);

  useEffect(() => {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const isSame = prevDeps.current.every((obj: any, index: any) =>
      isEqual(obj, deps[index])
    );

    if (isFirst.current || !isSame) {
      fn();
    }

    isFirst.current = false;
    prevDeps.current = deps;
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, deps);
}
