// useHomeImprovement.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  Dispatch,
  SetStateAction,
  useEffect,
  useState,
  useCallback,
  useContext,
  useRef,
} from 'react';
import { useHistory, useParams } from 'react-router-dom';
import {
  ItemSuggestion,
  CustomFile,
  ErrorMessageContext,
} from '@ptas/react-public-ui-library';
import { useMount, useUpdateEffect } from 'react-use';
import { debounce } from 'lodash';
import { apiService } from 'services/api/apiService';
import { hiApiService } from 'services/api/apiService/homeImprovement';
import { AppContext } from '../../../../contexts/AppContext';
import { HomeImprovementApplication } from './model/homeImprovementApplication';
import { Permit } from './model/permit';
import { FileAttachmentMetadata } from './model/fileAttachmentMetadata';
import { v4 as uuid } from 'uuid';
import { Property } from 'services/map/model/parcel';
import {
  APPLICATION_SOURCE_ONLINE,
  STATE_CODE_ACTIVE,
  STATUS_CODE_ADDITIONAL_INFO_NEEDED,
  STATUS_CODE_APP_CREATED,
  STATUS_CODE_COMPLETE,
  STATUS_CODE_UNSUBMITTED,
} from './constants';
import { TaxAccount } from 'services/map/model/taxAccount';
import * as fm from './formatMessage';
import { unionBy } from 'lodash';

