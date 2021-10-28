import React, { useState } from "react";
import { SectionContainer as Section } from "@ptas/react-ui-library";
import { Button } from "@material-ui/core";

const SectionContainer = () => {
  const [isLoading, setIsLoading] = useState<boolean>(false);

  return (
    <Section title='Data source' details='Last updated by...' isLoading={isLoading}>
      <div
        style={{
          height: "500px",
          backgroundColor: "antiquewhite"
        }}
      ><Button onClick={() => setIsLoading(!isLoading)}>{isLoading ? "No more loading" : "Start Loading"}</Button></div>
    </Section>
  );
};

export default SectionContainer;
