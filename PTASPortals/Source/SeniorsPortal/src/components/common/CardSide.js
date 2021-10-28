import React from 'react';
import HelpOutline from '@material-ui/icons/HelpOutline';
import './CardSide.css';
import { renderIf } from '../../lib/helpers/util';
import { Hidden } from '@material-ui/core';

class CardSide extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div className="card-side">
        <Hidden smDown>
          <HelpOutline className="help-icon" />
          {renderIf(
            this.props.header,
            <span className="help-header">{this.props.header}</span>
          )}
          {this.props.content &&
            this.props.content.map((paragraph, index) => {
              return (
                <p style={{ marginTop: '8px' }} key={index}>
                  {paragraph}
                </p>
              );
            })}
        </Hidden>
      </div>
    );
  }
}

export default CardSide;
