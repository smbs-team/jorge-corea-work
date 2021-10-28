import React from "react";
import SvgIcon from "@material-ui/core/SvgIcon";
import Cropper from "./react-cropper";

import "./cropper.css";
import { renderIf, arrayNullOrEmpty } from "../../lib/helpers/util";

import { makeStyles, withStyles } from "@material-ui/core/styles";
import Grid from "@material-ui/core/Grid";
import Button from "@material-ui/core/Button";

import * as fm from "./FormatTexts";
import WarningIcon from "@material-ui/icons/Warning";
import EditIcon from "@material-ui/icons/Edit";
import CheckIcon from "@material-ui/icons/Check";
import RotateLeftIcon from "@material-ui/icons/RotateLeft";
import RotateRightIcon from "@material-ui/icons/RotateRight";
import CropIcon from "@material-ui/icons/Crop";
import UndoIcon from "@material-ui/icons/Undo";
import ZoomInIcon from "@material-ui/icons/ZoomIn";
import ZoomOutIcon from "@material-ui/icons/ZoomOut";
import CloseIcon from "@material-ui/icons/Close";
import { FormattedMessage } from "react-intl";
import { validateFileExtension } from "../../lib/helpers/util";

const src = "";

function BrushIcon(props) {
  return (
    <SvgIcon {...props} onClick={props.onClick}>
      <path d="M0 0h24v24H0z" fill="none" />
      <path d="M7 14c-1.66 0-3 1.34-3 3 0 1.31-1.16 2-2 2 .92 1.22 2.49 2 4 2 2.21 0 4-1.79 4-4 0-1.66-1.34-3-3-3zm13.71-9.37l-1.34-1.34c-.39-.39-1.02-.39-1.41 0L9 12.25 11.75 15l8.96-8.96c.39-.39.39-1.02 0-1.41z" />
      <title> {props.title}</title>
      <text x="0" y="28" font-family="sans-serif" font-size="8px" fill="white">
        {props.label}
      </text>
    </SvgIcon>
  );
}

function RefreshIcon(props) {
  return (
    <SvgIcon {...props}>
      <path d="M17.65 6.35C16.2 4.9 14.21 4 12 4c-4.42 0-7.99 3.58-7.99 8s3.57 8 7.99 8c3.73 0 6.84-2.55 7.73-6h-2.08c-.82 2.33-3.04 4-5.65 4-3.31 0-6-2.69-6-6s2.69-6 6-6c1.66 0 3.14.69 4.22 1.78L13 11h7V4l-2.35 2.35z" />
      <path d="M0 0h24v24H0z" fill="none" />
      <title> {props.title}</title>
      <text x="0" y="28" font-family="sans-serif" font-size="8px" fill="white">
        {props.label}
      </text>
    </SvgIcon>
  );
}

function SaveIcon(props) {
  return (
    <SvgIcon {...props}>
      <path d="M0 0h24v24H0z" fill="none" />
      <path d="M17 3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.1 0 2-.9 2-2V7l-4-4zm-5 16c-1.66 0-3-1.34-3-3s1.34-3 3-3 3 1.34 3 3-1.34 3-3 3zm3-10H5V5h10v4z" />
      <title> {props.title}</title>
      <text x="0" y="28" font-family="sans-serif" font-size="8px" fill="white">
        {props.label}
      </text>
    </SvgIcon>
  );
}

function ClearIconNormal(props) {
  return (
    <SvgIcon {...props} onClick={props.onClick}>
      <path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z" />
      <path d="M0 0h24v24H0z" fill="none" />
      <title> {props.title}</title>
      <text x="0" y="28" font-family="sans-serif" font-size="8px" fill="white">
        {props.label}
      </text>
    </SvgIcon>
  );
}

function ClearIcon(props) {
  return (
    <SvgIcon {...props} onClick={props.onClick}>
      <path d="M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z" />
      <path d="M0 0h24v24H0z" fill="none" />
      <title> {props.title}</title>
      <text x="0" y="28" font-family="sans-serif" font-size="8px" fill="white">
        {props.label}
      </text>
    </SvgIcon>
  );
}

