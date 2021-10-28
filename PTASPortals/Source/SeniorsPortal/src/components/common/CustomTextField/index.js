import { TextField } from '@material-ui/core';
import React, { useEffect, useRef, useState } from 'react';
import PropTypes from 'prop-types';
import { debounce } from 'lodash';

const CustomTextField = props => {
  const [localValue, setLocalValue] = useState(props.value);
  const mountedRef = useRef(false);
  const eventRef = useRef();
  const onChangeRef = useRef();
  const {
    value,
    name,
    type,
    regExpr,
    onChange,
    changeDelay,
    label,
    fullWidth,
    ...restProps
  } = props;
  const numberRegex = new RegExp('^[0-9]*[0-9][0-9]*$', 'g');

  useEffect(() => {
    if (mountedRef.current && value !== localValue) {
      setLocalValue(value);
    } else if (!mountedRef.current) {
      mountedRef.current = true;
    }
  }, [value]);

  useEffect(() => {
    if (!mountedRef.current) return;
    clearTimeout(onChangeRef.current);
    onChangeRef.current = setTimeout(() => {
      onChange({ value: localValue, name }, eventRef.current);
    }, changeDelay || 0);
  }, [localValue]);

  const handleChange = e => {
    const { value: v } = e.currentTarget;
    eventRef.current = e;
    if (v === '') {
      setLocalValue(v);
    } else if (regExpr) {
      regExpr.test(v) && setLocalValue(v);
    } else if (!regExpr) {
      if (type === 'number') {
        const isValid = numberRegex.test(v);
        isValid && setLocalValue(v);
      } else {
        setLocalValue(v);
      }
    }
  };

  return (
    <TextField
      {...restProps}
      fullWidth={fullWidth}
      name={name}
      type={type}
      value={localValue}
      label={label}
      onChange={handleChange}
    />
  );
};

CustomTextField.propTypes = {
  value: PropTypes.string,
  label: PropTypes.string,
  name: PropTypes.string,
  type: PropTypes.string,
  regExpr: PropTypes.string,
  onChange: PropTypes.func,
  changeDelay: PropTypes.number,
  fullWidth: PropTypes.bool,
};

CustomTextField.defaultProps = {
  name: '',
  onChange: () => {},
  changeDelay: 0,
  fullWidth: false,
};

export default CustomTextField;
