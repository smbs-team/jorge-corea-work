// filename
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useEffect, useState } from 'react';
import { Field, dataTypes, SetField, FormValues } from './FormBuilder';
import { makeStyles } from '@material-ui/core';
import GridReader from './GridReader';
import {
  CustomSwitch,
  CustomTextField,
  CustomDatePicker,
} from '@ptas/react-ui-library';
import CamelToSpace from 'components/CamelToSpace';
import { CheckFieldForErrors } from './shared';
import { ReducerAction } from '.';
import { DropDown } from './DropDown';
import MultipleSelectAreas from './MultipleSelectAreas';

const FieldDisplay = ({
  field,
  value,
  setField,
  allValues,
  dispatchError,
  isLocked,
}: {
  field: Field;
  value: dataTypes;
  setField: SetField;
  allValues: FormValues;
  isLocked?: boolean;
  dispatchError?: (action: ReducerAction) => void;
}): JSX.Element => {
  const useStyles = makeStyles((theme) => ({
    formControl: {
      margin: theme.spacing(1),
      width: '100%',
      display: 'flex',
    },
    disabledLabel: {
      color: 'gray',
      fontStyle: 'italic',
    },
    displayField: {
      fontSize: '18px',
    },
  }));

  const styles = useStyles();

  const textBox = (
    field: Field,
    value: dataTypes,
    setField: SetField,
    errorMsg: string
  ): JSX.Element => {
    return (
      <Fragment>
        <CustomTextField
          fullWidth
          error={!!errorMsg}
          label={field.title}
          placeholder={field.placeholder}
          title={field.title}
          value={value}
          helperText={errorMsg}
          multiline={field.isMultiLine}
          onChange={(e: { target: { value: dataTypes } }): void =>
            setField(field.fieldName, e.target.value)
          }
        />
      </Fragment>
    );
  };

  const dateReader = (
    field: Field,
    value: dataTypes,
    setField: SetField
  ): JSX.Element => {
    const d = value as Date;
    return (
      <CustomDatePicker
        value={d}
        label={field.title}
        onChange={(e: Date): void => setField(field.fieldName, e as Date)}
      />
    );
  };

  const numberReader = (
    field: Field,
    value: dataTypes,
    setField: SetField
  ): JSX.Element => {
    return (
      // <CustomNumericField
      //   label={field.title}
      //   placeholder="-"
      //   value={value}
      //   onValueChange={(v): void =>
      //     setField(field.fieldName, v.formattedValue)
      //   }
      //   style={{ maxWidth: '20ch' }}
      // />
      <CustomTextField
        label={field.title}
        value={value}
        type="number"
        placeholder="-"
        onChange={(e: { target: { value: dataTypes } }): void =>
          setField(field.fieldName, e.target.value)
        }
        style={{ maxWidth: '20ch' }}
      />
    );
  };

  const boolField = (
    field: Field,
    value: dataTypes,
    setField: SetField
  ): JSX.Element => {
    return (
      <CustomSwitch
        label={field.title}
        isChecked={(v: dataTypes): void => setField(field.fieldName, v)}
      />
    );
  };

  const dropdown = (
    field: Field,
    value: dataTypes,
    setField: SetField
  ): JSX.Element => {
    if (field.originalParam?.isRequired && !value) {
      const [v] = field.options ?? [];
      if (v) {
        value = v.value;
      }
      //setTimeout(() => setField(field.fieldName, value), 10);
    }

    return <DropDown field={field} value={value} setField={setField} />;
  };

  const multipleSelection = (
    field: Field,
    value: dataTypes,
    setField: SetField
  ): JSX.Element => {
    if (field.originalParam?.isRequired && !value) {
      const [v] = field.options ?? [];
      value = v.value;
    }
    return (
      <MultipleSelectAreas
        field={field}
        setField={setField}
        areas={
          field.options?.map((o) => ({ Key: o.title, Value: o.value })) ?? []
        }
        value={value?.toString() ?? ''}
      />
    );
  };

  const displayField = (field: Field, value: dataTypes): JSX.Element => {
    return (
      <div className={styles.displayField}>
        <strong>{field.title}</strong>:{' '}
        {value ? (
          <span>{value}</span>
        ) : (
          <span className={styles.disabledLabel}>
            {field.placeholder || 'NA'}
          </span>
        )}
      </div>
    );
  };

  const getType = (field: Field): string => {
    if (field?.originalParam?.allowMultipleSelection) {
      return 'multipleSelection';
    }
    return field.type;
  };

  const fieldEditor = (
    field: Field,
    value: dataTypes,
    setField: SetField,
    errorMsg: string
  ): JSX.Element => {
    const processors: {
      [id: string]: (
        field: Field,
        value: dataTypes,
        setField: SetField,
        errorMsg: string,
        isLocked?: boolean
      ) => JSX.Element;
    } = {
      textbox: textBox,
      dropdown: dropdown,
      display: displayField,
      boolean: boolField,
      grid: GridReader,
      number: numberReader,
      date: dateReader,
      multipleSelection: multipleSelection,
      missing: () => {
        return <div>Unknown field type.</div>;
      },
    };
    return (processors[getType(field)] || processors.missing)(
      field,
      value,
      setField,
      errorMsg,
      isLocked
    );
  };

  const v = value;
  let f2: JSX.Element | null = null;
  if (field.isRange && field.toRangeField) {
    const fld2: Field = {
      ...field,
      fieldName: field.toRangeField || '',
      title: 'To',
    };
    f2 = (
      <Fragment>
        {fieldEditor(fld2, allValues[field.toRangeField || ''], setField, '')}
      </Fragment>
    );
  }

  const [errorMessage, setErrorMessage] = useState<string>('');

  useEffect(() => {
    const validationMessages = CheckFieldForErrors(field, v);
    const errorMessage = validationMessages.map((vm) => vm.message).join(', ');

    if (dispatchError) {
      if (errorMessage) {
        dispatchError({
          type: 'has-error',
          payload: { fieldName: field.fieldName, hasError: true },
        });
      } else {
        dispatchError({
          type: 'no-error',
          payload: { fieldName: field.fieldName },
        });
      }
    }

    setErrorMessage(errorMessage);
  }, [dispatchError, field, v]);

  return (
    <div className={field.className}>
      {/* {errorMessage} */}
      {field.type === 'grid' ? (
        fieldEditor(field, v, setField, errorMessage)
      ) : (
        <div style={{ display: 'flex' }}>
          {field.label ? (
            <div className="label">
              <CamelToSpace display={field.label} />:{' '}
            </div>
          ) : null}
          {f2 ? (
            <div className={styles.formControl}>
              <div className="ranges">
                {fieldEditor(field, v, setField, errorMessage)}
                &nbsp;
                {f2}
              </div>
            </div>
          ) : (
            <div className={styles.formControl}>
              {fieldEditor(field, v, setField, errorMessage)}
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default FieldDisplay;
