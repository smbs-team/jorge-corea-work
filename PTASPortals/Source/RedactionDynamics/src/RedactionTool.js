import React from "react";
//import ImageCrossOut from "./components/redaction-component/ImageCrossOut";
import ImageCrossOut from "./components/common/redaction-component/ImageCrossOut";
import logo from "./logo.svg";
import strings from "./strings.json";
import { cloneDeep } from "lodash";
import "./App.css";
import { validateFileExtension, getQueryVariable } from "./lib/helpers/util";
import { getDocument, getFile, updateDocument } from "./lib/helpers/services";
import decodeJWT from "jwt-decode";
import notAnImage from "./assets/notAnImage.png";
import { getIndexFromFileName } from "./lib/helpers/util";

/**
 * This is the redaction component.
 *
 * @class RedactionTool
 * @extends {React.Component}
 */
class RedactionTool extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      show: true,
      isSharePoint: false,
      index: 0,
      currentImage: {},
      error: null,
      saving: false,
      data: null,
      loadingMessage: strings.loading,
      loading: true,
      token: null,
      decodedToken: null,
      tokenExpirationTimeOutId: null,
      allOtherFiles: []
    };

    this.dataToSave = [];
  }

  async componentWillMount() {
    var docid = getQueryVariable("id");
    if (docid === null || docid === undefined) {
      this.setState({
        show: true,
        index: 0,
        currentImage: 0,
        error: strings.NotFoundError,
        saving: false,
        data: null
      });
      return;
    }
    const isSharePoint = getQueryVariable("isSharepoint");
    const token = getQueryVariable("token");
    const decodedToken = token && token != "notyet" ? decodeJWT(token) : null;
    const timeOutSetUp = this.setSessionTimeOut(decodedToken);

    if (timeOutSetUp) {
      this.setState(
        { token, decodedToken, isSharePoint: isSharePoint === "true" },
        async () => {
          const externalData = await this.getData(docid);

          this._asyncRequest = null;

          if (externalData == null) {
            this.setState({
              show: true,
              index: 0,
              currentImage: 0,
              saving: false,
              data: null
            });
          } else if (externalData && externalData.length === 0) {
            this.setState({
              error: `${strings.error}${strings.noImagesFound}`
            });
          } else {
            this.setState({
              show: true,
              index: 0,
              currentImage: 0,
              error: null,
              saving: false,
              data: externalData
            });
          }
        }
      );
    }
  }

  componentWillUnmount() {
    if (this._asyncRequest) {
      this._asyncRequest.cancel();
    }

    this.clearTokenTimeOut();
  }

  clearTokenTimeOut = () => {
    if (this.state.tokenExpirationTimeOutId) {
      clearTimeout(this.state.tokenExpirationTimeOutId);
    }
  };

  setError = errorMessage => {
    this.setState({ error: errorMessage });
  };

  getData = async docid => {
    let docInfo = await getDocument(
      docid,
      this.state.token,
      this.state.isSharePoint,
      this.setError
    );
    //let data = [docInfo ? docInfo.length : 0];
    let data = [];
    if (docInfo) {
      if ((docInfo.error && docInfo.error.message) || docInfo.message) {
        this.setState({ error: `${strings.error}${strings.notFoundError}` });

        return null;
      }

      this.dataToSave = [];

      this.setState({ loadingMessage: strings.loadingImages, loading: true });
      let promises = docInfo.map(async (element, index) => {
        const imageName = this.state.isSharePoint
          ? element.split("/")[1]
          : element.fileName;
        let imageInfo = await this.getImage(element);
        if (imageInfo) {
          if (imageInfo.length) {
            imageInfo.map(image => {
              data.push(this.createImageData(docid, image.fileName, image));
            });
          } else {
            data.push(this.createImageData(docid, imageName, imageInfo));
          }
        }
      });

      await Promise.all(promises);
      data.sort((a, b) => {
        const indexA = getIndexFromFileName(a.imageName);
        const indexB = getIndexFromFileName(b.imageName);
        if (indexA < indexB)
          //sort string ascending
          return -1;
        if (indexA > indexB) return 1;
        return 0; //default return value (no sorting)
      });
      this.setState({ loadingMessage: strings.loading, loading: false });
      return data;
    }
  };

  /**
   *
   *
   * @memberof RedactionTool
   */
  createImageData = (docid, imageName, imageInfo) => {
    return {
      documentId: docid,
      imageName: imageName,
      isDirty: false,
      isLoading: false,
      isUploaded: true,
      isUploading: false,
      isValid: imageInfo.isValidFile,
      error: !imageInfo.isValidFile
        ? `This is not an image file: ${imageName}`
        : null,
      url: imageInfo.image,
      blob: imageInfo.blob
    };
  };

  setSessionTimeOut = decodedToken => {
    if (decodedToken && decodedToken.exp) {
      const timestamp = Math.round(Date.now() / 1000);

      if (decodedToken.exp <= timestamp) {
        this.setState({ error: strings.expiredSession });
        return false;
      } else {
        // Expire session 1 minute before expiration time to avoid token expiring mid request.
        const secondsToExpire = decodedToken.exp - timestamp - 60;

        const tokenExpirationTimeOutId = setTimeout(() => {
          this.setState({ error: strings.expiredSession });
        }, secondsToExpire * 1000);

        this.setState({ tokenExpirationTimeOutId });
        return true;
      }
    } else {
      return true;
    }
  };

  getImage = async file => {
    const fileInfo = await getFile(
      file,
      this.state.token,
      this.state.isSharePoint
    );
    let image = null;

    if (fileInfo && fileInfo.length) {
      return fileInfo.map(file => {
        return {
          blob: file.blob,
          image: URL.createObjectURL(file.blob),
          isValidFile: true,
          fileName: file.fileName
        };
      });
    } else if (fileInfo && validateFileExtension(fileInfo.type)) {
      image = {
        blob: fileInfo,
        image: URL.createObjectURL(fileInfo),
        isValidFile: true
      };
    } else {
      image = {
        blob: fileInfo,
        image: notAnImage,
        isValidFile: false
      };
    }

    return image;
  };

  dataURLtoBlob(dataurl) {
    if (dataurl && dataurl.split(",").length > 1) {
      var arr = dataurl.split(","),
        mime = arr[0].match(/:(.*?);/)[1],
        bstr = atob(arr[1]),
        n = bstr.length,
        u8arr = new Uint8Array(n);
      while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
      }
      return new Blob([u8arr], { type: mime });
    }

    return null;
  }

  saveData = async e => {
    this.setState({
      show: false,
      // data: e.data,
      saving: true,
      currentImage: e.index
    });

    let fd = new FormData();

    // Add the updated files to the payload.
    this.state.data.map(element => {
      let blob = null;
      if (element.isValid) {
        blob = this.dataURLtoBlob(element.url);
        if (blob) {
          // make blob like file.
          blob.lastModifiedDate = new Date();
          blob.name = element.imageName;
        }
      }

      if (!blob) {
        blob = element.blob;
      }

      if (blob) {
        fd.append("files[]", blob, element.imageName);
      }
    });

    // Will incorporate the other type of files back to the payload so they don't get removed.
    this.state.allOtherFiles.map(otherFileType => {
      fd.append(
        "files[]",
        otherFileType.blob,
        `${otherFileType.fileId}${otherFileType.fileExtension}`
      );
    });

    // fd.append("documentId", e.data[0].documentId);
    const firstResponse = await updateDocument(
      e.data[0].documentId,
      fd,
      this.state.token,
      this.state.isSharePoint,
      this.setError
    );

    if (firstResponse?.Error) {
      this.setState({
        show: true,
        index: e.index,
        currentImage: null,
        error: firstResponse.errorMessage,
        saving: false,
        data: null
      });
    }

    if (!e.show && !firstResponse?.Error) {
      console.log("save inside close");
      this.setState({ isSaved: true, saving: false });
      this.clearTokenTimeOut();
      window.close();
    }
  };

  updateData = (index, url, isDirty) => {
    if (this.state.data[index].url !== url) {
      let data = cloneDeep(this.state.data);

      data[index].url = url;
      data[index].isDirty = isDirty;
      this.setState({ data });
    }
  };

  handleRedactionTool = async e => {
    if (e.index == e.data.length) {
      await this.saveData(e);
    } else {
      this.setState({
        show: true,
        index: e.index + 1 < e.data.length ? e.index + 1 : e.index,
        currentImage: e.index + 1 < e.data.length ? e.index + 1 : e.index,
        error: null,
        saving: false,
        data: e.data
      });
    }
    // } else {
    if (!e.show) {
      this.setState({
        show: false,
        data: null,
        error: strings.error_noeditedimage
      });
    }
    // }
  };

  render() {
    console.log("App: ", this.state);
    if (this.state.error != null) {
      return (
        <center>
          <div className="App-error">{this.state.error}</div>
        </center>
      );
    } else if (this.state.isSaved) {
      return (
        <center>
          <div className="App-message">
            <img src={logo} className="App-logo" alt="icon" />
            <br />
            {strings.isSaved}
          </div>
        </center>
      );
    } else if (this.state.saving) {
      return (
        <center>
          <div className="App-message">
            <img src={logo} className="App-logo" alt="icon" />
            <br />
            {strings.saving}
          </div>
        </center>
      );
    } else if (this.state.data == null || this.state.loading) {
      return (
        <center>
          <div className="App-message">
            <img src={logo} className="App-logo" />
            <br />
            {this.state.loadingMessage}
          </div>
        </center>
      );
    } else {
      return (
        <div className="App">
          <ImageCrossOut
            showDialog={this.state.show}
            data={this.state.data}
            showRedactionTool={this.handleRedactionTool}
            index={this.state.index}
            currentImage={this.state.index}
            dataURLtoBlob={this.dataURLtoBlob}
            updateData={this.updateData}
            hideVideoPlayer={true}
          />
        </div>
      );
    }
  }
}

export default RedactionTool;
