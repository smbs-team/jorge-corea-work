import React, { Fragment, useState } from "react";
import { ModalGrid as Grid } from "@ptas/react-ui-library";

const ModalGrid = () => {
  const [rowData] = useState([
    { make: "Toyota", model: "Celica", price: 35000 },
    { make: "Ford", model: "Mondeo", price: 32000 },
    { make: "Porsche", model: "Boxter", price: 72000 }
  ]);

  const columns = ["make", "model", "price"];
  const [open, setOpen] = useState<boolean>(false);
  return (
    <Fragment>
      {/* <Grid
        rows={rowData}
        columns={columns}
        isOpen={open}
        onClose={() => setOpen(false)}
        onButtonClick={(rows) => console.log(rows)}
      /> */}
      <button onClick={() => setOpen(true)} style={{ margin: 30 }}>
        Open modal
      </button>
    </Fragment>
  );
};

export default ModalGrid;
