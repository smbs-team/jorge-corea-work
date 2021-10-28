/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import { makeStyles } from "@material-ui/core";

export const useStyles = makeStyles((theme) => ({
    root: {
      display: 'flex',
      flexDirection: 'column',
      marginTop: '10px',
    },
    salesDataBottomRow: {
      display: 'flex',
      alignItems: 'center',
      marginBottom: theme.spacing(2.625),
    },
    salesDataCustomTabs: {
      width: 'unset !important',
      height: '30px',
    },
    salesLabel: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: '0.875rem',
      fontWeight: 'bold',
      marginRight: theme.spacing(1.5),
    },
    numberNameTextField: {
      bottom: 7,
      marginLeft: theme.spacing(2),
      marginRight: theme.spacing(2),
      width: 169,
    },
    popover: {
      width: 400,
    },
    areaList: {
      '& li:focus': {
        backgroundColor: 'red',
      },
    },
    popoverPaper: {
      borderRadius: 9,
      padding: theme.spacing(1),
      paddingTop: 0,
    },
    chipContainer: {
      display: 'flex',
      flexWrap: 'wrap',
    },
    noDataBox: {
      fontSize: '1rem',
      padding: 16,
    },
    addIcon: {
      color: theme.ptas.colors.theme.accent,
    },
    emptyChipContainer: {
      height: 40,
    },
  }));