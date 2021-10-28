import React from 'react';
import './ToolbarItem.css';
import { FormattedMessage } from 'react-intl';
const ToolbarItem = props => {
  return (
    <div
      id={'tab' + props.id}
      data-isSelected={props.isSelected}
      className={props.isSelected ? 'yearTabSelected' : 'yearTab'}
      onClick={props.onClick}
    >
      <FormattedMessage id={props.yearLoc} />
    </div>
  );
};

export default ToolbarItem;
