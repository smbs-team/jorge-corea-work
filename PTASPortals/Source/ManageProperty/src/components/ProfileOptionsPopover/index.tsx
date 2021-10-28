// ProfileOptionsPopover.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment } from 'react';
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
import ArrowForwardIosIcon from '@material-ui/icons/ArrowForwardIos';
import useAuth from 'auth/useAuth';

export type ProfileOption = 'editProfile' | 'signOut';

/**
 * Component props
 */
interface Props extends WithStyles<typeof styles> {
  anchorEl: HTMLElement | null | undefined;
  onClose: () => void;
  onClick: (option: ProfileOption) => void;
  editProfileText?: string;
  signOutText?: string;
}

/**
 * Component styles
 */
const styles = (theme: Theme): StyleRules<string, object> =>
  createStyles({
    itemContainer: {
      width: 270,
      marginBottom: theme.spacing(2),
      padding: theme.spacing(0, 3),
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: theme.ptas.typography.body.fontWeight,
    },
    item: {
      cursor: 'pointer',
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
      padding: '24px 20px 20px 22px',
      borderRadius: '9px',
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '19px',
    },
    buttonManageContainer: {
      display: 'flex',
      justifyContent: 'center',
    },
    buttonManage: {
      width: 'auto',
      height: 23,
      padding: '3px 22px',
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '17px',
      marginTop: theme.spacing(2),
      color: theme.ptas.colors.theme.accent,
    },
    border: {
      background:
        'linear-gradient(90deg, rgba(255, 255, 255, 0) 0%, #FFFFFF 49.48%, rgba(255, 255, 255, 0) 100%)',
      height: 1,
      display: 'block',
      width: '270px',
      margin: theme.spacing(0, 3),
    },
    arrowIcon: {
      fontSize: 14,
      marginBottom: '-1px',
    },
  });

/**
 * ProfileOptionsPopover
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ProfileOptionsPopover(props: Props): JSX.Element {
  const { classes, anchorEl, onClose, onClick } = props;
  const { signOut } = useAuth() ?? {};

  const SignOut = (): void => {
    signOut && signOut();
  };

  const editProfileContent = (
    <Fragment>
      <Box className={classes.itemContainer}>
        <span
          className={classes.item}
          onClick={(): void => onClick('editProfile')}
        >
          {props.editProfileText ?? 'Edit profile'}{' '}
          <ArrowForwardIosIcon className={classes.arrowIcon} />
        </span>
      </Box>
      <span className={classes.border}></span>
    </Fragment>
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
          buttons: classes.buttonManage,
          buttonContainer: classes.buttonManageContainer,
        }}
        contentText={editProfileContent}
        ptasVariant="info"
        okShowButton
        okButtonText={props.signOutText ?? 'Sign out'}
        okButtonClick={SignOut}
      />
    </CustomPopover>
  );
}

export default withStyles(styles)(ProfileOptionsPopover) as FC<
  GenericWithStyles<Props>
>;
