// PropertyInfo.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles } from "@material-ui/core";
import React, { ReactNode, useState } from "react";
import { Theme } from "@material-ui/core/styles";
import "react-responsive-carousel/lib/styles/carousel.min.css";
import { Carousel } from "react-responsive-carousel";
import ZoomInIcon from "@material-ui/icons/ZoomIn";
import GalleryView from "../GalleryView";

/**
 * Component props
 */

interface Props {
  urlImages: string[];
  children: ReactNode;
}

/**
 * Component styles
 */
const useStyles = makeStyles((theme: Theme) => ({
  root: {
    color: theme.ptas.colors.theme.white,
    borderRadius: 9,
    position: "relative",
    overflow: "hidden",
    display: "flex",
    flexDirection: "column",
    [theme.breakpoints.up("sm")]: {
      flexDirection: "row"
    }
  },
  carousel: {
    borderRadius: 9,
    height: "300px !important",
    maxWidth: "initial",
    overflow: "hidden",
    width: "100%",

    [theme.breakpoints.up("sm")]: {
      marginRight: 16,
      maxWidth: "400px"
    },

    "& .carousel-status": {
      width: 46,
      height: 25,
      right: 16,
      display: "flex",
      justifyContent: "center",
      alignItems: "center",
      top: "initial",
      bottom: 0,
      color: theme.ptas.colors.theme.black,
      fontSize: theme.ptas.typography.bodySmall.fontSize,
      padding: 0,
      textShadow: "none",
      fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
      background: "rgba(255, 255, 255, 0.5)",
      borderRadius: 15,
      overflow: "hidden"
    }
  },
  image: {
    height: 300,
    objectFit: "cover"
  },
  zoomIcon: {
    height: 27,
    width: 27,
    background: "rgba(255, 255, 255, 0.5)",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    borderRadius: 15,
    position: "absolute",
    left: 16,
    top: 260,
    color: theme.ptas.colors.theme.black,
    cursor: "pointer"
  }
}));

/**
 * PropertyInfo
 *
 * @param props - Component props
 * @returns A JSX element
 */
function PropertyInfo(props: Props): JSX.Element {
  const { urlImages, children } = props;
  const [open, setOpen] = useState<boolean>(false);
  const [autoPlay, setAutoplay] = useState<boolean>(true);
  const [imageindex, setImageIndex] = useState<number>(0);

  const classes = useStyles(props);

  const renderCarousel = (): JSX.Element[] => {
    return urlImages.map((url) => <img src={url} className={classes.image} />);
  };

  const toggleGalleryImage = () => {
    setOpen((prevState) => !prevState);
    handleAutoPlay();
  };

  const handleAutoPlay = () => setAutoplay((prevState) => !prevState);

  const handleSetImageIndex = (index: number) => setImageIndex(index);

  return (
    <div className={classes.root}>
      <Carousel
        stopOnHover={true}
        emulateTouch
        autoPlay={autoPlay}
        showIndicators={false}
        infiniteLoop
        showThumbs={false}
        showArrows={false}
        className={classes.carousel}
        dynamicHeight={false}
        onChange={handleSetImageIndex}
      >
        {renderCarousel()}
      </Carousel>
      <span className={classes.zoomIcon} onClick={toggleGalleryImage}>
        <ZoomInIcon />
      </span>
      {children}
      <GalleryView
        open={open}
        urlImages={urlImages}
        onClose={toggleGalleryImage}
        showCloseButton
        initialImageIndex={imageindex}
        showArrows
      />
    </div>
  );
}

export default PropertyInfo;
