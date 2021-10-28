// ErrorBuilder.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from "react";
import { InvalidRow } from "./index";

function ErrorBuilder(props: { errors: InvalidRow[] }): JSX.Element {
  return (
    <div style={{ fontSize: "1.2rem" }}>
      <ul style={{ listStyle: "none", color: "red" }}>
        {props.errors.map((e) => (
          <li>{`Row ${(e.index as number) + 1} - ${e.errorMessage}`}</li>
        ))}
      </ul>
    </div>
  );
}

export default ErrorBuilder;
