// directions.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { Geometry } from 'geojson/index';

export interface MapboxDirectionsAdmin {
  iso_3166_1_alpha3: string;
  iso_3166_1: string;
}

export interface MapboxDirectionsLeg {
  steps: unknown[];
  admins: MapboxDirectionsAdmin[];
  duration: number;
  distance: number;
  weight: number;
  summary: string;
}

export interface MapboxDirectionsRoute {
  weight_name: string;
  weight: number;
  duration: number;
  distance: number;
  legs: MapboxDirectionsLeg[];
  geometry: Geometry;
}

export interface MapboxDirectionsWayPoint {
  distance: number;
  name: string;
  location: number[];
}

export interface MapboxDirections {
  routes: MapboxDirectionsRoute[];
  waypoints: MapboxDirectionsWayPoint[];
  code: string;
  uuid: string;
}

export interface WalkingDistanceRowData {
  parcel: string;
  distance: number;
  address: string;
  directionsData: MapboxDirections;
}

export interface DrivingTimeRowData {
  parcel: string;
  time: number;
  address: string;
  directionsData: MapboxDirections;
}
