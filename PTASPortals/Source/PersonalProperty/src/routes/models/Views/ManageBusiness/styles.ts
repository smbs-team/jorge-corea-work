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
    maxWidth: 702,
    marginLeft: 'auto',
    marginRight: 'auto',
    backgroundColor: theme.ptas.colors.theme.white,
    [theme.breakpoints.up('sm')]: {
      borderRadius: 24,
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
  },
  title: {
    color: theme.ptas.colors.theme.gray,
    fontFamily: theme.ptas.typography.titleFontFamily,
    // fontSize: theme.ptas.typography.h6.fontSize,
    fontSize: '20px',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '23px',
    margin: 0,
    marginBottom: 32,
    textAlign: 'center',
  },
  name: {
    fontSize: '14px',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '17px',
  },
  back: {
    fontSize: '18px',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '18px',
  },
  content: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    marginBottom: theme.spacing(4),
  },
  businessName: {
    marginBottom: theme.spacing(1),
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontSize: '24px',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '28px',
  },
  propertyAccount: {
    marginBottom: theme.spacing(1),
    fontFamily: theme.ptas.typography.bodyFontFamily,
    lineHeight: '17px',
    fontSize: '14px',
    color: 'rgba(0, 0, 0, 0.5)',
  },
  dates: {
    marginBottom: theme.spacing(1),
    fontSize: theme.ptas.typography.bodySmallBold.fontSize,
    fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
    lineHeight: '19px',
  },
  filedDate: {
    marginRight: theme.spacing(4),
  },
  document: {
    cursor: 'pointer',
  },
  documentsCard: {
    width: '345px',
    backgroundColor: theme.ptas.colors.theme.grayLightest,
  },
  documentsCardContent: {
    padding: theme.spacing(0, 0.5),
    paddingBottom: '0px !important',
  },
  documentsWrapper: {
    display: 'flex',
    flexWrap: 'wrap',
    justifyContent: 'space-around',
    color: theme.ptas.colors.theme.accent,
    '&>*': {
      marginTop: '8px',
      marginBottom: '8px',
    },
  },
  documentIcon: {
    width: '18px',
    height: '18px',
    verticalAlign: 'text-top',
    marginRight: theme.spacing(0.5),
  },
  documentName: {
    fontSize: theme.ptas.typography.bodySmallBold.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '19px',
  },
  updateListingInfo: {
    width: 207,
  },
  labelOr: {
    fontSize: '20px',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '23px',
    margin: theme.spacing(0, 2),
  },
  attachListingInfo: {
    width: 207,
    background: 'linear-gradient(90deg, #CDF4FF 0%, #EBF9FD 100.15%)',
  },
  attachListingInfoLabel: {
    color: theme.ptas.colors.theme.accent,
    // fontSize: theme.ptas.typography.buttonLarge.fontSize,
    fontSize: '18px',
    lineHeight: '18px',
  },
  cloudUpload: {
    width: 18,
    height: 18,
    verticalAlign: 'bottom',
    marginRight: theme.spacing(0.5),
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    width: '100%',
    maxWidth: 520,
    margin: theme.spacing(4, 0),
    display: 'block',
  },
  removeCard: {
    marginTop: theme.spacing(4),
    width: '100%',
    maxWidth: 520,
    backgroundColor: theme.ptas.colors.theme.grayLightest,
  },
  removeCardContent: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    padding: theme.spacing(1, 0),
    paddingBottom: '8px !important',
  },
  businessMoved: {
    marginBottom: theme.spacing(4),
    fontSize: theme.ptas.typography.bodySmallBold.fontSize,
    fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
    lineHeight: '19px',
    '&>span': {
      color: theme.ptas.colors.theme.accent,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    },
    cursor: 'pointer',
  },
  removeButton: {
    color: theme.ptas.colors.utility.danger,
    fontSize: theme.ptas.typography.bodySmallBold.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: '19px',
  },
  buttonsWrap: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
    },
  },
  instructionCard: {
    width: '100%',
    maxWidth: 224,
    boxSizing: 'border-box',
    padding: '16px 22px',
    borderRadius: 9,
    background: theme.ptas.colors.theme.grayLightest,
    boxShadow: 'inset 8px 8px 16px rgba(0, 0, 0, 0.04)',
    marginBottom: 8,
    marginTop: 2,
  },
  instructionTitle: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontSize: theme.ptas.typography.body.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    marginBottom: 16,
    display: 'block',
    width: 160,
  },
  instructionList: {
    padding: 0,
    paddingLeft: 14,
    marginBottom: 0,
  },
  instructionItem: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    marginBottom: 15,
    '&:last-child': {
      marginBottom: 0,
    },
  },
  inputFileWrap: {
    width: '100%',
    maxWidth: 246,
    background: theme.ptas.colors.theme.white,
    boxSizing: 'border-box',
    boxShadow: ' 0px 16px 32px rgba(0, 0, 0, 0.2)',
    padding: '31px 9px 8px 9px',
    marginTop: 12,
    border: ' 0.5px solid rgba(0, 0, 0, 0.16)',
    borderRadius: 9,
    position: 'relative',
  },
  closeIcon: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
  },
  closeButton: {
    position: 'absolute',
    top: -4,
    right: -4,
    color: `${theme.ptas.colors.theme.black} !important`,
  },
  fileUploadRoot: {
    margin: 0,
  },
  link: {
    color: theme.ptas.colors.theme.accent,
    textDecoration: 'none',
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    '&::hover': {
      color: theme.ptas.colors.theme.accent,
      textDecoration: 'none',
    },
  },
  downloadTemplate: {
    color: 'rgba(0, 0, 0, 0.5)',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    lineHeight: '19px',
    fontSize: theme.ptas.typography.bodySmall.fontSize,
  },
  instructionSpace: {
    paddingRight: 4,
  },
  attachListingFile: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    display: 'block',
    marginBottom: 5,
  },
  rootPreview: {
    margin: '8px 0',
  },
  saveAttachBtn: {
    marginTop: '35px',
    marginBottom: '10px',
  },
}));
