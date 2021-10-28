// PageDetails.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles, Tooltip } from '@material-ui/core';
import React, { useEffect, useState } from 'react';
import useAreaTreeStore, { getRootFriendlyName } from 'stores/useAreaTreeStore';
import usePageManagerStore from 'stores/usePageManagerStore';
import PageName from './PageName';

const useStyles = makeStyles(theme => ({
  root: {
    display: 'flex',
    position: 'sticky',
    borderBottom: '1px solid #c0c0c0',
    paddingTop: 4,
    paddingBottom: 4,
    zIndex: 2,
    backgroundColor: 'white',
    top: 128,
    minWidth: 1492,
    alignItems: 'baseline',
    height: 32,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: '1rem',
    whiteSpace: 'nowrap',
  },
  areaNbhdContainer: {
    maxWidth: 685,
    display: 'flex',
    '@media (max-width:1919px)': {
      maxWidth: 525,
    },
    '@media (min-width:1921px)': {
      maxWidth: 910,
    },
  },
  labelValue: {
    fontWeight: 'bold',
  },
  separator: {
    color: 'lightgray',
    marginLeft: 8,
    marginRight: 8,
    borderLeft: '1px solid',
  },
  pageName: {
    position: 'absolute',
    left: '37%',
    fontWeight: 'bold',
    width: 545,
    textAlign: 'center',
  },
  labelKey: {
    marginRight: 4,
  },
  details: {
    overflow: 'hidden',
    textTransform: 'none',
    textOverflow: 'ellipsis',
    whiteSpace: 'nowrap',
  },
  tooltip: {
    fontSize: '1rem',
  },
}));

function PageDetails(): JSX.Element {
  const classes = useStyles();
  const areaTreeStore = useAreaTreeStore();
  const pageManagerStore = usePageManagerStore();
  const [details, setDetails] = useState<React.ReactNode>();

  const setName = (text: string) => {
    pageManagerStore.setPageName(text);
  };

  useEffect(() => {
    setDetails(
      <label>
        <strong>{`${getRootFriendlyName(
          areaTreeStore.selectedItem?.area?.entityName
        )} `}</strong>
        {`${
          areaTreeStore.selectedItem
            ? `${areaTreeStore.selectedItem.area?.ptasName} - ${areaTreeStore.selectedItem.area?.description}`
            : 'X - Area'
        }`}
        <span className={classes.separator}></span>
        <strong>Neighborhood: </strong>
        {`${
          areaTreeStore.selectedItem
            ? areaTreeStore.selectedItem.nbhdDescription
            : 'X - Neighborhood'
        }`}
      </label>
    );
  }, [areaTreeStore.selectedItem, classes.separator]);

  return (
    <div className={classes.root}>
      <div className={classes.areaNbhdContainer}>
        <Tooltip
          title={details as string}
          enterDelay={800}
          classes={{ tooltip: classes.tooltip }}
          arrow
        >
          <label className={classes.details}>{details as string}</label>
        </Tooltip>
      </div>
      <PageName
        onChange={setName}
        classes={{ root: classes.pageName }}
        defaultValue={pageManagerStore.pageName}
      />
    </div>
  );
}

export default PageDetails;
