// ImageWithZoom.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState } from "react";
import GalleryView from "../GalleryView";
import ZoomInIcon from "@material-ui/icons/ZoomIn";
import {
  IconButton,
  WithStyles,
  createStyles,
  withStyles
} from "@material-ui/core";

/**
 * Component props
 */

interface Props extends WithStyles<typeof useStyles> {
  imageUrl: string;
  showZoomIcon?: boolean;
}

/**
 * Component styles
 */
const useStyles = () =>
  createStyles({
    root: {
      borderRadius: 9,
      height: 101,
      width: 135,
      position: "relative",
      overflow: "hidden"
    },
    image: {
      objectFit: "cover",
      width: "100%",
      height: "100%"
    },
    zoomButton: {
      position: "absolute",
      right: 10,
      bottom: 10,
      padding: 0
    }
  });

/**
 * ImageWithZoom
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ImageWithZoom(props: Props): JSX.Element {
  const { imageUrl, showZoomIcon, classes } = props;
  const [open, setOpen] = useState<boolean>(false);

  const toggleGalleryImage = () => {
    setOpen((prevState) => !prevState);
  };

  const renderZoomIcon = (): JSX.Element => {
    if (!showZoomIcon) return <Fragment />;

    return (
      <IconButton onClick={toggleGalleryImage} className={classes.zoomButton}>
        <ZoomInIcon />
      </IconButton>
    );
  };

  const handleClick = (): void => {
    if (showZoomIcon) return;

    toggleGalleryImage();
  };

  return (
    <div className={classes.root}>
      <img src={imageUrl} onClick={handleClick} className={classes.image} />
      <GalleryView
        open={open}
        urlImages={[imageUrl]}
        onClose={toggleGalleryImage}
        showCloseButton
        showArrows={false}
      />
      {renderZoomIcon()}
    </div>
  );
}

export default withStyles(useStyles)(ImageWithZoom);
