// NotFound.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useEffectOnce } from "react-use";
import useSubToolbarStore from "stores/useSubToolbarStore";

function NotFound(): JSX.Element {
  const subToolBarStore = useSubToolbarStore();

  useEffectOnce(() => {
    subToolBarStore.setTitle(``);
  });

  return <h1>Not Found</h1>;
}

export default NotFound;
