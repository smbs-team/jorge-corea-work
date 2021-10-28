// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import axios from 'axios';

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
