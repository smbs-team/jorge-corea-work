/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import BottomNavigationAction, {
  BottomNavigationActionProps,
} from '@material-ui/core/BottomNavigationAction';
import { createStyles, StyleRules, Theme, withStyles } from '@material-ui/core';

const styles = (theme: Theme): StyleRules =>
  createStyles({
    root: {},
    wrapper: {
      color: theme.ptas.colors.theme.black,
    },
    label: (props: BottomNavigationActionProps) => {
      return {
        fontFamily: theme.ptas.typography.bodyFontFamily,
        opacity: '1 !important',
        paddingBottom: 5,
        borderBottomWidth: 4,
        borderBottomStyle: 'solid',
        borderBottomColor: props.selected
          ? theme.ptas.colors.utility.selection + 'e0'
          : theme.ptas.colors.theme.grayLight + '00',
      };
    },
  });

export default withStyles(styles)(BottomNavigationAction);
