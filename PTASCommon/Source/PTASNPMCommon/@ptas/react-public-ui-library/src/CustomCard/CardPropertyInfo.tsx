import React, { FC, PropsWithChildren, useMemo } from "react";
import { Theme } from "@material-ui/core/styles";
import {
  Box,
  createStyles,
  useTheme,
  WithStyles,
  withStyles
} from "@material-ui/core";
import CustomCard from "./CustomCard";
import { CustomTabs } from "../CustomTabs";
import { CardProps } from "material-ui";
import { ImageWithZoom } from "..";
import HomeIcon from "@material-ui/icons/Home";
import MailIcon from "@material-ui/icons/Mail";
import { GenericWithStyles } from "@ptas/react-ui-library";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import { ReactComponent as NoImageAvailable } from "../assets/image/No_image_available.svg";
// import BrokenImageOutlinedIcon from "@mui/icons-material/BrokenImageOutlined";

interface Props
  extends PropsWithChildren<CardProps>,
    WithStyles<typeof styles> {
  /**
   * For type 'full', include props mailName, mailAddress, options and onSelected
   */
  type: "simple" | "full";
  imageSrc: string;
  id: string;
  parcelNumber: string;
  propertyName: string;
  propertyAddress: string;
  mailName?: string;
  mailAddress?: string;
  upperStripText?: string;
  /**
   * Options for tabs
   */
  options?: {
    label: string;
    disabled: boolean;
    targetRef?: React.RefObject<HTMLDivElement> | null | undefined;
  }[];
  /**
   * Event fired when selecting a tab
   */
  onSelected?: (selectedItem: number) => void;
  shadow?: boolean;
  tabsBackgroundColor?: string;
  indicatorBackgroundColor?: string;
  itemTextColor?: string;
  selectedItemTextColor?: string;
  onClick?: (id: string) => void;
  defaultSelectedIndex?: number;
  selectedIndex?: number;
}

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    root: {
      width: 600
    },
    cardContent: {
      padding: theme.spacing(0) + " !important"
    },
    upperStrip: {
      height: 21,
      background: theme.ptas.colors.utility.changed,
      textAlign: "center",
      // fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "21px"
    },
    header: (props: Props) => ({
      padding:
        props.type === "full"
          ? props.children
            ? theme.spacing(2, 2, 0, 2)
            : theme.spacing(2, 2, 0.75, 2)
          : props.children
          ? theme.spacing(2, 2, 1, 2)
          : theme.spacing(2, 2, 2, 2),
      backgroundColor: theme.ptas.colors.theme.white,
      borderRadius: !props.upperStripText ? "7px 7px 0 0" : "0",
      cursor: "pointer"
    }),
    propertyInfo: {
      display: "flex",
      flexDirection: "column",
      [theme.breakpoints.up("sm")]: {
        flexDirection: "row"
      }
    },
    pictureArea: {
      marginLeft: "auto",
      marginRight: "auto",
      width: 175,
      marginBottom: 22,
      height: 110,
      [theme.breakpoints.up("sm")]: {
        width: 135,
        height: 101,
        marginRight: theme.spacing(1),
        marginBottom: 0,
        marginLeft: 0
      }
    },
    imageZoom: {
      [theme.breakpoints.up("sm")]: {
        width: 135,
        height: 101
      },
      width: "100%",
      height: 110
    },
    picture: {
      marginBottom: theme.spacing(0.5)
    },
    noImageWrapper: {
      display: "flex",
      justifyContent: "center"
    },
    noImage: {
      width: "100px",
      height: "100px"
    },
    // noImageIcon: {
    //   width: "100%",
    //   height: "100%",
    //   color: theme.ptas.colors.theme.gray
    // },
    parcelNumberText: {
      fontSize: theme.ptas.typography.finePrint.fontSize,
      fontWeight: theme.ptas.typography.finePrint.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "15px",
      textAlign: "center",
      [theme.breakpoints.up("sm")]: {
        textAlign: "start"
      }
    },
    parcelNumber: {
      // fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontSize: 14,
      fontWeight: theme.ptas.typography.bodySmall.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "17px"
    },
    propertyDetailsArea: {},
    address: (props: Props) => ({
      marginBottom: theme.spacing(2),
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
      textAlign: "center",
      flexDirection: "column",

      [theme.breakpoints.up("sm")]: {
        display: "grid",
        gridTemplateColumns: "[col1] auto [col2] auto",
        gridTemplateRows: "[row1] 50% [row2] 50%",
        columnGap:
          props.type === "full" ? theme.spacing(0.5) : theme.spacing(0),
        justifyContent: "start",
        textAlign: "start"
      }
    }),
    addressIcon: {
      gridColumnStart: 1,
      gridColumnEnd: 2,
      gridRowStart: 1,
      gridRowEnd: 3,
      color: theme.ptas.colors.theme.grayMedium,
      width: 41,
      height: 41
    },
    addressName: {
      gridColumnStart: 2,
      gridRowStart: 1,
      gridRowEnd: 2,
      fontSize: theme.ptas.typography.body.fontSize,
      fontWeight: theme.ptas.typography.bodyBold.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "22px"
    },
    addressDetails: {
      gridColumnStart: 2,
      gridRowStart: 2,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      lineHeight: "19px",
      color: theme.ptas.colors.theme.grayMedium
    },
    /**
     * Tabs
     */
    tabs: {
      display: "flex",
      justifyContent: "center",
      marginTop: theme.spacing(2)
    },
    label: {
      cursor: "pointer",
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight
    },
    labelRed: {
      color: theme.ptas.colors.utility.danger,
      cursor: "pointer",
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight
    },
    expandIcon: {
      width: 14,
      verticalAlign: "middle",
      color: theme.ptas.colors.theme.black
    },
    /**
     * Content
     */
    content: {
      borderRadius: "0 0 7px 7px",
      display: "flex",
      justifyContent: "center",
      padding: theme.spacing(2),
      backgroundColor: theme.ptas.colors.theme.grayLight
    },
    customTabsRoot: {
      width: "100%",
      maxWidth: "100%",

      "& > div > div": {
        justifyContent: "space-between"
      },

      [theme.breakpoints.up("sm")]: {
        maxWidth: "440px"
      }
    }
  });

