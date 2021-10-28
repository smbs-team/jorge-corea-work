// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { memo } from 'react';
import SettingsMenu from './SettingsMenu';
import HelpMenu from './HelpMenu';
import { Box } from '@material-ui/core';

const SecondaryMenu = (): JSX.Element => {
  return (
    <Box>
      <SettingsMenu />
      <HelpMenu />
    </Box>
  );
};

export default memo(SecondaryMenu);
