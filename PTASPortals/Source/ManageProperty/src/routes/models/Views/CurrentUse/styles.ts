//styles.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { makeStyles, Theme } from '@material-ui/core/styles';

/**
 * Component styles
 */
export const useStyles = makeStyles((theme: Theme) => ({
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
  contentWrapper: {
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
  signInButton: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
  },
  cardPropertyInfo: {
    width: '100%',
    maxWidth: 623,
    marginBottom: theme.spacing(4),
  },
  cardPropertyInfoContent: {
    background: 'rgba(0,0,0,0)',
  },
  cardPropertyInfoChildrenContent: {
    background: 'rgba(255, 255, 255, 0.5)',
  },
  indication: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    marginTop: 16,
    marginBottom: 32,
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
  exemptionContent: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    width: '100%',
  },
  availableFormsLabel: {
    width: 315,
    // fontSize: theme.ptas.typography.formLabel.fontSize,
    // fontWeight: theme.ptas.typography.formLabel.fontWeight,
    fontSize: theme.ptas.typography.finePrintBold.fontSize,
    fontWeight: theme.ptas.typography.finePrintBold.fontWeight,
    marginBottom: theme.spacing(0.5),
    lineHeight: '15px',
    textAlign: 'center',
    [theme.breakpoints.up('sm')]: {
      textAlign: 'start',
    },
  },
  hightlightLink: {
    color: theme.ptas.colors.theme.accent,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    cursor: 'pointer',
  },
  farmLand: {
    width: '100%',
    maxWidth: 470,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    margin: theme.spacing(2, 0, 0, 0),
  },
  farmLandInstructions: {
    width: '100%',
    maxWidth: 311,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodySmall.fontWeight,
    color: 'rgba(0, 0, 0, 0.5)',
  },
  farmLandInstructionsLabel: {
    marginBottom: theme.spacing(1),
  },
  forestLand: {
    width: '100%',
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    marginTop: 16,
    marginBottom: 24,
  },
  forestLandInstructions: {
    width: '100%',
    maxWidth: 340,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodySmall.fontWeight,
    color: 'rgba(0, 0, 0, 0.5)',
  },
  forestLandInstructionsLabel: {
    marginBottom: theme.spacing(1),
  },
  openSpace: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    width: '100%',
    marginTop: theme.spacing(2),
  },
  openSpaceInstructions: {
    width: '100%',
    maxWidth: 251,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodySmall.fontWeight,
    color: 'rgba(0, 0, 0, 0.5)',
  },
  openSpaceInstructionsLabel: {
    marginBottom: theme.spacing(1),
  },
  doneButton: {
    width: 135,
    marginTop: theme.spacing(3),
  },
}));
