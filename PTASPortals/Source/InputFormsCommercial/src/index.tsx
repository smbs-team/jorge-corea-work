// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { LicenseManager } from '@ag-grid-enterprise/core';
import { setBasepath } from 'hookrouter';

setBasepath(process.env.PUBLIC_URL);

LicenseManager.setLicenseKey(
  'CompanyName=ZonesLLC_on_behalf_of_KingCounty,LicensedApplication=PTAS,LicenseType=SingleApplication,LicensedConcurrentDeveloperCount=3,LicensedProductionInstancesCount=1,AssetReference=AG-013871,ExpiryDate=15_March_2022_[v2]_MTY0NzMwMjQwMDAwMA==66c5140fe0a21937e56500329e634bfd'
);

ReactDOM.render(<App />, document.getElementById('root'));

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
