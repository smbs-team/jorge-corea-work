import React, { Fragment } from "react";
import {
  OfficeFilePreview,
  FilePreview,
  PdfFilePreview,
  CustomExcelReader
} from "@ptas/react-public-ui-library";
import { Box } from "@material-ui/core";
import { useState } from "react";

const FilePreviewComp = () => {
  const [file, setFile] = useState<File | string>(
    "http://www.africau.edu/images/default/sample.pdf"
  );

  const [excelFile, setExcelFile] = useState<File | string>(
    "http://localhost:3000/Permit.xlsx"
  );

  return (
    <Fragment>
      <Box style={{ marginBottom: "30px" }}>
        <FilePreview />
      </Box>
      <Box style={{ height: 500, marginBottom: "30px" }}>
        {/* <OfficeFilePreview fileUrl='https://www.amity.edu/aiit/icrito2016/ieee-format.doc' /> */}
        <OfficeFilePreview fileUrl='https://ptasdevstorage.blob.core.windows.net/permits/90038d15-dbce-440d-ba4c-4e4da56cb502/test2-permit.xlsx?sp=r&st=2021-06-22T16:05:53Z&se=2021-06-23T00:05:53Z&sv=2020-02-10&sr=b&sig=ueKvMNd2FzhNTAI8FDtL2gvb3EalXZ1633RNnFBe0yM%3D' />
      </Box>
      <Box style={{ marginBottom: "30px" }}>
        <Box>
          PDF File:{" "}
          <input
            type='file'
            onChange={(evt) => {
              if (evt.target.files?.length) {
                setFile(evt.target.files[0]);
              }
            }}
          ></input>
        </Box>
        <Box>
          <PdfFilePreview elementId='pdfBody' file={file} />
        </Box>
      </Box>
      <Box style={{ marginBottom: "30px" }}>
        <Box>
          Excel File:
          <input
            type='file'
            onChange={(evt) => {
              if (evt.target.files?.length) {
                setExcelFile(evt.target.files[0]);
              }
            }}
          ></input>
        </Box>
        <Box>
          <CustomExcelReader file={excelFile} />
        </Box>
      </Box>
    </Fragment>
  );
};

export default FilePreviewComp;
