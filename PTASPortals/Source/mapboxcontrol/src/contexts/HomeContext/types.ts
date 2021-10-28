/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Dispatch, SetStateAction } from 'react';
import { ChipDataType, ColorConfiguration } from '@ptas/react-ui-library';
import {
  Folder,
  LayerSource,
  MapRenderer,
  OptionSetItemResp,
  RendererDataset,
} from 'services/map';
import {
  useDataSetPoints,
  UseDefaultMap,
  UseRendererCategories,
  UseUserMaps,
} from './hooks';
import { UseMapsTreeView } from './hooks/useMapsTreeView';
import { MapsTreeViewRow } from 'components/MapsTreeView/types';
import { RendererLabel } from 'services/map/renderer/textRenderer/textRenderersService';
import { BookmarkType } from 'components/ContentWrapper/SideInfoBar/BookmarkItem';

export interface HomeContextProps
  extends UseUserMaps,
    UseRendererCategories,
    UseDefaultMap {
  mapThumbnail: boolean;
  setMapThumbnail: React.Dispatch<React.SetStateAction<boolean>>;
  linearProgress: boolean;
  setLinearProgress: (val: boolean) => void;
  panelContent: JSX.Element | null;
  setPanelContent: Dispatch<SetStateAction<JSX.Element | null>>;
  panelHeight: number;
  setPanelHeight: Dispatch<SetStateAction<number>>;
  defaultDataSet: RendererDataset | undefined;
  userRendererFolders: Folder[];
  setUserRendererFolders: Dispatch<SetStateAction<Folder[]>>;
  mediaToken: string | undefined;
  userMapFolders: Folder[];
  setUserMapFolders: Dispatch<SetStateAction<Folder[]>>;
  currentLayer: MapRenderer | undefined;
  setCurrentLayer: Dispatch<SetStateAction<MapRenderer | undefined>>;
  b2cToken: string | undefined;
  setB2cToken: Dispatch<SetStateAction<string | undefined>>;
  openExportMapImgModal: boolean;
  setOpenExportMapImgModal: Dispatch<SetStateAction<boolean>>;
  mapImgCaptureSrc: string | undefined;
  setMapImgCaptureSrc: Dispatch<SetStateAction<string>>;
  userColorRamps: ColorConfiguration[];
  setUserColorRamps: Dispatch<SetStateAction<ColorConfiguration[]>>;
  createNewFolder: boolean;
  setCreateNewFolder: Dispatch<SetStateAction<boolean>>;
  newCategoryPopoverAnchor: HTMLElement | null;
  setNewCategoryPopoverAnchor: React.Dispatch<
    React.SetStateAction<HTMLElement | null>
  >;
  treeSelection: MapsTreeViewRow | undefined;
  setTreeSelection: React.Dispatch<
    React.SetStateAction<MapsTreeViewRow | undefined>
  >;
  saveNewMapPopoverAnchor: HTMLElement | null;
  setSaveNewMapPopoverAnchor: React.Dispatch<
    React.SetStateAction<HTMLElement | null>
  >;
  saveNewRendererPopoverAnchor: HTMLElement | null;
  setSaveNewRendererPopoverAnchor: React.Dispatch<
    React.SetStateAction<HTMLElement | null>
  >;
  isEditingUserMap: boolean;
  setIsEditingUserMap: React.Dispatch<React.SetStateAction<boolean>>;
  dataSetPoints: ReturnType<typeof useDataSetPoints>;
  bookmarkTags: ChipDataType[];
  setBookmarkTags: React.Dispatch<React.SetStateAction<ChipDataType[]>>;
  bookmarks: BookmarkType[];
  setBookmarks: React.Dispatch<React.SetStateAction<BookmarkType[]>>;
  bookmarkOptionSet: OptionSetItemResp[];
  setBookmarkOptionSet: React.Dispatch<
    React.SetStateAction<OptionSetItemResp[]>
  >;
  layerSources: LayerSource[];
  mapsTreeView: UseMapsTreeView;
  appliedRenderers: MapRenderer[];
  userMapLabels: RendererLabel[];

  savePendingChanges: boolean;
  setSavePendingChanges: React.Dispatch<React.SetStateAction<boolean>>;
}
