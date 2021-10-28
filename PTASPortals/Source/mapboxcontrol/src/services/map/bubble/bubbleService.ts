// bubbleService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import mapboxgl, { MapMouseEvent, MapEventType } from 'mapbox-gl';
import { once, uniqueId } from 'lodash';
import { utilService } from 'services/common';
import { MarkerEvent } from 'services/map';
import {
  BubbleSettingsProps,
  Settings as BubbleSettingsType,
} from '../../../components/BubbleSettings';
import { RGBA_BLACK, RGBA_WHITE } from 'appConstants';
import { FONT_STYLE_NAME } from 'components/BubbleSettings/Constants';

interface BubbleService {
  placeBubble: (map: mapboxgl.Map) => void;
  editBubbleText: (id: string, text: string) => void;
  onEditBubble: (cb: (bubble: BubbleSettingsProps) => void) => void;
  updateBubbleSettings?: (settings: BubbleSettingsType) => void;
  init: (map: mapboxgl.Map) => void;
}

interface OnDragBubbleOptions {
  arrowId: string;
  arrowPolygonId: string;
  initialLngLat: mapboxgl.LngLat;
}

interface UpdateBubbleArrowOptions {
  arrowId: string;
  arrowPolygonId: string;
  initialPoint: {
    x: number;
    y: number;
  };
}

type BubbleType = {
  id: string;
  arrowId: string;
  arrowPolygonId: string;
  settings: BubbleSettingsType;
};

const labelStyle = `
font-size: 1rem;
font-family: Cabin,helvetica,sans-serif;
font-weight: 300;
line-height: 1.5;`;

