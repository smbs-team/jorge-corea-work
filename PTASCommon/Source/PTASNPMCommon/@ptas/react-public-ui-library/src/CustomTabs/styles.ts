//styles.ts

import { createStyles, Theme } from "@material-ui/core/styles";
import { CustomTabsProps } from "./CustomTabs";
const VARIANT_SWITCH_ITEM_HEIGHT = 32;
const VARIANT_SWITCH_MEDIUM_ITEM_HEIGHT = 24;
const VARIANT_SWITCH_SMALL_ITEM_HEIGHT = 19;

/**
 * Component styles
 */
export const styles = (theme: Theme) =>
  createStyles({
    root: {
      display: "flex",
      width: "fit-content",
      height: 36,
      alignItems: "center",
      minWidth: "unset",
      minHeight: "unset",
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: theme.ptas.typography.lineHeight,
      maxWidth: "100%",
      position: "relative",

      "& > div": {
        overflowX: "auto !important"
      },

      "& > div::-webkit-scrollbar": {
        display: "none !important"
      }
    },
    opacityStart: {
      "&:before": {
        content: "''",
        zIndex: 200,
        position: "absolute",
        background: (props: CustomTabsProps): string =>
          `linear-gradient(90deg, ${props.opacityColor ??
            theme.ptas.colors.theme.accent} 36%, rgba(255,255,255,0) 100%)`,
        top: 0,
        left: -1,
        height: "100%",
        width: "30px"
      }
    },
    opacityEnd: {
      "&:after": {
        content: "''",
        zIndex: 200,
        position: "absolute",
        background: (props: CustomTabsProps): string =>
          `linear-gradient(90deg, rgba(255,255,255,0) -9%, ${props.opacityColor ??
            theme.ptas.colors.theme.accent} 68%)`,
        top: 0,
        right: -1,
        height: "100%",
        width: "30px"
      }
    },
    rootSimple: (props: CustomTabsProps) => ({
      borderBottom: `1px solid ${props.itemTextColor ??
        theme.ptas.colors.theme.accent}`
    }),
    rootCustomLabels: {},
    rootSwitch: (props: CustomTabsProps) => ({
      height: "40px !important",
      boxSizing: "border-box",
      borderRadius: 20,
      backgroundColor:
        props.tabsBackgroundColor ?? theme.ptas.colors.theme.accent,
      padding: 4
    }),
    rootSwitchMedium: (props: CustomTabsProps) => ({
      height: "29px !important",
      boxSizing: "border-box",
      borderRadius: 14.5,
      backgroundColor:
        props.tabsBackgroundColor ?? theme.ptas.colors.theme.accent,
      padding: "2px 3px"
    }),
    rootSwitchSmall: (props: CustomTabsProps) => ({
      height: "24px !important",
      boxSizing: "border-box",
      borderRadius: 12,
      backgroundColor:
        props.tabsBackgroundColor ?? theme.ptas.colors.theme.accent,
      padding: 2
    }),
    item: {
      padding: 0,
      minWidth: "unset",
      minHeight: "unset",
      textTransform: "none",
      opacity: "unset",
      transition: "all 300ms cubic-bezier(0.4, 0, 0.2, 1) 0ms",
      zIndex: 2
    },
    itemSimple: (props: CustomTabsProps) => ({
      height: 36,
      margin: "0px 12px 0px 2px",
      color: props.itemTextColor ?? theme.ptas.colors.theme.accent
    }),
    itemSwitch: (props: CustomTabsProps) => ({
      height: VARIANT_SWITCH_ITEM_HEIGHT,
      padding: "0 16px",
      color: props.itemTextColor ?? theme.ptas.colors.theme.white,
      fontSize: theme.ptas.typography.bodyLarge.fontSize,
      lineHeight: "24px"
    }),
    itemSwitchMedium: (props: CustomTabsProps) => ({
      height: VARIANT_SWITCH_MEDIUM_ITEM_HEIGHT,
      padding: "0 16px",
      color: props.itemTextColor ?? theme.ptas.colors.theme.white,
      fontSize: "14px",
      lineHeight: "17px"
    }),
    itemSwitchSmall: (props: CustomTabsProps) => ({
      height: VARIANT_SWITCH_SMALL_ITEM_HEIGHT,
      padding: "0 8px",
      color: props.itemTextColor ?? theme.ptas.colors.theme.white,
      fontSize: theme.ptas.typography.finePrint.fontSize,
      lineHeight: "17px"
    }),
    indicator: {
      backgroundColor: theme.ptas.colors.theme.black,
      height: "4px",
      zIndex: 1
    },
    indicatorSimple: (props: CustomTabsProps) => ({
      backgroundColor:
        props.indicatorBackgroundColor ?? theme.ptas.colors.theme.black
    }),
    indicatorCustomLabels: (props: CustomTabsProps) => ({
      backgroundColor:
        props.indicatorBackgroundColor ?? theme.ptas.colors.theme.black,
      borderRadius: 2
    }),
    indicatorSwitch: (props: CustomTabsProps) => ({
      height: VARIANT_SWITCH_ITEM_HEIGHT,
      backgroundColor:
        props.indicatorBackgroundColor ?? theme.ptas.colors.theme.white,
      borderRadius: VARIANT_SWITCH_ITEM_HEIGHT / 2,
      zIndex: 1
    }),
    indicatorSwitchMedium: (props: CustomTabsProps) => ({
      height: VARIANT_SWITCH_MEDIUM_ITEM_HEIGHT,
      backgroundColor:
        props.indicatorBackgroundColor ?? theme.ptas.colors.theme.white,
      borderRadius: VARIANT_SWITCH_MEDIUM_ITEM_HEIGHT / 2,
      zIndex: 1
    }),
    indicatorSwitchSmall: (props: CustomTabsProps) => ({
      height: VARIANT_SWITCH_SMALL_ITEM_HEIGHT,
      backgroundColor:
        props.indicatorBackgroundColor ?? theme.ptas.colors.theme.white,
      borderRadius: VARIANT_SWITCH_SMALL_ITEM_HEIGHT / 2,
      zIndex: 1
    }),
    selectedItem: {
      opacity: "unset"
    },
    selectedItemSimple: {},
    selectedItemSwitch: (props: CustomTabsProps) => ({
      color: props.selectedItemTextColor ?? theme.ptas.colors.theme.accent
    }),
    selectedItemSwitchMedium: (props: CustomTabsProps) => ({
      color: props.selectedItemTextColor ?? theme.ptas.colors.theme.accent
    }),
    selectedItemSwitchSmall: (props: CustomTabsProps) => ({
      color: props.selectedItemTextColor ?? theme.ptas.colors.theme.accent
    })
  });
