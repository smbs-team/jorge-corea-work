// NewBusiness.tsx
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
import Attach from './Attach';
import Assets from './Assets';
import Exemptions from './Exemptions';
import Supplies from './Supplies';
import Improvements from './Improvements';
import HelpSection from 'components/HelpSection';
import { withNewBusinessProvider } from 'contexts/NewBusiness';
import { useMount, useUpdateEffect } from 'react-use';
import { AppContext } from 'contexts/AppContext';
import useNewBusiness from './useNewBusiness';
import { businessApiService } from 'services/api/apiService/business';
import { apiService } from 'services/api/apiService';
import { useLocation } from 'react-router-dom';
import ProfileOptionsButton from 'components/ProfileOptionsButton';
import { STATUS_CODE_UNVERIFIED } from './constants';
import { getContactInfo } from 'utils/contact';
import { FileAttachmentMetadata } from 'models/fileAttachmentMetadata';
import { getOptionSetValue } from 'utils/business';

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
    width: 328,
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
  hideElement: {},
}));

export interface HomeState {
  createFromHome: boolean;
}

function NewBusiness(): JSX.Element {
  const classes = useStyles();
  const { state } = useLocation<HomeState>();
  const { portalContact } = useContext(AppContext);

  const {
    currentStepIndex,
    loadContactInfo,
    loadCountryOpts,
    loadNewBusinessInfo,
    loadStateOrProvince,
    loadPropertyType,
    persistDataOnJsonStore,
    loadPhoneTypes,
    loadStatusCode,
    newBusiness,
    fetchAccessLevelOpt,
    fetchStatusCodeOpt,
    statusCode,
    deleteInfo,
    createRelationContact,
    handleSelectedTab,
    loadCategoryAssets,
    loadChangeReason,
    loadYearsAcquired,
    assetsList,
    saveAssetsInJsonStorage,
    loadImprovementTypes,
    setEmailList,
    setPhoneList,
    setAddressList,
    setContactInfo,
    filesAttach,
  } = useNewBusiness();

  useUpdateEffect(() => {
    console.log(`newBusiness`, newBusiness);
    persistDataOnJsonStore();
  }, [newBusiness]);

  useMount(() => {
    loadContactInfo();
    loadStatusCode();
    loadPropertyType();
    loadCountryOpts();
    loadStateOrProvince();
    loadPhoneTypes();

    // assets info
    loadCategoryAssets();
    loadChangeReason();
    loadYearsAcquired();

    // improvement info
    loadImprovementTypes();

    if (!portalContact?.id) return;
    fetchStatusCodeOpt();
    fetchAccessLevelOpt();
  });

  useEffect(() => {
    loadNewBusinessInfo();
    setContactInformation();
    setContactInfo(portalContact);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [portalContact]);

  useUpdateEffect(() => {
    saveAssetsInJsonStorage();
  }, [assetsList]);

  const renderChildrenStep = (): JSX.Element => {
    const stepChildren = [
      <BasicInfo />, // currentStepIndex is 0
      <Location />, // currentStepIndex is 1
      <Contact />, // currentStepIndex is 2
      <Attach />, // currentStepIndex is 3
      <Assets />, // currentStepIndex is 4
      <Exemptions />, // currentStepIndex is 5
      <Supplies />, // currentStepIndex is 6
      <Improvements />, // currentStepIndex is 7
    ];

    return stepChildren[currentStepIndex] ?? <Fragment />;
  };

  const tabItems = [
    {
      label: fm.basicInfo,
      disable: false,
    },
    {
      label: fm.location,
      disable: false,
    },
    {
      label: fm.contact,
      disable: false,
    },
    {
      label: fm.attach,
      disable: false,
    },
    {
      label: fm.assets,
      disable: false,
    },
    {
      label: fm.exemptions,
      disable: false,
    },
    {
      label: fm.supplies,
      disable: false,
    },
    {
      label: fm.improvements,
      disable: false,
    },
  ];

  // get email, address and phone number
  const setContactInformation = async (): Promise<void> => {
    if (!portalContact) return;

    const { addresses, emails, phones } = await getContactInfo(
      portalContact.id
    );

    setAddressList(addresses);
    setPhoneList(phones);
    setEmailList(emails);
  };

  const renderFinishedStep = (): JSX.Element => {
    if (currentStepIndex !== 7) return <Fragment />; // if the step is not improvement

    return (
      <div className={classes.saveBusinessWrap}>
        <span className={classes.saveBusinessDesc}>
          {fm.whenYouFinishEditing}
        </span>
        <CustomButton
          classes={{ root: classes.saveBusinessButton }}
          ptasVariant="Inverse"
          onClick={handleCreateNewBusiness}
        >
          {fm.saveBusiness}
        </CustomButton>
      </div>
    );
  };

  const handleCreateNewBusiness = async (): Promise<void> => {
    const createFromHome = state?.createFromHome;
    deleteInfo();

    const { hasError } = await businessApiService.addBusiness({
      ...newBusiness,
      statusCode: statusCode.get(STATUS_CODE_UNVERIFIED) ?? 591500007,
    });

    if (createFromHome && portalContact?.id && !hasError) {
      createRelationContact();
    }

    createAssets(newBusiness.id);

    if (!filesAttach.length) return;

    createFileAttachMetadata();
    moveBlobFileToSharePoint();
  };

  const createAssets = (businessId: string): void => {
    if (!businessId) return;

    const assetsToSave = assetsList.map((asset) => ({
      ...asset,
      personalPropertyId: businessId,
    }));

    businessApiService.createAssetBusiness(assetsToSave);
  };

  const createFileAttachMetadata = async (): Promise<void> => {
    const file = filesAttach[0];
    if (!file) return;

    const fileAttach = new FileAttachmentMetadata();
    fileAttach.id = file.id ?? '';
    fileAttach.icsDocumentId = file.id ?? '';
    fileAttach.name = file.fileName;
    fileAttach.isBlob = false;
    fileAttach.isSharePoint = true;
    fileAttach.sharepointUrl = file.content as string;
    fileAttach.documentType = 'Listing';
    fileAttach.filingMethod = await getOptionSetValue(
      'ptas_fileattachmentmetadata',
      'ptas_filingmethod',
      'ezlisting'
    );
    fileAttach.listingStatus = await getOptionSetValue(
      'ptas_fileattachmentmetadata',
      'ptas_listingstatus',
      'Received'
    );
    fileAttach.year =
      (await (
        await businessApiService.getYearsIdByYears(new Date().getFullYear())
      ).data) ?? '';
    fileAttach.loadDate = new Date().toISOString();
    fileAttach.loadBy = process.env.REACT_APP_SYSTEM_USER_ID ?? '';

    businessApiService.saveFileAttachmentData(
      fileAttach,
      newBusiness?.id ?? ''
    );
  };

  const moveBlobFileToSharePoint = (): void => {
    const fileIds = filesAttach.map((fa) => fa.id ?? '');

    apiService.moveBlobStorageToSharePoint(
      fileIds,
      process.env.REACT_APP_BLOB_CONTAINER ?? ''
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
        {/* save business */}
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

export default withNewBusinessProvider(NewBusiness);
