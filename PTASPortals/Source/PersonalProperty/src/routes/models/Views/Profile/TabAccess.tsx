// TabAccess.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState } from 'react';
import { CardWithSlider } from '@ptas/react-public-ui-library';
import { makeStyles, useTheme, Theme } from '@material-ui/core/styles';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { useProfile } from './ProfileContext';
import { PersonalPropertyAccountAccess } from 'models/personalPropertyAccountAccess';
import { v4 as uuid } from 'uuid';
import { businessContactApiService } from 'services/api/apiService/businessContact';
import { PP_ACCESS_STATUS_CODE_ACTIVE } from './constants';
import { useAsync } from 'react-use';

interface Props {
  profileType: 'primary' | 'secondary';
}

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    width: '100%',
    maxWidth: 448,
    padding: theme.spacing(1.25, 0, 4, 0),
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
  },
  title: {
    fontSize: theme.ptas.typography.bodyLargeBold.fontSize,
    fontWeight: theme.ptas.typography.bodyLargeBold.fontWeight,
    lineHeight: '24px',
    paddingBottom: theme.spacing(4),
  },
  businesses: {},
  cardWithSlider: {
    marginBottom: theme.spacing(2),
  },
}));

function TabAccess(props: Props): JSX.Element {
  const { profileType } = props;
  const classes = useStyles();
  const theme = useTheme();
  const {
    businessAccessList,
    accessLevelList,
    businessAccessListSec,
    setBusinessAccessListSec,
    editingContactSec,
    accessStatusCodes,
  } = useProfile();
  const [noneAccessCode] = useState(
    accessLevelList.find((el) => el.label === 'None')?.value
  );
  const [viewAccessCode] = useState(
    accessLevelList.find((el) => el.label === 'View')?.value
  );
  const [editAccessCode] = useState(
    accessLevelList.find((el) => el.label === 'Edit')?.value
  );

  useAsync(async () => {
    if (!businessAccessListSec.length) {
      //Create new access for each personal property with 'None' access level,
      //so that this user can be included in the People section list
      //TODO
    }
  }, []);

  const onAccessChange = (
    ppAccountAccess: PersonalPropertyAccountAccess,
    tab: number
  ): void => {
    if (!editingContactSec) return;
    if (!noneAccessCode || !viewAccessCode || !editAccessCode) return;

    const businessAccess = businessAccessListSec.find(
      (el) => el.id === ppAccountAccess.id
    );
    const newAccessLevel =
      tab === 2 ? editAccessCode : tab === 1 ? viewAccessCode : noneAccessCode;

    if (!businessAccess) {
      const access = new PersonalPropertyAccountAccess();
      access.id = uuid();
      access.personalPropertyId = ppAccountAccess.personalPropertyId;
      access.portalContactId = editingContactSec?.id;
      access.accessLevel = newAccessLevel;
      access.statusCode = accessStatusCodes.get(
        PP_ACCESS_STATUS_CODE_ACTIVE
      ) as number;
      //Save new access
      businessContactApiService.addBusinessContact(access);
      setBusinessAccessListSec((prev) => {
        return [...prev, access];
      });
    } else {
      businessAccess.accessLevel = newAccessLevel;
      //Update access
      businessContactApiService.changeContactBusiness(businessAccess);
      setBusinessAccessListSec((prev) => {
        return prev.map((el) =>
          el.id === businessAccess.id ? businessAccess : el
        );
      });
    }
  };

  const getSelectedIndexPrimary = (accessLevel: number): number => {
    return accessLevel === viewAccessCode
      ? 1
      : accessLevel === editAccessCode
      ? 2
      : 0; //None
  };

  const getSelectedIndexSecondary = (
    ppAccountAccess: PersonalPropertyAccountAccess
  ): number => {
    const access = businessAccessListSec.find(
      (el) => el.personalPropertyId === ppAccountAccess.personalPropertyId
    );
    if (!access) return 0;

    if (access.accessLevel === viewAccessCode) return 1;
    if (access.accessLevel === editAccessCode) return 2;
    return 0;
  };

  return (
    <Box className={classes.root}>
      <Box className={classes.title}>{fm.accessNotices}</Box>
      <Box className={classes.businesses}>
        {businessAccessList.map((businessAccess) => (
          <CardWithSlider
            key={businessAccess.id}
            classes={{ root: classes.cardWithSlider }}
            shadow
            title={businessAccess.personalProperty?.businessName}
            subtitle={fm.accessParcel(
              businessAccess.personalProperty?.accountNumber ?? '-'
            )}
            selectedIndex={
              profileType === 'primary'
                ? getSelectedIndexPrimary(businessAccess.accessLevel)
                : getSelectedIndexSecondary(businessAccess)
            }
            options={[
              {
                label: fm.accessNone,
                disabled:
                  profileType === 'primary' ||
                  businessAccess.accessLevel !== editAccessCode,
              },
              {
                label: fm.accessView,
                disabled:
                  profileType === 'primary' ||
                  businessAccess.accessLevel !== editAccessCode,
              },
              {
                label: fm.accessEdit,
                disabled:
                  profileType === 'primary' ||
                  businessAccess.accessLevel !== editAccessCode,
              },
            ]}
            onSelected={(tab: number): void => {
              console.log('Tab selected:', tab);
              if (profileType === 'secondary') {
                onAccessChange(businessAccess, tab);
              }
            }}
            tabsBackgroundColor={
              businessAccess.accessLevel ===
              accessLevelList.find((el) => el.label === 'None')?.value
                ? theme.ptas.colors.theme.gray
                : theme.ptas.colors.utility.selection
            }
          />
        ))}
      </Box>
    </Box>
  );
}

export default TabAccess;
