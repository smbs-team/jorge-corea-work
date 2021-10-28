// GoPaperless.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect, useState } from 'react';
import {
  CustomTextField,
  CustomButton,
  CustomTextButton,
  ImageWithZoom,
  CustomPopover as PtasCustomPopover,
  Alert,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core/styles';
import * as fm from '../../routes/models/Views/Home/formatText';
import { ManagePropertyContext } from 'contexts/ManageProperty';
import { AppContext } from 'contexts/AppContext';
import { TaxAccount } from 'models/taxAccount';
import { taxAccountService } from 'services/api/apiService/taxaccount';
import { isNaN } from 'lodash';
import { getStringFromFormatMsgComp } from 'Utils';

interface Props {
  test?: string;
}

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    width: '100%',
    maxWidth: 541,
    textAlign: 'center',
  },
  inputAccountNumber: {
    display: 'block',
    maxWidth: '100%',
    marginBottom: 27,

    [theme.breakpoints.up('sm')]: {
      maxWidth: 140,
      marginBottom: 0,
      marginRight: 32,
    },
  },
  inputEvnCode: {
    display: 'block',
    maxWidth: '100%',

    marginBottom: 27,
    [theme.breakpoints.up('sm')]: {
      marginBottom: 0,
      maxWidth: 95,
    },
  },
  goPaperlessButton: {
    marginBottom: 16,
    width: 165,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    display: 'block',
    margin: '0 auto',
  },
  turnOffButton: {
    marginBottom: 16,
    width: 152,
    height: 23,
    padding: 0,
    fontSize: theme.ptas.typography.finePrint.fontSize,
    display: 'block',
    margin: '0 auto',
  },
  title: {
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    marginBottom: 16,
    display: 'block',
  },
  email: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    color: theme.ptas.colors.utility.success,
    marginBottom: 16,
    display: 'block',
  },
  mainWrapper: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'space-between',
    marginBottom: 16,
    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
    },
  },
  formTitle: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    marginBottom: 16,
    display: 'block',
  },
  inputWrap: {
    display: 'flex',
    flexDirection: 'column',
    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
    },
  },
  formWrapper: {
    width: '100%',
    maxWidth: 294,
  },
  requestEvnCodeButton: {
    marginBottom: 17,
    fontSize: 14,
    fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
  },
  description: {
    display: 'block',
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    color: theme.ptas.colors.theme.gray,
    width: '100%',
    maxWidth: 264,
    margin: '0 auto',
  },
  popoverRoot: {
    width: '100%',
    maxWidth: 406,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    color: theme.ptas.colors.theme.white,
    boxSizing: 'border-box',

    '& > a': {
      color: theme.ptas.colors.theme.white,
    },
  },
  shrink: {},
}));

