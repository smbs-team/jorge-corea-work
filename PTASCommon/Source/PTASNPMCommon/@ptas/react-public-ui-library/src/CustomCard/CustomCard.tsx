import React, { FC, ReactNode } from "react";
import {
  Card,
  CardContent,
  WithStyles,
  createStyles,
  withStyles
} from "@material-ui/core";
import clsx from "clsx";
import { GenericWithStyles } from "@ptas/react-ui-library";

interface Props extends WithStyles<typeof styles> {
  shadow?: boolean;
  children: string | ReactNode;
  variant?: "card" | "wrapper";
}

/**
 * Component styles
 */
const styles = () =>
  createStyles({
    root: {
      width: "fit-content",
      display: "flex",
      flexDirection: "column",
      borderRadius: "9px",
      backgroundColor: "rgba(0,0,0,0)"
    },
    shadow: {
      boxShadow: "0px 2px 12px rgba(0, 0, 0, 0.25)"
    },
    noShadow: {
      boxShadow: "none"
    },
    content: {},
    rootWrap: {
      width: "fit-content",
      padding: "8px 16px 32px",
      background: "rgba(255, 255, 255, 0.6)",
      borderRadius: 24,
      backdropFilter: "blur(6px)"
    },
    wrapperContent: {
      padding: 0,
      "&:last-child": {
        padding: 0
      }
    }
  });

const CustomCard = React.forwardRef(
  (props: Props, ref): JSX.Element => {
    const { children, classes, shadow, variant } = props;

    const setClassByVariant = (): { root: string; content: string } => {
      if (variant !== "wrapper") {
        return {
          root: classes.root,
          content: classes.content
        };
      }

      return {
        root: classes.rootWrap,
        content: classes.wrapperContent
      };
    };

    return (
      <Card
        ref={ref}
        classes={{
          root: shadow
            ? clsx(setClassByVariant().root, classes.shadow)
            : clsx(setClassByVariant().root, classes.noShadow)
        }}
      >
        <CardContent classes={{ root: setClassByVariant().content }}>
          {children}
        </CardContent>
      </Card>
    );
  }
);

export default withStyles(styles)(CustomCard) as FC<GenericWithStyles<Props>>;
