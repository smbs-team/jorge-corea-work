/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useEffect, useRef } from 'react';
import { MenuItem, InputLabel, Select } from '@material-ui/core';
import AgGrid from 'components/Grid';
import {
  AgGridChildType,
  GenericGridRowData,
  PostProcess,
  Project,
  ProjectDataset,
} from 'services/map.typings';

import { v4 as uuidv4 } from 'uuid';
import { usePrevious } from 'react-use';

interface RegressionGridProps {
  datasetId?: string;
  postProcess?: PostProcess;
  gridData?: GenericGridRowData[];
  datasets: ProjectDataset[];
  edit?: boolean;
  project?: Project | null | undefined;
  hideDatasets?: boolean;
  reloadGrid?: string;
}

export const RegressionGrid = (props: RegressionGridProps): JSX.Element => {
  const [reloadKey, setReloadKey] = useState<string>(uuidv4());
  const [datasetId, setDatasetId] = useState<string>(props.datasetId ?? '');
  const [postProcessId, setPostProcessId] = useState<string>('0');
  const [priority, setPriority] = useState<number>(0);
  const previousReload = usePrevious(props.reloadGrid);
  const previousDatasetId = usePrevious(datasetId);
  const gridRef = useRef<AgGridChildType>(null);
  const [counter, setCounter] = useState<number>(0);

  useEffect(() => {
    if (!props?.postProcess) return;
    setDatasetId(props?.postProcess.datasetId);
    setPriority(props?.postProcess.priority);
    setPostProcessId(props?.postProcess.datasetPostProcessId.toString());
    //eslint-disable-next-line
  }, [props?.postProcess]);

  const callSaveConfig = (): void => {
    saveConfig();
  };

  const saveConfig = async (): Promise<void> => {
    if (!props.reloadGrid?.length) return;
    if (counter === 0) return setCounter((prev) => prev + 1);
    if (previousReload !== props.reloadGrid) {
      await gridRef.current?.reloadOnlyData();
    }
  };

  useEffect(() => {
    if(previousDatasetId && datasetId && datasetId !== previousDatasetId) {
      gridRef.current?.reloadOnlyData();
    }
    //eslint-disable-next-line
  },[datasetId])

  useEffect(callSaveConfig, [props.reloadGrid]);

  useEffect(() => {
    if (props.datasetId) {
      setDatasetId(props.datasetId);
    }
  }, [props.datasetId]);

  useEffect(() => {
    if (props?.postProcess) return;
    if (props.datasets.length && !props.datasetId)
      setDatasetId(
        `${props?.datasets.find((p) => p.datasetRole === 'Sales')?.datasetId}`
      );
    //eslint-disable-next-line
  }, [props.datasets]);

  //eslint-disable-next-line
  const onChange = (e: any): void => {
    setDatasetId(`${e.target.value}`);
    if (props.project && props.postProcess) {
      const dataset = props.project.projectDatasets.find(
        (d) => d.datasetId === `${e.target.value}`
      )?.dataset;
      if (dataset) {
        const postProcess = dataset.dependencies?.postProcesses?.find(
          (pp) => pp.priority === priority
        );
        if (postProcess) {
          setPostProcessId(postProcess.datasetPostProcessId.toString());
        } else {
          setPostProcessId('0');
        }
      }
    }
    setReloadKey(uuidv4());
  };

  const handlePostProcessId = (): {} => {
    if (props?.postProcess) {
      if (JSON.parse(props?.postProcess?.resultPayload)?.Status === 'Success') {
        return {
          postProcessId: postProcessId ?? '0',
        };
      }
    }
    return {};
  };

  const renderGrid = (): JSX.Element => {
    let gridProps = {};
    if (!props.hideDatasets) {
      gridProps = { element: renderSelectDataset() };
    }
    if (props.edit)
      return (
        <AgGrid
          ref={gridRef}
          height={'450px'}
          id={datasetId}
          key={reloadKey}
          {...gridProps}
          reloadGrid={(): void => setReloadKey(uuidv4())}
          gridVariableData={props?.gridData ?? []}
          showSpinner={false}
          externalUse={true}
          {...handlePostProcessId()}
        ></AgGrid>
      );

    return (
      <AgGrid
        ref={gridRef}
        height={'450px'}
        id={datasetId}
        {...gridProps}
        gridVariableData={props?.gridData ?? []}
        reloadGrid={(): void => setReloadKey(uuidv4())}
        key={reloadKey}
        showSpinner={false}
        externalUse={true}
      ></AgGrid>
    );
  };

  const renderSelectDataset = (): JSX.Element => {
    if (props.hideDatasets) return <></>;
    return (
      <div
        className="TimeTrend-formGroup"
        style={{
          borderBottom: 'none',
          width: '50%',
          marginBottom: '0px',
          display: datasetId.length ? 'block' : 'none',
        }}
      >
        <div className={'DropdownWrapper'}>
          <InputLabel
            className="TimeTrend-label regression"
            id="label-for-dd"
            style={{ width: '225px' }}
          >
            Results Data sets:
          </InputLabel>
          <Select
            variant="outlined"
            className="drop-down"
            labelId="label-for-dd"
            fullWidth
            placeholder={''}
            value={datasetId}
            onChange={onChange}
          >
            {props.datasets.map((o, i) => (
              <MenuItem key={i} value={o.datasetId}>
                {o?.dataset.datasetName}
              </MenuItem>
            ))}
          </Select>
        </div>
      </div>
    );
  };

  return (
    <div style={{ width: '100%' }}>
      {/* {renderSelectDataset()} */}
      {datasetId.length && renderGrid()}
    </div>
  );
};
