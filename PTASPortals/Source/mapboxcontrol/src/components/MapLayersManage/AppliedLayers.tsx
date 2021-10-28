// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useCallback, useContext, useEffect, useState } from 'react';
import { makeStyles } from '@material-ui/core';
import clsx from 'clsx';
import { DragDropItem, DragDropList } from '@ptas/react-ui-library';
import { layerService, UserMap } from 'services/map';
import { HomeContext } from 'contexts';
import { MapLayersManageContext } from './Context';
import { MapRenderer } from 'services/map/model/MapRenderer';

type Props = {
  shrink?: boolean;
};

const useStyles = makeStyles(() => ({
  root: {
    padding: 0,
    width: 'fit-content',
    paddingRight: 24,
    paddingTop: 8,
    overflowY: 'auto',
    overflowX: 'hidden',
    height: 400,
  },
  shrink: {
    height: '273px !important',
  },
}));

const AppliedLayers = (props: Props): JSX.Element => {
  const classes = useStyles();
  const {
    currentLayer,
    setCurrentLayer,
    editingUserMap,
    setEditingUserMap,
  } = useContext(HomeContext);
  const { setBottomSection } = useContext(MapLayersManageContext);
  const [rows, setRows] = useState<DragDropItem[]>([]);

  useEffect(() => {
    const newRows =
      editingUserMap?.mapRenderers.map((l) => ({
        id: l.rendererRules.layer.id,
        content:
          layerService.layersConfiguration[l.rendererRules.layer.id]
            .layerSourceName,
      })) ?? [];
    setRows(newRows);
  }, [editingUserMap]);

  const onSelectionChange = useCallback(
    (itemSelected?: DragDropItem): void => {
      if (itemSelected?.id === currentLayer?.rendererRules.layer.id) return;
      if (itemSelected) {
        const newRenderer = editingUserMap?.mapRenderers.find(
          (renderer) => renderer.rendererRules.layer.id === itemSelected?.id
        );
        setCurrentLayer(newRenderer);
      } else {
        setBottomSection(undefined);
      }
    },
    [currentLayer, editingUserMap, setBottomSection, setCurrentLayer]
  );

  const onItemsChange = useCallback(
    (dragDropItems: DragDropItem[]): void => {
      setEditingUserMap((prevState) => {
        if (!prevState) return;
        if (
          dragDropItems.every(
            (l) => l.id !== currentLayer?.rendererRules.layer.id
          )
        ) {
          setCurrentLayer(undefined);
          setBottomSection(undefined);
        }

        const newRenderers: MapRenderer[] = [];
        dragDropItems.forEach((item) => {
          const rend = prevState.mapRenderers.find(
            (r) => r.rendererRules.layer.id === item.id
          );
          if (rend) newRenderers.push(rend);
        });
        if (dragDropItems.length !== newRenderers.length) return prevState;
        const newMap: UserMap = {
          ...prevState,
          mapRenderers: newRenderers,
        };

        return newMap;
      });
    },
    [currentLayer, setBottomSection, setCurrentLayer, setEditingUserMap]
  );

  return (
    <DragDropList
      data={rows}
      classes={{
        root: props.shrink ? clsx(classes.root, classes.shrink) : classes.root,
      }}
      onItemsChange={onItemsChange}
      onSelectionChange={onSelectionChange}
      selected={rows.find(
        (item) => item.id === currentLayer?.rendererRules.layer.id
      )}
    />
  );
};

export default AppliedLayers;
