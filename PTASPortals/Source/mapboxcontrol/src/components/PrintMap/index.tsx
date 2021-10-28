// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { MutableRefObject, RefObject, useRef } from 'react';
import { useReactToPrint } from 'react-to-print';
import kingCountyLogo from '../../assets/img/king-county-logo.svg';

interface Props {
  imgSrc: string;
  btnPrintRef: MutableRefObject<HTMLButtonElement | undefined>;
}

const PrintMap = (props: Props): JSX.Element => {
  const componentRef = useRef<HTMLDivElement>();
  const handlePrint = useReactToPrint({
    content: () => componentRef.current as HTMLDivElement,
  });

  return (
    <div style={{ display: 'none' }}>
      <button
        ref={props.btnPrintRef as RefObject<HTMLButtonElement>}
        onClick={handlePrint}
      >
        trigger print
      </button>
      <div
        id="map-print"
        ref={componentRef as RefObject<HTMLDivElement>}
        style={{
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'space-evenly',
          padding: '19px',
        }}
      >
        <div
          style={{
            display: 'flex',
            paddingTop: '1%',
            paddingBottom: '1%',
          }}
        >
          <span
            style={{
              display: 'flex',
              flexDirection: 'row',
              alignItems: 'center',
              justifyContent: 'center',
              padding: '10px',
              marginRight: '15px',
            }}
          >
            <img src={kingCountyLogo} alt="King county logo" />
          </span>
          <span
            style={{
              fontSize: '10pt',
            }}
          >
            The information included on this map has been compiled by King
            County staff from a variety of sources and is subject to change
            without notice. King County makes no representations or warranties,
            express or implied, as to accuracy, completeness, timeliness, or
            rights to the use of such information. King County shall not be
            liable for any general, special, indirect, incidental, or
            consequential damages including, but not limited to, lost revenues
            or lost profits resulting from the use or misuse of the information
            contained on this map. Any sale of this map or information on this
            map is prohibited except by written permission of King County.
          </span>
        </div>
        <div>
          <img
            src={props.imgSrc}
            alt="map preview"
            style={{
              height: 'auto',
              width: '100%',
            }}
          />
        </div>
      </div>
    </div>
  );
};

export default PrintMap;