export interface UseHomeImprovement {
  handleTabChange: (tab: number) => void;
  handleContinue: () => void;
  handleClickOnInstructions: () => void;
  handleOnSearchChange: (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => void;
  searchList: ItemSuggestion[];
  setSearchList: Dispatch<SetStateAction<ItemSuggestion[]>>;
  parcelData: Property[];
  setParcelData: Dispatch<SetStateAction<Property[]>>;
  applicationList: HomeImprovementApplication[];
  setApplicationList: Dispatch<SetStateAction<HomeImprovementApplication[]>>;
  currentHiApplication: HomeImprovementApplication | undefined;
  setCurrentHiApplication: Dispatch<
    SetStateAction<HomeImprovementApplication | undefined>
  >;
  prevApplication: HomeImprovementApplication | undefined;
  setPrevApplication: Dispatch<
    SetStateAction<HomeImprovementApplication | undefined>
  >;
  onSearchItemSelected: (item: ItemSuggestion) => void;
  currentStep: ApplicationStep;
  setCurrentStep: Dispatch<SetStateAction<ApplicationStep>>;
  highestStepNumber: number;
  setHighestStepNumber: Dispatch<SetStateAction<number>>;
  permits: Permit[];
  setPermits: Dispatch<SetStateAction<Permit[]>>;
  permitNotListed: boolean;
  setPermitNotListed: Dispatch<SetStateAction<boolean>>;
  selectedPermit: string | undefined;
  setSelectedPermit: Dispatch<SetStateAction<string | undefined>>;
  newPermit: Permit | undefined;
  setNewPermit: Dispatch<SetStateAction<Permit | undefined>>;
  loading: boolean;
  setSearchText: Dispatch<SetStateAction<string>>;
  searchText: string;
  updateCurrentHiApplication: (
    fieldName: keyof HomeImprovementApplication,
    value: string | number | Date | boolean | undefined
  ) => void;
  updateNewPermit: (
    fieldName: keyof Permit,
    value: string | number | Date
  ) => void;
  jurisdictionSuggestions: ItemSuggestion[];
  setJurisdictionSuggestions: Dispatch<SetStateAction<ItemSuggestion[]>>;
  onPermitIssuerSearchChange: (value: string) => void;
  fileAttachments: FileAttachmentMetadata[];
  setFileAttachments: Dispatch<SetStateAction<FileAttachmentMetadata[]>>;
  fileUploads: CustomFile[];
  setFileUploads: Dispatch<SetStateAction<CustomFile[]>>;
  statusCodes: Map<string, number>;
  setStatusCodes: Dispatch<SetStateAction<Map<string, number>>>;
  stateCodes: Map<string, number>;
  setStateCodes: Dispatch<SetStateAction<Map<string, number>>>;
  applicationSources: Map<string, number>;
  setApplicationSources: Dispatch<SetStateAction<Map<string, number>>>;
  isApplicationSaved: () => boolean;
  setInitApplicationFields: (
    application: HomeImprovementApplication
  ) => Promise<void>;
  redirectPage: (appId: string, step?: ApplicationStep) => void;
}

export type ApplicationStep = 'construction' | 'permit' | 'sign' | 'new'; //'new' is a special case for creating a new application
export type HomeImprovementParams = {
  contact: string;
  applicationId: string;
  step: ApplicationStep;
  parcelId: string;
};

function useHomeImprovement(): UseHomeImprovement {
  const isMountedRef = useRef<boolean>(true);
  const history = useHistory();
  const {
    contact,
    applicationId: applicationIdParam,
    step: applicationStep,
    parcelId: parcelIdForNewApp,
  } = useParams<HomeImprovementParams>();
  const [applicationId, setApplicationId] = useState<string | undefined>(
    applicationIdParam
  );
  //Ref used to indicate whether the params received by URL have
  // been processed (to open an existing or create a new application)
  const urlParamsProcessed = useRef<boolean>(false);

  const [parcelData, setParcelData] = useState<Property[]>([]);
  const [applicationList, setApplicationList] = useState<
    HomeImprovementApplication[]
  >([]);
  const [
    currentHiApplication,
    setCurrentHiApplication,
  ] = useState<HomeImprovementApplication>();
  const [
    prevApplication,
    setPrevApplication,
  ] = useState<HomeImprovementApplication>();

  const [currentStep, setCurrentStep] = useState<ApplicationStep>(
    'construction'
  );
  const [highestStepNumber, setHighestStepNumber] = useState<number>(0);
  const [permits, setPermits] = useState<Permit[]>([]);
  const [newPermit, setNewPermit] = useState<Permit>();
  const [permitNotListed, setPermitNotListed] = useState<boolean>(false);
  const [selectedPermit, setSelectedPermit] = useState<string>();
  const [loading, setLoading] = useState<boolean>(true);
  const [searchText, setSearchText] = useState<string>('');
  const [searchList, setSearchList] = useState<ItemSuggestion[]>([]);
  const [jurisdictionSuggestions, setJurisdictionSuggestions] = useState<
    ItemSuggestion[]
  >([]);
  const [fileAttachments, setFileAttachments] = useState<
    FileAttachmentMetadata[]
  >([]);
  const [fileUploads, setFileUploads] = useState<CustomFile[]>([]);
  const [statusCodes, setStatusCodes] = useState<Map<string, number>>(
    new Map()
  );
  const [stateCodes, setStateCodes] = useState<Map<string, number>>(new Map());
  const [applicationSources, setApplicationSources] = useState<
    Map<string, number>
  >(new Map());
  //Map with key=year like 2021 and value=ID of ptas_year entity
  const [years, setYears] = useState<Map<number, string>>(new Map());

  const { mediaToken, portalContact } = useContext(AppContext);
  const { setErrorMessageState } = useContext(ErrorMessageContext);

  useMount(() => {
    loadStateCodes();
    loadStatusCodes();
    loadApplicationSources();
    loadYears();
  });

  useEffect(() => {
    return (): void => {
      isMountedRef.current = false;
    };
  }, []);

  useEffect(() => {
    if (portalContact?.id && loadHiApplications) {
      loadHiApplications();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [portalContact?.id]);

  useEffect(() => {
    // setCurrentStep('construction');
    // setHighestStepNumber(0);
    loadPermits();
    loadFileAttachments();
    setSelectedPermit(currentHiApplication?.permitId);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentHiApplication?.homeImprovementId]);

  useUpdateEffect(() => {
    if (!currentHiApplication) return;
    setApplicationList((prev) => {
      return prev.map((app) =>
        app.homeImprovementId === currentHiApplication.homeImprovementId
          ? currentHiApplication
          : app
      );
    });
  }, [currentHiApplication]);

  const loadHiApplications = async (): Promise<void> => {
    if (!portalContact?.id) return;
    const appsRes = await hiApiService.getHomeImprovementAppsByContact(
      portalContact.id
    );
    if (appsRes.data && appsRes.data.length) {
      isMountedRef.current && setApplicationList(appsRes.data);

      const uniqueParcelIds = appsRes.data
        .map((i) => i.parcelId)
        .filter((v, i, a) => a.indexOf(v) === i);
      const parcelsRes = await apiService.getParcels(uniqueParcelIds);
      if (parcelsRes.data) {
        //Set the basic parcel data so it is displayed while
        // the images and tax account data is fetched
        isMountedRef.current &&
          setParcelData((prev) => {
            //This is necessary for cases when a new HI application
            // is created from URL params. Otherwise the parcel data
            // for that new application might be lost
            const joinedParcels = joinParcelData(prev, [
              ...(parcelsRes.data ?? []),
            ]);
            return joinedParcels;
          });

        const parcels = parcelsRes.data;
        //Get parcel pictures
        const parcelPicturesRes = await Promise.all(
          parcels.map((el) =>
            apiService.getMediaForParcel(el.ptas_name, mediaToken)
          )
        );
        //Get tax accounts
        const taxAccountsRes = await Promise.all(
          parcels.map((el) => {
            if (!el._ptas_taxaccountid_value) {
              return undefined;
            }
            return apiService.getTaxAccount(el._ptas_taxaccountid_value);
          })
        );

        parcels.forEach((parcel, i) => {
          if (parcelPicturesRes[i]?.data?.length) {
            parcel.picture = parcelPicturesRes[i].data?.[0] ?? '';
          }

          if (parcel._ptas_taxaccountid_value && taxAccountsRes[i]?.data) {
            parcel.taxAccountData = taxAccountsRes[i]?.data;
          }
        });

        isMountedRef.current &&
          setParcelData((prev) => {
            //This is necessary for cases when a new HI application
            // is created from URL params. Otherwise the parcel data
            // for that new application might be lost
            const joinedParcels = joinParcelData(prev, [...(parcels ?? [])]);
            return joinedParcels;
          });
      }
    }
  };

  const loadStateCodes = async (): Promise<void> => {
    const { data: stateCodes } = await apiService.getOptionSet(
      'ptas_homeimprovement',
      'statecode'
    );
    const map = new Map<string, number>(
      stateCodes?.map((c) => [c.value, c.attributeValue])
    );
    setStateCodes(map);
  };

  const loadStatusCodes = async (): Promise<void> => {
    const { data: statusCodes } = await apiService.getOptionSet(
      'ptas_homeimprovement',
      'statuscode'
    );
    //TODO: Temp. using the first status "Application created" (which is repeated)
    // as the status "Unsubmitted"
    // **Remember to also use the right status code on setInitApplicationFields and
    // on CardPropertyInfo/More (function stateCodeActiveHI)
    const statusCodeUnsubmitted = statusCodes?.find(
      (el) => el.value === 'Application created'
    );
    if (statusCodeUnsubmitted) {
      statusCodeUnsubmitted.value = 'Unsubmitted';
    }
    const map = new Map<string, number>(
      statusCodes?.map((c) => [c.value, c.attributeValue])
    );
    setStatusCodes(map);
  };

  const loadApplicationSources = async (): Promise<void> => {
    const { data: applicationSources } = await apiService.getOptionSet(
      'ptas_homeimprovement',
      'ptas_applicationsource'
    );
    const map = new Map<string, number>(
      applicationSources?.map((c) => [c.value, c.attributeValue])
    );
    setApplicationSources(map);
  };

  const loadYears = async (): Promise<void> => {
    const yearsRes = await apiService.getYears();
    if (yearsRes.data && yearsRes.data.length) {
      const yearsMap = new Map<number, string>(
        yearsRes.data.map((el) => [
          Number.parseInt(el.ptas_name),
          el.ptas_yearid,
        ])
      );
      setYears(yearsMap);
    } else {
      console.error('Error on load years:', yearsRes.errorMessage);
    }
  };

  useEffect(() => {
    //Process received URL params
    console.debug(
      'useHomeImprovement effect for URL params.',
      'portalContact:',
      portalContact?.id,
      'parcelData:',
      parcelData,
      'contact:',
      contact,
      'applicationId:',
      applicationId,
      'applicationStep:',
      applicationStep,
      'parcelIdForNewApp:',
      parcelIdForNewApp,
      'applicationList:',
      applicationList
    );

    if (!portalContact?.id) return;
    if (!applicationId) return;
    if (contact && contact !== portalContact.id) {
      return;
    }
    if (urlParamsProcessed.current) return;

    if (applicationStep === 'new' && parcelIdForNewApp) {
      urlParamsProcessed.current = true;
      console.debug('Processing new application from URL params');
      //TODO: Check if parcel data with parcelIdForNewApp

      //Create new home improvement application
      newHomeImprovementApplication(applicationId, parcelIdForNewApp);
      return;
    }

    //Parcel data might not be loaded yet. No need to validate applicationList,
    // since parcel data is loaded after the applications.
    if (!parcelData || !parcelData.length) {
      console.error('Missing parcel data. applicationId:', applicationId);
      return;
    }
    urlParamsProcessed.current = true;
    setApplicationId(undefined);
    const application = applicationList.find(
      (i) => i.homeImprovementId === applicationId
    );
    if (application) {
      setCurrentHiApplication(application);
    } else {
      console.error('HI Application not found:', applicationId);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    contact,
    applicationId,
    applicationStep,
    parcelIdForNewApp,
    applicationList,
    portalContact,
    parcelData,
  ]);

  const updateCurrentHiApplication = (
    fieldName: keyof HomeImprovementApplication,
    value: string | number | Date | boolean | undefined
  ): void => {
    setCurrentHiApplication((prev) => {
      if (!prev) return;
      return {
        ...prev,
        [fieldName]: value,
      };
    });
  };

  const updateNewPermit = (
    fieldName: keyof Permit,
    value: string | number | Date
  ): void => {
    setNewPermit((prev) => {
      if (!prev) {
        const permit = new Permit();
        return {
          ...permit,
          [fieldName]: value,
        };
      }
      return {
        ...prev,
        [fieldName]: value,
      };
    });
  };

  const handleTabChange = (tab: number): void => {
    if (tab === 1) {
      setCurrentStep('permit');
    } else if (tab === 2) {
      setCurrentStep('sign');
    } else {
      setCurrentStep('construction');
    }
  };

  const isApplicationSaved = (): boolean => {
    return currentHiApplication?.isSaved ?? false;
  };

  const isPermitSaved = (): boolean => {
    return !!newPermit?.permitId;
  };

  const handleContinue = async (): Promise<void> => {
    if (!currentHiApplication) return;
    const savingApp = { ...currentHiApplication };

    if (currentStep === 'construction') {
      setCurrentStep('permit');
      if (highestStepNumber < 1) setHighestStepNumber(1);

      //#region construction tab
      if (!isApplicationSaved()) {
        setInitApplicationFields(savingApp);
      }
      const saveRes = await hiApiService.saveHiApplication(
        savingApp,
        prevApplication
      );
      if (saveRes.hasError) {
        console.error(
          'Error saving home improvement application.',
          saveRes.errorMessage,
          savingApp
        );
        setErrorMessageState({
          open: true,
          errorHead: fm.saveApplicationError,
        });
        return;
      } else {
        savingApp.isSaved = true;
        setPrevApplication(savingApp);
        setCurrentHiApplication(savingApp);
      }
      //#endregion
    } else if (currentStep === 'permit') {
      setCurrentStep('sign');
      if (highestStepNumber < 2) setHighestStepNumber(2);

      //#region permit tab
      let shouldSaveApp = false;
      if (newPermit && !isPermitSaved() && !selectedPermit) {
        //New permit
        const savingPermit = { ...newPermit };
        savingPermit.permitId = uuid();
        savingPermit.parcelId = currentHiApplication.parcelId;
        const savePermitRes = await apiService.savePermit(savingPermit);
        if (savePermitRes.hasError) {
          console.error('Error saving permit.', savePermitRes.errorMessage);
          setErrorMessageState({
            open: true,
            errorHead: fm.savePermitError,
          });
        } else {
          setNewPermit(undefined);
          setPermitNotListed(false);
          setPermits((prev) => [...prev, savingPermit]);
          setSelectedPermit(savingPermit.permitId);
          //Set permit fields on HI application
          savingApp.permitId = savingPermit.permitId;
          savingApp.permitJurisdictionId = savingPermit.issuedById;
          savingApp.datePermitIssued = savingPermit.issuedDate ?? new Date();
          shouldSaveApp = true;
        }
      } else if (selectedPermit) {
        //Existing permit
        const currentPermit = permits.find(
          (permit) => permit.permitId === selectedPermit
        );
        if (currentPermit && currentPermit.permitId !== savingApp.permitId) {
          //Set permit fields on HI application
          savingApp.permitId = currentPermit.permitId;
          savingApp.permitJurisdictionId = currentPermit.issuedById;
          savingApp.datePermitIssued = currentPermit.issuedDate ?? new Date();
          shouldSaveApp = true;
        }
      }

      if (shouldSaveApp) {
        const saveRes = await hiApiService.saveHiApplication(
          savingApp,
          prevApplication
        );
        if (saveRes.hasError) {
          console.error(
            'Error saving home improvement application.',
            saveRes.errorMessage,
            savingApp
          );
          setErrorMessageState({
            open: true,
            errorHead: fm.saveApplicationError,
          });
          return;
        } else {
          setPrevApplication(savingApp);
          setCurrentHiApplication(savingApp);
        }
      }
      //#endregion
    } else if (currentStep === 'sign') {
      //#region tab sign
      const parcel = parcelData.find(
        (p) => p.ptas_parceldetailid === savingApp.parcelId
      );

      if (!parcel) return;

      //Set application date on HI application
      savingApp.dateApplicationReceived = new Date();
      //Update exemption begin year
      const currentYear = new Date().getFullYear();
      savingApp.exemptionBeginYearId = years.get(currentYear) ?? '';
      savingApp.exemptionBeginYear = currentYear;
      //Set HI application signed by taxpayer
      savingApp.signedByTaxpayer = !!savingApp.taxpayerName;
      //Set state and status codes on HI application
      savingApp.stateCode = stateCodes.get(STATE_CODE_ACTIVE) ?? null;
      savingApp.statusCode = statusCodes.get(STATUS_CODE_APP_CREATED) ?? null;

      const saveAppRes = await hiApiService.saveHiApplication(
        savingApp,
        prevApplication
      );
      if (saveAppRes.hasError) {
        console.error(
          'Error saving home improvement application.',
          saveAppRes.errorMessage
        );
        setErrorMessageState({
          open: true,
          errorHead: fm.saveApplicationError,
        });
        savingApp.statusCode = 0; //So it won't show as "application created"
        return;
      }

      //Move attached files to sharepoint
      moveBlobToSharePoint(
        currentHiApplication.homeImprovementId,
        fileAttachments
      ).then(() => {
        setFileAttachments([]);
        setFileUploads([]);
      });

      //Delete route from json store
      const { hasError: jsonError } = await apiService.deleteJson(
        `portals/home-improvement/${portalContact?.id}/${savingApp.homeImprovementId}/step/step`
      );
      if (jsonError) {
        console.error('Error to delete json');
      }

      //Clear selected HI application and reset current step
      setApplicationId(undefined);
      setHighestStepNumber(0);
      setCurrentStep('construction');
      setCurrentHiApplication(undefined);
      setPrevApplication(undefined);
      redirectPage(undefined);

      //Keep list of applications updated
      const newHiAppList = applicationList.map((app) =>
        app.homeImprovementId === savingApp.homeImprovementId ? savingApp : app
      );
      setApplicationList(newHiAppList);
      //#endregion
    }
  };

  const setInitApplicationFields = async (
    application: HomeImprovementApplication
  ): Promise<void> => {
    const parcel = parcelData.find(
      (p) => p.ptas_parceldetailid === application?.parcelId
    );
    if (!application || !parcel) return;

    //Set portal contact on HI application
    application.portalContactId = portalContact?.id ?? '';
    //Set tax account ID  on HI application
    application.taxAccountId = parcel._ptas_taxaccountid_value;
    //Set property and mailing addresses
    application.constructionPropertyAddress = parcel.ptas_address;
    application.compositeMailingAddress = parcel.ptas_address;
    //Set HI application source to 'Online'
    application.applicationSource =
      applicationSources.get(APPLICATION_SOURCE_ONLINE) ?? null;
    //Set exemption begin year
    const currentYear = new Date().getFullYear();
    application.exemptionBeginYearId = years.get(currentYear) ?? '';
    application.exemptionBeginYear = currentYear;
    //Set status code "Unsubmitted"
    application.stateCode = stateCodes.get(STATE_CODE_ACTIVE) ?? null;
    application.statusCode = statusCodes.get(STATUS_CODE_UNSUBMITTED) ?? null;
  };

  const handleOnSearchChange = (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ): void => {
    const inputValue = e.currentTarget.value;
    setSearchText(inputValue);
  };

  /**
   * Find property
   */
  const searchProperty = useCallback(
    debounce(async (valueData) => {
      const { data, hasError } = await apiService.getParcel(valueData);

      if (hasError || !data) {
        setSearchList([]);
        return;
      }

      try {
        const itemSuggestionData = data?.map((propertyItem) => {
          return {
            title: propertyItem.ptas_address,
            id: propertyItem.ptas_parceldetailid,
            subtitle: `${propertyItem.ptas_district}, WA ${
              propertyItem.ptas_zipcode ?? ''
            }`,
          };
        });

        setLoading(false);

        setSearchList(itemSuggestionData ?? []);
      } catch (e) {
        console.error('Error on searchProperty:', e);
      }
    }, 500),
    []
  );

  useUpdateEffect(() => {
    searchProperty(searchText);
  }, [searchText]);

  const onSearchItemSelected = async (
    parcelItem: ItemSuggestion
  ): Promise<void> => {
    if (!parcelItem.id) return;

    const {
      data: parcelApps,
    } = await hiApiService.getHomeImprovementAppByParcel(
      parcelItem.id as string
    );

    //#region Active apps
    const activeApps = parcelApps?.filter(
      (app): boolean => app.stateCode === stateCodes.get(STATE_CODE_ACTIVE)
    );

    if (activeApps?.length) {
      //App that have not been submitted or need more info
      const incompleteApp = activeApps.find(
        (app): boolean =>
          app.statusCode === statusCodes.get(STATUS_CODE_UNSUBMITTED) ||
          app.statusCode === statusCodes.get(STATUS_CODE_ADDITIONAL_INFO_NEEDED)
      );
      if (incompleteApp) {
        if (currentHiApplication?.parcelId === parcelItem.id) {
          //Already being edited
          return;
        }
        if (incompleteApp.portalContactId === portalContact?.id) {
          //Edit existing incomplete HI application
          setCurrentHiApplication(incompleteApp);
          setPrevApplication({ ...incompleteApp });
          return;
        } else {
          //There is an incomplete HI application for this parcel,
          //created by another contact
          setErrorMessageState({
            open: true,
            errorHead: fm.activeApplicationByAnotherContact,
          });
          return;
        }
      }

      //Check existence of approved/complete apps in the last 5 years
      const recentCompleteApp = activeApps.find((app): boolean => {
        const yearDiff = new Date().getFullYear() - app.exemptionBeginYear;
        return (
          app.statusCode === statusCodes.get(STATUS_CODE_COMPLETE) &&
          yearDiff <= 5
        );
      });
      if (recentCompleteApp) {
        setErrorMessageState({
          open: true,
          errorHead: fm.exemption5Years,
        });
        return;
      }

      //Active HI app that cannot be edited (because of its status)
      setErrorMessageState({
        open: true,
        errorHead: fm.activeApplication,
      });
      return;
    }
    //#endregion

    // New HI application
    //Set empty HI application so the UI gets updated
    //while the data is loaded from the API
    const tempApp = new HomeImprovementApplication();
    tempApp.name = 'temp_app';
    setCurrentHiApplication(tempApp);
    setApplicationList((prev) => {
      //Remove any unsaved HI application
      const savedApps = prev.filter((i) => i.isSaved);
      return [...savedApps, tempApp];
    });

    //Read parcel info
    const { data: parcels } = await apiService.getParcels([
      parcelItem.id as string,
    ]);
    if (!parcels || !parcels.length) return;
    const parcel = parcels[0];

    //Parcel picture
    const { data: parcelPictures } = await apiService.getMediaForParcel(
      parcel.ptas_name,
      mediaToken
    );
    const parcelPicture = parcelPictures?.length ? parcelPictures[0] : '';

    //Tax account
    let taxAccount: TaxAccount | undefined;
    if (parcel._ptas_taxaccountid_value) {
      const { data: taxAccountData } = await apiService.getTaxAccount(
        parcel._ptas_taxaccountid_value
      );
      taxAccount = taxAccountData;
    }

    //Update parcel data
    setParcelData((prev) => {
      const newParcels = [
        ...prev,
        {
          ...parcel,
          picture: parcelPicture,
          taxAccountData: taxAccount,
        },
      ];
      return newParcels;
    });

    //Create new home improvement application
    const hiApp = new HomeImprovementApplication();
    setPrevApplication({ ...hiApp });
    hiApp.parcelId = parcel.ptas_parceldetailid;
    hiApp.emailAddress =
      parcel.taxAccountData?.ptas_email ?? portalContact?.email?.email ?? '';
    hiApp.phoneNumber =
      parcel.taxAccountData?.ptas_phone1 ??
      portalContact?.phone.phoneNumber ??
      '';
    hiApp.statusCode = statusCodes.get(STATUS_CODE_UNSUBMITTED) ?? null;
    setApplicationList((prev) => {
      const prevApps = prev.filter((i) => i.name !== 'temp_app');
      return [...prevApps, hiApp];
    });
    setCurrentHiApplication(hiApp);
    setCurrentStep('construction');
    setHighestStepNumber(0);
  };

  const newHomeImprovementApplication = async (
    applicationId: string,
    parcelId: string
  ): Promise<void> => {
    //Set empty HI application so the UI gets updated
    //while the data is loaded from the API
    const tempApp = new HomeImprovementApplication();
    tempApp.name = 'temp_app';
    tempApp.homeImprovementId = applicationId;
    setCurrentHiApplication(tempApp);
    setApplicationList((prev) => {
      //Remove any unsaved HI application
      const savedApps = prev.filter((i) => i.isSaved);
      return [...savedApps, tempApp];
    });

    //Read parcel info
    const { data: parcels } = await apiService.getParcels([parcelId]);
    if (!parcels || !parcels.length) return;
    const parcel = parcels[0];

    //Parcel picture
    const { data: parcelPictures } = await apiService.getMediaForParcel(
      parcel.ptas_name,
      mediaToken
    );
    const parcelPicture = parcelPictures?.length ? parcelPictures[0] : '';

    //Tax account
    let taxAccount: TaxAccount | undefined;
    if (parcel._ptas_taxaccountid_value) {
      const { data: taxAccountData } = await apiService.getTaxAccount(
        parcel._ptas_taxaccountid_value
      );
      taxAccount = taxAccountData;
    }

    //Update parcel data
    setParcelData((prev) => {
      const newParcels = [
        ...prev,
        {
          ...parcel,
          picture: parcelPicture,
          taxAccountData: taxAccount,
        },
      ];
      return newParcels;
    });

    //Create new home improvement application
    const hiApp = new HomeImprovementApplication();
    hiApp.homeImprovementId = applicationId; //Keep the application id received as param
    setPrevApplication({ ...hiApp });
    hiApp.parcelId = parcel.ptas_parceldetailid;
    hiApp.emailAddress =
      parcel.taxAccountData?.ptas_email ?? portalContact?.email?.email ?? '';
    hiApp.phoneNumber =
      parcel.taxAccountData?.ptas_phone1 ??
      portalContact?.phone.phoneNumber ??
      '';
    hiApp.statusCode = statusCodes.get(STATUS_CODE_UNSUBMITTED) ?? null;
    setApplicationList((prev) => {
      const prevApps = prev.filter((i) => i.name !== 'temp_app');
      return [...prevApps, hiApp];
    });
    setCurrentHiApplication(hiApp);
    setCurrentStep('construction');
    setHighestStepNumber(0);
  };

  const loadPermits = async (): Promise<void> => {
    if (!currentHiApplication?.parcelId) return;
    //Read permits for parcel
    const permitsRes = await apiService.getPermitsByParcel(
      currentHiApplication.parcelId
    );
    if (permitsRes.data) {
      setPermits(permitsRes.data);
    } else {
      setPermits([]);
    }
  };

  const loadFileAttachments = async (): Promise<void> => {
    if (!currentHiApplication?.isSaved) {
      //If HI app is not saved, it cannot have file attachments
      setFileAttachments([]);
      setFileUploads([]);
      return;
    }
    //Read existing file attachments
    const fileAttachmentsRes = await apiService.getFileAttachmentsByHiApplication(
      currentHiApplication.homeImprovementId
    );
    if (fileAttachmentsRes.data?.length) {
      setFileAttachments(fileAttachmentsRes.data);
      const uploads: CustomFile[] = fileAttachmentsRes.data.map((f) => ({
        id: f.fileAttachmentMetadataId,
        contentType: 'publicUrl',
        content: f.isBlob ? f.blobUrl : f.sharepointUrl,
        fileName: f.name,
      }));
      setFileUploads(uploads);
    } else {
      setFileAttachments([]);
      setFileUploads([]);
    }
  };

  const moveBlobToSharePoint = async (
    hiApplicationId: string,
    files: FileAttachmentMetadata[]
  ): Promise<void> => {
    //The file id is composed of the HI application ID and the file attachment entity ID
    //so the files are grouped by application
    const fileIds = files.map(
      (fd) => hiApplicationId + '/' + fd.fileAttachmentMetadataId
    );

    const {
      data: sharepointData,
      hasError: sharepointError,
    } = await apiService.saveFileInSharePoint(
      fileIds,
      process.env.REACT_APP_BLOB_CONTAINER_HI ?? ''
    );

    console.log(`Sharepoint data`, sharepointData);

    if (sharepointError) {
      return console.error('Error to move blob to sharepoint');
    }

    if (!sharepointData) return;

    for (const updatedFile of sharepointData) {
      const updatedFileAttachment = new FileAttachmentMetadata();
      updatedFileAttachment.isBlob = false;
      updatedFileAttachment.isSharePoint = true;
      updatedFileAttachment.sharepointUrl = updatedFile.fileUrl;
      updatedFileAttachment.fileAttachmentMetadataId = updatedFile.id.split(
        '/'
      )[1];

      const { hasError } = await apiService.updateFileAttachmentMetadata(
        updatedFileAttachment
      );
      if (hasError) {
        console.error('error to update fileattachment');
      }
    }
  };

  const handleClickOnInstructions = (): void => {
    history.push('/instruction');
  };

  const onPermitIssuerSearchChange = async (value: string): Promise<void> => {
    if (!value) {
      //Clear selected jurisdiction
      updateNewPermit('issuedById', '');
      return;
    }
    const jurisdictionRes = await apiService.searchJurisdiction(value);
    if (jurisdictionRes.data?.length) {
      const list = jurisdictionRes.data.map((item) => ({
        id: item.id,
        title: item.name,
        subtitle: '',
      }));
      setJurisdictionSuggestions(list);
    }
  };

  const joinParcelData = (
    currentParcels: Property[],
    newParcels: Property[]
  ): Property[] => {
    if (!currentParcels.length) return newParcels;
    if (!newParcels.length) return currentParcels;

    return unionBy<Property>(newParcels, currentParcels, 'ptas_parceldetailid');
  };

  const redirectPage = (
    appId: string | undefined,
    step?: ApplicationStep
  ): void => {
    if (!portalContact?.id) return;
    console.debug(`URL management redirectPage appId: ${appId}. step: ${step}`);

    if (portalContact.id && appId && (step || currentStep)) {
      window.history.replaceState(
        null,
        '',
        `/home-improvement/${portalContact.id}/${appId}/${step ?? currentStep}`
      );
    } else if (portalContact.id && appId) {
      window.history.replaceState(
        null,
        '',
        `/home-improvement/${portalContact.id}/${appId}`
      );
    } else if (portalContact.id) {
      window.history.replaceState(null, '', `/home-improvement`);
    }
  };

  return {
    handleTabChange,
    handleContinue,
    handleClickOnInstructions,
    handleOnSearchChange,
    searchList,
    setSearchList,
    parcelData,
    setParcelData,
    applicationList,
    setApplicationList,
    currentHiApplication,
    setCurrentHiApplication,
    prevApplication,
    setPrevApplication,
    onSearchItemSelected,
    permits,
    setPermits,
    permitNotListed,
    setPermitNotListed,
    currentStep,
    setCurrentStep,
    highestStepNumber,
    setHighestStepNumber,
    selectedPermit,
    setSelectedPermit,
    newPermit,
    setNewPermit,
    loading,
    searchText,
    setSearchText,
    updateCurrentHiApplication,
    updateNewPermit,
    jurisdictionSuggestions,
    setJurisdictionSuggestions,
    onPermitIssuerSearchChange,
    fileAttachments,
    setFileAttachments,
    fileUploads,
    setFileUploads,
    statusCodes,
    setStatusCodes,
    stateCodes,
    setStateCodes,
    applicationSources,
    setApplicationSources,
    isApplicationSaved,
    setInitApplicationFields,
    redirectPage,
  };
}

export default useHomeImprovement;
