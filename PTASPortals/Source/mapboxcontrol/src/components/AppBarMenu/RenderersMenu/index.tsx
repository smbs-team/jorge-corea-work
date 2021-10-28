// RenderersMenu.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useContext, forwardRef } from 'react';
import { makeStyles, createStyles, Theme, Box } from '@material-ui/core';
import DoneIcon from '@material-ui/icons/Done';
import {
  MenuRootContent,
  RenderMenuProps,
  CustomNestedItem,
} from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import RenderersTreeView from 'components/RenderersTreeView';
import { useRenderersMenu } from './useRenderersMenu';
import { CustomMenuRoot, MenuRootItem } from 'components/common';
import userService from 'services/user/userService';
import { systemUserMapService } from 'services/map/systemUserMapService';
import { v4 as uuidV4 } from 'uuid';
import clsx from 'clsx';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      fontSize: '0.875rem',
      fontWeight: 'bold',
      minHeight: 45,
      paddingLeft: 52,
    },
    paper: {
      overflow: 'hidden',
    },
    leftIcon: {
      left: 4,
      top: -4,
      fontSize: 12,
    },
    subheader: {
      fontFamily: theme.ptas.typography.bodyFontFamily,
      height: 30,
      color: theme.ptas.colors.theme.black,
    },
    gutters: {
      paddingLeft: 36,
    },
    menuContent: {
      overflow: 'auto',
      maxHeight: 500,
    },
    nestedItem: {
      height: 40,
      fontSize: '0.875rem',
      fontWeight: 'normal',
      fontFamily: theme.ptas.typography.bodyFontFamily,
      paddingLeft: 48,
    },
    rootItem: {
      fontSize: '0.875rem',
      height: 18,
      paddingLeft: 30,
    },
    emptyCategory: {
      cursor: 'default',
    },
    edit: {
      fontSize: '0.875rem',
      minHeight: 40,
    },
    categoriesArray: {
      maxHeight: 200,
      overflowY: 'auto',
    },
  })
);

const RenderersMenu = forwardRef<HTMLElement, {}>(
  (): JSX.Element => {
    const classes = useStyles();
    const { loadUserMapsByCategory } = useRenderersMenu();
    const {
      setPanelContent,
      rendererCategories,
      selectedSystemUserMap,
    } = useContext(HomeContext);

    return (
      <CustomMenuRoot text="Renderers">
        {(renderMenuProps: RenderMenuProps): JSX.Element => (
          <MenuRootContent
            {...renderMenuProps.menuProps}
            classes={{ paper: classes.paper }}
          >
            <MenuRootItem
              key="none"
              leftIcon={
                selectedSystemUserMap?.userMapId === undefined ? (
                  <DoneIcon />
                ) : undefined
              }
              onClick={(): void => {
                systemUserMapService.applySystemUserMap(undefined);
              }}
            >
              None
            </MenuRootItem>
            <div className={classes.categoriesArray}>
              {rendererCategories.map((c, i) => {
                return (
                  <CustomNestedItem
                    key={i}
                    className={classes.nestedItem}
                    label={
                      <Box component="span" width="100%">
                        {c.categoryName}
                      </Box>
                    }
                    parentMenuOpen={renderMenuProps.isMenuRootOpen}
                    rightIcon={undefined}
                    onMouseEnter={async (): Promise<void> => {
                      if (!c.userMaps) {
                        loadUserMapsByCategory(c);
                      }
                    }}
                  >
                    {c.userMaps?.length === 0 && (
                      <MenuRootItem
                        key={uuidV4()}
                        classes={{
                          root: clsx(classes.rootItem, classes.emptyCategory),
                        }}
                      >
                        [empty category]
                      </MenuRootItem>
                    )}
                    {c.userMaps &&
                      c.userMaps.map((m) => {
                        return (
                          <MenuRootItem
                            key={m.userMapId}
                            classes={{
                              root: classes.rootItem,
                              leftIcon: classes.leftIcon,
                            }}
                            leftIcon={
                              m.userMapId ===
                              selectedSystemUserMap?.userMapId ? (
                                <DoneIcon />
                              ) : undefined
                            }
                            onClick={(): void => {
                              systemUserMapService.applySystemUserMap(
                                m.userMapId
                              );
                            }}
                          >
                            {m.userMapName}
                          </MenuRootItem>
                        );
                      })}
                  </CustomNestedItem>
                );
              })}
            </div>

            {userService.isAdminUser() && (
              <MenuRootItem
                classes={{
                  root: classes.edit,
                }}
                variant="fill"
                onClick={(): void => {
                  setPanelContent(<RenderersTreeView />);
                  renderMenuProps.closeMenu();
                }}
              >
                Edit renderer...
              </MenuRootItem>
            )}
          </MenuRootContent>
        )}
      </CustomMenuRoot>
    );
  }
);

export default RenderersMenu;
