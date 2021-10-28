// DnDList.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ButtonBase, makeStyles } from '@material-ui/core';
import { GenericPage } from '@ptas/react-ui-library';
import { CSSProperties } from 'react';
import {
  DragDropContext,
  Droppable,
  Draggable,
  DraggingStyle,
  NotDraggingStyle,
  DropResult,
} from 'react-beautiful-dnd';
import { useList } from 'react-use';
import { Page } from 'stores/usePageManagerStore';
import { reorder } from 'utils';
import DeleteIcon from '@material-ui/icons/Delete';
import { useEffect } from 'react';
import PageName from '../PageName';

interface Props {
  pages: GenericPage<Page>[];
  onChange?: (pages: GenericPage<Page>[]) => void;
}

type DraggableStyle = DraggingStyle | NotDraggingStyle | undefined;

const useStyles = makeStyles(() => ({
  pageName: {
    flex: 1,
    marginRight: 4,
    whiteSpace: 'unset',
  },
  delete: {
    color: 'lightgray',
    '&:hover': {
      color: 'red',
    },
  },
}));

function DnDList(props: Props): JSX.Element {
  const [pages, pagesActions] = useList(props.pages);
  const classes = useStyles();
  const { onChange } = props;

  const grid = 8;

  const onDragEnd = (result: DropResult) => {
    if (!result.destination) return;
    const reorderedPages = reorder(
      pages,
      result.source.index,
      result.destination.index
    );
    pagesActions.set(reorderedPages);
    props.onChange && props.onChange(reorderedPages);
  };

  const handleOnDelete = (index: number) => {
    pagesActions.removeAt(index);
  };

  const handleOnNameChange = (name: string, page: GenericPage<Page>) => {
    pagesActions.set(
      pages.map(p => (p.id === page.id ? { ...page, pageName: name } : p))
    );
  };

  useEffect(() => {
    onChange && onChange(pages);
  }, [pages, onChange]);

  const getListStyle = (_isDraggingOver: boolean): CSSProperties => ({
    padding: grid,
    width: 300,
    overflow: 'auto',
    flex: 1,
  });

  const getItemStyle = (
    isDragging: boolean,
    draggableStyle: DraggableStyle
  ): CSSProperties => ({
    display: 'flex',
    alignItems: 'center',
    userSelect: 'none',
    padding: grid * 2,
    margin: `0 0 ${grid}px 0`,
    background: isDragging ? '#d4e693' : 'white',
    border: '1px solid black',
    borderRadius: '4px',
    paddingBottom: 8,
    paddingTop: 8,
    ...draggableStyle,
  });

  return (
    <DragDropContext onDragEnd={onDragEnd}>
      <Droppable droppableId="droppable">
        {(provided, snapshot) => (
          <div
            {...provided.droppableProps}
            ref={provided.innerRef}
            style={getListStyle(snapshot.isDraggingOver)}
          >
            {pages.map((page, index) => (
              <Draggable
                key={page.id}
                draggableId={page.id.toString()}
                index={index}
              >
                {(provided, snapshot) => (
                  <div
                    ref={provided.innerRef}
                    {...provided.draggableProps}
                    {...provided.dragHandleProps}
                    style={getItemStyle(
                      snapshot.isDragging,
                      provided.draggableProps.style
                    )}
                  >
                    <PageName
                      defaultValue={page.pageName}
                      classes={{ root: classes.pageName }}
                      onChange={text => handleOnNameChange(text, page)}
                      disableToolTip
                    />
                    <div
                      style={{
                        width: 17,
                        height: 17,
                        backgroundColor: page.isReady ? '#72bb53' : '#ff3823',
                        borderRadius: '50%',
                        marginLeft: 'auto',
                      }}
                    />
                    <div style={{ marginLeft: 6 }}>
                      <ButtonBase
                        className={classes.delete}
                        onClick={() => handleOnDelete(index)}
                        disabled={pages.length === 1}
                        style={{
                          color: snapshot.isDragging ? '#797373' : undefined,
                        }}
                      >
                        <DeleteIcon />
                      </ButtonBase>
                    </div>
                  </div>
                )}
              </Draggable>
            ))}
            {provided.placeholder}
          </div>
        )}
      </Droppable>
    </DragDropContext>
  );
}

export default DnDList;
