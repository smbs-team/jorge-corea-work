// AreaTree.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { LazyTree } from '@ptas/react-ui-library';
import useAreaTreeStore from 'stores/useAreaTreeStore';
import usePageManagerStore from 'stores/usePageManagerStore';

function AreaTree(): JSX.Element {
  const store = useAreaTreeStore();
  const pageManagerStore = usePageManagerStore();

  return (
    <LazyTree
      onSelect={(row) => {
        store.setSelectedItem(row);
        pageManagerStore.reset();
      }}
      onDataLoad={store.getData}
      isLoading={store.isLoading}
      rows={store.data}
      expandedRowIds={store.expandedRowIds}
      onExpandedRowIdsChange={store.setExpandedRowIds}
    />
  );
}

export default AreaTree;
