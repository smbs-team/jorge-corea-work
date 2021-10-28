// types.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ItemSuggestion } from '@ptas/react-public-ui-library';
import { AddressLookup } from 'services/map/model/addresses';

export type ProfileStep = 'name' | 'email' | 'address' | 'phone';

export type ProfileFormIsValid = {
  step: ProfileStep;
  valid: boolean;
};

export type ProfileAction = 'create' | 'update';

export type SearchAddressSuggestions = {
  map: Map<string, AddressLookup[]>;
  loading: Map<string, boolean>;
};

export type SearchSuggestions = {
  map: Map<string, ItemSuggestion[]>;
  loading: Map<string, boolean>;
};
