// Home.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import {
  CustomCard,
  CustomTextButton,
  PropertySearcher,
} from '@ptas/react-public-ui-library';
import { Box, makeStyles, Theme } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import * as fm from './formatMessage';
import { useHI, withHomeImprovementProvider } from './HomeImprovementContext';
import ProfileOptionsButton from 'components/ProfileOptionsButton';
import HICardPropertyInfo from './HICardPropertyInfo';
import { STATUS_CODE_APP_CREATED } from './constants';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    maxWidth: 655,
    borderRadius: 0,
    display: 'block',
    boxSizing: 'border-box',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    paddingTop: theme.spacing(1),
    marginBottom: 15,

    [theme.breakpoints.up('sm')]: {
      borderRadius: 24,
      marginTop: 8,
    },
  },
  contentWrap: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
  },
  header: {
    width: '100%',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
  },
  title: {
    fontSize: theme.ptas.typography.h5.fontSize,
    fontFamily: theme.ptas.typography.titleFontFamily,
    marginBottom: 32,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    margin: 0,

    // [theme.breakpoints.up('sm')]: {
    //   fontSize: theme.ptas.typography.h2.fontSize,
    // },
  },
  indication: {
    fontSize: theme.ptas.typography.bodySmallBold.fontSize,
    fontWeight: theme.ptas.typography.bodySmall.fontWeight,
    lineHeight: '19px',
    marginTop: theme.spacing(2),
    marginBottom: theme.spacing(4),
    width: '100%',
    maxWidth: 310,
    textAlign: 'start',
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    width: '70%',
    marginBottom: 8,
  },
  textHelp: {
    fontSize: theme.ptas.typography.body.fontSize,
    fontWeight: theme.ptas.typography.bodyBold.fontWeight,
    marginBottom: theme.spacing(2),
  },
  cardWrapper: {
    width: '100%',
    maxWidth: '100%',
    display: 'flex',
    justifyContent: 'center',
    flexWrap: 'wrap',

    // [theme.breakpoints.up('sm')]: {
    //   width: '100%',
    //   maxWidth: 967,
    //   marginLeft: 'auto',
    //   marginRight: 'auto',
    //   flexWrap: 'nowrap',
    //   display: 'flex',
    //   justifyContent: 'flex-end',
    // },
  },
}));

function HomeImprovement(): JSX.Element {
  const {
    currentHiApplication,
    handleClickOnInstructions,
    handleOnSearchChange,
    searchList,
    applicationList,
    onSearchItemSelected,
    loading,
    searchText,
    statusCodes,
  } = useHI();
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
          <Box className={classes.header}>
            <h4 className={classes.title}>{fm.title}</h4>
            <ProfileOptionsButton />
          </Box>
          {!!applicationList?.length &&
            applicationList.map((application) => {
              // const parcelInfo = parcelData.find(
              //   (p) => p.ptas_parceldetailid === application.parcelId
              // );
              return (
                <HICardPropertyInfo
                  key={application.homeImprovementId}
                  application={application}
                  homeImprovementId={application.homeImprovementId}
                  selected={
                    currentHiApplication?.homeImprovementId ===
                    application.homeImprovementId
                  }
                  complete={
                    application.statusCode ===
                    statusCodes.get(STATUS_CODE_APP_CREATED)
                  }
                />
              );
            })}
          <span className={classes.border} style={{ marginBottom: 32 }}></span>
          <PropertySearcher
            label={fm.findMyProperty}
            textButton={fm.locateMe}
            textDescription={fm.enterAddress}
            value={''}
            onChange={handleOnSearchChange}
            suggestion={{
              List: searchList,
              onSelected: onSearchItemSelected,
              loading: !!searchText && loading,
            }}
            onClickSearch={(): void => {
              console.log('searcher icon was clicked');
            }}
            onClickTextButton={(): void => {
              console.log('locate me button was clicked');
            }}
          />
          <span className={classes.indication}>{fm.indication}</span>
          <span className={classes.border}></span>
          <span className={classes.textHelp}>{fm.help}</span>
          <CustomTextButton onClick={handleClickOnInstructions}>
            {fm.instruction}
          </CustomTextButton>
        </CustomCard>
      </div>
    </MainLayout>
  );
}

export default withHomeImprovementProvider(HomeImprovement);
