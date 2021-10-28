import React, { FC, Fragment, useEffect, useState, useCallback } from "react";
import { Theme } from "@material-ui/core/styles";
import {
  createStyles,
  WithStyles,
  TextField,
  withStyles
} from "@material-ui/core";
import clsx from "clsx";
import SearchIcon from "@material-ui/icons/Search";
import CustomSuggestion, { ItemSuggestion } from "../CustomSuggestion";
import CircularProgress from "@material-ui/core/CircularProgress";
import { GenericWithStyles } from "@ptas/react-ui-library";
import {
  formatMessageStructure,
  renderFormatMessage
} from "../utils/formatMessage";
import { useMount, useUpdateEffect } from "react-use";
import { debounce } from "lodash";

export interface Suggestion {
  List: ItemSuggestion[];
  onSelected?: (itemSelected: ItemSuggestion) => void;
  loading?: boolean;
  value?: string | number;
}

interface Props extends WithStyles<typeof useStyles> {
  ptasVariant?:
    | "underline"
    | "underline tall"
    | "outline"
    | "outline tall"
    | "inverse"
    | "squared outline";
  onClick?: () => void;
  suggestion?: Suggestion;
  error?: boolean;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onBlur?: () => void;
  value?: string;
  initialValue?: string;
  label?: string | React.ReactNode;
  helperText?: string;
  placeholder?: string | formatMessageStructure;
  autoFocus?: boolean;
  onClickInput?: () => void;
  readOnly?: boolean;
  hideSearchIcon?: boolean;
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
            ? `${theme.ptas.colors.utility.danger} !important`
            : `${theme.ptas.colors.utility.selection} !important`
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
      width: "100%",
      paddingRight: 0,
      borderRadius: 20,

      "& .MuiFilledInput-adornedEnd": {
        paddingRight: 0
      },
      "& .MuiOutlinedInput-adornedEnd": {
        paddingRight: 0
      }
    }),
    wrapper: {
      maxWidth: 170
    },
    underlineRoot: {
      "&:after": {
        top: "0 !important",
        background: "transparent !important"
      }
    },
    underlineTallRoot: {
      "&:after": {
        top: "0 !important",
        background: "transparent !important"
      },
      height: 40
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
      marginTop: "0 !important",
      lineHeight: "19px",
      /**
       * Fix for issue of label/placeholder text extending beyond the width of the input
       */
      overflow: "hidden",
      whiteSpace: "nowrap",
      textOverflow: "ellipsis",
      maxWidth: "calc(100% - 14px)"
    },
    animated: {
      top: -2,
      fontSize: "16px !important",
      left: -3
    },
    animateUnderline: {
      top: -7,
      left: 0
    },
    animateUnderlineTall: {
      top: 0,
      left: 0
    },
    animateOutlineTall: {
      top: 4,
      left: -4
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
    shrinkHidden: {
      display: "none"
    },
    underlineTallShrink: {
      display: "block",
      background: "transparent",
      top: -4,
      left: -5
    },
    helperText: {
      fontFamily: theme.ptas.typography.bodyFontFamily
    },
    underlineInput: {
      paddingTop: 8,
      paddingBottom: 6,
      paddingLeft: 13
    },
    inputTall: {
      height: 31
    },
    outlineInput: {
      padding: "8.5px 10px"
    },
    squaredOutlineInput: {
      padding: "8.5px 10px"
    },
    inverseInput: {
      padding: "9px 10px",
      color: theme.ptas.colors.theme.white
    },
    outlineRoot: {
      "& .MuiOutlinedInput-adornedStart": {
        paddingLeft: 0
      },
      "& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderRadius: 25,
        borderColor: theme.ptas.colors.theme.white
      }
    },
    squaredOutlineRoot: {
      borderRadius: "3px !important",
      "& .MuiOutlinedInput-adornedStart": {
        paddingLeft: 0
      },
      "& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderColor: theme.ptas.colors.theme.white
      },
      /**
       * Fix for issue of width changing when label is shrinked on the top of the input
       */
      "& fieldset": {
        minInlineSize: "auto"
      }
    },
    inverseRoot: {
      "& .MuiOutlinedInput-adornedStart": {
        paddingLeft: 0
      },
      "& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderRadius: 25,
        borderColor: `${theme.ptas.colors.theme.white} !important`
      },
      "&:hover .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
        borderColor: `${theme.ptas.colors.theme.white} !important`
      },
      "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline": {
        borderColor: `${theme.ptas.colors.utility.selection} !important`
      },
      "& label.Mui-focused": {
        color: theme.ptas.colors.theme.white
      },
      background: "transparent !important",
      borderRadius: 25
    },
    noLegend: {
      '& [class^="PrivateNotchedOutline-legendLabelled"]': {
        maxWidth: 0
      }
    },
    iconStyle: (props: Props) => ({
      paddingTop: 6,
      color: `${
        props.ptasVariant !== "inverse"
          ? theme.ptas.colors.theme.black
          : theme.ptas.colors.theme.white
      }`
    }),
    buttonSearch: {
      background: "transparent",
      outline: "none",
      border: "none",

      "&:hover": {
        cursor: "pointer"
      }
    },
    animatedInverse: {
      color: theme.ptas.colors.theme.white
    },
    suggestionWrapper: {
      position: "relative"
    },
    loadingIcon: (props: Props) => ({
      color:
        props.ptasVariant === "outline tall"
          ? theme.ptas.colors.theme.white
          : theme.ptas.colors.theme.black,
      width: "25px !important",
      height: "25px !important"
    })
  });

