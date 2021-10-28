// contact.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { PortalAddress, PortalEmail, PortalPhone } from 'models/portalContact';
import { contactApiService } from 'services/api/apiService/portalContact';

interface Contact {
  emails: PortalEmail[];
  addresses: PortalAddress[];
  phones: PortalPhone[];
}
/**
 *
 * @param portalContactId -
 * @returns List of emails, addresses and phone numbers
 */
export const getContactInfo = async (
  portalContactId: string
): Promise<Contact> => {
  const emailReq = contactApiService.getContactEmails(portalContactId);

  const addressesReq = contactApiService.getContactAddresses(portalContactId);

  const phonesReq = contactApiService.getContactPhones(portalContactId);

  const [emails, addresses, phones] = await Promise.all([
    emailReq,
    addressesReq,
    phonesReq,
  ]);

  return {
    emails: emails.data ?? [],
    addresses: addresses.data ?? [],
    phones: phones.data ?? [],
  };
};
