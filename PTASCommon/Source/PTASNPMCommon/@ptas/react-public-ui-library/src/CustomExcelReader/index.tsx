// Profile.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  FC,
  forwardRef,
  MutableRefObject,
  useEffect,
  useImperativeHandle,
  useState
} from "react";
import { useStyles } from "./styles";
import * as XLSX from "xlsx";
import ArrowRightIcon from "@material-ui/icons/ArrowRight";
import { WithStyles, withStyles } from "@material-ui/core";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { v4 as uuid } from "uuid";
import axios from "axios";

interface Props extends WithStyles<typeof useStyles> {
  file?: File | string | XlsxObjectFormat;
  showSheetList?: boolean;
}

export interface XlsxObjectFormat {
  SheetNames: string[];
  [sheet: string]: XLSX.WorkSheet;
}

export interface RefAttr {
  downloadExcel: () => void;
}

function ExcelReader(
  props: Props,
  ref:
    | MutableRefObject<RefAttr | null>
    | ((instance: RefAttr | null) => void)
    | null
): JSX.Element {
  const { classes, showSheetList, file } = props;
  // eslint-disable-next-line
  const [excelItems, setExcelItems] = useState<any[]>([]);
  const [columns, setColumns] = useState<
    {
      name: string;
      key: number;
    }[]
  >([]);
  const [workSheetsName, setWorkSheetsName] = useState<string[]>([]);
  const [wbSelected, setWbSelected] = useState<string>("");
  const [allSheets, setAllSheets] = useState<{
    [sheet: string]: XLSX.WorkSheet;
  }>({});

  const getFileFromUrl = async (url: string): Promise<File> => {
    const { data: axiosData } = await axios.get(url, {
      responseType: "blob",
      headers: {
        "Access-Control-Allow-Origin": "*"
      },
      method: "HEAD"
    });

    // const response = await fetch(url, { mode: "no-cors", method: "HEAD" });
    // const data = await response.blob();
    // console.log("response", response);
    // console.log("data", data);
    const fileRes = new File([axiosData], "permits_review", {
      type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    });
    return fileRes;
  };

  const readXlsxFile = (fileBlob: File): void => {
    const reader = new FileReader();
    const rABS = !!reader.readAsBinaryString;
    reader.onload = (e): void => {
      /* Parse data */
      const bstr = e.target?.result;
      const wb = XLSX.read(bstr, { type: rABS ? "binary" : "array" });

      console.clear();
      console.log("work book -------->", wb);

      /* Get worksheet by workbook selected*/
      const wsName =
        wb.SheetNames.find((workBookName) => workBookName === wbSelected) ??
        wb.SheetNames[0];

      // set wsName for the first time
      if (!wbSelected) setWbSelected(wsName);

      // set workbook names for the first
      if (!workSheetsName.length) setWorkSheetsName(wb.SheetNames);

      // set all sheets to download latter
      setAllSheets(wb.Sheets);

      const ws = wb.Sheets[wsName];
      /* Convert array of arrays */
      const data = XLSX.utils.sheet_to_json(ws, { header: 1 });

      console.log("Sheet to json", data);
      /* Update state */
      setExcelItems(data as []);
      setColumns(makeCols(ws["!ref"]));
    };
    if (rABS) reader.readAsBinaryString(fileBlob);
    else reader.readAsArrayBuffer(fileBlob);
  };

  const readXlsxFromObject = (xlsxObject: XlsxObjectFormat): void => {
    /* Get worksheet by workbook selected*/
    const wsName =
      xlsxObject.SheetNames.find(
        (workBookName) => workBookName === wbSelected
      ) ?? xlsxObject.SheetNames[0];

    // set wsName for the first time
    if (!wbSelected) setWbSelected(wsName);

    // set workbook names for the first
    if (!workSheetsName.length) setWorkSheetsName(xlsxObject.SheetNames);

    // set all sheets to download latter
    setAllSheets(xlsxObject.Sheets);

    const ws = xlsxObject.Sheets[wsName];
    /* Convert array of arrays */
    const data = XLSX.utils.sheet_to_json(ws, { header: 1 });
    /* Update state */
    setExcelItems(data as []);
    setColumns(makeCols(ws["!ref"]));
  };

  /* generate an array of column objects */
  function makeCols(
    ref?: string
  ): {
    name: string;
    key: number;
  }[] {
    const o = [];
    const range = XLSX.utils.decode_range(ref ?? "");

    for (let i = 0; i <= range.e.c; ++i) {
      o.push({ name: XLSX.utils.encode_col(i), key: i });
    }

    return o;
  }

  useEffect(() => {
    processXlsxFile(file);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [wbSelected, file]);

  const processXlsxFile = async (
    fileData: File | string | undefined | XlsxObjectFormat
  ): Promise<void> => {
    if (!fileData) return;

    if (typeof fileData === "object")
      return readXlsxFromObject(fileData as XlsxObjectFormat);

    const xlsxFile =
      typeof fileData === "string" ? await getFileFromUrl(fileData) : fileData;

    readXlsxFile(xlsxFile);
  };

  const exportFile = (): void => {
    const wb = XLSX.utils.book_new();

    for (const sheetKey in allSheets) {
      XLSX.utils.book_append_sheet(wb, allSheets[sheetKey], sheetKey);
    }

    XLSX.writeFile(wb, "review_import.xlsx");
  };

  const handleClickSelectSheet = (sheetName: string) => (): void => {
    setWbSelected(sheetName);
  };

  const sheetListButton = (): JSX.Element[] | void => {
    if (!showSheetList) return;

    // list workBooks
    return workSheetsName.map((wb) => (
      <button
        className={`${classes.sheetButton} ${wb === wbSelected && "selected"}`}
        onClick={handleClickSelectSheet(wb)}
        key={uuid()}
      >
        {wb}
      </button>
    ));
  };

  useImperativeHandle<RefAttr, RefAttr>(ref, () => ({
    downloadExcel(): void {
      exportFile();
    }
  }));

  const renderColumns = (): JSX.Element[] => {
    return columns
      .filter((_, j) => j < excelItems[0].length)
      .map((c) => (
        <th className={classes.th} key={uuid()}>
          {c.name}
        </th>
      ));
  };

  const renderRows = (): JSX.Element[] => {
    return excelItems
      .filter((row) => row?.length)
      .map((r, i) => (
        <tr key={uuid()}>
          <td className={classes.th}>{i + 1}</td>
          {columns
            .filter((_, j) => j < excelItems[0].length)
            .map((c) => (
              <td className={classes.td} key={uuid()}>
                {r[c.key]}
              </td>
            ))}
        </tr>
      ));
  };

  return (
    <div className={classes.wrapTable}>
      {sheetListButton()}
      <table className={classes.table}>
        <thead>
          <tr>
            <th className={classes.th}>
              <ArrowRightIcon className={classes.arrowIcon} />
            </th>
            {renderColumns()}
          </tr>
        </thead>
        <tbody>{renderRows()}</tbody>
      </table>
    </div>
  );
}

export default withStyles(useStyles)(forwardRef(ExcelReader)) as FC<
  GenericWithStyles<Props>
>;
