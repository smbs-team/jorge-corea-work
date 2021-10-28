import React, { useContext } from "react";
import {
  FileUpload,
  CustomFile,
  ErrorMessageContext
} from "@ptas/react-public-ui-library";
import { FormattedMessage } from "react-intl";
import { useEffect } from "react";
import { useState } from "react";
// import axios from "axios"; //Temporarilly commented out, due to issues on one dev environment

// const useStyles = makeStyles(() => ({
//   uploadRootPreview: {
//     border: "2px solid black"
//   }
// }));

const FileUploadComp = () => {
  // const classes = useStyles();
  const [currentFiles, setCurrentFiles] = useState<CustomFile[]>([]);
  useEffect(() => {
    getInitFiles();
  }, []);
  const { setErrorMessageState } = useContext(ErrorMessageContext);

  const getInitFiles = async () => {
    // const res = await axios({
    //   method: "post",
    //   // url: process.env.REACT_APP_UPLOAD_API_URL,
    //   url: "http://localhost:8080/api/download",
    //   headers: {}
    // });
    // console.log("Download res:", res);
    // if (res.data) {
    //   setCurrentFiles([
    //     {
    //       fileName: res.data.files[0].fileName,
    //       contentType: "base64",
    //       content: res.data.files[0].content
    //     },
    //     {
    //       fileName: res.data.files[1].fileName,
    //       contentType: "base64",
    //       content: res.data.files[1].content
    //     }
    //   ]);
    // }
  };

  const uploadFile = async (selectedFile: File) => {
    try {
      if (selectedFile) {
        // Creating a FormData object
        let fileData = new FormData();
        // Setting the 'image' field and the selected file
        fileData.set(
          "image", //This has to match with the field name on the back-end
          selectedFile,
          selectedFile.name //`${selectedFile.lastModified}-${selectedFile.name}`
        );
        // const res = await axios({
        //   method: "post",
        //   // url: process.env.REACT_APP_UPLOAD_API_URL,
        //   url: "http://localhost:8080/api/upload",
        //   data: fileData,
        //   headers: { "Content-Type": "multipart/form-data" }
        // });
        // return res;
      }
    } catch (error) {
      console.error("Upload error", error);
    }
  };

  return (
    <div style={{ display: "flex", flexDirection: "column" }}>
      <button
        onClick={() => {
          setErrorMessageState({
            open: true,
            type: "errorDetails",
            errorDesc: "Error test",
            onClickReport: () => console.log("report error...")
          });
        }}
      >
        Show error message
      </button>
      <button
        onClick={() => {
          setErrorMessageState({
            open: true
          });
        }}
      >
        Show error Simple
      </button>
      <div style={{ marginBottom: "30px" }}>
        <span style={{ display: "block", marginBottom: "15px" }}>
          Upload file
        </span>
        <FileUpload
          // classes={{
          //   rootPreview: classes.uploadRootPreview
          // }}
          attrAccept={[
            { extension: ".png", mimeType: "image/png" },
            { extension: ".jpg", mimeType: "image/jpeg" },
            { extension: ".pdf", mimeType: "application/pdf" }
          ]}
          maxFileSize={2048}
          dragRejectText={
            <FormattedMessage
              id='dragRejectText'
              defaultMessage='Invalid file selection.'
            />
          }
          bigDropzoneForAdditionalFiles={true}
          onFileUpload={async (files) => {
            console.log("uploading files:", files);
            //Before starting uploads, update the init files
            const uploading = files.map((f) => ({
              ...f,
              isUploading: false
            }));

            setCurrentFiles((prev) => [...prev, ...uploading]);

            // for (const file of files) {
            //   if (file.file) {
            //     const res = await uploadFile(file.file);
            //     console.log("Upload res:", res);
            //     if (res?.data) {
            //       const uploadedFile: CustomFile = {
            //         fileName: res.data.fileName,
            //         contentType: "base64",
            //         content: res.data.content
            //       };
            //       setCurrentFiles((prevFiles) => {
            //         const newFiles = [...prevFiles];
            //         for (const newFile of newFiles) {
            //           //If filename is changed, then an original filename field or an ID is required
            //           if (newFile.fileName === uploadedFile.fileName) {
            //             newFile.isUploading = false;
            //             newFile.contentType = uploadedFile.contentType;
            //             newFile.content = uploadedFile.content;
            //           }
            //         }
            //         return newFiles;
            //       });
            //     }
            //   }
            // }

            console.log("simulate long upload");
            return new Promise((resolve) => {
              setTimeout(() => {
                resolve(files);
              }, 3000);
            });
          }}
          currentFiles={currentFiles}
          onRemoveFile={(file) => {
            console.log("Removing ", file.fileName);
            setCurrentFiles((prev) =>
              prev.filter((f) => f.fileName !== file.fileName)
            );
          }}
          onZoomIn={() => console.log("Click on zoom")}
          // onZoomIn={(file) => console.log("Zoom in ", file)}
          fileValidationFn={(files) => {
            return new Promise((resolve, reject) => {
              //Uncomment next code to mark first file as invalid
              // if(files.length){
              //   files[0].invalid = true;
              // }
              setTimeout(() => resolve(files), 2000);
            });
          }}
          // additionalUploads={false}
          // multipleFilesUploadAtOnce={false}
        />
      </div>
    </div>
  );
};

export default FileUploadComp;
