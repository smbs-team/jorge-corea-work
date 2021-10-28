import React, { label, onclick } from 'react';
import './ExpandLink.css';
import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import Fab from '@material-ui/core/Fab';
import AddIcon from '@material-ui/icons/Add';
import shadows from '@material-ui/core/styles/shadows';

const useStyles = makeStyles(theme => ({
  button: {
    margin: theme.spacing(1),
  },
  leftIcon: {
    marginRight: theme.spacing(1),
  },
  rightIcon: {
    marginLeft: theme.spacing(1),
  },
  iconSmall: {
    fontSize: 20,
  },
}));

const style = {
  color: 'white',
  backgroundColor: '#7a7a7a',
  width: '28px',
  height: '28px',
  minHeight: '28px',
  minWidth: '28px',
  boxShadow: 'none',
};

class ExpandLink extends React.Component {
  constructor(props) {
    super(props);
    // this.classes = useStyles();
  }

  handleClick() {
    this.props.onclick();
  }

  render() {
    return (
      <div className="expand-link">
        <Fab
          className="expand-link-icon"
          style={style}
          aria-label="Add"
          onClick={this.handleClick.bind(this)}
        >
          <AddIcon className="expand-link-add-icon" />
        </Fab>

        <div
          data-testid={this.props.testid}
          className="expand-link-label"
          onClick={this.handleClick.bind(this)}
        >
          {this.props.label}
        </div>
      </div>
    );
  }
}

export default ExpandLink;
