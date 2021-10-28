// PageName.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  createStyles,
  Tooltip,
  withStyles,
  WithStyles,
} from '@material-ui/core';
import { useEffect } from 'react';
import { Fragment, useRef, useState } from 'react';
import useContentHeaderStore from 'stores/useContentHeaderStore';

interface Props extends WithStyles<typeof useStyles> {
  onChange?: (text: string) => void;
  defaultValue?: string;
  disableToolTip?: boolean;
}

const useStyles = createStyles({
  root: {
    textOverflow: 'ellipsis',
    overflow: 'hidden',
    whiteSpace: 'nowrap',
  },
  tooltip: {
    fontSize: '1rem',
  },
});

function PageName(props: Props): JSX.Element {
  const { classes } = props;
  const { hidePagination } = useContentHeaderStore();
  const nameEle = useRef<HTMLDivElement>(null);
  const [value, setValue] = useState<string>(props.defaultValue ?? '');

  const handleValueChange = () => {
    if (!nameEle.current) return;
    const newValue = nameEle.current.textContent;
    if (newValue) {
      setValue(newValue);
      props.onChange && props.onChange(newValue);
    } else {
      nameEle.current.textContent = value;
    }

    nameEle.current.contentEditable = 'false';
    nameEle.current.scrollLeft = 0;
    nameEle.current.blur();
    nameEle.current.style.cursor = 'inherit';
  };

  const handleOnClick = (element: HTMLDivElement) => {
    if (!nameEle.current) return;
    element.contentEditable = 'true';
    setTimeout(function() {
      if (document.activeElement !== element) {
        element.contentEditable = 'false';
        element.style.cursor = 'inherit';
      }
    }, 300);
  };

  useEffect(() => {
    if (!props.defaultValue || !nameEle.current) return;
    setValue(props.defaultValue);
    nameEle.current.textContent = props.defaultValue;
  }, [props.defaultValue]);

  return (
    <Fragment>
      {!hidePagination && (
        <Tooltip
          title={value}
          enterDelay={800}
          classes={{ tooltip: classes.tooltip }}
          arrow
          disableHoverListener={props.disableToolTip ?? false}
          disableFocusListener={props.disableToolTip ?? false}
          disableTouchListener={props.disableToolTip ?? false}
        >
          <div
            ref={nameEle}
            className={classes.root}
            suppressContentEditableWarning
            onBlur={handleValueChange}
            onClick={e => handleOnClick(e.currentTarget)}
            onFocus={() => document.execCommand('selectAll', false, undefined)}
            onDoubleClick={e => {
              e.currentTarget.style.cursor = 'text';
            }}
            onKeyDown={e => {
              if (e.key === 'Enter') {
                e.preventDefault();
                nameEle.current?.blur();
              }
              if (e.key === 'Escape') {
                if (nameEle.current) {
                  nameEle.current.textContent = value;
                  nameEle.current.blur();
                  e.stopPropagation();
                }
              }
            }}
          >
            {value}
          </div>
        </Tooltip>
      )}
    </Fragment>
  );
}

export default withStyles(useStyles)(PageName);
