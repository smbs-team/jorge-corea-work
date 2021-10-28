/* eslint-disable @typescript-eslint/no-explicit-any */
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

type F = (...args: any[]) => any;

// type RemoveFnArgs = (
//   f: F,
//   ...indexes: number[]
// ) => (...args: any[]) => ReturnType<typeof f>;

// const removeFnArgs: RemoveFnArgs = (f, ...indexes) => (
//   ...args
// ): ReturnType<typeof f> => f(...args.filter((_, i) => !indexes.includes(i)));

export const removeFirstArg = (f: F) => (
  ...args: any[]
): ReturnType<typeof f> => f(...args.slice(1));
