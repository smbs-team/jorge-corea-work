// formatMessage.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { FormattedMessage } from 'react-intl';
import React from 'react';

export const title = (
  <FormattedMessage
    id="current_use_title"
    defaultMessage="Current use exemption"
  />
);

export const signIn = (
  <FormattedMessage id="current_use_signin" defaultMessage="Sign in" />
);

export const appliedExemption = (date: string): JSX.Element => (
  <FormattedMessage
    id="current_use_applied_exemption"
    defaultMessage="Applied {date}"
    values={{ date: date }}
  />
);

export const availableForms = (
  <FormattedMessage
    id="current_use_available_forms"
    defaultMessage="Available current use forms"
  />
);

export const tabFarmLand = (
  <FormattedMessage id="current_use_tab_farm_land" defaultMessage="Farm land" />
);

export const farmLandHeader = (
  <FormattedMessage
    id="current_use_farm_land_header"
    defaultMessage="The farm and agricultural land exemption provides land value reduction when the land is used for the production of commercial livestock or agriculture. "
  />
);

export const farmLandDownload = (
  <FormattedMessage
    id="current_use_farm_land_download"
    defaultMessage="1. Download "
  />
);

export const farmLandDownloadFileName = (
  <FormattedMessage
    id="current_use_farm_land_download_file_name"
    defaultMessage="KC-Exemption-Farmland.pdf"
  />
);

export const farmLandCompleteApplication = (
  <FormattedMessage
    id="current_use_farm_land_complete_application"
    defaultMessage="2. Complete the application"
  />
);

export const farmLandAttachApplication = (
  <FormattedMessage
    id="current_use_farm_land_attach_application"
    defaultMessage="3. Attach the application, including map and supporting documents (cover up account and soc. security #s)"
  />
);

export const tabForestLand = (
  <FormattedMessage
    id="current_use_tab_forest_land"
    defaultMessage="Forest land"
  />
);

export const forestLandHeader = (
  <FormattedMessage
    id="current_use_forest_land_header"
    defaultMessage="Forest land exemption is similar to the “open space - timber land” program but is for property containing more than 20 acres of eligible forest land primarily devoted to the growth and harvest of timber."
  />
);

export const forestLandDownload = (
  <FormattedMessage
    id="current_use_forest_land_download"
    defaultMessage="1. Download "
  />
);

export const forestLandOr = (
  <FormattedMessage id="current_use_forest_land_or" defaultMessage=" or " />
);

export const forestLandDownLoadFileName1 = (
  <FormattedMessage
    id="current_use_forest_land_download_file_name_1"
    defaultMessage="KC-Exemption-Forestland.pdf"
  />
);

export const forestLandDownLoadFileName2 = (
  <FormattedMessage
    id="current_use_forest_land_download_file_name_2"
    defaultMessage="KC-Exemption-Forestland-multi-owner.pdf"
  />
);

export const forestLandCompleteApplication = (
  <FormattedMessage
    id="current_use_forest_land_complete_application"
    defaultMessage="2. Complete the application"
  />
);

export const forestLandAttachApplication = (
  <FormattedMessage
    id="current_use_forest_land_attach_application"
    defaultMessage="3. Attach the application (include timber management plan)"
  />
);

export const tabOpenSpace = (
  <FormattedMessage
    id="current_use_tab_open_space"
    defaultMessage="Open space"
  />
);

export const openSpaceHeader = (
  <FormattedMessage
    id="current_use_open_space_header"
    defaultMessage="The “open space” exemption uses the points-based Public Benefit Rating System (PBRS). The total points awarded translate to a 50%-90% reduction in the land assessed value for the portion of the property enrolled."
  />
);

export const openSpaceGoTo = (
  <FormattedMessage
    id="current_use_open_space_go_to"
    defaultMessage="1. Go to the "
  />
);

export const openSpaceInfoPage = (
  <FormattedMessage
    id="current_use_open_space_info_page"
    defaultMessage="current use info page"
  />
);

export const openSpaceDownloadForm = (
  <FormattedMessage
    id="current_use_open_space_download_form"
    defaultMessage="2. Download the open space form"
  />
);

export const openSpaceFollowDirections = (
  <FormattedMessage
    id="current_use_open_space_follow_directions"
    defaultMessage="3. Follow the directions on the form"
  />
);

export const buttonDone = (
  <FormattedMessage id="current_use_button_done" defaultMessage="Done" />
);

export const indication = (
  <FormattedMessage
    id="current_use_indication"
    defaultMessage="If you think your property is eligible for a current use exemption, find your property and apply now."
  />
);

export const help = (
  <FormattedMessage id="current_use_help" defaultMessage="Need some help?" />
);

export const instruction = (
  <FormattedMessage
    id="current_use_instruction"
    defaultMessage="Instructions"
  />
);

export const findMyProperty = (
  <FormattedMessage
    id="current_use_find_my_property"
    defaultMessage="Find my property"
  />
);

export const enterAddress = (
  <FormattedMessage
    id="current_use_enter_address"
    defaultMessage="Enter an address, parcel #, or commercial project"
  />
);

export const locateMe = (
  <FormattedMessage id="current_use_locate_me" defaultMessage="Locate me" />
);

export const uploadDragReject = (
  <FormattedMessage
    id="current_use_upload_drag_reject"
    defaultMessage="Invalid file selection."
  />
);
