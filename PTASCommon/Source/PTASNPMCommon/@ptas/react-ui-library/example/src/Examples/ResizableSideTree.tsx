import React, { Fragment, useState } from "react";
import { ResizableSideTree as Tree, SideTreeRow } from "@ptas/react-ui-library";

const ResizableSideTree = () => {
  const [testRows, setTestRows] = useState<SideTreeRow[]>([
    {
      id: 0,
      parentId: null,
      name: "Animales",
      isCheckable: true,
      isSelected: false,
      isChecked: false,
      disableCheckBox: true
    },
    {
      id: 1,
      parentId: 0,
      name: "Caninos",
      isCheckable: true,
      isSelected: false,
      isChecked: false
    },
    {
      id: 2,
      parentId: 0,
      name: "Felinos",
      isCheckable: true,
      isSelected: false,
      isChecked: false
    },
    {
      id: 3,
      parentId: 1,
      name: "Perro",
      isCheckable: false,
      isSelected: false,
      isChecked: false,
      shapeType: "pill",
      fillColor: "rgb(255, 0, 0)",
      outlineColor: "rgb(0, 0, 0)"
    },
    {
      id: 4,
      parentId: 1,
      name: "Cuadrado",
      isCheckable: false,
      isSelected: false,
      isChecked: false,
      shapeType: "square",
      fillColor: "red",
      outlineColor: "blue"
    },
    {
      id: 5,
      parentId: 1,
      name: "Pildora 2",
      isCheckable: false,
      isSelected: false,
      isChecked: false,
      shapeType: "pill",
      fillColor: "red",
      outlineColor: "rgba(255, 0, 0, 0)"
    },
    {
      id: 6,
      parentId: 1,
      name: "Pildora 3",
      isCheckable: false,
      isSelected: false,
      isChecked: false,
      shapeType: "pill",
      fillColor: "red",
      outlineColor: "rgba(255, 0, 0, 0)"
    },
    {
      id: 7,
      parentId: 1,
      name: "Pildora 4",
      isCheckable: false,
      isSelected: false,
      isChecked: false,
      shapeType: "pill",
      fillColor: "red",
      outlineColor: "rgba(255, 0, 0, 0)"
    },
    {
      id: 8,
      parentId: 1,
      name: "Pildora 5",
      isCheckable: false,
      isSelected: false,
      isChecked: false,
      shapeType: "pill",
      fillColor: "red",
      outlineColor: "rgba(255, 0, 0, 0)"
    },
  ]);

  const addRow = (): void => {
    setTestRows((r) => [
      ...r,
      {
        id: 5,
        parentId: 0,
        name: "Secreto",
        isCheckable: false,
        isSelected: false,
        isChecked: false
      }
    ]);
  };

  return (
    <Fragment>
      <div style={{ height: "100vh", display: "flex" }}>
        <Tree rows={testRows} onSelected={(r) => console.log(r)} />
        <button
          onClick={(): void => addRow()}
          style={{ height: 50, width: 100 }}
        >
          Add row
        </button>
      </div>
    </Fragment>
  );
};

export default ResizableSideTree;
