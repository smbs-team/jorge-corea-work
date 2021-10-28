// UpdateBusiness.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect } from 'react';
import {
  CustomCard,
  CustomTextButton,
  CustomTabs,
  CustomButton,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core/styles';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatText';
import * as fmGeneral from '../../../../GeneralFormatMessage';
import BasicInfo from './BasicInfo';
import Location from './Location';
import Contact from './Contact';
import Assets from './Assets';
import Exemptions from './Exemptions';
import Supplies from './Supplies';
import Improvements from './Improvements';
import ViewChanges from './ViewChanges';
import HelpSection from 'components/HelpSection';
import ProfileOptionsButton from 'components/ProfileOptionsButton';
import { withUpdateBusinessProvider } from 'contexts/UpdateBusiness/index';
import UseUpdateBusiness from './useUpdateBusiness';
import { AppContext } from 'contexts/AppContext';
import useUpdateBusiness from './useUpdateBusiness';
import { contactApiService } from 'services/api/apiService/portalContact';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    maxWidth: 1027,
    display: 'flex',
    justifyContent: 'center',
    position: 'relative',
    boxSizing: 'border-box',
    paddingTop: 24,
    marginLeft: 'auto',
    marginRight: 'auto',
    background: theme.ptas.colors.theme.white,
    paddingRight: 8,
    paddingLeft: 8,
    borderRadius: 0,
    [theme.breakpoints.up('sm')]: {
      paddingRight: 33,
      paddingLeft: 33,
      borderRadius: 24,
    },
  },
  contentWrap: {
    width: '100%',
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    marginBottom: 8,
    display: 'block',
    margin: '0 auto',
    width: '100%',
    maxWidth: 320,
  },
  closeIcon: {
    color: theme.ptas.colors.theme.black,
    fontSize: 34,
  },
  wrapper: {
    width: '100%',
    maxWidth: 303,
    display: 'flex',
    justifyContent: 'center',
    flexDirection: 'column',
    margin: '0 auto',
  },
  item: {
    fontSize: theme.ptas.typography.body.fontSize,
    listStyle: 'none',

    '&::before': {
      content: `'•'`,
      marginRight: 8,
    },
  },
  subtitle: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
  },
  list: {
    padding: 0,
    margin: 0,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    marginBottom: 15,
  },
  date: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    marginBottom: 16,
    textAlign: 'center',
  },
  joinWebinarButton: {
    width: 160,
    height: 30,
    margin: '0 auto',
  },
  headTitle: {
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    fontFamily: theme.ptas.typography.titleFontFamily,
    lineHeight: '23px',
  },
  title: {
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: 24,
    fontFamily: theme.ptas.typography.titleFontFamily,
    display: 'block',
    lineHeight: '28px',
    marginBottom: 5,
  },
  head: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
    marginBottom: 29,
    width: '100%',
  },
  titlesWrap: {
    textAlign: 'center',
  },
  description: {
    fontSize: theme.ptas.typography.body.fontSize,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    display: 'block',
    textAlign: 'center',
  },
  tabs: {
    marginBottom: 32,
  },
  saveBusinessWrap: {
    margin: '0 auto',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    width: 428,
    borderRadius: 9,
    padding: '10px 16px',
    background: theme.ptas.colors.utility.success,
    marginBottom: 32,
    boxSizing: 'border-box',
  },
  saveBusinessDesc: {
    color: theme.ptas.colors.theme.white,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
  },
  saveBusinessButton: {
    fontSize: 14,
    width: 128,
    height: 23,
  },

  buttonMenu: {
    marginLeft: 10,
  },
}));

