// react-app-env.d.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

/// <reference types="react-scripts" />
declare namespace NodeJS {
  export interface ProcessEnv {
    /**
     *Application environment to determine whether is production, development, uat or something else
     */
    NODE_ENV: string;
    /**
     *Default map zoom level when setting USE_EXTENT_FOR_FEATURE_DATA as false
     */
    REACT_APP_DEFAULT_FEATURE_DATA_ZOOM_LEVEL: number;
    /**
     *Map tile host url
     */
    REACT_APP_MAP_TILE_SERVICE_HOST: string;
    /**
     *Access token for mapbox api
     */
    REACT_APP_MAPBOX_GL_ACCESS_TOKEN: string;
    /**
     *Mapbox api for search places
     */
    REACT_APP_MAPBOX_SEARCH_API_URL: string;
    /**
     *Custom searches url
     */
    REACT_APP_CUSTOM_SEARCHES_URL: string;
    /**
     * URL of parcel details in Dynamics
     */
    REACT_APP_DYNAMICS_PARCEL_DETAILS_URL: string;
    /**
     * URL for pictometry tool
     */
    REACT_APP_PICTOMETRY_URL: string;
    /**
     * URL for Mapbox Directions API
     */
    REACT_APP_MAPBOX_DIRECTIONS_API_URL: string;
    /**
     * Info mode top most layers
     */
    REACT_APP_INFO_MODE_TOP_MOST: number;
    /**
     * Admin roles, separated by comma
     */
    REACT_APP_ADMIN_ROLES: string;
    /**
     * Default map url
     */
    REACT_APP_DEFAULT_MAP_URL?: string;
    /**
     * Default map layer min zoom
     */
    REACT_APP_DEFAULT_MAP_MIN_ZOOM?: string;
    /**
     * Load default map as default
     */
    REACT_APP_USE_DEFAULT_MAP: ?string;
    /**
     * Appeals url for parent frame
     */
    REACT_APP_EAPPEALS_URL: string;
    /** Expressions filter for classbreak renderer */
    REACT_APP_USE_CLASSBREAK_EXPRESSIONS_FILTER?: string;
  }
}
