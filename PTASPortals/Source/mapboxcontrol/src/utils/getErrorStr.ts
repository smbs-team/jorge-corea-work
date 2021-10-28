/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export const getErrorStr = (error: unknown): string | undefined => {
  if (error instanceof Error) {
    return error.stack ?? error.message;
  } else {
    return JSON.stringify(error);
  }
};
