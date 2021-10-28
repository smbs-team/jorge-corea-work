import React, { FC, useState, useCallback } from "react";
import { Theme } from "@material-ui/core/styles";
import {
  createStyles,
  TextField,
  withStyles,
  WithStyles
} from "@material-ui/core";
import MaskedInput from "react-text-mask";
import PhoneAndroidIcon from "@material-ui/icons/PhoneAndroid";
import { GenericWithStyles } from "@ptas/react-ui-library";
import {
  formatMessageStructure,
  renderFormatMessage
} from "../utils/formatMessage";
import { useUpdateEffect } from "react-use";
import clsx from "clsx";
import { debounce } from "lodash";

interface Props extends WithStyles<typeof useStyles> {
  secondaryLabel?: string;
  error?: boolean;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onBlur?: () => void;
  helperText?: string | React.ReactNode;
  label?: string | React.ReactNode;
  placeholder?: string | formatMessageStructure;
  value?: string;
  variant?: "filled" | "standard" | "outlined" | undefined;
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
          props.error
            ? theme.ptas.colors.utility.danger
            : theme.ptas.colors.theme.black
        }`
      },
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
      "& .MuiOutlinedInput-adornedStart": {
        paddingLeft: 0
      },
      "&:after": {
        content: `"${props.secondaryLabel}"`,
        display: props.secondaryLabel ? "block" : "none",
        color: theme.ptas.colors.theme.grayMedium,
        fontWeight: "normal",
        fontFamily: theme.ptas.typography.bodyFontFamily,
        fontSize: theme.ptas.typography.bodySmall.fontSize,
        position: "absolute",
        top: -8,
        right: 20,
        background: theme.ptas.colors.theme.white,
        paddingLeft: 3,
        paddingRight: 3,
        borderRadius: 4
      },
      maxWidth: 170,
      width: "100%"
    }),
    standardRoot: {
      "& .MuiInputBase-root": {
        marginTop: 0,
        backgroundColor: theme.ptas.colors.theme.white
      },
      "& .MuiInput-underline:before": {
        borderBottom: "none"
      },
      "& .MuiSvgIcon-root": {
        marginRight: 8
      }
    },
    standardAnimated: {
      top: -10,
      left: 10
    },
    standardShrink: {
      top: "-8px !important",
      left: "4px !important",
      zIndex: 10
    },
    inputRoot: {
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      padding: 10
    },
    labelRoot: {
      color: theme.ptas.colors.theme.grayMedium,
      fontWeight: "normal",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.body.fontSize,
      marginTop: "0 !important",
      "&:focus": {
        backgroundColor: theme.ptas.colors.theme.white,
        borderRadius: 3,
        padding: 5
      }
    },
    animated: {
      top: -1,
      left: -3
    },
    shrinkRoot: {
      fontSize: 16,
      top: 1,
      background: theme.ptas.colors.theme.white,
      paddingLeft: 7,
      borderRadius: 3,
      paddingRight: 7,
      left: 0
    },
    helperText: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      paddingLeft: 8
    },
    phoneIcon: {
      fontSize: 17
    }
  });

interface TextMaskCustomProps {
  inputRef: (ref: HTMLInputElement | null) => void;
}

function TextMaskCustom(props: TextMaskCustomProps) {
  const { inputRef, ...other } = props;

  return (
    <MaskedInput
      {...other}
      ref={(ref: any) => {
        inputRef(ref ? ref.inputElement : null);
      }}
      mask={[
        /[1-9]/,
        /\d/,
        /\d/,
        "-",
        /\d/,
        /\d/,
        /\d/,
        "-",
        /\d/,
        /\d/,
        /\d/,
        /\d/
      ]}
      placeholderChar={"\u2000"}
    />
  );
}

function CustomPhoneTextField(props: Props): JSX.Element {
  const {
    classes,
    name,
    variant,
    error,
    label,
    onChange,
    helperText,
    placeholder,
    value: valueProps,
    onChangeDelay
  } = props;

  const [value, setValue] = useState<string>(props.value ?? "");

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>): void => {
    e.persist();

    setValue(e.target.value);
    triggerOnChangeProps({ ...e });
  };

  const triggerOnChangeProps = useCallback(
    debounce((e: React.ChangeEvent<HTMLInputElement>) => {
      onChange?.(e);
    }, onChangeDelay ?? 0),
    props.onChangeDeps ?? []
  );

  useUpdateEffect(() => {
    setValue(props.value ?? "");
  }, [valueProps]);

  const setClassByVariant = (): {
    root: string;
    animated: string;
    shrink: string;
  } => {
    switch (variant) {
      case "outlined":
        return {
          root: classes.root,
          animated: classes.animated,
          shrink: classes.shrinkRoot
        };
      case "standard":
        return {
          root: clsx(classes.root, classes.standardRoot),
          animated: classes.standardAnimated,
          shrink: clsx(classes.shrinkRoot, classes.standardShrink)
        };
      default:
        return {
          root: classes.root,
          animated: classes.animated,
          shrink: classes.shrinkRoot
        };
    }
  };

  return (
    <TextField
      variant={variant ?? "outlined"}
      onChange={handleChange}
      onBlur={props.onBlur}
      error={error}
      size='small'
      value={value}
      label={label ?? "Default label"}
      placeholder={renderFormatMessage(placeholder)}
      className={setClassByVariant().root}
      InputProps={{
        classes: {
          input: classes.inputRoot
        },
        endAdornment: <PhoneAndroidIcon className={classes.phoneIcon} />,
        inputComponent: TextMaskCustom as any,
        name
      }}
      FormHelperTextProps={{
        className: classes.helperText
      }}
      helperText={helperText}
      InputLabelProps={{
        shrink: placeholder || value ? true : false,
        classes: {
          root: classes.labelRoot,
          animated: setClassByVariant().animated,
          shrink: setClassByVariant().shrink
        }
      }}
    />
  );
}

export default withStyles(useStyles)(CustomPhoneTextField) as FC<
  GenericWithStyles<Props>
>;
