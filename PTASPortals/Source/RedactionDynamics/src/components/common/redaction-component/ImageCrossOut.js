import React from "react";
import "./ImageCrossOut.css";
import Dialog from "@material-ui/core/Dialog";
import CropperComponent from "../cropper-component/cropperComponent";

class ImageCrossOut extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    let data =
      this.props.data && this.props.data.length ? this.props.data : null;
    let isMobile = window.innerWidth <= 760;
    return (
      <div data-testid="imageCrossOut">
        <Dialog
          maxWidth={"lg"}
          fullScreen
          onClose={this.handleClose}
          aria-labelledby="customized-dialog-title"
          open={this.props.showDialog}
        >
          <CropperComponent
            style={{ minHeight: "800px" }}
            showSavingOverlay={this.props.showSavingOverlay}
            showRedactionTool={this.props.showRedactionTool}
            data={this.props.data}
            index={this.props.index}
            currentImage={this.props.currentImage}
            hideVideoPlayer={this.props.hideVideoPlayer}
          />
        </Dialog>
      </div>
    );
  }
}

export default ImageCrossOut;
