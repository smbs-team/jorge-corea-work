//rendererDataset
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Key } from 'react';
import mapColumnType from 'utils/mapColumnType';

export type DataSetColumnType =
  | 'string'
  | 'number'
  | 'date'
  | 'boolean'
  | 'unknown';
export type ApiColumnType =
  | 'Double'
  | 'String'
  | 'Int32'
  | 'Decimal'
  | 'Int64'
  | 'DateTime'
  | 'Boolean'
  | 'DateTimeOffset'
  | 'Long';

export class RendererDataSetColumn {
  datasetId: string;
  customSearchDefinitionId: number;
  columnName: string;
  columnType: DataSetColumnType;
  // columnType: ApiColumnType;
  columnTypeLength: number;
  canBeUsedAsLookup: boolean;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  columnCategory?: any;
  isEditable: boolean;
  forceEditLookupExpression: boolean;
  isIndexed: boolean;
  isCalculatedColumn: boolean;
  hasEditLookupExpression: boolean;
  expressions: Record<
    string & 'expressionRole',
    Key & 'RangedValuesOverrideExpression'
  >[];

  constructor(
    fields: Omit<RendererDataSetColumn, 'datasetId' | 'columnType'> & {
      columnType: ApiColumnType;
    },
    datasetId: string
  ) {
    this.customSearchDefinitionId = fields.customSearchDefinitionId;
    this.columnName = fields.columnName;
    this.columnTypeLength = fields.columnTypeLength;
    this.canBeUsedAsLookup = fields.canBeUsedAsLookup;
    this.columnCategory = fields.columnCategory;
    this.isEditable = fields.isEditable;
    this.forceEditLookupExpression = fields.forceEditLookupExpression;
    this.isIndexed = fields.isIndexed;
    this.isCalculatedColumn = fields.isCalculatedColumn;
    this.datasetId = datasetId;
    this.columnType = mapColumnType(fields.columnType);
    this.hasEditLookupExpression = fields.hasEditLookupExpression;
    this.expressions = fields.expressions;
  }
}

export class RendererDataset {
  datasetId: string;
  customSearchDefinitionId: number;
  userId: string;
  parentFolderId: number;
  datasetName: string;
  parameterValues: string;
  generatedTableName: string;
  generateSchemaElapsedMs: number;
  executeStoreProcedureElapsedMs: number;
  isLocked: boolean;
  createdTimestamp: string;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  datasetClientState: any;

  constructor(fields: RendererDataset) {
    this.datasetId = fields.datasetId;
    this.customSearchDefinitionId = fields.customSearchDefinitionId;
    this.userId = fields.userId;
    this.parentFolderId = fields.parentFolderId;
    this.datasetName = fields.datasetName;
    this.parameterValues = fields.parameterValues;
    this.generatedTableName = fields.generatedTableName;
    this.generateSchemaElapsedMs = fields.generateSchemaElapsedMs;
    this.executeStoreProcedureElapsedMs = fields.executeStoreProcedureElapsedMs;
    this.isLocked = fields.isLocked;
    this.createdTimestamp = fields.createdTimestamp;
    this.datasetClientState = fields.datasetClientState;
  }
}