const bubbleService = (): BubbleService => {
  const { createElementFromHTML } = utilService;
  const initialSettings: BubbleSettingsType = {
    id: '',
    text: '',
    textColor: RGBA_BLACK,
    fontWeight: 'normal', // normal | bold
    fontStyle: FONT_STYLE_NAME.normal, // normal | italic
    fontSize: '12pt',
    backgroundColor: RGBA_WHITE,
    borderColor: RGBA_BLACK,
    borderWidth: 1,
  };
  let map: mapboxgl.Map;
  let bubbles: BubbleType[] = [];
  const arrowDimensions = {
    w: 30,
    h: 25,
  };
  let openBubbleSettings: (bubble: BubbleSettingsProps) => void;
  const onEditBubble = (cb: (bubble: BubbleSettingsProps) => void): void => {
    openBubbleSettings = cb;
  };

  const init = once((_map: mapboxgl.Map): void => {
    map = _map;
  });

  const bubbleMouseDown = (): void => map.dragPan.disable();

  const bubbleMouseLeave = (): void => map.dragPan.enable();

  const updateBubbleArrow = (
    options: UpdateBubbleArrowOptions,
    markerPoint: { x: number; y: number }
  ): void => {
    const arrowEl = document.getElementById(options.arrowId);
    const arrowPolygon = document.getElementById(options.arrowPolygonId);
    if (!arrowEl || !arrowPolygon) return;
    const a = options.initialPoint.x - markerPoint.x;
    const b = options.initialPoint.y - markerPoint.y;

    const topBox = b > 0 ? markerPoint.y : options.initialPoint.y;
    const leftBox = a > 0 ? markerPoint.x : options.initialPoint.x;
    const bAbs = Math.abs(b);
    const heightBox = bAbs > 2 ? bAbs : 2;
    const aAbs = Math.abs(a);
    const widthBox = aAbs > 20 ? aAbs : 20;
    let p1 = '0% 0%';
    let p2 = '0% 100%';
    let p3 = '100% 100%';
    let polygonPoints = `0,0 0,0 0,0`;
    let topOffset = 0;
    let leftOffset = 0;

    if (widthBox === 20) {
      leftOffset = a > 0 ? 10 - aAbs : 10;
      p1 = '0% 0%';
      p2 = '100% 0%';
      p3 = '50% 100%';

      polygonPoints = `0,0 ${widthBox},0 ${widthBox / 2},${heightBox}`;
    } else if (heightBox === 2) {
      topOffset = 5;
      if (a < 0) {
        p1 = '100% 0%';
        p2 = '0% 100%';
        p3 = '100% 100%';

        polygonPoints = `${widthBox},0 0,${heightBox} ${widthBox},${heightBox}`;
      }
    } else if (a > 0) {
      const offset = widthBox - 30;
      const d = 100 - (offset / widthBox) * 100;
      p1 = '0% 0%';
      p2 = `${d}% 0%`;
      p3 = '100% 100%';

      polygonPoints = `0,0 30,0 ${widthBox},${heightBox}`;
    } else {
      const offset = widthBox - 30;
      const d = (offset / widthBox) * 100;
      p1 = `${d}% 0%`;
      p2 = '100% 0%';
      p3 = '0% 100%';

      polygonPoints = `${offset},0 ${widthBox},0 0,${heightBox}`;
    }

    arrowPolygon.setAttribute('points', polygonPoints);

    arrowEl.style.top = `${topBox - topOffset}px`;
    arrowEl.style.left = `${leftBox - leftOffset}px`;
    arrowEl.style.height = `${heightBox}px`;
    arrowEl.style.width = `${widthBox}px`;
    arrowEl.style.clipPath = `polygon(${p1}, ${p2}, ${p3})`;
    arrowEl.style.transform = `rotateX(${b > 0 ? 0 : 180}deg)`;
  };

  const onDragBubble = (
    options: OnDragBubbleOptions
  ): ((e: MarkerEvent) => void) => {
    return (e: MarkerEvent): void => {
      const initialPoint = map.project(options.initialLngLat);
      updateBubbleArrow(
        {
          ...options,
          initialPoint,
        } as UpdateBubbleArrowOptions,
        e.target._pos
      );
    };
  };

  const createArrowSvg = (
    id: string,
    points: string,
    style: string
  ): string => {
    return `
        <svg height="100%" width="100%">
            <polygon id="${id}" points="${points}" style="${style}" />
        </svg>
    `;
  };

  const getBubbleHtml = (text: string, id: string): HTMLDivElement => {
    const svgMenuIcon = `<svg class="MuiSvgIcon-root" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M3 18h18v-2H3v2zm0-5h18v-2H3v2zm0-7v2h18V6H3z"></path></svg>`;
    return createElementFromHTML<HTMLDivElement>(`
        <div class='speech-bubble'} id='${id}'>
          <span id="bubble-text-${id}" class="bubble-text">
          ${text}
          </span>
          <div id="popup-${id}" class="popup">
            ${svgMenuIcon}
            <ul>
              <li>
                <a href="#" id="bubbleEditBtn-${id}" style="${labelStyle}">Edit</a>
              </li>
              <li>
                <a href="#" id="bubbleDeleteBtn-${id}" style="${labelStyle}">Delete</a>
              </li>
            </ul>
          </div>
          <div id="bubble-resizer-${id}" class="bubble-resizer"></div>
        </div>
   `);
  };

  const createMarker = (e: MapMouseEvent): void => {
    const id = uniqueId('bubble-');
    const mapContainer = map.getContainer();
    const el = getBubbleHtml('Hello', id);
    const arrowId = id + '-arrow';
    const arrowPolygonId = `arrow-polygon-${id}`;
    const marker = new mapboxgl.Marker({
      element: el,
      anchor: 'bottom',
      draggable: true,
      offset: new mapboxgl.Point(0, -arrowDimensions.h),
    })
      .setLngLat(e.lngLat)
      .addTo(map)
      .on(
        'drag',
        onDragBubble({
          arrowId,
          arrowPolygonId,
          initialLngLat: e.lngLat,
        })
      );

    const arrow = document.createElement('div');

    map
      .on('movestart', () => {
        arrow.style.visibility = 'hidden';
      })
      .on('moveend', () => {
        arrow.style.visibility = 'visible';
        const markerLngLat = marker
          .setOffset(new mapboxgl.Point(0, 0))
          .getLngLat();
        if (!markerLngLat) return;
        const markerPoint = map.project(markerLngLat);
        const initialPoint = map.project(e.lngLat);
        updateBubbleArrow(
          {
            arrowId,
            arrowPolygonId,
            initialPoint,
          },
          markerPoint
        );
      });

    arrow.className = 'speech-bubble-arrow';
    arrow.id = arrowId;
    arrow.style.width = arrowDimensions.w + 'px';
    arrow.style.height = arrowDimensions.h + 'px';
    arrow.style.top = e.point.y - arrowDimensions.h - 1 + 'px';
    arrow.style.left = e.point.x - arrowDimensions.w / 2 + 'px';
    arrow.innerHTML = createArrowSvg(
      arrowPolygonId,
      `0,0 ${arrowDimensions.w},0 ${arrowDimensions.w / 2},${
        arrowDimensions.h
      }`,
      ''
    );

    el.addEventListener('mousedown', bubbleMouseDown);
    el.addEventListener('mouseleave', bubbleMouseLeave);
    mapContainer.append(arrow);

    // save bubbles
    bubbles = [
      ...bubbles,
      {
        id,
        arrowId,
        arrowPolygonId,
        settings: { ...initialSettings, id },
      },
    ];

    updateBubbleStyles({ ...initialSettings, id }); // apply initial style

    const popupEl = document.getElementById(`popup-${id}`);

    popupEl?.addEventListener('click', () => {
      popupEl.classList.toggle('popup-show');
    });

    document
      .getElementById(`bubbleEditBtn-${id}`)
      ?.addEventListener('click', (e: MouseEvent) => {
        e.preventDefault();
        triggerOpenBubble(id, el);
      });
    document
      .getElementById(`bubbleDeleteBtn-${id}`)
      ?.addEventListener('click', (e: MouseEvent) => {
        e.preventDefault();
        marker.remove();
        const arrowElement = document.getElementById(arrowId);
        arrowElement?.parentNode?.removeChild(arrowElement);
        removeBubbleData(id);
        bubbleMouseLeave();
      });

    // resizer
    let bubbleEl = document.getElementById(id);
    let bubbleBoundingClientRect = bubbleEl?.getBoundingClientRect();
    const resizerEl = document.getElementById(`bubble-resizer-${id}`);

    resizerEl?.addEventListener('mousedown', (e: MouseEvent) => {
      bubbleEl = document.getElementById(id);
      bubbleBoundingClientRect = bubbleEl?.getBoundingClientRect();
      marker.isDraggable() && marker.setDraggable(false);
      window.addEventListener('mousemove', HandleResize);
      window.addEventListener('mouseup', () => {
        window.removeEventListener('mousemove', HandleResize);
        if (!bubbleEl) return;
        bubbleEl.style.top = '0';
        bubbleEl.style.left = '0';
        !marker.isDraggable() && marker.setDraggable(true);
      });
    });
    const HandleResize = (e: MouseEvent): void => {
      if (!bubbleEl || !bubbleBoundingClientRect) return;
      const newHeight = e.pageY - bubbleBoundingClientRect.top;
      const newWidth = e.pageX - bubbleBoundingClientRect.left;
      bubbleEl.style.top = `${
        newHeight -
        (bubbleBoundingClientRect.bottom - bubbleBoundingClientRect.top)
      }px`;
      bubbleEl.style.left = `${
        (newWidth -
          (bubbleBoundingClientRect.right - bubbleBoundingClientRect.left)) /
        2
      }px`;
      bubbleEl.style.height = `${newHeight}px`;
      bubbleEl.style.width = `${newWidth}px`;
    };
  };

  const triggerOpenBubble = (id: string, el: HTMLDivElement): void => {
    const bubbleFound = bubbles.find((b) => b.id === id);
    if (!bubbleFound) return;
    const settings = bubbleFound.settings;
    openBubbleSettings &&
      openBubbleSettings({
        anchorEl: el,
        settings,
      });
  };

  const updateBubbleStyles = (config: BubbleSettingsType): void => {
    const {
      id,
      backgroundColor,
      borderWidth,
      borderColor,
      textColor,
      text,
      fontWeight,
      fontStyle,
      fontSize,
    } = config;
    const bubbleFound = bubbles.find((b) => b.id === id);
    if (!bubbleFound) return;
    const bubble = document.getElementById(bubbleFound.id);
    const bubbleText = document.getElementById(`bubble-text-${bubbleFound.id}`);
    const arrowPolygon = document.getElementById(bubbleFound.arrowPolygonId);
    if (!bubble || !bubbleText || !arrowPolygon) return;
    bubble.style.backgroundColor = backgroundColor;
    bubble.style.border = `${borderWidth}px solid ${borderColor}`;

    arrowPolygon.setAttribute(
      'style',
      `fill:${backgroundColor};stroke:${borderColor};stroke-width:${borderWidth};`
    );

    bubbleText.style.color = textColor;
    bubbleText.style.fontWeight = fontWeight; // normal | bold
    bubbleText.style.fontStyle = fontStyle; // normal | italic
    bubbleText.style.fontSize = fontSize;
    bubbleText.innerHTML = text;

    const bubblesupdated = bubbles.map((b) => {
      if (b.id !== id) return b;
      return {
        ...b,
        settings: config,
      };
    });
    bubbles = bubblesupdated;
  };

  const removeBubbleData = (id: string): void => {
    const bubbleUpdated = bubbles.filter((b) => b.id !== id);
    bubbles = bubbleUpdated;
  };

  const editBubbleText = (id: string, text: string): void => {
    const textEl = document.getElementById(`bubble-text-${id}`);
    if (!textEl) return;
    textEl.innerText = text;
  };

  const placeBubbleMove = (): void => {
    map.getCanvas().style.cursor = 'crosshair';
  };

  const placeBubbleClick = (e: MapEventType['click']): void => {
    map.getCanvas().style.cursor = '';
    map.off('mousemove', placeBubbleMove);
    createMarker(e);
    map.fire('bubble-created');
  };

  const placeBubble = (): void => {
    map.off('mousemove', placeBubbleMove);
    map.off('click', placeBubbleClick);
    map.on('mousemove', placeBubbleMove);
    map.once('click', placeBubbleClick);
  };

  return {
    init,
    placeBubble,
    editBubbleText,
    onEditBubble,
    updateBubbleSettings: updateBubbleStyles,
  };
};

export default bubbleService();