function UpdateBusiness(): JSX.Element {
  const classes = useStyles();

  const { portalContact } = useContext(AppContext);

  const { setEditingContact } = useUpdateBusiness();

  const {
    currentStepIndex,
    viewChangesOpen,
    handleSelectedTab,
    handleViewChangesClose,
    setViewChangesOpen,
    loadBusiness,
    loadPropertyType,
    loadStateOrProvince,
    setAddressList,
    setEmailList,
    setPhoneList,
    loadCountryOpts,
    loadPhoneTypes,
    loadYearsAcquired,
    loadChangeReason,
    loadCategoryAssets,
    loadImprovementTypes,
  } = UseUpdateBusiness();

  const renderChildrenStep = (): JSX.Element => {
    const stepChildren = [
      <BasicInfo />, // currentStepIndex is 0
      <Location />, // currentStepIndex is 1
      <Contact />, // currentStepIndex is 2
      <Assets />, // currentStepIndex is 3
      <Exemptions />, // currentStepIndex is 4
      <Supplies />, // currentStepIndex is 5
      <Improvements />, // currentStepIndex is 6
    ];

    return stepChildren[currentStepIndex] ?? <Fragment />;
  };

  const tabItems = [
    {
      label: fm.basicInfo,
      disabled: false,
    },
    {
      label: fm.location,
      disabled: false,
    },
    {
      label: fm.contact,
      disabled: false,
    },
    {
      label: fm.assets,
      disabled: false,
    },
    {
      label: fm.exemptions,
      disabled: false,
    },
    {
      label: fm.supplies,
      disabled: false,
    },
    {
      label: fm.improvements,
      disabled: false,
    },
  ];

  useEffect(() => {
    loadBusiness();
    loadStateOrProvince();
    loadPropertyType();
    loadCountryOpts();
    loadPhoneTypes();
    loadYearsAcquired();
    loadChangeReason();
    loadCategoryAssets();
    loadImprovementTypes();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // get email, address and phone number
  const setContactInformation = (): void => {
    if (!portalContact) return;

    const emailReq = contactApiService.getContactEmails(portalContact.id);

    const addressesReq = contactApiService.getContactAddresses(
      portalContact.id
    );

    const phonesReq = contactApiService.getContactPhones(portalContact.id);

    Promise.all([emailReq, addressesReq, phonesReq]).then((response) => {
      const [emails, addresses, phones] = response;

      setEmailList(emails.data ?? []);
      setAddressList(addresses.data ?? []);
      setPhoneList(phones.data ?? []);
    });
  };

  useEffect(() => {
    if (!portalContact) return;

    setEditingContact(portalContact);

    setContactInformation();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [portalContact]);

  const renderFinishedStep = (): JSX.Element => {
    if (currentStepIndex !== 6) return <Fragment />; // if the step is not improvement

    return (
      <div className={classes.saveBusinessWrap}>
        <span className={classes.saveBusinessDesc}>
          {fm.whenYouFinishEditing}
        </span>
        <CustomButton
          classes={{ root: classes.saveBusinessButton }}
          ptasVariant="Inverse"
          onClick={(): void => setViewChangesOpen(true)}
        >
          {fm.viewChange}
        </CustomButton>
        <span className={classes.saveBusinessDesc}>{fm.beforeSaving}</span>

        <ViewChanges open={viewChangesOpen} onClose={handleViewChangesClose} />
      </div>
    );
  };

  return (
    <MainLayout>
      <CustomCard
        variant="wrapper"
        classes={{
          rootWrap: classes.card,
          wrapperContent: classes.contentWrap,
        }}
      >
        <div className={classes.head}>
          <CustomTextButton ptasVariant="Clear">
            {fmGeneral.back}
          </CustomTextButton>
          <div className={classes.titlesWrap}>
            <span className={classes.description}>
              {fm.businessPersonalProperty}
            </span>
            <span className={classes.headTitle}>{fm.newBusiness}</span>
          </div>
          <div className={classes.buttonMenu}>
            <ProfileOptionsButton />
          </div>
        </div>
        {renderFinishedStep()}

        {/* tabs */}
        <CustomTabs
          ptasVariant="Switch"
          items={tabItems}
          onSelected={handleSelectedTab}
          classes={{
            root: classes.tabs,
          }}
          // selectedIndex={currentIndexTab}
        />
        {/* end tabs */}
        {/* body */}
        {renderChildrenStep()}
        {/* end body */}
        {/* footer */}
        <HelpSection />
      </CustomCard>
    </MainLayout>
  );
}

export default withUpdateBusinessProvider(UpdateBusiness);
