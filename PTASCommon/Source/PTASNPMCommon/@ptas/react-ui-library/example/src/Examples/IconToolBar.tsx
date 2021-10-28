import React from "react";
import { IconToolBar as Bar } from "@ptas/react-ui-library";
import AssignmentLateIcon from "@material-ui/icons/AssignmentLate";
import FavoriteBorderIcon from "@material-ui/icons/FavoriteBorder";
import GitHubIcon from "@material-ui/icons/GitHub";

const IconToolBar = () => {
    const myIcons = [
        { icon: <AssignmentLateIcon />, text: "Save" },
        { icon: <FavoriteBorderIcon />, text: "Favorite" },
        { icon: <GitHubIcon />, text: "Git" }
      ];

  return (
    <Bar icons={myIcons}/>
  );
};

export default IconToolBar;