function AttachIcon(props) {
  return (
    <SvgIcon {...props}>
      <path d="M16.5 6v11.5c0 2.21-1.79 4-4 4s-4-1.79-4-4V5c0-1.38 1.12-2.5 2.5-2.5s2.5 1.12 2.5 2.5v10.5c0 .55-.45 1-1 1s-1-.45-1-1V6H10v9.5c0 1.38 1.12 2.5 2.5 2.5s2.5-1.12 2.5-2.5V5c0-2.21-1.79-4-4-4S7 2.79 7 5v12.5c0 3.04 2.46 5.5 5.5 5.5s5.5-2.46 5.5-5.5V6h-1.5z" />
      <path d="M0 0h24v24H0z" fill="none" />
      <title> {props.title}</title>
      <text x="0" y="28" font-family="sans-serif" font-size="8px" fill="white">
        {props.label}
      </text>
    </SvgIcon>
  );
}
let msg = "";
const myelement = <p> </p>;

function ErrorIcon(props) {
  return (
    <SvgIcon
      width="0.133333in"
      height="0.116667in"
      viewBox="0 0 40 35"
      {...props}
    >
      <path d="M0 0h24v24H0z" fill="white" />
      <path
        d="M 19.00,2.00
        C 19.00,2.00 1.00,35.00 1.00,35.00
          1.00,35.00 39.00,35.00 39.00,35.00
          39.00,35.00 21.00,2.00 21.00,2.00
          21.00,2.00 19.00,2.00 19.00,2.00 Z
        M 22.00,10.00
        C 22.00,10.00 22.00,24.00 22.00,24.00
          22.00,24.00 18.00,24.00 18.00,24.00
          18.00,24.00 18.00,10.00 18.00,10.00
          18.00,10.00 22.00,10.00 22.00,10.00 Z
        M 22.00,26.00
        C 22.00,26.00 22.00,31.00 22.00,31.00
          22.00,31.00 18.00,31.00 18.00,31.00
          18.00,31.00 18.00,26.00 18.00,26.00
          18.00,26.00 22.00,26.00 22.00,26.00 Z"
      />
    </SvgIcon>
  );
}

class cropperComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      src,
      cropResult: null,
      currentImgIndex: 0,
      currentContinueImgIndex: 0,
      croppedImagesList: [],
      dragMode: "move",
      autoCrop: false,
      useBlackCropper: false,
      isCropping: false,
      isRedacting: false,
      isRotating: false,
      isunDoing: false,
      imageList: null,
      IPMessage: "",
      error: false,
      errorText: ""
    };
    this.cropImage = this.cropImage.bind(this);
    this.onChange = this.onChange.bind(this);
    this.finishCrop = this.finishCrop.bind(this);
    this.finishMask = this.finishMask.bind(this);
    this.startCrop = this.startCrop.bind(this);
    this.startMask = this.startMask.bind(this);
    this.rotateLeft = this.rotateLeft.bind(this);
    this.rotateRight = this.rotateRight.bind(this);
    this.zoomIn = this.zoomIn.bind(this);
    this.zoomOut = this.zoomOut.bind(this);
    this.undoCrop = this.undoCrop.bind(this);
    this.useDefaultImage = this.useDefaultImage.bind(this);
    this.handleClickOutside = this.handleClickOutside.bind(this);
  }

  onChange(e) {
    e.preventDefault();
    let files;
    if (e.dataTransfer) {
      files = e.dataTransfer.files;
    } else if (e.target) {
      files = e.target.files;
    }
    const reader = new FileReader();
    reader.onload = () => {
      this.setState({ src: reader.result });
    };
    reader.readAsDataURL(files[0]);
  }

  handleClickOutside(event) {
    console.log("e.target", event.target);
    console.log("e.target.classList", event.target.classList);
  }

  handleClickOutside1 = e => {
    console.log("e.target.classList111", e.target);
  };
  zoomOut() {
    this.cropper.zoom(-0.5);
  }
  zoomIn() {
    this.cropper.zoom(0.5);
  }
  rotateLeft() {
    this.cropper.rotate(-45);
  }
  rotateRight() {
    this.cropper.rotate(45);
  }
  startCrop() {
    this.setState({ useBlackCropper: false });
    this.cropper.crop();
    this.cropper.setCropBoxData({ width: 400, height: 400 });
  }
  startMask() {
    let croppedCanvas = this.cropper.getCroppedCanvas();
    let thisSrc = croppedCanvas.toDataURL();
    this.setState({ useBlackCropper: true, src: thisSrc }, () => {
      setTimeout(() => {
        this.cropper.crop();
        this.cropper.setCropBoxData({ width: 250, height: 40 });
      }, 500);
    });
  }
  finishMask() {
    // Crop

    // Mask
    let maskedCanvas = this.getMaskedCanvas();
    let thisSrc = maskedCanvas.toDataURL();
    var newArr = this.state.croppedImagesList.concat(thisSrc);
    this.setState({ croppedImagesList: newArr });
    this.setState({
      currentImgIndex: this.state.croppedImagesList.length
    });

    console.log("this.props.data src: after Finish ", this.state.src);
    this.setState({ src: maskedCanvas.toDataURL() });
    console.log("this.props.data src: maskedCanvas ", maskedCanvas);
    console.log("this.props.data src: after Finish ", this.state.src);
  }
  finishCrop() {
    let croppedCanvas;

    // let maskedImage;

    // currentImgIndex++;

    // Crop
    croppedCanvas = this.cropper.getCroppedCanvas();
    let thisSrc = croppedCanvas.toDataURL();
    // image.src = maskedImage.src;

    //console.log('image data', image);

    //var img = document.createElement('img');
    //img.src = image.src;
    var newArr = this.state.croppedImagesList.concat(thisSrc);
    this.setState({ croppedImagesList: newArr });
    this.setState({
      currentImgIndex: this.state.croppedImagesList.length
    });

    this.setState({ src: thisSrc });
  }

  undoCrop() {
    if (this.state.currentImgIndex > 0) {
      let thisInex = this.state.currentImgIndex - 1;
      this.setState({
        currentImgIndex: thisInex
      });
      let thisSrc = this.state.croppedImagesList[
        this.state.currentImgIndex - 1
      ];

      this.setState({ src: thisSrc });
    }
  }
  getMaskedCanvas() {
    var canvas = document.createElement("canvas");
    var context = canvas.getContext("2d");
    var maskWidth = this.cropper.getData().width;
    var maskHeight = this.cropper.getData().height;
    var maskTop = this.cropper.getData().y;
    var maskLeft = this.cropper.getData().x;
    var imageWidth = this.cropper.getImageData().naturalWidth;
    var imageHeight = this.cropper.getImageData().naturalHeight;

    canvas.width = imageWidth;
    canvas.height = imageHeight;

    context.imageSmoothingEnabled = true;

    var image = new Image();
    image.src = this.state.src;
    console.log(" this.state.src", this.state.src);
    console.log(" this.state.src PropsData", this.props.data);
    context.drawImage(image, 0, 0, imageWidth, imageHeight);
    context.fillRect(maskLeft, maskTop, maskWidth, maskHeight);
    return canvas;
  }
  cropImage() {
    if (typeof this.cropper.getCroppedCanvas() === "undefined") {
      return;
    }
    this.setState({
      cropResult: this.cropper.getCroppedCanvas().toDataURL()
    });
  }
  checkIfError() {
    if (this.props.data[this.state.currentContinueImgIndex].error) {
      this.setState({
        error: true,
        errorText: this.props.data[this.state.currentContinueImgIndex].error
      });
    } else {
      this.setState({
        error: false
      });
    }
  }
  finishAction = () => {
    this.finalizeImageEdit();
    if (this.state.isCropping) {
      this.setState({ IPMessage: "Saving" });
      this.finishCrop();
      setTimeout(() => {
        this.setState({ isCropping: false, IPMessage: "" });
      }, 1000);
    }
    if (this.state.isunDoing) {
      this.setState({ IPMessage: "Saving" });
      this.finishCrop();
      setTimeout(() => {
        this.setState({ isunDoing: false, IPMessage: "" });
      }, 1000);
    }
    if (this.state.isRotating) {
      this.setState({ IPMessage: "Saving" });
      this.finishCrop();
      setTimeout(() => {
        this.setState({ isRotating: false, IPMessage: "" });
      }, 1000);
    }
    if (this.state.isRedacting) {
      this.setState({ IPMessage: "Saving" });
      this.finishMask();
      setTimeout(() => {
        this.setState({ isRedacting: false, IPMessage: "" });
      }, 1000);
    }
  };

  setstateback = () => {
    this.setState({ isRedacting: false });
  };

  finalizeImageEdit = () => {
    let canvas = null;
    let editedImage;

    this.state.isRedacting && (canvas = this.getMaskedCanvas());
    this.state.isunDoing && (canvas = this.cropper.getCroppedCanvas());
    this.state.isCropping && (canvas = this.cropper.getCroppedCanvas());
    this.state.isRotating && (canvas = this.cropper.getCroppedCanvas());
    //canvas && (editedImage = canvas.toDataURL());
    canvas && (editedImage = canvas.toDataURL("image/jpeg", 0.5));
    console.log("editedImage ", editedImage);
    editedImage &&
      (this.props.data[this.state.currentContinueImgIndex].url = editedImage);
    editedImage &&
      (this.props.data[this.state.currentContinueImgIndex].isDirty = true);
  };

  useDefaultImage() {
    this.setState({ src });
  }

  componentWillUnmount() {
    document.addEventListener("mousedown", this.handleClickOutside1);
  }

  componentDidMount = async () => {
    // this.context.currentUser();

    // this.props.data.push(this.props.data[0]);

    console.log("this.props.data1", this.props.data);

    this.setState(
      {
        currentContinueImgIndex: 0
      },
      () => {
        console.log("this.props.index", this.props.index);
        let newArr = this.state.croppedImagesList.concat(
          this.props.data[this.props.data.length - 1].url
        );
        this.setState({ croppedImagesList: newArr }, () => {
          this.setState({
            currentImgIndex: 0
          });
          console.log("this.props.data2", this.props.data);

          this.setState({
            src: this.props.data[0].url
          });
          this.checkIfError();
        });
      }
    );

    document.addEventListener("mousedown", this.handleClickOutside1);
  };

  render() {
    return (
      <React.Fragment>
        {" "}
        <Grid
          container
          style={{
            height: "100%"
          }}
          spacing={12}
        >
          <Grid
            lg={3}
            md={4}
            sm={12}
            style={{
              background: "#6f1f62",
              color: "white",
              width: "100%"
            }}
          >
            <Grid container spacing={12}>
              <Grid lg={12} md={12}>
                <div
                  style={{
                    margin: "0 auto",

                    float: "none",
                    display: "flex",
                    textAlign: "center"
                  }}
                >
                  <div
                    style={{
                      position: "relative",
                      float: "left",
                      padding: 5,
                      width: "20%",
                      cursor: "pointer"
                    }}
                    onClick={() => {
                      if (this.state.isRedacting) {
                        this.finishAction();
                      } else {
                        this.finishAction();
                        setTimeout(this.startMask, 500);

                        this.setState({
                          isRedacting: true,
                          IPMessage: "Click image to redact"
                        });
                      }
                    }}
                  >
                    <EditIcon
                      style={{
                        paddingBottom: 10,
                        cursor: "pointer",
                        fontSize: 24,
                        zoom: "1.4"
                      }}
                    />

                    <p
                      style={{
                        fontSize: 14,
                        marginTop: -16,
                        borderBottom: this.state.isRedacting
                          ? "3px solid #d4e693"
                          : "none"
                      }}
                    >
                      Redaction
                    </p>
                  </div>
                  <div
                    style={{
                      position: "relative",
                      float: "left",
                      padding: 5,
                      width: "20%",
                      cursor: "pointer"
                    }}
                    onClick={() => {
                      if (this.state.isRotating) {
                        this.rotateLeft();
                      } else {
                        this.finishAction();
                        setTimeout(this.rotateLeft, 500);
                        this.setState({ isRotating: true });
                      }
                    }}
                  >
                    <RotateLeftIcon
                      style={{
                        paddingBottom: 10,
                        cursor: "pointer",
                        fontSize: 24,
                        zoom: "1.4"
                      }}
                    />

                    <p
                      style={{
                        fontSize: 14,
                        marginTop: -16
                      }}
                    >
                      Turn left
                    </p>
                  </div>
                  <div
                    style={{
                      position: "relative",
                      float: "left",
                      padding: 5,
                      width: "20%",
                      cursor: "pointer"
                    }}
                    onClick={() => {
                      if (this.state.isRotating) {
                        this.rotateRight();
                      } else {
                        this.finishAction();
                        setTimeout(this.rotateRight, 500);
                        this.setState({ isRotating: true });
                      }
                    }}
                  >
                    <RotateRightIcon
                      style={{
                        paddingBottom: 10,
                        cursor: "pointer",
                        fontSize: 24,
                        zoom: "1.4"
                      }}
                    />

                    <p
                      style={{
                        fontSize: 14,
                        marginTop: -16
                      }}
                    >
                      Turn Right
                    </p>
                  </div>

                  <div
                    style={{
                      width: 30,
                      position: "relative",
                      float: "left",
                      padding: 5,
                      width: "20%",
                      cursor: "pointer"
                    }}
                    onClick={() => {
                      if (this.state.isCropping) {
                        this.finishAction();
                      } else {
                        this.finishAction();
                        setTimeout(this.startCrop, 500);
                        this.setState({
                          isCropping: true,
                          IPMessage: "Resize Crop Area"
                        });
                      }
                    }}
                  >
                    <CropIcon
                      style={{
                        paddingBottom: 10,
                        cursor: "pointer",
                        fontSize: 24,
                        zoom: "1.4"
                      }}
                    />
                    <p
                      style={{
                        fontSize: 14,
                        marginTop: -16,
                        borderBottom: this.state.isCropping
                          ? "3px solid #d4e693"
                          : "none"
                      }}
                    >
                      Crop
                    </p>
                  </div>
                  <div
                    style={{
                      width: 30,
                      position: "relative",
                      float: "left",
                      padding: 5,
                      width: "20%",
                      cursor: "pointer"
                    }}
                    onClick={() => {
                      this.setstateback({ isunDoing: true });
                      this.undoCrop();
                      this.finishAction();
                    }}
                  >
                    <UndoIcon
                      style={{
                        paddingBottom: 10,
                        cursor: "pointer",
                        fontSize: 24,
                        zoom: "1.4"
                      }}
                      onClick={() => {
                        this.undoCrop();
                      }}
                    />

                    <p
                      style={{
                        fontSize: 14,
                        marginTop: -16
                      }}
                    >
                      Undo
                    </p>
                  </div>
                </div>
              </Grid>
            </Grid>
            <Grid style={{ textAlign: "center" }} container spacing={12}>
              <div
                style={{
                  textAlign: "center",
                  height: "43px",
                  backgroundColor: "black",
                  width: "100%",
                  fontSize: "20px",
                  paddingLeft: "30px",
                  paddingRight: "30px"
                }}
              >
                {this.state.IPMessage}{" "}
              </div>
              <div
                style={{
                  padding: "20px 20px 20px 0px",
                  margin: "0 auto",

                  float: "none",
                  display: "flex"
                }}
              >
                <div
                  style={{
                    width: 80,
                    padding: 5,
                    cursor: "pointer"
                  }}
                  onClick={() => {
                    this.zoomIn();
                  }}
                >
                  <ZoomInIcon
                    style={{
                      paddingBottom: 10,
                      cursor: "pointer",
                      fontSize: 24,
                      zoom: "1.4"
                    }}
                  />

                  <p
                    style={{
                      fontSize: 14,
                      marginTop: -16,
                      marginLeft: 6
                    }}
                  >
                    Zoom in
                  </p>
                </div>
                <div
                  style={{
                    width: 80,
                    padding: 5,
                    cursor: "pointer"
                  }}
                  onClick={() => {
                    this.zoomOut();
                  }}
                >
                  <ZoomOutIcon
                    style={{
                      paddingBottom: 10,
                      cursor: "pointer",
                      fontSize: 24,
                      zoom: "1.4"
                    }}
                  />

                  <p
                    style={{
                      fontSize: 14,
                      marginTop: -16,
                      marginLeft: 6
                    }}
                  >
                    Zoom out
                  </p>
                </div>
              </div>
            </Grid>
            <div className="btn-group">
              <div style={{ padding: "20px" }}></div>

              <div
                id="navigation"
                style={{
                  margin: " 0 auto",
                  textAlign: "center",
                  minWidth: 340,
                  maxWidth: 600,
                  bottom: 0,
                  position: "absolute"
                }}
              >
                {this.props.data.length === 1 ? (
                  <React.Fragment>
                    <Button
                      align="center"
                      style={{
                        textAlign: "center",
                        float: "left",
                        position: "absolute",
                        left: "100px",
                        bottom: "10px",
                        display: "block",
                        background: "white",
                        color: "black",
                        align: "center",
                        width: "200px",
                        height: "50px"
                      }}
                      onClick={() => {
                        this.finishAction();

                        setTimeout(
                          function() {
                            var canvas = this.cropper.getCroppedCanvas();
                            var editedImage = canvas.toDataURL();
                            console.log(this.props);
                            this.props.data[0].url = editedImage;
                            this.props.showRedactionTool({
                              show: false,
                              data: this.props.data,
                              editedImage: editedImage,
                              index: this.state.currentContinueImgIndex,
                              isBack: false
                            });
                          }.bind(this),
                          1000
                        );
                      }}
                      color="primary"
                      data-testid="continue"
                    >
                      Close and save
                    </Button>
                  </React.Fragment>
                ) : (
                  <React.Fragment>
                    {this.state.currentContinueImgIndex > 0 ? (
                      <React.Fragment>
                        <p
                          style={{
                            textAlign: "center",

                            bottom: "80px",
                            display: "block",
                            color: "white",
                            align: "center",
                            height: "50px",
                            paddingBottom: 40
                          }}
                        >
                          <Button
                            align="center"
                            style={{
                              textAlign: "center",
                              fontSize: 16,
                              color: "white",
                              align: "center",
                              height: "50px",
                              paddingRight: 50
                            }}
                            onClick={() => {
                              this.finishAction();

                              this.setState(
                                {
                                  currentContinueImgIndex:
                                    this.state.currentContinueImgIndex - 1
                                },
                                () => {
                                  this.setState({
                                    src: this.props.data[
                                      this.state.currentContinueImgIndex
                                    ].url
                                  });
                                  this.checkIfError();
                                }
                              );
                            }}
                            color="primary"
                            data-testid="continue"
                          >
                            Previous
                          </Button>
                          <span style={{ textAlign: "right" }}>
                            {" "}
                            {this.state.currentContinueImgIndex + 1} of{" "}
                            {this.props.data.length}
                          </span>
                        </p>
                      </React.Fragment>
                    ) : (
                      <React.Fragment>
                        <p
                          style={{
                            textAlign: "center",

                            bottom: "68px",
                            display: "block",
                            marginLeft: 167,
                            color: "white",
                            align: "center",
                            height: 80
                          }}
                        >
                          {this.state.currentContinueImgIndex + 1} of{" "}
                          {this.props.data.length}
                        </p>
                      </React.Fragment>
                    )}

                    {this.state.currentContinueImgIndex !=
                    this.props.data.length - 1 ? (
                      <React.Fragment>
                        <Button
                          align="center"
                          style={{
                            textAlign: "center",

                            bottom: "30px",

                            background: "white",
                            color: "black",
                            align: "center",
                            width: "200px",
                            height: "50px",
                            display: "block",
                            margin: " 0 auto"
                          }}
                          onClick={() => {
                            this.finishAction();

                            this.setState(
                              {
                                currentContinueImgIndex:
                                  this.state.currentContinueImgIndex + 1
                              },
                              () => {
                                console.log(
                                  "this.state.currentContinueImgIndex",
                                  this.state.currentContinueImgIndex
                                );
                                this.setState({
                                  src: this.props.data[
                                    this.state.currentContinueImgIndex
                                  ].url
                                });

                                this.checkIfError();
                              }
                            );
                          }}
                          color="primary"
                          data-testid="continue"
                        >
                          Next page
                        </Button>
                      </React.Fragment>
                    ) : (
                      <Button
                        align="center"
                        style={{
                          textAlign: "center",

                          bottom: "30px",

                          background: "white",
                          color: "black",
                          align: "center",
                          width: "200px",
                          height: "50px"
                        }}
                        onClick={() => {
                          this.finishAction();

                          setTimeout(
                            function() {
                              var canvas = this.cropper.getCroppedCanvas();
                              var editedImage = canvas.toDataURL();
                              var tempEditedImage;

                              this.props.data[
                                this.state.currentContinueImgIndex
                              ].url = editedImage;

                              this.props.data[
                                this.state.currentContinueImgIndex
                              ].isDirty = true;
                              this.props.data.forEach(a => {
                                if (validateFileExtension(a.url)) {
                                  let canvas = document.createElement("canvas");
                                  let context = canvas.getContext("2d");
                                  let base_image = new Image();
                                  base_image.src = a.url;
                                  context.drawImage(base_image, 10, 10);
                                  let pngUrl = canvas.toDataURL(); // PNG is the default
                                  a.url = pngUrl;
                                }
                              });
                              this.props.showRedactionTool({
                                show: false,
                                data: this.props.data,
                                editedImage: editedImage,
                                index: this.state.currentContinueImgIndex,
                                isBack: false
                              });
                            }.bind(this),
                            1000
                          );
                        }}
                        color="primary"
                        data-testid="continue"
                      >
                        <FormattedMessage
                          id="imagecrossout_continueAll"
                          defaultMessage="Close and Save"
                        />
                      </Button>
                    )}
                  </React.Fragment>
                )}
              </div>
            </div>
          </Grid>
          <Grid style={{ margin: " 0 auto" }} lg={9} md={8} style={{}}>
            <div>
              <div style={{ width: "100%" }}>
                <Cropper
                  style={{ width: "100%", maxHeight: "100vh" }}
                  aspectRatio={NaN}
                  className={this.state.useBlackCropper ? "blackCropper" : ""}
                  src={this.state.src}
                  dragMode={this.state.dragMode}
                  cropBoxData={{ width: 200, height: 20 }}
                  autoCrop={this.state.autoCrop}
                  viewMode={1}
                  setstateback={this.setstateback}
                  isRedacting={this.state.isRedacting}
                  onClick={this.handleClickOutside1}
                  onMouseDown={this.handleClickOutside}
                  ref={cropper => {
                    this.cropper = cropper;
                  }}
                />

                {this.state.error ? (
                  <div className="errorRedaction">
                    <div
                      style={{
                        paddingTop: "16px",
                        textAlign: "right",
                        width: "5%",
                        float: "left",
                        paddingRight: "20px"
                      }}
                    >
                      <WarningIcon style={{ fontSize: 35 }} />
                    </div>
                    <div
                      style={{
                        width: "66%",
                        float: "left",
                        paddingTop: 20
                      }}
                    >
                      {this.state.errorText}
                    </div>
                    <div
                      style={{
                        width: "10%",
                        float: "left",
                        paddingTop: 10
                      }}
                    >
                      <CloseIcon
                        style={{
                          fontSize: 24,
                          cursor: "pointer",
                          color: "black"
                        }}
                        onClick={() => {
                          this.setState({ error: null });
                        }}
                      />
                    </div>
                  </div>
                ) : (
                  ""
                )}
              </div>
            </div>
          </Grid>
        </Grid>
      </React.Fragment>
    );
  }
}

//cropperComponent.contextType = AuthContext;
export default cropperComponent;
