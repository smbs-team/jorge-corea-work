// PreviewFile.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useState } from 'react';
import { makeStyles, Theme, Box } from '@material-ui/core';
import {
  OfficeFilePreview,
  PdfFilePreview,
  CustomFile,
  CustomPopover,
} from '@ptas/react-public-ui-library';
import clsx from 'clsx';
import { useUpdateEffect } from 'react-use';

interface Props {
  file?: CustomFile;
  open?: boolean;
  onClose?: () => void;
}

const useStyles = makeStyles((theme: Theme) => ({
  popover: {
    height: '100%',
    borderRadius: '24px 24px 0 0',
    overflow: 'hidden',
    maxHeight: '100%',
    width: '100%',
    display: 'block',
    position: 'relative',
    marginTop: 11,
  },
  imageWith: {
    width: 'fit-content',
    height: 'auto',
  },
  smallImageWith: {
    width: '50%',
    height: 'auto',
  },
  closeButton: {
    right: '12px !important',
    top: '8px !important',
    background: 'rgba(0, 0, 0, 0.54)',
    display: 'flex',
    borderRadius: 22,
  },
  closeIcon: {
    color: theme.ptas.colors.theme.white,
    width: 20,
  },
  backdropRoot: {
    background:
      'linear-gradient(180deg, rgba(0, 0, 0, 0.1) 0%, rgba(0, 0, 0, 0.4) 100%)',
  },
  popoverRoot: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
  },
  popoverContent: {
    width: '100%',
    height: '100%',
    background: '#F0F0F0',
    boxShadow: 'inset 0px 2px 16px rgba(0, 0, 0, 0.25)',
    boxSizing: 'border-box',
  },
  excelPopoverContent: {
    padding: 20,
  },
  officeViewerPopoverContent: {
    paddingBottom: 100,
    height: '100%',
  },
  imagePreviewContainer: {
    height: '100%',
    width: '100%',
  },
  dimBlackColor: {
    backgroundColor: 'rgba(0,0,0,0.7)',
    height: 'auto',
  },
  imagePreview: {
    width: '100%',
    height: '100%',
    objectFit: 'cover',
  },
  smallWidth: {
    width: 'auto',
    margin: '0 auto',
    display: 'block',
  },
}));

function PreviewFile(props: Props): JSX.Element {
  const classes = useStyles(props);
  const { file, open, onClose } = props;

  const [filePreview, setFilePreview] = useState<CustomFile>();
  const [anchorElement, setAnchorElement] = useState<HTMLElement | null>(null);
  const [imageWidth, setImageWidth] = useState<number>(0);
  const [fileType, setFileType] = useState<
    'pdf' | 'word' | 'excel' | 'image' | 'other' | undefined
  >(undefined);

  const closePopover = (): void => {
    setAnchorElement(null);
    onClose?.();
  };

  useEffect(() => {
    if (file?.content) {
      const fileName = file?.fileName.toLowerCase();

      if (fileName.endsWith('.pdf')) {
        setFileType('pdf');
      } else if (fileName.endsWith('.doc') || fileName.endsWith('.docx')) {
        setFileType('word');
      } else if (fileName.endsWith('.xls') || fileName.endsWith('.xlsx')) {
        setFileType('excel');
      } else if (fileName.endsWith('.png') || fileName.endsWith('.jpg')) {
        setFileType('image');
        console.log('is a image');

        // set image width
        const img = new Image();
        img.onload = (): void => {
          setImageWidth(img.width);
        };
        img.src = file.content as string;
      } else {
        setFileType('other');
      }
    } else {
      setFileType(undefined);
    }

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [filePreview]);

  useUpdateEffect(() => {
    setFilePreview(file);
  }, [file]);

  useUpdateEffect(() => {
    if (open) return setAnchorElement(document.body);
  }, [open]);
  return (
    <CustomPopover
      marginThreshold={0}
      anchorEl={anchorElement}
      onClose={(): void => {
        closePopover();
      }}
      ptasVariant="success"
      showCloseButton
      anchorPosition={{
        top: 0,
        left: 0,
      }}
      classes={{
        paper: clsx(classes.popover),
        closeIcon: classes.closeIcon,
        closeButton: classes.closeButton,
        backdropRoot: classes.backdropRoot,
        root: classes.popoverRoot,
      }}
    >
      <div
        className={clsx(
          classes.popoverContent,
          fileType === 'excel'
            ? classes.excelPopoverContent
            : fileType === 'word'
            ? classes.officeViewerPopoverContent
            : fileType === 'image' && imageWidth > 616
            ? classes.imageWith
            : classes.smallImageWith
        )}
      >
        {filePreview && fileType === 'excel' && (
          <OfficeFilePreview fileUrl={filePreview.content} />
        )}
        {filePreview && fileType === 'image' && (
          <Box
            className={clsx(
              classes.imagePreviewContainer,
              imageWidth < 616 && classes.dimBlackColor
            )}
          >
            <img
              src={filePreview.content as string}
              alt="Preview"
              // 616 is minimun size
              className={clsx(
                classes.imagePreview,
                imageWidth < 616 && classes.smallWidth
              )}
            />
          </Box>
        )}
        {filePreview && fileType === 'pdf' && (
          <PdfFilePreview elementId={'pdfBody'} file={filePreview.file} />
        )}
        {filePreview && fileType === 'word' && (
          <OfficeFilePreview fileUrl={filePreview?.content as string} />
        )}
      </div>
    </CustomPopover>
  );
}

export default PreviewFile;
