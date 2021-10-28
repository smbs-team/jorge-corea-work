// utilService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { kebabCase } from 'lodash';
import { CSSProperties } from '@material-ui/core/styles/withStyles';

export const FEET_PER_METER = 3.281;
export const INCHES_PER_METER = 39.3701;

/**
 * An utility for all the app
 */
class UtilService {
  /**
   * Converts longitude position to tile
   *
   * @remarks
   * See
   * {@link https://developers.google.com/maps/documentation/javascript/coordinates}
   *
   * @param lon - The longitude to be converted
   * @param zoom - The current map zoom level
   * @returns The resulted tile of the longitude and the zoom level
   */
  public long2tile = (lon: number, zoom: number): number =>
    Math.floor(((lon + 180) / 360) * Math.pow(2, zoom));

  /**
   * Converts latitude position to tile
   *
   * @remarks
   * See
   * {@link https://developers.google.com/maps/documentation/javascript/coordinates}
   *
   * @param lat - The latitude to be converted
   * @param zoom - The current map zoom level
   * @returns The resulted tile of the latitude and the zoom level
   */
  public lat2tile = (lat: number, zoom: number): number =>
    Math.floor(
      ((1 -
        Math.log(
          Math.tan((lat * Math.PI) / 180) + 1 / Math.cos((lat * Math.PI) / 180)
        ) /
          Math.PI) /
        2) *
        Math.pow(2, zoom)
    );

  bytesToMb = (bytes: number): number => +(bytes / (1024 * 1024)).toFixed(0);

  /**
   * Converts square meters to square feet
   *
   * @param meters - The meters to convert
   * @returns Square feet equivalent of given square meters
   */
  squareMetersToSquareFeet = (meters: number): number => meters * 10.7639;

  /**
   * Converts meters to feet
   *
   * @param meters - The  meters to convert
   * @returns Feet equivalent of given meters
   */
  metersToFeet = (meters: number): number => meters * FEET_PER_METER;
  feetToMeters = (feet: number): number => feet / FEET_PER_METER;

  /**
   * Converts kilometers to feet
   *
   * @param km - The  kilometers to convert
   * @returns Feet equivalent of given kilometers
   */
  kilometersToFeet = (km: number): number => this.metersToFeet(km * 1000);

  /**
   * Apply a filter to an object
   *
   * @param o - The object to be filtered
   * @param filterFn - The function to be executed for each object own property
   * @returns The filtered object result of the filter function
   */
  filterObject = <T>(
    o: T,
    filterFn: {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      (item: any): boolean;
    }
  ): T =>
    Object.entries(o)
      .filter(filterFn)
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      .reduce<T>((acc, [k, v]) => ({ ...acc, [k]: v }), {} as any);

  getLocalDateString(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(Date.parse(dateString));
    let hour = date.getHours();
    const minutes = date.getMinutes();
    let minutesStr = minutes.toString();
    let meridian = 'am';
    if (hour > 11) {
      meridian = 'pm';
      hour -= 12;
    }
    if (hour === 0) hour += 12;
    if (minutes < 10) minutesStr = '0' + minutes;

    return (
      date.getMonth() +
      1 +
      '/' +
      date.getDate() +
      '/' +
      date.getFullYear().toString().substr(2) +
      ', ' +
      hour +
      ':' +
      minutesStr +
      meridian
    );
  }

  /**
   * Converts Unix timestamp string (with ms) to Date
   * @param unixTimestamp - Unix timestamp
   */
  unixTimeToDate(unixTimestamp: string): Date | undefined {
    if (unixTimestamp) {
      return new Date(Number.parseInt(unixTimestamp));
    }
    return undefined;
  }

  /**
   * Copies string value to clipboard
   */
  copyStringToClipboard(str: string): void {
    const el = document.createElement('textarea');
    el.value = str;
    // Set non-editable to avoid focus and move outside of view
    el.setAttribute('readonly', '');
    el.style.position = 'absolute';
    el.style.left = '-9999px';
    document.body.appendChild(el);
    // Select text inside element
    el.select();
    // Copy text to clipboard
    document.execCommand('copy');
    document.body.removeChild(el);
  }
  /**
   * convert json to styles string
   * @param style -Json style
   */
  jsonToStyle = (style: CSSProperties): string =>
    Object.entries(style)
      .map(([k, v]) => `${kebabCase(k)}:${v}`)
      .join(';');

  /**
   * given an html string convert it to Html Element
   * @param htmlString - Html string
   */
  createElementFromHTML = <T extends HTMLElement>(htmlString: string): T => {
    const div = document.createElement('div');
    div.innerHTML = htmlString.trim();
    // Change this to div.childNodes to support multiple top-level nodes
    return (div.firstChild ?? div) as T;
  };

  percentage = (n: number, p: number): number => n * (p * 0.01);
  /**
   * Convert map zoom to meter/pixels
   *
   * @remarks See: https://stackoverflow.com/questions/30703212/how-to-convert-radius-in-metres-to-pixels-in-mapbox-leaflet
   * @param latitude - The map latitude
   * @param zoomLevel - The map zoom level
   */
  mapZoomToMetersPerPixel = (latitude: number, zoomLevel: number): number => {
    const c = 40075017;
    const latitudeRadians = latitude * (Math.PI / 180);
    //Original algorithm adds 8, but mapboxgl adds 9, because of the tile size of 512px x 512px
    return (c * Math.cos(latitudeRadians)) / Math.pow(2, zoomLevel + 9);
  };

  /**
   * Convert meters/pixel to map zoom
   *
   * @param latitude - The map latitude
   * @param zoomLevel - The map zoom level
   */
  metersPerPixelToMapZoom = (latitude: number, scale: number): number => {
    const c = 40075017;
    const latitudeRadians = latitude * (Math.PI / 180);
    return (
      Math.log((c * Math.cos(latitudeRadians)) / (scale * Math.pow(2, 9))) /
      Math.log(2)
    );
  };

  metersToInches = (meters: number): number => meters * 39.3701;

  numberWithCommas = (x: number): string => {
    return x.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ',');
  };

  roundToNearest10 = (x: number): number => {
    return Math.round(x / 10) * 10;
  };

  getPathArray = (pathStr: string): string[] =>
    pathStr.replace(/^\/|\/$/g, '').split('/') ?? [];

  pixelsToPt = (pixels: number): number => pixels * 0.75;
  ptToPixels = (pt: number): number => pt / 0.75;

  screenDPI = ((): number => {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const el = document.createElement('div') as any;
    el.style = 'width: 1in;';
    document.body.appendChild(el);
    const dpi = el.offsetWidth;
    document.body.removeChild(el);
    return dpi;
  })();

  getUrlSearchParam = (
    key: string,
    url: URL = new URL(window.location.href)
  ): string | undefined =>
    new URLSearchParams(url.search).get(key) || undefined;
}

export default new UtilService();
