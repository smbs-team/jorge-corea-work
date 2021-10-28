// useHome.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  Dispatch,
  SetStateAction,
  useContext,
  useEffect,
  useState,
} from 'react';
import { PersonalProperty } from 'models/personalProperty';
import { businessContactApiService } from 'services/api/apiService/businessContact';
import { AppContext } from 'contexts/AppContext';
import { PersonalPropertyAccountAccess } from 'models/personalPropertyAccountAccess';
import { businessApiService } from 'services/api/apiService/business';
import { useHistory } from 'react-router-dom';
import { PortalContact } from 'models/portalContact';
import { apiService } from 'services/api/apiService';
import { LEVEL_EDIT } from './constants';
import { useAsync } from 'react-use';

interface UseHome {
  businessList: PersonalProperty[];
  businessContactList: PersonalPropertyAccountAccess[];
  peopleList: PortalContact[];
  setPeopleList: Dispatch<SetStateAction<PortalContact[]>>;
  handleAddBusiness: () => void;
  handleAddPerson: () => void;
  addBusiness: boolean;
  setAddBusiness: Dispatch<boolean>;
  isStaffUser: boolean;
  setIsStaffUser: Dispatch<boolean>;
  wcmPopoverAnchor: HTMLButtonElement | undefined;
  onWhoCanManageClick: (
    businessId: string
  ) => (e: React.MouseEvent<HTMLButtonElement>) => void;
  closeWhoCanManagePopover: () => void;
  fetchBusinessByContact: () => Promise<void>;
  onManageBusinessClick: (businessId: string) => () => void;
  contactsByBusinessSelect: PortalContact[];
  editAccessLevelId: number | undefined;
}

function useHome(): UseHome {
  const [levelOpts, setLevelOpts] = useState<Map<string, number>>(new Map());
  const [businessContactList, setBusinessContactList] = useState<
    PersonalPropertyAccountAccess[]
  >([]);
  const [peopleList, setPeopleList] = useState<PortalContact[]>([]);
  const [addBusiness, setAddBusiness] = useState<boolean>(false);
  const [isStaffUser, setIsStaffUser] = useState<boolean>(false);
  const [wcmPopoverAnchor, setWcmPopoverAnchor] =
    useState<HTMLButtonElement | undefined>();
  const [contactsByBusiness, setContactsByBusiness] = useState<
    Map<string, PortalContact[]>
  >(new Map());
  const [contactsByBusinessSelect, setContactsByBusinessSelect] = useState<
    PortalContact[]
  >([]);
  const { portalContact } = useContext(AppContext);
  const history = useHistory();

  useEffect(() => {
    fetchAccessLevelOpt();
  }, []);

  //Refresh people list
  useAsync(async () => {
    if (
      !businessContactList ||
      businessContactList.length === 0 ||
      !portalContact
    ) {
      setPeopleList([]);
      return;
    }

    const { data: contactsMap } =
      await businessContactApiService.getContactsWithBusinessAccess(
        businessContactList.map((el) => el.personalPropertyId)
      );
    if (contactsMap) {
      const contactsWithAccess = Array.from(contactsMap.values());
      console.log('contactsWithAccess:', contactsWithAccess);
      setPeopleList(
        contactsWithAccess
          ? contactsWithAccess.filter((el) => el.id !== portalContact.id)
          : []
      );
    }
  }, [businessContactList]);

  useEffect(() => {
    if (portalContact && portalContact.id) {
      fetchBusinessByContact();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [portalContact]);

  const fetchAccessLevelOpt = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_personalpropertyaccountaccess',
      'ptas_accesslevel'
    );
    if (!data) return;
    const newOpts = data.map(
      (opt) => [opt.value, opt.attributeValue] as [string, number]
    );
    setLevelOpts(new Map(newOpts));
  };

  const fetchContactsByBusiness = async (
    businessIds: string[]
  ): Promise<void> => {
    const { data } = await businessContactApiService.getContactByBusinesses(
      businessIds
    );
    if (!data) return;
    setContactsByBusiness(data);
  };

  const fetchBusinessByContact = async (): Promise<void> => {
    if (!portalContact?.id) return;
    const { data } =
      await businessContactApiService.getBusinessContactByContact(
        portalContact.id
      );
    if (!data) return;
    const businessContact = data.filter((p) => p.personalProperty);
    const businessIds = businessContact.map((bc) => bc.personalPropertyId);
    fetchContactsByBusiness(businessIds); // portal contact grouped by business
    const { data: assessedDates } = await businessApiService.getAssessedDates(
      businessIds
    );
    const businessComplete = assessedDates?.size
      ? businessContact.map((bc) => {
          const assessedYear = assessedDates
            .get(bc.personalPropertyId)
            ?.toString();
          return {
            ...bc,
            personalProperty: { ...bc.personalProperty, assessedYear },
          } as PersonalPropertyAccountAccess;
        })
      : businessContact;
    setBusinessContactList(businessComplete);
  };

  const handleAddBusiness = (): void => {
    setAddBusiness((prev) => !prev);
  };

  const handleAddPerson = (): void => {
    console.log('Add primary profile');
    history.push('/sec-profile');
  };

  const onManageBusinessClick = (businessId: string) => (): void => {
    history.push(`/manage-business/${businessId}`);
  };

  const onWhoCanManageClick =
    (businessId: string) =>
    (e: React.MouseEvent<HTMLButtonElement>): void => {
      const contacts = contactsByBusiness.get(businessId) ?? [];
      setContactsByBusinessSelect(contacts);
      setWcmPopoverAnchor(e.currentTarget);
    };

  const closeWhoCanManagePopover = (): void => {
    setContactsByBusinessSelect([]);
    setWcmPopoverAnchor(undefined);
  };

  const getBusinessList = (): PersonalProperty[] => {
    return businessContactList.map(
      (bc) => bc.personalProperty as PersonalProperty
    );
  };

  return {
    businessList: getBusinessList(),
    businessContactList,
    peopleList,
    setPeopleList,
    handleAddBusiness,
    handleAddPerson,
    addBusiness,
    setAddBusiness,
    isStaffUser,
    setIsStaffUser,
    wcmPopoverAnchor,
    onWhoCanManageClick,
    closeWhoCanManagePopover,
    fetchBusinessByContact,
    onManageBusinessClick,
    contactsByBusinessSelect,
    editAccessLevelId: levelOpts.get(LEVEL_EDIT),
  };
}

export default useHome;
