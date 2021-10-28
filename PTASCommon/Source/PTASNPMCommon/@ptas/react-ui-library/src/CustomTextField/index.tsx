import React from "react";
import {
  createStyles,
  Theme,
  withStyles,
  WithStyles
} from "@material-ui/core/styles";
import { TextField, TextFieldProps } from "@material-ui/core";
import { omit } from "lodash";
import { GenericWithStyles } from "../common";

type Props = WithStyles<typeof useStyles> & TextFieldProps;
export type CustomTextFieldProps = GenericWithStyles<Props>;

const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      "& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderColor: theme.ptas.colors.theme.black
      },
      "&:hover .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderColor: theme.ptas.colors.theme.black
      },
      "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline": {
        borderColor: theme.ptas.colors.utility.selection
      },
      "& .MuiOutlinedInput-root.Mui-focused": {
        color: "black"
      },
      "& .Mui-focused": {
        color: theme.ptas.colors.utility.selection
      },
      "& .MuiInputBase-input": {
        color: "black"
      }
    },
    inputRoot: {},
    input: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      padding: theme.spacing(9 / 8)
    },
    label: {
      color: theme.ptas.colors.theme.grayMedium,
      fontWeight: "normal",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      borderRadius: "3px"
    },
    animated: {},
    shrink: {},
    helperText: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      marginTop: 0
    }
  });

function CustomTextField(props: Props): JSX.Element {
  const { classes } = props;
  return (
    <TextField
      variant='outlined'
      size='small'
      className={classes.root}
      InputProps={{
        className: classes.label,
        classes: {
          root: classes.inputRoot,
          input: classes.input
        },
        color: "secondary"
      }}
      InputLabelProps={{
        classes: {
          root: classes.label,
          animated: classes.animated,
          shrink: classes.shrink
        },
        color: "secondary"
      }}
      FormHelperTextProps={{ classes: { root: classes.helperText } }}
      {...{ ...omit(props, "classes"), classes: { root: classes.root } }}
    />
  );
}
export default withStyles(useStyles)(CustomTextField);
