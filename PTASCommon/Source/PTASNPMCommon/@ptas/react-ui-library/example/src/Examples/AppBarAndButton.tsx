import React, { Fragment } from "react";
import { CustomAppBar, MenuButton, PanelHeader } from "@ptas/react-ui-library";
import { AppBar, makeStyles, ThemeProvider, Toolbar } from "@material-ui/core";
import AssignmentLateIcon from "@material-ui/icons/AssignmentLate";
import FavoriteBorderIcon from "@material-ui/icons/FavoriteBorder";
import GitHubIcon from "@material-ui/icons/GitHub";

const useStyles = makeStyles({
    panelHeader: {
      position: "sticky",
      top: 48,
    },
    toolbar: {
      marginLeft: -14
    }, 
    title: {
      marginLeft: 2
    }
  });

function AppBarAndButton() {
  const classes = useStyles();

  const myIcons = [
    { icon: <AssignmentLateIcon />, text: "Save", onClick: () => console.log("save")},
    { icon: <FavoriteBorderIcon />, text: "Favorite", disabled: true },
    { icon: <GitHubIcon />, text: "Git" }
  ];

  return (
      <Fragment>
      <CustomAppBar classes={{toolbar: classes.toolbar}}>
        <MenuButton
          content="Hola"
          onClick={() => console.log("redirect?")}
        />
        <MenuButton
          content="Hola"
          onClick={() => console.log("redirect?")}
        />
        <MenuButton
          content="Hola"
          onClick={() => console.log("redirect?")}
        />
        <MenuButton
          content="Hola"
          onClick={() => console.log("redirect?")}
        />
        <MenuButton
          content="Hola"
          onClick={() => console.log("redirect?")}
        />
        <MenuButton
          content="Hola"
          onClick={() => console.log("redirect?")}
        />
        <MenuButton
          content="Hola"
          onClick={() => console.log("redirect?")}
        />
        <MenuButton
          content="Options"
          menuItems={[
            {
              content: "dos",
              onItemClick: () => console.log("clic"),
              disabled: true,
            },
            {
              content: "uno",
              children: [
                { content: "Hijo" },
                { content: "otro hijo", children: [{ content: "Más" }] },
              ],
            },
          ]}
        />
        <MenuButton
          content={<GitHubIcon />}
          ButtonProps={{style: {marginLeft: "auto"}}}
        />
      </CustomAppBar>
      <PanelHeader 
        icons={myIcons}
        title="Chocolate"
        classes={{root: classes.panelHeader, title: classes.title}}
        // detailTop="Los detalles de arriba"
        // detailBottom= "Los detalles de abajo"
      />
      <div style={{height: 1000, backgroundColor: "antiquewhite"}}></div>
      <div style={{height: 1000, backgroundColor: "salmon"}}></div>
      <div style={{height: 1000, backgroundColor: "darkseagreen"}}></div>
      </Fragment>

  );
}

export default AppBarAndButton;