function CustomSearchTextField(props: Props): JSX.Element {
  const {
    suggestion,
    ptasVariant,
    onChange,
    onClick,
    classes,
    autoFocus,
    readOnly,
    onClickInput
  } = props;

  const [value, setValue] = useState<string>(props.value ?? "");
  const [showSuggestion, setShowSuggestion] = useState<boolean>(false);
  const [suggestionList, setSuggestionList] = useState<
    ItemSuggestion[] | undefined
  >([]);

  const [onBlurDisable, setOnBlurDisable] = useState<
    boolean | null | undefined
  >(null);

  useMount(() => {
    if (!props.value && props.initialValue) {
      setValue(props.initialValue);
    }
  });

  useEffect(() => {
    setSuggestionList(suggestion?.List);
  }, [suggestion?.List]);

  useUpdateEffect(() => {
    setValue(props.value ?? "");
  }, [props.value]);

  const setTextFieldVariant = ():
    | "filled"
    | "standard"
    | "outlined"
    | undefined => {
    switch (ptasVariant) {
      case "underline":
      case "underline tall":
        return "filled";
      case "outline":
      case "outline tall":
      case "squared outline":
      case "inverse":
        return "outlined";
      default:
        return "filled";
    }
  };

  const setVariants = (): {
    input: string;
    root: string;
    shrink?: string;
    animated?: string;
  } => {
    switch (ptasVariant) {
      case "underline":
        return {
          input: clsx(classes.inputRoot, classes.underlineInput),
          root: clsx(classes.root, classes.underlineRoot),
          animated: clsx(classes.animated, classes.animateUnderline),
          shrink: clsx(classes.shrinkHidden)
        };
      case "underline tall":
        return {
          input: clsx(
            classes.inputRoot,
            classes.underlineInput,
            classes.inputTall
          ),
          root: clsx(classes.root, classes.underlineTallRoot),
          animated: clsx(classes.animated, classes.animateUnderlineTall),
          shrink: clsx(classes.shrinkRoot, classes.underlineTallShrink)
        };
      case "outline":
        return {
          input: clsx(classes.inputRoot, classes.outlineInput),
          root: clsx(classes.root, classes.outlineRoot, classes.noLegend),
          shrink: classes.shrinkHidden
        };
      case "outline tall":
        return {
          input: clsx(
            classes.inputRoot,
            classes.outlineInput,
            classes.inputTall
          ),
          root: clsx(classes.root, classes.outlineRoot),
          animated: clsx(classes.animated, classes.animateOutlineTall)
        };
      case "squared outline":
        return {
          input: clsx(classes.inputRoot, classes.squaredOutlineInput),
          root: clsx(classes.root, classes.squaredOutlineRoot),
          shrink: classes.shrinkRoot
        };
      case "inverse":
        return {
          input: clsx(classes.inputRoot, classes.inverseInput),
          root: clsx(classes.root, classes.inverseRoot, classes.noLegend),
          shrink: classes.shrinkHidden,
          animated: clsx(classes.animated, classes.animatedInverse)
        };
      default:
        return {
          input: classes.inputRoot,
          root: classes.root
        };
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>): void => {
    e.persist();
    const inputValue = e.currentTarget.value;
    setValue(inputValue);

    triggerOnChangeProps({ ...e });
  };

  const triggerOnChangeProps = useCallback(
    debounce((e: React.ChangeEvent<HTMLInputElement>) => {
      if (e.target?.value) {
        setShowSuggestion(true);
      }

      if (!e.target?.value) {
        setShowSuggestion(false);
      }
      onChange?.(e);
    }, props.onChangeDelay ?? 0),
    props.onChangeDeps ?? []
  );

  const handleClick = (): void => {
    if (onClick) {
      onClick();
    }
  };

  const onCloseSuggestion = () => {
    setShowSuggestion(false);
  };

  const handleOnBlur = () => {
    if (!onBlurDisable) {
      onCloseSuggestion();
    }
  };

  const onOpenSuggestion = () => {
    if (suggestion?.List && suggestion.List.length) {
      setShowSuggestion(!!value);
    }
  };

  const renderSuggestion = (): JSX.Element => {
    if (!suggestion) return <Fragment />;

    return (
      <CustomSuggestion
        items={suggestionList}
        show={showSuggestion}
        onDisableBlur={(disable: boolean): void => {
          setOnBlurDisable(disable);
        }}
        onSelected={(item) => {
          onCloseSuggestion();
          if (suggestion && suggestion.onSelected) {
            suggestion.onSelected(item);
          }
          if (item) setValue(item.title);
        }}
        loading={suggestion.loading}
      />
    );
  };

  const renderLoadingIcon = (): JSX.Element => {
    if (!suggestion?.loading) return <Fragment />;

    return (
      <div>
        <CircularProgress className={classes.loadingIcon} />
      </div>
    );
  };

  const renderSearchIcon = (): JSX.Element => {
    if (!props.hideSearchIcon) {
      return (
        <button onClick={handleClick} className={classes.buttonSearch}>
          <SearchIcon className={classes.iconStyle} />
        </button>
      );
    }
    return <Fragment></Fragment>;
  };

  return (
    <div
      onBlur={(e): void => {
        //Only run onBlur if this div does not contain the clicked element.
        //This is necessary, for instance, so that the onBlur event does
        //not run when the user has typed something in the input and then
        //selects an option from the search results.
        if (!e.currentTarget.contains(e.relatedTarget as Node)) {
          props.onBlur?.();
        }
      }}
      className={classes.wrapper}
    >
      <TextField
        variant={setTextFieldVariant()}
        onChange={handleChange}
        error={props.error}
        autoFocus={autoFocus}
        size='small'
        value={value}
        label={props.label ?? "Default label"}
        placeholder={renderFormatMessage(props.placeholder)}
        className={setVariants().root}
        InputProps={{
          classes: {
            input: setVariants().input
          },
          endAdornment: (
            <Fragment>
              {renderLoadingIcon()}
              {renderSearchIcon()}
            </Fragment>
          ),
          onBlur: () => {
            handleOnBlur();
          },
          onFocus: onOpenSuggestion,
          onClick: onClickInput,
          readOnly: readOnly
        }}
        helperText={props.helperText}
        InputLabelProps={{
          shrink: props.placeholder || value ? true : false,
          classes: {
            root: classes.labelRoot,
            animated: setVariants().animated ?? classes.animated,
            shrink: setVariants().shrink ?? classes.shrinkRoot
          }
        }}
      />
      <div tabIndex={0} className={classes.suggestionWrapper}>
        {renderSuggestion()}
      </div>
    </div>
  );
}

export default withStyles(useStyles)(CustomSearchTextField) as FC<
  GenericWithStyles<Props>
>;
