// DataSetFieldDropDown.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useEffect, useContext } from 'react';
import { SimpleDropDown, DropDownItem } from '@ptas/react-ui-library';
import { RuleContext } from '../Context';
import { createStyles, makeStyles, useTheme } from '@material-ui/core';
import { HomeContext } from 'contexts';
import { AppLayers } from 'appConstants';
import {
  RendererClassBreakRule,
  RendererSimpleRule,
  RendererUniqueValuesRule,
} from 'services/map/model';
import { useDataSetColumns } from './useDataSetColumns';

const useStyles = makeStyles(() =>
  createStyles({
    root: {
      width: '100%',
    },
  })
);

function DataSetFieldDropDown(): JSX.Element {
  const ruleContext = useContext(RuleContext);
  const {
    dataSetColumns,
    setDatasetField,
    datasetField,
    embeddedDataFields,
    embeddedDataField,
    setEmbeddedDataField,
    setRule,
  } = ruleContext;
  const { currentLayer } = useContext(HomeContext);
  const { dataSetColumnsDropDownItems } = useDataSetColumns();
  const [fieldToRenderValue, setFieldToRenderValue] = useState('');
  const classes = useStyles(useTheme());

  useEffect(() => {
    if (currentLayer?.rendererRules.layer.id === AppLayers.PARCEL_LAYER) {
      if (datasetField?.columnName) {
        if (
          dataSetColumnsDropDownItems.find(
            (item) => item.value === datasetField?.columnName
          )
        ) {
          setFieldToRenderValue(datasetField.columnName);
        }
      } else {
        if (dataSetColumnsDropDownItems.length) {
          setFieldToRenderValue(
            dataSetColumnsDropDownItems[0].value.toString()
          );
        }
      }
    }
  }, [datasetField, dataSetColumnsDropDownItems, currentLayer]);

  useEffect(() => {
    if (!(currentLayer?.rendererRules.layer.id === AppLayers.PARCEL_LAYER)) {
      if (embeddedDataField?.FieldName) {
        if (
          dataSetColumnsDropDownItems.find(
            (item) => item.value === embeddedDataField.FieldName
          )
        ) {
          setFieldToRenderValue(embeddedDataField.FieldName);
        }
      } else {
        if (dataSetColumnsDropDownItems.length) {
          setFieldToRenderValue(
            dataSetColumnsDropDownItems[0].value.toString()
          );
        }
      }
    }
  }, [embeddedDataField, dataSetColumnsDropDownItems, currentLayer]);

  const onSelected = (s: DropDownItem): void => {
    if (!currentLayer) return;
    if (!currentLayer.rendererRules.colorRule?.rule) return;
    if (currentLayer.rendererRules.layer.id === AppLayers.PARCEL_LAYER) {
      //Parcel layers
      if (!dataSetColumns) return;
      const newDataSetField = dataSetColumns.find(
        (column) => column.columnName === s.value
      );
      setRule((prevRule) => {
        if (newDataSetField) {
          if (prevRule instanceof RendererSimpleRule) {
            return new RendererSimpleRule({
              ...prevRule,
              columnName: s.value as string,
              columnType: newDataSetField.columnType,
              datasetId: newDataSetField.datasetId,
            });
          } else if (prevRule instanceof RendererUniqueValuesRule) {
            return new RendererUniqueValuesRule({
              ...prevRule,
              columnName: s.value as string,
              columnType: newDataSetField.columnType,
              datasetId: newDataSetField.datasetId,
            });
          } else if (prevRule instanceof RendererClassBreakRule) {
            return new RendererClassBreakRule({
              ...prevRule,
              columnName: s.value as string,
              columnType: newDataSetField.columnType,
              datasetId: newDataSetField.datasetId,
            });
          }
        }
        return;
      });
      setDatasetField(newDataSetField);
    } else {
      if (!embeddedDataFields) return;
      const newDataSetField = embeddedDataFields.find(
        (column) => column.FieldName === s.value
      );
      const ds = dataSetColumns.find((column) => column.columnName === s.value)
        ?.datasetId;

      if (ds) {
        setRule((prevRule) => {
          if (!newDataSetField) return;
          if (prevRule instanceof RendererSimpleRule) {
            return new RendererSimpleRule({
              ...prevRule,
              columnName: s.value as string,
              columnType: newDataSetField.FieldType,
              datasetId: ds,
            });
          } else if (prevRule instanceof RendererUniqueValuesRule) {
            return new RendererUniqueValuesRule({
              ...prevRule,
              columnName: s.value as string,
              columnType: newDataSetField?.FieldType ?? 'unknown',
              datasetId: ds,
            });
          } else if (prevRule instanceof RendererClassBreakRule) {
            return new RendererClassBreakRule({
              ...prevRule,
              columnName: s.value as string,
              columnType: newDataSetField?.FieldType ?? 'unknown',
              datasetId: ds,
            });
          }
          return;
        });
      }
      setEmbeddedDataField(newDataSetField);
    }
  };

  return (
    <SimpleDropDown
      classes={{ root: classes.root }}
      items={dataSetColumnsDropDownItems}
      onSelected={onSelected}
      label="Field to Render"
      titleTop
      value={fieldToRenderValue}
    />
  );
}

export default DataSetFieldDropDown;
