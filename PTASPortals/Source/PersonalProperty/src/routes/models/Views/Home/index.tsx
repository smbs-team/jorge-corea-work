// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState } from 'react';
import { useHistory } from 'react-router-dom';
import {
  CustomCard,
  CustomTextButton,
  CardPersonalProperty,
  CustomPopover,
  Alert,
  utilService
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { Box } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatText';
import HelpSection from '../../../../components/HelpSection';
import useHome from './useHome';
import AddBusinessCard from './AddBusinessCard';
import HomeStaff from './HomeStaff';
import ProfileOptionsButton from 'components/ProfileOptionsButton';
import BusinessAccessPopover from './BusinessAccessPopover';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    maxWidth: '100%',
    width: '640px',
    display: 'block',
    boxSizing: 'border-box',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    paddingTop: 16,
    marginBottom: 15,
    marginLeft: 'auto',
    marginRight: 'auto',
    overflow: 'visible',
    borderRadius: 0,
    [theme.breakpoints.up('sm')]: {
      borderRadius: 24,
    },
  },
  contentWrap: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
    width: '100%',
    maxWidth: 800,
    margin: '16px auto',
  },
  title: {
    fontSize: theme.ptas.typography.h6.fontSize,
    fontFamily: theme.ptas.typography.titleFontFamily,
    marginBottom: 32,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    margin: 0,
    width: '100%',
    maxWidth: 283,
    [theme.breakpoints.up('sm')]: {
      fontSize: theme.ptas.typography.h5.fontSize,
      maxWidth: 329,
    },
  },
  content: {
    display: 'flex',
    flexDirection: 'column',
    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
    },
  },
  businesses: {
    marginRight: 0,
    [theme.breakpoints.up('sm')]: {
      marginRight: theme.spacing(4),
    },
  },
  subtitle: {
    display: 'flex',
    alignItems: 'center',
    marginBottom: theme.spacing(2),
    textAlign: 'center',
  },
  subtitleText: {
    // fontSize: theme.ptas.typography.h6.fontSize,
    fontSize: '20px',
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '23px',
    marginRight: theme.spacing(4),
  },
  subtitleButton: {
    fontSize: theme.ptas.typography.buttonSmall.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '19px',
  },
  people: {},
  peopleAccessText: {
    fontSize: '14px',
    fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
    lineHeight: '17px',
  },

  personalPropertyCard: {
    backgroundColor: theme.ptas.colors.theme.white,
    marginBottom: theme.spacing(2),
  },

  textHelp: {
    fontSize: theme.ptas.typography.body.fontSize,
    fontWeight: theme.ptas.typography.bodyBold.fontWeight,
    marginBottom: 16,
    color: theme.ptas.colors.theme.black,
  },

  name: {
    position: 'absolute',
    top: 10,
    right: 16,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    width: '50%',
    marginBottom: 8,
    marginTop: 32,
    display: 'block',
  },
  marginBottom: {
    marginBottom: 32,
  },
  propertyInfoRoot: {
    width: '100%',
    maxWidth: 668,
  },
  propertyInfoContent: {},
  textButtonRoot: {
    fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
  },
  whoCanManageList: {
    display: 'flex',
    flexDirection: 'column',
  },
  whoCanManageItem: {
    margin: '5px 0',
    padding: '3px 0',
  },
}));

