// types.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export type ProfileStep =
  | 'name'
  | 'email'
  | 'address'
  | 'phone'
  | 'access'
  | 'password';

export type ProfileFormIsValid = {
  step: ProfileStep;
  valid: boolean;
};

export type ProfileAction = 'create' | 'update';
