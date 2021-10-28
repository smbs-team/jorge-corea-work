import React, { useState, useEffect } from "react";
import { makeStyles } from "@material-ui/core/styles";
import VimeoPlayer from "./VimeoPlayer";
import PlayArrowIcon from "@material-ui/icons/PlayArrow";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import "./VimeoPlayer.css";
import TableRow from "@material-ui/core/TableRow";
import * as vimeoHelper from "../../../services/vimeoHelper";

import Grid from "@material-ui/core/Grid";

export default function VimeoSlider(props) {
  const useStyles = makeStyles(theme => ({
    tableRow: {
      cursor: "pointer",
      border: "0px"
    },
    tableCell: {
      padding: "0px",
      border: "0px"
    },
    span: {
      color: "black",
      fontWeight: 600,
      fontSize: 14
    },
    spanTime: {
      color: "black"
    },
    thumbContainer: {
      position: "relative",
      width: "120px",
      height: "89px",
      marginRight: "16px"
    },
    playArrow: {
      position: "absolute",
      left: "25%",
      top: "18%",
      width: "60px",
      height: "60px",
      borderRadius: "30px"
    },
    image: {
      position: "absolute",
      width: "120px",
      height: "auto",
      borderRadius: "10px"
    }
  }));
  const classes = useStyles();
  const [videoList, setvideoList] = useState();

  const [showVimeo, setShowVimeo] = useState(false);
  useEffect(() => {
    const results = props.videos.map(async x => {
      return vimeoHelper.getVimeoData(x.url);
    });
    Promise.all(results).then(completed => {
      setvideoList(completed);
    });
  }, [props.videos]);

  function handleOpenVimeo(vid) {
    setShowVimeo(true);
  }
  function onPopupClose() {
    setShowVimeo(false);
  }

  return (
    <div>
      <Table className={classes.table} aria-label="simple table">
        <TableBody>
          {videoList &&
            videoList.map((vid, indx) => (
              <TableRow
                hover
                className={classes.tableRow}
                key={indx}
                onClick={() => {
                  handleOpenVimeo(vid);
                }}
              >
                <TableCell className={classes.tableCell}>
                  <Grid
                    container
                    style={{ margin: "5px" }}
                    direction="row"
                    justify="flex-start"
                    alignItems="center"
                  >
                    <div className={classes.thumbContainer}>
                      <img
                        className={classes.image}
                        src={vid.thumbnail}
                        alt="vimeo"
                      />
                      <PlayArrowIcon
                        className={classes.playArrow}
                      ></PlayArrowIcon>
                    </div>
                    <Grid item sm>
                      <span className={classes.span}>{vid.title}</span>
                      {" ("}
                      <span className={classes.spanTime}>
                        {vid.duration.length > 3
                          ? vid.duration
                          : vid.duration.replace(":", ":0")}
                      </span>
                      {")"}
                    </Grid>
                  </Grid>
                </TableCell>
              </TableRow>
            ))}
        </TableBody>

        <VimeoPlayer
          width={"1000"}
          height={"430"}
          videos={props.videos}
          isPopup
          //showList
          autoplay
          onPopupClose={onPopupClose}
          //hasPlaybar
          noTitle
          show={showVimeo}
        />
      </Table>
    </div>
  );
}
