// Save.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  useState,
  PropsWithChildren,
  useEffect,
  Fragment,
  Key
} from "react";
import {
  withStyles,
  WithStyles,
  Box,
  Typography,
  Switch,
  Theme,
  createStyles
} from "@material-ui/core";
import { AddCircleOutline } from "@material-ui/icons";
import clsx from "clsx";
import CustomTextField from "./../../CustomTextField";
import { CustomButton } from "./../../CustomButton";
import CustomPopover from "./../../CustomPopover";
import ListSearch, { ListItem } from "./../../ListSearch";
import NewFolder, { NewFolderAcceptEvt, NewFolderProps } from "../NewFolder";
import DropDownTree, {
  DropDownTreeProps,
  DropdownTreeRow
} from "../../DropDownTree";
import CustomIconButton from "../../CustomIconButton";
import NewCategory, { NewCategoryAcceptEvt } from "../NewCategory";
import { useMount } from "react-use";

type SaveClassKey =
  | "root"
  | "header"
  | "body"
  | "bodyRow"
  | "bodyRowCategories"
  | "textFieldContainer"
  | "select"
  | "selectContainer"
  | "checkBoxContainer"
  | "switchChecked"
  | "switchTrack"
  | "selectIcon"
  | "okButton"
  | "title"
  | "customIconButton"
  | "newCategoryButton"
  | "closeButton"
  | "closeIcon"
  | "listSearch";

const styles = (theme: Theme) =>
  createStyles<SaveClassKey, Props>({
    root: {
      flexGrow: 1,
      margin: theme.spacing(2, 4, 4)
    },
    header: {},
    body: {
      display: "flex",
      flexDirection: "column",
      justifyContent: "center"
    },
    bodyRow: {
      display: "flex",
      alignItems: "center",
      marginTop: theme.spacing(4)
    },
    bodyRowCategories: (props) => {
      return {
        display: "flex",
        alignItems: "flex-start",
        marginTop: props.disableRename ? 12 : theme.spacing(4)
      };
    },
    textFieldContainer: {
      marginRight: theme.spacing(22 / 8),
      width: 230
    },
    select: {
      borderRadius: 0,
      borderColor: "#c4c4c4",
      "&:hover": {
        borderColor: "black"
      }
    },
    selectContainer: {
      marginRight: theme.spacing(22 / 8),
      width: 230
    },
    checkBoxContainer: {},
    switchChecked: {
      color: theme.ptas.colors.utility.selection
    },
    switchTrack: {
      backgroundColor: theme.ptas.colors.theme.black
    },
    selectIcon: {
      borderRadius: 0,
      border: "none",
      background: "none"
    },
    okButton: {
      marginLeft: "auto",
      marginRight: 0
    },
    title: {},
    customIconButton: {},
    newCategoryButton: {
      marginTop: theme.spacing(2.5),
      marginLeft: theme.spacing(2.5)
    },
    closeButton: {
      position: "absolute",
      top: theme.spacing(2),
      right: theme.spacing(2),
      cursor: "pointer"
    },
    closeIcon: {},
    listSearch: {
      maxWidth: 250,
      maxHeight: 250
    }
  });

export interface SaveAcceptEvent {
  folderName?: string;
  isChecked?: boolean;
  route?: string;
  row?: DropdownTreeRow;
  comments?: string;
  categories?: (number | string)[];
}

/**
 * Component props
 */
interface Props {
  title: string;
  dropdownRows: DropdownTreeRow[];
  newFolderDropdownRows?: DropdownTreeRow[];
  checkboxText?: string;
  buttonText?: string;
  popoverTitle?: string;
  DropDownTreeProps?: DropDownTreeProps;
  NewFolderProps?: NewFolderProps;
  removeCheckBox?: boolean;
  defaultRoute?: string;
  defaultName?: string;
  defaultComments?: string;
  defaultChecked?: boolean;
  showComments?: boolean;
  isNewSave?: boolean;
  okClick: (event: SaveAcceptEvent) => void;
  newFolderOkClick?: (event: NewFolderAcceptEvt) => void;
  newCategoryOkClick?: (event: NewCategoryAcceptEvt) => void;
  useCategories?: boolean;
  categoriesData?: ListItem[];
  selectedCategories?: number[] | string[];
  onCategoryClick?: (selectedItem: Key, isSelected: boolean) => void;
  disableRename?: boolean;
}

/**
 * Save
 *
 * @param props - Component props
 * @returns A JSX element
 */
