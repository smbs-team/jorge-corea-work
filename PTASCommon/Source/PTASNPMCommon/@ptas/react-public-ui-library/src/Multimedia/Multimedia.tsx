// Multimedia.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { FC, PropsWithChildren, useState } from "react";
import {
  createStyles,
  Theme,
  WithStyles,
  withStyles
} from "@material-ui/core/styles";
import Vimeo, { VimeoProps, TimeUpdateEvent } from "@u-wave/react-vimeo";
import { omit } from "lodash";
import { GenericWithStyles } from "@ptas/react-ui-library";

interface Props
  extends PropsWithChildren<VimeoProps>,
    WithStyles<typeof useStyles> {
  showTime?: boolean;
}

/**
 * Component styles
 */

const useStyles = (theme: Theme) =>
  createStyles({
    root: ({ width, height }: Props) => ({
      position: "relative",
      width: "100%",
      maxWidth: width ? `${width}px` : "300px",
      height: height ? `${width}px` : "300px",
      marginLeft: "auto",
      marginRight: "auto"
    }),
    time: {
      position: "absolute",
      right: 7,
      top: 3,
      fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
      zIndex: 10
    }
  });

const VimeoPlayer = (props: Props) => {
  const [time, setTime] = useState<string>("00:00");
  const { classes } = props;
  const newProps = omit(props, ["showTime"]);

  const handleChangeTime = (event: TimeUpdateEvent) => {
    const date = new Date(event.seconds * 1000).toISOString().substr(14, 5);
    setTime(date);
  };

  return (
    <div className={classes.root}>
      {props.showTime && <span className={classes.time}>{time}</span>}
      <Vimeo
        id='iframe-multimedia-vimeo-id'
        onTimeUpdate={handleChangeTime}
        {...newProps}
      />
    </div>
  );
};

export default withStyles(useStyles)(VimeoPlayer) as FC<
  GenericWithStyles<Props>
>;
