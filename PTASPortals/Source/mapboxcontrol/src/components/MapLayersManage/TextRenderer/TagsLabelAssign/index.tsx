/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from 'react';
import { createStyles, makeStyles, Theme } from '@material-ui/core';
import { DragDropItem, DragDropList } from '@ptas/react-ui-library';
import { TagsManageContext } from '../TagsManageContext';
import { useUpdateEffect } from 'react-use';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    dragDropListRoot: {
      maxHeight: '100%',
      padding: 0,
      overflow: 'visible',
    },
  })
);

const TagLabelsAssign = (): JSX.Element => {
  const classes = useStyles();
  const { labelConfigs, setLabelConfigs, setSelectedLabel } = useContext(
    TagsManageContext
  );
  const [selectedLabelId, setSelectedLabelId] = useState<string>();
  const [dragDropLabels, setDragDropLabels] = useState<DragDropItem[]>([]);

  useUpdateEffect(() => {
    setDragDropLabels(() => {
      const next = (labelConfigs ?? []).map((l) => ({
        id: l.id,
        content: l.labelName ?? '',
      }));
      return next;
    });
  }, [JSON.stringify(labelConfigs)]);

  useEffect(() => {
    setSelectedLabel((prev) => {
      if (!prev) return;
      const found = dragDropLabels.find((dndItem) => dndItem.id === prev.id);
      if (!found) return;
      if (found.content === prev.labelName) return prev;
      return {
        ...prev,
        labelName: found.content,
      };
    });
  }, [dragDropLabels, setSelectedLabel]);

  const onItemsChange = useCallback(
    (dragDropItems: DragDropItem[]): void => {
      const groupedDragDropItems = dragDropItems.reduce<
        Record<string, DragDropItem>
      >(
        (prev, curr) => ({
          ...prev,
          [curr.id]: curr,
        }),
        {}
      );
      setLabelConfigs(
        (prev) =>
          prev
            .filter((_label) => !!groupedDragDropItems[_label.id])
            ?.map((_label) => {
              const labelName =
                groupedDragDropItems[_label.id]?.content ?? _label.labelName;
              return {
                ..._label,
                labelName,
              };
            }) ?? []
      );
    },
    [setLabelConfigs]
  );

  useUpdateEffect(() => {
    setSelectedLabel(
      labelConfigs?.find((_label) => _label.id === selectedLabelId)
    );
  }, [selectedLabelId]);

  return useMemo(
    () => (
      <DragDropList
        classes={{ root: classes.dragDropListRoot }}
        data={dragDropLabels}
        onSelectionChange={(item): void => setSelectedLabelId(item?.id)}
        onItemsChange={onItemsChange}
        useContextMenu={true}
      />
    ),
    [classes.dragDropListRoot, dragDropLabels, onItemsChange]
  );
};

export default TagLabelsAssign;
