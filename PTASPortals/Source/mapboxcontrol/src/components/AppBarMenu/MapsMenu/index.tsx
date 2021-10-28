/* eslint-disable react-hooks/exhaustive-deps */
// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useState, useCallback, useMemo } from 'react';
import {
  makeStyles,
  createStyles,
  Theme,
  Checkbox,
  Tooltip,
  MenuProps,
} from '@material-ui/core';
import DoneIcon from '@material-ui/icons/Done';
import { MenuRootContent, CustomNestedItem } from '@ptas/react-ui-library';
import MapsMenuToolbar from './MapsMenuToolbar';
import { HomeContext } from 'contexts';
import { default as MapsTreeView } from 'components/MapsTreeView';
import { useMapsMenu } from './useMapsMenu';
import { MenuRootItem, CustomMenuRoot } from 'components/common';
import { MapsTreeViewRow } from 'components/MapsTreeView/types';
import { mapService } from 'services/map';
import { getChildRows } from 'utils/componentUtil';
import userService from 'services/user/userService';
import clsx from 'clsx';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    rootPaper: {
      backgroundColor: theme.ptas.colors.theme.white,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      overflow: 'hidden',
    },
    mapsArray: {
      maxHeight: 200,
      overflowY: 'auto',
    },
    checkbox: {
      zIndex: 1,
    },
    nestedItem: {
      maxWidth: 240,
      minWidth: 200,
    },
  })
);

function MapsMenu(): JSX.Element {
  const classes = useStyles();
  const {
    mapsTreeView,
    defaultMap,
    saveDefaultMap,
    selectedUserMap,
    saveUserMap,
    selectedSystemUserMap,
    setPanelContent,
  } = useContext(HomeContext);
  const { activeTab, setActiveTab } = useMapsMenu();
  const [isOpen, setIsOpen] = useState(false);
  const { rows } = mapsTreeView;
  const selectedUserMapId = selectedUserMap?.userMapId;

  const renderMenu = useCallback(
    (items: MapsTreeViewRow[], areNested = false): (JSX.Element | null)[] => {
      return items.map((item) => {
        const childRows = getChildRows<MapsTreeViewRow>(item, rows);
        return childRows.length ? (
          <CustomNestedItem
            key={item.id}
            label={<div>{item.title}</div>}
            parentMenuOpen={isOpen}
          >
            {renderMenu(childRows, true)}
          </CustomNestedItem>
        ) : !item.folder ? (
          <MenuRootItem
            classes={{
              root: clsx(areNested && classes.nestedItem),
            }}
            onClick={function (): void {
              mapService.updateSelectedMap(item.id as number);
            }}
            key={item.id}
            leftIcon={item.id === selectedUserMapId ? <DoneIcon /> : undefined}
            rightElement={
              <Tooltip
                title={
                  defaultMap === item.id
                    ? 'This is your default map map'
                    : 'Make this your default map'
                }
              >
                <Checkbox
                  onClick={(e): void => {
                    e.stopPropagation();
                  }}
                  classes={{ root: classes.checkbox }}
                  checked={defaultMap === item.id}
                  onChange={async (_evt, checked): Promise<void> => {
                    if (checked) {
                      saveDefaultMap(+item.id);
                    } else {
                      const defMap = await userService.getFallBackDefaultMap();
                      if (defMap) {
                        saveDefaultMap(defMap);
                      }
                    }
                    return;
                  }}
                  name="toggle-map-check"
                  color="primary"
                />
              </Tooltip>
            }
          >
            {item.title}
          </MenuRootItem>
        ) : null;
      });
    },
    [
      classes.checkbox,
      classes.nestedItem,
      defaultMap,
      isOpen,
      JSON.stringify(rows),
      saveDefaultMap,
      selectedUserMapId,
    ]
  );

  const filteredFolderId = useMemo(
    () =>
      rows.find((r) => r.title.toLowerCase() === activeTab && r.parent === null)
        ?.id,
    [activeTab, rows]
  );

  const saveMap = (): void => {
    selectedUserMap && saveUserMap(selectedUserMap);
    selectedSystemUserMap && saveUserMap(selectedSystemUserMap);
  };

  return (
    <CustomMenuRoot text="Maps" onToggle={setIsOpen}>
      {({
        closeMenu,
        menuProps,
      }: {
        closeMenu: () => void;
        menuProps: MenuProps;
      }): JSX.Element => (
        <MenuRootContent
          {...menuProps}
          classes={{ paper: classes.rootPaper }}
          key="maps-menu-root-content"
        >
          <MapsMenuToolbar activeTab={activeTab} setActiveTab={setActiveTab} />
          <MenuRootItem
            key="none"
            leftIcon={
              selectedUserMapId === undefined ? <DoneIcon /> : undefined
            }
            onClick={(): void => {
              mapService.updateSelectedMap(undefined);
            }}
          >
            None
          </MenuRootItem>
          <div className={classes.mapsArray}>
            {renderMenu(rows.filter((r) => r.parent === filteredFolderId))}
          </div>
          <MenuRootItem
            variant="fill"
            onClick={(): void => {
              closeMenu();
              setPanelContent(<MapsTreeView />);
            }}
          >
            Edit maps...
          </MenuRootItem>
          <MenuRootItem variant="fill" onClick={saveMap}>
            Save map.
          </MenuRootItem>
        </MenuRootContent>
      )}
    </CustomMenuRoot>
  );
}

export default MapsMenu;
