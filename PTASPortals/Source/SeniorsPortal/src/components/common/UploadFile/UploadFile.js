import React, {
  useCallback,
  useState,
  useReducer,
  useEffect,
  useRef,
} from 'react';
import './UploadFile.css';
import { makeStyles, withStyles } from '@material-ui/core/styles';
import CloudUploadIcon from '@material-ui/icons/Photo';
import Image from '@material-ui/icons/ImageRounded';
import Info from '@material-ui/icons/InfoOutlined';
import ZoomIn from '@material-ui/icons/ZoomIn';
import DeleteForever from '@material-ui/icons/Cancel';
import Grid from '@material-ui/core/Grid';
import { FormattedMessage } from 'react-intl';
import CheckCircle from '@material-ui/icons/CheckCircle';
import HighlightOff from '@material-ui/icons/Cancel';
import LinearProgress from '@material-ui/core/LinearProgress';
import CircularProgress from '@material-ui/core/CircularProgress';
import { useDropzone } from 'react-dropzone';
import IconButton from '@material-ui/core/IconButton';
import Tooltip from '@material-ui/core/Tooltip';
import {
  isValidImage,
  cancelRequests,
  isValidImageRemote,
} from '../../../services/cognitiveService';
import { createUrl } from './helpers';
import ImageCrossOut from '../redaction-component/ImageCrossOut';
import CropperComponent from '../cropper-component/cropperComponent';

import AlertDialog from '../AlertDialog';

import { isMobile, renderIf } from '../../../lib/helpers/util';
import {
  uploadFile,
  deleteBlob,
  getImage,
  pdfToImg,
  getPdfImage,
} from '../../../services/blobService';

import {
  dataURLtoFile,
  replaceDotBySymbol,
  arrayNullOrEmpty,
} from '../../../lib/helpers/util';

//import imageCompression from 'browser-image-compression';
import jic from 'j-i-c';
import deepEqual from 'deep-equal';
import * as fm from './FormatTexts';
const ColorLinearProgress = withStyles({
  colorPrimary: {
    backgroundColor: 'black',
  },
  barColorPrimary: {
    backgroundColor: '#d4e693',
    height: 6,
  },
})(LinearProgress);

