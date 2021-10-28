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
    display: 'block',
    boxSizing: 'border-box',
    borderRadius: 0,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    paddingTop: 16,
    marginBottom: 15,
    maxWidth: 640,
    marginLeft: 'auto',
    marginRight: 'auto',
    marginTop: 0,

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
    width: '100%',
    margin: '0 auto',
  },
  header: {
    width: '100%',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    marginBottom: 32,
  },
  title: {
    color: theme.ptas.colors.theme.black,
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontSize: theme.ptas.typography.h5.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '35px',
    margin: 0,
  },
  name: {
    fontSize: '14px !important',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '17px',
  },
  inputWrap: {
    width: '100%',
    maxWidth: 336,
    background: theme.ptas.colors.theme.white,
    padding: 8,
    border: '0.5px solid rgba(0, 0, 0, 0.16)',
    borderRadius: 9,
    boxShadow: '0px 16px 32px rgba(0, 0, 0, 0.2)',
    boxSizing: 'border-box',
  },
  uploadContentWrap: {
    margin: '0 auto',
    width: '100%',
    maxWidth: 230,
  },
  fileUploadRoot: {
    margin: 0,
    marginBottom: 16,
  },
  uploadTitle: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    marginBottom: 4,
    display: 'block',
  },
  uploadDesc: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    marginBottom: 4,
    display: 'block',
    color: theme.ptas.colors.theme.gray,
    textAlign: 'center',
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    width: '100%',
    maxWidth: 320,
    marginBottom: 10,
    display: 'block',
  },
  instructionButton: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
  },
  buttonWrap: {
    marginLeft: 'auto',
    marginRight: 'auto',
    width: 100,
  },
  instructionCardWrap: {
    padding: '16px 22px',
    margin: '0 auto',
    background: theme.ptas.colors.theme.grayLightest,
    boxShadow: 'inset 8px 8px 16px rgba(0, 0, 0, 0.04)',
    borderRadius: 9,
    boxSizing: 'border-box',
    width: '100%',
    maxWidth: 244,
  },
  instructionTitle: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    marginBottom: 16,
    display: 'block',
  },
  instructionList: {
    margin: 0,
    paddingLeft: 16,
  },
  instructionListItem: {
    marginBottom: 16,
    '&:last-child': {
      marginBottom: 0,
    },
  },
  popover: {
    left: 'calc(100% - 99.7%) !important',
    top: '11px !important',
    width: '99.3%',
    height: '100%',
    borderRadius: '24px 24px 0 0',
    overflow: 'hidden',
    maxHeight: '100%',
    maxWidth: '100%',
  },
  closeIcon: {
    color: theme.ptas.colors.theme.black,
    width: 20,
  },
  popoverHeader: {
    width: '100%',
    paddingTop: 16,
    paddingBottom: 17,
    paddingLeft: 0,

    [theme.breakpoints.up('sm')]: {
      paddingLeft: 16,
    },
  },
  closeButton: {
    right: '12px !important',
    top: '8px !important',
  },
  headerContent: {
    display: 'flex',
    flexDirection: 'column',
    width: '100%',
    justifyContent: 'center',
    alignItems: 'center',

    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
      justifyContent: 'flex-start',
      alignItems: 'center',
    },
  },
  popoverContent: {
    width: '100%',
    height: '91%',

    background: '#F0F0F0',
    boxShadow: 'inset 0px 2px 16px rgba(0, 0, 0, 0.25)',
    padding: 20,
    boxSizing: 'border-box',
    paddingBottom: 70,
  },
  headerTitle: {
    display: 'block',
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    fontSize: '1.5rem',
  },
  headerDescription: {
    display: 'block',
    fontSize: theme.ptas.typography.body.fontSize,
    fontFamily: theme.ptas.typography.bodyFontFamily,
  },
  textWrap: {
    width: '100%',
    textAlign: 'center',
    [theme.breakpoints.up('sm')]: {
      width: 'calc(45.6% - 16px)',
      textAlign: 'start',
    },
  },
  saveButton: {
    marginTop: 10,
    width: 165,
    height: 38,
    marginRight: '32px !important',

    [theme.breakpoints.up('sm')]: {
      margin: 'auto 0',
    },
  },
  errorButton: {
    width: 225,
    background: `${theme.ptas.colors.utility.danger} !important`,
    color: `${theme.ptas.colors.theme.white} !important`,
  },
  backdropRoot: {
    background:
      'linear-gradient(180deg, rgba(0, 0, 0, 0.1) 0%, rgba(0, 0, 0, 0.4) 100%)',
  },
  downloadTemplateLink: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    color: theme.ptas.colors.theme.accent,
    lineHeight: '19px',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    display: 'block',
    marginBottom: 16,
    textDecoration: 'none',
    '. &:hover': {
      color: theme.ptas.colors.theme.accent,
    },
  },
}));
