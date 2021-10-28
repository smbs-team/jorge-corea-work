/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  RendererClassBreakRule,
  RendererSimpleRule,
  RendererUniqueValuesRule,
  UserDetails,
} from './renderer';
import { MapRenderer } from './MapRenderer';

export interface MapsUserDetails {
  userMaps: UserMap[];
  usersDetails: UserDetails[];
}

export class UserMap {
  userMapId: number;
  userMapName: string;
  createdBy: string;
  lastModifiedBy: string;
  parentFolderId: number;
  isLocked: boolean;
  createdTimestamp: string;
  lastModifiedTimestamp: string;
  folderPath: string;
  folderItemType?: string;
  mapRenderers: MapRenderer[];

  constructor(
    fields: Omit<UserMap, 'mapRenderers' | 'isSystemRenderer'> & {
      mapRenderers: MapRenderer[] | null;
    }
  ) {
    this.userMapId = fields.userMapId;
    this.userMapName = fields.userMapName;
    this.createdBy = fields.createdBy;
    this.lastModifiedBy = fields.lastModifiedBy;
    this.parentFolderId = fields.parentFolderId;
    this.isLocked = fields.isLocked;
    this.createdTimestamp = fields.createdTimestamp;
    this.lastModifiedTimestamp = fields.lastModifiedTimestamp;
    this.folderPath = fields.folderPath;
    this.folderItemType = fields.folderItemType;
    this.mapRenderers = fields.mapRenderers
      ? fields.mapRenderers.map((r) => {
          return new MapRenderer(r);
        })
      : [];
  }

  public toCustomString? = (mapRenderers: MapRenderer[]): string => {
    const renderersStr = !mapRenderers.length
      ? ''
      : mapRenderers
          .map((r) => {
            if (!r.toCustomString) return '';
            return r.toCustomString({
              ...r.rendererRules,
            });
          })
          .reduce((acc, curr) => {
            return acc + curr;
          });
    return (
      '_mapId=' + this.userMapId + '_mapName=' + this.userMapName + renderersStr
    );
  };
}

export class UserMapCategory {
  userMapCategoryId: number;
  categoryName: string;
  categoryDescription: string;
  userMaps: UserMap[] | null;
  constructor(fields: UserMapCategory) {
    this.userMapCategoryId = fields.userMapCategoryId;
    this.categoryName = fields.categoryName;
    this.categoryDescription = fields.categoryDescription;
    this.userMaps = fields.userMaps;
  }
}

export type ColorRule =
  | RendererSimpleRule
  | RendererClassBreakRule
  | RendererUniqueValuesRule;
