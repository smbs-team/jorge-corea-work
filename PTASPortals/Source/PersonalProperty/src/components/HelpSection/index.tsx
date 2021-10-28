// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { Box, makeStyles, Theme } from '@material-ui/core';
import { CustomTextButton } from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import { useHistory } from 'react-router-dom';

type Props = {
  hideSeparator?: boolean;
};

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    fontFamily: theme.ptas.typography.bodyFontFamily,
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    width: '100%',
    maxWidth: 320,
    marginBottom: 8,
    display: 'block',
  },
  textHelp: {
    fontSize: theme.ptas.typography.body.fontSize,
    fontWeight: theme.ptas.typography.bodyBold.fontWeight,
    marginBottom: 17,
    display: 'block',
  },
  buttonWrap: {
    width: 281,
    display: 'flex',
    justifyContent: 'space-between',
  },
  buttonOptions: {
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
  },
}));

function HelpSection(props: Props): JSX.Element {
  const classes = useStyles();
  const history = useHistory();

  const goToInstructions = (): void => {
    history.push('/instruction');
  };

  const goToWebinar = (): void => {
    history.push('/webinar');
  };

  const goToTutorial = (): void => {
    history.push('/tutorial');
  };

  return (
    <Box className={classes.root}>
      {!props.hideSeparator && <span className={classes.border}></span>}
      <span className={classes.textHelp}>{fm.help}</span>
      <div className={classes.buttonWrap}>
        <CustomTextButton
          classes={{ root: classes.buttonOptions }}
          onClick={goToInstructions}
        >
          {fm.instructions}
        </CustomTextButton>
        <CustomTextButton
          classes={{ root: classes.buttonOptions }}
          onClick={goToTutorial}
        >
          {fm.tutorial}
        </CustomTextButton>
        <CustomTextButton
          classes={{ root: classes.buttonOptions }}
          onClick={goToWebinar}
        >
          {fm.webinar}
        </CustomTextButton>
      </div>
    </Box>
  );
}

export default HelpSection;
