/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { SideTreeRow } from '@ptas/react-ui-library';
import { uniqBy } from 'lodash';
import { RendererLabel } from 'services/map/renderer/textRenderer/textRenderersService';

export const groupRows = (
  labels: RendererLabel[],
  rootLayerRow: SideTreeRow
): SideTreeRow[] => {
  const groups = uniqBy(
    labels.filter((label) => label.labelConfig.group?.length),
    (item) => item.labelConfig.group
  );
  return groups.map((group) => {
    const allLabelsChecked = !!labels
      .filter((item) => item.labelConfig.group === group.labelConfig.group)
      .every(
        (item) =>
          item.layer.layout?.visibility === 'visible' &&
          item.labelConfig.group === group.labelConfig.group
      );
    return {
      id: group.labelConfig.group + '-' + group.labelConfig.refLayerId,
      name: group.labelConfig.group ?? '',
      parentId: rootLayerRow.id,
      isCheckable: true,
      isChecked: allLabelsChecked,
      isSelected: true,
      disableCheckBox: rootLayerRow.disableCheckBox || !rootLayerRow.isChecked,
      isGroup: true,
    };
  });
};

export const labelRows = (
  labels: RendererLabel[],
  rootLayerRow: SideTreeRow
): SideTreeRow[] => {
  return labels.map((label, i) => ({
    id: label.layer.id,
    isCheckable: true,
    isChecked: label.layer.layout?.visibility === 'visible',
    isSelected: true,
    name: label.labelConfig?.labelName ?? 'Unnamed label ' + i + 1,
    parentId: label.labelConfig.group?.length
      ? label.labelConfig.group + '-' + label.labelConfig.refLayerId
      : rootLayerRow.id,
    disableCheckBox: rootLayerRow.disableCheckBox || !rootLayerRow.isChecked,
  }));
};
