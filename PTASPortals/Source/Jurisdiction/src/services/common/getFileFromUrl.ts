// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import axios from 'axios';
/**
 * Get the mime type file through the file extension
 */
const getMimeTypeByExt = (ext: string): string => {
  switch (ext) {
    case 'xlsx':
    case 'xls':
      return 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
    case 'doc':
    case 'docx':
      return 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
    case 'png':
      return 'image/png';
    case 'jpg':
      return 'image/jpeg';
    case 'pdf':
      return 'application/pdf';
    default:
      return '';
  }
};

/**
 * Gets the contents of the file
 */
export const readFileAsync = (
  file: File
): Promise<string | ArrayBuffer | null> => {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();

    reader.onload = (): void => {
      resolve(reader.result);
    };

    reader.onerror = reject;

    reader.readAsArrayBuffer(file);
  });
};

/**
 * Gets a blob file
 */
export const getFileFromUrl = async (url: string): Promise<File> => {
  // get latest item
  const fileName = url.split('/').slice(-1)[0];
  const ext = url.split('.').slice(-1)[0];

  const { data: axiosData } = await axios.get(url, {
    responseType: 'blob',
  });

  const fileRes = new File([axiosData], fileName, {
    type: getMimeTypeByExt(ext),
  });

  return fileRes;
};
