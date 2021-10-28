import React, { useRef, useState, useEffect } from "react";
import Tooltip from "@material-ui/core/Tooltip";
import {
  createStyles,
  Theme,
  Typography,
  TypographyProps,
  withStyles,
  WithStyles
} from "@material-ui/core";

interface Props extends WithStyles<typeof useStyles> {
  TypographyProps?: TypographyProps;
}

const useStyles = (theme: Theme) =>
  createStyles({
    text: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      textTransform: "none",
      textOverflow: "ellipsis",
      overflow: "hidden",
      whiteSpace: "nowrap"
    }
  });

const OverflowToolTip = (props: React.PropsWithChildren<Props>) => {
  const { classes } = props;

  const [isOverflowed, setIsOverflow] = useState(false);
  const textElementRef = useRef<HTMLSpanElement | null>(null);

  useEffect(() => {
    if (!textElementRef || !textElementRef.current) return;
    setIsOverflow(
      textElementRef.current.scrollWidth > textElementRef.current.clientWidth
    );
  }, []);

  return (
    <Tooltip
      title={props.children as string}
      disableHoverListener={!isOverflowed}
    >
      <Typography
        classes={{ root: classes.text }}
        ref={textElementRef}
        {...props.TypographyProps}
      >
        {props.children}
      </Typography>
    </Tooltip>
  );
};

export default withStyles(useStyles)(OverflowToolTip);
