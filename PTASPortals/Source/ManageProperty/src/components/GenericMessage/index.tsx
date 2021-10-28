// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomCard } from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core';
import MainLayout from 'components/Layouts/MainLayout';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    display: 'block',
    boxSizing: 'border-box',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    paddingTop: 32,
    marginBottom: 15,
    width: 486,
    borderRadius: 24,
  },
  contentWrap: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
  },
  indication: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    marginTop: 8,
    marginBottom: 32,
    width: '100%',
    maxWidth: 228,
    textAlign: 'center',
    color: theme.ptas.colors.theme.black,
  },
  cardWrapper: {
    marginTop: 40,
    width: '100%',
    maxWidth: 967,
    marginLeft: 'auto',
    marginRight: 'auto',
    flexWrap: 'nowrap',
    display: 'flex',
    justifyContent: 'center',
  },
}));

interface Props {
  msg: JSX.Element | string;
}

function GenericMessage(props: Props): JSX.Element {
  const { msg } = props;
  const classes = useStyles();

  return (
    <MainLayout>
      <div className={classes.cardWrapper}>
        <CustomCard
          variant="wrapper"
          classes={{
            rootWrap: classes.card,
            wrapperContent: classes.contentWrap,
          }}
        >
          <span className={classes.indication}>{msg}</span>
        </CustomCard>
      </div>
    </MainLayout>
  );
}

export default GenericMessage;
