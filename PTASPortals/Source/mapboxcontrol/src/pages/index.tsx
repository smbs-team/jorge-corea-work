/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { BackdropLoader } from 'components/common';
import { AppContext } from 'contexts';
import { DrawToolbarProvider } from 'contexts/DrawToolbarContext';
import React, { useContext } from 'react';
import { BrowserRouter, Route } from 'react-router-dom';
import Eappeals from './Eappeals';
import Home from './Home';
import TestPage from './TestPage';

export const Pages = (): JSX.Element => {
  const { showBackdrop } = useContext(AppContext);
  return (
    <BrowserRouter basename={process.env.PUBLIC_URL}>
      <DrawToolbarProvider>
        <Route exact path="/" component={Home} />
        <Route path="/eappeals" component={Eappeals} />
        <Route path="/test" component={TestPage} />
      </DrawToolbarProvider>
      {showBackdrop && <BackdropLoader />}
    </BrowserRouter>
  );
};
