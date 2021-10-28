// FILENAME
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import CloseIcon from '@material-ui/icons/Close';
import {
  Backdrop,
  Fade,
  Modal,
  createStyles,
  Theme,
  withStyles,
  WithStyles,
  StyleRules,
} from '@material-ui/core';
import {
  CustomButton,
  CustomIconButton,
  CustomNumericField,
  DropDownItem,
  SimpleDropDown,
} from '@ptas/react-ui-library';
import React, { Fragment, useEffect } from 'react';
import { useState } from 'react';
import { LandVariableGridRowData } from 'services/map.typings';
import { dropdownItemFromApi, ValuesLoader } from './adjustments-helpers';
export interface NewViewModalValues {
  viewType: number;
  quality: number;
  valueMethod: number;
  startValue: number;
  endValue: string;
}

interface Props extends WithStyles<typeof useStyles> {
  isOpen?: boolean;
  onSave?: (data: LandVariableGridRowData) => void;
  onClose?: () => void;
  values: NewViewModalValues;
}

interface LandVariablePartialType {
  minadjmoney: string;
  maxadjmoney: string;
  maxadjpercentaje: string;
  minadjpercentaje: string;
}

const useStyles = (theme: Theme): StyleRules =>
  createStyles({
    root: {
      borderRadius: 12,
      boxShadow: theme.shadows[5],
      width: 617,
      height: 414,
      backgroundColor: 'white',
      position: 'absolute',
      padding: theme.spacing(2.5, 5, 2.5, 5),
    },
    iconButton: {
      position: 'absolute',
      top: 13,
      right: 34,
      color: 'black',
    },
    closeIcon: {
      fontSize: 42,
    },
    label: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: '1.375rem',
      marginBottom: 14,
      display: 'block',
    },
    messageLabel: {
      fontSize: '18px',
      paddingBottom: '1em',
      display: 'block',
    },
    userInput: {
      maxWidth: '230px',
    },
    dropdown: {
      width: '100%',
      marginBottom: '1em',
      maxWidth: '230px',
    },
    content: {},
    numeric: {
      marginBottom: 16,
    },
    columnTitle: {
      fontFamily: theme.ptas.typography.titleFontFamily,
      fontSize: '1.25rem',
      marginBottom: 24,
      fontWeight: 'bolder',
    },
    button: {
      display: 'flex',
      marginLeft: 'auto',
      marginTop: 60,
    },
  });

