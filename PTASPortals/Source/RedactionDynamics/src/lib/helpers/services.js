//-----------------------------------------------------------------------
// <copyright file="services.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import strings from "../../strings.json";
import http from "../../services/httpService";
import { isTifImage } from "./util";
import { base64StringToBlob } from "blob-util";

export /**
 * Gets the files in the requested document from either SharePoint or ILinx document storages.
 *
 * @param {string} documentId
 * @param {string} token
 * @param {bool} isSharePoint
 * @param {function} onError
 * @returns
 */
const getDocument = async (documentId, token, isSharePoint, onError) => {
  if (!documentId && onError) {
    onError(`${strings.error}${strings.failedToFetch}`);
    return null;
  }

  if (isSharePoint) {
    return await getDocumentFromSharePoint(documentId, token, onError);
  } else {
    return await getDocumentFromILinx(documentId, token, onError);
  }
};

/**
 * Returns the file information for a document in the ILinx file storage.
 *
 * @param {*} documentId
 * @param {*} token
 * @param {*} onError
 * @returns
 */
const getDocumentFromILinx = async (documentId, token, onError) => {
  let docInfo = null;
  try {
    const docFetcher = await fetch(
      `${process.env.REACT_APP_DOCS_API_URL}/documents/?id=${documentId}&TokenType=AAD`,
      {
        method: "GET",
        mode: "cors",
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json"
        }
      }
    );
    docInfo = await docFetcher.json();
  } catch (e) {
    // Will catch any failed or network errors
    if (onError) {
      onError(`${strings.error}${strings.failedToFetch}`);
    }
    return null;
  }

  return docInfo.files;
};

/**
 * Returns all file from inside a SharePoint folder with the matching document id.
 *
 * @param {*} documentId
 * @param {*} token
 * @param {*} onError
 * @returns
 */
const getDocumentFromSharePoint = async (documentId, token, onError) => {
  let docInfo = null;
  try {
    const docFetcher = await fetch(
      `${
        process.env.REACT_APP_SHAREPOINT_API_URL
      }/GetFileCollection/${documentId}?${
        process.env.REACT_APP_SHAREPOINT_API_CODE
          ? `code=${process.env.REACT_APP_SHAREPOINT_API_CODE}&`
          : ""
      }TokenType=AAD`,
      {
        method: "GET",
        mode: "cors",
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json"
        }
      }
    );
    docInfo = await docFetcher.json();
  } catch (e) {
    // Will catch any failed or network errors
    if (onError) {
      onError(`${strings.error}${strings.failedToFetch}`);
    }
    return null;
  }

  return docInfo;
};

export /**
 * Returns an specific file from the either SharePoint or ILinx document stores.
 *
 * @param {string | object} file
 * @param {string} token
 * @param {bool} isSharePoint
 * @returns
 */
const getFile = async (file, token, isSharePoint) => {
  if (isSharePoint) {
    return await getFileFromSharePoint(file, token);
  } else {
    return await getFileFromILinx(file.fileName, token);
  }
};

/**
 * Get a file from the ILinx storage.
 *
 * @param {string } fileId
 * @param {string} token
 * @returns
 */
const getFileFromILinx = async (fileId, token, retries = 0) => {
  const instance = http.create({
    url: `/Files?Id=${fileId}&TokenType=AAD`,
    method: "get",
    timeout: process.env.REACT_APP_API_TIMEOUT,
    baseURL: process.env.REACT_APP_DOCS_API_URL,
    headers: {
      Authorization: `Bearer ${token}`
    },
    responseType: "blob"
  });

  try {
    const { data } = await instance.request();
    return data;
  } catch (e) {
    // try to get the file again max 3 times.
    if (retries < 3) {
      retries += 1;
      return await getFileFromILinx(fileId, token, retries);
    }

    return null;
  }
};

/**
 *
 *
 * @param {*} fileId
 * @param {*} token
 * @param {*} tiffsAsTiffs
 * @param {number} [retries=0]
 * @returns
 */
