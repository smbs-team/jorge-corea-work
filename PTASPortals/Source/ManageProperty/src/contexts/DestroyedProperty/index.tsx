// AppContext.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  useState,
  PropsWithChildren,
  FC,
  createContext,
  Dispatch,
  SetStateAction,
} from 'react';
import { Task } from 'routes/models/Views/HomeDestroyedProperty/Task';
import { FileAttachmentMetadataTask } from 'routes/models/Views/HomeDestroyedProperty/fileAttachment';
import { CustomFile, ItemSuggestion } from '@ptas/react-public-ui-library';
import { OptionSetValue } from '../../routes/models/Views/HomeImprovement/model/optionSet';
import { Property } from 'services/map/model/parcel';

type FileSection = 'fire' | 'natural disaster' | 'voluntary demolition';
type destructionFileType = {
  name: FileSection;
  file: CustomFile[];
};

export interface DestroyedPropContextProps {
  task: Task | undefined;
  setTask: Dispatch<SetStateAction<Task>>;
  stateCode: Map<string, number>;
  setOldDesPropertyApp: Dispatch<SetStateAction<Task>>;
  oldDesPropertyApp: Task | undefined;
  statusCode: Map<string, number>;
  setFileAttachmentMetadataDestruction: Dispatch<
    SetStateAction<FileAttachmentMetadataTask[]>
  >;
  filesAttachmentMetadataDestruction: FileAttachmentMetadataTask[];
  fileAttachmentDoc: FileAttachmentMetadataTask[];
  jsonSteps: object;
  setJsonSteps: Dispatch<SetStateAction<object>>;
  setFileAttachmentDoc: Dispatch<SetStateAction<FileAttachmentMetadataTask[]>>;
  setStateCode: Dispatch<SetStateAction<Map<string, number>>>;
  setStatusCode: Dispatch<SetStateAction<Map<string, number>>>;
  // card property info
  contactId: string | undefined;
  setContactId: Dispatch<SetStateAction<string | undefined>>;
  currentExemptionId: string | undefined;
  setCurrentExemptionId: Dispatch<SetStateAction<string | undefined>>;
  continueButtonDisabled: boolean;
  setContinueButtonDisabled: Dispatch<SetStateAction<boolean>>;
  // destruction tab
  propDestroyedReason: OptionSetValue[];
  setPropDestroyedReason: Dispatch<SetStateAction<OptionSetValue[]>>;
  filePreview: CustomFile | undefined;
  setFilePreview: Dispatch<SetStateAction<CustomFile | undefined>>;
  openPreviewFileModal: boolean;
  setOpenPreviewFileModal: Dispatch<SetStateAction<boolean>>;
  fireFile: CustomFile[];
  setFireFile: Dispatch<SetStateAction<CustomFile[]>>;
  naturalDisasterFile: CustomFile[];
  setNaturalDisasterFile: Dispatch<SetStateAction<CustomFile[]>>;
  dateOfDestructionInputError: boolean;
  setDateOfDestructionInputError: Dispatch<SetStateAction<boolean>>;
  lossOccurringAsAResultOfError: boolean;
  setLossOccurringAsAResultOf: Dispatch<SetStateAction<boolean>>;
  jurisdictionSuggestions: ItemSuggestion[];
  setJurisdictionSuggestions: Dispatch<SetStateAction<ItemSuggestion[]>>;
  propDescriptionInputError: boolean;
  setPropDescriptionInputError: Dispatch<SetStateAction<boolean>>;
  emailInputError: boolean;
  setEmailInputError: Dispatch<SetStateAction<boolean>>;
  phoneInputError: boolean;
  setPhoneInputError: React.Dispatch<SetStateAction<boolean>>;
  destroyedPropertyList: Task[];
  setDestroyedPropertyList: Dispatch<SetStateAction<Task[]>>;
  initialApps: Task[];
  setInitialApps: Dispatch<SetStateAction<Task[]>>;
  parcelInfo: Property[];
  setParcelInfo: Dispatch<SetStateAction<Property[]>>;
  fireCheck: boolean;
  setFireCheck: Dispatch<SetStateAction<boolean>>;
  naturalDisasterCheck: boolean;
  setNaturalDisasterCheck: Dispatch<SetStateAction<boolean>>;
  voluntaryDemolitionCheck: boolean;
  setVoluntaryDemolitionCheck: Dispatch<SetStateAction<boolean>>;
  // file state
  fileLibraryList: Map<string, number>;
  setFileLibraryList: Dispatch<SetStateAction<Map<string, number>>>;
}

export const DestroyedPropContext = createContext<DestroyedPropContextProps>(
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  null as any
);

/**
 * A Context provider component
 *
 * @param props - Component children
 */

