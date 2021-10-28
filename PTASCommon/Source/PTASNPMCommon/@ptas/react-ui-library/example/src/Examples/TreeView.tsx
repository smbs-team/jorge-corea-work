import React, { useState } from "react";
import { TreeView as Tree } from "@ptas/react-ui-library";
import { OptionsMenu } from "@ptas/react-ui-library";
import SentimentVerySatisfiedIcon from "@material-ui/icons/SentimentVerySatisfied";
import TouchAppIcon from "@material-ui/icons/TouchApp";

const TreeView = () => {
  const columns = [
    { name: "org", title: "Org" },
    { name: "name", title: "Name" },
    { name: "menu", title: "..." },
    { name: "title", title: "Title" }
  ];

  interface ExampleModel {
    id: number;
    parent: number | string | null;
    org: string;
    name: string;
    title: string;
    linkTo?: string;
  }

  const columnW = [{ columnName: "name", width: 210 }];

  const data = [
    {
      id: 1,
      name: "Name 1 Name 1 Name 1 Name 1 Name 1 Name 1 Name 1",
      title: "WLDR",
      org: "My maps",
      parent: null,
      linkTo: "myLink"
    },
    {
      id: 2,
      name: "Name 2",
      title: "XGTH",
      org: "My maps",
      parent: 1
    },
    {
      id: 3,
      name: "Name 3",
      title: "TEXC",
      org: "My maps",
      parent: 2
    }
  ];

  const [isLoading, setIsLoading] = useState<boolean>(false);

  return (
    <div style={{ width: "70%" }}>
      <Tree<ExampleModel>
        onNameClick={(r) => console.log(r)}
        isLoading={isLoading}
        groupFolderIcon={
          <SentimentVerySatisfiedIcon
            style={{
              marginRight: 4,
              fontSize: 24,
              verticalAlign: "top"
            }}
          />
        }
        virtual
        hideEye
        columns={columns}
        groupBy='org'
        rows={data}
        displayGroupInColumn='name'
        tableColumnExtensions={columnW}
        renderMenu={(row): JSX.Element => (
          <OptionsMenu<ExampleModel>
            row={row}
            onItemClick={(action, row): void => console.log(row)}
            items={[
              { id: 1, label: "Edit" },
              { id: 2, label: "Delete" },
              { id: 3, label: "Rename" },
              { id: 4, label: "Configuration" }
            ]}
          ></OptionsMenu>
        )}
      />
      <button onClick={() => setIsLoading(!isLoading)}>Is loading</button>
    </div>
  );
};

export default TreeView;
