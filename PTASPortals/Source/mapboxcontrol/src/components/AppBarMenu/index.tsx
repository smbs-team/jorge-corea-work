// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useEffect, useState } from 'react';
import MapsMenu from './MapsMenu';
import RenderersMenu from './RenderersMenu';
import ToolsMenu from './ToolsMenu';
import { Box } from '@material-ui/core';
import ViewMenu from './ViewMenu';
import DsMenu from './DsMenu';
import { utilService } from 'services/common';
import { QUERY_PARAM } from 'appConstants';
import { rendererService } from 'services/map';
import { useObservable } from 'react-use';
import { $onSelectedLayersChange } from 'services/map/mapServiceEvents';

const AppBarMenu = (): JSX.Element => {
  const [showDsMenu, setShowDsMenu] = useState(false);
  const renderers = useObservable($onSelectedLayersChange);

  useEffect(() => {
    setShowDsMenu(
      !!utilService.getUrlSearchParam(QUERY_PARAM.DATASET_ID) ||
        !!rendererService.getDatasetId()
    );
  }, [renderers]);
  return (
    <Box>
      <MapsMenu />
      <RenderersMenu />
      <ViewMenu />
      <ToolsMenu />
      {showDsMenu && <DsMenu />}
    </Box>
  );
};

export default AppBarMenu;
