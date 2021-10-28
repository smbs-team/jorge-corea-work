import React, {
  name,
  label,
  value,
  placeholder,
  helperText,
  onChange,
  readOnly,
} from 'react';
import './EmailInput.css';
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
    '& .MuiInput-formControl': {
      [theme.breakpoints.up('sm')]: {
        width: '320px',
        fontSize: '20px',
      },
      [theme.breakpoints.down('xs')]: {
        width: '220px',
        fontSize: '16px',
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
      mask={[/[0-9]/, '@', /\*/, '.', /\*/]}
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
  }
  render() {
    return (
      <div className="email-input">
        <CssTextField
          name={this.props.name}
          disabled={this.props.disabled}
          id="standard-dense"
          className="email-input"
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
            readOnly: this.props.readOnly,
          }}
        ></CssTextField>
      </div>
    );
  }
}
