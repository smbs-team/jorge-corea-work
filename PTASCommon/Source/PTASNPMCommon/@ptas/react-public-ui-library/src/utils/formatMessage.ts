// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useIntl } from "react-intl";

export interface formatMessageStructure {
  id: string;
  defaultMessage: string;
  description?: string | object;
}

export const renderFormatMessage = (
  obj: formatMessageStructure | string | undefined
): string => {
  if (!obj) return "";
  if (typeof obj === "string") return obj;

  const intl = useIntl();
  return intl.formatMessage(obj);
};