const NewViewModal = (props: Props): JSX.Element => {
  const { classes } = props;
  const [open, setOpen] = React.useState(false);
  const [viewItems, setViewItems] = useState([] as DropDownItem[]);
  const [selectedView, setSelectedView] = useState(props.values.viewType);
  const [selectedQuality, setSelectedQuality] = useState(props.values.quality);
  const [selectedValueMethod, setSelectedValueMethod] = useState(
    props.values.valueMethod
  );
  const [autoFocus, setAutofocus] = useState<string>('');
  const [startValue, setStartValue] = useState(props.values.startValue);
  const [endValue, setEndValue] = useState(props.values.endValue);

  const [qualityItems, setQualityItems] = useState([] as DropDownItem[]);
  const [characteristicTypes, setCharacteristicTypes] = useState(
    [] as DropDownItem[]
  );
  const [valueMethods, setValueMethods] = useState([] as DropDownItem[]);
  const [valueMethodName, setValueMethodName] = useState('');

  const callLoad = (): void => {
    (async (): Promise<void> => {
      const viewTypes = await ValuesLoader('ptas_viewtype');
      setViewItems(dropdownItemFromApi(viewTypes));
      if (selectedView === -1) setSelectedView(+viewTypes[0].id);

      const qualityTypes = await ValuesLoader('ptas_quality');
      setQualityItems(dropdownItemFromApi(qualityTypes));
      if (selectedQuality === -1) setSelectedQuality(+qualityTypes[0].id);

      const characteristicTypes = await ValuesLoader('ptas_characteristictype');
      setCharacteristicTypes(dropdownItemFromApi(characteristicTypes));

      let valueMethods = await ValuesLoader('ptas_valuemethodcalculation');
      valueMethods = valueMethods?.filter(method =>
        ['$ adjustment', '% adjustment'].includes(method.label)
      );
      if (selectedValueMethod === -1)
        setSelectedValueMethod(+valueMethods[0].id);
      setValueMethods(dropdownItemFromApi(valueMethods));
    })();
  };
  useEffect(callLoad, []);

  useEffect(() => {
    if (props.isOpen === undefined) return;
    setOpen(props.isOpen);
  }, [props.isOpen]);

  useEffect(() => {
    if (!valueMethods) return;
    const vm = valueMethods.find(
      method => method.value === selectedValueMethod
    );
    setValueMethodName(vm?.label || '');
  }, [selectedValueMethod, valueMethods]);

  const handleClose = (): void => {
    setOpen(false);
    props.onClose && props.onClose();
  };

  const getValues = (): LandVariablePartialType => {
    if (
      valueMethods.find(vm => vm.value === selectedValueMethod)?.label ===
      '$ adjustment'
    ) {
      return {
        minadjmoney: startValue.toString(),
        maxadjmoney: endValue ? endValue.toString() : '',
        maxadjpercentaje: '',
        minadjpercentaje: '',
      };
    }
    return {
      minadjpercentaje: startValue.toString(),
      maxadjpercentaje: endValue ? endValue.toString() : '',
      maxadjmoney: '',
      minadjmoney: '',
    };
  };

  const getModalData = (): LandVariableGridRowData => ({
    characteristicType: 'View',
    description: `${viewItems.find(v => v.value === selectedView)?.label} - ${
      qualityItems.find(v => v.value === selectedQuality)?.label
    }`,
    value: `${
      valueMethods.find(vm => vm.value === selectedValueMethod)?.label
    }`,
    ptas_characteristictype: parseInt(
      `${characteristicTypes.find(v => v.label === 'View')?.value}`
    ),
    ptas_viewtype: selectedView,
    viewType: viewItems.find(v => v.value === selectedView)?.label,
    quality: qualityItems.find(v => v.value === selectedQuality)?.label,
    ptas_quality: selectedQuality,
    ptas_valuemethodcalculation: selectedValueMethod,
    ...getValues(),
  });

  return (
    <Modal
      open={open}
      onClose={handleClose}
      closeAfterTransition
      BackdropComponent={Backdrop}
      BackdropProps={{
        timeout: 500,
      }}
      style={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
      }}
    >
      <Fade in={open}>
        <Fragment>
          <div className={classes.root} style={{ height: 'auto' }}>
            <label className={classes.label}>Land Characteristic: View</label>
            {selectedView && (
              <SimpleDropDown
                label="View Type"
                items={viewItems}
                classes={{ root: classes.dropdown }}
                value={selectedView}
                onSelected={(v): void => {
                  const n = +v.value;
                  setSelectedView(n);
                }}
              ></SimpleDropDown>
            )}
            <div className={classes.content}>
              <SimpleDropDown
                label="Quality"
                classes={{ root: classes.dropdown }}
                items={qualityItems}
                value={selectedQuality}
                onSelected={(v): void => {
                  const n = +v.value;
                  setSelectedQuality(n);
                }}
              ></SimpleDropDown>
            </div>
            {selectedView && (
              <SimpleDropDown
                label="Value Method"
                items={valueMethods}
                classes={{ root: classes.dropdown }}
                value={selectedValueMethod}
                onSelected={(v): void => {
                  const n = +v.value;
                  setSelectedValueMethod(n);
                }}
              ></SimpleDropDown>
            )}
            {valueMethodName && (
              <Fragment>
                <CustomNumericField
                  value={startValue}
                  onValueChange={(v): void => setStartValue(v.floatValue ?? 0)}
                  classes={{ root: classes.dropdown }}
                  label={valueMethodName}
                  NumericProps={{
                    inputMode: 'numeric',
                    onFocus: (): void => {
                      setAutofocus('minValue');
                    },
                    autoFocus: autoFocus === 'minValue',
                  }}
                ></CustomNumericField>

                <label className={classes.messageLabel}>
                  If this is a range, add the max value
                </label>
                <CustomNumericField
                  value={endValue}
                  onValueChange={(v): void => setEndValue(`${v.floatValue}`)}
                  classes={{ root: classes.dropdown }}
                  label={valueMethodName}
                  NumericProps={{
                    inputMode: 'numeric',
                    onFocus: (): void => {
                      setAutofocus('maxValue');
                    },
                    autoFocus: autoFocus === 'maxValue',
                  }}
                ></CustomNumericField>
              </Fragment>
            )}
            <CustomButton
              classes={{ root: classes.button }}
              onClick={(): void => {
                props.onSave && props.onSave(getModalData());
                handleClose();
              }}
              fullyRounded
            >
              Add
            </CustomButton>
            <CustomIconButton
              icon={<CloseIcon className={classes.closeIcon} />}
              className={classes.iconButton}
              onClick={handleClose}
            />
          </div>
        </Fragment>
      </Fade>
    </Modal>
  );
};
export default withStyles(useStyles)(NewViewModal);
