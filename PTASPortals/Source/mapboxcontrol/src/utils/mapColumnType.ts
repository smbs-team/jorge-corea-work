//mapColumnType.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ApiColumnType, DataSetColumnType } from 'services/map';

const mapColumnType = (val: ApiColumnType): DataSetColumnType => {
  switch (val) {
    case 'String': {
      return 'string';
    }
    case 'Double':
    case 'Int64':
    case 'Int32':
    case 'Decimal':
    case 'Long': {
      return 'number';
    }
    case 'DateTime':
    case 'DateTimeOffset': {
      return 'date';
    }
    case 'Boolean': {
      return 'boolean';
    }
    default: {
      console.error('unhandled column type ' + val);
      return 'unknown';
    }
  }
};

export default mapColumnType;
