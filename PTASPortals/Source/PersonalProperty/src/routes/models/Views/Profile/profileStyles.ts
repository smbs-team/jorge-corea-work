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
  titleWrapper: {
    margin: 0,
    // marginBottom: 32,
    textAlign: 'center',
  },
  title: {
    margin: 0,
    fontFamily: theme.ptas.typography.titleFontFamily,
    // fontSize: theme.ptas.typography.h6.fontSize,
    fontSize: '20px',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '23px',
  },
  colorGray: {
    color: theme.ptas.colors.theme.gray,
  },
  colorBlack: {
    color: theme.ptas.colors.theme.black,
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
  continueButton: {
    width: 135,
    marginTop: theme.spacing(2),
    marginBottom: theme.spacing(4),
  },
  doneButton: {
    width: 107,
    marginBottom: theme.spacing(4),
  },
  tabWideItem: {
    padding: theme.spacing(0, 4),
  },
  textButton: {
    marginBottom: theme.spacing(4),
    color: theme.ptas.colors.utility.danger,
    fontSize: theme.ptas.typography.bodySmallBold.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '19px',
  },
}));
