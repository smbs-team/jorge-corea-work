// Tutorial.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomCard, Multimedia } from '@ptas/react-public-ui-library';
import { IconButton } from '@material-ui/core';
import CloseIcon from '@material-ui/icons/Close';
import MainLayout from '../../../../components/Layouts/MainLayout';
import { useHistory } from 'react-router-dom';
import * as fm from './formatText';
import useStyles from './styles';

function Tutorial(): JSX.Element {
  const classes = useStyles();
  const history = useHistory();

  const handleClick = (): void => {
    history.goBack();
  };

  return (
    <MainLayout>
      <CustomCard
        variant="wrapper"
        classes={{
          rootWrap: classes.card,
          wrapperContent: classes.contentWrap,
        }}
      >
        <IconButton className={classes.closeButton} onClick={handleClick}>
          <CloseIcon className={classes.closeIcon} />
        </IconButton>

        <h2 className={classes.title}>{fm.businessPersonalProperty}</h2>
        <span className={classes.border}></span>
        <Multimedia
          // todo: then replace with the correct one
          video="https://vimeo.com/382824142"
          showTime
          showTitle
          width={'563'}
          height={'422'}
          responsive
        />
      </CustomCard>
    </MainLayout>
  );
}

export default Tutorial;
