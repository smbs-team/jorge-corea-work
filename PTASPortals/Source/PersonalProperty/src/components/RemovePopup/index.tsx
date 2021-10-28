// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment } from 'react';
import {
  createStyles,
  StyleRules,
  Theme,
  WithStyles,
  withStyles,
  Popper,
} from '@material-ui/core';
import { GenericWithStyles } from '@ptas/react-public-ui-library';

interface Props extends WithStyles<typeof styles> {
  // anchorEl: HTMLElement | null | undefined;
  anchorEl: HTMLElement;
  alertContent: string | React.ReactNode;
  onClose: () => void;
  onButtonClick: () => void;
  buttonText?: string | React.ReactNode;
}

const styles = (theme: Theme): StyleRules<string, Props> =>
  createStyles({
    removePopover: {
      borderRadius: '9px',
    },
    removeAlert: {
      width: '304px',
      boxSizing: 'border-box',
      padding: '16px 32px 27px 32px',
      borderRadius: '9px',

      fontSize: theme.ptas.typography.bodyLargeBold.fontSize,
      fontWeight: theme.ptas.typography.bodyLargeBold.fontWeight,
      lineHeight: '24px',
    },
    removeAlertButton: {
      width: 'auto',
      height: 38,
      marginLeft: 'auto',
      marginRight: 'auto',
      display: 'block',
      fontSize: '18px',
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      lineHeight: '18px',
      padding: '10px 32px 10px 32px',
    },

    arrow: {
      visibility: 'hidden',

      position: 'absolute',
      width: '8px',
      height: '8px',
      background: 'inherit',

      '&::before': {
        position: 'absolute',
        width: '8px',
        height: '8px',
        background: 'inherit',

        visibility: 'visible',
        content: '',
        transform: 'rotate(45deg)',
      },
    },
  });

function RemovePopup(props: Props): JSX.Element {
  // const { classes } = props;

  return (
    <Popper
      open={Boolean(props.anchorEl)}
      placement="bottom-end"
      disablePortal={false}
      anchorEl={props.anchorEl}
      modifiers={{
        flip: {
          enabled: true,
        },
        preventOverflow: {
          enabled: true,
          boundariesElement: 'scrollParent',
        },
        arrow: {
          enabled: true,
          element: props.anchorEl,
        },
      }}
    >
      <Fragment>
        <div id="tooltip" role="tooltip">
          My tooltip
          <div className="arrow" data-popper-arrow></div>
        </div>
        {/* <Alert
          contentText={props.alertContent}
          ptasVariant="danger"
          classes={{
            root: classes.removeAlert,
            // text: classes.removeAlertText,
            buttons: classes.removeAlertButton,
          }}
          okShowButton
          okButtonText={props.buttonText}
          okButtonClick={(): void => {
            props.onButtonClick();
          }}
        /> */}
      </Fragment>
    </Popper>
  );
}

export default withStyles(styles)(RemovePopup) as FC<GenericWithStyles<Props>>;
