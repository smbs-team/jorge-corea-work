// Save.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  MutableRefObject,
  useContext,
  useEffect,
  useState,
} from 'react';
import {
  Box,
  Typography,
  Theme,
  makeStyles,
  useTheme,
} from '@material-ui/core';
import { AddCircleOutline } from '@material-ui/icons';
import {
  CustomButton,
  CustomPopover,
  DropDownTree,
  CustomIconButton,
  NewFolder,
  OptionsMenuRefProps,
  SnackContext,
} from '@ptas/react-ui-library';
import { MapsTreeViewRow } from './types';
import userService from 'services/user/userService';
import { ROOT_FOLDER } from 'appConstants';
import { HomeContext } from 'contexts';
import { mapService } from 'services/map';
import { apiService } from 'services/api';
import { useCreateFolder } from 'hooks/api';
import { createFolderRows } from 'utils/userMapFolder';
import { getFolderPath } from 'utils/componentUtil';
import { utilService } from 'services/common';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    flexGrow: 1,
    margin: theme.spacing(2, 4, 4),
  },
  header: {},
  body: {
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'center',
  },
  bodyRow: {
    display: 'flex',
    alignItems: 'center',
    marginTop: theme.spacing(4),
  },
  bodyRowCategories: {
    display: 'flex',
    alignItems: 'flex-start',
    marginTop: theme.spacing(4),
  },
  textFieldContainer: {
    marginRight: theme.spacing(22 / 8),
    width: 230,
  },
  select: {
    borderRadius: 0,
    borderColor: '#c4c4c4',
    '&:hover': {
      borderColor: 'black',
    },
  },
  selectContainer: {
    marginRight: theme.spacing(22 / 8),
    width: 230,
  },
  checkBoxContainer: {},
  switchChecked: {
    color: theme.ptas.colors.utility.selection,
  },
  switchTrack: {
    backgroundColor: theme.ptas.colors.theme.black,
  },
  selectIcon: {
    borderRadius: 0,
    border: 'none',
    background: 'none',
  },
  okButton: {
    marginLeft: 'auto',
    marginRight: 0,
  },
  title: {},
  customIconButton: {},
  newCategoryButton: {
    marginTop: theme.spacing(2.5),
    marginLeft: theme.spacing(2.5),
  },
  closeButton: {
    position: 'absolute',
    top: theme.spacing(2),
    right: theme.spacing(2),
    cursor: 'pointer',
  },
  closeIcon: {},
  listSearch: {
    maxWidth: 250,
    maxHeight: 250,
  },
}));
interface Props {
  row: MapsTreeViewRow;
  rows: MapsTreeViewRow[];
  menuRef: MutableRefObject<OptionsMenuRefProps | undefined>;
  okClick?: (folderPath: string) => void;
  title?: string;
  duplicate?: true;
}

