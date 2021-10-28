// OldInfo.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomTextField } from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import useStyles from './styles';
import EmailIcon from '@material-ui/icons/Email';
import StayCurrentPortraitIcon from '@material-ui/icons/StayCurrentPortrait';
import clsx from 'clsx';

function OldInfo(): JSX.Element {
  const classes = useStyles();

  const dummyData = {
    prevBusinessName: 'Auburn Center Dry Cleaning',
    prevTaxpayer: 'WTCWEND',
    prevUBINumber: '897651435',
    prevBusinessType: 'Sole proprietor',
    prevNAICSNumber: '001',
    stateOfIncorporation: 'TN - Tennessee',
    prevOwner: 'Kimberly',
    prevEmail: 'kbass@smsholdings.com',
    prevPhone: '615-799-1535',
    prevMailingAddress: '1801 Howard Rd',
    city: 'Auburn',
    state: 'WA',
    zip: '98002',
  };

  return (
    <div className={classes.oldInfoTab}>
      <CustomTextField
        ptasVariant="underline"
        label={fm.previousBusinessName}
        readOnly
        value={dummyData.prevBusinessName}
        classes={{ root: classes.readOnly }}
      />
      <CustomTextField
        ptasVariant="underline"
        label={fm.previousTaxpayer}
        classes={{ root: classes.readOnly }}
        readOnly
        value={dummyData.prevTaxpayer}
      />
      <CustomTextField
        ptasVariant="underline"
        label={fm.previousUBINumber}
        classes={{ root: classes.readOnly }}
        readOnly
        value={dummyData.prevUBINumber}
      />
      <CustomTextField
        ptasVariant="underline"
        label={fm.previousBusinessType}
        classes={{ root: classes.readOnly }}
        readOnly
        value={dummyData.prevBusinessType}
      />
      <CustomTextField
        ptasVariant="underline"
        label={fm.previousNAICSNumber}
        classes={{ root: classes.readOnly }}
        readOnly
        value={dummyData.prevNAICSNumber}
      />
      <CustomTextField
        ptasVariant="underline"
        label={fm.stateOfIncorporation}
        classes={{ root: classes.readOnly }}
        readOnly
        value={dummyData.stateOfIncorporation}
      />
      <span className={classes.border}></span>
      <CustomTextField
        ptasVariant="underline"
        label={fm.previousOwnerFullName}
        classes={{ root: classes.readOnly }}
        readOnly
        value={dummyData.prevOwner}
      />
      <CustomTextField
        ptasVariant="underline"
        label={fm.previousEmail}
        classes={{ root: classes.readOnly }}
        type="email"
        endAdornment={<EmailIcon className={classes.emailIcon} />}
        readOnly
        value={dummyData.prevEmail}
      />
      <CustomTextField
        ptasVariant="underline"
        label={fm.previousPhone}
        classes={{ root: classes.readOnly }}
        type="email"
        endAdornment={<StayCurrentPortraitIcon className={classes.emailIcon} />}
        readOnly
        value={dummyData.prevPhone}
      />
      <span className={classes.border}></span>
      <CustomTextField
        ptasVariant="underline"
        label={fm.previousMailingAddress}
        classes={{ root: classes.readOnly }}
        readOnly
        value={dummyData.prevMailingAddress}
      />
      <div className={clsx(classes.inputWrap, classes.marginBottomNone)}>
        <CustomTextField
          ptasVariant="underline"
          label={fm.city}
          classes={{
            root: clsx(
              classes.readOnly,
              classes.city,
              classes.marginBottomNone
            ),
          }}
          readOnly
          value={dummyData.city}
        />
        <CustomTextField
          ptasVariant="underline"
          classes={{
            root: clsx(
              classes.readOnly,
              classes.state,
              classes.marginBottomNone
            ),
          }}
          label={fm.state}
          readOnly
          value={dummyData.state}
        />
        <CustomTextField
          ptasVariant="underline"
          classes={{
            root: clsx(classes.readOnly, classes.zip, classes.marginBottomNone),
          }}
          label={fm.zip}
          readOnly
          value={dummyData.zip}
        />
      </div>
      <span className={classes.border}></span>
      <CustomTextField
        ptasVariant="underline"
        label={fm.previousLocationAddress}
        classes={{ root: classes.readOnly }}
        readOnly
        value={dummyData.prevMailingAddress}
      />
      <div className={classes.inputWrap}>
        <CustomTextField
          ptasVariant="underline"
          label={fm.city}
          classes={{ root: `${classes.readOnly} ${classes.city}` }}
          readOnly
          value={dummyData.city}
        />
        <CustomTextField
          ptasVariant="underline"
          classes={{ root: `${classes.readOnly} ${classes.state}` }}
          label={fm.state}
          readOnly
          value={dummyData.state}
        />
        <CustomTextField
          ptasVariant="underline"
          classes={{ root: `${classes.readOnly} ${classes.zip}` }}
          label={fm.zip}
          readOnly
          value={dummyData.zip}
        />
      </div>
    </div>
  );
}

export default OldInfo;
