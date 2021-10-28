import React, { Fragment, useCallback, useEffect, useState } from "react";
import { Theme } from "@material-ui/core/styles";
import {
  Box,
  RootRef,
  Paper,
  IconButton,
  Button,
  LinearProgress,
  createStyles,
  withStyles,
  WithStyles
} from "@material-ui/core";
import CloudUpload from "@material-ui/icons/CloudUpload";
import CheckIcon from "@material-ui/icons/CheckCircle";
import CancelIcon from "@material-ui/icons/Cancel";
import ZoomInIcon from "@material-ui/icons/ZoomIn";
import { useDropzone } from "react-dropzone";
import { v4 as uuid } from "uuid";
import GalleryView from "../GalleryView";
import utilService from "../services/common/utilService";
import DescriptionIcon from "@material-ui/icons/Description";

export interface CustomFile {
  fileName: string;
  /**
   * Indicates what is in the "content" field.
   * For image files, use only "publicUrl" or "base64"
   */
  contentType: "publicUrl" | "base64" | "arrayBuffer";
  /**
   * Content or URL of file, as indicated in @contentType
   */
  content: string | ArrayBuffer;
  /**
   * Object of file selected
   * The caller does not have to set this field, but it is
   * provided by the component in case the caller needs it
   */
  file?: File;
  /**
   * Used to indicate if the file is uploading.
   * The caller has to set this property to false when
   * the file has been uploaded, in order for this component
   * to stop displaying the loading indicator and display
   * the preview if possible.
   */
  isUploading?: boolean;
  /**
   * Used to indicate if the file did not pass custom validation
   * made by the caller
   */
  invalid?: boolean;

  /** Unique file id */
  id?: string;

  /**Hide remove button */
  hideRemoveButton?: boolean;
}

interface Props extends WithStyles<typeof styles> {
  /**
   * Valid file formats. For example:
   *  [
   *    { extension: ".png", mimeType: "image/png" },
   *    { extension: ".jpg", mimeType: "image/jpeg" }
   *  ]
   */
  attrAccept?: { extension: string; mimeType: string }[];
  /**
   * Max file size in KB
   */
  maxFileSize?: number;
  /**
   * Function called when the user dropped or selected files
   * @files files to be uploaded (the ones that passed previous validations)
   */
  onFileUpload: (files: CustomFile[]) => Promise<CustomFile[]>;
  /**
   * Array of files to display each on a box (with or without preview)
   */
  currentFiles: CustomFile[];
  /**
   * Function called when the presses the Remove button
   * @file file to be removed
   */
  onRemoveFile?: (file: CustomFile) => void;
  /**
   * Function called when the presses the Zoom-in button.
   * @file file to be zoomed in
   */
  onZoomIn?: (file: CustomFile) => void;
  /**
   * Function called when the user dropped or selected files.
   * Called after file type and size validation, and before
   * calling @onFileUpload prop
   * @files files to be uploaded (the ones that passed previous validations)
   */
  fileValidationFn?: (files: CustomFile[]) => Promise<CustomFile[]>;
  /**
   * Whether to show the full size dropzone area (as shown when there are no uploaded files)
   * or show the smaller version for additional files
   */
  bigDropzoneForAdditionalFiles?: boolean;
  /**
   * Whether or not to show the upload dropzone after having some file uploaded
   */
  additionalUploads?: boolean;
  /**
   * Whether or not to allow dropping or selecting in file dialog multiple files at once
   */
  multipleFilesUploadAtOnce?: boolean;
  mainDropzoneText?: React.ReactNode;
  dragActiveText?: React.ReactNode;
  dragRejectText?: React.ReactNode;
  invalidFileSizeText?: React.ReactNode;
  /**
   * Message displayed when some file does not pass custom validation by caller, on prop @fileValidationFn
   */
  invalidFileText?: React.ReactNode;
}

