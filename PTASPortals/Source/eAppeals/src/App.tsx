/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { createRef } from 'react';
import { useEffectOnce } from 'react-use';

function App(): JSX.Element {
  const frameRef = createRef<HTMLIFrameElement>();
  const iFrameUrl = `${process.env.REACT_APP_IFRAME_URL}?parcelsQuery=3224900010`;
  const sendMessage = () => {
    frameRef.current?.contentWindow?.postMessage(
      {
        type: 'SELECT_PARCELS',
        payload: ['12345678', '0011223344'],
      },
      iFrameUrl
    );
  };

  useEffectOnce(() => {
    window.addEventListener(
      'message',
      (event) => {
        if (event.origin !== new URL(process.env.REACT_APP_IFRAME_URL).origin)
          return;
        console.log('Message received');
        console.log(event);
      },
      false
    );
  });

  return (
    <div className="App">
      <header style={{ textAlign: 'center' }}>
        <h1>
          Embedded map test <button onClick={sendMessage}>Send message</button>
        </h1>
      </header>
      <section>
        <iframe
          ref={frameRef}
          title="embedded map"
          src={iFrameUrl}
          width={200}
          height={200}
          style={{ border: 'none' }}
        />
      </section>
    </div>
  );
}

export default App;