function Home(): JSX.Element {
  const classes = useStyles();
  const {
    businessList,
    businessContactList,
    peopleList,
    handleAddBusiness,
    handleAddPerson,
    addBusiness,
    setAddBusiness,
    isStaffUser,
    wcmPopoverAnchor,
    onWhoCanManageClick,
    closeWhoCanManagePopover,
    fetchBusinessByContact,
    onManageBusinessClick,
    contactsByBusinessSelect,
    editAccessLevelId,
  } = useHome();
  const history = useHistory();
  const [businessAccessAnchor, setBusinessAccessAnchor] =
    useState<HTMLElement | null>(null);
  const [popoverBusinessList, setPopoverBusinessList] = useState<string[]>([]);

  const renderWhoCanManageList = (): JSX.Element => {
    const whoCanManageListRender = contactsByBusinessSelect.map((pc) => {
      return (
        <span key={pc.id} className={classes.whoCanManageItem}>
          {`${pc.firstName ?? '-'} ${pc.lastName}`}
        </span>
      );
    });
    return (
      <div className={classes.whoCanManageList}>{whoCanManageListRender}</div>
    );
  };

  const renderBusinessCards = (): JSX.Element[] => {
    return businessContactList.map((businessContact) => {
      if (!businessContact.personalProperty) return <React.Fragment />;
      const business = businessContact.personalProperty;
      const disabledOutlineButton =
        businessContact.accessLevel !== editAccessLevelId;
      return (
        <CardPersonalProperty
          key={business.id}
          type={'business'}
          filedDate={business.filedDate}
          assessedYear={business.assessedYear}
          shadow
          title={business.businessName}
          subtitle={fm.personalPropertyAccount(business.accountNumber)}
          disabledOutlineButton={disabledOutlineButton}
          onOutlineButtonClick={onManageBusinessClick(business.id)}
          onTextButtonClick={onWhoCanManageClick(business.id)}
          outlineButtonText={fm.businessesManageButton}
          textButtonText={fm.businessesWhoManageButton}
          classes={{ root: classes.personalPropertyCard }}
        />
      );
    });
  };

  return (
    <MainLayout>
      <CustomCard
        variant="wrapper"
        classes={{
          rootWrap: classes.card,
        }}
      >
        <div className={classes.name}>
          <ProfileOptionsButton />
        </div>
        {!isStaffUser && (
          <Fragment>
            <h2 className={classes.title}>{fm.title}</h2>
            <div className={classes.contentWrap}>
              <Box className={classes.content}>
                <Box className={classes.businesses}>
                  <Box className={classes.subtitle}>
                    <label className={classes.subtitleText}>
                      {fm.businesses}
                    </label>
                    <CustomTextButton
                      classes={{ root: classes.subtitleButton }}
                      onClick={handleAddBusiness}
                    >
                      {fm.businessesAddBusiness}
                    </CustomTextButton>
                  </Box>

                  {(addBusiness || businessList.length === 0) && (
                    <AddBusinessCard
                      fetchBusinessByContact={fetchBusinessByContact}
                      setAddBusiness={setAddBusiness}
                    />
                  )}

                  <Box>{renderBusinessCards()}</Box>
                  <CustomPopover
                    anchorEl={wcmPopoverAnchor}
                    onClose={closeWhoCanManagePopover}
                    ptasVariant="success"
                    showCloseButton
                    tail
                    tailPosition="start"
                  >
                    <Alert
                      contentText={renderWhoCanManageList()}
                      ptasVariant="success"
                    />
                  </CustomPopover>
                </Box>
                <Box className={classes.people}>
                  <Box className={classes.subtitle}>
                    <label className={classes.subtitleText}>{fm.people}</label>
                    <CustomTextButton
                      classes={{ root: classes.subtitleButton }}
                      onClick={handleAddPerson}
                      disabled={!businessContactList.length}
                    >
                      {fm.peopleAddPerson}
                    </CustomTextButton>
                  </Box>

                  {!businessContactList.length && (
                    <Box className={classes.peopleAccessText}>
                      {fm.addBusinessFirst}
                    </Box>
                  )}

                  {!!businessContactList.length && !peopleList.length && (
                    <Box className={classes.peopleAccessText}>
                      {fm.addPeopleAccess}
                    </Box>
                  )}

                  <Box>
                    {peopleList.map((person) => (
                      <CardPersonalProperty
                        key={person.id}
                        type={'person'}
                        shadow
                        title={person.firstName + ' ' + person.lastName}
                        subtitle={fm.lastSeenDate(
                          utilService
                            .unixTimeToDate('[last seen date]')
                            ?.toLocaleDateString() ?? ''
                        )}
                        onOutlineButtonClick={(
                          evt: React.MouseEvent<HTMLButtonElement>
                        ): void => {
                          console.log('Edit secondary profile');
                          history.push(`/sec-profile/${person.id}`);
                        }}
                        onTextButtonClick={(
                          evt: React.MouseEvent<HTMLButtonElement>
                        ): void => {
                          setBusinessAccessAnchor(evt.currentTarget);
                          setPopoverBusinessList(
                            person.businessAccess.map((el) => el.name)
                          );
                        }}
                        outlineButtonText={fm.peopleEdit}
                        textButtonText={fm.peopleHasAccess}
                        classes={{ root: classes.personalPropertyCard }}
                      />
                    ))}
                  </Box>
                </Box>
              </Box>
            </div>

            {businessAccessAnchor && (
              <BusinessAccessPopover
                anchorEl={businessAccessAnchor}
                businessList={popoverBusinessList}
                onClose={(): void => setBusinessAccessAnchor(null)}
              />
            )}
          </Fragment>
        )}

        {isStaffUser && (
          <Fragment>
            <HomeStaff />
          </Fragment>
        )}
        <HelpSection />
      </CustomCard>
    </MainLayout>
  );
}

export default Home;
