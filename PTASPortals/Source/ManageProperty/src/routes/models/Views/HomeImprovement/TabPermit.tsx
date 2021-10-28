// TabPermit.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Box } from '@material-ui/core';
import { Theme, makeStyles } from '@material-ui/core/styles';
import React, { Fragment, useEffect } from 'react';
import {
  CustomDatePicker,
  CustomTextField,
  CustomCard,
  CustomSwitch,
  CustomSearchTextField,
  ItemSuggestion,
  useTextFieldValidation,
  useDateValidation,
  utilService,
} from '@ptas/react-public-ui-library';
import * as fm from './formatMessage';
import PermitCard from './PermitCard';
import { useHI } from './HomeImprovementContext';
import { CHANGE_DELAY_MS } from './constants';

const useStyles = makeStyles((theme: Theme) => ({
  permit: {
    width: 270,
    maxWidth: 270,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    margin: theme.spacing(3, 0),
  },
  selectPermitLabel: {
    // fontSize: theme.ptas.typography.bodyLargeBold.fontSize,
    fontSize: 18,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '22px',
    marginBottom: theme.spacing(2),
  },
  permitCardsWrapper: {
    width: 270,
    paddingTop: theme.spacing(3 / 8),
    paddingLeft: theme.spacing(1),
    // paddingBottom: theme.spacing(1),
    boxSizing: 'border-box',
    marginBottom: theme.spacing(2),
    backgroundColor: 'rgba(255, 255, 255, 0.5)',
  },
  permitCardContentWrap: {
    padding: 0,
    paddingBottom: '0 !important',
  },
  permitsFoundLabel: {
    // fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontSize: 14,
    fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
    lineHeight: '17px',
    display: 'block',
    marginBottom: theme.spacing(1),
  },
  noPermitsLabel: {
    fontSize: theme.ptas.typography.bodyExtraBold.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
  },
  newPermit: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
  },
  permitNumberInput: {
    maxWidth: 180,
    marginTop: theme.spacing(4),
    marginBottom: theme.spacing(4),
  },
  permitIssuedBy: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    fontSize: theme.ptas.typography.finePrintBold.fontSize,
    fontWeight: theme.ptas.typography.finePrintBold.fontWeight,
    lineHeight: '15px',
    marginBottom: theme.spacing(1.25),
  },
  permitIssuerInput: {
    maxWidth: 180,
    marginBottom: theme.spacing(0.875),
  },
  permitIssuerLabel: {
    width: '100%',
  },
  permitIssuedDate: {
    backgroundColor: theme.ptas.colors.theme.white,
  },
  searchWrapper: {
    maxWidth: 180,
    width: 180,
    marginBottom: theme.spacing(0.875),
  },
}));

interface Props {
  updateFormIsValid: (isValid: boolean) => void;
}

