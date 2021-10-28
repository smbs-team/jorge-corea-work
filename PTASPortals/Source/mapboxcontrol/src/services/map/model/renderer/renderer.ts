/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { RendererSimpleRule } from './rendererSimpleRule';
import { RendererClassBreakRule } from './rendererClassBreakRule';
import { RendererUniqueValuesRule } from './RendererUniqueValuesRule';
import { LayerSource } from '../LayerSource';
import { MapRenderer } from '../MapRenderer';
import { RendererLabel } from 'services/map/renderer/textRenderer/textRenderersService';

export type Rule =
  | RendererSimpleRule
  | RendererClassBreakRule
  | RendererUniqueValuesRule;

export type RendererRules = {
  layer: LayerSource['defaultMapboxLayer'];
  rule: Rule;
  label?: RendererLabel;
};

export interface RenderersUserDetails {
  mapRenderers: MapRenderer[];
  usersDetails: UserDetails[];
}

export interface UserDetails {
  id: string;
  fullName: string;
  email: string;
  roles: Role[];
  teams: Team[];
}

export interface Role {
  id: string;
  name: string;
}

export interface Team {
  id: string;
  name: string;
}

export class RendererCategory {
  mapRendererCategoryId: string;
  categoryName: string;
  categoryDescription: string;
  mapRenderers: MapRenderer[];
  constructor(fields: RendererCategory) {
    this.mapRendererCategoryId = fields.mapRendererCategoryId;
    this.categoryName = fields.categoryName;
    this.categoryDescription = fields.categoryDescription;
    this.mapRenderers = fields.mapRenderers;
  }
}