function Save(
  props: PropsWithChildren<Props> & WithStyles<typeof styles>
): JSX.Element {
  const {
    classes,
    title,
    checkboxText,
    okClick,
    newFolderOkClick,
    newCategoryOkClick,
    buttonText,
    popoverTitle,
    children,
    useCategories,
    categoriesData,
    selectedCategories,
    onCategoryClick
  } = props;

  const [
    newFolderAnchorEl,
    setNewFolderAnchorEl
  ] = useState<HTMLElement | null>();
  const [
    newCategoryAnchorEl,
    setNewCategoryAnchorEl
  ] = useState<HTMLElement | null>();
  const [selectedRow, setSelectedRow] = useState<DropdownTreeRow>();
  const [route, setRoute] = useState<string>();
  const [checkBoxState, setCheckBoxState] = useState<boolean>();
  const [name, setName] = useState<string>();
  const [comments, setComments] = useState<string>();
  const [dropRows, setDropRows] = useState<DropdownTreeRow[]>(
    props.dropdownRows
  );

  const _onAcceptButtonClick = () => {
    okClick({
      row: selectedRow,
      folderName: name,
      isChecked: checkBoxState,
      route: route,
      comments: comments,
      categories: selectedCategories
    });
  };

  const stripTrailingSlash = (str: string): string => {
    return str.endsWith("/") ? str.slice(0, -1) : str;
  };

  useMount(() => {
    setName(props.defaultName);
  });

  useEffect(() => {
    setRoute(props.defaultRoute);
    setComments(props.defaultComments);
    setCheckBoxState(props.defaultChecked);
    if (!props.defaultRoute) return;
    const routeName = stripTrailingSlash(props.defaultRoute);
    setSelectedRow(
      props.dropdownRows[
        props.dropdownRows.findIndex(
          (r) => r.title === routeName.substring(routeName.lastIndexOf("/") + 1)
        )
      ]
    );
  }, [dropRows]);

  useEffect(() => {
    setDropRows(props.dropdownRows);
  }, [props.dropdownRows]);

  return (
    <div className={classes.root}>
      <Box className={classes.header}>
        <Typography variant={"body1"} className={classes.title}>
          {title}
        </Typography>
      </Box>
      <Box className={classes.body}>
        {props.disableRename !== true && (
          <Box className={classes.bodyRow}>
            <CustomTextField
              label='Name'
              onChange={(value) => {
                setName(value.target.value);
              }}
              classes={{
                root: clsx(classes.textFieldContainer)
              }}
              defaultValue={props.defaultName}
            />

            {!props.removeCheckBox && !!!useCategories && (
              <Fragment>
                <Switch
                  onChange={(_value, checked) => {
                    setCheckBoxState(checked);
                  }}
                  classes={{
                    root: classes.checkBoxContainer,
                    checked: classes.switchChecked,
                    track: classes.switchTrack
                  }}
                  defaultChecked={props.defaultChecked ?? false}
                  color='primary'
                />
                {checkboxText}
              </Fragment>
            )}
          </Box>
        )}
        {!!!useCategories && (
          <Box className={classes.bodyRow}>
            <DropDownTree
              label='In folder'
              placeholder='Select'
              classes={{ container: classes.selectContainer }}
              onSelected={(val, route): void => {
                setSelectedRow(val);
                setRoute(route);
              }}
              rows={dropRows}
              defaultValue={props.defaultRoute}
              {...props.DropDownTreeProps}
            />
            <CustomIconButton
              icon={<AddCircleOutline />}
              text='New folder'
              className={classes.customIconButton}
              onClick={(e) => setNewFolderAnchorEl(e.currentTarget)}
            />
          </Box>
        )}
        {useCategories && (
          <Box className={classes.bodyRowCategories}>
            <Box>
              <ListSearch
                classes={{ root: classes.listSearch }}
                data={categoriesData || []}
                selectedData={selectedCategories}
                TextFieldProps={{ label: "Search categories" }}
                onClick={onCategoryClick}
              />
            </Box>
            <CustomIconButton
              icon={<AddCircleOutline />}
              text={"New category"}
              className={classes.newCategoryButton}
              onClick={(e) => setNewCategoryAnchorEl(e.currentTarget)}
            />
          </Box>
        )}
      </Box>
      {children && <Box className={classes.bodyRow}>{children}</Box>}
      {props.showComments && (
        <Box className={classes.bodyRow}>
          <CustomTextField
            multiline
            rows={4}
            label='Comments'
            style={{ width: "100%" }}
            onChange={(t): void => {
              setComments(t.currentTarget.value);
            }}
            defaultValue={props.defaultComments}
          />
        </Box>
      )}
      <Box className={classes.bodyRow}>
        <CustomButton
          onClick={_onAcceptButtonClick}
          classes={{ root: classes.okButton }}
          disabled={
            props.disableRename
              ? false
              : useCategories
              ? !name || !selectedCategories || selectedCategories.length < 1
              : props.isNewSave
              ? !name || !selectedRow
              : !name ||
                !selectedRow ||
                (name === props.defaultName &&
                  selectedRow ===
                    props.dropdownRows[
                      props.dropdownRows.findIndex(
                        (r) => r.title === props.defaultRoute?.substring(1)
                      )
                    ] &&
                  comments === props.defaultComments)
          }
          fullyRounded
        >
          {buttonText ?? "OK"}
        </CustomButton>
      </Box>
      <CustomPopover
        anchorEl={useCategories ? newCategoryAnchorEl : newFolderAnchorEl}
        onClose={() => {
          useCategories
            ? setNewCategoryAnchorEl(null)
            : setNewFolderAnchorEl(null);
        }}
        showCloseButton
      >
        {/* New Folder */}
        {!!!useCategories && props.newFolderDropdownRows !== undefined && (
          <NewFolder
            title={popoverTitle ?? "New folder"}
            buttonText='Save'
            okClick={(e) => {
              newFolderOkClick?.(e);
              setNewFolderAnchorEl(null);
            }}
            DropDownTreeProps={{
              rows: props.newFolderDropdownRows
            }}
            {...props.NewFolderProps}
          />
        )}
        {useCategories && (
          <NewCategory
            title={popoverTitle ?? "New category"}
            buttonText='Save'
            okClick={(e) => {
              newCategoryOkClick?.(e);
              setNewCategoryAnchorEl(null);
            }}
          />
        )}
      </CustomPopover>
    </div>
  );
}

export default withStyles(styles)(Save);
