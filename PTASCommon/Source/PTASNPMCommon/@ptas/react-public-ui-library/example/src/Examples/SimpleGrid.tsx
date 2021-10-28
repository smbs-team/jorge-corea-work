import React, { useState } from "react";
import { SimpleGrid as Grid } from "@ptas/react-public-ui-library";

const SimpleGrid = () => {
  const [rowData] = useState([
    { make: "Toyota", model: "Celica", price: 35000, bold: true },
    { make: "Ford", model: "Mondeo", price: 32000, bold: false },
    { make: "Porsche", model: "Boxter", price: 72000 }
  ]);
  //TODO: check if changing columns updates the grid
  const columns = [
    { field: "make", headerName: "Marca", highlight: true },
    { field: "model", headerName: "Modelo" },
    { field: "price", headerName: "Precio" }
  ];

  return (
    <div style={{ width: "400px", height: "200px" }}>
      <Grid rows={rowData} columns={columns} />
    </div>
  );
};

export default SimpleGrid;
