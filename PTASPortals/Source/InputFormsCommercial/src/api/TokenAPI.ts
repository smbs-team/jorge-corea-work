// TokenAPI.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { get } from './AxiosLoader';

export const getMagicToken = async (paramToken: string): Promise<string> => {
  if (!process.env.REACT_APP_MAGIC_LINK)
    throw new Error('.env magic link is empty or does not exist');
  const { data } = await get<string>(process.env.REACT_APP_MAGIC_LINK, {
    headers: { Authorization: `Bearer ${paramToken}` },
  });

  return data;
};
