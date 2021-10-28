import { Fragment, useState } from "react";
import { GenericRow, LazyTree as Lazy } from "@ptas/react-ui-library";
import React from "react";

interface MyProps {
  mono: string;
}

const LazyTree = () => {
  const URL = "https://js.devexpress.com/Demos/Mvc/api/treeListData";
  const [data, setData] = useState<GenericRow<MyProps>[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const onLoad = (ids: React.ReactText[]): void => {
    setIsLoading(true);
    Promise.all(
      ids.map((rowId) =>
        fetch(`${URL}?parentIds=${rowId}`, { mode: "cors" }).then((response) =>
          response.json()
        )
      )
    )
      .then((loadedData) => {
        setData(data.concat(...loadedData));
        setIsLoading(false);
      })
      .catch(() => setIsLoading(false));
  };

  return (
    <div style={{ width: 300, height: 600, marginLeft: 25 }}>
      <Lazy<MyProps>
        onSelect={(row) => console.log("Selected row: ", row)}
        onDataLoad={onLoad}
        isLoading={isLoading}
        rows={data}
      />
    </div>
  );
}

export default LazyTree;
