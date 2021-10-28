import React, { Fragment } from "react";
import { Save as SaveBox, SaveAcceptEvent } from "@ptas/react-ui-library";
import { ListItem } from "../../../dist/ListSearch";

const categoriesData: ListItem[] = [
  {
    Key: "cat1",
    Value: 1
  },
  {
    Key: "cat2",
    Value: 2
  }
];

const Save = () => {
  return (
    <Fragment>
      <div style={{ maxWidth: 500 }}>
        <SaveBox
          useCategories={true}
          categoriesData={categoriesData}
          isNewSave
          title='Save renderer'
          okClick={(e: SaveAcceptEvent): void => {
            console.log("okClick");
          }}
          buttonText='Save'
          popoverTitle='New Category'
          checkboxText='Lock'
          dropdownRows={[]}
          newFolderDropdownRows={[]}
          newFolderOkClick={() => console.log("newFolderOkClick")}
          defaultRoute={"/User"}
          defaultName={""}
          defaultChecked={false}
        ></SaveBox>
      </div>

      {/* <div style={{ maxWidth: 500 }}>
        <SaveBox
          isNewSave
          title='Save renderer'
          okClick={(e: SaveAcceptEvent): void => {
            console.log("okClick");
          }}
          buttonText='Save'
          popoverTitle='New Folder'
          cancelClick={() => {}}
          checkboxText='Lock'
          dropdownRows={[]}
          newFolderDropdownRows={[]}
          newFolderOkClick={() => console.log("newFolderOkClick")}
          defaultRoute={"/User"}
          defaultName={""}
          defaultChecked={false}
        ></SaveBox>
      </div> */}
    </Fragment>
  );
};

export default Save;
