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
    borderRadius: 0,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    marginBottom: 15,

    [theme.breakpoints.up('sm')]: {
      borderRadius: 24,
      marginTop: 8,
    },
  },
  cardHeader: {},
  headerLine: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    width: '100%',
    marginBottom: '32px',
  },
  title: {
    color: theme.ptas.colors.theme.gray,
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '23px',
    margin: 0,
  },
  name: {
    fontSize: '14px !important',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '17px',
  },
  back: {
    fontSize: theme.ptas.typography.body.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '18px',
  },
  cardContent: {},
  content: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    paddingBottom: '32px',
  },
  tab: {
    width: 270,
    padding: theme.spacing(4, 0, 4, 0),
  },
  tabName: {
    marginTop: 40,
    marginBottom: 49,
    padding: 0,
  },
  textButtonContainer: {
    display: 'flex',
    justifyContent: 'flex-end',
    marginBottom: theme.spacing(0.5),
  },
  textButton: {
    fontSize: theme.ptas.typography.bodySmallBold.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '19px',
  },

  formInput: {
    maxWidth: '100%',
    marginBottom: theme.spacing(3.125),
  },
  continueButton: {
    width: 135,
  },
  doneButton: {
    width: 107,
  },
  line: {
    display: 'flex',
    justifyContent: 'space-between',
    marginBottom: theme.spacing(3.125),
    flexDirection: 'column',
    '& > div': {
      marginBottom: 10,
    },

    '& > div:last-child': {
      marginBottom: 0,
    },

    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
      '& > div': {
        marginBottom: 0,
      },
    },
  },
  addressLine: {
    maxWidth: '270px',
  },
  lastLine: {
    marginBottom: theme.spacing(2.125),
  },
  city: {
    width: '107px',
  },
  state: {
    width: '45px',
  },
  statePadding: {
    padding: 0,

    '& > fieldset': {
      padding: 0,
    },
  },
  shrink: {
    left: -6,
  },
  zip: {
    width: '101px',
  },

  wrapper: {
    maxWidth: 270,
    width: 270,
  },
}));