const VALID_IMAGE_CONTENT_TYPES = ["publicUrl", "base64"];

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    rootDropzone: {
      width: 230,
      backgroundColor: theme.ptas.colors.theme.white,
      border: "2px dashed " + theme.ptas.colors.theme.gray,
      boxSizing: "border-box",
      cursor: "pointer",
      position: "relative",
      borderRadius: 0,
      margin: theme.spacing(1),
      "&:focus": {
        outline: "none"
      }
    },
    rootPreview: {
      width: 230,
      height: 230,
      backgroundColor: theme.ptas.colors.theme.white,
      border: "2px solid " + theme.ptas.colors.theme.black,
      boxSizing: "border-box",
      position: "relative",
      margin: theme.spacing(1)
    },
    dropzoneContent: {
      height: "100%",
      display: "flex",
      flexDirection: "column",
      justifyContent: "center",
      alignItems: "center",
      padding: "0 60px"
    },
    uploadIcon: {
      width: 112.5 /* 75x(3/2) because of space added by material-ui around the icon path */,
      height: 75 /* 50x(3/2)*/,
      color: theme.ptas.colors.theme.gray
    },
    text: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
      lineHeight: "19px",
      color: theme.ptas.colors.theme.gray,
      textAlign: "center"
    },
    select: {
      color: theme.ptas.colors.theme.accent,
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight
    },
    previewContent: {
      display: "flex",
      flexDirection: "column"
    },
    previewImage: {
      width: 226,
      height: 226,
      maxWidth: 226,
      maxHeight: 226,
      objectFit: "cover"
    },
    linearProgress: {
      width: "-webkit-fill-available",
      position: "absolute",
      top: 0,
      color: theme.ptas.colors.utility.selectionLight
    },
    linearProgressColorPrimary: {
      backgroundColor: theme.ptas.colors.theme.black
    },
    linearProgressBarColorPrimary: {
      backgroundColor: theme.ptas.colors.utility.selectionLight,
      height: 6
    },
    genericDocumentIcon: {
      width: 112.5 /* 75x(3/2) because of space added by material-ui around the icon path */,
      height: 75 /* 50x(3/2)*/,
      marginTop: 45,
      color: theme.ptas.colors.theme.gray,
      cursor: "pointer"
    },
    footer: {
      width: "-webkit-fill-available",
      position: "absolute",
      zIndex: 3,
      bottom: 0,
      padding: "3.5px 7px",
      cursor: "auto"
    },
    fileActionsBox: {
      display: "flex",
      justifyContent: "space-between"
    },
    zoomInButton: {
      padding: 0,
      color: theme.ptas.colors.theme.black
    },
    removeFileButton: {
      fontSize: theme.ptas.typography.buttonSmall.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "14px"
    },
    cancelIcon: {
      width: 16,
      height: 16,
      verticalAlign: "bottom"
    },
    fileNameBox: {
      overflow: "hidden",
      fontSize: theme.ptas.typography.finePrintBold.fontSize,
      fontWeight: theme.ptas.typography.finePrintBold.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "15px",
      height: 15,
      overflowY: "hidden"
    },
    smallCheckIcon: {
      width: 14,
      height: 14,
      verticalAlign: "bottom",
      color: theme.ptas.colors.utility.success,
      marginRight: 2
    },
    smallCloudIcon: {
      width: 14,
      height: 14,
      verticalAlign: "bottom",
      color: theme.ptas.colors.theme.grayLight,
      marginRight: 2
    },
    errorLabelGeneral: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "16px",
      color: theme.ptas.colors.utility.danger,
      width: 230,
      boxSizing: "border-box",
      padding: theme.spacing(1)
    },
    dragActiveText: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "16px",
      color: theme.ptas.colors.theme.gray
    },
    dragRejectText: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "16px",
      color: theme.ptas.colors.utility.danger
    }
  });

