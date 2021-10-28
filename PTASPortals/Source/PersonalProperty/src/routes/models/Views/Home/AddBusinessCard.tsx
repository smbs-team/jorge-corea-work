// AddBusinessCard.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useCallback, useContext, useEffect, useState } from 'react';
import {
  CustomCard,
  CustomButton,
  CustomTextField,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { Box } from '@material-ui/core';
import * as fm from './formatText';
import { PersonalProperty } from 'models/personalProperty';
import { businessApiService } from 'services/api/apiService/business';
import { debounce } from 'lodash';
import { useHistory } from 'react-router-dom';
import { businessContactApiService } from 'services/api/apiService/businessContact';
import { PersonalPropertyAccountAccess } from 'models/personalPropertyAccountAccess';
import { v4 as uuidV4 } from 'uuid';
import { AppContext } from 'contexts/AppContext';
import { apiService } from 'services/api/apiService';
import { LEVEL_EDIT, STATUS_CODE_ACTIVE } from './constants';

interface Props {
  fetchBusinessByContact: () => Promise<void>;
  setAddBusiness: React.Dispatch<boolean>;
}

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    display: 'block',
    boxSizing: 'border-box',
    padding: theme.spacing(2),
    marginBottom: 15,
    maxWidth: '330px',
    marginLeft: 'auto',
    marginRight: 'auto',
    overflow: 'visible',
    borderRadius: '9px',
    backgroundColor: theme.ptas.colors.theme.white,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    // fontSize: theme.ptas.typography.bodySmallBold.fontSize,
    fontSize: '14px',
    fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
    lineHeight: '17px',
  },
  textExistingBusiness: {
    marginBottom: theme.spacing(2),
  },
  textNewBusiness: {
    marginBottom: theme.spacing(1),
    marginTop: theme.spacing(4),
  },
  inputs: {
    marginBottom: theme.spacing(4.5),
  },
  accountNumberInput: {
    width: '109px',
    marginRight: theme.spacing(4),
  },
  accessCodeInput: {
    width: '114px',
  },
  button: {
    width: 'auto',
    height: 23,
    padding: '3px 22px',
    fontSize: 14,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '17px',
  },
}));

function AddBusinessCard(props: Props): JSX.Element {
  const classes = useStyles();
  const { fetchBusinessByContact, setAddBusiness } = props;
  const [businessFound, setBusinessFound] = useState<
    PersonalProperty | undefined
  >();
  const [accountNumber, setAccountNumber] = useState<string>('');
  const [accessCode, setAccessCode] = useState<string>('');
  const [statusCodeOpts, setStatusCodeOpts] = useState<Map<string, number>>(
    new Map()
  );
  const [levelOpts, setLevelOpts] = useState<Map<string, number>>(new Map());
  const { portalContact } = useContext(AppContext);
  const history = useHistory();
  const accountErrorHelperText =
    accountNumber && !businessFound ? fm.addBusinessWrongAcc : '';
  const codeErrorHelperText =
    businessFound && businessFound.accessCode !== accessCode
      ? fm.addBusinessWrongCode
      : '';

  useEffect(() => {
    fetchStatusCodeOpt();
    fetchAccessLevelOpt();
  }, []);

  useEffect(() => {
    if (accountNumber) {
      fetchBusiness(accountNumber);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [accountNumber]);

  const fetchStatusCodeOpt = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_personalpropertyaccountaccess',
      'statuscode'
    );
    if (!data) return;
    const newOpts = data.map(
      opt => [opt.value, opt.attributeValue] as [string, number]
    );
    setStatusCodeOpts(new Map(newOpts));
  };

  const fetchAccessLevelOpt = async (): Promise<void> => {
    const { data } = await apiService.getOptionSet(
      'ptas_personalpropertyaccountaccess',
      'ptas_accesslevel'
    );
    if (!data) return;
    const newOpts = data.map(
      opt => [opt.value, opt.attributeValue] as [string, number]
    );
    setLevelOpts(new Map(newOpts));
  };

  const fetchBusiness = useCallback(
    debounce(async (accountNumber: string) => {
      const { data } = await businessApiService.getBusinessByAccount(
        accountNumber
      );
      if (!data) return;
      setBusinessFound(data);
    }, 800),
    []
  );

  const onChangeCustomTextField = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    switch (e.target.name) {
      case 'accountNumber':
        setAccountNumber(e.target.value);
        break;
      case 'accessCode':
        setAccessCode(e.target.value);
        break;
      default:
        break;
    }
  };

  const closeThisCard = (): void => {
    setAddBusiness(false);
  };

  const handleAddExistingBusiness = async (): Promise<void> => {
    if (
      !portalContact ||
      !businessFound ||
      accountErrorHelperText ||
      codeErrorHelperText
    )
      return;
    const newBusinessContact = new PersonalPropertyAccountAccess();
    newBusinessContact.id = uuidV4();
    newBusinessContact.portalContactId = portalContact.id;
    newBusinessContact.accessLevel = levelOpts.get(LEVEL_EDIT) as number;
    newBusinessContact.personalPropertyId = businessFound.id;
    newBusinessContact.stateCode = 0;
    newBusinessContact.statusCode = statusCodeOpts.get(
      STATUS_CODE_ACTIVE
    ) as number;
    await businessContactApiService.addBusinessContact(newBusinessContact);
    fetchBusinessByContact();
    closeThisCard();
  };

  const redirectToCreateNewBusiness = (): void => {
    history.push('/new-business', { createFromHome: true });
  };

  return (
    <CustomCard
      variant="wrapper"
      classes={{
        rootWrap: classes.card,
      }}
    >
      <Box className={classes.textExistingBusiness}>{fm.addBusinessText1}</Box>
      <Box className={classes.inputs}>
        <CustomTextField
          name="accountNumber"
          ptasVariant="outline"
          label={fm.addBusinessAcctNo}
          helperText={accountErrorHelperText || fm.addBusinessAcctNoDesc}
          placeholder="00000000"
          classes={{ root: classes.accountNumberInput }}
          error={!!accountErrorHelperText}
          onChange={onChangeCustomTextField}
        />
        <CustomTextField
          name="accessCode"
          ptasVariant="outline"
          label={fm.addBusinessAccessCode}
          helperText={codeErrorHelperText || fm.addBusinessAccessCodeDesc}
          placeholder="xxxxxxxx"
          classes={{ root: classes.accessCodeInput }}
          error={!!codeErrorHelperText}
          onChange={onChangeCustomTextField}
        />
      </Box>
      <CustomButton
        classes={{
          root: classes.button,
        }}
        onClick={handleAddExistingBusiness}
        ptasVariant="Primary"
        disabled={!businessFound || businessFound.accessCode !== accessCode}
      >
        {fm.addBusinessButtonExisting}
      </CustomButton>
      <Box className={classes.textNewBusiness}>{fm.addBusinessText2}</Box>
      <CustomButton
        classes={{
          root: classes.button,
        }}
        onClick={redirectToCreateNewBusiness}
        ptasVariant="Outline"
      >
        {fm.addBusinessButtonNew}
      </CustomButton>
    </CustomCard>
  );
}

export default AddBusinessCard;
