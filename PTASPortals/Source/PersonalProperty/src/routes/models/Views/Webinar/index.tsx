// Webinar.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomCard, CustomButton } from '@ptas/react-public-ui-library';
import { IconButton } from '@material-ui/core';
import CloseIcon from '@material-ui/icons/Close';
import MainLayout from '../../../../components/Layouts/MainLayout';
import { useHistory } from 'react-router-dom';
import * as fm from './formatText';
import useStyles from './styles';

function Webinar(): JSX.Element {
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
        <div className={classes.wrapper}>
          <span className={classes.subtitle}>{fm.joinOurWebinar}</span>
          <ul className={classes.list}>
            <li className={classes.item}>{fm.personalPropertyIntroduction}</li>
            <li className={classes.item}>{fm.listingTips}</li>
            <li className={classes.item}>{fm.commonListing}</li>
          </ul>
          <span className={classes.date}>{fm.date}</span>
          <CustomButton classes={{ root: classes.joinWebinarButton }}>
            {fm.joinWebinar}
          </CustomButton>
        </div>
      </CustomCard>
    </MainLayout>
  );
}

export default Webinar;
