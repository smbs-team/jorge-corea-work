// Exemptions.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { CustomSwitch } from '@ptas/react-public-ui-library';
import * as fm from './formatText';
import * as fmGeneral from '../../../../GeneralFormatMessage';
import { makeStyles, Theme } from '@material-ui/core';
import clsx from 'clsx';

const useStyles = makeStyles((theme: Theme) => ({
  exemptionsWrap: {
    width: '100%',
    maxWidth: 802,
    marginBottom: 56,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
  },
  title: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    marginBottom: 16,
    display: 'block',
  },
  rwcExemption: {
    display: 'block',
    marginBottom: 16,
    color: theme.ptas.colors.theme.accent,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
  },
  description: {
    marginBottom: 20,
    display: 'block',
  },
  instruction: {
    display: 'block',
    '&::before': {
      content: `'-'`,
      marginRight: 8,
    },
  },
  marginTop: {
    marginTop: 15,
  },
  list: {
    marginTop: 0,
    paddingLeft: 28,
  },
  item: {
    marginBottom: 20,
    '&:last-child': {
      marginBottom: 0,
    },
  },
  switchFormControl: {
    marginBottom: 49,
  },
  likeToApplyFormControl: {
    marginBottom: 49,
  },
  equipmentExemptionDes: {
    marginBottom: 20,
  },
}));

function Exemptions(): JSX.Element {
  const classes = useStyles();

  return (
    <div className={classes.exemptionsWrap}>
      <span className={classes.title}>{fm.headOfFamily}</span>
      <span className={classes.rwcExemption}>{fm.rcwExemptions}</span>
      <span className={classes.description}>{fm.exemptionsApplies}</span>
      <span className={classes.instruction}>{fm.mustBeAppliedForAnnually}</span>
      <span className={classes.instruction}>{fm.onlyOneExemption}</span>
      <span className={classes.instruction}>
        {fm.exemptionIsOnlyAllowedForOneProperty}
      </span>
      <span className={clsx(classes.description, classes.marginTop)}>
        {fm.followingQualification}
      </span>
      <ol className={classes.list}>
        <li className={classes.item}>{fm.livingWithASpouseOrDependent}</li>
        <li className={classes.item}>{fm.residingContinually}</li>
        <li className={classes.item}>{fm.survivingSpouse}</li>
      </ol>
      <CustomSwitch
        onLabel={fmGeneral.yes}
        offLabel={fmGeneral.no}
        label={fm.qualifyForThisExemption}
        ptasVariant="small"
        showOptions
        classes={{
          formControlRoot: classes.switchFormControl,
        }}
      />
      <span className={classes.title}>{fm.farmMachinery}</span>
      <span className={classes.rwcExemption}>{fm.rcwExemptions2}</span>
      <p className={classes.equipmentExemptionDes}>
        {fm.exemptsQualifyingFarmingMachinery}
      </p>
      <CustomSwitch
        onLabel={fmGeneral.yes}
        offLabel={fmGeneral.no}
        label={fm.likeToApply}
        ptasVariant="small"
        showOptions
      />
    </div>
  );
}

export default Exemptions;