function TabPermit(props: Props): JSX.Element {
  const { updateFormIsValid } = props;
  const classes = useStyles();
  const {
    permits,
    selectedPermit,
    setSelectedPermit,
    permitNotListed,
    setPermitNotListed,
    newPermit,
    updateNewPermit,
    jurisdictionSuggestions,
    onPermitIssuerSearchChange,
  } = useHI();
  const {
    isValid: nameIsValid,
    hasError: nameInputHasError,
    valueChangedHandler: nameInputChangedHandler,
    inputBlurHandler: nameInputBlurHandler,
  } = useTextFieldValidation(newPermit?.name ?? '', utilService.isNotEmpty);
  const {
    isValid: issuerIsValid,
    hasError: issuerInputHasError,
    valueChangedHandler: issuerInputChangedHandler,
    inputBlurHandler: issuerInputBlurHandler,
  } = useTextFieldValidation(
    newPermit?.issuedById ?? '',
    utilService.isNotEmpty
  );
  const {
    isValid: issuedDateIsValid,
    hasError: issuedDateInputHasError,
    valueChangedHandler: issuedDateInputChangedHandler,
    inputBlurHandler: issuedDateInputBlurHandler,
  } = useDateValidation(
    newPermit?.issuedDate?.toString() ?? '',
    utilService.validDateStr
  );

  useEffect(() => {
    if (
      (permitNotListed || !permits.length) &&
      (!nameIsValid || !issuerIsValid || !issuedDateIsValid)
    ) {
      updateFormIsValid(false);
    } else if (!permitNotListed && permits.length && !selectedPermit) {
      updateFormIsValid(false);
    } else {
      updateFormIsValid(true);
    }
  }, [
    updateFormIsValid,
    nameIsValid,
    issuerIsValid,
    issuedDateIsValid,
    permitNotListed,
    permits.length,
    selectedPermit,
  ]);

  const onPermitNameChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    const value = e.target.value ? e.target.value.trim() : '';
    updateNewPermit('name', value);
  };

  const handlePermitIssuerSearchChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): void => {
    onPermitIssuerSearchChange && onPermitIssuerSearchChange(e.target.value);
  };

  return (
    <Box className={classes.permit}>
      {permits.length > 0 && (
        <Fragment>
          {' '}
          <label className={classes.selectPermitLabel}>{fm.permitSelect}</label>
          <CustomCard
            classes={{
              root: classes.permitCardsWrapper,
              content: classes.permitCardContentWrap,
            }}
          >
            <label className={classes.permitsFoundLabel}>
              {fm.permitsFound}
            </label>
            {permits.map((permit) => (
              <PermitCard
                key={permit.permitId}
                permitNumber={permit.name}
                amount={permit.permitValue}
                date={permit.issuedDate}
                description={permit.description}
                onClick={(): void => {
                  setSelectedPermit(permit.permitId);
                }}
                isSelected={selectedPermit === permit.permitId}
              />
            ))}
          </CustomCard>
        </Fragment>
      )}
      {permits.length > 0 && (
        <CustomSwitch
          label={fm.permitNotListed}
          ptasVariant="normal"
          showOptions
          isChecked={(isChecked: boolean): void => {
            setPermitNotListed(isChecked);
            if (isChecked) {
              setSelectedPermit(undefined);
            }
          }}
        />
      )}
      {permits.length === 0 && (
        <Box className={classes.noPermitsLabel}>{fm.permitEnterInfo}</Box>
      )}
      {(permitNotListed || permits.length === 0) && (
        <Box className={classes.newPermit}>
          <CustomTextField
            classes={{ root: classes.permitNumberInput }}
            ptasVariant="outline"
            label={fm.permitNumber}
            value={newPermit?.name}
            onChange={(
              e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
            ): void => {
              nameInputChangedHandler(e);
              onPermitNameChange(e);
            }}
            onChangeDelay={CHANGE_DELAY_MS}
            onBlur={nameInputBlurHandler}
            error={nameInputHasError}
            helperText={nameInputHasError ? fm.fieldRequired : ''}
          />
          <Box className={classes.permitIssuedBy}>
            <CustomSearchTextField
              ptasVariant="squared outline"
              label={fm.permitIssuedBy}
              value={newPermit?.issuedByName}
              classes={{
                wrapper: classes.searchWrapper,
              }}
              onChange={(
                e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
              ): void => {
                issuerInputChangedHandler(e);
                handlePermitIssuerSearchChange(e);
              }}
              onChangeDelay={CHANGE_DELAY_MS}
              onBlur={issuerInputBlurHandler}
              suggestion={{
                List: jurisdictionSuggestions,
                onSelected: (item: ItemSuggestion): void => {
                  updateNewPermit('issuedById', item.id ?? '');
                  updateNewPermit('issuedByName', item.title ?? '');
                },
              }}
              error={issuerInputHasError}
              helperText={issuerInputHasError ? fm.fieldRequired : ''}
            />
            <Box className={classes.permitIssuerLabel}>
              {fm.permitIssuerName}
            </Box>
          </Box>
          <CustomDatePicker
            classes={{ root: classes.permitIssuedDate }}
            label={fm.permitDateIssued}
            value={newPermit?.issuedDate}
            onChange={(date: Date): void => {
              issuedDateInputChangedHandler();
              updateNewPermit('issuedDate', date);
            }}
            onBlur={issuedDateInputBlurHandler}
            error={issuedDateInputHasError}
            helperText={issuedDateInputHasError ? fm.fieldRequired : ''}
          />
        </Box>
      )}
    </Box>
  );
}

export default TabPermit;
