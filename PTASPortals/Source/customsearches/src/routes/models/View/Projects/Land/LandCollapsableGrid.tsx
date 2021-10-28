/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState } from 'react';
import { Theme, createStyles, makeStyles } from '@material-ui/core/styles';
import Accordion from '@material-ui/core/Accordion';
import AccordionSummary from '@material-ui/core/AccordionSummary';
import AccordionDetails from '@material-ui/core/AccordionDetails';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import { RegressionGrid } from '../../Regression/RegressionGrid';
import { GenericGridRowData, PostProcess, Project, ProjectDataset } from 'services/map.typings';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: '100%',
    },
    accordion: {
      background: '#666666',
    },
    gridWrapper: {
      width: '100%',
      '& > .ag-theme-alpine': {
        padding: '0px 6px',
      },
    },
    heading: {
      color: '#fff',
      fontSize: theme.typography.pxToRem(15),
      fontWeight: theme.typography.fontWeightRegular,
    },
  })
);

interface LandCollapsableGridProps {
  // datasetId: string;
  // postProcessId: string;
  // key: string;
  gridData: GenericGridRowData[];
  datasets: ProjectDataset[];
  project: Project;
  postProcess: PostProcess;
  reloadGrid: string;
  datasetId: string;
}

const LandCollapsableGrid = (props: LandCollapsableGridProps): JSX.Element => {
  const classes = useStyles();
  const [expanded, setExpanded] = useState<boolean>(false);

  return (
    <div className={classes.root}>
      <Accordion expanded={expanded}>
        <AccordionSummary
          onClick={(): void => setExpanded(!expanded)}
          expandIcon={<ExpandMoreIcon />}
          aria-controls="panel1a-content"
          id="panel1a-header"
          className={classes.accordion}
        >
          <Typography className={classes.heading}>Dataset</Typography>
        </AccordionSummary>
        <AccordionDetails>
          {expanded && (
            <div className={classes.gridWrapper}>
              <RegressionGrid
                // hideDatasets
                datasetId={props.datasetId}
                gridData={props.gridData}
                reloadGrid={props.reloadGrid}
                project={props.project}
                datasets={props.datasets}
                postProcess={props.postProcess}
                edit
              />
            </div>
          )}
        </AccordionDetails>
      </Accordion>
    </div>
  );
};

export default LandCollapsableGrid;
