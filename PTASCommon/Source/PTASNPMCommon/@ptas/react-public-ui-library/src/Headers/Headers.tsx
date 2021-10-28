// Headers.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, Fragment } from "react";
import { createStyles, WithStyles, withStyles } from "@material-ui/core";
import { Theme } from "@material-ui/core/styles";
import { ReactComponent as KingCountyLogo } from "../assets/image/king-county-logo.svg";
import { ReactComponent as KingCountyLogoWhite } from "../assets/image/king-county-logo-white.svg";
import SearchIcon from "@material-ui/icons/Search";
import MenuIcon from "@material-ui/icons/Menu";
import { SimpleDropDown, DropDownItem } from "../SimpleDropDown";
import clsx from "clsx";
import { GenericWithStyles } from "@ptas/react-ui-library";

interface Props extends WithStyles<typeof useStyles> {
  ptasVariant?: "transparent" | "black";
  items?: DropDownItem[];
  showSearchIcon?: boolean;
  showDropdown?: boolean;
  showMenuIcon?: boolean;
  logo?: string;
  dropDownPlaceHolder?: string;
  defaultValue?: string;
  value?: string;
  onSelected?: (item: DropDownItem) => void;
}

const useStyles = (theme: Theme) =>
  createStyles({
    root: (props: Props) => ({
      display: "flex",
      flexDirection: "column",
      margin: 0,
      background:
        props.ptasVariant === "transparent" || props.ptasVariant === undefined
          ? "gba(255, 255, 255, 0.7);"
          : theme.ptas.colors.theme.black,
      boxShadow:
        props.ptasVariant === "transparent" || props.ptasVariant === undefined
          ? "20px 20px 50px rgba(0, 0, 0, 0.1)"
          : "none",
      [theme.breakpoints.up("medium")]: {
        flexDirection: "row"
      },
      backdropFilter: "blur(16px)"
    }),
    wrapper: {
      display: "flex",
      flexDirection: "row",
      justifyContent: "space-between",
      width: "100%",
      boxSizing: "border-box",
      paddingLeft: 8,

      [theme.breakpoints.up("sm")]: {
        paddingLeft: 40
      }
    },
    logoWrap: {
      paddingTop: 15
    },
    items: {
      display: "flex",
      alignItems: "center"
    },
    dropDown: (props: Props) => ({
      flexShrink: 0,
      marginRight: 23,
      display: "none",
      "& .MuiOutlinedInput-root": {
        "& fieldset": {
          borderColor: "transparent !important",
          BackgroundColor: "transparent !important"
        },
        "&.Mui-focused fieldset": {
          borderColor: "transparent !important",
          BackgroundColor: "transparent !important"
        }
      },
      color:
        props.ptasVariant === "transparent" || props.ptasVariant === undefined
          ? theme.ptas.colors.theme.black
          : theme.ptas.colors.theme.white,
      "& .MuiSelect-select": {
        "&:focus": {
          backgroundColor: "transparent !important"
        },
        paddingRight: "22px !important"
      },
      "&.MuiSelect-select": {
        paddingRight: 22
      },
      [theme.breakpoints.up("medium")]: {
        display: "block"
      }
    }),
    dropDownMobile: {
      display: "flex !important",
      alignItems: "flex-end",
      marginRight: "58px !important",
      marginLeft: "auto",
      [theme.breakpoints.up("medium")]: {
        display: "none !important"
      }
    },
    dropDownBgColor: {
      background: "rgba(255, 255, 255, 0.4)"
    },
    dropDownWidth: {
      minWidth: "50px !important"
    },
    color: (props: Props) => ({
      color:
        props.ptasVariant === "transparent" || props.ptasVariant === undefined
          ? theme.ptas.colors.theme.black
          : theme.ptas.colors.theme.white
    }),
    searchIcon: (props: Props) => ({
      marginRight: 27,
      paddingTop: 5,
      color:
        props.ptasVariant === "transparent" || props.ptasVariant === undefined
          ? theme.ptas.colors.theme.black
          : theme.ptas.colors.theme.white
    }),
    menuIcon: {
      height: 64,
      width: 64,
      display: "flex",
      justifyContent: "center",
      alignItems: "center",
      background: theme.ptas.colors.theme.accentDark,
      color: theme.ptas.colors.theme.white
    }
  });

function Header(props: Props): JSX.Element {
  const { classes } = props;

  const defaultItems: DropDownItem[] = [
    {
      label: "English",
      value: "en"
    },
    {
      label: "Spanish",
      value: "es"
    }
  ];

  const renderSearchIcon = (): JSX.Element =>
    props.showSearchIcon ? (
      <span className={classes.searchIcon}>
        <SearchIcon />
      </span>
    ) : (
      <Fragment />
    );

  const renderMenuIcon = (): JSX.Element =>
    props.showMenuIcon ? (
      <span className={classes.menuIcon}>
        <MenuIcon />
      </span>
    ) : (
      <Fragment />
    );

  const renderDropDown = (): JSX.Element =>
    props.showDropdown ? (
      <SimpleDropDown
        classes={{
          root: classes.dropDown,
          textFieldRoot: classes.dropDownWidth,
          inputRoot: classes.color,
          arrow: classes.color
        }}
        defaultValue={props.defaultValue}
        items={props.items ?? defaultItems}
        onSelected={props.onSelected}
        placeholder={props.dropDownPlaceHolder}
      />
    ) : (
      <Fragment />
    );

  const renderDropDownMobile = (): JSX.Element =>
    props.showDropdown ? (
      <SimpleDropDown
        classes={{
          root: clsx(classes.dropDown, classes.dropDownMobile),
          textFieldRoot: classes.dropDownWidth,
          inputRoot: classes.color,
          arrow: classes.color
        }}
        defaultValue={props.defaultValue}
        items={props.items ?? defaultItems}
        onSelected={props?.onSelected}
        placeholder={props.dropDownPlaceHolder}
        value={props.value}
      />
    ) : (
      <Fragment />
    );

  const renderLogo = (): JSX.Element => {
    if (props.logo) <img src={props.logo} alt='King county logo' />;
    return props.ptasVariant === "transparent" ||
      props.ptasVariant === undefined ? (
      <KingCountyLogo />
    ) : (
      <KingCountyLogoWhite />
    );
  };

  return (
    <header className={classes.root}>
      <div className={classes.wrapper}>
        <span className={classes.logoWrap}>{renderLogo()}</span>
        <div className={classes.items}>
          {renderDropDown()}
          {renderSearchIcon()}
          {renderMenuIcon()}
        </div>
      </div>
      <div className={classes.dropDownBgColor}>{renderDropDownMobile()}</div>
    </header>
  );
}

export default withStyles(useStyles)(Header) as FC<GenericWithStyles<Props>>;
