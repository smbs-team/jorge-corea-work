// isLocalHost.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

const isLocalHost = (): boolean =>
  window.location.hostname === '_localhost' ||
  window.location.hostname === '_127.0.0.1';

export default isLocalHost;
