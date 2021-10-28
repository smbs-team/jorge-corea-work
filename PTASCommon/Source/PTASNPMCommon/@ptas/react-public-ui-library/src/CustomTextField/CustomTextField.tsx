import React, { Fragment, useState, FC, useCallback, forwardRef } from "react";
import { Theme } from "@material-ui/core/styles";
import {
  createStyles,
  TextField,
  WithStyles,
  withStyles
} from "@material-ui/core";
import InputAdornment from "@material-ui/core/InputAdornment";
import PersonIcon from "@material-ui/icons/Person";
import clsx from "clsx";
import EmailIcon from "@material-ui/icons/Email";
import { GenericWithStyles } from "@ptas/react-ui-library";
import {
  formatMessageStructure,
  renderFormatMessage
} from "../utils/formatMessage";
import { useUpdateEffect } from "react-use";
import { debounce } from "lodash";

interface Props extends WithStyles<typeof useStyles> {
  ptasVariant?: "underline" | "outline" | "overlay" | "currency" | "email";
  secondaryLabel?: string;
  showIcon?: boolean;
  error?: boolean;
  value?: string;
  label?: string | React.ReactNode;
  helperText?: string | React.ReactNode;
  placeholder?: string | formatMessageStructure;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onBlur?: () => void;
  type?: "text" | "email" | "password" | "number";
  variant?: "filled" | "standard" | "outlined" | undefined;
  startAdornment?: React.ReactNode;
  endAdornment?: React.ReactNode;
  readOnly?: boolean;
  name?: string;
  onChangeDelay?: number;
  /**
   * Dependencies for updating the onChange memoized callback (triggerOnChangeProps).
   * E.g. states used by the caller inside the onChange event.
   */
  onChangeDeps?: unknown[];
}

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: (props: Props) => ({
      "& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderColor: `${
          props.readOnly
            ? "transparent !important"
            : props.error
            ? theme.ptas.colors.utility.danger
            : theme.ptas.colors.theme.black
        }`
      },
      "& .MuiFormHelperText-root": {
        fontFamily: theme.ptas.typography.bodyFontFamily,
        fontSize: theme.ptas.typography.finePrint.fontSize,
        marginTop: 0,
        marginLeft: 8
      },
      backgroundColor: theme.ptas.colors.theme.white,
      "&:hover .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderColor: `${
          props.error
            ? theme.ptas.colors.utility.danger
            : theme.ptas.colors.theme.black
        }`
      },
      "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline": {
        borderColor: `${
          props.error
            ? theme.ptas.colors.utility.danger
            : theme.ptas.colors.utility.selection
        }`,
        border: "1px solid"
      },
      "& .MuiInput-underline:after": {
        borderBottomColor: `${
          props.error
            ? theme.ptas.colors.utility.danger
            : theme.ptas.colors.utility.selection
        }`
      },
      "&:after": {
        content: `"${props.secondaryLabel}"`,
        display: props.secondaryLabel ? "block" : "none",
        color: theme.ptas.colors.theme.grayMedium,
        fontWeight: "normal",
        fontFamily: theme.ptas.typography.bodyFontFamily,
        fontSize: theme.ptas.typography.finePrint.fontSize,
        position: "absolute",
        top: -8,
        right: 20,
        background: theme.ptas.colors.theme.white,
        paddingLeft: 3,
        paddingRight: 3,
        borderRadius: 4
      },
      borderRadius: 3,
      maxWidth: 135,
      width: "100%",
      height: 36,
      fontSize: theme.ptas.typography.body.fontSize
    }),
    underlineRoot: {
      "&:after": {
        top: "0 !important",
        background: "transparent !important"
      }
    },
    inputRoot: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      width: "100%"
    },
    labelRoot: {
      color: theme.ptas.colors.theme.grayMedium,
      fontWeight: "normal",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.finePrint.fontSize,
      marginTop: "0 !important"
    },
    animated: {
      top: -2,
      fontSize: 16,
      left: -4
    },
    animateUnderline: {
      top: -3,
      left: 0
    },
    animatedCurrency: {
      left: 16
    },
    animatedOverlay: {
      left: 10,
      top: -10
    },
    shrinkRoot: {
      fontSize: 16,
      top: 1,
      background: theme.ptas.colors.theme.white,
      borderRadius: 3,
      left: 0
    },
    standardShrink: {
      top: -8,
      left: 4
    },
    underlineShrink: {
      top: -5,
      left: 0,
      background: "transparent",
      padding: 0
    },
    helperText: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.finePrint.fontSize,
      lineHeight: "15px",
      marginLeft: 8,
      marginTop: 0
    },
    underlineInput: {
      paddingTop: 10,
      paddingBottom: 3,
      paddingLeft: 0
    },
    overlayInput: {
      paddingTop: 10,
      paddingBottom: 7,
      paddingLeft: 10,
      height: 36,
      boxSizing: "border-box"
    },
    underlineLabel: {
      background: "transparent !important"
    },
    outlineInput: {
      padding: "8px 10px"
    },
    outlineRoot: {
      "& .MuiOutlinedInput-adornedStart": {
        paddingLeft: 0
      }
    },
    overlayRoot: (props: Props) => ({
      "& .MuiInput-formControl": {
        marginTop: 0
      },
      "& .MuiInput-underline:before": {
        height: 10,
        borderBottom: "none !important"
      },
      "& .MuiInput-underline:after": {
        height: 10,
        borderBottom: "none !important",
        bottom: "0 !important"
      },
      "& .MuiInput-underline.Mui-focused:after": {
        borderBottom: `2px solid ${
          props.error
            ? theme.ptas.colors.utility.danger
            : theme.ptas.colors.utility.selection
        } !important`,
        bottom: 2
      },
      "& .MuiInput-underline.Mui-focused:before": {
        borderBottom: "none !important"
      }
    }),
    currencyInput: {
      paddingLeft: 0
    },
    iconSize: {
      fontSize: 17
    },
    fullWidth: {
      width: "100%",
      height: "100%"
    }
  });

const CustomTextField = forwardRef<HTMLDivElement | null, Props>(
  (props: Props, ref): JSX.Element => {
    const { classes, name, readOnly } = props;
    const [value, setValue] = useState<string>(props.value ?? "");

    useUpdateEffect(() => {
      setValue(props.value ?? "");
    }, [props.value]);

    const setTextFieldVariant = ():
      | "filled"
      | "standard"
      | "outlined"
      | undefined => {
      switch (props.ptasVariant) {
        case "underline":
          return "filled";
        case "overlay":
          return "standard";
        case "outline":
        case "currency":
        case "email":
          return "outlined";
        default:
          return "standard";
      }
    };

    const setVariants = (): {
      input: string;
      root: string;
      shrink?: string;
      animated?: string;
    } => {
      switch (props.ptasVariant) {
        case "underline":
          return {
            input: clsx(classes.inputRoot, classes.underlineInput),
            root: clsx(classes.root, classes.underlineRoot),
            animated: clsx(classes.animated, classes.animateUnderline),
            shrink: clsx(classes.shrinkRoot, classes.underlineShrink)
          };
        case "outline":
        case "email":
          return {
            input: clsx(classes.inputRoot, classes.outlineInput),
            root: clsx(classes.root, classes.outlineRoot)
          };
        case "currency":
          return {
            input: clsx(classes.inputRoot, classes.currencyInput),
            root: clsx(classes.root),
            animated: clsx(classes.animated, classes.animatedCurrency)
          };
        case "overlay":
          return {
            input: clsx(classes.inputRoot, classes.overlayInput),
            root: clsx(classes.root, classes.overlayRoot),
            shrink: clsx(classes.shrinkRoot, classes.standardShrink),
            animated: clsx(classes.animated, classes.animatedOverlay)
          };
        default:
          return {
            input: classes.inputRoot,
            root: classes.root
          };
      }
    };

    const triggerOnChangeProps = useCallback(
      debounce((e: React.ChangeEvent<HTMLInputElement>) => {
        props.onChange?.(e);
      }, props.onChangeDelay ?? 0),
      props.onChangeDeps ?? []
    );

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>): void => {
      e.persist();
      setValue(e.currentTarget.value);
      triggerOnChangeProps({ ...e });
    };

    const renderCurrencyIcon = (): JSX.Element =>
      props.ptasVariant === "currency" ? (
        <InputAdornment position='start'>$</InputAdornment>
      ) : (
        <Fragment />
      );

    const renderIcon = (): JSX.Element => {
      if (props.ptasVariant === "email") {
        return <EmailIcon className={classes.iconSize} />;
      }

      return props.showIcon ? (
        <PersonIcon className={classes.iconSize} />
      ) : (
        <Fragment />
      );
    };

    return (
      <TextField
        ref={ref}
        variant={props.variant ? props.variant : setTextFieldVariant()}
        onChange={handleChange}
        onBlur={props.onBlur}
        error={props.error}
        size='small'
        type={props.type ? props.type : "text"}
        value={value}
        label={props.label ?? "Default label"}
        placeholder={renderFormatMessage(props.placeholder)}
        className={setVariants().root}
        InputProps={{
          classes: {
            input: setVariants().input,
            root: classes.fullWidth
          },
          startAdornment: props.startAdornment ?? renderCurrencyIcon(),
          endAdornment: props.endAdornment ?? renderIcon(),
          readOnly,
          name
        }}
        helperText={props.helperText}
        InputLabelProps={{
          shrink: props.placeholder ? true : !!value,
          classes: {
            root: classes.labelRoot,
            animated: setVariants().animated ?? classes.animated,
            shrink: setVariants().shrink ?? classes.shrinkRoot
          }
        }}
        FormHelperTextProps={{
          classes: {
            root: classes.helperText
          }
        }}
      />
    );
  }
);

export default withStyles(useStyles)(CustomTextField) as FC<
  GenericWithStyles<Props>
>;
