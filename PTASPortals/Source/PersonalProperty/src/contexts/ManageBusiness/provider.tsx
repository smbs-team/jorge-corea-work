// provider.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { PropsWithChildren, useState } from 'react';
import { CustomFile } from '@ptas/react-public-ui-library';
import { ManageBusinessContext } from '.';
import { PersonalProperty } from 'models/personalProperty';
import { PreviewData } from './types';

const ManageBusinessProvider = ({
  children,
}: PropsWithChildren<{}>): JSX.Element => {
  const [business, setBusiness] = useState<PersonalProperty | undefined>();
  const [
    removeAlertAnchor,
    setRemoveAlertAnchor,
  ] = useState<HTMLElement | null>(null);
  const [currentFiles, setCurrentFiles] = useState<CustomFile[]>([]);
  const [sharePointFiles, setSharePointFiles] = useState<CustomFile[]>([]);
  const [savingFiles, setSavingFiles] = useState<boolean>(false);
  const [showInstruction, setShowInstruction] = useState<boolean>(false);
  const [showAttachListingInfo, setShowAttachListingInfo] = useState<boolean>(
    false
  );
  const [previewData, setPreviewData] = useState<PreviewData>({ open: false });

  return (
    <ManageBusinessContext.Provider
      value={{
        business,
        setBusiness,
        removeAlertAnchor,
        setRemoveAlertAnchor,
        currentFiles,
        setCurrentFiles,
        sharePointFiles,
        setSharePointFiles,
        savingFiles,
        setSavingFiles,
        showInstruction,
        setShowInstruction,
        showAttachListingInfo,
        setShowAttachListingInfo,
        previewData,
        setPreviewData,
      }}
    >
      {children}
    </ManageBusinessContext.Provider>
  );
};

export default ManageBusinessProvider;
