// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, useState } from 'react';
import Modal from '@material-ui/core/Modal';
import { makeStyles, Theme } from '@material-ui/core/styles';
import Paper from '@material-ui/core/Paper';
import Grid from '@material-ui/core/Grid';
import { HomeContext } from 'contexts';
import { Checkbox, FormControlLabel, IconButton } from '@material-ui/core';
import { Close, CloudDownload } from '@material-ui/icons';
import html2canvas from 'html2canvas';
import kingCountyLogo from '../../assets/img/king-county-logo.svg';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    flexGrow: 1,
  },
  paper: {
    padding: theme.spacing(4),
    color: theme.palette.text.secondary,
  },
  modal: {
    display: 'flex',
    alignContent: 'center',
    alignItems: 'center',
    justifyContent: 'center',
    padding: '10px',
  },
  floatToRight: {
    display: 'flex',
    justifyContent: 'flex-end',
  },
  img: {
    height: 'auto',
    width: '100%',
  },
  control: {
    paddingLeft: '20px',
    paddingRight: '20px',
    width: '100%',
  },
  mapInfo: {
    backgroundColor: '#FFF',
    padding: '20px',
  },
  logoContainer: {
    display: 'flex',
    flexDirection: 'row',
    justifyContent: 'center',
    padding: 5,
    width: '100%',
  },
  modalContent: {
    maxHeight: '80vh',
    overflowY: 'scroll',
  },
  label: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.formLabel.fontSize,
    fontWeight: theme.ptas.typography.formLabel.fontWeight,
    lineHeight: theme.ptas.typography.lineHeight,
  },
  disclaimerText: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodySmall.fontWeight,
    lineHeight: theme.ptas.typography.lineHeight,
  },
  date: {
    display: 'flex',
    justifyContent: 'flex-end',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.finePrint.fontSize,
    fontWeight: theme.ptas.typography.finePrint.fontWeight,
    lineHeight: theme.ptas.typography.lineHeight,
  },
}));

const ExportMapImgModal = (): JSX.Element => {
  const [showLogo, setShowLogo] = useState<boolean>(false);
  const [showDisclaimer, setShowDisclaimer] = useState<boolean>(false);
  const [showDate, setShowDate] = useState<boolean>(false);
  const {
    openExportMapImgModal,
    setOpenExportMapImgModal,
    mapImgCaptureSrc,
  } = useContext(HomeContext);
  const currenDateTime = new Date();
  const classes = useStyles();

  const onCloseModal = (): void => {
    setOpenExportMapImgModal(false);
  };

  const changeShowLogo = (
    e: React.ChangeEvent<HTMLInputElement>,
    checked: boolean
  ): void => setShowLogo(checked);

  const changeShowDisclaimer = (
    e: React.ChangeEvent<HTMLInputElement>,
    checked: boolean
  ): void => setShowDisclaimer(checked);

  const changeShowDate = (
    e: React.ChangeEvent<HTMLInputElement>,
    checked: boolean
  ): void => setShowDate(checked);

  const downloadImg = (): void => {
    const container = document.getElementById('map-info-ss');
    html2canvas(container as HTMLElement, {
      scale: 4,
      allowTaint: true,
    }).then(function (canvas) {
      const img = canvas.toDataURL('image/png');
      const link = document.createElement('a');
      link.href = img;
      link.download = 'map.png';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    });
  };

  return (
    <Modal
      open={openExportMapImgModal}
      onClose={onCloseModal}
      className={classes.modal}
    >
      <div className={classes.root}>
        <Paper className={classes.paper}>
          <div className={classes.control}>
            <Grid container spacing={3}>
              <Grid item xs={11}>
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={showDisclaimer}
                      onChange={changeShowDisclaimer}
                      name="cb-show-disclaimer"
                      color="primary"
                    />
                  }
                  label={<span className={classes.label}>Show disclaimer</span>}
                />
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={showLogo}
                      onChange={changeShowLogo}
                      name="cb-show-logo"
                      color="primary"
                    />
                  }
                  label={<span className={classes.label}>Show logo</span>}
                />
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={showDate}
                      onChange={changeShowDate}
                      name="cb-show-date"
                      color="primary"
                    />
                  }
                  label={<span className={classes.label}>Show date</span>}
                />
                <label>
                  <IconButton
                    color="primary"
                    component="span"
                    onClick={downloadImg}
                  >
                    <CloudDownload />
                  </IconButton>
                </label>
              </Grid>
              <Grid item xs={1}>
                <label className={classes.floatToRight}>
                  <IconButton
                    color="primary"
                    component="span"
                    onClick={onCloseModal}
                  >
                    <Close />
                  </IconButton>
                </label>
              </Grid>
            </Grid>
          </div>
          <div className={classes.modalContent}>
            <div id="map-info-ss" className={classes.mapInfo}>
              <Grid container spacing={3}>
                <Grid item xs={showDisclaimer || showLogo ? 9 : 12}>
                  <img
                    src={mapImgCaptureSrc}
                    className={classes.img}
                    alt="map preview"
                  />
                </Grid>
                {(showDisclaimer || showLogo) && (
                  <Grid item xs={3}>
                    {showDisclaimer && (
                      <span className={classes.disclaimerText}>
                        The information included on this map has been compiled
                        by King County staff from a variety of sources and is
                        subject to change without notice. King County makes no
                        representations or warranties, express or implied, as to
                        accuracy, completeness, timeliness, or rights to the use
                        of such information. King County shall not be liable for
                        any general, special, indirect, incidental, or
                        consequential damages including, but not limited to,
                        lost revenues or lost profits resulting from the use or
                        misuse of the information contained on this map. Any
                        sale of this map or information on this map is
                        prohibited except by written permission of King County.
                      </span>
                    )}
                    {showLogo && (
                      <span className={classes.logoContainer}>
                        <img src={kingCountyLogo} alt="King county logo" />
                      </span>
                    )}
                  </Grid>
                )}
                <Grid item xs={12}>
                  {showDate && (
                    <span className={classes.date}>
                      {`${currenDateTime.toLocaleDateString()} ${currenDateTime.toLocaleTimeString()}`}
                    </span>
                  )}
                </Grid>
              </Grid>
            </div>
          </div>
        </Paper>
      </div>
    </Modal>
  );
};

export default ExportMapImgModal;
