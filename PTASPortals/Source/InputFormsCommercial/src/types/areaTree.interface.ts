// areaTree.interface.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export interface AreaChanges {
  PtasDescription: string;
  PtasName: string;
}

export interface SpecialtyArea {
  PtasAreanumber: string;
  PtasSpecialtyareaid: string;
}

export interface GeoArea {
  PtasGeoareaid: string;
  PtasCommercialdistrict: string;
}

export interface SpecialtyNeighborhood {
  PtasNbhdnumber: string;
  PtasSpecialtyneighborhoodid: string;
  PtasSpecialtyareaidValue: string;
}

export interface GeoNeighborhood {
  PtasGeoneighborhoodid: string;
  PtasGeoareaidValue: string;
}

export type AreaTreeChanges = SpecialtyArea &
  SpecialtyNeighborhood &
  GeoArea &
  GeoNeighborhood &
  AreaChanges;
