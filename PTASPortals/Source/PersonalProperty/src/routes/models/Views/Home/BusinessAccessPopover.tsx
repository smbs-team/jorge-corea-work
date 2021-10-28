// BusinessAccessPopover.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC } from 'react';
import {
  createStyles,
  WithStyles,
  withStyles,
  StyleRules,
  Box,
} from '@material-ui/core';
import { Theme } from '@material-ui/core/styles';
import {
  GenericWithStyles,
  CustomPopover,
  Alert,
} from '@ptas/react-public-ui-library';
import clsx from 'clsx';
import { v4 as uuid } from 'uuid';

export type ProfileOption = 'editProfile' | 'signOut';

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles> {
  anchorEl: HTMLElement | null | undefined;
  businessList: string[];
  onClose: () => void;
}

/**
 * Component styles
 */
const styles = (theme: Theme): StyleRules<string, object> =>
  createStyles({
    itemContainer: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '19px',
    },
    item: {
      marginBottom: theme.spacing(2),
    },
    root: {},
    popover: {},
    popupColor: {
      backgroundColor: theme.ptas.colors.theme.accent,
      color: theme.ptas.colors.theme.white,
    },
    closePopupButton: {
      color: theme.ptas.colors.theme.white,
    },
    backdrop: {},
    alert: {
      width: 'fit-content',
      minWidth: '100px',
      padding: '26px 23.5px 9px 21.5px',
      borderRadius: '9px',
    },
  });

/**
 * BusinessAccessPopover
 *
 * @param props - Component props
 * @returns A JSX element
 */
function BusinessAccessPopover(props: Props): JSX.Element {
  const { classes, anchorEl, businessList, onClose } = props;

  const renderBusinessList = (list: string[]): JSX.Element => (
    <Box className={classes.itemContainer}>
      {list.map((businessName) => (
        <Box key={uuid()} className={classes.item}>
          {businessName}
        </Box>
      ))}
    </Box>
  );

  return (
    <CustomPopover
      classes={{
        root: `${classes.root} ${props.classes?.root}`,
        paper: classes.popover,
        tail: classes.popupColor,
        closeButton: classes.closePopupButton,
        backdropRoot: classes.backdrop,
      }}
      anchorEl={anchorEl}
      onClose={(): void => {
        onClose();
      }}
      ptasVariant="info"
      showCloseButton
      tail
      tailPosition="end"
      anchorOrigin={{
        vertical: 'bottom',
        horizontal: 'right',
      }}
      transformOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
    >
      <Alert
        classes={{
          root: clsx(classes.alert, classes.popupColor),
        }}
        contentText={renderBusinessList(businessList)}
        ptasVariant="info"
      />
    </CustomPopover>
  );
}

export default withStyles(styles)(BusinessAccessPopover) as FC<
  GenericWithStyles<Props>
>;
