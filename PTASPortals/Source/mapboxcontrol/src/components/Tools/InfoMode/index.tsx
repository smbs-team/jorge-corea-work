// InfoMode.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect } from 'react';
import { makeStyles } from '@material-ui/core';
import BasePanel from 'components/BasePanel';
import { DropDownItem, SimpleDropDown, TreeView } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import {
  InfoModeDropDownKey,
  LayerTreeItem,
  useFeaturesTree,
} from './useFeaturesTree';
import { DrawToolBarContext } from 'contexts/DrawToolbarContext';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    width: '100%',
    padding: theme.spacing(4.25, 4.25, 0, 4.25),
  },
  header: {
    display: 'flex',
    alignItems: 'center',
    height: 74,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontWeight: 'normal',
    fontSize: '1rem',
  },
  dropdown: {
    width: 400,
    marginRight: 30,
  },
  headerLabel: {
    marginRight: 30,
  },
  refreshIcon: {
    marginRight: 15,
  },
  leftTree: {
    width: 280,
    fontSize: '1.25rem',
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontWeight: 'bold',
    flexShrink: 0,
    paddingRight: 8,
    height: 'fit-content',
  },
  leftTreeLabel: {
    display: 'block',
    marginBottom: 14,
  },
  rightTree: {
    borderLeft: '1px solid black',
    paddingLeft: 37,
    height: 'fit-content',
  },
  rightTableContainer: {
    '& table': {
      '& thead tr th': {
        fontSize: '1.25rem',
        fontFamily: theme.ptas.typography.titleFontFamily,
        fontWeight: 'bold',
        borderBottom: '1px solid #c0c0c0',
        paddingBottom: 16,
      },
    },
  },
  rightTreeRoot: {
    '& tbody tr': {},
    '& tbody tr td': {},
  },
  rightTreeNoGroup: {
    fontWeight: 'normal',
  },
}));

const dropdownItems: DropDownItem[] = [
  { label: '<Visible layers>', value: 'visible' },
  { label: '<Top-most layers>', value: 'top' },
  { label: '<All layers>', value: 'all' },
];

const leftColumns = [{ name: 'name', title: 'Name' }];
const rightColumns = [
  { name: 'field', title: 'Field' },
  { name: 'value', title: 'Value' },
];

const columnSizes = [
  { columnName: 'field', width: 200 },
  { columnName: 'value', width: 600 },
];

/**
 * InfoMode
 *
 * @param props - Component props
 * @returns A JSX element
 */
function InfoMode(): JSX.Element {
  const classes = useStyles();
  const { panelHeight } = useContext(HomeContext);
  const { setActionMode}=useContext(DrawToolBarContext)
  const {
    layersTree,
    setDropDownOption,
    setRightTreeData,
    rightTreeData,
  } = useFeaturesTree();

  const handleLeftTreeSelect = (_row?: LayerTreeItem): void => {
    if (!_row) {
      setRightTreeData([]);
      return;
    }
    if (_row.parent === null) return;
    if (!_row.featureProps) return;
    const _rightTreeData = Object.entries(_row.featureProps ?? {})
      .map(([k, v]) => ({
        layerId: _row.id,
        field: k,
        id: Math.random(),
        parent: null,
        value: v,
      }))
      .filter((item) => {
        if (typeof item.field === 'string' && item.field.endsWith('__'))
          return false;
        return true;
      });
    setRightTreeData(_rightTreeData);
  };

  useEffect(() => {
    return (): void => {
      setActionMode(null);
    };
  }, [setActionMode]);

  return (
    <BasePanel
      disableScrollY
      toolbarItems={
        <Fragment>
          <div className={classes.header}>
            <label className={classes.headerLabel}>Identity from:</label>
            <SimpleDropDown
              label="Layers"
              items={dropdownItems}
              value="visible"
              onSelected={(s): void =>
                setDropDownOption(s.value as InfoModeDropDownKey)
              }
              classes={{ root: classes.dropdown }}
            />
          </div>
        </Fragment>
      }
    >
      <div className={classes.root}>
        <div className={classes.leftTree}>
          <label className={classes.leftTreeLabel}>Features</label>
          <TreeView
            rows={layersTree}
            columns={leftColumns}
            displayGroupInColumn="name"
            onSelect={handleLeftTreeSelect}
            noDataMessage="No point selected on the map"
            virtualTableProps={{
              height:  panelHeight - 173,
            }}
            virtual
            hideEye
            hideHeader
            disableGrouping
          />
        </div>
        <div className={classes.rightTree}>
          <TreeView
            rows={rightTreeData}
            columns={rightColumns}
            displayGroupInColumn="field"
            virtualTableProps={{
              height:panelHeight - 130,
            }}
            classes={{
              tableContainer: classes.rightTableContainer,
              root: classes.rightTreeRoot,
              noGroups: classes.rightTreeNoGroup,
            }}
            resizeDefaultColumnWidths={columnSizes}
            sortBy={[{ columnName: 'field', direction: 'desc' }]}
            noDataMessage="No feature selected"
            enableColumnResize
            virtual
            hideEye
            disableGrouping
          />
        </div>
      </div>
    </BasePanel>
  );
}

export default  InfoMode;
