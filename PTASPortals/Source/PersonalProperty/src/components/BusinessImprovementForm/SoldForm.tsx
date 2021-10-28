// SoldForm.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useState } from 'react';
import {
  SimpleDropDown,
  CustomTextField,
  CustomTextButton,
  DropDownItem,
  CustomPhoneTextField,
  ContactInfoAlert,
  ListItem,
  ContactInfoModal,
  AddressInfo,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core';
import * as fm from '../BusinessForm/formatText';
import * as fmGeneral from '../../GeneralFormatMessage';
import Collapse from '@material-ui/core/Collapse';
import * as fmProfile from '../../routes/models/Views/Profile/formatText';

const useStyles = makeStyles((theme: Theme) => ({
  normalInput: {
    maxWidth: 270,
    marginBottom: 25,
    display: 'block',
  },
  inputsWrap: {
    display: 'flex',
    width: 414,
    justifyContent: 'space-between',
  },
  soldForm: {
    width: 270,
    marginBottom: 25,
    marginLeft: 'auto',
  },
  dropdownCity: {
    minWidth: 62,

    '& > .MuiSelect-outlined.MuiSelect-outlined': {
      paddingRight: 0,
    },
  },
  city: {
    maxWidth: 107,
  },
  state: {
    width: 45,
  },
  statePadding: {
    padding: 0,

    '& > fieldset': {
      padding: 0,
    },
  },
  zip: {
    width: 101,
  },
  inputWrap: {
    display: 'flex',
    justifyContent: 'space-between',
    marginBottom: 25,
    width: 270,
  },
  wrap: {
    width: 270,
    display: 'flex',
    justifyContent: 'space-between',
  },
  shrinkCity: {
    left: -6,
  },
  useOtherInfo: {
    marginLeft: 'auto',
    display: 'flex',
  },
  buyerPhone: {
    maxWidth: 196,
    marginBottom: 17,
  },
}));

interface Props {
  isOpen?: boolean;
}

export interface SoldFormData {
  businessName?: string;
  buyerContact?: string;
  buyerPhone?: string;
  buyerAddress?: string;
  address?: string;
  city?: string;
  state?: string;
  zip?: string;
  country?: string | number;
}

const addressList: AddressInfo[] = [
  {
    id: '1',
    title: 'Auburn store',
    country: 'USA',
    addressLine1: '1801 Howard Rd',
    addressLine2: '',
    city: 'Auburn',
    state: 'WA',
    zip: '98002',
  },
  {
    id: '2',
    title: 'Headquarters',
    country: 'USA',
    addressLine1: '7135 Charlotte Pike Ste 100',
    addressLine2: '',
    city: 'Nashville',
    state: 'TN',
    zip: '37209',
  },
];

const countryItems = [
  { value: 'USA', label: 'USA' },
  { value: 'Canada', label: 'Canada' },
];

function SoldForm(props: Props): JSX.Element {
  const classes = useStyles();
  const { isOpen } = props;
  const [addressAnchor, setAddressAnchor] = useState<HTMLElement | null>(null);

  const [openForm, setOpenForm] = useState<boolean>(false);
  const [data, setData] = useState<SoldFormData>({});
  const [manageInfoAnchor, setManageInfoAnchor] = useState<HTMLElement | null>(
    null
  );

  const itemsCountry = [
    { value: 'usa', label: 'USA' },
    { value: 'canada', label: 'Canada' },
  ];

  useEffect(() => {
    setOpenForm(!!isOpen);
  }, [isOpen]);

  const AddressPopup = (
    <ContactInfoAlert
      anchorEl={addressAnchor}
      items={[
        { value: 1, label: 'Auburn store' },
        { value: 2, label: 'Headquarters' },
      ]}
      onItemClick={(item: ListItem): void =>
        console.log('Selected item:', item)
      }
      buttonText={fmProfile.manageAddresses}
      onButtonClick={(): void => {
        setManageInfoAnchor(document.body);
      }}
      onClose={(): void => {
        setManageInfoAnchor(null);
        setAddressAnchor(null);
      }}
    />
  );

  const ManageAddressModal = (
    <ContactInfoModal
      variant={'address'}
      anchorEl={manageInfoAnchor}
      onClickNewItem={(): void => console.log('onClickNewItem')}
      onClose={(): void => setManageInfoAnchor(null)}
      onRemoveItem={(id: string | number): void =>
        console.log('Remove address:', id)
      }
      addressList={addressList}
      countryItems={countryItems}
      newItemText={fmProfile.newAddress}
      addressTexts={{
        titleLabel: fmProfile.modalAddressTitle,
        addressLine1Label: fmProfile.modalAddressLine1,
        addressLine2Label: fmProfile.modalAddressLine2,
        countryLabel: fmProfile.modalAddressCountry,
        cityLabel: fmProfile.modalAddressCity,
        stateLabel: fmProfile.modalAddressState,
        zipLabel: fmProfile.modalAddressZip,
        removeButtonText: fmProfile.modalRemoveButtonAddress,
        removeAlertText: fmProfile.modalRemoveAlertAddress,
        removeAlertButtonText: fmProfile.modalRemoveAlertButtonAddress,
      }}
    />
  );

  const handleChangeFormSoldValue = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    e.persist();

    setData(prevState => {
      return {
        ...prevState,
        [e.target.name]: e.target.value,
      };
    });
  };

  const handleSelectCountry = (item: DropDownItem): void => {
    setData(prevState => {
      return {
        ...prevState,
        country: item.value,
      };
    });
  };

  return (
    <Collapse in={openForm}>
      <div className={classes.soldForm}>
        <CustomTextField
          ptasVariant="outline"
          label={fm.businessNameOfBuyer}
          classes={{ root: classes.normalInput }}
          name="businessName"
          onChange={handleChangeFormSoldValue}
          value={data.businessName}
        />
        <CustomTextField
          ptasVariant="outline"
          label={fm.buyerContact}
          classes={{ root: classes.normalInput }}
          name="buyerContact"
          onChange={handleChangeFormSoldValue}
          value={data.buyerContact}
        />
        <CustomPhoneTextField
          label={fm.buyerPhone}
          placeholder="000-000-000"
          classes={{ root: classes.buyerPhone }}
          onChange={handleChangeFormSoldValue}
          name="buyerPhone"
          value={data.buyerPhone}
        />
        <CustomTextButton
          ptasVariant="Text more"
          classes={{ root: classes.useOtherInfo }}
          onClick={(
            evt: React.MouseEvent<HTMLButtonElement, MouseEvent>
          ): void => setAddressAnchor(evt.currentTarget)}
        >
          {fm.useOtherAddress}
        </CustomTextButton>
        <CustomTextField
          ptasVariant="outline"
          label={fm.buyerAddress}
          classes={{ root: classes.normalInput }}
          name="buyerAddress"
          value={data.buyerAddress}
          onChange={handleChangeFormSoldValue}
        />
        <CustomTextField
          ptasVariant="outline"
          label={fm.address}
          classes={{ root: classes.normalInput }}
          name="address"
          value={data.address}
          onChange={handleChangeFormSoldValue}
        />
        <div className={classes.inputWrap}>
          <CustomTextField
            ptasVariant="outline"
            label={fmGeneral.city}
            classes={{ root: classes.city }}
            name="city"
            onChange={handleChangeFormSoldValue}
            value={data.city}
          />
          <CustomTextField
            ptasVariant="outline"
            classes={{
              root: classes.state,
              fullWidth: classes.statePadding,
              shrinkRoot: classes.shrinkCity,
            }}
            name="state"
            value={data.state}
            onChange={handleChangeFormSoldValue}
            label={fmGeneral.state}
          />
          <CustomTextField
            ptasVariant="outline"
            classes={{ root: classes.zip }}
            label={fmGeneral.zip}
            name="zip"
            onChange={handleChangeFormSoldValue}
            value={data.zip}
          />
        </div>
        <SimpleDropDown
          items={itemsCountry}
          label={fmGeneral.country}
          defaultValue="usa"
          classes={{
            root: classes.dropdownCity,
            textFieldRoot: classes.dropdownCity,
            inputRoot: classes.dropdownCity,
          }}
          value={data.country as string}
          onSelected={handleSelectCountry}
        />
        {AddressPopup}
        {ManageAddressModal}
      </div>
    </Collapse>
  );
}

export default SoldForm;