const DestroyedPropProvider = (props: PropsWithChildren<{}>): JSX.Element => {
  const [taskEntity, setTaskEntity] = useState<Task>(new Task());
  const [stateCode, setStateCode] = useState<Map<string, number>>(new Map());
  const [statusCode, setStatusCode] = useState<Map<string, number>>(new Map());
  const [oldDesPropertyApp, setOldDesPropertyApp] = useState<Task>(new Task());
  /** save file from destruction tab */
  const [
    filesAttachmentMetadataDestruction,
    setFileAttachmentMetadataDestruction,
  ] = useState<FileAttachmentMetadataTask[]>([]);
  // file attachment After Submission
  const [fileAttachmentDoc, setFileAttachmentDoc] = useState<
    FileAttachmentMetadataTask[]
  >([]);
  // status to avoid repeated data storage
  const [jsonSteps, setJsonSteps] = useState<object>({});
  const [contactId, setContactId] = useState<string>();
  const [currentExemptionId, setCurrentExemptionId] = useState<string>();

  // bock continue button
  const [continueButtonDisabled, setContinueButtonDisabled] = useState<boolean>(
    true
  );
  //#region destruction tab state

  const [propDestroyedReason, setPropDestroyedReason] = useState<
    OptionSetValue[]
  >([]);
  const [filePreview, setFilePreview] = useState<CustomFile>();
  const [openPreviewFileModal, setOpenPreviewFileModal] = useState<boolean>(
    false
  );
  const [fireFile, setFireFile] = useState<CustomFile[]>([]);
  const [naturalDisasterFile, setNaturalDisasterFile] = useState<CustomFile[]>(
    []
  );

  const [
    dateOfDestructionInputError,
    setDateOfDestructionInputError,
  ] = useState<boolean>(false);
  const [
    lossOccurringAsAResultOfError,
    setLossOccurringAsAResultOf,
  ] = useState<boolean>(false);
  //#endregion destruction tab state
  const [jurisdictionSuggestions, setJurisdictionSuggestions] = useState<
    ItemSuggestion[]
  >([]);
  const [
    propDescriptionInputError,
    setPropDescriptionInputError,
  ] = useState<boolean>(false);
  //#region property tab state

  //#region sign tab states
  const [emailInputError, setEmailInputError] = useState<boolean>(false);
  const [phoneInputError, setPhoneInputError] = useState<boolean>(false);
  //#endregion sign tab states
  const [destroyedPropertyList, setDestroyedPropertyList] = useState<Task[]>(
    []
  );
  const [initialApps, setInitialApps] = useState<Task[]>([]);
  const [parcelInfo, setParcelInfo] = useState<Property[]>([]);
  const [fireCheck, setFireCheck] = useState<boolean>(false);
  const [naturalDisasterCheck, setNaturalDisasterCheck] = useState<boolean>(
    false
  );
  const [
    voluntaryDemolitionCheck,
    setVoluntaryDemolitionCheck,
  ] = useState<boolean>(false);

  const [fileLibraryList, setFileLibraryList] = useState<Map<string, number>>(
    new Map()
  );

  return (
    <DestroyedPropContext.Provider
      value={{
        setTask: setTaskEntity,
        stateCode,
        setOldDesPropertyApp,
        oldDesPropertyApp,
        statusCode,
        filesAttachmentMetadataDestruction,
        setFileAttachmentMetadataDestruction,
        fileAttachmentDoc,
        jsonSteps,
        setJsonSteps,
        setFileAttachmentDoc,
        task: taskEntity,
        setStateCode,
        setStatusCode,
        contactId,
        setContactId,
        continueButtonDisabled,
        setContinueButtonDisabled,
        currentExemptionId,
        setCurrentExemptionId,
        filePreview,
        setFilePreview,
        openPreviewFileModal,
        setOpenPreviewFileModal,
        propDestroyedReason,
        setPropDestroyedReason,
        dateOfDestructionInputError,
        setDateOfDestructionInputError,
        lossOccurringAsAResultOfError,
        setLossOccurringAsAResultOf,
        jurisdictionSuggestions,
        setJurisdictionSuggestions,
        propDescriptionInputError,
        setPropDescriptionInputError,
        emailInputError,
        setEmailInputError,
        phoneInputError,
        setPhoneInputError,
        destroyedPropertyList,
        setDestroyedPropertyList,
        parcelInfo,
        setParcelInfo,
        fireCheck,
        setFireCheck,
        naturalDisasterCheck,
        setNaturalDisasterCheck,
        voluntaryDemolitionCheck,
        setVoluntaryDemolitionCheck,
        fileLibraryList,
        setFileLibraryList,
        fireFile,
        setFireFile,
        naturalDisasterFile,
        setNaturalDisasterFile,
        initialApps,
        setInitialApps,
      }}
    >
      {props.children}
    </DestroyedPropContext.Provider>
  );
};

export const withDestroyedPropProvider = (Component: FC) => (
  props: object
): JSX.Element => (
  <DestroyedPropProvider>
    <Component {...props} />
  </DestroyedPropProvider>
);
