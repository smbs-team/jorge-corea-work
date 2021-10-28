import { Button, makeStyles, Popover } from "@material-ui/core";
import React, { Fragment, useState } from "react";
import CustomPopover from "@ptas/react-ui-library";
import { CommAlert as CAlert } from "@ptas/react-ui-library";

const useStyles = makeStyles({
  root: {
    borderRadius: 12
  }
});

function CommAlert() {
  const classes = useStyles();

  const [anchorEl, setAnchorEl] = useState<HTMLButtonElement | null>(null);

  const handleOnClick = (event: any) => {
    setAnchorEl(event.currentTarget);
  };

  return (
    <Fragment>
      <Button onClick={handleOnClick}>Open!</Button>
      <Popover
        open={anchorEl != null}
        anchorEl={anchorEl}
        onClose={() => {
          setAnchorEl(null);
        }}
        classes={{ paper: classes.root }}
      >
        <CAlert
          title='Error!'
          content="Page 8 has not been marked 'ready for valuation'"
          onButtonClick={() => {
            setAnchorEl(null);
          }}
        />
      </Popover>
    </Fragment>
  );
}

export default CommAlert;
