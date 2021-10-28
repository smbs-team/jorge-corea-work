// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, Key, Fragment, useEffect } from "react";
import {
  withStyles,
  WithStyles,
  createStyles,
  TextField,
  Theme,
  TextFieldProps
} from "@material-ui/core";
import SearchIcon from "@material-ui/icons/Search";
import ListItem from "../ListSearch/ListItem";
import Scrollbar from "react-scrollbars-custom";
import { GenericWithStyles } from "../common";
import { filter } from "lodash";

/**
 * Component props
 */
interface Props<T> extends WithStyles<typeof useStyles> {
  data: GenericListObject<T>[];
  selectedData?: (number | string)[];
  textFieldLabel?: string;
  isSelected?: boolean;
  onClick?: (selectedItem: GenericListObject<T>) => void;
  TextFieldProps?: TextFieldProps;
  lockedArea?: number;
}

interface ListItem {
  key: string;
  value: React.ReactText;
}

export type GenericListObject<T> = T & ListItem;
export type GenericListSearch<T> = GenericWithStyles<Props<T>>;

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      width: "400px !important",
      height: "350px !important",
      padding: 16,
      paddingTop: 0,
      overflow: "auto"
    },
    textField: {
      width: "100%",
      position: "sticky",
      top: 0,
      left: theme.spacing(2),
      paddingRight: theme.spacing(4),
      marginBottom: theme.spacing(1.375),
      backgroundColor: "white"
    },
    areaList: {
      padding: theme.spacing(0, 2),
      listStyleType: "none",
      margin: 0
    },
    trackY: {
      background: "unset !important"
    }
  });

/**
 * GenericListSearch
 *
 * @param props - Component props
 * @returns A JSX element
 */
function GenericListSearch<T>(props: Props<T>): JSX.Element {
  const { classes, textFieldLabel, TextFieldProps } = props;
  const [searchResults, setSearchResults] = useState<GenericListObject<T>[]>(
    props.data
  );
  const [name, setName] = useState<string>("");

  useEffect(() => {
    setSearchResults(props.data);
  }, [props.data]);

  const filterList = (text: string) => {
    setName(text);
    if (text !== "") {
      setSearchResults(
        filter(props.data, (d) =>
          (d.value as string).toLowerCase().includes(text.toLowerCase())
        )
      );
    } else {
      setSearchResults(props.data);
    }
  };

  return (
    <Fragment>
      <TextField
        onChange={(e): void => filterList(e.target.value)}
        value={name}
        className={classes.textField}
        InputProps={{
          endAdornment: <SearchIcon />
        }}
        label={textFieldLabel ?? "Placeholder text"}
        {...TextFieldProps}
      />
      <Scrollbar
        className={classes.root}
        trackYProps={{
          renderer: (props) => {
            const { elementRef, ...restProps } = props;
            return (
              <span
                {...restProps}
                ref={elementRef}
                className={classes.trackY}
              />
            );
          }
        }}
      >
        {
          <ul className={classes.areaList}>
            {searchResults && searchResults.length > 0 ? (
              searchResults.map((item, index) => (
                <ListItem
                  key={index}
                  isSelected={
                    props.selectedData &&
                    props.selectedData.some((s: Key) => s === item.key)
                  }
                  onClick={(): void => {
                    props.onClick?.(item);
                    setName("");
                  }}
                  isDeselectable={
                    props.lockedArea === (item.value as number) ? false : true
                  }
                >
                  {item.value ?? item.key}
                </ListItem>
              ))
            ) : (
              <h4>No results found</h4>
            )}
          </ul>
        }
      </Scrollbar>
    </Fragment>
  );
}

export default withStyles(useStyles, { withTheme: true })(
  GenericListSearch
) as <T>(props: GenericListSearch<T>) => React.ReactElement;
