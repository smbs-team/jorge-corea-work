// GenericCommInc.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  CommAgGridElement,
  CommercialAgGrid,
  CommercialAgGridProps,
  CustomRowData,
  IconToolBarItem,
  InvalidRow,
  OnChangeEventTypes,
} from '@ptas/react-ui-library';
import { GridSection } from 'components/shared';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import CachedIcon from '@material-ui/icons/Cached';
import InsertDriveFileIcon from '@material-ui/icons/InsertDriveFile';
import { Fragment, useRef } from 'react';
import CheckCircleIcon from '@material-ui/icons/CheckCircle';
import CancelIcon from '@material-ui/icons/Cancel';
import { getCellStyle } from 'components/shared/gridUtility';
import BuildIcon from '@material-ui/icons/Build';
import { omit } from 'lodash';

interface Props<T> {
  GridOptions?: CommercialAgGridProps<T>;
  title?: string;
  errors?: InvalidRow[];
  isDirty?: boolean;
  isAdjusted?: boolean;
  onChange?: (rows: CustomRowData<T>[], type: OnChangeEventTypes) => void;
  onSave?: () => void;
  onReset?: () => void;
  iconOptions?: IconOptions;
}

interface IconOptions {
  saveText?: string;
  addRowText?: string;
  resetText?: string;
  extraIcons?: IconToolBarItem[];
  hideIcons?: React.ReactText[];
  hideAll?: boolean;
}

function GenericSectionGrid<T>(props: Props<T>): JSX.Element {
  const gridEle = useRef<CommAgGridElement<T>>(null);

  return (
    <GridSection
      title={props.title}
      icons={
        props.iconOptions?.hideAll
          ? []
          : [
              {
                id: 'save',
                icon: <InsertDriveFileIcon />,
                text: props.iconOptions?.saveText ?? 'Save',
                onClick: props.onSave,
              },
              {
                id: 'addRow',
                icon: <AddCircleOutlineIcon />,
                text: props.iconOptions?.addRowText ?? 'Add Row',
                onClick: () => {
                  gridEle.current?.addRow({} as T);
                },
              },
              {
                id: 'reset',
                icon: <CachedIcon />,
                text: props.iconOptions?.resetText ?? 'Reset',
                onClick: props.onReset,
              },
              ...(props.iconOptions?.extraIcons ?? []),
            ].filter(
              (icon) => !props.iconOptions?.hideIcons?.includes(icon.id ?? '')
            )
      }
      miscContent={
        <Fragment>
          {props.isDirty ? (
            props.errors && !props.errors.length ? (
              <CheckCircleIcon style={{ color: 'green', marginLeft: 8 }} />
            ) : (
              <CancelIcon style={{ color: 'red', marginLeft: 8 }} />
            )
          ) : undefined}
          {props.isAdjusted && <BuildIcon style={{ marginLeft: 8 }}/>}
        </Fragment>
      }
    >
      <CommercialAgGrid<T>
        showRemove
        showRowNumber
        ref={gridEle}
        gridOptions={{
          defaultColDef: {
            editable: true,
            cellStyle: getCellStyle,
            ...props.GridOptions?.gridOptions?.defaultColDef,
          },
          ...omit(props.GridOptions?.gridOptions, 'defaultColDef'),
        }}
        errors={props.errors ?? []}
        onChange={props.onChange}
        {...omit(props.GridOptions, 'gridOptions')}
      />
    </GridSection>
  );
}

export default GenericSectionGrid;
