// types.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Dispatch, SetStateAction } from 'react';
import { CustomFile } from '@ptas/react-public-ui-library';
import { PersonalProperty } from 'models/personalProperty';

export interface ManageBusinessContextProps {
  business: PersonalProperty | undefined;
  setBusiness: React.Dispatch<
    React.SetStateAction<PersonalProperty | undefined>
  >;
  showAttachListingInfo: boolean;
  setShowAttachListingInfo: React.Dispatch<React.SetStateAction<boolean>>;
  removeAlertAnchor: HTMLElement | null;
  setRemoveAlertAnchor: Dispatch<SetStateAction<HTMLElement | null>>;
  currentFiles: CustomFile[];
  setCurrentFiles: Dispatch<SetStateAction<CustomFile[]>>;
  sharePointFiles: CustomFile[];
  setSharePointFiles: Dispatch<SetStateAction<CustomFile[]>>;
  savingFiles: boolean;
  setSavingFiles: Dispatch<SetStateAction<boolean>>;
  showInstruction: boolean;
  setShowInstruction: React.Dispatch<React.SetStateAction<boolean>>;
  previewData: PreviewData;
  setPreviewData: React.Dispatch<React.SetStateAction<PreviewData>>;
}

export interface PreviewData {
  open: boolean;
  file?: CustomFile;
}
