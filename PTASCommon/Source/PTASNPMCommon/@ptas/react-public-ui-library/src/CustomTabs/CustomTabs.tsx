// CustomTabs.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, ReactNode, useEffect, useState, useRef } from "react";
import { Tab, Tabs, withStyles, WithStyles } from "@material-ui/core";
import clsx from "clsx";
import { styles } from "./styles";
import { useMount } from "react-use";
import { GenericWithStyles } from "@ptas/react-ui-library";

/**
 * Component props
 */
export interface CustomTabsProps extends WithStyles<typeof styles> {
  items: tabItem[];
  ptasVariant?:
    | "Simple"
    | "CustomLabels"
    | "Switch"
    | "SwitchMedium"
    | "SwitchSmall";
  onSelected: (selectedItem: number) => void;
  defaultSelectedIndex?: number; //Set initially selected tab, for none tab selected, set this to -1
  selectedIndex?: number;
  tabsBackgroundColor?: string;
  indicatorBackgroundColor?: string;
  itemTextColor?: string;
  /** default color is accent */
  opacityColor?: string;
  selectedItemTextColor?: string;
  nonSelectableTabs?: number[]; //Tab indexes that can be clicked, but not selectable (no indicator)
}

export interface tabItem {
  label: string | ReactNode;
  disabled?: boolean;
  targetRef?: React.RefObject<HTMLDivElement> | null;
}

/**
 * CustomTabs
 *
 * @param props - Component props
 * @returns A JSX element
 */
function CustomTabs(props: CustomTabsProps): JSX.Element {
  const {
    items,
    onSelected,
    defaultSelectedIndex,
    selectedIndex,
    nonSelectableTabs,
    classes
  } = props;
  const [value, setValue] = useState<number>(-1);
  const [hasScroll, setHasScroll] = useState<boolean>(false);
  const selectItemRef = useRef<HTMLDivElement | null>(null);
  const tabWrapRef = useRef<HTMLDivElement | null>(null);

  useMount(() => {
    const getDefaultSelectedIndex = (): number => {
      if (!defaultSelectedIndex) return 0;
      if (defaultSelectedIndex <= items.length - 1) {
        return defaultSelectedIndex;
      } else {
        return 0;
      }
    };
    setValue(getDefaultSelectedIndex());
  });

  useEffect(() => {
    if (selectedIndex === undefined) return;
    if (defaultSelectedIndex !== undefined) return;
    if (selectedIndex >= 0 && selectedIndex < items.length) {
      setValue(selectedIndex);
    } else {
      console.error(`index ${selectedIndex} out of range for CustomTabs`);
      setValue(0);
    }
  }, [selectedIndex]);

  const setVariant = (
    type: "root" | "indicator" | "item" | "selectedItem"
  ): string => {
    switch (props.ptasVariant) {
      case "Switch":
        return type === "root"
          ? clsx(classes.root, classes.rootSwitch)
          : type === "indicator"
          ? clsx(classes.indicator, classes.indicatorSwitch)
          : type === "item"
          ? clsx(classes.item, classes.itemSwitch)
          : clsx(classes.selectedItem, classes.selectedItemSwitch);
      case "SwitchMedium":
        return type === "root"
          ? clsx(classes.root, classes.rootSwitchMedium)
          : type === "indicator"
          ? clsx(classes.indicator, classes.indicatorSwitchMedium)
          : type === "item"
          ? clsx(classes.item, classes.itemSwitchMedium)
          : clsx(classes.selectedItem, classes.selectedItemSwitchMedium);
      case "SwitchSmall":
        return type === "root"
          ? clsx(classes.root, classes.rootSwitchSmall)
          : type === "indicator"
          ? clsx(classes.indicator, classes.indicatorSwitchSmall)
          : type === "item"
          ? clsx(classes.item, classes.itemSwitchSmall)
          : clsx(classes.selectedItem, classes.selectedItemSwitchSmall);
      case "CustomLabels":
        return type === "root"
          ? clsx(classes.root, classes.rootCustomLabels)
          : type === "indicator"
          ? clsx(classes.indicator, classes.indicatorCustomLabels)
          : type === "item"
          ? clsx(classes.item, classes.itemSimple)
          : clsx(classes.selectedItem, classes.selectedItemSimple);
      case "Simple":
      default:
        return type === "root"
          ? clsx(classes.root, classes.rootSimple)
          : type === "indicator"
          ? clsx(classes.indicator, classes.indicatorSimple)
          : type === "item"
          ? clsx(classes.item, classes.itemSimple)
          : clsx(classes.selectedItem, classes.selectedItemSimple);
    }
  };

  const handleChange = (newValue: number) => {
    if (!nonSelectableTabs || nonSelectableTabs.indexOf(newValue) === -1) {
      setValue(newValue);
    }
    onSelected(newValue);
  };

  useEffect(() => {
    const itemRef = items[value]?.targetRef ?? selectItemRef;

    if (itemRef) {
      itemRef.current?.scrollIntoView({
        behavior: "smooth",
        block: "nearest",
        inline: "center"
      });
    }
  }, [value]);

  const handleSetHasScroll = (): void => {
    const tabWrapper = tabWrapRef.current?.childNodes[0];

    if (!tabWrapper) return;

    const listWrap = tabWrapper?.childNodes[0];

    if (!listWrap) return;

    const buttonElements = Array.from(
      listWrap.childNodes as NodeListOf<HTMLDivElement>
    );

    if (!buttonElements.length) return;

    const totalWidthButtons = buttonElements.reduce(
      (acc, currentItem) => acc + currentItem.clientWidth,
      0
    );

    const itemsWrapperWidth = tabWrapRef.current?.clientWidth ?? 0;

    setHasScroll(() => totalWidthButtons > itemsWrapperWidth);
  };

  useEffect(() => {
    handleSetHasScroll();
    window.addEventListener("resize", handleSetHasScroll);

    return () => {
      window.removeEventListener("resize", handleSetHasScroll);
    };
  }, []);

  const setOpacity = (): string => {
    const showOpacityStart = value !== 0;
    const showOpacityEnd = value !== items.length - 1;

    return clsx(
      showOpacityStart && classes.opacityStart,
      showOpacityEnd && classes.opacityEnd
    );
  };

  const handleSetLocalRef = (index: number) => (
    ref: HTMLDivElement | null
  ): void => {
    if (index === value) {
      selectItemRef.current = ref;
    }
  };

  return (
    <Tabs
      value={value < 0 ? false : value}
      classes={{
        root: clsx(setVariant("root"), hasScroll && setOpacity()),
        indicator: setVariant("indicator")
      }}
      onChange={(_e, v) => handleChange(v)}
      innerRef={tabWrapRef}
    >
      {items?.map((item, index) => {
        return (
          <Tab
            ref={item.targetRef ?? handleSetLocalRef(index)}
            key={index}
            label={item.label}
            id={`tab-${index}`}
            classes={{
              root: setVariant("item"),
              selected: setVariant("selectedItem")
            }}
            disableRipple
            disabled={item.disabled}
          />
        );
      })}
    </Tabs>
  );
}

export default withStyles(styles)(CustomTabs) as FC<
  GenericWithStyles<CustomTabsProps>
>;
