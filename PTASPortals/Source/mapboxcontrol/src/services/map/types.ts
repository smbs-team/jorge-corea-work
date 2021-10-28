// types.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Feature, Point, FeatureCollection } from '@turf/turf';
import { UserMap } from './model';
import { UserDetails } from './model/renderer';

export interface UserMapsChangeEvent {
  userMaps?: UserMap[];
  userDetails: UserDetails[];
  defaultUserMapId?: number;
}

/**
 * Search parcel by address response item
 */
export interface ParcelByAddressItem {
  relevance: number;
  country: string;
  streetname: string;
  formattedaddr: string;
  state: string;
  laitude: number;
  longitude: number;
}

/**
 * Search address by location response item
 */
export interface AddressByLocationItem {
  addressFull: string;
  lat: number;
  lng: number;
}

/**
 * Search parcel by address response item
 */
export interface AddressItem {
  addressFull: string;
  lat: number;
  lng: number;
}

export interface BookmarkTagsItem {
  ptas_bookmarktagid: string;
  ptas_name: string;
}

export interface OptionSetItemResp {
  value: string;
  attributename: string;
  versionnumber: string;
  langid: string;
  objecttypecode: string;
  stringmapid: string;
  organizationid: string;
  displayorder: string;
  attributeValue: number;
}

export interface BookmarkItemResp {
  ptas_bookmarkdate: string;
  ptas_bookmarkid: string;
  ptas_bookmarknote?: string;
  ptas_bookmarktype: number;
}

export interface CreateUpdateBookmarkData {
  id: string;
  date: string;
  tagNames: string;
  tag1Id: string | null;
  tag2Id: string | null;
  tag3Id: string | null;
  tag4Id: string | null;
  tag5Id: string | null;
  type: number | null;
  note: string;
  parcelDetailId: string;
}

/**
 * Search by parcel pin response
 */
export interface SearchByParcelResult {
  min: [number, number];
  max: [number, number];
  center: [number, number];
}

/**
 * Feature for search by address result by from mapbox api
 */
interface GetParcelByAddressMapBoxApiResFeature extends Feature<Point> {
  address: string;
  center: [number, number];
  matching_place_name?: string;
  place_name?: string;
  relevance: number;
}
/**
 * Feature for search by location result by from mapbox api
 */
interface GetAddressByLocationApiResFeature extends Feature<Point> {
  center: [number, number];
  place_name?: string;
}
/**
 * Feature for search by location result by from mapbox api
 */
interface GetAddressSuggestionsApiResFeature extends Feature<Point> {
  center: [number, number];
  place_name?: string;
}
/**
 * Interface for search by address result from mapbox search api
 */
export interface GetParcelByAddressMapBoxApiRes extends FeatureCollection {
  attribution: string;
  query: string;
  features: GetParcelByAddressMapBoxApiResFeature[];
}

export interface GetAddressByLocation extends FeatureCollection {
  attribution: string;
  query: string;
  features: GetAddressByLocationApiResFeature[];
}

export interface GetAddressSuggestions extends FeatureCollection {
  attribution: string;
  query: string;
  features: GetAddressSuggestionsApiResFeature[];
}

export interface MarkerEvent {
  target: {
    _pos: {
      x: number;
      y: number;
    };
  };
}

export interface FolderData {
  folderPath: string;
  folderItemType: 'UserMap' | 'MapRenderer';
}

export interface RenameFolderRequest {
  folderPath: string;
  newName: string;
  folderItemType: 'UserMap' | 'MapRenderer';
}
