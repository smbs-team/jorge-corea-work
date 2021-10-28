// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles } from '@material-ui/core';
import { CustomButton, GenericPage } from '@ptas/react-ui-library';
import { useState } from 'react';
import usePageManagerStore, { Page } from 'stores/usePageManagerStore';
import DnDList from './DnDList';

interface Props {
  onSave?: () => void;
  onCancel?: () => void;
}

const useStyles = makeStyles(() => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    minHeight: '35vh',
    maxHeight: '80vh',
    padding: 8,
  },
  buttonContainer: {
    display: 'flex',
    marginTop: 24,
  },
}));

function ViewPages(props: Props): JSX.Element {
  const classes = useStyles();
  const pageManagerStore = usePageManagerStore();
  const [editedPages, setEditedPages] = useState<GenericPage<Page>[]>(
    pageManagerStore.pages
  );

  const handleSave = () => {
    props.onSave && props.onSave();
    pageManagerStore.setPages(editedPages);
    pageManagerStore.refresh();
  };

  return (
    <div className={classes.root}>
      {pageManagerStore.isFilterActive && (
        <label style={{ color: 'red' }}>Filter is active</label>
      )}
      <DnDList pages={pageManagerStore.pages} onChange={setEditedPages} />
      <div className={classes.buttonContainer}>
        <CustomButton
          ptasVariant="commercial"
          style={{ marginLeft: 'auto', marginRight: 16 }}
          onClick={props.onCancel}
        >
          Cancel
        </CustomButton>
        <CustomButton ptasVariant="commercial" onClick={handleSave}>
          Save
        </CustomButton>
      </div>
    </div>
  );
}

export default ViewPages;
