//-----------------------------------------------------------------------
// <copyright file="CropperComponent.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
import React from "react";
import Cropper from "./react-cropper";
import "./cropper.css";
import Hidden from "@material-ui/core/Hidden";
import Grid from "@material-ui/core/Grid";
import ArrowBackIosIcon from "@material-ui/icons/ArrowBackIos";
import CustomButton from "../CustomButton";
import VimeoPlayer from "../../../components/common/vimeo-player/VimeoPlayer";
import * as fm from "./FormatTexts";
import WarningIcon from "@material-ui/icons/Warning";
import Cached from "@material-ui/icons/Cached";
import AddBoxIcon from "@material-ui/icons/AddBox";
import CropIcon from "@material-ui/icons/Crop";
import UndoIcon from "@material-ui/icons/Undo";
import ZoomInIcon from "@material-ui/icons/ZoomIn";
import ZoomOutIcon from "@material-ui/icons/ZoomOut";
import CloseIcon from "@material-ui/icons/Close";
import { renderIf } from "../../../lib/helpers/util";

class cropperComponent extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      src: "",
      showPopupVideo: false,
      cropResult: null,
      currentImgIndex: 0,
      currentContinueImgIndex: 0,
      croppedImagesList: [],
      dragMode: "move",
      autoCrop: false,
      useBlackCropper: false,
      isMobile: false,
      isCropping: false,
      isRedacting: false,
      isRedactClicking: false,
      isRefresh: false,
      cropBoxData: false,
      isRotating: false,
      isunDoing: false,
      counter: false,
      imageList: null,
      infoPanelMessage: "",
      error: false,
      errorText: "",
      imgWidth: "",
      imgHeight: "",
      unListenHistoryHandler: null
    };
  }

  onCropperReady = e => {
    this.updateDimensions(e);
  };

  zoomOut = () => {
    this.cropper && this.cropper.zoom(-0.5);
  };

  zoomIn = () => {
    this.cropper && this.cropper.zoom(0.5);
  };

  rotateLeft = () => {
    this.cropper && this.cropper.rotate(-90);

    this.finishCrop();
    this.markImageAsDirty();
  };

  rotateRight = () => {
    this.markImageAsDirty();
    this.cropper && this.cropper.rotate(45);
  };

  startCrop = () => {
    this.setState({
      useBlackCropper: false,
      isCropping: true,
      isRedacting: false
    });
    //this.cropper && this.cropper.reset().clear();
    this.cropper && this.cropper.crop();
  };

  startMask = () => {
    this.setState({
      useBlackCropper: true,
      isRedacting: true,
      isCropping: false
    });
    this.cropper && this.cropper.setDragMode("crop");
    this.cropper && this.cropper.setCropBoxData({ width: 250, height: 40 });
  };

  updateImageData = (imageSource, index) => {
    this.props.data[index].url = imageSource;
    this.props.data[index].isDirty = true;
  };

  finishMask = () => {
    let maskedCanvas = this.getMaskedCanvas();
    const mimeType = this.props.data[this.state.currentContinueImgIndex].blob
      ? this.props.data[this.state.currentContinueImgIndex].blob.type
      : "image/png";
    let thisSrc = maskedCanvas.toDataURL(mimeType);
    let newArr = this.state.croppedImagesList.concat(thisSrc);
    this.markImageAsDirty();
    this.updateImageData(thisSrc, this.state.currentContinueImgIndex);
    this.setState(
      {
        croppedImagesList: newArr,
        currentImgIndex: newArr.length - 1,
        src: thisSrc,
        useBlackCropper: false,
        isRedacting: false,
        isCropping: false
      },
      () => {
        //setTimeout(() => {
        //this.cropper && this.cropper.dismissAll();

        this.cropper && this.cropper.setDragMode("move");
        //}, 2000);
      }
    );
  };

  finishCrop = () => {
    let croppedCanvas;

    this.cropper && (croppedCanvas = this.cropper.getCroppedCanvas());
    let thisSrc;
    const mimeType = this.props.data[this.state.currentContinueImgIndex].blob
      ? this.props.data[this.state.currentContinueImgIndex].blob.type
      : "image/png";

    croppedCanvas && (thisSrc = croppedCanvas.toDataURL(mimeType));

    let newArr = this.state.croppedImagesList.concat(thisSrc);
    this.markImageAsDirty();
    this.updateImageData(thisSrc, this.state.currentContinueImgIndex);
    this.setState(
      {
        croppedImagesList: newArr,
        currentImgIndex: newArr.length - 1,
        src: thisSrc,
        isCropping: false,
        infoPanelMessage: "",
        isRedacting: false
      },
      () => {
        this.cropper && this.cropper.setDragMode("move");
      }
    );
  };

  undoCrop = () => {
    if (this.state.currentImgIndex > 0) {
      this.state.croppedImagesList.pop();
      const newIndex = this.state.croppedImagesList.length - 1;

      let thisSrc = this.state.croppedImagesList[newIndex];
      this.setState({
        currentImgIndex: newIndex,
        src: thisSrc
      });
    }
  };

  getMaskedCanvas = () => {
    let canvas = document.createElement("canvas");
    let context = canvas.getContext("2d");
    let maskWidth = this.cropper && this.cropper.getData().width;
    let maskHeight = this.cropper && this.cropper.getData().height;
    let maskTop = this.cropper && this.cropper.getData().y;
    let maskLeft = this.cropper && this.cropper.getData().x;
    let imageWidth = this.cropper && this.cropper.getImageData().naturalWidth;
    let imageHeight = this.cropper && this.cropper.getImageData().naturalHeight;

    canvas.width = imageWidth;
    canvas.height = imageHeight;

    context.imageSmoothingEnabled = true;

    let image = new Image();
    image.src = this.state.src;
    context.drawImage(image, 0, 0, imageWidth, imageHeight);
    context.fillRect(maskLeft, maskTop, maskWidth, maskHeight);
    return canvas;
  };

  checkIfError = () => {
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
  };

  finishAction = () => {
    if (this.cropper) {
      //this.cropper && this.cropper.dismissAll();
      this.setState({
        isCropping: false,
        isRotating: false,
        isunDoing: false,
        isRedacting: false,
        isRedactClicking: false,
        isRefresh: false,
        infoPanelMessage: ""
      });
    }
  };

  setstateback = () => {
    this.setState({ isRedacting: false, useBlackCropper: false });
  };

  markImageAsDirty = () => {
    this.props.data[this.state.currentContinueImgIndex].isDirty = true;
  };

  componentWillUnmount() {
    window.removeEventListener("resize", this.updateDimensions);
    document.removeEventListener("click", this.handleClick);
    if (this.state.unListenHistoryHandler) this.state.unListenHistoryHandler();
  }

  componentDidMount = () => {
    const { history } = this.props;

    // this.props.data.length > 0 &&
    //   this.props.data.forEach((item, index) => {
    //     let tempCanvas = document.createElement('canvas');

    //     let context = tempCanvas.getContext('2d');
    //     let imageWidth = 0;
    //     let imageHeight = 0;

    //     context.imageSmoothingEnabled = true;

    //     let image = new Image();
    //     let myProps = this.props;
    //     image.addEventListener('load', function() {
    //       tempCanvas.width = this.naturalWidth;
    //       tempCanvas.height = this.naturalHeight;
    //       imageWidth = this.naturalWidth;
    //       imageHeight = this.naturalHeight;

    //       context.drawImage(image, 0, 0, imageWidth, imageHeight);

    //       myProps.data[index].url = tempCanvas.toDataURL();
    //     });
    //     image.src = this.props.data[index].url;
    //   });
    //Back button interception
    this.setState(
      {
        currentContinueImgIndex: this.props.currentImage,
        croppedImagesList: []
      },
      () => {
        if (this.props.data.length > 0) {
          let newArr = this.state.croppedImagesList.concat(
            this.props.data[this.props.currentImage].url
          );

          this.setState({ croppedImagesList: newArr }, () => {
            this.setState({
              currentImgIndex: this.state.croppedImagesList.length - 1,
              src: this.props.data[this.props.currentImage].url
            });

            this.checkIfError();
          });
        }
      }
    );
    const unListenHistoryHandler = history?.listen((newLocation, action) => {
      //Get lastTab to prevent app from going /home on back button press

      if (action === "POP") {
        this.state.isRedacting && this.confirmOption("r");
        this.state.isCropping && this.confirmOption("c");
        history.block();

        //TODO avoid running on cancel as well
        if (
          !this.state.isRedacting &&
          !this.state.isCropping &&
          !this.state.isRefresh
        ) {
          this.handleBackImageSave();
          return;
        }
      }
    });

    this.setState({ unListenHistoryHandler });
    window.addEventListener("resize", this.updateDimensions);
    document.addEventListener("click", this.handleClick);
  };

  confirmOption = action => {
    const { history } = this.props;
    var r = window.confirm(
      "Are you sure you want to go back? Any unfinished changes will be lost!"
    );

    if (r === true) {
      this.cropper && this.cropper.dismissAll();
      this.handleBackImageSave();
    } else {
      this.cropper && this.cropper.dismissAll();
      action === "r" && this.startMask();
      action === "c" && this.startCrop();
      this.setState({ isRefresh: true });
      action === "r" && this.cropper.setCropBoxData({ width: 200, height: 50 });
      history && history.block();
    }
  };

  updateDimensions = e => {
    this.cropper.zoomTo(1);
    let canvasData = this.cropper && this.cropper.getCanvasData();
    let elem = document.getElementsByClassName("cropper-wrap-box");
    const marginLeft = elem[0].offsetWidth / 4;
    const width = elem[0].offsetWidth / 2;

    if (
      Object.entries(canvasData).length !== 0 &&
      canvasData.constructor === Object
    ) {
      canvasData.width = width;
      canvasData.height = elem[0].offsetHeight;
      canvasData.left = marginLeft;
      canvasData.top = 0;

      this.cropper && this.cropper.setCanvasData(canvasData);
    }
  };

  finishIfCrop = () => {
    if (this.state.isRedacting) {
      this.markImageAsDirty();
      this.finishMask();
      this.finishAction();
      //this.cropper && this.cropper.dismissAll();
      this.setState({ isRedacting: false, infoPanelMessage: "" });
    }
    // if (this.state.isCropping) {
    //   this.finishCrop();
    //   this.finishAction();
    //   this.cropper && this.cropper.dismissAll();
    //   this.markImageAsDirty();
    //   this.setState({ isCropping: false, infoPanelMessage: "" });
    // }
    // if (this.state.isRotating) {
    //   this.markImageAsDirty();
    //   this.finishCrop();
    //   this.finishAction();
    //   this.cropper && this.cropper.dismissAll();
    //   this.setState({ isRotating: false, infoPanelMessage: "" });
    // }
  };

  saveImageCropper = isBack => {
    // if (this.cropper) {
    //   this.cropper && this.cropper.dismissAll();
    //   let canvas = this.cropper.getCroppedCanvas();
    //   let editedImage = canvas.toDataURL(
    //     this.props.data[this.state.currentContinueImgIndex].blob.type
    //   );
    //   this.props.data[this.state.currentContinueImgIndex].url = editedImage;
    //   this.props.data[this.state.currentContinueImgIndex].isDirty = true;

    //   this.props.showRedactionTool({
    //     show: isBack ? true : false,
    //     data: this.props.data,
    //     editedImage: editedImage,
    //     index: isBack
    //       ? this.state.currentContinueImgIndex
    //       : this.state.currentContinueImgIndex + 1,
    //     isBack: false
    //   });
    // }

    this.props.data[this.state.currentContinueImgIndex].url = this.state.src;
    this.props.data[this.state.currentContinueImgIndex].isDirty = true;
    this.movePage(isBack);
  };

  clearAndSetUndoQueue = newImage => {
    const croppedImagesList = [newImage];
    this.setState({ croppedImagesList });
  };

  movePage = isBack => {
    this.setState({ croppedImagesList: [] });
    this.props.showRedactionTool({
      show: isBack
        ? true
        : this.state.currentContinueImgIndex + 1 >= this.props.data.length
        ? false
        : true,
      data: this.props.data,
      editedImage: this.state.src,
      index: isBack
        ? this.state.currentContinueImgIndex - 1
        : this.state.currentContinueImgIndex + 1,
      isBack: isBack
    });
  };

  handlePreviousImgClick = () => {
    this.finishIfCrop();
    // this.saveImageCropper(true);
    //this.movePage()

    this.setState(
      {
        currentContinueImgIndex: this.state.currentContinueImgIndex - 1
      },
      () => {
        this.clearAndSetUndoQueue(
          this.props.data[this.state.currentContinueImgIndex].url
        );
        this.setState({
          src: this.props.data[this.state.currentContinueImgIndex].url
        });
        this.checkIfError();
      }
    );
  };

  handleNextImageClick = () => {
    this.finishIfCrop();
    //this.saveImageCropper(true);
    this.setState(
      {
        currentContinueImgIndex: this.state.currentContinueImgIndex + 1
      },
      () => {
        this.clearAndSetUndoQueue(
          this.props.data[this.state.currentContinueImgIndex].url
        );
        this.setState({
          src: this.props.data[this.state.currentContinueImgIndex].url
        });

        this.checkIfError();
      }
    );
  };

  handleMultiImageContinueClick = () => {
    // this.finishAction();
    this.finishIfCrop();
    //this.saveImageCropper();
    this.movePage(false);
  };

  handleSingleImageContinueClick = () => {
    //this.finishAction();
    this.finishIfCrop();
    // setTimeout(
    //   function() {
    this.movePage(false);
    // if (this.cropper) {
    //   let canvas;
    //   this.cropper && (canvas = this.cropper.getCroppedCanvas());
    //   let editedImage;
    //   canvas &&
    //     (editedImage = canvas.toDataURL(
    //       this.props.data[this.state.currentContinueImgIndex].blob.type
    //     ));

    //   this.props.data[0].url = editedImage;
    //   this.props.data[0].isDirty = true;
    //   this.props.showRedactionTool({
    //     show: false,
    //     data: this.props.data,
    //     editedImage: editedImage,
    //     index: this.state.currentContinueImgIndex + 1,
    //     isBack: false
    //   });
    // }
    //   }.bind(this),
    //   1000
    // );
  };

  handleRotateLeftClick = () => {
    if (this.state.isRotating) {
      this.rotateLeft();
    } else {
      this.finishAction();
      setTimeout(this.rotateLeft, 500);

      this.setState({ isRotating: true });
    }
  };

  handleRedactClick = () => {
    if (this.state.isRedacting) {
      this.finishMask();

      this.finishAction();
      this.setState({ isRedacting: false, infoPanelMessage: "" });
    } else {
      this.setState(
        {
          isRedacting: true,
          useBlackCropper: true,
          infoPanelMessage: "Draw a privacy box"
        },
        () => {
          //this.cropper && this.cropper.reset().clear();
          this.cropper && this.cropper.setDragMode("drag");

          this.cropper && this.cropper.crop();
          this.cropper &&
            this.cropper.setCropBoxData({ width: 250, height: 40 });
        }
      );
    }
  };

  handleCropClick = () => {
    if (this.state.isCropping) {
      // If crop is pressed again when cropping, this means cancel the crop.
      this.handleUndoClick();
    } else {
      this.finishAction();
      this.startCrop();
      //setTimeout(this.startCrop, 500);
      this.setState({
        isCropping: true,
        infoPanelMessage: "Resize crop area"
      });
    }
  };

  clickVideoDiv = () => {
    this.setState({ showPopupVideo: true });
  };

  handleClick = e => {
    let canvas = document.getElementsByClassName("cropper-container")[0];

    if (canvas && e.target.classList[0] == "cropper-drag-box") {
      var rect = canvas.getBoundingClientRect();
      if (this.state.isRedacting && !this.state.isRedactClicking) {
        this.setState({ isRedactClicking: true });
        //this.cropper && this.cropper.reset().clear();
        this.cropper && this.cropper.crop();
        this.cropper &&
          this.cropper.setCropBoxData({
            left: e.clientX - rect.left,
            top: e.clientY,
            width: 200,
            height: 50
          });
      } else if (this.state.isCropping && !this.state.isRedactClicking) {
        this.setState({ isRedactClicking: true });
        //this.cropper && this.cropper.reset().clear();
        this.cropper && this.cropper.crop();
        this.cropper &&
          this.cropper.setCropBoxData({
            left: e.clientX - rect.left,
            top: e.clientY,
            width: 200,
            height: 200
          });
      }
    }
  };

  handleUndoClick = () => {
    this.setstateback({ isunDoing: true });
    this.cropper && this.cropper.dismissAll();
    this.undoCrop();
    this.finishAction();
    this.updateDimensions();
  };

  handleBackImageSave = () => {
    if (this.props.data.length === 1) {
      this.handleSingleImageContinueClick();
    } else {
      this.handleMultiImageContinueClick();
    }
  };

  onPopupClose = () => {
    this.setState({ showHelpDialog: false });
  };

  render() {
    return (
      <Grid
        container
        style={{
          height: "100%"
        }}
        spacing={0}
      >
        {/* Mobile Purple Top Bar */}
        <Hidden only={["lg", "md", "xl"]}>
          <Grid container spacing={0}>
            <Grid
              item
              lg={12}
              md={12}
              style={{
                margin: "0 auto",
                background: "#6f1f62",
                width: "100%",
                color: "white",
                textAlign: "center"
              }}
            >
              <div
                style={{
                  margin: "0 auto",
                  background: "#6f1f62",
                  float: "none",
                  display: "flex",
                  textAlign: "center",
                  height: 30
                }}
              >
                <div
                  style={{
                    display: "flex",
                    paddingLeft: "25px",
                    width: "50%"
                  }}
                >
                  <div
                    style={{
                      position: "relative",
                      float: "left",
                      padding: 10,
                      height: 30,
                      cursor: "pointer",
                      width: "40px",
                      backgroundColor: this.state.isRedacting
                        ? "#9b6692"
                        : "#6f1f62",
                      textShadow: this.state.isRedacting
                        ? "2px 2px #000000"
                        : "none"
                    }}
                    className={"editDiv"}
                    onClick={this.handleRedactClick}
                  >
                    <AddBoxIcon
                      className={
                        this.state.isRedacting ? "svgShadow" : "editIco"
                      }
                      style={{
                        cursor: "pointer",
                        fontSize: 24,
                        zoom: "1.4"
                      }}
                    />
                  </div>
                  <div
                    style={{
                      width: 30,
                      position: "relative",
                      float: "left",
                      padding: 10,
                      height: 30,
                      width: "40px",
                      cursor: "pointer"
                    }}
                    onClick={this.handleUndoClick}
                  >
                    <UndoIcon
                      style={{
                        cursor: "pointer",
                        fontSize: 24,
                        zoom: "1.4"
                      }}
                    />
                  </div>
                </div>
                <div
                  style={{
                    display: "flex",
                    justifyContent: "flex-end",
                    width: "50%",
                    paddingRight: "25px"
                  }}
                >
                  <div
                    style={{
                      width: 30,
                      position: "relative",
                      float: "left",
                      padding: 10,
                      height: 30,
                      cursor: "pointer",
                      width: "40px",
                      backgroundColor: "#6f1f62",
                      textShadow: "none"
                    }}
                    onClick={this.handleCropClick}
                  >
                    <CropIcon
                      style={{
                        cursor: "pointer",
                        fontSize: 24,
                        zoom: "1.4"
                      }}
                    />
                  </div>

                  <div
                    style={{
                      position: "relative",
                      float: "left",
                      padding: 10,
                      height: 30,
                      cursor: "pointer",
                      width: "40px"
                    }}
                    onClick={this.handleRotateLeftClick}
                  >
                    <Cached
                      style={{
                        cursor: "pointer",
                        fontSize: 24,
                        zoom: "1.4"
                      }}
                    />
                  </div>
                </div>
              </div>
              <br></br>
              <div
                style={{
                  textAlign: "left",
                  height: "43px",
                  backgroundColor: "black",
                  width: "100%",
                  fontSize: "16px",
                  fontWeight: "bold",
                  paddingRight: "30px"
                }}
              >
                <div
                  style={{
                    textAlign: "left",

                    fontSize: "16px",
                    fontWeight: "bold",
                    paddingRight: "30px",
                    float: "left",
                    paddingTop: 10,
                    paddingLeft: 10
                  }}
                >
                  {this.state.infoPanelMessage}{" "}
                </div>
                <div
                  style={{
                    textAlign: "Right",
                    height: "43px",
                    color: "#aaaaaa",

                    fontSize: "12px",
                    fontWeight: "bold",
                    paddingRight: 10,
                    paddingTop: 20
                  }}
                >
                  Pinch to zoom
                </div>
              </div>
            </Grid>
          </Grid>
        </Hidden>
        {/* Desktop Purple Left Bar */}
        <Hidden only={["sm", "xs"]}>
          <Grid
            item
            lg={3}
            md={4}
            sm={12}
            style={{
              background: "#6f1f62",
              color: "white",
              width: "100%",
              position: "relative",
              maxWidth: 340
            }}
          >
            <Grid container spacing={0}>
              <Grid item lg={12} md={12}>
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
                      width: 30,
                      position: "relative",
                      float: "left",
                      padding: 5,
                      width: "20%",
                      cursor: "pointer"
                    }}
                    onClick={this.handleUndoClick}
                  >
                    <UndoIcon
                      style={{
                        paddingBottom: 5,
                        cursor: "pointer",
                        fontSize: 42
                      }}
                    />

                    <p className={"redact-icon-label"}>{fm.undo}</p>
                  </div>

                  <div
                    style={{
                      position: "relative",
                      float: "left",
                      padding: 5,
                      width: "60%",
                      backgroundColor: this.state.isRedacting
                        ? "#9b6692"
                        : "#6f1f62",
                      cursor: "pointer",
                      textShadow: this.state.isRedacting
                        ? "2px 2px #000000"
                        : "none"
                    }}
                    className={"editDiv"}
                    onClick={this.handleRedactClick}
                  >
                    <AddBoxIcon
                      className={
                        this.state.isRedacting ? "svgShadow" : "editIco"
                      }
                      style={{
                        paddingBottom: 5,
                        cursor: "pointer",
                        fontSize: 42
                      }}
                    />
                    <p className={"redact-icon-label"}>{fm.privacyBox}</p>
                  </div>
                </div>
              </Grid>
            </Grid>
            <Grid style={{ textAlign: "center" }} container spacing={0}>
              <div
                style={{
                  width: "100%",
                  paddingLeft: 5,
                  paddingRight: 5
                }}
                onClick={this.clickVideoDiv}
              >
                {this.state.showHelpDialog ? (
                  <VimeoPlayer
                    width={"1000"}
                    height={"430"}
                    videos={[
                      { url: "382824142" },
                      { url: "382824153" },
                      { url: "382824157" },
                      { url: "385316103" },
                      { url: "382824179" },
                      { url: "382824186" },
                      { url: "382824199" },
                      { url: "382824211" },
                      { url: "382824222" },
                      { url: "382824232" },
                      { url: "382824246" },
                      { url: "382824273" }
                    ]}
                    isPopup
                    defaultUrl={"382824211"}
                    showList
                    autoplay
                    onPopupClose={this.onPopupClose}
                    //hasPlaybar
                    noTitle
                    show={this.state.showHelpDialog}
                  />
                ) : (
                  <p></p>
                )}
                <div className="cropperVideoContainer" id="cropperVideoDiv">
                  {renderIf(
                    !this.props.hideVideoPlayer,
                    <React.Fragment>
                      <VimeoPlayer
                        videos={[{ url: "382824211" }]}
                        hasPlaybar
                        muted
                        autoplay
                        noTitle
                      />
                      <div
                        className="cropperVideoOverlay"
                        onClick={() => {
                          this.setState({ showHelpDialog: true });
                        }}
                      />
                    </React.Fragment>
                  )}
                </div>
              </div>
              <div
                style={{
                  padding: "20px 10px 20px 0px",
                  margin: "0 auto",
                  width: "100%",
                  float: "none",
                  display: "flex"
                }}
              >
                <div
                  style={{
                    width: 30,
                    position: "relative",
                    float: "left",
                    padding: 5,
                    width: "25%",
                    backgroundColor: "#6f1f62",
                    textShadow: "none",
                    cursor: "pointer"
                  }}
                  onClick={this.handleCropClick}
                >
                  <CropIcon
                    style={{
                      paddingBottom: 10,
                      cursor: "pointer",
                      fontSize: 42
                    }}
                  />
                  <p className={"redact-icon-label-small"}>{fm.crop}</p>
                </div>
                <div
                  style={{
                    position: "relative",
                    float: "left",
                    padding: 5,
                    width: "25%",
                    cursor: "pointer"
                  }}
                  onClick={this.handleRotateLeftClick}
                >
                  <Cached
                    style={{
                      paddingBottom: 10,
                      cursor: "pointer",
                      fontSize: 42
                    }}
                  />

                  <p className={"redact-icon-label-small"}>{fm.rotate}</p>
                </div>
                <div
                  style={{
                    width: 80,
                    padding: 5,
                    cursor: "pointer",
                    width: "25%"
                  }}
                  onClick={this.zoomIn}
                >
                  <ZoomInIcon
                    style={{
                      paddingBottom: 10,
                      cursor: "pointer",
                      fontSize: 42
                    }}
                  />

                  <p
                    className={"redact-icon-label-small"}
                    style={{
                      marginLeft: 6
                    }}
                  >
                    {fm.zoomIn}
                  </p>
                </div>
                <div
                  style={{
                    width: 80,
                    padding: 5,
                    cursor: "pointer",
                    paddingRight: 8,
                    width: "25%"
                  }}
                  onClick={this.zoomOut}
                >
                  <ZoomOutIcon
                    style={{
                      paddingBottom: 10,
                      cursor: "pointer",
                      fontSize: 42
                    }}
                  />

                  <p className={"redact-icon-label-small"}>{fm.zoomOut}</p>
                </div>
              </div>
            </Grid>
            <Grid
              style={{ textAlign: "center", height: "100%" }}
              container
              spacing={0}
            >
              <div
                id="navigation"
                style={{
                  margin: " 0 auto",
                  textAlign: "center",
                  zIndex: 10,
                  bottom: 50,
                  position: "absolute",
                  width: "100%"
                }}
              >
                {this.props.data.length === 1 ? (
                  <React.Fragment>
                    <CustomButton
                      style={{
                        margin: "10px 0",
                        width: 180
                      }}
                      btnBigLabel={true}
                      secondary={true}
                      onClick={this.handleSingleImageContinueClick}
                      data-testid="continue"
                      label={fm.continueButton}
                    />
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
                            height: "50px"
                          }}
                        >
                          <CustomButton
                            style={{
                              margin: "10px 0"
                            }}
                            // secondary={true}
                            leftIcon={<ArrowBackIosIcon />}
                            isText={true}
                            //btnBigLabel={true}
                            onClick={this.handlePreviousImgClick}
                            data-testid="continue"
                            label={fm.prevPage}
                          />

                          <span style={{ textAlign: "right", marginLeft: 40 }}>
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
                            margin: "0px 0px 0px 167px",
                            color: "white",
                            align: "center",
                            height: "50px"
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
                        <CustomButton
                          style={{
                            margin: "10px 0"
                          }}
                          secondary={true}
                          btnBigLabel={true}
                          onClick={this.handleNextImageClick}
                          data-testid="continue"
                          label={fm.nextPage}
                        />
                      </React.Fragment>
                    ) : (
                      <CustomButton
                        style={{
                          margin: "10px 0"
                        }}
                        secondary={true}
                        btnBigLabel={true}
                        onClick={this.handleMultiImageContinueClick}
                        data-testid="continue"
                        label={fm.continueButton}
                      />
                    )}
                  </React.Fragment>
                )}
              </div>
              <div className="btn-group no-focus">
                <div style={{ padding: "20px" }}>
                  <font className=" no-focus" size="3">
                    {fm.cropperWhatShouldObscure}
                  </font>
                </div>
              </div>
            </Grid>
          </Grid>
        </Hidden>
        {/* Cropper Content  */}

        <Grid item lg={9} md={8} style={{ margin: " 0 auto" }}>
          <Cropper
            aspectRatio={NaN}
            className={this.state.useBlackCropper ? "blackCropper" : ""}
            minContainerWidth={130}
            src={this.state.src}
            dragMode={this.state.dragMode}
            autoCrop={this.state.autoCrop}
            viewMode={0}
            movable={true}
            finishCrop={this.finishCrop}
            setstateback={this.setstateback}
            isRedacting={this.state.isRedacting}
            isCropping={this.state.isCropping}
            ref={cropper => {
              this.cropper = cropper;
            }}
            ready={this.onCropperReady}
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
        </Grid>

        {/* Mobile Purple Buttom Bar */}
        <Hidden only={["lg", "md", "xl"]}>
          <Grid
            item
            lg={3}
            md={4}
            sm={12}
            style={{
              background: "#6f1f62",
              color: "white",
              width: "100%"
            }}
          >
            <Grid style={{ textAlign: "center" }} container spacing={0}></Grid>
            <div className="btn-group">
              <div
                id="navigation"
                style={{
                  margin: " 0 auto",
                  textAlign: "center",
                  minWidth: 340,
                  maxWidth: 600
                }}
              >
                {this.props.data.length === 1 ? (
                  <React.Fragment>
                    <CustomButton
                      style={{
                        margin: "10px 0"
                      }}
                      secondary={true}
                      btnBigLabel={true}
                      onClick={this.handleSingleImageContinueClick}
                      data-testid="continue"
                      label={fm.continueButton}
                    />
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
                          <CustomButton
                            style={{
                              margin: "10px 0"
                            }}
                            // secondary={true}
                            isText={true}
                            leftIcon={<ArrowBackIosIcon />}
                            //btnBigLabel={true}
                            onClick={this.handlePreviousImgClick}
                            data-testid="continue"
                            label={fm.prevPage}
                          />

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
                        <CustomButton
                          style={{
                            margin: "10px 0"
                          }}
                          secondary={true}
                          btnBigLabel={true}
                          onClick={this.handleNextImageClick}
                          data-testid="continue"
                          label={fm.nextPage}
                        />
                      </React.Fragment>
                    ) : (
                      <CustomButton
                        style={{
                          margin: "10px 0"
                        }}
                        secondary={true}
                        btnBigLabel={true}
                        onClick={this.handleMultiImageContinueClick}
                        data-testid="continue"
                        label={fm.continueButton}
                      />
                    )}
                  </React.Fragment>
                )}
              </div>
              <div style={{ padding: "20px" }}>
                <font className=" no-focus" size="3">
                  {fm.cropperWhatShouldObscure}
                </font>
              </div>
            </div>
          </Grid>
        </Hidden>
      </Grid>
    );
  }
}

export default cropperComponent;