const useStyles = makeStyles(theme => ({
  button: {
    margin: theme.spacing(1),
  },
  buttonLink: {
    background: 'none!important',
    border: 'none',
    padding: '0!important',
    color: '#6f1f62',
    textDecoration: 'underline',
    cursor: 'pointer',
  },
  rightIconDrag: {
    color: '#7a7a7a',
    margin: '38px 71px -17px',
    fontSize: 88,
  },
  rightIconDragMobile: {
    color: '#6f1f62',
    marginLeft: 65,
    right: 72,
    fontSize: 38,
    float: 'left',
    paddingTop: 6,
  },
  rightIconButton: {
    marginLeft: theme.spacing(1),
  },
  checkIcon: {
    position: 'absolute',
    bottom: 4,
    color: '#43a047',
    left: 3,
    fontSize: 16,
  },
  invalidIcon: {
    position: 'absolute',
    color: '#d20000',
    bottom: 2,
    right: 0,
    fontSize: 16,
  },
  removeButton: {
    margin: theme.spacing(1),
  },
  progress: {
    margin: theme.spacing(2),
    display: 'inline-block',
  },
  img: {
    width: 100,
    margin: theme.spacing(1),
  },
  img2: {
    margin: 0,
    display: 'block',
    height: 210,
    width: 228,
    objectFit: 'cover',
  },

  img3: {
    margin: 0,
    display: 'block',
    height: 180,
    width: 228,
    objectFit: 'cover',
    marginBottom: 35,
  },
  root: {
    flexGrow: 1,
    marginBottom: 15,
    width: 230,
    height: 230,
  },
  rootError: {
    flexGrow: 1,
    marginBottom: 65,
    width: 229,
    height: 0,
  },
  paper: {
    position: 'relative',
    marginTop: 8,
    height: 235,
    maxWidth: 230,
    border: '1px solid #000',
    minHeight: 21,
  },
  paperError: {
    position: 'relative',
    padding: 0,
    marginTop: 8,
    height: 19,
    width: 230,
    border: '1px solid #d20000',
    cursor: 'pointer',
  },
  dropzone: {
    background: 'white',
    position: 'relative',
    cursor: 'pointer',
    width: 230,
    height: 230,
    border: '2px dashed #7a7a7a',
    fontSize: 12,
  },
  dropzoneError: {
    borderColor: '#d20000',
  },
  smallDropzone: {
    background: 'white',
    position: 'relative',
    cursor: 'pointer',
    width: 230,
    height: 76,
    border: '2px dashed #7a7a7a',
    fontSize: 12,
  },
  dropzoneMobile: {
    background: 'white',
    position: 'relative',
    cursor: 'pointer',
    width: 230,
    height: 48,
    borderRadius: 5,
    border: '1px solid #6f1f62',
    fontSize: 12,
    textAlign: 'center',
    marginLeft: 'auto',
    marginRight: 'auto',
  },
  gridLabelLeft: {
    fontSize: 14,
    position: 'absolute',
    top: -27,
    left: 0,
    width: 215,
    textAlign: 'left',
  },
  gridLabelRight: {
    fontSize: 14,
    position: 'absolute',
    top: -25,
    right: 21,
    color: 'rgba(0, 0, 0, 0.87)',
  },
  infoIcon: {
    position: 'absolute',
    fontSize: 20,
    zoom: 0.8,
    top: -46,
    right: -15,
    color: 'black',
  },
  imageLoadingIcon: {
    position: 'absolute',
    fontSize: 48,
    top: 45,
    right: 57,
    color: '#7a7a7a',
  },
  imageLoadingContainer: {
    position: 'relative',
    height: 230,
    width: 230,
    /*backgroundColor: 'white',*/
  },
  gridLabelDefaultMessage: {
    textAlign: 'center',
    margin: '14px 35px',
    color: '#7a7a7a',
    fontSize: 16,
  },
  gridLabelDefaultMessageMobile: {
    textAlign: 'center',
    marginLeft: 8,
    marginTop: 12,
    color: '#6f1f62',
    fontSize: 16,
    float: 'left',
  },
  gridLabelOnDrag: {
    color: '#9f9b9b',
    textAlign: 'center',
    width: '100%',
  },
  dropzoneActive: {
    borderColor: '#2196f3',
  },
  dropzoneAccept: {
    borderColor: '#00e676',
  },
  dropzoneReject: {
    borderColor: '#ff1744',
  },
  relative: {
    position: 'relative',
  },
  deleteIcon: {
    cursor: 'pointer',
    position: 'absolute',
    bottom: 0,
    right: -6,
    color: 'black',
    transform: 'scale(0.8)',
    fontSize: 14,
    fontWeight: 'bold',
    verticalAlign: 'middle',
  },
  deleteIconLoading: {
    cursor: 'pointer',
    position: 'absolute',
    top: -15,
    right: -15,
    color: 'black',
    transform: 'scale(1.14)',
  },
  zoomIcon: {
    cursor: 'pointer',
    position: 'absolute',
    bottom: 0,
    left: 2,
    color: 'black',
    fontSize: 14,
    transform: 'scale(1.14)',
  },
  label: {
    fontSize: 14,
    width: 417,
    textAlign: 'left',
  },

  uploadContainerLeft: {
    height: '264px',
    width: '58%',
    float: 'left',
  },
  infoRight: {
    width: '47%',
    marginLeft: 30,
    float: 'left',
    marginTop: 20,
  },
  fileNameLabel: {
    position: 'absolute',
    bottom: 4,
    left: 21,
    fontSize: 12,
    display: 'inline-block',
    width: 196,
    textOverflow: 'ellipsis',
    overflow: 'hidden',
    whiteSpace: 'nowrap',
    textAlign: 'left',
  },
  errorLabel: {
    position: 'absolute',
    top: 24,
    color: '#d20000',
    textAlign: 'center',
    width: '100%',
  },
  errorLabel2: {
    position: 'absolute',
    top: 20,
    textAlign: 'left',
    fontSize: 12,
    color: '#d20000',
    left: 0,
  },
  errorLabelGeneral: {
    fontSize: 12,
    color: '#d20000',
  },
  linearProgress: {
    color: '#d4e693',
    top: 0,
    left: -1,
  },
  labelsBox: {
    position: 'relative',
    marginTop: 25,
  },
  messageBox: {
    display: 'flex',
    flexDirection: 'row',
    alignItems: 'flex-start',
    justifyContent: 'space-between',
    marginTop: '-25px',
    paddingBottom: '8px',
    width: 228,
  },
  leftMessageBox: {
    width: 200,
    wordWrap: 'break-word',
  },
}));

