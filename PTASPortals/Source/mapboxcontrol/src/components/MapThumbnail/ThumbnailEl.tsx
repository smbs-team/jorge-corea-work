/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { CardMedia, makeStyles, Theme, useTheme } from '@material-ui/core';
import React from 'react';
import { parcelUtil } from 'utils';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    textAlign: 'center',
    height: 240,
    width: 310,
    color: theme.ptas.colors.theme.white,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    position: 'relative',
    display: 'flex',
    flexDirection: 'column',
  },
  header: {
    fontWeight: theme.ptas.typography.h5.fontWeight,
    fontSize: '1.5rem',
    zIndex: 1,
    flexGrow: 1,
  },
  cardMedia: {
    height: '100%',
    width: '100%',
    position: 'absolute',
  },
  footer: {
    zIndex: 1,
    marginBottom: 6,
    overflow: 'auto',
  },
}));

type Props = {
  pin: string;
  imageUrl: string;
  footerText: {
    lineOne: string;
    lineTwo: string;
  };
};

function ThumbnailEl({ pin, imageUrl, footerText }: Props): JSX.Element {
  const classes = useStyles(useTheme());
  return (
    <div className={classes.root} key={pin}>
      <CardMedia
        classes={{ root: classes.cardMedia }}
        image={imageUrl}
        title={pin}
      />
      <header className={classes.header}>{parcelUtil.formatPin(pin)}</header>
      <footer className={classes.footer}>
        <div>{footerText.lineOne}</div>
        <div>{footerText.lineTwo}</div>
      </footer>
    </div>
  );
}

export default ThumbnailEl;
