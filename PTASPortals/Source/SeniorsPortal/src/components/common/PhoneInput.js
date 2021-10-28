import React, { name, label, placeholder, onChange, readOnly } from 'react';
import './PhoneInput.css';
import { withStyles } from '@material-ui/core/styles';
import TextField from '@material-ui/core/TextField';
import PropTypes from 'prop-types';
import MaskedInput from 'react-text-mask';

const CssTextField = withStyles(theme => ({
  root: {
    '& label.Mui-focused': {
      color: '#a5c727',
    },
    '& .MuiInput-underline:after': {
      borderBottomColor: '#a5c727',
    },
    '& .MuiInput-underline:before': {
      borderBottomColor: '#000000',
    },
    '& .MuiOutlinedInput-root': {
      width: '145px',
      '& fieldset': {
        borderColor: 'red',
      },
      '&:hover fieldset': {
        borderColor: 'yellow',
      },
      '&.Mui-focused fieldset': {
        borderColor: '#a5c727',
      },
    },
    '& .MuiInputLabel-root': {
      width: '320px',
    },
    '& .MuiInputBase-root': {
      [theme.breakpoints.up('sm')]: {
        fontSize: 20,
      },
      [theme.breakpoints.down('xs')]: {
        fontSize: 16,
      },
    },
  },
}))(TextField);

function TextMaskCustom(props) {
  const { inputRef, ...other } = props;

  return (
    <MaskedInput
      {...other}
      ref={ref => {
        inputRef(ref ? ref.inputElement : null);
      }}
      mask={[
        /\d/,
        /\d/,
        /\d/,
        '-',
        /\d/,
        /\d/,
        /\d/,
        '-',
        /\d/,
        /\d/,
        /\d/,
        /\d/,
      ]}
      placeholderChar={'\u2000'}
    />
  );
}

TextMaskCustom.propTypes = {
  inputRef: PropTypes.func.isRequired,
};

export default class EmailInput extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      label: props.label,
      value: props.value,
      helperText: props.helperText,
    };
  }

  render() {
    return (
      <div className="phone-input">
        <CssTextField
          name={this.props.name}
          id="standard-dense"
          className="phone-input"
          value={this.props.value}
          label={this.props.label}
          placeholder={this.props.placeholder}
          margin="dense"
          helperText={this.props.helperText}
          onChange={this.props.onChange}
          InputLabelProps={{
            //Show label and placeholder at the same time
            shrink: true,
          }}
          InputProps={{
            inputComponent: TextMaskCustom,
            readOnly: this.props.readOnly,
          }}
        ></CssTextField>
      </div>
    );
  }
}
