// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const title = (
  <FormattedMessage
    id="manage_business_title"
    defaultMessage="Business personal property"
  />
);

export const propertyAccount = (
  <FormattedMessage
    id="manage_business_property_account"
    defaultMessage="Personal property account #"
  />
);

export const filed = (
  <FormattedMessage id="manage_business_filed" defaultMessage="Filed" />
);

export const assessed = (
  <FormattedMessage id="manage_business_assessed" defaultMessage="Assessed" />
);

export const correction = (
  <FormattedMessage
    id="manage_business_correction"
    defaultMessage="Correction"
  />
);

export const omission = (
  <FormattedMessage id="manage_business_omission" defaultMessage="Omission" />
);

export const refund = (
  <FormattedMessage id="manage_business_refund" defaultMessage="Refund" />
);

export const valueSheet = (
  <FormattedMessage
    id="manage_business_value_sheet"
    defaultMessage="Value sheet"
  />
);

export const valueNotice = (
  <FormattedMessage
    id="manage_business_value_notice"
    defaultMessage="Value notice"
  />
);

export const updateListingInfo = (
  <FormattedMessage
    id="manage_business_update_listing_info"
    defaultMessage="Update listing info"
  />
);

export const attachListingInfo = (
  <FormattedMessage
    id="manage_business_attach_listing_info"
    defaultMessage="Attach listing info"
  />
);

export const businessMovedSoldClosed = (
  <FormattedMessage
    id="manage_business_moved_sold_closed"
    defaultMessage="This business has {movedSoldClosed}"
    values={{
      movedSoldClosed: <span>moved, sold or closed</span>,
    }}
  />
);

export const businessRemove = (
  <FormattedMessage id="manage_business_remove" defaultMessage="Remove" />
);

export const removeAlert = (
  <FormattedMessage
    id="manage_business_remove_alert_1"
    defaultMessage="{line1}{line2}"
    values={{
      line1: (
        <span>
          Removes this business from your account.
          <br />
          <br />
        </span>
      ),
      line2: (
        <span>
          You and people you’ve assigned lose the ability to see or edit this
          listing information.
        </span>
      ),
    }}
  />
);

export const removeBusiness = (
  <FormattedMessage
    id="manage_business_remove_business"
    defaultMessage="Remove business"
  />
);

export const removeAlert2 = (
  <FormattedMessage
    id="manage_business_remove_alert_2"
    defaultMessage="You and people you’ve assigned lose the ability to see or edit this listing information."
  />
);

export const saveDocument = (
  <FormattedMessage
    id="manage_business_save_document"
    defaultMessage="Save document"
  />
);

export const previewTitle = (
  <FormattedMessage
    id="manage_business_preview_title"
    defaultMessage="Preview"
  />
);

export const convertPtmsToExcel = (
  <FormattedMessage
    id="attach_listing_info_convert_PTMS"
    defaultMessage="Convert PTMS to Excel"
  />
);

export const createYourPtmsReport = (
  <FormattedMessage
    id="attach_listing_info_create_report"
    defaultMessage="Create your PTMS report "
  />
);

export const toolbar = (
  <FormattedMessage
    id="attach_listing_info_toolbar"
    defaultMessage="Toolbar > Export button"
  />
);

export const exportFormatExcel = (
  <FormattedMessage
    id="attach_listing_info_export_format_excel"
    defaultMessage="Export > Format > Excel"
  />
);

export const exportDiskLike = (
  <FormattedMessage
    id="attach_listing_info_export_disk_like"
    defaultMessage="Export > Disk file"
  />
);
export const exportOkay = (
  <FormattedMessage
    id="attach_listing_info_export_ok"
    defaultMessage="Export > Okay"
  />
);

export const exportSave = (
  <FormattedMessage
    id="attach_listing_info_export_save"
    defaultMessage="Export > Save"
  />
);

export const download = (
  <FormattedMessage
    id="attach_listing_info_download"
    defaultMessage="Download: "
  />
);

export const template = (
  <FormattedMessage
    id="attach_listing_info_template"
    defaultMessage=" template"
  />
);

export const eListing = (
  <FormattedMessage
    id="attach_listing_info_template"
    defaultMessage="eListing.xlsx"
  />
);

export const ptmsToExcel = (
  <FormattedMessage
    id="attach_listing_info_PTMS_to_Excel"
    defaultMessage=" PTMS to Excel"
  />
);

export const attachListingFile = (
  <FormattedMessage
    id="attach_listing_info_attach_listing_file"
    defaultMessage="Attach listing file(s):"
  />
);
