// useScrollBehavior.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { throttle } from 'lodash';
import { useEffect, useRef } from 'react';

interface UseScrollBehavior {
  headerRef: React.MutableRefObject<unknown>;
  contentRef: React.MutableRefObject<unknown>;
}

const THROTTLE_MS = 10;

export type HomeParams = {
  contact: string;
};

/**
 * Listens to scroll events in order to apply clipping path to scrolled up card content,
 * so it won't show behind the header card
 */
function useScrollBehavior(): UseScrollBehavior {
  const contentRef = useRef(); //Ref to card element with content that should scroll. E.g. <CustomCard ref={contentRef}
  const headerRef = useRef(); //Ref to card element with header that will stick to the top of the page

  useEffect(() => {
    const onScroll = throttle(
      (/*event*/) => {
        if (
          contentRef &&
          contentRef.current &&
          headerRef &&
          headerRef.current
        ) {
          const headerEl = (headerRef.current as unknown) as HTMLElement;
          const contentEl = (contentRef.current as unknown) as HTMLElement;
          const headerRect = headerEl.getBoundingClientRect();
          const contentRect = contentEl.getBoundingClientRect();

          if (headerRect.bottom > contentRect.top) {
            //Apply clipping path on content element
            const scrolledPx = headerRect.bottom - contentRect.top;
            const pxToCut = scrolledPx - 70; //70 is arbitrary, to not cut the bottom 70px of the content
            const percentage = (pxToCut * 100) / contentRect.height;
            contentEl.style.clipPath = `polygon(0 ${percentage}%, 100% ${percentage}%, 100% 100%, 0% 100%)`;
          } else {
            contentEl.style.clipPath = 'none';
          }
        }
      },
      THROTTLE_MS
    );

    window.addEventListener('scroll', onScroll);

    return (): void => window.removeEventListener('scroll', onScroll);
  }, []);

  return {
    contentRef,
    headerRef,
  };
}

export default useScrollBehavior;
