import React, { Fragment } from "react";
import SimpleGrid from "./SimpleGrid";
import TransposedGrid from "./TransposedGrid";

const Grids = () => {
  return (
    <Fragment>
      <div key={1} style={{ margin: "16px 0" }}>
        Simple Grid
      </div>
      <SimpleGrid key={2} />
      <div key={3} style={{ margin: "16px 0" }}>
        Transposed Grid
      </div>
      <TransposedGrid key={4} />
    </Fragment>
  );
};

export default Grids;
