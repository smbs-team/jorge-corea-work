// rendererDataSetColumn
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export class RendererDataSetColumn {
  customSearchDefinitionId: number;
  columnName: string;
  columnType: string;
  columnTypeLength: number;
  canBeUsedAsLookup: boolean;
  columnCategory: string;
  isEditable: boolean;
  forceEditLookupExpression: boolean;
  isIndexed: boolean;
  isCalculatedColumn: boolean;

  constructor(fields: RendererDataSetColumn) {
    this.customSearchDefinitionId = fields.customSearchDefinitionId;
    this.columnName = fields.columnName;
    this.columnType = fields.columnType;
    this.columnTypeLength = fields.columnTypeLength;
    this.canBeUsedAsLookup = fields.canBeUsedAsLookup;
    this.columnCategory = fields.columnCategory;
    this.isEditable = fields.isEditable;
    this.forceEditLookupExpression = fields.forceEditLookupExpression;
    this.isIndexed = fields.isIndexed;
    this.isCalculatedColumn = fields.isCalculatedColumn;
  }
}
