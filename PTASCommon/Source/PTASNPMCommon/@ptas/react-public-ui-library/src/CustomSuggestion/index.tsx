// CustomSuggestion.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles } from "@material-ui/core";
import React from "react";
import { Theme } from "@material-ui/core/styles";
import clsx from "clsx";

/**
 * Component props
 */

export interface ItemSuggestion {
  title: string;
  subtitle: string;
  mainTitle?: string;
  id?: number | string;
}

interface Props {
  items?: ItemSuggestion[];
  search?: string;
  show?: boolean;
  onSelected?: (itemSelected: ItemSuggestion) => void;
  onDisableBlur?: (disable: boolean) => void;
  loading?: boolean;
}

/**
 * Component styles
 */
const useStyles = makeStyles((theme: Theme) => ({
  root: {
    maxHeight: 211,
    minHeight: 25,
    overflow: "hidden",
    border: `1px solid ${theme.ptas.colors.theme.grayLight}`,
    boxShadow: "4px 4px 10px rgba(0, 0, 0, 0.25)",
    borderRadius: 9,
    paddingTop: 16,
    paddingRight: 6,
    paddingBottom: 4,
    paddingLeft: 16,
    position: "absolute",
    width: "100%",
    top: 2,
    display: "block",
    background: theme.ptas.colors.theme.white,
    boxSizing: "border-box",
    zIndex: 100,
    fontFamily: theme.ptas.typography.bodyFontFamily
  },
  title: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.finePrintBold.fontWeight,
    display: "block"
  },
  subtitle: {
    fontSize: 14,
    color: "rgba(0, 0, 0, 0.54)",
    display: "block"
  },
  item: {
    paddingBottom: 10,
    cursor: "pointer"
  },
  itemsWrap: {
    overflowY: "auto",
    maxHeight: 191,
    "&::-webkit-scrollbar": {
      width: 8
    },

    /* Track */
    "&::-webkit-scrollbar-track": {
      borderRadius: 10
    },

    /* Handle */
    "&::-webkit-scrollbar-thumb": {
      background: "#666666",
      borderRadius: 4
    }
  },
  border: {
    background:
      "linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)",
    width: "80%",
    height: 1,
    marginTop: 10,
    display: "block"
  },
  hide: {
    display: "none"
  }
}));

/**
 * CustomSuggestion
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomSuggestion(props: Props): JSX.Element {
  const { items, show, onDisableBlur, loading } = props;

  const classes = useStyles(props);

  const handleOnSelected = (itemSelected: ItemSuggestion) => () => {
    if (props.onSelected) {
      props.onSelected(itemSelected);
    }
  };

  const handleDisableBlur = (disable: boolean): void => {
    if (onDisableBlur) {
      onDisableBlur(disable);
    }
  };

  const handleOnMouseEnter = (): void => {
    handleDisableBlur(true);
  };

  const handleOnMouseLeave = (): void => {
    handleDisableBlur(false);
  };

  const renderSuggestionItems = (): JSX.Element[] | JSX.Element => {
    if (loading) {
      return <span>Loading...</span>;
    }

    if (!items?.length) return <span>Not found</span>;

    return items.map((item, index) => (
      <div
        className={classes.item}
        key={`${index}_`}
        onClick={handleOnSelected(item)}
      >
        {item.mainTitle && (
          <span className={classes.title}>{item.mainTitle}</span>
        )}
        <span className={classes.title}>{item.title}</span>
        <span className={classes.subtitle}>{item.subtitle}</span>
        <span className={classes.border}></span>
      </div>
    ));
  };

  return (
    <div
      className={clsx(classes.root, !show && classes.hide)}
      onMouseEnter={handleOnMouseEnter}
      onMouseLeave={handleOnMouseLeave}
    >
      <div className={classes.itemsWrap}>{renderSuggestionItems()}</div>
    </div>
  );
}

export default CustomSuggestion;
