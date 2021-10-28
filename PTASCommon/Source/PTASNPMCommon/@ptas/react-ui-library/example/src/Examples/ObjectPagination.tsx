import { Fragment, useState } from "react";
import { GenericPage, ObjectPagination as Pag } from "@ptas/react-ui-library";
import React from "react";

interface MyCustomProps {
    id: string;
  }

const ObjectPagination = () => {
    const pages: GenericPage<MyCustomProps>[] = [
        { id: "1", isReady: false },
        { id: "2", isReady: true },
        { id: "3", isReady: false },
        { id: "4", isReady: false },
        { id: "5", isReady: false },
        { id: "6", isReady: false },
        { id: "7", isReady: true },
        { id: "8", isReady: false },
        { id: "9", isReady: false },
        { id: "10", isReady: false },
        { id: "11", isReady: false },
      ];
    
      return (
        <div style={{ marginLeft: 25, marginTop: 50 }}>
          <Pag<MyCustomProps>
            pages={pages}
            onClick={(pageData) => {
              console.log("selected page: ", pageData);
            }}
            showStatusCircle
          />
        </div>
      );
}

export default ObjectPagination;
