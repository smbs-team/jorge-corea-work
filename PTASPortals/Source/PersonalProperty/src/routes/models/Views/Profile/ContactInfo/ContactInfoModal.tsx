// ContactInfoModal.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment, useEffect } from 'react';
import {
  createStyles,
  WithStyles,
  withStyles,
  Box,
  StyleRules,
} from '@material-ui/core';
import { Theme } from '@material-ui/core/styles';
import {
  GenericWithStyles,
  CustomPopover,
  CustomButton,
  DropDownItem,
  ItemSuggestion,
} from '@ptas/react-public-ui-library';
import { AddCircleOutline as AddIcon } from '@material-ui/icons';
import EmailEditor, { EmailEditorTextProps } from './EmailEditor';
import AddressEditor, { AddressEditorTextProps } from './AddressEditor';
import { PortalAddress, PortalEmail, PortalPhone } from 'models/portalContact';
import PhoneEditor, { PhoneEditorTextProps } from './PhoneEditor';
import { AddressLookupEntity, BasicAddressData } from 'models/addresses';

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles> {
  variant: 'email' | 'address' | 'phone';
  newItemText: string | React.ReactNode;
  anchorEl: HTMLElement | null | undefined;
  onClose: () => void;
  onClickNewItem?: () => void;
  onRemoveItem: (id: string) => void;
  onChangeItem: (item: PortalEmail | PortalAddress | PortalPhone) => void;
  addressSuggestions?: AddressLookupEntity[];
  countryItems?: DropDownItem[];
  citySuggestions?: ItemSuggestion[];
  stateSuggestions?: ItemSuggestion[];
  zipCodeSuggestions?: ItemSuggestion[];
  emailList?: PortalEmail[];
  addressList?: PortalAddress[];
  phoneList?: PortalPhone[];
  emailTexts?: EmailEditorTextProps;
  addressTexts?: AddressEditorTextProps;
  phoneTexts?: PhoneEditorTextProps;
  phoneTypeList?: { value: number; label: string }[];
  onAddressSearchChange?: (value: string) => void;
  onCitySearchChange?: (value: string) => void;
  onStateSearchChange?: (value: string, countryId: string) => void;
  onZipCodeSearchChange?: (value: string) => void;
  getAddressData: (
    addressSuggestion: AddressLookupEntity
  ) => Promise<BasicAddressData>;
}

/**
 * Component styles
 */
const styles = (theme: Theme): StyleRules<string, object> =>
  createStyles({
    root: {},
    paper: {
      width: '380px',
      boxSizing: 'border-box',
      padding: '42px 10px 16px 10px',
      borderRadius: '9px',
      marginTop: 0,

      [theme.breakpoints.up('sm')]: {
        marginTop: '60px',
        padding: '42px 30px 16px 30px',
      },

      overflowX: 'auto',
    },
    backdrop: {
      backgroundColor: 'rgba(0, 0, 0, 0.5) !important',
    },
    closeButton: {
      color: theme.ptas.colors.theme.black,
    },
    buttonNew: {
      width: 'auto',
      height: 24,
      padding: '3px 22px',
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '17px',
    },
    addIcon: {
      width: '18px',
      height: '18px',
      marginRight: theme.spacing(0.5),
    },
    border: {
      background:
        'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
      height: 1,
      width: '100%',
      display: 'block',
      marginTop: theme.spacing(2),
      marginBottom: theme.spacing(1),
    },
    entities: {
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
    },
  });

/**
 * ContactInfoModal
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ContactInfoModal(props: Props): JSX.Element {
  const {
    classes,
    variant,
    newItemText,
    anchorEl,
    onClose,
    onClickNewItem,
    onRemoveItem,
    onChangeItem,
    addressSuggestions,
    countryItems,
    citySuggestions,
    stateSuggestions,
    zipCodeSuggestions,
    emailList,
    addressList,
    phoneList,
    onAddressSearchChange,
    onCitySearchChange,
    onStateSearchChange,
    onZipCodeSearchChange,
    getAddressData,
  } = props;

  useEffect(() => {
    //Workaround for displaying correct width of indicator for tabs component (used in phone modal).
    //Issue https://github.com/mui-org/material-ui/issues/9337
    if (variant === 'phone') {
      setTimeout(() => {
        window.dispatchEvent(new CustomEvent('resize'));
      }, 10);
    }
  });

  return (
    <Box>
      <CustomPopover
        classes={{
          root: `${classes.root} ${props.classes?.root}`,
          paper: `${classes.paper} ${props.classes?.paper}`,
          closeButton: `${classes.closeButton} ${props.classes?.closeButton}`,
          backdropRoot: classes.backdrop,
        }}
        anchorEl={anchorEl}
        onClose={(): void => {
          onClose();
        }}
        ptasVariant="info"
        showCloseButton
        anchorOrigin={{
          vertical: 'top',
          horizontal: 'center',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'center',
        }}
      >
        <Box>
          <CustomButton
            onClick={onClickNewItem}
            classes={{ root: classes.buttonNew }}
          >
            <AddIcon
              classes={{
                root: classes.addIcon,
              }}
            />
            {newItemText ?? 'New item'}
          </CustomButton>
          {variant === 'email' && (
            <Box className={classes.entities}>
              {emailList?.length &&
                emailList.map((email, i) => (
                  <Fragment key={email.id}>
                    <EmailEditor
                      email={email}
                      disableRemove={emailList.length === 1}
                      onRemove={(): void => onRemoveItem(email.id)}
                      onChange={onChangeItem}
                      {...props.emailTexts}
                    />
                    {i < emailList.length - 1 && (
                      <span className={classes.border}></span>
                    )}
                  </Fragment>
                ))}
            </Box>
          )}
          {variant === 'address' && (
            <Box className={classes.entities}>
              {addressList &&
                addressList.length > 0 &&
                addressList.map((address, i) => (
                  <Fragment key={address.id}>
                    <AddressEditor
                      key={address.id}
                      address={address}
                      onRemove={onRemoveItem}
                      onChange={onChangeItem}
                      addressSuggestions={addressSuggestions}
                      countryItems={countryItems}
                      citySuggestions={citySuggestions}
                      stateSuggestions={stateSuggestions}
                      zipCodeSuggestions={zipCodeSuggestions}
                      onAddressSearchChange={onAddressSearchChange}
                      onCitySearchChange={onCitySearchChange}
                      onStateSearchChange={onStateSearchChange}
                      onZipCodeSearchChange={onZipCodeSearchChange}
                      getAddressData={getAddressData}
                      {...props.addressTexts}
                    />
                    {i < addressList.length - 1 && (
                      <span className={classes.border}></span>
                    )}
                  </Fragment>
                ))}
            </Box>
          )}
          {variant === 'phone' && (
            <Box>
              <Box className={classes.entities}>
                {phoneList &&
                  phoneList.length > 0 &&
                  phoneList.map((phone, i) => (
                    <Fragment key={phone.id}>
                      <PhoneEditor
                        key={phone.id}
                        phone={phone}
                        phoneTypeList={props.phoneTypeList}
                        onRemove={onRemoveItem}
                        onChange={onChangeItem}
                        {...props.phoneTexts}
                      />
                      {i < phoneList.length - 1 && (
                        <span className={classes.border}></span>
                      )}
                    </Fragment>
                  ))}
              </Box>
            </Box>
          )}
        </Box>
      </CustomPopover>
    </Box>
  );
}

export default withStyles(styles)(ContactInfoModal) as FC<
  GenericWithStyles<Props>
>;
