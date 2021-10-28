/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useEffect, useState } from 'react';
import { makeStyles } from '@material-ui/core';
import { ModalGrid as PtasModalGrid } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import { LayerSource } from 'services/map';

interface Props {
  isOpen: boolean;
  onButtonClick: (selectedRows: LayerRowData[]) => void;
  onClose: () => void;
}

export interface LayerRowData
  extends Pick<
    LayerSource,
    'nativeMapboxLayers' | 'defaultMapboxLayer' | 'layerSourceId'
  > {
  org: string;
  alias: string;
  name: string;
  abstract: string;
  dataType: string;
  subject: string;
  isSelected: boolean;
}

const useStyles = makeStyles(() => ({
  searchField: {
    width: 415,
  },
}));

/**
 * ModalGrid
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ModalGrid(props: Props): JSX.Element {
  const classes = useStyles();
  const {
    selectedUserMap,
    editingUserMap,
    isEditingUserMap,
    layerSources,
  } = useContext(HomeContext);
  const [rowData, setRowData] = useState<LayerRowData[]>([]);

  useEffect(() => {
    const currentLayersSources: number[] = [];
    if (isEditingUserMap) {
      editingUserMap?.mapRenderers.forEach((r) =>
        currentLayersSources.push(r.layerSourceId)
      );
    } else {
      selectedUserMap?.mapRenderers.forEach((r) =>
        currentLayersSources.push(r.layerSourceId)
      );
    }

    setRowData(
      layerSources.map((source) => ({
        org: source.organization,
        alias: source.layerSourceAlias,
        name: source.layerSourceName,
        abstract: source.metadata?.abstract ?? source.description,
        dataType: source.metadata?.dataType ?? source.layerSourceType,
        subject: source.metadata?.subject ?? '',
        isSelected: currentLayersSources.includes(source.layerSourceId),
        layerSourceId: source.layerSourceId,
        defaultMapboxLayer: source.defaultMapboxLayer,
        nativeMapboxLayers: source.nativeMapboxLayers,
      })) ?? []
    );
  }, [selectedUserMap, isEditingUserMap, editingUserMap, layerSources]);

  const gridOptions = {
    defaultColDef: {
      sortable: true,
      resizable: true,
      suppressSizeToFit: true,
    },
    columnDefs: [
      { field: 'select', checkboxSelection: true },
      { field: 'org' },
      { field: 'alias' },
      { field: 'name' },
      { field: 'abstract', suppressSizeToFit: false },
      { field: 'dataType' },
      { field: 'subject' },
    ],
    rowMultiSelectWithClick: true,
  };

  return (
    <PtasModalGrid<LayerRowData>
      rows={rowData}
      isOpen={props.isOpen}
      onClose={props.onClose}
      onButtonClick={(rows): void => {
        props.onButtonClick(rows);
        props.onClose();
      }}
      classes={{ textField: classes.searchField }}
      TextFieldProps={{
        placeholder: 'Find org, alias, name, abstract, data type or subject',
      }}
      gridOptions={gridOptions}
    />
  );
}

export default ModalGrid;
