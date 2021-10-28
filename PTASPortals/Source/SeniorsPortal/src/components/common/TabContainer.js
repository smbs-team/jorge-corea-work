import React from 'react';
import './TabContainer.css';
import Container from '@material-ui/core/Container';

// This component will be the base style for the containers replacing the content under the toolbar control
const TabContainer = props => {
  //TODO: Add styles to container.
  return (
    <div className="tab-container">
      <Container
        maxWidth="xl"
        className={'container ' + (props.nomargin ? 'summaryCont' : '')}
      >
        {props.children}
      </Container>
    </div>
  );
};

export default TabContainer;
