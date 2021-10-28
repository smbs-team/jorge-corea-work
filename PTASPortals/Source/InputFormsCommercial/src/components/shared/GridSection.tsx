// GridSection.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { createStyles, Theme, withStyles, WithStyles } from '@material-ui/core';
import {
  IconToolBar,
  IconToolBarItem,
  SectionContainer,
} from '@ptas/react-ui-library';
import { SectionContainerProps } from '@ptas/react-ui-library/dist/SectionContainer';
import { PropsWithChildren } from 'react';

interface Props
  extends WithStyles<typeof useStyles>,
    Omit<SectionContainerProps, 'classes'> {
  icons?: IconToolBarItem[];
}

const useStyles = (theme: Theme) =>
  createStyles({
    sectionWrapper: {
      display: 'flex',
      width: 1000,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.body.fontSize,
      minWidth: 907,
    },
    sectionContentWrapper: {
      display: 'flex',
      minHeight: 50,
      flexDirection: 'column',
      overflow: 'hidden',
    },
    iconToolBar: {
      marginBottom: 16,
    },
    header: {},
    title: {}
  });

function GridSection(props: PropsWithChildren<Props>): JSX.Element {
  const { classes } = props;

  return (
    <div className={classes.sectionWrapper}>
      <SectionContainer
        title={props.title}
        details={props.details}
        isLoading={props.isLoading}
        miscContent={props.miscContent}
        classes={{ header: classes.header, title: classes.title }}
      >
        {props.icons && (
          <IconToolBar
            icons={props.icons}
            classes={{ root: classes.iconToolBar }}
          />
        )}
        <div className={classes.sectionContentWrapper}>{props.children}</div>
      </SectionContainer>
    </div>
  );
}

export default withStyles(useStyles)(GridSection);
