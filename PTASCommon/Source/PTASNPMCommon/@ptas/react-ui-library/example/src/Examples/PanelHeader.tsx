import React from "react";
import { ThemeProvider } from "@material-ui/core";
import { ptasCamaTheme, PanelHeader as Panel } from "@ptas/react-ui-library";

import AssignmentLateIcon from "@material-ui/icons/AssignmentLate";
import FavoriteBorderIcon from "@material-ui/icons/FavoriteBorder";
import GitHubIcon from "@material-ui/icons/GitHub";

const PanelHeader = () => {
  const myIcons = [
    { icon: <AssignmentLateIcon />, text: "Save" },
    { icon: <FavoriteBorderIcon />, text: "Favorite" },
    { icon: <GitHubIcon />, text: "Git" }
  ];

  return (
    <ThemeProvider theme={ptasCamaTheme}>
      <Panel
        icons={myIcons}
        route={[<label>Uno</label>]}
        detailTop='Sales: 688   |  Population: 10,280  |  Area(s): 85, 83, 10'
        detailBottom="Last sync on 8/2/2020 at 7:43pm, by Adam Neel"
      />
    </ThemeProvider>
  );
};

export default PanelHeader;