function FileUpload(props: Props): JSX.Element {
  const {
    classes,
    invalidFileSizeText,
    dragActiveText,
    dragRejectText,
    mainDropzoneText
  } = { ...props };
  const [uploadedFiles, setUploadedFiles] = useState<CustomFile[]>([]);
  const [zoomImageSrc, setZoomImageSrc] = useState<string | null>(null);
  const [errorNode, setErrorNode] = useState<React.ReactNode | null>(null);
  const [uniqueId, setUniqueId] = useState<string>(uuid());

  useEffect(() => {
    if (props.currentFiles) {
      setUploadedFiles(props.currentFiles);
    }
  }, [props.currentFiles]);

  useEffect(() => {
    setUniqueId(uuid());
  }, [uploadedFiles]);

  const onDrop = useCallback(async (files: File[]) => {
    setErrorNode(null);

    if (!files[0]) {
      setErrorNode(dragRejectText ?? "Invalid file selection");
      return;
    }

    //Validate file size
    const filesToExclude: string[] = [];
    if (props.maxFileSize) {
      for (const file of files) {
        if (file.size / 1024 > props.maxFileSize) {
          setErrorNode(
            invalidFileSizeText ??
              "File size exceeds limit of " + props.maxFileSize + "KB"
          );
          filesToExclude.push(file.name);
        }
      }
    }
    files = files.filter((f) => filesToExclude.indexOf(f.name) < 0);

    if (files.length) {
      const customFiles = await getFilesContent(files);

      //Update this state so that the UI shows the files that are 'in validations'
      //or 'uploading', with the corresponding loading indicator
      setUploadedFiles((prev) => {
        return [...prev, ...customFiles];
      });

      //Custom validation by caller
      const validatedFiles = props.fileValidationFn
        ? await props.fileValidationFn(customFiles)
        : customFiles;

      const invalidFiles = validatedFiles.filter((f) => f.invalid);
      if (invalidFiles.length) {
        //Remove invalid files from state to update UI
        setUploadedFiles((prevFiles) => {
          const _newUploadedFiles: CustomFile[] = [];
          for (const prevFile of prevFiles) {
            if (
              !prevFile.isUploading ||
              !invalidFiles.find((f) => f.fileName === prevFile.fileName)
            ) {
              _newUploadedFiles.push(prevFile);
            }
          }
          return _newUploadedFiles;
        });
        setErrorNode(
          props.invalidFileText ?? "Some files did not pass validation"
        );
      }

      const filesToUpload = validatedFiles.filter((f) => !f.invalid);
      if (filesToUpload.length) {
        await props.onFileUpload(filesToUpload);
      }
    }
  }, [props.onFileUpload, props.maxFileSize, props.fileValidationFn]);
  //This function could be used to check why the files were rejected by
  //the dropzone control. Add it to the options param of useDropzone.
  // const onDropRejected = useCallback((files) => {
  //   // files[i].errors[j] is something like:
  //   //{code: "file-invalid-type", message: "File type must be one of image/png"}
  // }, []);

  const getFilesContent = async (files: File[]): Promise<CustomFile[]> => {
    if (!files || !files.length) return [];
    const customFiles: CustomFile[] = [];

    for (const file of files) {
      const arrayBuffer = await readFileAsync(file);
      customFiles.push({
        file,
        fileName: file.name,
        contentType: "arrayBuffer",
        content: arrayBuffer ? arrayBuffer : "",
        isUploading: true
      });
    }
    return customFiles;
  };

  const readFileAsync = (file: File): Promise<string | ArrayBuffer | null> => {
    return new Promise((resolve, reject) => {
      let reader = new FileReader();

      reader.onload = () => {
        resolve(reader.result);
      };

      reader.onerror = reject;

      reader.readAsArrayBuffer(file);
    });
  };

  const {
    getRootProps,
    getInputProps,
    isDragActive,
    isDragReject
  } = useDropzone({
    onDrop,
    accept: props.attrAccept?.map((item) => item.mimeType),
    // onDropAccepted,
    // onDropRejected,
    multiple: props.multipleFilesUploadAtOnce ?? true
  });
  const { ref, ...rootProps } = getRootProps();

  const handleZoom = (file: CustomFile) => () => {
    if (props.onZoomIn) {
      props.onZoomIn(file);
    } else {
      setZoomImageSrc(typeof file.content === "string" ? file.content : "");
    }
  };

  return (
    <Fragment>
      {uploadedFiles.length > 0 &&
        uploadedFiles.map((file, i) => (
          <Box key={`${uniqueId}-${i}`} className={classes.rootPreview}>
            {file.isUploading && (
              <LinearProgress
                classes={{
                  root: classes.linearProgress,
                  colorPrimary: classes.linearProgressColorPrimary,
                  barColorSecondary: classes.linearProgressBarColorPrimary
                }}
              />
            )}
            {!file.isUploading && (
              <Box className={classes.previewContent}>
                {utilService.isImageFile(file.fileName) &&
                  VALID_IMAGE_CONTENT_TYPES.indexOf(file.contentType) >= 0 &&
                  typeof file.content === "string" && (
                    <Fragment>
                      <Box>
                        <img
                          className={classes.previewImage}
                          src={file.content}
                        />
                      </Box>
                      <Box
                        style={{
                          position: "absolute",
                          zIndex: 2,
                          width: "100%",
                          height: "100%",
                          background:
                            "linear-gradient(to top, rgba(255,255,255,1), rgba(255,255,255,1) 10%, rgba(0,0,0,0) 50%)"
                        }}
                      />
                    </Fragment>
                  )}
                {!utilService.isImageFile(file.fileName) && (
                  <Box style={{ display: "flex", justifyContent: "center" }}>
                    <DescriptionIcon
                      className={classes.genericDocumentIcon}
                      onClick={handleZoom(file)}
                    />
                  </Box>
                )}
              </Box>
            )}
            {/* Footer of file preview */}
            <Box
              className={classes.footer}
              onClick={(evt) => evt.stopPropagation()}
            >
              <Box className={classes.fileActionsBox}>
                {/* Zoom in */}
                <IconButton
                  disabled={
                    file.isUploading ||
                    (!props.onZoomIn &&
                      !utilService.isImageFile(file.fileName)) ||
                    (!props.onZoomIn &&
                      utilService.isImageFile(file.fileName) &&
                      VALID_IMAGE_CONTENT_TYPES.indexOf(file.contentType) < 0)
                  }
                  className={classes.zoomInButton}
                  onClick={handleZoom(file)}
                >
                  <ZoomInIcon />
                </IconButton>
                {/* Remove file */}
                {!file.hideRemoveButton && (
                  <Button
                    disabled={file.isUploading}
                    className={classes.removeFileButton}
                    onClick={() => {
                      props.onRemoveFile?.(file);
                    }}
                  >
                    Remove <CancelIcon className={classes.cancelIcon} />
                  </Button>
                )}
              </Box>
              <Box className={classes.fileNameBox}>
                {!file.isUploading && (
                  <CheckIcon className={classes.smallCheckIcon} />
                )}
                {file.isUploading && (
                  <CloudUpload className={classes.smallCloudIcon} />
                )}
                {file.fileName ?? file.file?.name}
              </Box>
            </Box>
          </Box>
        ))}

      {(!uploadedFiles.length || props.additionalUploads !== false) && (
        <RootRef rootRef={ref}>
          <Paper
            className={classes.rootDropzone}
            style={{
              height:
                uploadedFiles.length === 0 ||
                props.bigDropzoneForAdditionalFiles
                  ? 230
                  : 80
            }}
            {...rootProps}
          >
            <input {...getInputProps()} />
            {!isDragActive && (
              <Box className={classes.dropzoneContent}>
                {(uploadedFiles.length === 0 ||
                  props.bigDropzoneForAdditionalFiles) && (
                  <CloudUpload
                    classes={{
                      root: classes.uploadIcon
                    }}
                  />
                )}
                <Box className={classes.text}>
                  {mainDropzoneText ?? (
                    <Fragment>
                      <span className={classes.select}>Select</span> or drag in
                      attachments
                    </Fragment>
                  )}
                </Box>
              </Box>
            )}
            {isDragActive && !isDragReject && (
              <Box className={classes.dragActiveText}>
                {dragActiveText ?? "Drop files here"}
              </Box>
            )}
            {isDragReject && (
              <Box className={classes.dragRejectText}>
                {dragRejectText ?? "Invalid file selection"}
              </Box>
            )}
          </Paper>
        </RootRef>
      )}
      {errorNode !== null && (
        <Box className={classes.errorLabelGeneral}>{errorNode}</Box>
      )}
      {zoomImageSrc && (
        <GalleryView
          open={!!zoomImageSrc}
          urlImages={[zoomImageSrc]}
          onClose={() => setZoomImageSrc(null)}
          showCloseButton
          showArrows={false}
        />
      )}
    </Fragment>
  );
}
export default withStyles(styles)(FileUpload);
