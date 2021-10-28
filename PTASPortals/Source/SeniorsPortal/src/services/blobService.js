//-----------------------------------------------------------------------
// <copyright file="blobService.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import http from './httpService';
import { getAppInsightsInstance } from './telemetryService';
import { refreshToken } from '../contexts/MagicLinkContext';
import { getRootPath } from './dataServiceProvider';

const appInsights = getAppInsightsInstance();
let rootPath = getRootPath();

const instance = http.create({
  baseURL: `${process.env.REACT_APP_DOCS_API_URL}/api`,
  timeout: process.env.REACT_APP_API_TIMEOUT,
});

export async function getImage(id) {
  rootPath = getRootPath();

  try {
    const { data } = await instance.post('/blob64files', null, {
      headers: {
        Authorization: `Bearer ${refreshToken()}`,
      },
      responseType: 'application/json',
      params: {
        Id: id,
        contactId: rootPath.contactId,
      },
    });
    if (data) {
      data.fileBytes = `data:image/png;base64,${data.fileBytes}`;
    }

    return data;
  } catch (error) {
    //if (http.isCancel(error)) console.log('Upload Cancelled');
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
}

export async function getPdfImage(id) {
  try {
    const { data } = await instance.get('/extractedimage', null, {
      headers: {
        Authorization: `Bearer ${refreshToken()}`,
      },
      responseType: 'blob',
      params: {
        Id: id,
      },
    });

    return data;
  } catch (error) {
    //if (http.isCancel(error)) console.log('Upload Cancelled');
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
}

export async function uploadFile(
  image,
  appId,
  detailsId,
  section,
  document,
  checkImage = false
) {
  var bodyFormData = new FormData();
  bodyFormData.append('files[]', image, image.name);
  bodyFormData.append('seniorApplicationId', appId);
  if (detailsId) {
    bodyFormData.append('seniorApplicationDetailsId', detailsId);
  }

  bodyFormData.append('section', section);
  bodyFormData.append('document', document);
  // tells the API to use OCR before uploading the image.
  bodyFormData.append('checkImage', checkImage);

  try {
    await instance.post('/azurefileblob', bodyFormData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        Authorization: `Bearer ${refreshToken()}`,
      },
    });
  } catch (error) {
    //if (http.isCancel(error)) console.log('Upload Cancelled');
    console.log('current error', error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
}

export async function pdfToImg(pdf) {
  var bodyFormData = new FormData();
  bodyFormData.append('files[]', pdf);

  try {
    const { data } = await instance.post('/pdfConvert', bodyFormData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        Authorization: `Bearer ${refreshToken()}`,
      },
    });
    return data;
  } catch (error) {
    //if (http.isCancel(error)) console.log('Upload Cancelled');
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
}

export async function deleteBlob(id) {
  const instance = http.create({
    url: '/api/blobfiles',
    method: 'delete',
    timeout: process.env.REACT_APP_API_TIMEOUT,
    baseURL: process.env.REACT_APP_DOCS_API_URL,
    headers: {
      Authorization: `Bearer ${refreshToken()}`,
    },
    responseType: 'blob',
    params: {
      Id: id,
    },
  });

  try {
    await instance.delete('/blobfiles', null, {
      headers: {
        Authorization: `Bearer ${refreshToken()}`,
      },
      responseType: 'blob',
      params: {
        Id: id,
      },
    });
  } catch (error) {
    //if (http.isCancel(error)) console.log('Upload Cancelled');
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
}

/**
 *
 *
 * @export
 * @param {*} html
 * @param {*} fullName
 * @param {*} email
 * @param {*} returnUrl
 * @param {*} signerClientId
 * @returns
 */
export async function getRedirectedDocuSignUrlHtml(
  html,
  fullName,
  email,
  returnUrl,
  signerClientId
) {
  let bodyFormData = {
    htmlContent: html,
    signerClientId: signerClientId,
    returnUrl: returnUrl,
    email: email,
    fullName: fullName,
  };

  bodyFormData = JSON.stringify(bodyFormData);
  try {
    const { data } = await instance.post('/SignHtml', bodyFormData, {
      headers: {
        'content-type': 'application/json',
        Authorization: `Bearer ${refreshToken()}`,
      },
    });
    return data;
  } catch (error) {
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
}

export async function getRedirectedDocuSignUrl(
  files,
  fullName,
  email,
  returnUrl,
  signerClientId
) {
  var bodyFormData = new FormData();
  bodyFormData.append('files', files);
  bodyFormData.append('signerClientId', signerClientId);
  bodyFormData.append('returnUrl', returnUrl);
  bodyFormData.append('email', email);
  bodyFormData.append('fullName', fullName);

  try {
    const { data } = await instance.post('/DocuSign', bodyFormData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        Authorization: `Bearer ${refreshToken()}`,
      },
    });
    return data;
  } catch (error) {
    //if (http.isCancel(error)) console.log('Upload Cancelled');
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
}

export async function uploadDocuSignDocuments(
  envelopeId,
  seniorAppId,
  section,
  document
) {
  const instance = http.create({
    url: '/api/DocuSign',
    method: 'patch',
    timeout: process.env.REACT_APP_API_TIMEOUT,
    baseURL: process.env.REACT_APP_DOCS_API_URL,
    headers: {
      Authorization: `Bearer ${refreshToken()}`,
    },
    responseType: 'application/json',
    params: {
      envelopeId: envelopeId,
      seniorApplicationId: seniorAppId,
      section: section,
      document: document,
    },
  });

  try {
    const { data } = await instance.patch('/DocuSign', null, {
      headers: {
        Authorization: `Bearer ${refreshToken()}`,
      },
      responseType: 'application/json',
      params: {
        envelopeId: envelopeId,
        seniorApplicationId: seniorAppId,
        section: section,
        document: document,
      },
    });
    return data;
  } catch (error) {
    //if (http.isCancel(error)) console.log('Upload Cancelled');
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
}

export const createDocuSignHtml = async summaryData => {
  try {
    const { data } = await instance.post('/CreateSignDocument', summaryData, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${refreshToken()}`,
      },
    });
    return data;
  } catch (error) {
    //if (http.isCancel(error)) console.log('Upload Cancelled');
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
};

export const moveFilesToiLinx = async metadata => {
  const instance = http.create({
    url: '/api/blobMove',
    method: 'post',
    timeout: process.env.REACT_APP_API_TIMEOUT,
    baseURL: process.env.REACT_APP_DOCS_API_URL,
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${refreshToken()}`,
    },
    data: metadata,
  });

  try {
    const { data } = await instance.post('/blobMove', metadata, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${refreshToken()}`,
      },
    });
    return data;
  } catch (error) {
    //if (http.isCancel(error)) console.log('Upload Cancelled');
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
};

export /**
 * Calls API to generate a QR code based on the provided parameters
 *
 * @param {*} contactId
 * @param {*} seniorAppId
 * @returns json object with base64 image response
 */
const getQRCode = async (contactId, seniorAppId, email, qrToken) => {
  const payload = {
    r: `${contactId}/${seniorAppId}/uploads`,
    e: email,
    t: qrToken,
    env: process.env.REACT_APP_QR_ENV,
  };

  try {
    const { data } = await instance.post('/QR', payload, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${refreshToken()}`,
      },
    });
    return data;
  } catch (error) {
    //if (http.isCancel(error)) console.log('Upload Cancelled');
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
};

export /**
 *  Calls web service to apply the json to dynamics and the move to the files from the blob storage to the 
    configured permanent storage location.
 *
 * @param {*} contactId
 * @param {*} seniorAppId
 * @param {*} accountNumber
 * @param {*} docType
 * @param {*} rollYear
 */
const applyJsonAndMoveToPermanentStorage = async (
  contactId,
  seniorAppId,
  accountNumber,
  docType,
  rollYear
) => {
  if (
    process.env.REACT_APP_PERMANENTSTORAGE &&
    process.env.REACT_APP_PERMANENTSTORAGE === 'SHAREPOINT'
  ) {
    if (process.env.REACT_APP_DataToSubmit === 'FILES') {
      return await moveToSharePoint(
        contactId,
        seniorAppId,
        accountNumber,
        docType,
        rollYear
      );
    }
    return await applyJsonAndMoveToSharePoint(
      contactId,
      seniorAppId,
      accountNumber,
      docType,
      rollYear
    );
  } else {
    return await applyJsonAndMoveToIlinx(
      contactId,
      seniorAppId,
      accountNumber,
      docType,
      rollYear
    );
  }
};

export /**
 * Calls web service to apply the json to dynamics and the move to ilinx controller one after the other
 *
 * @param {string} contactId
 * @param {string} seniorAppId
 * @param {string} accountNumber
 * @param {string} docType
 * @param {string} rollYear
 * @returns
 */
const applyJsonAndMoveToIlinx = async (
  contactId,
  seniorAppId,
  accountNumber,
  docType,
  rollYear
) => {
  try {
    const { data } = await instance.post(
      '/ApplyJsonAndMoveToIlinx',
      {
        route: `${contactId}/${seniorAppId}`,
        blobId: seniorAppId,
        accountNumber: accountNumber,
        recId: '500',
        docType: docType,
        rollYear: rollYear,
      },
      {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${refreshToken()}`,
        },
      }
    );
    return data;
  } catch (error) {
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
};

export /**
 * Calls web service to apply the json to dynamics and then move to SharePoint controller one after the other.
 *
 * @param {string} contactId
 * @param {string} seniorAppId
 * @param {string} accountNumber
 * @param {string} docType
 * @param {string} rollYear
 * @returns
 */
const applyJsonAndMoveToSharePoint = async (
  contactId,
  seniorAppId,
  accountNumber,
  docType,
  rollYear
) => {
  try {
    const { data } = await instance.post(
      '/ApplyJsonAndMoveToSharepoint',
      {
        route: `${contactId}/${seniorAppId}`,
        blobId: seniorAppId,
        accountNumber: accountNumber,
        recId: '500',
        docType: docType,
        rollYear: rollYear,
      },
      {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${refreshToken()}`,
        },
      }
    );

    return data;
  } catch (error) {
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
};

export /**
 * Calls web service to move the files to SharePoint form the blob storage.
 *
 * @param {string} contactId
 * @param {string} seniorAppId
 * @param {string} accountNumber
 * @param {string} docType
 * @param {string} rollYear
 * @returns
 */
const moveToSharePoint = async (
  contactId,
  seniorAppId,
  accountNumber,
  docType,
  rollYear
) => {
  try {
    const { data } = await instance.post(
      '/BlobMoveToSharepoint',
      {
        route: `${contactId}/${seniorAppId}`,
        blobId: seniorAppId,
        accountNumber: accountNumber,
        recId: '500',
        docType: docType,
        rollYear: rollYear,
      },
      {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${refreshToken()}`,
        },
      }
    );

    return data;
  } catch (error) {
    console.log(error);
    appInsights.trackException({
      exception: new Error(error),
    });
  }
};
