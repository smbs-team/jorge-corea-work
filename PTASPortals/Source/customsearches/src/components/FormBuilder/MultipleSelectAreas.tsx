// SalesData.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useCallback, useEffect } from 'react';
import { Box } from '@material-ui/core';
import {
  ChipContainer,
  ChipItem,
  CustomPopover,
  ListSearch,
  CustomIconButton,
} from '@ptas/react-ui-library';
import AddCircleOutlineIcon from '@material-ui/icons/AddCircleOutline';
import { useStyles } from './styles';
import { Field, SetField } from './FormBuilder';

interface SearchArea {
  Key: string;
  Value: string;
}

interface Props {
  selectedItems?: string[];
  areas: SearchArea[];
  setField: SetField;
  field: Field;
  value: string;
}

function SalesData(props: Props): JSX.Element {
  const classes = useStyles();
  const [event, setEvent] = useState<HTMLElement | null>(null);
  const [chipItems, setChipItems] = useState<ChipItem[]>();
  const [selectedItems, setSelectedItems] = useState<string[]>([]);

  const cleanState = useCallback((): void => {
    setEvent(null);
  }, []);

  useEffect(() => {
    if (!props.value) return;
    setSelectedItems(props.value.split(','));
    //eslint-disable-next-line
  }, []);

  const paintChipItem = (): void => {
    const selectedAreas = selectedItems.map((v, i) => {
      const text = props.areas?.find((a) => a.Value === v);
      return { key: i, label: text?.Key ?? text?.Value, isDeletable: true } as ChipItem;
    });

    setChipItems(selectedAreas);
    props.setField?.(props.field?.fieldName, selectedItems.join(";"));
  };

  const onSelectedItem = (value: string, isSelected: boolean): void => {
    if(!isSelected){
      const values = selectedItems.filter(s=> !value.includes(s));
      setSelectedItems([...values]);
      return;
    }
    let values = [...selectedItems, value];
    if (selectedItems.some((s) => s === value)) {
      values = selectedItems.filter((s) => s !== value);
    }
    setSelectedItems([...values]);
  };

  useEffect(paintChipItem, [selectedItems]);

  const List = (data: SearchArea[] | undefined): JSX.Element => {
    if (!data || data.length < 1) return MessageBox('The search has no areas.');
    return (
      <ListSearch
        data={data}
        onClick={(selectedItem: React.Key, isSelected: boolean): void => {
          onSelectedItem(selectedItem.toString(), isSelected);
        }}
        selectedData={selectedItems}
        textFieldLabel="Find section name, page or code"
      />
    );
  };

  const MessageBox = (message: string): JSX.Element => {
    return <Box className={classes.noDataBox}>{message}</Box>;
  };

  const getList = (): JSX.Element => {
    return List(props.areas);
  };
  return (
    <Box className={classes.root}>
      <Box className={classes.salesDataBottomRow}>
        <CustomIconButton
          icon={<AddCircleOutlineIcon />}
          text={`Add ${props.field.fieldName}`}
          onClick={(e): void => setEvent(e.currentTarget)}
        />
        <CustomPopover
          anchorEl={event}
          onClose={(): void => {
            cleanState();
          }}
          anchorOrigin={{
            vertical: 'center',
            horizontal: 'right',
          }}
          transformOrigin={{
            vertical: 'top',
            horizontal: 'left',
          }}
          classes={{ paper: classes.popoverPaper }}
        >
          {getList()}
        </CustomPopover>
      </Box>
      {chipItems && chipItems?.length > 0 ? (
        <ChipContainer
          classes={{ root: classes.chipContainer }}
          chipData={chipItems ?? []}
          onChange={(c): void => onSelectedItem(c.label, false)}
        />
      ) : (
        <div className={classes.emptyChipContainer}></div>
      )}
    </Box>
  );
}

export default SalesData;
