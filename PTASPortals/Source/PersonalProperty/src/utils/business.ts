// business.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { apiService } from 'services/api/apiService';

export const getOptionSetValue = async (
  objectId: string,
  optionSetId: string,
  optionSetVal: string
): Promise<number> => {
  return (
    ((await apiService.getOptionSet(objectId, optionSetId)).data || []).find(
      (op) => op.value === optionSetVal
    )?.attributeValue ?? -1
  );
};