const initialState = { fileArray: [] };

const fileArrayReducer = (state, action) => {
  let newState = null;
  switch (action.type) {
    case 'replace':
      return { fileArray: [...action.value] };
    case 'add':
      let add =
        action.value.url != ''
          ? { fileArray: [...state.fileArray, ...action.value] }
          : { fileArray: [...state.fileArray] };

      if (add && add.fileArray) {
        add.fileArray = add.fileArray
          //Dont show duplicate imageName
          //.filter(a => a.isLoading)
          .map(e => e['imageName'])
          // store the keys of the unique objects
          .map((e, i, final) => final.indexOf(e) === i && i)
          // eliminate the dead keys & store unique objects
          .filter(e => add.fileArray[e])
          .map(e => add.fileArray[e]);
      }

      return add;
    case 'update':
      newState = {
        fileArray: state.fileArray.map(a =>
          a.imageName === action.theImageName ? { ...a, ...action.value } : a
        ),
      };

      if (action.handler)
        action.handler(
          newState.fileArray.filter(a => a.isUploaded === true || a.url != '')
        );

      return newState;
    case 'set':
      return { fileArray: (state.fileArray = action.value.slice(0)) };
    case 'remove':
      newState = {
        fileArray: state.fileArray.filter(
          f => f.imageName !== action.imageName
        ),
      };

      if (action.handler)
        action.handler(
          newState.fileArray.filter(a => a.isUploaded === true && a.url != '')
        );
      return newState;
    case 'returnValidObjects':
      return {
        fileArray: state.fileArray.filter(
          a => a.isValid === true || a.isLoading === true
        ),
      };
    default:
      throw new Error();
  }
};

