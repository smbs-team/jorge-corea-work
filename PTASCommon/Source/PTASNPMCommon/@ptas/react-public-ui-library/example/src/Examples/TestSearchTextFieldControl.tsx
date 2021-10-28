import React, { useState } from "react";
import { ItemSuggestion } from "@ptas/react-public-ui-library";
import { makeStyles, TextField } from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";
import clsx from "clsx";

const useStyles = makeStyles((theme: Theme) => ({
  wrapper: {
    // maxWidth: 170,
    width: "45px",
    // backgroundColor: "red"
    /**
     * Fix for issue of width changing when label is shrinked on the top of the input
     */
    "& fieldset": {
      minInlineSize: "auto",
      "& legend": {
        maxWidth: "100%"
      }
    }
  },
  root: () => ({
    "& .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
      borderColor: `${theme.ptas.colors.theme.black}`
    },
    backgroundColor: theme.ptas.colors.theme.white,
    "&:hover .MuiOutlinedInput-root .MuiOutlinedInput-notchedOutline": {
      borderColor: `${theme.ptas.colors.theme.black}`
    },
    "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline": {
      borderColor: `${`${theme.ptas.colors.utility.selection} !important`}`,
      border: "1px solid"
    },
    "& .MuiInput-underline:after": {
      borderBottomColor: `${theme.ptas.colors.utility.selection}`
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
  /** INPUT **/
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
      borderColor: theme.ptas.colors.theme.black
      // maxWidth: "25px"
    },
    "& .PrivateNotchedOutline-legendNotched": {
      maxWidth: "50%",
      overflow: "hidden"
    }
  },

  inputRoot: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodySmall.fontWeight,
    fontFamily: theme.ptas.typography.bodyFontFamily
  },
  outlineInput: {
    padding: "8.5px 10px"
  },
  squaredOutlineInput: {
    padding: "8.5px 10px"
  },
  /** LABEL **/
  labelRoot: {
    color: theme.ptas.colors.theme.grayMedium,
    fontWeight: "normal",
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.body.fontSize,
    marginTop: "0 !important",
    // overflowX: "hidden",
    height: "19px",
    // width: "25px",
    maxWidth: "calc(100% - 20px)",
    overflow: "hidden",
    whiteSpace: "nowrap",
    textOverflow: "ellipsis"
  },
  shrinkRoot: {
    fontSize: 16,
    top: 1,
    background: theme.ptas.colors.theme.white,
    paddingLeft: 7,
    borderRadius: 3,
    paddingRight: 7,
    left: 0
    /****************/
    // left: -6
  }
}));

const CustomButtonComp = () => {
  const classes = useStyles();
  const [list, setList] = useState<ItemSuggestion[]>([]);

  const suggestionList: ItemSuggestion[] = [
    {
      title: "Test 1",
      subtitle: "Test 1"
    },
    {
      title: "Test 2",
      subtitle: "Test 2"
    }
  ];

  const handleOnChange = (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ) => {
    const inputValue = e.currentTarget.value;

    if (inputValue) {
      const newList = suggestionList.filter((item) =>
        item.title.toLowerCase().includes(inputValue.toLowerCase())
      );

      setList(newList);
    }
  };

  return (
    <div style={{ display: "flex", flexWrap: "wrap" }}>
      <div style={{ marginRight: 5, marginLeft: 5 }}>
        {/* Component */}
        <div className={classes.wrapper}>
          <TextField
            key={"testField"}
            variant={"outlined"}
            onChange={handleOnChange}
            error={false}
            autoFocus={true}
            size='small'
            // value={""}
            label={"Default label"}
            placeholder={"Default placeholder"}
            className={classes.squaredOutlineRoot}
            InputProps={{
              classes: {
                input: clsx(classes.inputRoot, classes.squaredOutlineInput)
              }
              // endAdornment: (
              //   <Fragment>
              //     {renderLoadingIcon()}
              //     {renderSearchIcon()}
              //   </Fragment>
              // ),
              // onBlur: () => {
              //   handleOnBlur();
              //   props.onBlur?.();
              // },
              // onFocus: onOpenSuggestion,
              // onClick: onClickInput,
              // readOnly: readOnly
            }}
            helperText={""}
            InputLabelProps={{
              // shrink: props.placeholder || value ? true : false,
              shrink: true,
              classes: {
                root: classes.labelRoot,
                // animated: setVariants().animated ?? classes.animated,
                shrink: classes.shrinkRoot
              }
            }}
          />
          {/* <div className={classes.suggestionWrapper}>{renderSuggestion()}</div> */}
        </div>
      </div>
    </div>
  );
};

export default CustomButtonComp;
