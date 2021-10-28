// Property.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  ChangeEvent,
  Fragment,
  useEffect,
  useState,
  useCallback,
} from 'react';
import { makeStyles, Theme, Collapse, Box } from '@material-ui/core';
import * as fm from './formatText';
import * as fmGeneral from '../../../../GeneralFormatMessage';

import {
  CustomTextarea,
  CustomSwitch,
  CustomDatePicker,
  CustomSearchTextField,
  ItemSuggestion,
  useDateValidation,
} from '@ptas/react-public-ui-library';
import clsx from 'clsx';
import { apiService } from 'services/api/apiService';
import useDestroyedProperty from './useDestroyedProperty';
import utilService from 'services/utilService';

interface Props {
  test?: string;
}

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    width: '100%',
    maxWidth: 270,
    '& .MuiGrid-justify-xs-space-around': {
      justifyContent: 'start',
    },
    paddingBottom: 33,
  },
  permitIssuedInput: {
    maxWidth: '100%',
    backgroundColor: theme.ptas.colors.theme.white,
    marginBottom: 19,
  },
  propertyDates: {
    width: '100%',
    display: 'flex',
    justifyContent: 'space-between',
  },
  datePickerInput: {
    backgroundColor: theme.ptas.colors.theme.white,
  },
  datePickerShrink: {
    color: `${theme.ptas.colors.theme.black} !important`,
    paddingRight: 0,
  },
  dateOfDestructionLabel: {
    fontSize: theme.ptas.typography.finePrint.fontSize,
  },
  destructionDate: {
    width: 129,
  },
  labelTitle: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    display: 'block',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    textAlign: 'start',
  },
  labelDesc: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    color: theme.ptas.colors.theme.gray,
    display: 'block',
    lineHeight: '20px',
    marginBottom: 4,
  },
  notchedOutline: {
    borderColor: 'transparent',
  },
  description: {
    marginTop: 24,
    marginBottom: 19,
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
  searchWrapper: {
    maxWidth: 270,
    width: 270,
    marginBottom: theme.spacing(0.875),
  },
  permitIssuerLabel: {
    width: '100%',
    color: 'rgba(0, 0, 0, 0.54)',
    paddingLeft: 9,
    boxSizing: 'border-box',
  },
}));

