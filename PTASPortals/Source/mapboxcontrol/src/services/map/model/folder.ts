// folder.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export type FolderItemType = 'UserMap' | 'MapRenderer';

export class Folder {
  folderId: number;
  parentFolderId: number | null;
  rootFolder?: string;
  folderName: string;
  children: Folder[] | null;
  subject?: string;

  constructor(fields: Folder) {
    this.folderId = fields.folderId;
    this.parentFolderId = fields.parentFolderId;
    this.folderName = fields.folderName;
    this.children = fields.children;
  }
}

export interface CreateFolderReq {
  FolderPath: string;
  FolderItemType: FolderItemType;
  UserId: string;
}
