import React from "react";
import { Theme } from "@material-ui/core/styles";
import { Box, makeStyles, useTheme } from "@material-ui/core";
import CustomCard from "./CustomCard";
import { CustomTabs } from "../CustomTabs";

type Props = {
  title: string | React.ReactNode;
  subtitle: string | React.ReactNode;
  options: {
    label: string | React.ReactNode;
    disabled: boolean;
  }[];
  selectedIndex: number;
  onSelected: (selectedItem: number) => void;
  shadow?: boolean;
  tabsBackgroundColor?: string;
  indicatorBackgroundColor?: string;
  itemTextColor?: string;
  selectedItemTextColor?: string;
  classes?: {
    root?: string;
    cardContent?: string;
  };
};

/**
 * Component styles
 */
const useStyles = makeStyles((theme: Theme) => ({
  root: {},
  cardContent: {
    padding: theme.spacing(2, 4, 4, 4) + " !important"
  },
  title: {
    marginBottom: theme.spacing(1),
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontSize: "24px",
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    lineHeight: "28px"
  },
  subtitle: {
    marginBottom: theme.spacing(1),
    fontFamily: theme.ptas.typography.bodyFontFamily,
    lineHeight: "17px",
    fontSize: "14px",
    color: "rgba(0, 0, 0, 0.5)"
  }
}));

function CardWithSlider(props: Props): JSX.Element {
  const classes = useStyles(props);
  const theme = useTheme();

  return (
    <CustomCard
      shadow={props.shadow}
      classes={{ root: classes.root, content: classes.cardContent }}
    >
      <Box className={classes.title}>{props.title}</Box>
      <Box className={classes.subtitle}>{props.subtitle}</Box>
      <Box>
        <CustomTabs
          ptasVariant='SwitchMedium'
          items={props.options}
          selectedIndex={props.selectedIndex}
          onSelected={(tab: number) => {
            props.onSelected(tab);
          }}
          tabsBackgroundColor={
            props.tabsBackgroundColor ?? theme.ptas.colors.utility.selection
          }
          itemTextColor={props.itemTextColor ?? theme.ptas.colors.theme.white}
          selectedItemTextColor={
            props.selectedItemTextColor ?? theme.ptas.colors.theme.black
          }
          indicatorBackgroundColor={
            props.indicatorBackgroundColor ?? theme.ptas.colors.theme.white
          }
        />
      </Box>
    </CustomCard>
  );
}
export default CardWithSlider;