function Property(props: Props): JSX.Element {
  const [startDate, setStartDate] = useState<string>('');
  const [endDate, setEndDate] = useState<string>('');
  const [isMounted, setIsMounted] = useState<boolean>(false);
  const {
    hasError: errorStartDate,
    inputBlurHandler: onBlurValidStartDate,
  } = useDateValidation(startDate, utilService.validDate);
  const {
    hasError: errorEndDate,
    inputBlurHandler: onBlurValidEndDate,
  } = useDateValidation(endDate, utilService.validDate);

  const classes = useStyles(props);
  const {
    setTask,
    task,
    saveTask,
    jurisdictionSuggestions,
    setJurisdictionSuggestions,
    propDescriptionInputError,
    setPropDescriptionInputError,
  } = useDestroyedProperty();

  const handleSetPermitIssued = (isChecked: boolean): void => {
    setTask((prev) => {
      return {
        ...prev,
        repairDateUnknownAtThisTime: isChecked,
      };
    });
  };

  const handleChangeDescription = (
    e: ChangeEvent<HTMLTextAreaElement>
  ): void => {
    const description = e.target.value;

    saveTask('destroyedPropertyDescription', description);

    if (!description) return setPropDescriptionInputError(true);

    setPropDescriptionInputError(false);
  };

  const setDateState = useCallback(() => {
    if (!isMounted) return;
    setIsMounted(true);

    const validStartDate = utilService.validDate(startDate);
    const validEndDate = utilService.validDate(endDate);

    const startDateVal = `${validStartDate && startDate ? startDate : 'N.A'}`;
    const endDateVal = `${validEndDate && endDate ? endDate : 'N.A'}`;
    const fullDate = `${startDateVal}-${endDateVal}`;

    setTask((prev) => ({
      ...prev,
      anticipatedRepairDates: fullDate !== 'N.A-N.A' ? fullDate : '',
    }));
  }, [endDate, startDate, setTask, isMounted, setIsMounted]);

  useEffect(() => {
    setDateState();
  }, [setDateState]);

  const handleChangeStartDate = (_: Date, value: string): void =>
    setStartDate(value);

  const handleChangeEndDate = (_: Date, value: string): void =>
    setEndDate(value);

  const onPermitIssuerChange = async (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ): Promise<void> => {
    const value = e.target.value;
    if (!value) return;

    const jurisdictionRes = await apiService.searchJurisdiction(value);

    if (!jurisdictionRes.data?.length) return;

    const list = jurisdictionRes.data.map((item) => ({
      id: item.id,
      title: item.name,
      subtitle: '',
    }));
    setJurisdictionSuggestions(list);
  };

  const onSelectJurisdiction = (item: ItemSuggestion): void => {
    setTask((prev) => {
      return {
        ...prev,
        permitIssuedBy: item.title,
      };
    });
  };

  const Form = (
    <Collapse in={task?.repairDateUnknownAtThisTime}>
      <div className={clsx(classes.propertyDates)}>
        <CustomDatePicker
          key="date-start-date"
          classes={{
            root: classes.destructionDate,
            animated: classes.dateOfDestructionLabel,
            shrink: classes.datePickerShrink,
            input: classes.datePickerInput,
          }}
          label={fm.propertyStartDate}
          onChange={handleChangeStartDate}
          value={task?.anticipatedRepairDates.split('-')[0] || ''}
          onChangeDelay={500}
          onBlur={onBlurValidStartDate}
          placeholder="mm/dd/yyyy"
          helperText={errorStartDate ? fm.enterAValidDate : undefined}
          error={errorStartDate}
        />
        <CustomDatePicker
          key="date-end-date"
          classes={{
            root: classes.destructionDate,
            animated: classes.dateOfDestructionLabel,
            shrink: classes.datePickerShrink,
            input: classes.datePickerInput,
          }}
          label={fm.propertyEndDate}
          onChange={handleChangeEndDate}
          value={task?.anticipatedRepairDates.split('-')[1] ?? ''}
          onChangeDelay={500}
          placeholder="mm/dd/yyyy"
          helperText={errorEndDate ? fm.enterAValidDate : fm.estimate}
          error={errorEndDate}
          onBlur={onBlurValidEndDate}
        />
      </div>
      <Box className={classes.permitIssuedBy}>
        <CustomSearchTextField
          ptasVariant="squared outline"
          label={fm.permitIssuedLabel}
          value={task?.permitIssuedBy}
          classes={{
            wrapper: classes.searchWrapper,
          }}
          onChange={onPermitIssuerChange}
          onChangeDelay={500}
          // onClick={onClickSearch}
          suggestion={{
            List: jurisdictionSuggestions,
            onSelected: onSelectJurisdiction,
          }}
        />
        <Box className={classes.permitIssuerLabel}>
          {fm.permitIssuedHelperText}
        </Box>
      </Box>
    </Collapse>
  );

  const PermitIssuedLabel = (
    <Fragment>
      <span className={classes.labelTitle}>{fm.repairLabel}</span>
      <span className={clsx(classes.labelDesc)}>{fm.repairLabelDesc}</span>
    </Fragment>
  );

  return (
    <div className={classes.root}>
      <CustomTextarea
        placeholder={fm.textAreaPlaceholder}
        helperText={
          propDescriptionInputError
            ? fm.fieldRequired
            : !task?.repairDateUnknownAtThisTime && fm.textAreaHelperText
        }
        classes={{
          root: classes.description,
        }}
        onChange={handleChangeDescription}
        onChangeDelay={500}
        value={task?.destroyedPropertyDescription}
        error={propDescriptionInputError}
      />
      <CustomSwitch
        label={PermitIssuedLabel}
        ptasVariant="small"
        showOptions
        isChecked={handleSetPermitIssued}
        onLabel={fmGeneral.yes}
        offLabel={fmGeneral.no}
        value={!!task?.repairDateUnknownAtThisTime}
      />
      {Form}
    </div>
  );
}

export default Property;
