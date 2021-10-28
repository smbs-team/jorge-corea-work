// ProjectDetails.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext } from 'react';
import { makeStyles, Box } from '@material-ui/core';
import { CustomTextField } from '@ptas/react-ui-library';
import { ProjectContext } from 'context/ProjectsContext';

const useStyles = makeStyles((theme) => ({
  root: {
    marginTop: theme.spacing(2),
  },
  sectionTitle: {
    paddingLeft: theme.spacing(3),
    fontSize: '1.5rem',
    fontWeight: 'bold',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    borderBottom: '1px solid gray',
    display: 'block',
  },
  divider: {
    marginBottom: theme.spacing(3),
    marginTop: theme.spacing(1),
  },
  commentsTextField: {
    width: 'unset',
    marginLeft: theme.spacing(2),
    marginRight: theme.spacing(2),
  },
  editContent: {
    padding: theme.spacing(2, 3, 2, 3),
  },
}));

interface ProjectDetailsProps {
  versionType?: string;
}

/**
 * ProjectDetails
 *
 * @param props - Component props
 * @returns A JSX element
 */
function ProjectDetails(props: ProjectDetailsProps): JSX.Element {
  const classes = useStyles();
  const context = useContext(ProjectContext);

  return (
    <Box className={classes.root}>
      <div className="ProjectDetail-header">
        <label className={classes.sectionTitle}>{context.projectDetails}</label>
        <label
          className={`ProjectDetail-versionType ${props?.versionType?.toLowerCase()}`}
        >
          {props.versionType}
        </label>
      </div>
      <Box className={classes.editContent}>
        <CustomTextField
          className={classes.commentsTextField}
          autoFocus
          fullWidth
          label="Comments"
          multiline
          rowsMax={4}
          rows={4}
          value={context.comments}
          onChange={(e): void =>
            context.setComments && context.setComments(e.target.value)
          }
          InputProps={{ readOnly: true }}
        />
      </Box>
    </Box>
  );
}

export default ProjectDetails;
