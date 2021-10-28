//-----------------------------------------------------------------------
// <copyright file="ContentViewer.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React, { Component } from 'react';

import { Document, Page, pdfjs } from 'react-pdf';
import { arrayNullOrEmpty, renderIf } from 'util';

class ContentViewer extends Component {
  constructor(props) {
    super(props);
    this.state = {
      numPages: null,
      pageNumber: 1,
      dismissExternalLibraries: false,
    };
  }
  onDocumentLoadSuccess = ({ numPages }) => {
    this.setState({ numPages });
  };
  componentWillMount() {
    var script = document.createElement('script');
    script.src = 'https://unpkg.com/x-frame-bypass';
    let dismissExtL =
      process.env.REACT_APP_IGNORE_EXTERNAL_CORS === 'true' ? true : false;
    this.setState({ dismissExternalLibraries: dismissExtL });

    !this.state.dismissExternalLibraries && document.head.appendChild(script);
  }
  componentDidMount() {
    console.log('contentviewer', pdfjs.version);
    pdfjs.GlobalWorkerOptions.workerSrc =
      '//cdnjs.cloudflare.com/ajax/libs/pdf.js/' +
      pdfjs.version +
      '/pdf.worker.js';
  }
  createPages = pnumPages => {
    let table = [];

    for (let i = 0; i < pnumPages; i++) {
      table.push(<Page pageNumber={i + 1} />);
    }
    return table;
  };
  render() {
    const { pageNumber, numPages } = this.state;
    let dismissExternalLibraries =
      process.env.REACT_APP_IGNORE_EXTERNAL_CORS === 'true' ? true : false;
    return (
      <React.Fragment>
        {this.state.dismissExternalLibraries ? (
          <div>
            {this.props.pdfUrl && (
              <Document
                file={this.props.pdfUrl}
                onLoadSuccess={this.onDocumentLoadSuccess}
              >
                {this.createPages(numPages)}
              </Document>
            )}
            {this.props.siteUrl && (
              <div>
                <iframe
                  title="frame"
                  ref={this.props.siteUrl}
                  src={this.props.siteUrl}
                ></iframe>
              </div>
            )}
          </div>
        ) : (
          <div>
            {this.props.pdfUrl && (
              <Document
                file={
                  'https://cors-anywhere.herokuapp.com/' + this.props.pdfUrl
                }
                onLoadSuccess={this.onDocumentLoadSuccess}
              >
                {this.createPages(numPages)}
              </Document>
            )}
            {this.props.siteUrl && (
              <div>
                <iframe
                  title="frame"
                  is="x-frame-bypass"
                  ref={this.props.siteUrl}
                  src={this.props.siteUrl}
                ></iframe>
              </div>
            )}
          </div>
        )}
      </React.Fragment>
    );
  }
}

export default ContentViewer;
