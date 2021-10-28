// GalleryView.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles } from "@material-ui/core";
import React, { useState, useEffect, Fragment } from "react";
import { Theme } from "@material-ui/core/styles";
import Backdrop from "@material-ui/core/Backdrop";
import Modal from "@material-ui/core/Modal";
import CloseIcon from "@material-ui/icons/Close";
import ArrowBackIosIcon from "@material-ui/icons/ArrowBackIos";
import ArrowForwardIosIcon from "@material-ui/icons/ArrowForwardIos";

/**
 * Component props
 */

interface Props {
  open: boolean;
  onClose?: () => void;
  showCloseButton?: boolean;
  showArrows?: boolean;
  urlImages: string[];
  initialImageIndex?: number;
}

/**
 * Component styles
 */
const useStyles = makeStyles((theme: Theme) => ({
  root: {
    height: "100%",
    width: "80%",
    position: "relative",
    outline: "none",
    background: "transparent"
  },
  modal: {
    display: "flex",
    alignItems: "center",
    justifyContent: "center"
  },
  closeButton: {
    position: "absolute",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    background: " rgba(0, 0, 0, 0.54)",
    borderRadius: 22,
    cursor: "pointer",
    right: 8,
    top: 8,
    color: theme.ptas.colors.theme.white
  },
  image: {
    width: "100%",
    height: "100%",
    objectFit: "cover"
  },
  nextButton: {
    position: "absolute",
    top: "50%",
    right: "-8%",
    color: theme.ptas.colors.theme.white,
    width: 44,
    height: 44,
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    background: " rgba(0, 0, 0, 0.54)",
    borderRadius: 22,
    cursor: "pointer"
  },
  prevButton: {
    top: "50%",
    left: "-8%",
    position: "absolute",
    color: theme.ptas.colors.theme.white,
    width: 44,
    height: 44,
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    background: " rgba(0, 0, 0, 0.54)",
    borderRadius: 22,
    cursor: "pointer"
  }
}));

/**
 * GalleryView
 *
 * @param props - Component props
 * @returns A JSX element
 */
function GalleryView(props: Props): JSX.Element {
  const {
    open,
    showCloseButton,
    onClose,
    urlImages,
    initialImageIndex,
    showArrows
  } = props;
  const [openModal, setOpenModal] = useState<boolean>(false);
  const [imageIndex, setImageIndex] = useState<number>(0);

  const classes = useStyles(props);

  useEffect(() => {
    setOpenModal(open);
    if (open) setImageIndex(initialImageIndex ?? 0);
  }, [open]);

  const handleClose = (): void => {
    setOpenModal((prevState: boolean) => !prevState);
    if (onClose) {
      onClose();
    }
  };

  const handleNext = () => {
    if (imageIndex < urlImages.length - 1) {
      setImageIndex(imageIndex + 1);
    }
  };

  const handlePrev = () => {
    if (imageIndex > 0) {
      setImageIndex(imageIndex - 1);
    }
  };

  const renderControls = (): JSX.Element => {
    if (!showArrows) return <Fragment />;

    return (
      <Fragment>
        {imageIndex > 0 && (
          <span onClick={handlePrev} className={classes.prevButton}>
            <ArrowBackIosIcon />
          </span>
        )}
        {imageIndex < urlImages.length - 1 && (
          <span onClick={handleNext} className={classes.nextButton}>
            <ArrowForwardIosIcon />
          </span>
        )}
      </Fragment>
    );
  };

  return (
    <Modal
      className={classes.modal}
      open={openModal}
      onClose={handleClose}
      closeAfterTransition
      BackdropComponent={Backdrop}
      BackdropProps={{
        timeout: 500
      }}
    >
      <div className={classes.root}>
        {showCloseButton && (
          <span className={classes.closeButton} onClick={handleClose}>
            <CloseIcon />
          </span>
        )}
        {renderControls()}
        <img src={urlImages[imageIndex]} className={classes.image} />
      </div>
    </Modal>
  );
}

export default GalleryView;
