// LayerContent.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { memo } from 'react';
import { Box } from '@material-ui/core';
import { useTheme } from '@material-ui/core/styles';
import clsx from 'clsx';
import { useLayerContentStyles } from './useLayerContentStyles';
import { useManageLayerCommonStyles } from '../styles';

const LayerContent = memo(
  (): JSX.Element => {
    const theme = useTheme();
    const classes = {
      ...useManageLayerCommonStyles(theme),
      ...useLayerContentStyles(theme),
    };

    return (
      <Box className={clsx(classes.root)}>
        <Box className={classes.line}>
          <Box
            style={{
              width: '18px',
              height: '18px',
              backgroundColor: 'blue',
              marginRight: theme.spacing(2),
            }}
          ></Box>
          <Box>Land of type xxx</Box>
        </Box>
        <Box className={classes.line}>
          <Box
            style={{
              width: '18px',
              height: '18px',
              backgroundColor: 'green',
              marginRight: theme.spacing(2),
            }}
          ></Box>
          <Box>Land of type yyy</Box>
        </Box>
        <Box className={classes.line}>
          <Box
            style={{
              width: '18px',
              height: '18px',
              backgroundColor: 'lightblue',
              marginRight: theme.spacing(2),
            }}
          ></Box>
          <Box>Land of type zzz</Box>
        </Box>
      </Box>
    );
  }
);

export default LayerContent;
