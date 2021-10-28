import React, { FC } from "react";
import { Box } from "@material-ui/core";
import {
  WithStyles,
  createStyles,
  withStyles,
  Theme
} from "@material-ui/core/styles";
import { GenericWithStyles } from "@ptas/react-ui-library";
import { PropsWithChildren } from "react";

interface Props extends WithStyles<typeof styles>, PropsWithChildren<{}> {
  width: number;
  header: React.ReactNode;
  refs: {
    headerRef: React.MutableRefObject<HTMLDivElement>;
    contentRef: React.MutableRefObject<HTMLDivElement>;
  };
}

/**
 * Component styles
 */
const styles = (theme: Theme) =>
  createStyles({
    root: {
      width: "fit-content",
      padding: 0,
      boxSizing: "border-box",
      // background: "rgba(255, 255, 255, 0.6)",
      // backdropFilter: "blur(6px)"
      backgroundColor: "transparent",
      borderRadius: 0,
      marginLeft: "auto",
      marginRight: "auto",

      [theme.breakpoints.up("sm")]: {
        borderRadius: 24
      }
    },
    header: {
      width: "100%",
      display: "flex",
      flexDirection: "column",
      justifyContent: "center",
      alignItems: "center",
      position: "sticky",
      top: "0",
      boxSizing: "border-box",
      padding: "16px 16px 0 16px",
      zIndex: 1,
      backgroundColor: "white",
      borderRadius: 0,

      [theme.breakpoints.up("sm")]: {
        borderRadius: "24px 24px 0 0"
      }
    },
    content: {
      display: "flex",
      justifyContent: "center",
      alignItems: "center",
      flexDirection: "column",
      width: "100%",
      backgroundColor: theme.ptas.colors.theme.white,
      borderRadius: 0,

      [theme.breakpoints.up("sm")]: {
        borderRadius: "0 0 24px 24px"
      }
    }
  });

const MainCard = (props: Props): JSX.Element => {
  const { children, header, classes, refs, width } = props;

  return (
    <Box className={classes.root}>
      <div
        ref={refs.headerRef}
        className={classes.header}
        style={{ width: width }}
      >
        {header}
        <div
          style={{
            width: "702px",
            height: "22px",
            position: "absolute",
            bottom: "-22px",
            background:
              "linear-gradient(0deg, rgba(255,255,255,0) 0%, rgba(255,255,255,1) 100%)"
          }}
        ></div>
      </div>
      <div
        ref={refs.contentRef}
        className={classes.content}
        style={{ width: width }}
      >
        {children}
      </div>
    </Box>
  );
};

export default withStyles(styles)(MainCard) as FC<GenericWithStyles<Props>>;
