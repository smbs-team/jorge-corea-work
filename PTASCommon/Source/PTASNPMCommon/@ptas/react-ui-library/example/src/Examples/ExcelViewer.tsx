import { Button, createStyles, StyleRules, ThemeProvider, WithStyles, withStyles} from "@material-ui/core";
import React, { Fragment } from "react";
import { ExcelViewer as Viewer } from "@ptas/react-ui-library";
import { CustomModalV2 as Modal2 } from "@ptas/react-ui-library";

interface Props extends WithStyles<typeof useStyles> {
}

const useStyles = () =>
  createStyles({
    root: {
      width: "80%",
      height: "80%",
    },
  });

function ExcelViewer(props: Props) {
  const [isOpen, setIsOpen] = React.useState<boolean>(false);
  const EXCEL_LINK =
    "https://www.cmu.edu/blackboard/files/evaluate/tests-example.xls";

  const handleOnClose = () => {
    setIsOpen(false);
  };

  return (
      <Fragment>
        <Button onClick={() => setIsOpen(true)}>Open Modal</Button>
        <Modal2 open={isOpen} onClose={handleOnClose} classes={{root: props.classes.root}}> 
          <Viewer
            fileUrl={EXCEL_LINK}
            details={[
              { key: "Geo Area:", value: "45 - West Seattle" },
              { key: "Neighborhood:", value: "10 - Admiral District" },
            ]}
            title="Indicated Income Value Review"
            onCancel={() => setIsOpen(false)}
            onSave={() => {console.log("Saved!"); setIsOpen(false)}}
          />
        </Modal2>
      </Fragment>
  );
}

export default withStyles(useStyles)(ExcelViewer);