const removeSlashes = (str: string): string => str.replace(/\//g, '');

/**
 * Move map
 *
 * @param props - Component props
 * @returns A JSX element
 */
function Move(props: Props): JSX.Element {
  const { setSnackState } = useContext(SnackContext);
  const { setLinearProgress } = useContext(HomeContext);
  const createFolder = useCreateFolder();
  const { row, rows, menuRef } = props;
  const [newFolderAnchorEl, setNewFolderAnchorEl] = useState<HTMLElement>();
  const [route, setRoute] = useState<string>(row.folderPath + '' ?? '');
  const [folders, setFolders] = useState<MapsTreeViewRow[]>([]);
  const classes = useStyles(useTheme());
  const [createdFolderPath, setCreatedFolderPath] = useState<string>();

  /** Filter folder rows and permissions */
  useEffect(() => {
    setFolders(
      rows.filter((r) => {
        if (!row.folderPath) return false;
        if (r.folder) {
          if (userService.isAdminUser()) {
            if (
              removeSlashes(row.folderPath).startsWith(
                removeSlashes(ROOT_FOLDER.USER)
              )
            ) {
              return true;
            } else {
              if (row.createdBy === userService.userInfo.id) return true;
              else {
                const currRowFolderPath = getFolderPath(r, rows, r.folder);
                if (
                  removeSlashes(currRowFolderPath).startsWith(
                    utilService.getPathArray(row.folderPath).shift() ?? ''
                  )
                )
                  return true;
                return false;
              }
            }
          } else if (
            getFolderPath(r, rows, r.folder).startsWith(ROOT_FOLDER.SYSTEM)
          ) {
            return false;
          } else {
            return true;
          }
        }
        return false;
      })
    );
  }, [row.createdBy, row.folderPath, rows]);

  const updateMapFolder = async (): Promise<void> => {
    try {
      menuRef.current?.closePopup();
      if (typeof row.id !== 'number') return;
      setLinearProgress(true);
      if ('system'.match(route.toLowerCase().replace(/\//g, ''))) {
        await mapService.duplicateUserMap(row.id, row.title, route);
        await apiService.deleteUserMap(row.id);
      } else {
        const res = await apiService.updateUserMap(row.id, {
          folderPath: route,
        });
        if (res.hasError) {
          throw new Error(res.errorMessage);
        }
      }
      await mapService.refreshUserMaps();
      await mapService.refreshUserMapFolders();
    } catch (e) {
      setSnackState({
        severity: 'error',
        text: 'Error moving map ' + row.title,
      });
      if (createdFolderPath) {
        apiService.deleteFolder({
          folderPath: createdFolderPath,
          folderItemType: 'UserMap',
        });
      }
    } finally {
      setLinearProgress(false);
    }
  };

  const onCreateFolder = async (folderPath: string): Promise<void> => {
    try {
      setNewFolderAnchorEl(undefined);
      setRoute(folderPath);
      await createFolder(folderPath, { silent: true });
      setCreatedFolderPath(folderPath);
      const foldersRes = await apiService.getUserFolders(
        userService.userInfo.id,
        'UserMap'
      );
      if (foldersRes.length) {
        setFolders(createFolderRows(foldersRes));
      }
    } catch (e) {
      setSnackState({
        severity: 'error',
        text: 'Error creating folder ' + folderPath,
      });
    }
  };

  return (
    <div className={classes.root}>
      <Box className={classes.header}>
        <Typography variant={'body1'} className={classes.title}>
          {props.title
            ? props.title
            : `Move ${props.row.folder ? ' folder' : 'map'} ${props.row.title}`}
        </Typography>
      </Box>
      <Box className={classes.body}>
        <Box className={classes.bodyRow}>
          <DropDownTree
            label="In folder"
            placeholder="Select"
            classes={{ container: classes.selectContainer }}
            onSelected={(_, route): void => {
              setRoute(route);
            }}
            rows={folders}
            defaultValue={route}
          />
          <CustomIconButton
            icon={<AddCircleOutline />}
            text="New folder"
            className={classes.customIconButton}
            onClick={(e): void => {
              setNewFolderAnchorEl(e.currentTarget);
            }}
          />
        </Box>
      </Box>
      <Box className={classes.bodyRow}>
        <CustomButton
          classes={{ root: classes.okButton }}
          fullyRounded
          onClick={(): void => {
            if (props.okClick) {
              return props.okClick(route);
            }
            updateMapFolder();
          }}
          disabled={
            !props.duplicate &&
            removeSlashes(row.folderPath ?? '') === removeSlashes(route)
          }
        >
          Ok
        </CustomButton>
      </Box>
      <CustomPopover
        anchorEl={newFolderAnchorEl}
        showCloseButton
        onClose={(): void => {
          setNewFolderAnchorEl(undefined);
        }}
      >
        <NewFolder
          title={'New folder'}
          buttonText="Save"
          okClick={(e): void => {
            onCreateFolder(e.route + e.folderName);
          }}
          DropDownTreeProps={{
            rows: folders,
          }}
          defaultRoute={route}
        />
      </CustomPopover>
    </div>
  );
}

export default Move;
