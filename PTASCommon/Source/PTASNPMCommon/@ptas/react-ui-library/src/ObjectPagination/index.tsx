// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment } from "react";
import {
  Theme,
  WithStyles,
  createStyles,
  withStyles,
  ButtonBase
} from "@material-ui/core";
import {
  usePagination,
  UsePaginationItem,
  UsePaginationProps
} from "@material-ui/lab/Pagination";
import NavigateBeforeIcon from "@material-ui/icons/NavigateBefore";
import NavigateNextIcon from "@material-ui/icons/NavigateNext";
import clsx from "clsx";
import { GenericWithStyles } from "../common";

interface Props<T> extends WithStyles<typeof useStyles> {
  pages: GenericPage<T>[];
  onClick?: (pageData: GenericPage<T>) => void;
  UsePaginationProps?: UsePaginationProps;
  showStatusCircle?: boolean;
}

interface Page {
  isReady: boolean;
}

export type GenericPage<T> = T & Page;
export type ObjectPaginationProps<T> = GenericWithStyles<Props<T>>;
type PageItem = Page & UsePaginationItem;

const useStyles = (theme: Theme) =>
  createStyles({
    button: {
      width: 31,
      height: 34,
      fontFamily: theme.ptas.typography.bodyFontFamily,
      fontSize: "0.875rem",
      transition:
        "color 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms,background-color 250ms cubic-bezier(0.4, 0, 0.2, 1) 0ms"
    },
    ul: {
      listStyle: "none",
      padding: 0,
      margin: 0,
      display: "flex"
    },
    li: {
      position: "relative",
      border: "1px solid rgba(0, 0, 0, 0.23)",
      borderLeft: "none",
      "&:first-child": {
        borderLeft: "1px solid rgba(0, 0, 0, 0.23)",
        borderTopLeftRadius: 3,
        borderBottomLeftRadius: 3
      },
      "&:last-child": {
        borderTopRightRadius: 3,
        borderBottomRightRadius: 3
      }
    },
    ellipsisLi: {
      border: "none",
      borderRight: "1px solid rgba(0, 0, 0, 0.23)",
      fontSize: "1.4rem !important",
      textAlign: "center",
      userSelect: "none"
    },
    statusCircle: {
      width: 17,
      height: 17,
      position: "absolute",
      borderRadius: "50%",
      left: "6.870px",
      bottom: -22
    }
  });

/**
 * ObjectPagination
 *
 * @param props - Component props
 * @see https://material-ui.com/api/pagination/ - For UsePaginationProps
 * @returns A JSX element
 */
function ObjectPagination<T>(props: Props<T>): JSX.Element {
  const { classes, pages, UsePaginationProps } = props;

  const { items } = usePagination({
    count: pages.length,
    ...UsePaginationProps
  });

  const itemsPlus = [
    ...items.map((item) =>
      item.type === "page"
        ? Object.assign({}, item, pages[item.page - 1])
        : item
    )
  ] as PageItem[];

  return (
    <nav>
      <ul className={classes.ul}>
        {itemsPlus.map((item, index) => {
          let children = null;
          const isEllipsis =
            item.type === "start-ellipsis" || item.type === "end-ellipsis";

          if (isEllipsis) {
            children = "…";
          } else if (item.type === "page") {
            children = (
              <Fragment>
                <ButtonBase
                  className={classes.button}
                  onClick={(event) => {
                    item.onClick(event);
                    props.onClick?.(pages[item.page - 1]);
                  }}
                  style={{
                    backgroundColor: item.selected ? "#169db3" : undefined
                  }}
                  disabled={item.disabled}
                >
                  {item.page}
                </ButtonBase>
                {props.showStatusCircle && (
                  <span
                    className={classes.statusCircle}
                    style={{
                      backgroundColor: item.isReady ? "#72bb53" : "#ff3823"
                    }}
                  />
                )}
              </Fragment>
            );
          } else {
            children = (
              <ButtonBase
                onClick={(event) => {
                  item.onClick(event);
                  props.onClick?.(pages[item.page - 1]);
                }}
                disabled={item.disabled}
                className={classes.button}
              >
                {item.type === "previous" ? (
                  <NavigateBeforeIcon
                    style={{
                      fontSize: 16,
                      opacity: item.disabled ? "0.38" : undefined
                    }}
                  />
                ) : (
                  <NavigateNextIcon
                    style={{
                      fontSize: 16,
                      opacity: item.disabled ? "0.38" : undefined
                    }}
                  />
                )}
              </ButtonBase>
            );
          }

          return (
            <li
              key={index}
              className={
                isEllipsis
                  ? clsx(classes.ellipsisLi, classes.button)
                  : classes.li
              }
            >
              {children}
            </li>
          );
        })}
      </ul>
    </nav>
  );
}

export default withStyles(useStyles, { withTheme: true })(ObjectPagination) as <
  T
>(
  props: ObjectPaginationProps<T>
) => React.ReactElement;
