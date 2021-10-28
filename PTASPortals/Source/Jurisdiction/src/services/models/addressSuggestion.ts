// addressSuggestion.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface AddressSuggestion {
    id: string;
    city: string;
    country: string;
    formattedaddr: string;
    laitude: number;
    longitude: number;
    neighborhood: string;
    relevance: number;
    state: string;
    streetname: string;
    zip: string;
  }