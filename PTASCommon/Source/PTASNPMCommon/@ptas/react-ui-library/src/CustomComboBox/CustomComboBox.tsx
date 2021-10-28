import React, { Fragment } from "react";
import {
  createStyles,
  Theme,
  withStyles,
  WithStyles
} from "@material-ui/core/styles";
import { TextField, Checkbox, Box } from "@material-ui/core";
import Autocomplete from "@material-ui/lab/Autocomplete";
import {
  Visibility as VisibilityIcon,
  VisibilityOff as VisibilityOffIcon,
  Search as SearchIcon
} from "@material-ui/icons";

interface Props extends WithStyles<typeof useStyles> {
  options: any[];
  optionLabel?: string;
  optionSelected?: string;
  onChange?: (event: React.SyntheticEvent<HTMLDivElement, Event>) => void;
  visibility?: boolean;
  defaultValue?: any[];
  rounded?: boolean;
  placeholder?: string;
  style?: React.CSSProperties;
  anchor?: any;
}

const useStyles = (theme: Theme) =>
  createStyles({
    option: {
      '&[aria-selected="true"]': {
        backgroundColor: "transparent"
      },
      padding: theme.spacing(5 / 8)
    },
    optionSelected: {
      color: theme.ptas.colors.utility.selection
    },
    optionMulti: {
      backgroundColor: "transparent"
    },
    visible: {
      color: theme.ptas.colors.theme.black
    },
    hidden: {
      color: theme.ptas.colors.theme.grayMedium
    },
    hideChip: { display: "none" },
    listbox: {
      maxHeight: "40vh",
      width: 346
    },
    paper: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      borderRadius: 0,
      marginTop: -3
    },
    rounded: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      borderRadius: 9
    },
    placeholder: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.body.fontSize
    },
    icon: {
      transform: "rotate(180deg)",
      color: theme.ptas.colors.theme.black
    },
    underline: {
      borderBottom: "2px solid " + theme.ptas.colors.theme.black
    },
    formControl: {
      margin: 0 + " " + theme.spacing(2) + "px",
      width: "calc(100% - " + theme.spacing(4) + "px)"
    },
    popupIndicator: {
      marginRight: theme.spacing(1)
    },
    popper: { width: "346 !important" }
  });

function CustomComboBox(props: Props): JSX.Element {
  const { classes } = props;
  const visibleIcon = <VisibilityIcon className={classes.visible} />;
  const hiddenIcon = <VisibilityOffIcon className={classes.hidden} />;
  /*const popper = (props: any) => {
    return (
      <Popper
        {...props}
        placement='bottom-start'
        //className={classes.popper}
        style={{ zIndex: 1100, width: 346 }}
      ></Popper>
    );
  };*/
  return (
    <Box>
      <Autocomplete
        open
        style={props.style}
        multiple
        disableCloseOnSelect
        options={props.options}
        popupIcon={<SearchIcon className={classes.icon} />}
        renderInput={(params) => (
          <TextField
            {...params}
            placeholder={props.placeholder}
            InputProps={{
              ...params.InputProps,
              classes: {
                root: classes.placeholder,
                underline: classes.underline,
                formControl: classes.formControl
              }
            }}
          />
        )}
        getOptionLabel={(option) => option[props.optionLabel ?? -1]}
        renderOption={
          props.visibility
            ? (option) => {
                return (
                  <Fragment>
                    <Checkbox
                      icon={hiddenIcon}
                      checkedIcon={visibleIcon}
                      checked={option[props.optionSelected ?? -1]}
                      size='small'
                    />
                    {option[props.optionLabel ?? -1]}
                  </Fragment>
                );
              }
            : (option) => {
                return (
                  <Fragment>
                    <span
                      className={
                        option[props.optionSelected ?? -1]
                          ? classes.optionSelected
                          : undefined
                      }
                      style={{
                        pointerEvents: "none"
                      }}
                    >
                      {option[props.optionLabel ?? -1]}
                    </span>
                  </Fragment>
                );
              }
        }
        onChange={props.onChange}
        classes={{
          option: props.visibility ? classes.option : classes.optionMulti,
          listbox: classes.listbox,
          paper: props.rounded ? classes.rounded : classes.paper,
          popper: props.anchor ? props.anchor().anchor : classes.popper,
          popupIndicator: classes.popupIndicator
        }}
        defaultValue={props.defaultValue}
        ChipProps={{
          classes: {
            root: classes.hideChip
          },
          variant: "outlined"
        }}
        //PopperComponent={popper}
      />
    </Box>
  );
}
export default withStyles(useStyles)(CustomComboBox);