function CardPropertyInfo(props: Props): JSX.Element {
  const { classes } = props;
  const theme = useTheme();

  const tabOptions = useMemo(() => {
    if (props.options) {
      return props.options.map((option, i) => {
        if (i === (props.options?.length ?? 0) - 1) {
          //Color red for last tab
          return {
            label: (
              <label className={classes.labelRed}>
                {option.label}
                <ExpandMoreIcon className={classes.expandIcon} />
              </label>
            ),
            disabled: option.disabled,
            targetRef: option.targetRef
          };
        }
        return {
          label: (
            <label className={classes.label}>
              {option.label}
              <ExpandMoreIcon className={classes.expandIcon} />
            </label>
          ),
          disabled: option.disabled
        };
      });
    }
    return [];
  }, [props.options]);

  return (
    <CustomCard
      shadow={props.shadow}
      classes={{ root: classes.root, content: classes.cardContent }}
    >
      {props.upperStripText && (
        <Box className={classes.upperStrip}>{props.upperStripText}</Box>
      )}
      <Box
        className={classes.header}
        onClick={() => {
          props.onClick && props.onClick(props.id);
        }}
      >
        <Box className={classes.propertyInfo}>
          <Box className={classes.pictureArea}>
            <Box
              className={classes.picture}
              onClick={(evt) => evt.stopPropagation()}
            >
              {props.imageSrc && (
                <ImageWithZoom
                  imageUrl={props.imageSrc}
                  classes={{ root: classes.imageZoom }}
                />
              )}
              {!props.imageSrc && (
                <Box className={classes.noImageWrapper}>
                  <NoImageAvailable className={classes.noImage} />
                  {/* <BrokenImageOutlinedIcon
                    classes={{ root: classes.noImageIcon }}
                  /> */}
                </Box>
              )}
            </Box>
            {props.type === "full" && (
              <Box className={classes.parcelNumberText}>
                Parcel{" "}
                <span className={classes.parcelNumber}>
                  #{props.parcelNumber}
                </span>
              </Box>
            )}
          </Box>
          <Box className={classes.propertyDetailsArea}>
            <Box className={classes.address}>
              {props.type === "full" && (
                <HomeIcon className={classes.addressIcon} />
              )}
              <Box className={classes.addressName}>{props.propertyName}</Box>
              <Box className={classes.addressDetails}>
                {props.propertyAddress}
              </Box>
            </Box>
            {props.type === "full" && props.mailName && props.mailAddress && (
              <Box className={classes.address}>
                <MailIcon className={classes.addressIcon} />
                <Box className={classes.addressName}>{props.mailName}</Box>
                <Box className={classes.addressDetails}>
                  {props.mailAddress}
                </Box>
              </Box>
            )}
            {props.type === "simple" && (
              <Box className={classes.parcelNumberText}>
                Parcel{" "}
                <span className={classes.parcelNumber}>
                  #{props.parcelNumber}
                </span>
              </Box>
            )}
          </Box>
        </Box>
        {props.type === "full" && props.options && (
          <Box className={classes.tabs}>
            <CustomTabs
              ptasVariant='CustomLabels'
              defaultSelectedIndex={props.defaultSelectedIndex ?? -1}
              selectedIndex={props.selectedIndex}
              items={tabOptions}
              tabsBackgroundColor={
                props.tabsBackgroundColor ?? theme.ptas.colors.utility.selection
              }
              itemTextColor={
                props.itemTextColor ?? theme.ptas.colors.theme.white
              }
              selectedItemTextColor={
                props.selectedItemTextColor ?? theme.ptas.colors.theme.black
              }
              indicatorBackgroundColor={
                props.indicatorBackgroundColor ?? theme.ptas.colors.theme.white
              }
              classes={{
                root: classes.customTabsRoot
              }}
              onSelected={(tab: number) => {
                props.onSelected && props.onSelected(tab);
              }}
              opacityColor={theme.ptas.colors.theme.white}
              nonSelectableTabs={[3]} //Index 3 ("Remove" button) should be clickable but not selectable as a tab
            />
          </Box>
        )}
      </Box>
      {props.children && (
        <Box className={classes.content}>{props.children}</Box>
      )}
    </CustomCard>
  );
}
export default withStyles(styles)(CardPropertyInfo) as FC<
  GenericWithStyles<Props>
>;
