import React, { useState, useEffect } from "react";
import { makeStyles } from "@material-ui/core/styles";

import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import "./VimeoPlayer.css";
import TableRow from "@material-ui/core/TableRow";
import Vimeo from "@u-wave/react-vimeo";
import IconButton from "@material-ui/core/IconButton";
import CloseIcon from "@material-ui/icons/Close";
import LiveTvIcon from "@material-ui/icons/LiveTv";
import PlayCircleOutlineIcon from "@material-ui/icons/PlayCircleOutline";
import MuiDialogTitle from "@material-ui/core/DialogTitle";
import StopIcon from "@material-ui/icons/Stop";
import Dialog from "@material-ui/core/Dialog";
import Grid from "@material-ui/core/Grid";
import DialogContent from "@material-ui/core/DialogContent";
import * as vimeoHelper from "../../../services/vimeoHelper";
import { FormattedMessage } from "react-intl";
import deepEqual from "deep-equal";
import { cloneDeep } from "lodash";

export default function VimeoPlayer(props) {
  const useStyles = makeStyles(theme => ({
    tableRow: {
      cursor: "pointer",
      borderBottom: "2px solid black"
    },
    tableRowActive: {
      cursor: "pointer",
      backgroundColor: "#d4e693",
      borderBottom: "2px solid black"
    },
    closeButton: {
      position: "absolute",
      right: theme.spacing(1),
      top: theme.spacing(1),
      color: "black"
    },
    table: {
      float: "right",
      width: props.isPopup || props.showList ? "100%" : "25%",
      backgroundColor: "white"
    },

    player: {
      float: "left",
      width: "100%",
      "& iframe": {
        paddingLeft: "0px",
        maxWidth: "1900px !important"
      }
    },
    playerList: {
      float: "left",
      width: "70%",
      "& iframe": {
        paddingLeft: "0px"
      }
    },
    image: {
      border: "1px solid black",
      maxWidth: "97%",
      height: "auto"
    },
    span: {
      color: "black",
      fontWeight: "bold",
      fontSize: 12,
      margin: 5,
      display: "flex",
      alignItems: "center"
    },
    tableCell: {
      position: "relative"
    },
    spanTime: {
      position: "absolute",
      right: theme.spacing(1),
      bottom: theme.spacing(1),
      color: "black"
    },
    playbar: {
      backgroundColor: "#444444",
      color: "white"
    },
    playSpan: {
      bottom: "5px",
      margin: "5px",
      position: "relative"
    },
    videoListDialog: {
      overflow: "auto",
      height: props.height + "px",
      float: "left",
      width: "30%"
    }
  }));

  const classes = useStyles();
  const [videoIndex, setVideoIndex] = useState(0);
  const [videoList, setVideoList] = useState([]);
  const [videos, setVideos] = useState([]);
  const [fetchingVideoList, setFetchingVideoList] = useState(false);
  const [defaultVideo, setDefaultVideo] = useState(null);
  const [isPlaying, setIsPlaying] = useState(true);

  useEffect(() => {
    // Load video list

    if (!deepEqual(props.videos, videos) && !fetchingVideoList) {
      setFetchingVideoList(true);
      setVideos(cloneDeep(props.videos));
      const results = props.videos.map(async x => {
        return vimeoHelper.getVimeoData(x.url);
      });

      Promise.all(results).then(completed => {
        setVideoList(completed);
        setFetchingVideoList(false);
      });
    }
    // DONT ADD MORE VARIABLES HERE: props.videos
  }, [props.videos]);

  useEffect(() => {
    //Set Default video selected

    if (
      videoList &&
      videoList.length > 0 &&
      !defaultVideo &&
      props.defaultUrl
    ) {
      let videoToSet = videoList.findIndex(
        video => video.url == props.defaultUrl
      );
      setDefaultVideo(props.defaultUrl);
      setCurrIndex(videoToSet);
      // DONT ADD MORE VARIABLES HERE: videoList
    }
  }, [videoList]);

  function handleCloseDialog() {
    if (props.onPopupClose) {
      props.onPopupClose();
    }
  }

  function Next() {
    let newVideoIndex = videoIndex + 1;
    if (newVideoIndex < props.videos.length) {
      setVideoIndex(newVideoIndex);
    } else {
      handlePlay();
    }
  }

  function onPlay() {
    if (!isPlaying) setIsPlaying(true);
  }

  function setCurrIndex(idx) {
    setVideoIndex(idx);
  }

  function handlePlay() {
    setIsPlaying(!isPlaying);
  }

  function VimeoDialog(myprops) {
    //
    return (
      <Dialog
        open={props.show}
        onClose={handleCloseDialog}
        maxWidth={props.width}
        aria-labelledby="form-dialog-title"
      >
        <MuiDialogTitle disableTypography className={classes.root}>
          {!props.noTitle ? (
            <React.Fragment>
              <span style={{ fontSize: 18 }}>
                <FormattedMessage
                  id="Help_Videos"
                  defaultMessage="Help Videos"
                />{" "}
                <LiveTvIcon style={{ fontSize: 22 }} />
              </span>
              <span style={{ fontSize: 22 }}>
                <FormattedMessage
                  id="we_recommend_that_you_watch"
                  defaultMessage="We recommend that you watch a short video before you begin this
              part of the application"
                />
              </span>
            </React.Fragment>
          ) : (
            ""
          )}
          <IconButton
            aria-label="close"
            className={classes.closeButton}
            onClick={handleCloseDialog}
            id="closeButton"
          >
            <CloseIcon />
          </IconButton>
        </MuiDialogTitle>

        <DialogContent>
          <div
            style={{
              margin: "0 auto",
              width: props.width + "px",
              height: props.height + "px"
            }}
          >
            {myprops.children}
          </div>
        </DialogContent>
      </Dialog>
    );
  }
  function VideoListTable() {
    return (
      <Table className={classes.table} aria-label="simple table">
        <TableBody>
          {videoList &&
            videoList.map((vid, indx) => (
              <TableRow
                hover
                className={
                  videoIndex == indx ? classes.tableRowActive : classes.tableRow
                }
                key={indx}
                onClick={() => {
                  setCurrIndex(indx);
                }}
              >
                <TableCell className={classes.tableCell}>
                  <Grid container style={{ marginTop: "5px" }}>
                    <Grid item xs={5}>
                      <img
                        className={classes.image}
                        src={vid.thumbnail}
                        alt="vimeo"
                      />
                    </Grid>
                    <Grid item xs={7}>
                      <span className={classes.span}>{vid.title}</span>
                    </Grid>
                    <Grid item xs={1}>
                      <span className={classes.spanTime}>
                        {vid.duration.length > 3
                          ? vid.duration
                          : vid.duration.replace(":", ":0")}
                      </span>
                    </Grid>
                  </Grid>
                </TableCell>
              </TableRow>
            ))}
        </TableBody>
      </Table>
    );
  }
  return (
    <React.Fragment>
      {props.isPopup ? (
        <VimeoDialog>
          {props.showList ? (
            <React.Fragment>
              <Vimeo
                className={props.showList ? classes.playerList : classes.player}
                video={
                  props.videos[videoIndex] ? props.videos[videoIndex].url : null
                }
                autoplay={props.autoplay}
                responsive
                onEnd={Next}
                width={(props.width / 4) * 3}
                height={props.height}
                muted={props.muted}
                paused={!isPlaying}
                onPlay={onPlay}
              />
              <div className={classes.videoListDialog}>
                <VideoListTable />
              </div>
            </React.Fragment>
          ) : (
            <Vimeo
              className={props.showList ? classes.playerList : classes.player}
              video={props.videos[videoIndex].url}
              responsive
              autoplay={props.autoplay}
              paused={!isPlaying}
              muted={props.muted}
              onEnd={Next}
              onPlay={onPlay}
              width={props.width}
              height={props.height}
            />
          )}
        </VimeoDialog>
      ) : (
        <React.Fragment>
          {props.showList ? (
            <React.Fragment>
              <div
                style={{
                  height: props.height + "px",
                  overflowY: "auto",
                  overflowX: "hidden",
                  float: "right",
                  width: "30%",
                  maxHeight: props.height + "px"
                }}
              >
                <VideoListTable />
              </div>

              <Vimeo
                className={props.showList ? classes.playerList : classes.player}
                video={props.videos[videoIndex].url}
                autoplay={props.autoplay}
                muted={props.muted}
                responsive
                onPlay={onPlay}
                onEnd={Next}
                paused={!isPlaying}
                width={(props.width / 4) * 3 + "px"}
                height={props.height + "px"}
              />
            </React.Fragment>
          ) : (
            <Vimeo
              className={props.showList ? classes.playerList : classes.player}
              video={props.videos[videoIndex].url}
              autoplay={props.autoplay}
              muted={props.muted}
              responsive
              controls={!props.hasPlaybar}
              onPlay={onPlay}
              paused={!isPlaying}
              onEnd={Next}
              width={props.width}
              height={props.height}
            />
          )}
          {props.hasPlaybar ? (
            <div className={classes.playbar}>
              {!isPlaying ? (
                <PlayCircleOutlineIcon
                  style={{ marginTop: 10 }}
                  onClick={handlePlay}
                />
              ) : (
                <StopIcon onClick={handlePlay} style={{ marginTop: 10 }} />
              )}
              {videoList && videoList[0] ? (
                <React.Fragment>
                  <span className={classes.playSpan}>
                    {videoList[0] ? videoList[0].duration : ""}
                  </span>
                  <span className={classes.playSpan}>
                    {videoList[0] ? videoList[0].title : ""}
                  </span>
                </React.Fragment>
              ) : (
                ""
              )}
            </div>
          ) : (
            ""
          )}
        </React.Fragment>
      )}
    </React.Fragment>
  );
}