const UploadFile = React.forwardRef((props, ref) => {
  //

  const [state, dispatch] = useReducer(fileArrayReducer, initialState);
  const [error, setError] = useState(null);
  const [showRedactionTool, setShowRedactionTool] = useState(false);
  const [data, setData] = useState([]);
  // let latestProps = useRef(props);
  const [index, setIndex] = useState(0);
  const [currentImage, setCurrentImage] = useState(0);
  const [isPdf, setIsPdf] = useState(false);
  const [open, setOpen] = useState(false);
  const [alertText, setAlertText] = useState('');

  const {
    onArrayUpdate,
    isSummary,
    fileArray,
    helpText,
    onDelete,
    onCreate,
    required,
    scheduleid,
    currentData,
    currDataName,
    setParentState,
  } = props;

  // useEffect(() => {
  //   latestProps.current = props;
  // }, [props]);

  useEffect(() => {
    if (state.fileArray.length > 0) {
      setData(state.fileArray);

      let x = state.fileArray.filter(a => a.isUploaded === true && a.url != '');
      if (onArrayUpdate) onArrayUpdate(x);
    }
  }, [onArrayUpdate, state.fileArray]);

  useEffect(() => {
    if (!deepEqual(state.fileArray, fileArray)) {
      if (
        !arrayNullOrEmpty(state.fileArray) &&
        fileArray.length <= state.fileArray.length
      ) {
        dispatch({
          type: 'replace',
          value: arrayNullOrEmpty(fileArray) ? [] : fileArray,
        });
      } else {
        dispatch({
          type: 'add',
          value: arrayNullOrEmpty(fileArray) ? [] : fileArray,
        });
      }
    }

    // Do not add state.fileArray it causes a loop
  }, [fileArray]);

  useEffect(() => {
    setAlertText(helpText);
  }, [helpText]);

  const handleFile = useCallback(async input => {
    const maxFileSize = fm.maxFileSize;
    const maxPDFFileSize = fm.maxPDFFileSize;
    const analysisError = fm.analysisError;
    const invalidDocumentType = fm.invalidDocumentType;
    const fileAlreadyProcessed = fm.fileAlreadyProcessed;
    let thiserr = '';
    //Get only valid objects
    dispatch({ type: 'returnValidObjects' });

    if (!input[0]) {
      setError(invalidDocumentType);
      return;
    }

    let showRedact = false;
    const inputPromises = input.map(async element => {
      let image = element;
      const imageName = image.name;
      setError('');

      /*if (state.fileArray) {
        state.fileArray
         /// .filter(d => d.url && d.url != '')
          .forEach((v, i) => {
            if (v.imageName === image.name) {
              return setError(fileAlreadyProcessed);
            }
          });
      }*/

      const extension = image.type;
      const ext = extension.split('/').pop();

      if (!ext.match(/^(jpeg|jpg|png|gif|bmp|pdf)$/)) {
        setError(invalidDocumentType);
        return;
      }

      if (ext.match(/^(pdf)$/)) {
        setIsPdf(true);
        let response = await pdfToImg(image);
        setIsPdf(false);
        const reg = /.pdf/i;
        if (response) {
          const promises = response.images.map(async (url, index) => {
            let pdfName = `${imageName.replace(reg, '')}${index}.png`;

            if (state.fileArray) {
              state.fileArray
                .filter(d => d.url && d.url != '')
                .forEach((v, i) => {
                  if (v.imageName === pdfName) {
                    return setError(fileAlreadyProcessed);
                  }
                });
            }

            const objLoading = [{ imageName: pdfName, isLoading: true }];

            dispatch({
              type: 'add',
              value: objLoading,
            });
            const arrayImage = `data:image/png;base64,${url}`;

            const filePdf = dataURLtoFile(arrayImage, pdfName);
            let result = true;
            if (filePdf.size && filePdf.size / 1024 / 1024 < 4) {
              result = await isValidImage(filePdf);
            } else {
              // if converted files are over the max size for OCR upload them anyway
              result = true;
            }

            const obj = {
              isValid: result,
              isLoading: false,
              url: arrayImage,
              isDirty: true,
              error: result ? null : analysisError,
            };
            dispatch({
              type: 'update',
              theImageName: pdfName,
              value: obj,
            });
            showRedact = true;
            if (!result) {
              thiserr = analysisError;
            }
          });
          await Promise.all(promises);
        } else {
          setError(maxPDFFileSize);
          const obj = {
            error: maxPDFFileSize,
            isValid: false,
            isLoading: false,
          };
          dispatch({
            type: 'update',
            theImageName: imageName,
            value: obj,
          });
          return;
        }
      } else {
        let obj = [{ imageName: imageName, isLoading: true }];

        if (image.size && image.size / 1024 / 1024 >= 4) {
          thiserr = maxFileSize;
          setError(maxFileSize);
          return;
        } else {
          dispatch({
            type: 'add',
            value: obj,
          });
          try {
            const arrayBuffer = await readFileAsync(image);
            let theArray = new Uint8Array(arrayBuffer);
            let result = await isValidImage(theArray);
            showRedact = true;
            //if the image is valid, update the object created at the beginning.
            if (result) {
              const url = createUrl(image);

              const object = {
                url: url,
                isDirty: true,
                isValid: true,
                isLoading: false,
              };

              dispatch({
                type: 'update',
                theImageName: imageName,
                value: object,
              });
            } else {
              const url = createUrl(image);

              const updatedInvalidObject = {
                isValid: false,
                error: analysisError,
                url: url,
                isUploading: false,
                isLoading: false,
              };
              dispatch({
                type: 'update',
                theImageName: imageName,
                value: updatedInvalidObject,
              });

              thiserr = analysisError;
            }
          } catch (error) {
            console.log(error);
          }
        }
      }
    });

    await Promise.all(inputPromises);
    if (showRedact) {
      handleRedactionTool({ show: true, index: 0 });
    }
  });

  const readFileAsync = file => {
    return new Promise((resolve, reject) => {
      let reader = new FileReader();

      reader.onload = () => {
        resolve(reader.result);
      };

      reader.onerror = reject;

      reader.readAsArrayBuffer(file);
    });
  };

  const handleDelete = async file => {
    file.url = '';
    const { appId, detailsId, section, document } = props;

    const imgId = `${appId}${
      detailsId ? '.' + detailsId : ''
    }.${section}.${document}.${replaceDotBySymbol(file.imageName, '_')}`;

    const remove = async () => {
      dispatch({
        type: 'remove',
        imageName: file.imageName,
        handler: onDelete,
      });
    };

    if (file.isLoading) {
      await remove();
      cancelRequests();
      return;
    }

    if (!file.isUploading) {
      try {
        await deleteBlob(imgId);
        await remove();
        // onDelete(
        //   state.fileArray.filter(a => a.isUploaded === true && a.url != '')
        // );
      } catch (error) {
        console.log(error);
      }
    }
  };

  const callCheckImageDimentions = dataURL => {
    let result = false;
    result = checkImageDimensionsWide(dataURL);

    return result;
  };
  const checkImageDimensionsWide = imgUrl => {
    let base64string = imgUrl;
    let result = false;
    let im = document.createElement('img');
    im.setAttribute('src', base64string);

    let height2x = im.height * 2.5;
    let width1x = im.width;

    if (width1x > height2x) {
      result = true;
    } else {
    }

    return result;
  };

  const changeCurrentImage = isBack => {
    var tmpCurrentImage = currentImage;
    if (isBack) {
      tmpCurrentImage = currentImage - 1;
    } else {
      tmpCurrentImage = currentImage + 1;
    }
    if (tmpCurrentImage < data.length && tmpCurrentImage >= 0) {
      setCurrentImage(tmpCurrentImage);
    }
  };

  const handleRedactionTool = params => {
    let show = params.show;
    let data = params.data;
    let editedImage = params.editedImage;
    let index = params.index;
    let isBack = params.isBack;

    setCurrentImage(index);
    if (show) {
      setShowRedactionTool(true);
      /* Push to mouseflow as another page state */
      window._mfq = window._mfq || [];
      window._mfq.push(['newPageView', 'document-editor-popup']);
    } else {
      setShowRedactionTool(false);

      let indexfe = 0;
      data.forEach(async element => {
        indexfe = indexfe + 1;

        const file = dataURLtoFile(element.url, element.imageName);

        if (element.isValid && element.isDirty) {
          dispatch({
            type: 'update',
            theImageName: element.imageName,
            value: {
              isUploading: true,
            },
          });
          if (props.traceFileArray) props.traceFileArray();
          const { appId, detailsId, section, document } = props;

          const imgId = `${appId}${
            detailsId ? '.' + detailsId : ''
          }.${section}.${document}.${replaceDotBySymbol(
            element.imageName,
            '_'
          )}`;

          try {
            if (file.size && file.size / 1024 / 1024 < 4) {
              // const compressedFile = await imageCompression(file, options);
              //const compressedFile = await compressImage(file, 80, 'png');
              console.log('current o IF', file);
              await uploadFile(file, appId, detailsId, section, document);
            } else {
              console.log('current o ELSE', file);
              await uploadFile(file, appId, detailsId, section, document);
            }

            // let image = await getImage(imgId);

            // newUrl = createUrl(image);
            // console.log(newUrl);

            dispatch({
              type: 'update',
              theImageName: element.imageName,
              value: {
                //url: 'data:image/jpg;base64,' + image,
                isUploading: false,
                isUploaded: true,
                isDirty: false,
                timeStamp: new Date().toUTCString(),
              },
              handler: onCreate,
            });
            // onCreate(state.fileArray.filter(a => a.isUploaded === true));
          } catch (error) {
            console.log('current Error: ' + error);
          }
        } else {
          dispatch({
            type: 'update',
            theImageName: element.imageName,
            value: { isLoading: false, isUploading: false },
          });
        }
      });
    }
  };

  const handleCloseErrorMessage = () => {
    const errorFile = state.fileArray.filter(item => {
      return item.hasOwnProperty('error') || item.error;
    })[0];
    if (errorFile) {
      handleDelete(errorFile);
    }
  };

  const classes = useStyles();

  const onDrop = useCallback(
    acceptedFiles => {
      handleFile(acceptedFiles);
    },
    [handleFile]
  );

  const {
    getRootProps,
    getInputProps,
    isDragActive,
    isDragReject,
  } = useDropzone({
    onDrop,
    accept: 'image/jpeg, image/png, image/gif, image/bmp, application/pdf',
    disabled: false,
  });

  return (
    <div className="upload-file" ref={ref}>
      <ImageCrossOut
        showDialog={showRedactionTool}
        data={data}
        showSavingOverlay={props.showSavingOverlay}
        showRedactionTool={handleRedactionTool}
        index={index}
        currentImage={currentImage}
        changeCurrentImage={changeCurrentImage}
      />
      <AlertDialog
        text={alertText}
        isOpen={open}
        onClose={() => setOpen(false)}
      />
      {isSummary && (
        <div style={{ margin: '8px 0 0 0', width: '100%', flexGrow: 1 }}>
          <Grid
            container
            justify="flex-start"
            alignItems="center"
            direction="row"
          >
            {error && error != '' ? (
              <div className={classes.paperError}>
                <label
                  data-testid="specificError"
                  className={classes.errorLabel2}
                >
                  {error}
                </label>
              </div>
            ) : (
              ''
            )}
            {state.fileArray
              //.filter(d => d.url && d.url != '')
              //.sort()
              .filter(d => !d.error || d.error == '')
              .map((data, index) => (
                <Grid
                  item
                  className={data.error ? classes.paperError : classes.paper}
                  style={{ marginRight: '10px' }}
                  key={data.imageName}
                >
                  <React.Fragment>
                    {data.isValid && (
                      <React.Fragment>
                        {data.isUploading ? (
                          <ColorLinearProgress
                            className={classes.linearProgress}
                          />
                        ) : null}
                        <IconButton
                          disabled={data.isUploading ? true : false}
                          className={classes.zoomIcon}
                          style={{ top: '-15px', left: '-15px' }}
                          onClick={() =>
                            handleRedactionTool({ show: true, index })
                          }
                        >
                          <ZoomIn />
                        </IconButton>

                        {console.log(
                          'uplaoder223 data checkImageDimensionsWide(data.url)',
                          callCheckImageDimentions(data.url)
                        )}
                        <img
                          className={
                            'mouseflow-hide ' +
                            (callCheckImageDimentions(data.url)
                              ? classes.img3
                              : classes.img2)
                          }
                          alt={data.imageName}
                          src={data.url}
                          onClick={() =>
                            handleRedactionTool({ show: true, index })
                          }
                        />
                      </React.Fragment>
                    )}

                    <Tooltip title={data.imageName}>
                      <label className={classes.fileNameLabel}>
                        {data.imageName}
                      </label>
                    </Tooltip>

                    {data.isValid && !data.isUploading ? (
                      <CheckCircle className={classes.checkIcon}></CheckCircle>
                    ) : data.isValid === false ? (
                      <HighlightOff
                        className={classes.invalidIcon}
                      ></HighlightOff>
                    ) : null}
                  </React.Fragment>
                </Grid>
              ))}
          </Grid>
        </div>
      )}

      {!isSummary && (
        <div className="uploadFile" style={{ width: 232 }}>
          <Grid
            container
            spacing={1}
            justify="center"
            alignContent="center"
            alignItems="flex-start"
          >
            {/* <Grid item xs>
          <label>{leftMessage}</label>
          <input
            style={{ display: 'none' }}
            type="file"
            id="image"
            accept="image/jpeg, image/png, image/gif, image/bmp"
            onChange={e => onChange(e.currentTarget.files)}
            multiple
          ></input>
          <label htmlFor="image">
            <Button
              variant="contained"
              component="span"
              className={classes.button}
            >
              {fm.uploadButton}
              <CloudUploadIcon className={classes.rightIconButton} />
            </Button>
          </label>
          <label>{rightMessage}</label>
        </Grid> */}
            <Grid className="grid-upload">
              <div className={classes.labelsBox}>
                <div className={classes.messageBox}>
                  <div className={classes.leftMessageBox}>
                    {props.leftMessage}
                  </div>
                  <div style={required ? { color: '#d20000' } : {}}>
                    {props.rightMessage}
                  </div>
                </div>
                {!props.hideIcon && (
                  <React.Fragment>
                    {/*<IconButton
                      className={classes.infoIcon}
                      onClick={() => {
                        setOpen(true);
                        setAlertText(helpText);
                      }}
                    >
                      <Info />
                    </IconButton>*/}
                  </React.Fragment>
                )}
              </div>
              {isPdf && (
                <div style={{ textAlign: 'center' }}>
                  <CircularProgress className={classes.progress} />
                </div>
              )}
              {state.fileArray
                /* .sort(function(a, b) {
                  var nameA = parseInt(
                    a.imageName.toUpperCase().replace(/\D/g, '')
                  ); // ignore upper and lowercase
                  var nameB = parseInt(
                    b.imageName.toUpperCase().replace(/\D/g, '')
                  ); // ignore upper and lowercase
                  if (nameA < nameB) {
                    return -1;
                  }
                  if (nameA > nameB) {
                    return 1;
                  }

                  // names must be equal
                  return 0;
                })
                */
                //.filter(d => d.url && d.url != '')
                .filter(d => !d.error || d.error == '')
                .map((data, index) => (
                  <div
                    className={data.error ? classes.rootError : classes.root}
                    key={data.imageName}
                  >
                    <div
                      className={
                        data.error ? classes.paperError : classes.paper
                      }
                    >
                      <Grid container spacing={2} direction="column">
                        {data.isValid && (
                          <Grid item className={classes.relative}>
                            {data.isUploading ? (
                              <ColorLinearProgress
                                className={classes.linearProgress}
                              />
                            ) : null}
                            <div className="upload-gradient"></div>
                            <IconButton
                              disabled={data.isUploading ? true : false}
                              className={classes.zoomIcon}
                              onClick={() =>
                                handleRedactionTool({ show: true, index })
                              }
                              data-testid="zoomIcon"
                            >
                              <ZoomIn />
                            </IconButton>
                            <IconButton
                              disabled={data.isUploading ? true : false}
                              className={classes.deleteIcon}
                              onClick={() => handleDelete(data)}
                              data-testid="deleteIconUploaded"
                            >
                              Remove &nbsp;
                              <DeleteForever />
                            </IconButton>
                            <img
                              id={props.id + 'UploadedImage' + index}
                              className={
                                'mouseflow-hide ' +
                                (callCheckImageDimentions(data.url)
                                  ? classes.img3
                                  : classes.img2)
                              }
                              alt={data.imageName}
                              src={data.url}
                              style={
                                data.placeholder
                                  ? {
                                      margin: '80px auto',
                                      cursor: 'pointer',
                                      width: '50px',
                                      height: '50px',
                                    }
                                  : {
                                      cursor: 'pointer',
                                    }
                              }
                              onClick={() =>
                                handleRedactionTool({ show: true, index })
                              }
                            />
                          </Grid>
                        )}
                        <Grid item>
                          {data.isLoading && (
                            <React.Fragment>
                              <div className={classes.imageLoadingContainer}>
                                <ColorLinearProgress
                                  data-testid="linearProgress"
                                  className={classes.linearProgress}
                                />
                                {/*<IconButton
                                className={classes.deleteIconLoading}
                                onClick={() => handleDelete(data)}
                                data-testid="deleteIconLoading"
                              >
                                <DeleteForever />
                              </IconButton>*/}
                                <Image
                                  className={classes.imageLoadingIcon}
                                  data-testid="imageLoadingIcon"
                                  style={{
                                    margin: '45px 35px',
                                    cursor: 'pointer',
                                  }}
                                ></Image>
                              </div>
                            </React.Fragment>
                          )}
                          <Tooltip
                            title={data.imageName}
                            style={
                              !data.error
                                ? { overflow: 'hidden' }
                                : { top: '-1px', overflow: 'initial' }
                            }
                          >
                            <label
                              id={props.id + 'FileName' + index}
                              className={classes.fileNameLabel}
                            >
                              {data.imageName}
                            </label>
                          </Tooltip>

                          {data.isValid && !data.isUploading ? (
                            <CheckCircle
                              id={props.id + 'IsValidImage' + index}
                              className={classes.checkIcon}
                            ></CheckCircle>
                          ) : data.isValid === false ? (
                            <HighlightOff
                              className={classes.invalidIcon}
                              data-testid="invalidIcon"
                              onClick={handleCloseErrorMessage}
                            ></HighlightOff>
                          ) : null}
                        </Grid>
                      </Grid>
                      <label
                        data-testid="specificError"
                        className={classes.errorLabel2}
                      >
                        {data.error}
                      </label>
                    </div>
                  </div>
                ))}
              {renderIf(
                state.fileArray.length > 0,
                <p>
                  <FormattedMessage
                    id="needAnotherDocument"
                    defaultMessage="Do you need to attach another document or page?"
                  />
                </p>
              )}
              <div
                {...getRootProps()}
                className={
                  (isMobile()
                    ? classes.dropzoneMobile
                    : state.fileArray.length < 1
                    ? props.required
                      ? classes.dropzone + ' ' + classes.dropzoneError
                      : classes.dropzone
                    : classes.smallDropzone) +
                  ' ' +
                  (props.customClass ? props.customClass : '')
                }
              >
                <input
                  {...getInputProps()}
                  id={props.id}
                  className={props.customClass ? props.customClass : ''}
                  accept=".jpeg, image/jpeg, .jpg, image/jpg, image/png, image/gif, image/bmp, application/pdf"
                />
                {isDragActive ? (
                  <p className={classes.gridLabelOnDrag}>
                    {fm.dragNdropDropTheFilesHere}
                  </p>
                ) : (
                  <React.Fragment>
                    {renderIf(
                      state.fileArray.length < 1,
                      <CloudUploadIcon
                        className={
                          isMobile()
                            ? classes.rightIconDragMobile
                            : classes.rightIconDrag
                        }
                      />
                    )}
                    <p
                      className={
                        isMobile()
                          ? classes.gridLabelDefaultMessageMobile
                          : classes.gridLabelDefaultMessage
                      }
                    >
                      {isMobile()
                        ? fm.dragNdropDefaultMessageMobile
                        : fm.dragNdropDefaultMessage}
                    </p>
                  </React.Fragment>
                )}
                {isDragReject && (
                  <p className={classes.errorLabel}>
                    {fm.dragNdropInvalidFile}
                  </p>
                )}
              </div>
            </Grid>
          </Grid>

          <label
            className={classes.errorLabelGeneral}
            data-testid="generalError"
          >
            {error}
          </label>
        </div>
      )}
    </div>
  );
});

const helpTextObscure = (
  <React.Fragment>
    <p>{fm.helpText1}</p>
    <p>{fm.helpText2}</p>
    <p>{fm.helpText3}</p>
  </React.Fragment>
);

export default UploadFile;
