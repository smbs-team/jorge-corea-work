import React, {
  label,
  name,
  value,
  onChange,
  source,
  information,
  readOnly,
} from 'react';
import './SelectInput.css';
import Select from '@material-ui/core/Select';
import { FormattedMessage } from 'react-intl';
import { withStyles } from '@material-ui/core/styles';
import FormControl from '@material-ui/core/FormControl';
import InputLabel from '@material-ui/core/InputLabel';
import Input from '@material-ui/core/Input';
import MenuItem from '@material-ui/core/MenuItem';
import Information from '../common/Information';
import { renderIf } from '../../lib/helpers/util';
import PropTypes from 'prop-types';

const MenuItemCSS = withStyles({
  selected: {
    backgroundColor: '#a5c727 !important',
  },
})(MenuItem);

const FormControlCSS = withStyles({
  root: {
    '& label.Mui-focused': {
      color: '#a5c727',
    },
    '& .MuiInput-underline:after': {
      borderBottomColor: '#a5c727',
    },
    '& .MuiInput-root': {
      width: '300px',
      fontSize: '24px',
      fontWeight: 'bold',
    },
    '& .MuiFormLabel-root': {
      width: '320px',
    },
  },
})(FormControl);

const styles = theme => ({
  select: {},
});

class SelectInputW extends React.Component {
  constructor(props) {
    super(props);
  }

  InformationAvailable() {
    if (this.props.information != null) {
      return (
        <Information
          style={{
            verticalAlign: 'bottom',
            marginLeft: '105px',
            marginTop: '10px',
          }}
        />
      );
    }
  }

  render() {
    const { classes } = this.props;
    let input = this.props.readOnly ? (
      <Input id="select1" readOnly />
    ) : (
      <Input id="select1" />
    );
    return (
      <div className="align-left">
        <div style={{ display: 'inline-block' }}>
          <FormControlCSS className="select-input">
            <InputLabel htmlFor="select-multiple" shrink={true}>
              {this.props.label}
            </InputLabel>
            <Select
              name={this.props.name}
              displayEmpty={this.props.displayEmpty}
              value={this.props.value}
              onChange={this.props.onChange}
              input={input}
              InputLabelProps={{
                //Show label and placeholder at the same time
                shrink: true,
              }}
              className={classes.select}
            >
              <MenuItem value="0">
                <em>
                  <FormattedMessage
                    id="select_one"
                    defaultMessage="Select one"
                    description="Select one label"
                  />
                </em>
              </MenuItem>
              {renderIf(
                this.props.sourceLabel,
                this.props.source.map(option => (
                  <MenuItemCSS value={option[this.props.sourceValue]}>
                    {<FormattedMessage id={option[this.props.sourceLabel]} />}
                  </MenuItemCSS>
                )),
                this.props.source.map(option => (
                  <MenuItemCSS value={option[0]}>{option[1]}</MenuItemCSS>
                ))
              )}
            </Select>
          </FormControlCSS>
        </div>
        {this.InformationAvailable()}
      </div>
    );
  }
}

SelectInputW.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(SelectInputW);
