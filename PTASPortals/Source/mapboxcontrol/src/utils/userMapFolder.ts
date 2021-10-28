/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { MapsTreeViewRow } from 'components/MapsTreeView/types';
import { Folder } from 'services/map';

const flatten = (rootFolder?: string) => (
  prev: Folder[],
  curr: Folder
): Folder[] => {
  if (Array.isArray(curr.children) && curr.children.length) {
    curr.children.reduce(flatten(rootFolder ?? curr.folderName), prev);
  }
  curr.rootFolder = rootFolder;
  prev.push(curr);
  return prev;
};

export const flatFolders = (folders: Folder[]): Folder[] =>
  folders.reduce(flatten(), []).map((folder) => {
    return {
      ...folder,
      children: [],
    };
  });

export const createFolderRows = (folders: Folder[]): MapsTreeViewRow[] =>
  folders.map((folder) => ({
    //Add 'f' prefix in order to avoid having folders and renderers with
    // equal ID, which may cause problems on the treeview
    id: 'f' + folder.folderId,
    title: folder.folderName,
    subject: folder.subject ?? '',
    parent: folder.parentFolderId ? 'f' + folder.parentFolderId : null,
    folder: true,
    visible: true,
  }));
