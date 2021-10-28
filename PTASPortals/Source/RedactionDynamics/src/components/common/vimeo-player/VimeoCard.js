//-----------------------------------------------------------------------
// <copyright file="VimeoCard.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import React, { useState, useEffect } from "react";
import { makeStyles } from "@material-ui/core/styles";
import VimeoPlayer from "./VimeoPlayer";
import Card from "@material-ui/core/Card";

import CardContent from "@material-ui/core/CardContent";
import { FormattedMessage } from "react-intl";
import "./VimeoPlayer.css";
import TableRow from "@material-ui/core/TableRow";
import * as vimeoHelper from "../../../services/vimeoHelper";
import PlayArrowIcon from "@material-ui/icons/PlayArrow";
import Grid from "@material-ui/core/Grid";

export default function VimeoCard(props) {
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
      fontWeight: "bold",
      fontSize: 18,
      height: "100%",
      display: "flex",
      alignItems: "center"
    },
    spanTime: {
      color: "black",
      height: "100%",
      display: "flex",
      alignItems: "center"
    },
    image: {
      maxWidth: "100%",
      height: "auto"
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
    setShowVimeo(!showVimeo);
  }
  function onPopupClose() {
    if (showVimeo) {
      setShowVimeo(false);
    }
  }

  return (
    <div>
      <Card
        className="card"
        style={{
          cursor: "pointer",
          width: 277,
          height: 160,
          maxHeight: 160,
          border: "1px solid black"
        }}
        onClick={() => {
          handleOpenVimeo();
        }}
      >
        <CardContent>
          <div
            style={{
              fontSize: 16,
              fontWeight: "bold",

              marginLeft: -18,
              marginTop: -18,
              marginBottom: 18
            }}
            className="seniorExemptionTitle text-center"
          >
            <FormattedMessage
              id="senior_exemption_help_videos"
              defaultMessage="Senior Exemption help videos"
            />
          </div>
          <div className="text-center">
            <PlayArrowIcon style={{ fontSize: 80 }} />
          </div>
        </CardContent>
        {showVimeo ? (
          <VimeoPlayer
            width={"1000"}
            height={"430"}
            videos={props.videos}
            isPopup
            showList
            //autoplay
            show={videoList.length > 0}
            onPopupClose={onPopupClose}
            //hasPlaybar
            noTitle
          />
        ) : (
          ""
        )}
      </Card>
    </div>
  );
}
