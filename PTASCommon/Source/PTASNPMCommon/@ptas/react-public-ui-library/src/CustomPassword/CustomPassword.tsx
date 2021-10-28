import React, { PropsWithChildren, useState } from "react";
import { Theme } from "@material-ui/core/styles";
import {
  InputAdornment,
  makeStyles,
  TextField,
  TextFieldProps
} from "@material-ui/core";
import { omit } from "lodash";
import Visibility from "@material-ui/icons/Visibility";
import VisibilityOff from "@material-ui/icons/VisibilityOff";
import IconButton from "@material-ui/core/IconButton";
import clsx from "clsx";

type Props = PropsWithChildren<TextFieldProps> & {};

/**
 * Component styles
 */
const useStyles = makeStyles((theme: Theme) => ({
  root: {
    "& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
      borderColor: theme.ptas.colors.theme.black
    },
    backgroundColor: "transparent",
    "&:hover .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
      borderColor: theme.ptas.colors.theme.black
    },
    "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline": {
      borderColor: theme.ptas.colors.utility.selection
    },
    borderRadius: 3,
    maxWidth: 230,
    width: "100%",

    "& .MuiFormHelperText-contained": {
      marginLeft: "auto",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight
    }
  },
  wrapper: {
    position: "relative"
  },
  inputRoot: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodySmall.fontWeight,
    fontFamily: theme.ptas.typography.bodyFontFamily
  },
  labelRoot: {
    color: theme.ptas.colors.theme.grayMedium,
    fontWeight: "normal",
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.body.fontSize,
    marginTop: "0 !important"
  },
  animated: {
    top: "0",
    fontSize: 16,
    left: 0
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
  outlineInput: {
    padding: 10
  },
  iconSize: {
    fontSize: 17
  },
  statusPasswordRoot: {
    height: 3,
    borderRadius: 2,
    position: "absolute",
    left: 0,
    bottom: 8
  },
  failPassword: {
    background: theme.ptas.colors.utility.danger,
    width: 40
  },
  weakPassword: {
    background: theme.ptas.colors.utility.warning,
    width: 87
  },
  strongPassword: {
    background: theme.ptas.colors.utility.success,
    width: 150
  },
  warning: {
    color: theme.ptas.colors.utility.warning
  },
  success: {
    color: theme.ptas.colors.utility.success
  },
  danger: {
    color: theme.ptas.colors.utility.danger
  }
}));

function CustomPassword(props: Props): JSX.Element {
  const classes = useStyles(props);
  const [value, setValue] = useState<string>("");
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [passwordMatch, setPasswordMatch] = useState<
    "weak" | "fail" | "strong" | null
  >(null);
  const newProps = omit(props, ["onChange", "helperText"]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>): void => {
    const inputValue = e.currentTarget.value;
    setValue(inputValue);

    setPasswordState(inputValue);

    if (typeof props.onChange === "function") {
      props.onChange(e as React.ChangeEvent<HTMLInputElement>);
    }
  };

  const setPasswordState = (password: string): void => {
    if (!password) return setPasswordMatch(null);

    const lettersAndNumbersWithLength = /^(?=\w*\d)\S{8,16}/g;
    const uppercase = /(?=.*[A-Z])/g;
    const specialCharacter = /(?=.*[!@#$&*])/g;

    const contentLettersAndNumbers = lettersAndNumbersWithLength.test(password);
    const contentUppercase = uppercase.test(password);
    const contentSpecialCharacter = specialCharacter.test(password);

    if (contentUppercase && contentLettersAndNumbers && contentSpecialCharacter)
      return setPasswordMatch("strong");

    if (contentLettersAndNumbers && contentUppercase)
      return setPasswordMatch("weak");

    setPasswordMatch("fail");
  };

  const handleClickShowPassword = (): void =>
    setShowPassword((prevState) => !prevState);

  const renderIcon = (): JSX.Element => {
    return (
      <InputAdornment position='end'>
        <IconButton
          aria-label='toggle password visibility'
          onClick={handleClickShowPassword}
        >
          {showPassword ? <Visibility /> : <VisibilityOff />}
        </IconButton>
      </InputAdornment>
    );
  };

  const setClassByPasswordMatch = (): string => {
    switch (passwordMatch) {
      case "strong":
        return classes.strongPassword;
      case "weak":
        return classes.weakPassword;
      case "fail":
        return classes.failPassword;
      default:
        return "";
    }
  };

  const setHelperTextColor = (): string => {
    switch (passwordMatch) {
      case "strong":
        return classes.success;
      case "weak":
        return classes.warning;
      case "fail":
        return classes.danger;
      default:
        return "";
    }
  };

  return (
    <div className={classes.wrapper}>
      <TextField
        variant='outlined'
        onChange={handleChange}
        type={showPassword ? "text" : "password"}
        size='small'
        value={value}
        label={props.label ?? "Default label"}
        placeholder={props.placeholder}
        className={classes.root}
        InputProps={{
          classes: {
            input: classes.inputRoot
          },
          endAdornment: renderIcon()
        }}
        helperText={`${passwordMatch ?? ""}`}
        InputLabelProps={{
          shrink: props.placeholder || value ? true : false,
          classes: {
            root: classes.labelRoot,
            animated: classes.animated,
            shrink: classes.shrinkRoot
          }
        }}
        FormHelperTextProps={{
          className: setHelperTextColor()
        }}
        {...newProps}
      />
      {passwordMatch && (
        <span
          className={clsx(
            setClassByPasswordMatch(),
            classes.statusPasswordRoot
          )}
        ></span>
      )}
    </div>
  );
}
export default CustomPassword;
