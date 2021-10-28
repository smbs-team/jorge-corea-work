import React, { Component } from 'react';

class CustomIFrame extends Component {
  constructor(props) {
    super(props);
    this.state = {};
    this.iFrame = React.createRef();
  }

  componentDidMount() {
    let summaryHtml = this.iFrame.current.contentWindow.document;
    summaryHtml.open();
    let html =
      this.props.summaryHtml === null
        ? JSON.parse(
            localStorage.getItem(
              `${this.props.seniorApp.seapplicationid}_DocuSignHTML`
            )
          )
        : this.props.summaryHtml;
    summaryHtml.write(html);
    summaryHtml.close();
  }
  resizeIframe() {
    if (this.iFrame && this.iFrame.current && this.iFrame.current.style) {
      this.iFrame.current.style.height =
        this.iFrame.current.contentWindow.document.documentElement
          .scrollHeight + 'px';
    }
  }
  render() {
    return (
      <iframe
        onLoad={this.resizeIframe()}
        id={this.props.id}
        ref={this.iFrame}
        class="summary-iframe"
        src="about:blank"
      ></iframe>
    );
  }
}

export default CustomIFrame;
