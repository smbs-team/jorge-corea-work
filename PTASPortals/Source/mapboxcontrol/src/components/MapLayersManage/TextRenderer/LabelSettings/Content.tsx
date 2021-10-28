// Content.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import 'codemirror/lib/codemirror.css';
import 'codemirror/theme/neo.css';
import 'codemirror/mode/javascript/javascript.js';
import { UnControlled as CodeMirror } from 'react-codemirror2';
import React, { useContext, useEffect, useRef, useState } from 'react';
import {
  DropDownItem,
  IconToolBar,
  SimpleDropDown,
} from '@ptas/react-ui-library';
import { useDebounce } from 'react-use';
import { createStyles, useTheme, Theme, makeStyles } from '@material-ui/core';
import { PlayForWork, VerticalAlignBottom } from '@material-ui/icons';
import { TagsManageContext } from '../TagsManageContext';
import { PropertyItem } from '../typing';
interface DocObject {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  getCursor: () => any;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  replaceRange: (text: string, position: any) => void;
}

interface EditorObject {
  getDoc: () => DocObject;
}

interface Props {
  allProperties: PropertyItem[];
}

const dataFilter = [
  {
    label: 'All',
    value: 'all',
  },
  {
    label: 'Numerical',
    value: 'number',
  },
  {
    label: 'String',
    value: 'string',
  },
  {
    label: 'Date',
    value: 'date',
  },
];

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: '800px',
    },
    rowContainer: {
      display: 'flex',
    },
    dropdown: {
      marginRight: theme.spacing(2),
      marginBottom: theme.spacing(2),
      width: '250px',
    },
    textAreaContent: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: theme.ptas.typography.formField.fontSize,
      lineHeight: theme.ptas.typography.lineHeight,
      width: '600px',
      border: '1px solid #000',
      borderRadius: '5px',
      padding: '8px',
    },
    btnContainer: {
      paddingTop: '6px',
    },
  })
);

function Content(props: Props): JSX.Element {
  const { allProperties } = props;
  const classes = useStyles(useTheme());
  const contentEditorRef = useRef<EditorObject>();
  const { selectedLabel, setSelectedLabel } = useContext(TagsManageContext);
  const [content, setContent] = useState<string>('');
  const [propertiesFiltered, setPropertiesFiltered] = useState<DropDownItem[]>(
    []
  );
  const [propertySel, setPropertySel] = useState<DropDownItem>();
  const [filterSelected, setFilterSelected] = useState<DropDownItem>(
    dataFilter[0]
  );

  useEffect(() => {
    if (selectedLabel) {
      const { content: contentLabel } = selectedLabel;
      setContent(contentLabel ?? '');
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedLabel?.id]);

  useEffect(() => {
    if (!filterSelected) return;

    const filtered =
      filterSelected.value === 'all'
        ? allProperties
        : allProperties.filter((p) => p.type === filterSelected.value);
    const propertiesFound = filtered.map((p) => ({
      label: p.name,
      value: p.name,
    }));
    setPropertiesFiltered(propertiesFound);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [filterSelected, allProperties]);

  useDebounce(
    (): void => {
      setSelectedLabel((prev) => {
        if (!prev) return;
        return {
          ...prev,
          content,
        };
      });
    },
    100,
    [content]
  );

  const filterChange = (e: DropDownItem): void => {
    setFilterSelected(e);
  };

  const propertyChange = (e: DropDownItem): void => {
    setPropertySel(e);
  };

  const contentChange = (
    editor: EditorObject,
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    data: any,
    value: string
  ): void => {
    contentEditorRef.current = editor;
    setContent(value);
  };

  const insertOnContent = (textToInsert: string): void => {
    if (contentEditorRef.current) {
      const doc = contentEditorRef.current.getDoc();
      const cursor = doc.getCursor();
      doc.replaceRange(textToInsert, cursor);
    }
  };

  const insertVariableClick = (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ): void => {
    if (propertySel && propertySel.value) {
      insertOnContent(`{${propertySel.value}}`);
    }
  };

  const insertBreakClick = (
    e: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ): void => {
    insertOnContent('\\n');
  };

  return (
    <div className={classes.root}>
      <div className={classes.rowContainer}>
        <SimpleDropDown
          classes={{
            root: classes.dropdown,
          }}
          label="Filter"
          items={dataFilter}
          onSelected={filterChange}
          value={filterSelected?.value}
        />
        <SimpleDropDown
          classes={{
            root: classes.dropdown,
          }}
          label="Property"
          items={propertiesFiltered}
          onSelected={propertyChange}
          value={propertySel?.value}
        />
        <span className={classes.btnContainer}>
          <IconToolBar
            icons={[
              {
                text: 'Insert variable',
                icon: <PlayForWork />,
                onClick: insertVariableClick,
              },
            ]}
          />
        </span>
      </div>
      <div className={classes.rowContainer}>
        <CodeMirror
          editorDidMount={(editor): void => {
            contentEditorRef.current = editor;
          }}
          value={content}
          className={classes.textAreaContent}
          options={{
            mode: 'javascript',
            theme: 'neo',
          }}
          onChange={contentChange}
          onCursorActivity={(editor): void => {
            contentEditorRef.current = editor;
          }}
          autoCursor={false}
        />
        <span className={classes.btnContainer}>
          <IconToolBar
            icons={[
              {
                text: 'Insert break',
                icon: <VerticalAlignBottom />,
                onClick: insertBreakClick,
              },
            ]}
          />
        </span>
      </div>
    </div>
  );
}

export default Content;
