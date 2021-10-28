// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, FC, useEffect } from "react";
import { useMount } from "react-use";
import {
  withStyles,
  WithStyles,
  createStyles,
  Tabs,
  Tab,
  Theme,
  Box,
  FormLabel
} from "@material-ui/core";
import { GenericWithStyles } from "../common";

/**
 * Component props
 */
interface Props extends WithStyles<typeof useStyles> {
  items: string[];
  onSelected: (selectedItem: number) => void;
  selectedIndex?: number;
  defaultSelection?: number;
  switchVariant?: boolean;
  invertColors?: boolean;
  label?: string;
  disabled?: boolean;
}

export type CustomTabsProps = GenericWithStyles<Props>;

/**
 * Component styles
 */
const useStyles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      width: "fit-content",
      height: 36,
      borderRadius: 8,
      backgroundColor: (props: Props) =>
        props.invertColors ? theme.ptas.colors.theme.white : "#333333",
      alignItems: "center",
      minWidth: "unset",
      minHeight: "unset"
    },
    rootVariant: {
      height: "22px !important"
    },
    item: {
      width: 98,
      height: 32,
      color: (props: Props) =>
        props.invertColors
          ? theme.ptas.colors.theme.black
          : theme.ptas.colors.theme.white,
      borderRadius: 8,
      margin: "0px 2px 0px 2px",
      minWidth: "unset",
      minHeight: "unset",
      textTransform: "none",
      fontSize: "0.875rem",
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontWeight: "bold",
      lineHeight: theme.ptas.typography.lineHeight,
      opacity: "unset",
      transition: "all 300ms cubic-bezier(0.4, 0, 0.2, 1) 0ms",
      zIndex: 2
    },
    indicator: {
      backgroundColor: (props: Props) =>
        props.invertColors
          ? theme.ptas.colors.theme.black
          : theme.ptas.colors.theme.white,
      borderRadius: 8,
      height: 32,
      zIndex: 1
    },
    selectedItem: {
      opacity: "unset",
      color: (props: Props) =>
        props.invertColors ? theme.ptas.colors.theme.white : "#333333"
    },
    variantIndicator: {
      backgroundColor: (props: Props) =>
        props.invertColors
          ? theme.ptas.colors.theme.black
          : theme.ptas.colors.theme.white,
      borderRadius: 7,
      height: "55%",
      zIndex: 1,
      bottom: 7
    },
    variantItem: {
      width: "72px !important"
    },
    label: {
      fontSize: "0.625rem",
      marginBottom: theme.spacing(0.25)
    }
  });

/**
 * CustomTabs
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomTabs(props: CustomTabsProps): JSX.Element {
  const {
    classes,
    items,
    onSelected,
    defaultSelection,
    switchVariant,
    selectedIndex,
    label
  } = props;
  const [value, setValue] = useState(0);

  const handleChange = (newValue: number) => {
    setValue(newValue);
    onSelected(newValue);
  };

  useMount(() => {
    const getDefaultSelection = (): number => {
      if (!defaultSelection) return 0;
      if (defaultSelection <= items.length - 1) {
        return defaultSelection;
      } else {
        return 0;
      }
    };
    setValue(getDefaultSelection());
  });

  useEffect(() => {
    if (selectedIndex === undefined) return;
    if (defaultSelection !== undefined) return;
    if (selectedIndex >= 0 && selectedIndex < items.length) {
      setValue(selectedIndex);
    } else {
      console.error(`index ${selectedIndex} out of range for CustomTabs`);
    }
  }, [selectedIndex]);

  return (
    <Box>
      {label && (
        <FormLabel component='legend' classes={{ root: classes?.label }}>
          {label}
        </FormLabel>
      )}
      <Tabs
        value={value}
        onChange={(_e, v) => handleChange(v)}
        aria-label='tabs select'
        classes={{
          root: switchVariant
            ? `${classes?.rootVariant} ${classes?.root}`
            : classes?.root,
          indicator: switchVariant
            ? classes?.variantIndicator
            : classes?.indicator
        }}
      >
        {items?.map((itemName, index) => {
          return (
            <Tab
              key={index}
              label={itemName}
              id={`tab-${index}`}
              classes={{
                root: switchVariant
                  ? `${classes?.item} ${classes?.variantItem}`
                  : classes?.item,
                selected: classes?.selectedItem
              }}
              disableRipple
              disabled={props.disabled}
            />
          );
        })}
      </Tabs>
    </Box>
  );
}

export default withStyles(useStyles)(CustomTabs) as FC<CustomTabsProps>;
