// PreviousYear.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  CommAgGridElement,
  CommercialAgGrid,
  CustomRowData,
  InvalidRow,
} from '@ptas/react-ui-library';
import { GridSection } from 'components/shared';
import { Box, makeStyles } from '@material-ui/core';
import { ColDef, ColGroupDef, GridOptions } from 'ag-grid-community';
import usePageManagerStore from 'stores/usePageManagerStore';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import InsertDriveFileIcon from '@material-ui/icons/InsertDriveFile';
import { useRef, useState } from 'react';
import CreateIcon from '@material-ui/icons/Create';
import { omit } from 'lodash';
import { useEffect } from 'react';
import useAreaTreeStore from 'stores/useAreaTreeStore';
import ClearIcon from '@material-ui/icons/Clear';
import CheckCircleIcon from '@material-ui/icons/CheckCircle';
import CancelIcon from '@material-ui/icons/Cancel';

interface Options<T> {
  title?: string;
  rowData?: T[];
  columnDefs?: (ColDef | ColGroupDef)[];
  width?: string;
  gridOptions?: Omit<GridOptions, 'rowData' | 'columnDefs'>;
  onSave?: () => void;
  onChange?: (rows: CustomRowData<T>[]) => void;
  onReset?: () => void;
  isDirty?: boolean;
  errors?: InvalidRow[];
}

interface Props<T, K> {
  topOptions?: Options<T>;
  bottomOptions?: Options<K>;
}

const useStyles = makeStyles({
  sectionWrapper: {
    width: '100%',
  },
  previousYear: {
    border: '1px solid lightgray',
    display: 'flex',
    flexDirection: 'column',
    padding: 22,
    alignItems: 'center',
    width: 'fit-content',
  },
  previousYearTitle: {
    fontSize: '1.5rem',
    fontWeight: 'bold',
  },
  sectionTitle: {
    color: '#72bb53',
  },
});

function PreviousYear<T, K>(props: Props<T, K>): JSX.Element {
  const classes = useStyles();
  const pageManagerStore = usePageManagerStore();
  const areaTreeStore = useAreaTreeStore();
  const topGridEle = useRef<CommAgGridElement<T>>(null);
  const bottomGridEle = useRef<CommAgGridElement<K>>(null);
  const [isEditableTop, setIsEditableTop] = useState<boolean>(false);
  const [isEditableBottom, setIsEditableBottom] = useState<boolean>(false);

  useEffect(() => {
    resetGrid();
  }, [
    pageManagerStore.pages,
    pageManagerStore.selectedYear,
    areaTreeStore.selectedItem,
  ]);

  const resetGrid = () => {
    setIsEditableTop(false);
    setIsEditableBottom(false);
  };

  const onTopSave = () => {
    props?.topOptions?.onSave?.();
  };

  const onBottomSave = () => {
    props?.bottomOptions?.onSave?.();
  };

  return (
    <div className={classes.previousYear}>
      <div className={classes.previousYearTitle}>{`Previous Year ${
        parseInt(pageManagerStore.selectedYear?.label ?? '0') - 1
      }`}</div>
      <GridSection
        title={props.topOptions?.title}
        classes={{
          sectionWrapper: classes.sectionWrapper,
          title: classes.sectionTitle,
        }}
        icons={
          !isEditableTop
            ? [
                {
                  icon: <CreateIcon />,
                  text: 'Edit',
                  onClick: () => setIsEditableTop(true),
                  disabled: !pageManagerStore.selectedYear?.isCurrent
                },
              ]
            : [
                {
                  icon: <InsertDriveFileIcon />,
                  text: 'Save',
                  onClick: onTopSave,
                },
                {
                  icon: <AddCircleOutlineIcon />,
                  text: 'Add Row',
                  onClick: () => {
                    topGridEle.current?.addRow({} as T);
                  },
                },
                { icon: <ClearIcon />, text: 'Cancel', onClick: resetGrid },
              ]
        }
        miscContent={
          props.topOptions?.isDirty ? (
            !props.topOptions.errors?.length ? (
              <CheckCircleIcon style={{ color: 'green', marginLeft: 8 }} />
            ) : (
              <CancelIcon style={{ color: 'red', marginLeft: 8 }} />
            )
          ) : undefined
        }
      >
        <Box width={props.topOptions?.width ?? 870}>
          <CommercialAgGrid
            showRowNumber
            ref={topGridEle}
            rowData={props.topOptions?.rowData ?? []}
            columnDefs={props.topOptions?.columnDefs ?? []}
            rowNumberOptions={{ hide: false, rowDrag: isEditableTop }}
            onChange={props.topOptions?.onChange}
            gridOptions={{
              defaultColDef: {
                sortable: false,
                editable: isEditableTop,
                ...props.topOptions?.gridOptions?.defaultColDef,
              },
              ...omit(props.topOptions?.gridOptions, 'defaultColDef'),
            }}
            showRemove={isEditableTop}
            errors={props.topOptions?.errors}
          />
        </Box>
      </GridSection>
      <GridSection
        title={props.bottomOptions?.title}
        classes={{
          sectionWrapper: classes.sectionWrapper,
          title: classes.sectionTitle,
        }}
        icons={
          !isEditableBottom
            ? []
            : [
                {
                  icon: <InsertDriveFileIcon />,
                  text: 'Save',
                  onClick: onBottomSave,
                },
                {
                  icon: <AddCircleOutlineIcon />,
                  text: 'Add Row',
                  onClick: () => {
                    bottomGridEle.current?.addRow({} as K);
                  },
                },
                { icon: <ClearIcon />, text: 'Cancel', onClick: resetGrid },
              ]
        }
        miscContent={
          props.bottomOptions?.isDirty ? (
            !props.bottomOptions.errors?.length ? (
              <CheckCircleIcon style={{ color: 'green', marginLeft: 8 }} />
            ) : (
              <CancelIcon style={{ color: 'red', marginLeft: 8 }} />
            )
          ) : undefined
        }
      >
        <Box width={props.bottomOptions?.width ?? 870}>
          <CommercialAgGrid
            showRowNumber
            rowNumberOptions={{ hide: false, rowDrag: isEditableBottom }}
            ref={bottomGridEle}
            rowData={props.bottomOptions?.rowData ?? []}
            columnDefs={props.bottomOptions?.columnDefs ?? []}
            onChange={props.bottomOptions?.onChange}
            gridOptions={{
              defaultColDef: {
                sortable: false,
                editable: isEditableBottom,
                ...props.bottomOptions?.gridOptions?.defaultColDef,
              },
              ...omit(props.bottomOptions?.gridOptions, 'defaultColDef'),
            }}
            showRemove={isEditableBottom}
            errors={props.bottomOptions?.errors}
          />
        </Box>
      </GridSection>
    </div>
  );
}

export default PreviousYear;
