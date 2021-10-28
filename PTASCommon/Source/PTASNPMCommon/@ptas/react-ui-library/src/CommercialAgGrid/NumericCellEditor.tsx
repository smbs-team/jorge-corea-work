// NumericEditor.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ICellEditor, ICellEditorParams } from "ag-grid-community";
import React from "react";
import {
  forwardRef,
  useEffect,
  useRef,
  useState,
  useImperativeHandle
} from "react";
import NumberFormat, { NumberFormatProps } from "react-number-format";
import { omit } from "lodash";

interface NumericEditorProps extends CombinedProps {
  roundBy?: number;
}

type CombinedProps = Omit<NumberFormatProps, "value"> & ICellEditorParams;

const NumericCellEditor = forwardRef<ICellEditor, NumericEditorProps>(
  (props: NumericEditorProps, ref): JSX.Element => {
    const [value, setValue] = useState<string>(props.value);
    const refInput = useRef<HTMLInputElement | null>(null);

    useEffect(() => {
      if (!refInput.current) return;
      const input = refInput.current;
      setTimeout(() => {
        input.focus();
        input.select();
      });
    }, []);

    useImperativeHandle(ref, () => {
      return {
        // the final value to send to the grid, on completion of editing
        getValue() {
          if (props.roundBy) {    
            return parseFloat(
              (
                Math.round(parseFloat(value) / props.roundBy) * props.roundBy
              ).toFixed(2)
            ).toString();
          } 
          
          return value;     
        },

        // Gets called once before editing starts, to give editor a chance to
        // cancel the editing before it even starts.
        isCancelBeforeStart() {
          return false;
        },

        // Gets called once when editing is finished (eg if Enter is pressed).
        // If you return true, then the result of the edit will be ignored.
        isCancelAfterEnd() {
          return false;
        }
      };
    });

    return (
      <NumberFormat
        className='ag-input-field-input ag-text-field-input number-format'
        getInputRef={refInput}
        value={value}
        onValueChange={(values) => {
          setValue(values.formattedValue);
        }}
        {...omit(props, "value")}
      />
    );
  }
);

export default NumericCellEditor;
