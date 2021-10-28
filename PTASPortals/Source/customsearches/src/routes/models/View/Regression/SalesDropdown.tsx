// FILENAME
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useState, useContext } from 'react';
import {
  InputLabel,
  MenuItem,
  Select as SimpleSelect,
  makeStyles,
} from '@material-ui/core';
import { ProjectDataset } from 'services/map.typings';
import Select from 'react-select';
import { v4 as uuidv4 } from 'uuid';
import { ProjectContext } from 'context/ProjectsContext';

interface MultiSelectItem {
  value: string;
  label: string;
  isFixed?: boolean;
}
interface SalesDropdownPropsType {
  options: ProjectDataset[];
  getDatasetId: (datasetId: string) => void;
  datasetId?: string;
  getSecondaryDataset?: (values: string[]) => void;
  selectedValues?: string[];
  changeDataset?: () => void;
  edit?: boolean;
  hideSecondaryDatasets?: boolean;
  land?: boolean;
  onChange?: () => void;
  timeTrend?: boolean;
  unlockPopulation?: boolean;
  role?: string;
  previousModel?: string;
  actualModel?: string;
}

const useStyles = makeStyles((theme) => ({
  dropdown: {
    width: '50%',
  },
}));

export const SalesDropdown = (props: SalesDropdownPropsType): JSX.Element => {
  const classes = useStyles();
  const [value, setValue] = useState<string>('');
  const [selected, setSelected] = useState<MultiSelectItem[]>([]);
  const [datasetsToValidate, setDatasetsToValidate] = useState<string[]>([]);
  const [changeSelect, setChangeSelect] = useState<string>('');
  const [options, setOptions] = useState<MultiSelectItem[]>([]);
  const [manualChange, setManualChange] = useState<boolean>(false);
  const [validationMessage, setValidationMessage] = useState<string>('');
  const [datasetName, setDatasetName] = useState<string>('');
  const projectContext = useContext(ProjectContext);

  useEffect(() => {
    if (!selected.length) return;
    if (!value) return;
    setDatasetsToValidate([...selected.map((s) => s.value), value]);
  }, [value, selected]);

  const removeLastComma = (strng: string): string => {
    const n = strng.lastIndexOf(',');
    const a = strng.substring(0, n);
    return a;
  };

  useEffect(() => {
    if (!props.role) return;
    if (!props.options) return;
    const missingPostProcess = '';
    let datasetName = '';
    datasetsToValidate.forEach((d) => {
      const exist = props.options?.some(
        (op) =>
          op.datasetId === d &&
          op.dataset.dependencies.postProcesses?.some(
            (pp) => pp.postProcessRole === props.role
          )
      );
      if (!exist) {
        datasetName = datasetName.concat(
          `${
            props.options?.find((op) => op.datasetId === d)?.dataset.datasetName
          }`
        );
        // .concat(', ');
        // missingPostProcess = ` needs to be included as part of the selected datasets for ${props.previousModel} before it can be included as part of the ${props.actualModel}.`;
      }
    });
    if (datasetName) setDatasetName(removeLastComma(datasetName));
    setValidationMessage(missingPostProcess);
    //eslint-disable-next-line
  }, [datasetsToValidate]);

  useEffect(() => {
    const validateSelectedValues = (): void => {
      if (props.selectedValues) {
        const values = props.selectedValues.map<MultiSelectItem>((se) => ({
          value: se,
          label:
            props.options.find((op) => op.datasetId === se)?.dataset
              ?.datasetName ?? '',
          isFixed: !props.unlockPopulation
            ? props.options.find((op) => op.datasetId === se)?.dataset
                ?.datasetName === 'Population'
            : false,
        }));
        setSelected(values);
      }
    };
    validateSelectedValues();
    //eslint-disable-next-line
  }, [props.selectedValues]);

  useEffect(() => {
    const options = props.options
      .filter((op) => op.datasetId !== value)
      .map((op) => ({
        value: op.datasetId,
        label: op.dataset.datasetName,
        isFixed: !props.unlockPopulation
          ? op.datasetRole === 'Population'
          : false,
      }));
    setOptions(options);
    //eslint-disable-next-line
  }, [props.options]);

  useEffect(() => {
    if (props.datasetId) {
      setValue(props.datasetId);
    }
    //eslint-disable-next-line
  }, [props.datasetId]);

  useEffect(() => {
    if (!changeSelect.length) return;
    const datasetIds = selected.map((s) => s.value);
    if (props.getSecondaryDataset) {
      props.getSecondaryDataset(datasetIds);
    }
    //eslint-disable-next-line
  }, [changeSelect]);

  useEffect(() => {
    if (manualChange) {
      props.getDatasetId && props.getDatasetId(value);
    }
    //eslint-disable-next-line
  }, [manualChange]);

  //eslint-disable-next-line
  const onChange = (e: any): void => {
    if (value === e?.target?.value) return;
    if (!e.target.value) return;
    setValue(`${e.target.value}`);
    setManualChange(true);
    props.getDatasetId(`${e.target.value}`);
    props?.changeDataset?.();
  };

  const orderOptions = (values: MultiSelectItem[]): MultiSelectItem[] => {
    return values
      .filter((v) => v.isFixed)
      .concat(values.filter((v) => !v.isFixed));
  };

  const onChangeSecondary = (
    //eslint-disable-next-line
    value: any,
    //eslint-disable-next-line
    { action, removedValue }: any
  ): void => {
    switch (action) {
      case 'remove-value':
      case 'pop-value':
        if (removedValue.isFixed) {
          return;
        }
        break;
      case 'clear':
        value = options.filter((v) => v.isFixed);
        break;
    }
    const datasetIds = orderOptions(value);
    setSelected(datasetIds);
    props.getSecondaryDataset?.(datasetIds.map((d) => d.value));
    setChangeSelect(uuidv4());
    props?.onChange?.();
  };

  const styles = {
    //eslint-disable-next-line
    multiValue: (base: any, state: any) => {
      return state.data.isFixed ? { ...base, backgroundColor: 'gray' } : base;
    },
    //eslint-disable-next-line
    multiValueLabel: (base: any, state: any) => {
      return state.data.isFixed
        ? { ...base, fontWeight: 'bold', color: 'white', paddingRight: 6 }
        : base;
    },
    //eslint-disable-next-line
    multiValueRemove: (base: any, state: any) => {
      return state.data.isFixed ? { ...base, display: 'none' } : base;
    },
  };

  const renderErrorMessage = (): JSX.Element => {
    if (validationMessage.length) {
      return (
        <span style={{ color: 'red', fontSize: '16px' }}>
          <span style={{ fontWeight: 700, fontStyle: 'italic' }}>
            {datasetName}
          </span>
          {validationMessage}
        </span>
      );
    }
    return <></>;
  };

  const renderSecondaryDatasets = (): JSX.Element => {
    let params = {};
    if (!props.unlockPopulation) {
      params = {
        isClearable: selected.some((s) => !s.isFixed),
      };
    }

    if (props.hideSecondaryDatasets) return <></>;
    return (
      <div className={'DropdownWrapper'}>
        <InputLabel className="TimeTrend-label sales" id="label-for-dd">
          Select secondary dataset:
        </InputLabel>
        <Select
          styles={styles}
          isMulti
          name="datasets"
          value={selected}
          {...params}
          onChange={onChangeSecondary}
          className={`basic-multi-select ${classes.dropdown}`}
          classNamePrefix="select"
          options={options.filter((s) => s.value !== value)}
          isDisabled={projectContext.modelDetails?.isLocked}
        />
      </div>
    );
  };

  return (
    <>
      <div className="TimeTrend-formGroup sales no-border">
        <div className={'DropdownWrapper'}>
          <InputLabel className="TimeTrend-label sales" id="label-for-dd">
            Select main dataset:
          </InputLabel>
          <SimpleSelect
            variant="outlined"
            className="drop-down"
            labelId="label-for-dd"
            fullWidth
            placeholder={''}
            value={value}
            onChange={onChange}
            disabled={projectContext.modelDetails?.isLocked || props.edit}
          >
            {props.options?.map((o, i) => (
              <MenuItem value={o?.datasetId}>
                {o?.dataset?.datasetName}
              </MenuItem>
            ))}
          </SimpleSelect>
        </div>
        {renderSecondaryDatasets()}
      </div>
      <div style={{ paddingLeft: '30px' }}>{renderErrorMessage()}</div>
    </>
  );
};
