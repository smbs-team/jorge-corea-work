/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Box } from '@material-ui/core';
import { MenuRootContent } from '@ptas/react-ui-library';
import { CustomMenuRoot } from 'components/common';
import { HomeContext } from 'contexts';
import { withMap } from 'hoc/withMap';
import React, { useContext } from 'react';
import { DataSetParcelItem } from 'services/api';
import selectionService from 'services/parcel/selection';
import { fitAllBbox, fitSelectedBbox } from 'utils';
import { NestedMenu } from '../common';
import MenuItem from '../common/MenuItem';

const DsMenu = withMap(function ({ map }): JSX.Element {
  const { dataSetPoints } = useContext(HomeContext);
  const { loadingDataSetParcels } = dataSetPoints;

  const showDots = (closeMenu: () => void) => async (
    selected?: DataSetParcelItem['selected']
  ): Promise<void> => {
    if (loadingDataSetParcels) return;
    closeMenu();
    dataSetPoints.clear();
    selectionService.clearSelection({
      doApiRequest: false,
    });
    await dataSetPoints.load(selected);
    if (selected) {
      fitSelectedBbox(map, selectionService.getSelectedDotsList());
    } else {
      fitAllBbox(map);
    }
  };
  return (
    <CustomMenuRoot text="Data Set">
      {({ closeMenu, menuProps, isMenuRootOpen }): JSX.Element => {
        return (
          <MenuRootContent {...menuProps}>
            <NestedMenu
              label={
                <Box component="span" width="100%">
                  Show rows
                </Box>
              }
              parentMenuOpen={isMenuRootOpen}
            >
              <MenuItem
                onClick={(): void => {
                  dataSetPoints.clear();
                  selectionService.clearSelection();
                }}
              >
                None
              </MenuItem>
              <MenuItem
                onClick={(): void => {
                  showDots(closeMenu)();
                }}
              >
                All
              </MenuItem>
              <MenuItem
                onClick={(): void => {
                  showDots(closeMenu)('selected');
                }}
              >
                Selected
              </MenuItem>
              <MenuItem
                onClick={(): void => {
                  showDots(closeMenu)('notSelected');
                }}
              >
                Non Selected
              </MenuItem>
            </NestedMenu>
          </MenuRootContent>
        );
      }}
    </CustomMenuRoot>
  );
});
export default DsMenu;
