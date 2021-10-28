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

export interface NewNuisanceModalValues {
  viewType: number;
  airportNoiseLevel: number;
  trafficNoiseLevel: number;
  valueMethod: number;
  startValue: number;
  endValue: string;
}

interface Props extends WithStyles<typeof useStyles> {
  isOpen?: boolean;
  onSave?: (data: LandVariableGridRowData) => void;
  onClose?: () => void;
  values: NewNuisanceModalValues;
}

interface LandVariablePartialType {
  minadjmoney: string;
  maxadjmoney: string;
  maxadjpercentaje: string;
  minadjpercentaje: string;
}

interface LandVariableNoiseType {
  noiseLevel?: string;
  ptas_noiselevel?: string | number;
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

const NewNuisanceModal = (props: Props): JSX.Element => {
  const airportId = 591500000;
  const trafficId = 591500007;

  const { classes } = props;
  const [open, setOpen] = React.useState(false);
  const [nuisanceItems, setNuisanceItems] = useState([] as DropDownItem[]);
  const [selectedNuisance, setSelectedNuisance] = useState(
    props.values.viewType
  );
  const [selectedAirportNoiseLevel, setSelectedAirportNoiseLevel] = useState(
    props.values.airportNoiseLevel
  );

  const [selectedTrafficNoiseLevel, setSelectedTrafficNoiseLevel] = useState(
    props.values.trafficNoiseLevel
  );

  const [selectedValueMethod, setSelectedValueMethod] = useState(
    props.values.valueMethod
  );
  const [startValue, setStartValue] = useState(props.values.startValue);
  const [endValue, setEndValue] = useState(props.values.endValue);

  const [noiseLevels, setNoiseLevels] = useState([] as DropDownItem[]);
  const [valueMethods, setValueMethods] = useState([] as DropDownItem[]);
  const [valueMethodName, setValueMethodName] = useState('');
  const [autoFocus, setAutofocus] = useState<string>('');
  const [characteristicTypes, setCharacteristicTypes] = useState(
    [] as DropDownItem[]
  );

  const callLoad = (): void => {
    (async (): Promise<void> => {
      const nuisanceTypes = await ValuesLoader('ptas_nuisancetype');
      setNuisanceItems(dropdownItemFromApi(nuisanceTypes));
      if (selectedNuisance === -1) setSelectedNuisance(+nuisanceTypes[0].id);

      const noiseLevels = await ValuesLoader('ptas_noiselevel');
      setNoiseLevels(dropdownItemFromApi(noiseLevels));
      if (selectedAirportNoiseLevel <= 0)
        setSelectedAirportNoiseLevel(+noiseLevels[0].id);

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

  const getNoiseLevel = (): LandVariableNoiseType => {
    if (selectedNuisance === airportId) {
      return {
        noiseLevel:
          noiseLevels.find(v => v.value === selectedAirportNoiseLevel)?.label ??
          '',
        ptas_noiselevel: noiseLevels.find(
          v => v.value === selectedAirportNoiseLevel
        )?.value,
      };
    }
    return {};
  };

  const getModalData = (): LandVariableGridRowData => {
    const nuisanceName =
      nuisanceItems.find(v => v.value === selectedNuisance)?.label ?? '';
    const noiseLevel =
      selectedNuisance === airportId
        ? ' - ' +
          noiseLevels.find(v => v.value === selectedAirportNoiseLevel)?.label
        : selectedNuisance === trafficId
        ? ' - ' + selectedTrafficNoiseLevel
        : '';
    return {
      characteristicType: 'Nuisance',
      description: `${nuisanceName}${noiseLevel}`,
      value: `${
        valueMethods.find(vm => vm.value === selectedValueMethod)?.label
      }`,
      ptas_nuisancetype: selectedNuisance,
      nuisanceType:
        nuisanceItems.find(v => v.value === selectedNuisance)?.label ?? '',
      viewType: nuisanceName,
      ptas_valuemethodcalculation: selectedValueMethod,
      ptas_characteristictype: parseInt(
        `${characteristicTypes.find(v => v.label === 'Nuisance')?.value}`
      ),
      ...getNoiseLevel(),
      ...getValues(),
    };
  };

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
            <label className={classes.label}>
              Land Characteristic: Nuisance
            </label>
            <div className={classes.content}>
              {selectedNuisance && (
                <SimpleDropDown
                  label="Nuisance"
                  items={nuisanceItems}
                  classes={{ root: classes.dropdown }}
                  value={selectedNuisance}
                  onSelected={(v): void => {
                    const n = +v.value;
                    setSelectedNuisance(n);
                  }}
                ></SimpleDropDown>
              )}
              {selectedNuisance === airportId && (
                <SimpleDropDown
                  label="Airport Noise Level"
                  classes={{ root: classes.dropdown }}
                  items={noiseLevels}
                  value={selectedAirportNoiseLevel}
                  onSelected={(v): void => {
                    const n = +v.value;
                    setSelectedAirportNoiseLevel(n);
                  }}
                ></SimpleDropDown>
              )}
              {selectedNuisance === trafficId && (
                <CustomNumericField
                  value={selectedTrafficNoiseLevel}
                  onValueChange={(v): void =>
                    setSelectedTrafficNoiseLevel(v.floatValue ?? 0)
                  }
                  classes={{ root: classes.dropdown }}
                  label="Traffic noise level"
                  NumericProps={{
                    inputMode: 'numeric',
                    onFocus: (): void => {
                      setAutofocus('noiseLevel');
                    },
                    autoFocus: autoFocus === 'noiseLevel',
                  }}
                ></CustomNumericField>
              )}
            </div>
            {selectedNuisance && (
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
export default withStyles(useStyles)(NewNuisanceModal);
