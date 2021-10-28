/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { MapsTreeViewRow } from 'components/MapsTreeView/types';
import { PanelProps } from 'components/common/panel';

export type MapsLayersManageGridCol = {
  name:
    | 'layerSourceId'
    | 'org'
    | 'alias'
    | 'jurisdiction'
    | 'description'
    | 'layerId';
  title: string;
};

export type MapsLayersManageProps = PanelProps & {
  row?: MapsTreeViewRow;
  isSystemUserMap?: boolean;
  duplicate?: boolean;
};