const getJsonFileFromSharePoint = async (
  fileId,
  token,
  tiffsAsTiffs,
  retries = 0
) => {
  const instance = http.create({
    url: `/GetOneFile/${fileId}?${
      process.env.REACT_APP_SHAREPOINT_API_CODE
        ? `code=${process.env.REACT_APP_SHAREPOINT_API_CODE}&`
        : ""
    }TokenType=AAD&tiffsAsTiffs=${tiffsAsTiffs}`,
    method: "get",
    timeout: process.env.REACT_APP_API_TIMEOUT,
    baseURL: process.env.REACT_APP_SHAREPOINT_API_URL,
    headers: {
      Authorization: `Bearer ${token}`
    },
    responseType: "application/json"
  });
  try {
    const { data } = await instance.request();
    if (data.length) {
      return data.map(file => {
        const blob = base64StringToBlob(file.file, file.type);
        blob.name = file.name;
        return {
          blob,
          fileName: file.name
        };
      });
    }
  } catch (e) {
    if (retries < 3) {
      retries += 1;
      return await getJsonFileFromSharePoint(
        fileId,
        token,
        tiffsAsTiffs,
        retries
      );
    }

    return null;
  }
};

/**
 * Gets one file from the SharePoint storage.
 *
 * @param {string} fileId
 * @param {string } token
 * @returns
 */
const getFileFromSharePoint = async (fileId, token, retries = 0) => {
  const fileParts = fileId.split("/");
  const isTif = isTifImage(fileParts[fileParts.length - 1]);
  const instance = http.create({
    url: `/GetOneFile/${fileId}?${
      process.env.REACT_APP_SHAREPOINT_API_CODE
        ? `code=${process.env.REACT_APP_SHAREPOINT_API_CODE}&`
        : ""
    }TokenType=AAD${isTif ? "&tiffsAsTiffs=false" : ""}`,
    method: "get",
    timeout: process.env.REACT_APP_API_TIMEOUT,
    baseURL: process.env.REACT_APP_SHAREPOINT_API_URL,
    headers: {
      Authorization: `Bearer ${token}`
    },
    responseType: "blob"
  });

  try {
    const { data, headers } = await instance.request();

    // If values returned is a json object, pull it again as json.
    if (headers["content-type"] && headers["content-type"].includes("json")) {
      return await getJsonFileFromSharePoint(fileId, token, !isTif, retries);
    } else {
      return data;
    }
  } catch (e) {
    // try to get the file again max 3 times.
    if (retries < 3) {
      retries += 1;
      return await getFileFromSharePoint(fileId, token, retries);
    }

    return null;
  }
};

export /**
 * Updates the files of an specific document.
 *
 * @param {string} documentId
 * @param {FormData} payload
 * @param {string} token
 * @param {bool} isSharePoint
 * @param {function} onError
 * @returns
 */
const updateDocument = async (
  documentId,
  payload,
  token,
  isSharePoint,
  onError
) => {
  if (isSharePoint) {
    return await updateDocumentSharePoint(documentId, payload, token, onError);
  } else {
    return await updateDocumentILinx(documentId, payload, token, onError);
  }
};

/**
 * Updates a document on the ILinx file store.
 *
 * @param {string} documentId
 * @param {FormData} payload
 * @param {string} token
 * @param {function} onError
 * @returns
 */
const updateDocumentILinx = async (documentId, payload, token, onError) => {
  try {
    payload.append("documentId", documentId);
    const response = await fetch(
      `${process.env.REACT_APP_DOCS_API_URL}/documents/?TokenType=AAD`,
      {
        method: "PATCH",
        body: payload,
        mode: "cors",
        headers: {
          Authorization: `Bearer ${token}`
        }
      }
    );

    return await response.json();
  } catch (e) {
    // Will catch any failed or network errors
    if (onError) {
      onError(`${strings.error}${strings.failedToFetch}`);
    }
  }
};

/**
 * Updates a document on the SharePoint repository.
 *
 * @param {string} documentId
 * @param {FormData} payload
 * @param {string} token
 * @param {function} onError
 * @returns
 */
const updateDocumentSharePoint = async (
  documentId,
  payload,
  token,
  onError
) => {
  try {
    const response = await fetch(
      `${
        process.env.REACT_APP_SHAREPOINT_API_URL
      }/PutFileCollection/${documentId}?${
        process.env.REACT_APP_SHAREPOINT_API_CODE
          ? `code=${process.env.REACT_APP_SHAREPOINT_API_CODE}&`
          : ""
      }TokenType=AAD`,
      {
        method: "POST",
        body: payload,
        mode: "cors",
        headers: {
          Authorization: `Bearer ${token}`
        }
      }
    );

    return await response.json();
  } catch (e) {
    // Will catch any failed or network errors
    if (onError) {
      onError(`${strings.error}${strings.failedToFetch}`);
    }
  }
};
