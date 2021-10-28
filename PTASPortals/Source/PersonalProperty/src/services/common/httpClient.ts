// httpClient.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

type ApiError = 'REQUEST_TIMEOUT';

export class ApiServiceResult<T> {
  data?: T;
  errorCode?: ApiError;
  errorMessage?: string;
  hasError: boolean;
  constructor(fields: Omit<ApiServiceResult<T>, 'hasError'>) {
    this.data = fields.data;
    this.errorMessage = fields.errorMessage;
    this.hasError = !!fields.errorCode || !!fields.errorMessage;
    this.errorCode = this.hasError ? fields.errorCode : undefined;
  }
}

export const handleReq = async <T>(
  fn: () => Promise<ApiServiceResult<T>>,
  onError?: Function
): Promise<ApiServiceResult<T>> => {
  try {
    return await fn();
  } catch (e) {
    onError && onError(e);
    const retVal = new ApiServiceResult<T>({
      errorMessage: e.message,
    });
    if (e.message.includes('timeout')) {
      retVal.errorCode = 'REQUEST_TIMEOUT';
    }

    return retVal;
  }
};
