import { Button } from "@material-ui/core";
import React, { Fragment, useRef, useState } from "react";
import {
  CommAgGridElement,
  CommercialAgGrid as Grid
} from "@ptas/react-ui-library";

interface TestRowData {
  make: string;
  model: string;
  price: string;
}

function CommercialAgGrid() {
  const childRef = useRef<CommAgGridElement<TestRowData>>(null);

  const [data, setData] = useState<TestRowData[]>([]);

  // const emptyRow: TestRowData = {
  //   make: "",
  //   model: "",
  //   price: ""
  // };

  return (
    <Fragment>
      <Button
        style={{ marginBottom: 50 }}
        onClick={() =>
          setData([
            {
              make: "Toyota Toyota Toyota Toyota",
              model: "Celica",
              price: "35000"
            },
            { make: "Ford", model: "Mondeo", price: "32000" },
            { make: "Porsche", model: "Boxter", price: "72000" }
          ])
        }
      >
        Populate
      </Button>
      <Button
        onClick={() => childRef.current?.updateCell("0", "model", "MIGUEL")}
      >
        Change cell
      </Button>
      <div style={{ width: 500, margin: 30 }}>
        <Grid<TestRowData>
          ref={childRef}
          showRemove
          rowData={data}
          columnDefs={[
            { field: "make" },
            { field: "model" },
            { field: "price" }
          ]}
          onRemove={(row) => console.log("row to remove: ", row)}
        />
      </div>
    </Fragment>
  );
}

export default CommercialAgGrid;
