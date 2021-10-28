import React from "react";
import { ListSearch as List } from "@ptas/react-ui-library";

const data = [
  { Key: "Hola", Value: 1 },
  { Key: "Mascara", Value: 2 }
];

//It filters by key.
const ListSearch = () => {
  return <List data={data} TextFieldProps={{ label: "Custom text" }} />;
};

export default ListSearch;
