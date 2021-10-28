// common.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import fileDownload from 'js-file-download';
import { apiService } from 'services/api';

export interface ExcelSheet {
  [sheetNum: string]: {
    headers?: string[];
    rows: string[][];
  };
}

export interface HeadCell {
  id: string;
  label: string;
  width?: string;
}

export const exportToExcel = async (
  columnsData: HeadCell[],
  rows: string[][],
  fileName: string
): Promise<void> => {
  const columns = columnsData.flatMap((h) => h.label);
  const toSend: ExcelSheet = {
    Sheet1: {
      headers: columns,
      rows: rows,
    },
  };

  const response = await apiService.convertFromJsonToExcel(toSend);
  if (!response) return;
  fileDownload(response, fileName + '.xlsx');
};
