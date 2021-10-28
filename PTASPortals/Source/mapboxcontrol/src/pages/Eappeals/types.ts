/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

interface TestType1 {
  type: 'test-type1';
  payload: {
    name: string;
  };
}

interface TestType2 {
  type: 'test-type2';
  payload: string;
}

export type AppealMessage = TestType1 | TestType2;
