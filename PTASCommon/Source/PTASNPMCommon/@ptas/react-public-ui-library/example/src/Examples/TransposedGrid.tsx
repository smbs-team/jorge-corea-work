import React, { useState } from "react";
import {
  TransposedGrid as Grid,
  TransposedGridOrder
} from "@ptas/react-public-ui-library";

interface Car {
  make: string;
  model: string;
  price: number;
  bold?: boolean;
}
const SimpleGrid = () => {
  const [rowData /*setRowData*/] = useState<Car[]>([
    { make: "Toyota", model: "Celica", price: 35000, bold: true },
    { make: "Ford", model: "Mondeo", price: 32000, bold: false },
    { make: "Porsche", model: "Boxter", price: 72000 }
  ]);
  const columns = [
    { field: "make", headerName: "Marca" },
    { field: "model", headerName: "Modelo/Estilo" },
    { field: "price", headerName: "Precio", bold: true }
  ];
  const [order, setOrder] = useState<TransposedGridOrder>("asc");
  const [orderBy, setOrderBy] = useState<string>("price");

  return (
    <div style={{ height: 280, width: "500px" }}>
      <Grid
        rows={rowData}
        columns={columns}
        order={order}
        setOrder={setOrder}
        orderBy={orderBy}
        setOrderBy={setOrderBy}
      />
    </div>
  );
};

export default SimpleGrid;
