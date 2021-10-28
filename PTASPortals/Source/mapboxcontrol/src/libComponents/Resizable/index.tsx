// Resizable.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useRef, PropsWithChildren, useState, useEffect } from 'react';
import {
  createStyles,
  withStyles,
  WithStyles,
  Divider,
  StyleRules,
} from '@material-ui/core';
import { useMount } from 'react-use';
import { useWindowSize } from 'react-use';

const DEFAULT_MIN_HEIGH = 50;
const MAX_HEIGHT_VH = 83;

const styles = (): StyleRules<string, ResizableProps> =>
  createStyles({
    root: (props) => ({
      overflow: 'hidden',
      maxHeight: MAX_HEIGHT_VH + 'vh',
      minHeight: props.minHeight ?? DEFAULT_MIN_HEIGH,
    }),
    resizerToggle: {
      width: '100%',
      height: '12px',
      background: 'transparent',
      position: 'absolute',
      right: '0',
      bottom: '0',
      cursor: 's-resize',
      zIndex: 1000,
      textAlign: 'center',
      userSelect: 'none',
    },
    resizerToggleLine: {
      width: '99%',
      marginTop: '6px',
      textAlign: 'center',
      '& hr': {
        border: 'thin solid #444444',
        marginLeft: '0.5%',
      },
    },
    divider: {
      background: '#444444',
    },
    dividerHandle: {
      width: '60px',
      height: '15px',
      position: 'absolute',
      bottom: '-2px',
      left: '48%',
      background: '#444444',
      borderRadius: '7px',
    },
    dividerHandleDot: {
      position: 'absolute',
      width: '4px',
      height: '4px',
      left: '29%',
      top: '35%',
      background: 'white',
    },
  });

interface Dimensions {
  width?: number;
  height: number;
}

export interface OnResizeFields extends Dimensions {
  initialRootRect: DOMRect;
  diff: Dimensions;
}

export interface ResizableProps {
  x?: boolean;
  y?: boolean;
  minHeight?: number;
  initialHeight?: number;
  onResize?: (fields: OnResizeFields) => void;
  onResizeStart?: (fields: OnResizeFields) => void;
  onResizeStop?: (fields: OnResizeFields) => void;
}

const Resizable = (
  props: PropsWithChildren<ResizableProps> & WithStyles<typeof styles>
): JSX.Element => {
  const rootRef = useRef<HTMLDivElement | null>(null);
  const resizeLineRef = useRef<HTMLDivElement | null>(null);
  const [dimensions, setDimensions] = useState<Dimensions>();
  const dimensionsRef = useRef(dimensions);
  const initialRootRect = useRef<DOMRect>();
  const startY = useRef<number>(0);
  const startHeight = useRef<number>(0);
  const minHeight = props.minHeight || DEFAULT_MIN_HEIGH;
  const { onResize } = props;
  const resizing = useRef(false);
  const { height: windowHeight } = useWindowSize();

  const getHeightDiff = useRef(
    () =>
      (dimensionsRef.current?.height || 0) -
      (initialRootRect.current?.height || 0)
  );

  const _getBoxSizes = (): Partial<Dimensions> => {
    return dimensions?.height && dimensions.height >= 0
      ? { height: dimensions.height }
      : {};
  };

  const _onResize = useRef((fields: OnResizeFields) => {
    onResize && onResize(fields);
  });

  const doResize = useRef((e: MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    const newHeight = startHeight.current + e.clientY - startY.current;
    const windowH = window.innerHeight;
    if (newHeight < minHeight) return;
    if (e.clientY > windowH - (windowH * 5) / 100) return;
    resizing.current = true;
    setDimensions({
      height: newHeight,
    });
  });

  const stopResizing = useRef((): void => {
    window.removeEventListener('mousemove', doResize.current);
    window.removeEventListener('mouseup', stopResizing.current);
    const heightDiff = getHeightDiff.current();
    const currentHeight = dimensionsRef.current?.height ?? 0;
    const maxHeight = windowHeight * (MAX_HEIGHT_VH / 100);

    if (dimensionsRef.current && initialRootRect.current) {
      props.onResizeStop?.({
        ...dimensionsRef.current,
        height: currentHeight > maxHeight ? maxHeight : currentHeight,
        initialRootRect: initialRootRect.current,
        diff: {
          height: heightDiff,
        },
      });
    }
  });

  const initDrag = (e: React.MouseEvent<HTMLDivElement, MouseEvent>): void => {
    if (!rootRef.current) return;
    startY.current = e.clientY;
    startHeight.current = rootRef.current.getBoundingClientRect().height;
    props.onResizeStart &&
      dimensionsRef.current &&
      initialRootRect.current &&
      props.onResizeStart({
        ...dimensionsRef.current,
        initialRootRect: initialRootRect.current,
        diff: {
          height: getHeightDiff.current(),
        },
      });
    window.addEventListener('mousemove', doResize.current);
    window.addEventListener('mouseup', stopResizing.current);
  };

  useEffect(() => {
    initialRootRect.current &&
      dimensions &&
      _onResize.current({
        ...dimensions,
        initialRootRect: initialRootRect.current,
        diff: {
          height: getHeightDiff.current(),
        },
      });
    dimensionsRef.current = dimensions;
  }, [dimensions, getHeightDiff]);

  useMount(() => {
    if (!rootRef.current) return;
    initialRootRect.current = rootRef.current.getBoundingClientRect();
    setDimensions({
      height: props.initialHeight ?? initialRootRect.current.height - 1,
    });
  });

  return (
    <div
      className={props.classes.root}
      ref={rootRef}
      style={{ ..._getBoxSizes() }}
    >
      {props.children}
      <div
        ref={resizeLineRef}
        className={props.classes.resizerToggle}
        onMouseDown={initDrag}
      >
        <div className={props.classes.resizerToggleLine}>
          <Divider className={props.classes.divider} />
          <div className={props.classes.dividerHandle}>
            <div
              className={props.classes.dividerHandleDot}
              style={{ left: '29%' }}
            ></div>
            <div
              className={props.classes.dividerHandleDot}
              style={{ left: '48%' }}
            ></div>
            <div
              className={props.classes.dividerHandleDot}
              style={{ left: '66%' }}
            ></div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default withStyles(styles)(Resizable);
