// useScrollBehaviorBak.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { throttle } from 'lodash';
import { Dispatch, SetStateAction, useEffect, useRef, useState } from 'react';

interface UseScrollBehavior {
  hideHeader: boolean;
  setHideHeader: Dispatch<SetStateAction<boolean>>;
  contentRef: React.MutableRefObject<unknown>;
}

export type HomeParams = {
  contact: string;
};

/**
 * Listens to scroll events to hide (scroll down) or show (scroll up) the page header
 */
function useScrollBehaviorBak(): UseScrollBehavior {
  const [hideHeader, setHideHeader] = useState(false); //Set as prop "hideHeader" to MainLayout element
  const contentRef = useRef(); //Ref of element on which the scroll behavior is applied. E.g. <CustomCard ref={contentRef}

  useEffect(() => {
    let previousPosition = ((contentRef.current as unknown) as HTMLElement)
      .scrollTop;
    console.log('Init pos:', previousPosition);
    const onScroll = throttle(e => {
      console.log('e:', e);
      const scrollTop = e.target.scrollTop;
      console.log('scrollTop:', scrollTop);

      if (scrollTop > previousPosition) {
        console.log('down...');
        setHideHeader(true);
      } else {
        console.log('up...');
        setHideHeader(false);
      }
      previousPosition = scrollTop;
    }, 25);

    if (contentRef && contentRef.current) {
      ((contentRef.current as unknown) as HTMLElement).addEventListener(
        'scroll',
        onScroll
      );
    }

    return (): void => window.removeEventListener('scroll', onScroll);
  }, []);

  return {
    hideHeader,
    setHideHeader,
    contentRef,
  };
}

export default useScrollBehaviorBak;
