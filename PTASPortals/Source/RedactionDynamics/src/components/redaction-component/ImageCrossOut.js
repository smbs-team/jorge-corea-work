import React from "react";
import StatusChange from "./status-change-example.jpg";
import ImageClear from "./round-cancel-24px.svg";
import Resize from "./baseline-arrow_drop_down_circle-28px.svg";
import paper from "paper/dist/paper-full";
import SvgIcon from "@material-ui/core/SvgIcon";
import { withStyles } from "@material-ui/core/styles";
import Button from "@material-ui/core/Button";
import Dialog from "@material-ui/core/Dialog";
import MuiDialogTitle from "@material-ui/core/DialogTitle";
import MuiDialogContent from "@material-ui/core/DialogContent";
import MuiDialogActions from "@material-ui/core/DialogActions";
import IconButton from "@material-ui/core/IconButton";
import CloseIcon from "@material-ui/icons/Close";
import Typography from "@material-ui/core/Typography";
import { renderIf } from "../../lib/helpers/util";
import { makeStyles } from "@material-ui/core/styles";
import { blue, red } from "@material-ui/core/colors";
import { project, Color } from "paper";
import { isDeepStrictEqual } from "util";
import CropperComponent from "../cropper-component/cropperComponent";

const iconStyles = theme => ({
  root: {
    display: "flex",
    justifyContent: "center",
    alignItems: "flex-end",
    padding: 0
  },
  actionIcons: {
    justifyContent: "center",
    alignItems: "center"
  },
  icon: {
    margin: 2
  },
  iconHover: {
    margin: 2,
    "&:hover": {
      color: red[800]
    }
  }
});

const styles = theme => ({
  root: {
    margin: 0,
    padding: 2
  },
  closeButton: {
    position: "absolute",
    right: 1,
    top: 1,
    color: theme.palette.grey[500]
  }
});

const DialogTitle = withStyles(styles)(props => {
  const { children, classes, onClose } = props;
  return (
    <MuiDialogTitle disableTypography className={classes.root}>
      <Typography variant="h6">{children}</Typography>
      {onClose ? (
        <IconButton
          aria-label="close"
          className={classes.closeButton}
          onClick={onClose}
        >
          <CloseIcon />
        </IconButton>
      ) : null}
    </MuiDialogTitle>
  );
});

const DialogContent = withStyles(theme => ({
  root: {
    padding: theme.spacing(2)
  }
}))(MuiDialogContent);

const DialogActions = withStyles(theme => ({
  root: {
    margin: 0,
    padding: theme.spacing(1)
  }
}))(MuiDialogActions);

class ImageCrossOut extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      isDrawing: true,
      pathsList: Array(1000).fill(null),
      rectangleList: Array(1000).fill(null),
      eraseButtonList: Array(1000).fill(null),
      resizeButtonList: Array(1000).fill(null),
      selectedPathIndex: -1,
      pathListIndex: 0,
      currentPathListIndex: 0,
      showDialog: true,
      boxSize: new paper.Size(200, 50),
      raster: null,
      isRedactionUnderline: true,
      isZoomInUnderline: false,
      isZoomOutUnderline: false,
      isErrorVisible: true,
      canvas: null
    };

    this.canvasRef = React.createRef();
  }

  componentDidMount() {}

  componentDidUpdate = (prevProps, prevState) => {};

  render() {
    const classes = iconStyles();
    let data =
      this.props.data && this.props.data.length ? this.props.data : null;
    console.log("data - ", data);
    return (
      <Dialog
        maxWidth={true}
        maxHeight={true}
        fullScreen={true}
        onClose={this.handleClose}
        aria-labelledby="customized-dialog-title"
        style={{ padding: 0 }}
        open={this.props.showDialog}
      >
        <DialogContent style={{ padding: 0, width: "100%" }}>
          <CropperComponent
            style={{ minHeight: "800px", padding: 0 }}
            showSavingOverlay={this.props.showSavingOverlay}
            showRedactionTool={this.props.showRedactionTool}
            data={this.props.data}
            index={this.props.index}
            dataURLtoBlob={this.props.dataURLtoBlob}
            updateData={this.props.updateData}
          />
        </DialogContent>
      </Dialog>
    );
  }
}

export default ImageCrossOut;