function GoPaperless(props: Props): JSX.Element {
  const classes = useStyles(props);
  const [event, setEvent] = useState<HTMLElement | null>(null);
  const [goPaperlessActive, setGoPaperlessActive] = useState<boolean>(false);
  const { taxAccountByParcelSel, parcelInfoSelected } = useContext(
    ManagePropertyContext
  );
  const { portalContact } = useContext(AppContext);
  const [accountNumber, setAccountNumber] = useState<string>('');
  const [evnCode, setEvnCode] = useState<string>('');
  const [accountInputError, setAccountInputError] = useState<string>('');
  const [evnInputError, setEvnInputError] = useState<string>('');

  useEffect(() => {
    setGoPaperlessActive(taxAccountByParcelSel?.paperless ?? false);
  }, [taxAccountByParcelSel]);

  const cleanState = (): void => {
    setEvent(null);
  };

  const changePaperlessOnTaxAccount = async (
    email: string | null
  ): Promise<void> => {
    if (accountInputError || evnInputError) return;
    const paperless = !!email;
    if (!taxAccountByParcelSel) return;
    const taxAccountToUpdate = {
      ...taxAccountByParcelSel,
      paperless,
      email,
    } as TaxAccount;
    const { hasError } = await taxAccountService.updateTaxAccountPaperless(
      taxAccountToUpdate
    );
    if (hasError) return;
    setGoPaperlessActive(paperless);
  };

  const handleActiveGoPaperless = (): void => {
    const { email } = portalContact?.email ?? {};
    if (!email) return;
    changePaperlessOnTaxAccount(email);
  };

  const handleInactiveGoPaperless = (): void => {
    changePaperlessOnTaxAccount(null);
  };

  const handleAccountInputChange = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    const { value } = e.currentTarget;
    setAccountNumber(value);
    const valueCleaned = value.replace(/[\s, -]/g, '');
    const formatNotValidMsg =
      valueCleaned.length !== 10 && !isNaN(valueCleaned)
        ? getStringFromFormatMsgComp(fm.accountNumberHelper)
        : '';
    const notFoundMsg =
      taxAccountByParcelSel?.accountNumber !== valueCleaned
        ? getStringFromFormatMsgComp(fm.accountNumberNotValid)
        : '';
    const finalMsg = formatNotValidMsg || notFoundMsg || '';
    setAccountInputError(finalMsg);
  };

  const handleEvnNumberInputChange = (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    const { value } = e.currentTarget;
    setEvnCode(value);
    const valueCleaned = value.replace(/[\s, -]/g, '');
    const formatNotValidMsg =
      valueCleaned.length !== 6 && !isNaN(valueCleaned)
        ? getStringFromFormatMsgComp(fm.evnCodeHelper)
        : '';
    const notFoundMsg =
      parcelInfoSelected?.ptas_evncode !== valueCleaned
        ? getStringFromFormatMsgComp(fm.codeNotValid)
        : '';
    const finalMsg = formatNotValidMsg || notFoundMsg || '';
    setEvnInputError(finalMsg);
  };

  const imageurl =
    'https://qph.fs.quoracdn.net/main-qimg-da158b59e1c4786392ef06ce208bb9c9';

  const renderContent = (): JSX.Element => {
    if (goPaperlessActive) return <Fragment />;

    return (
      <div className={classes.mainWrapper}>
        <div className={classes.formWrapper}>
          <span className={classes.formTitle}>{fm.goPaperlessFormTitle}</span>
          <div className={classes.inputWrap}>
            <CustomTextField
              ptasVariant="overlay"
              name="accountNumber"
              value={accountNumber}
              onChange={handleAccountInputChange}
              label={fm.accountNumberLabel}
              placeholder="000000-0000"
              classes={{
                root: classes.inputAccountNumber,
              }}
              error={!!accountInputError}
              helperText={accountInputError || fm.accountNumberHelper}
              onChangeDeps={[taxAccountByParcelSel]}
            />
            <CustomTextField
              name="evncode"
              value={evnCode}
              ptasVariant="overlay"
              onChange={handleEvnNumberInputChange}
              label={fm.evnCodeLabel}
              placeholder="000000"
              classes={{
                root: classes.inputEvnCode,
              }}
              error={!!evnInputError}
              helperText={evnInputError || fm.evnCodeHelper}
              onChangeDeps={[parcelInfoSelected]}
            />
          </div>
        </div>
        <ImageWithZoom imageUrl={imageurl} showZoomIcon />
      </div>
    );
  };

  const renderButton = (): JSX.Element => {
    if (goPaperlessActive) {
      return (
        <CustomButton
          classes={{ root: classes.turnOffButton }}
          ptasVariant="Danger"
          onClick={handleInactiveGoPaperless}
        >
          {fm.turnOff}
        </CustomButton>
      );
    }

    return (
      <CustomButton
        classes={{ root: classes.goPaperlessButton }}
        onClick={handleActiveGoPaperless}
        disabled={!!(accountInputError || evnInputError)}
      >
        {fm.goPaperlessButton}
      </CustomButton>
    );
  };

  return (
    <div className={classes.root}>
      <p className={classes.title}>
        {goPaperlessActive ? fm.electronicProperty : fm.goPaperlessTitle}
      </p>
      <span className={classes.email}>
        {portalContact?.email?.email ?? '-'}
      </span>
      {renderContent()}
      {renderButton()}
      <CustomTextButton
        ptasVariant="Text more"
        classes={{ root: classes.requestEvnCodeButton }}
        onClick={(e: React.MouseEvent<HTMLButtonElement>): void => {
          setEvent(e.currentTarget);
        }}
      >
        {fm.requestEvnCode}
      </CustomTextButton>
      <span className={classes.description}>{fm.goPaperlessDescription}</span>
      <PtasCustomPopover
        anchorEl={event}
        onClose={(): void => {
          cleanState();
        }}
        ptasVariant="success"
        showCloseButton
        tail
        tailPosition="center"
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'center',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'center',
        }}
      >
        <Alert
          contentText={fm.popupMessage}
          ptasVariant="success"
          classes={{
            root: classes.popoverRoot,
          }}
        />
      </PtasCustomPopover>
    </div>
  );
}

export default GoPaperless;
