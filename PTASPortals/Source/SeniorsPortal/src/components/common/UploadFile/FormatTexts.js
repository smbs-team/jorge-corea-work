import { FormattedMessage } from 'react-intl';
import React from 'react';

export const maxFileSize = (
  <FormattedMessage
    id="uploadFile_maxFileSize"
    defaultMessage="The document you are trying to attach is over the allowable file size of 4 MB."
    description="Max file size label"
  />
);

export const maxPDFFileSize = (
  <FormattedMessage
    id="uploadFile_maxPDFFileSize"
    defaultMessage="The PDF document you are trying to attach is over the allowable file size."
    description="Max file size label"
  />
);

export const analysisError = (
  <FormattedMessage
    id="uploadFile_analysisError"
    defaultMessage="We can't upload your document because we can't find any readable text."
    description="Analysis error message"
  />
);
export const invalidDocumentType = (
  <FormattedMessage
    id="uploadFile_invalidDocumentType"
    defaultMessage="The document type you are trying to attach is invalid. Please try one of the valid document types (.pdf, .jpg, .jpeg, .png, .gif)"
    description="Invalid document type label message"
  />
);
export const fileAlreadyProcessed = (
  <FormattedMessage
    id="uploadFile_fileAlreadyProcessed"
    defaultMessage="File already processed."
    description="File already processed label message"
  />
);
export const uploadButton = (
  <FormattedMessage
    id="uploadFile_uploadButton"
    defaultMessage="Upload"
    description="Upload button"
  />
);
export const dragNdropDropTheFilesHere = (
  <FormattedMessage
    id="uploadFile_dragNdropDropTheFilesHere"
    defaultMessage="Drop the files here"
    description="Label when you hover a file over the drag and drop box"
  />
);

export const dragNdropDefaultMessage = (
  <FormattedMessage
    id="uploadFile_dragNdropDefaultMessage"
    defaultMessage={'{select} attachments (or drag in)'}
    description="Label when you hover a file over the drag and drop box"
    values={{ select: <u>Select</u> }}
  />
);

export const dragNdropDefaultMessageMobile = (
  <FormattedMessage
    id="uploadFile_dragNdropDefaultMessageMobile"
    defaultMessage={'Upload'}
    description="Label when you hover a file over the drag and drop box"
  />
);

export const dragNdropInvalidFile = (
  <FormattedMessage
    id="uploadFile_dragNdropInvalidFile"
    defaultMessage="Invalid document type"
    description="Label when you try to drop an invalid file"
  />
);
export const whatObscure = (
  <FormattedMessage
    id="uploadFile_whatObscure"
    defaultMessage="What should I obscure?"
  />
);
export const helpText1 = (
  <FormattedMessage
    id="uploadFile_obscureMessage"
    defaultMessage="It's important to obscure Social Security and account numbers on your documents, which become part of the public record."
  />
);
export const helpText2 = (
  <FormattedMessage
    id="uploadFile_obscureMessage2"
    defaultMessage="You can obscure them with our online tool as you upload your documents."
  />
);
export const helpText3 = (
  <FormattedMessage
    id="uploadFile_obscureMessage3"
    defaultMessage="Or, before you upload, copy your documents and obscure this information using paper or tape, a felt pen, or an online image-editing program. Then upload the redacted copies."
  />
);
