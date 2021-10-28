import React from "react";
import { ListObject, ListSelect as List } from "@ptas/react-ui-library";

const ListSelect = () => {
  const objBuilder = (
    numberToGenerate: number
  ): ListObject<{ isNew: boolean }>[] => {
    let total: number = 0;
    const list: ListObject<{ isNew: boolean }>[] = [];

    while (total < numberToGenerate) {
      total++;
      list.push({ id: total, name: `Object ${total}`, isNew: true });
    }

    return list;
  };

  return (
    <List
      onCancel={() => console.log("canceled!")}
      inputList={[
        {
          id: "x",
          name: `Prueba Prueba Prueba Prueba Prueba Prueba Prueba Prueba Prueba`,
          isNew: true
        },
        ...objBuilder(300)
      ]}
      onDone={(selectedItems) => console.log("selectedItems", selectedItems)}
      selectedItems={[]}
    />
  );
};

export default ListSelect;
