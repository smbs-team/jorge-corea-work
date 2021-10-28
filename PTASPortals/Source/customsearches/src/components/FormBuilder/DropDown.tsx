/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect } from 'react';
import { Select, InputLabel, MenuItem } from '@material-ui/core';
import { dataTypes, Field, SetField } from './FormBuilder';
import { usePrevious } from 'react-use';

interface DropDownProps {
  field: Field;
  value: dataTypes;
  setField: SetField;
}

export const DropDown = (props: DropDownProps): JSX.Element => {
  const { field, value, setField } = props;
  const previousValue = usePrevious(value);

  const callVerifyValue = (): void => {
    if (previousValue !== value) {
      setField(field.fieldName, value as string);
    }
  };

  useEffect(callVerifyValue, [value]);
  return (
    <>
      {/* <pre>{JSON.stringify(field, null, 2)}</pre> */}
      {!field.isRange && (
        <InputLabel className="" id="label-for-dd">
          {field.title}
        </InputLabel>
      )}
      <Select
        className="drop-down"
        labelId="label-for-dd"
        fullWidth
        placeholder={field.placeholder}
        title={field.title}
        value={value === null ? '' : value}
        onChange={(e): void => {
          return setField(field.fieldName, e.target.value as string);
        }}
      >
        {field.options?.map((o, i) => (
          <MenuItem key={i} value={o.value}>
            {o.title}
          </MenuItem>
        ))}
      </Select>
    </>
  );
};
