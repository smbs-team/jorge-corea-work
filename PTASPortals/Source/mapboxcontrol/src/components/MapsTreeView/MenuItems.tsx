/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  MenuOption,
  OptionsMenu,
  OptionsMenuRefProps,
} from '@ptas/react-ui-library';
import React from 'react';
import userService from 'services/user/userService';
import { inRootFolder } from 'utils/userMap';
import DeleteMap from './DeleteMap';
import Move from './Move';
import { MapsTreeViewRow } from './types';

type Props = {
  optionsMenuRef: React.MutableRefObject<OptionsMenuRefProps | undefined>;
  row: MapsTreeViewRow;
  rows: MapsTreeViewRow[];
  onMenuItemClick: (
    _action: MenuOption,
    row?: MapsTreeViewRow | undefined
  ) => void;
  duplicateUserMap: (row: MapsTreeViewRow) => Promise<void>;
};

export default function MenuItems({
  optionsMenuRef,
  row,
  rows,
  onMenuItemClick,
  duplicateUserMap,
}: Props): JSX.Element {
  const filterLocked = (menuOption: MenuOption): boolean => {
    const optionId = menuOption.id.toString();
    if (optionId === 'Lock/unlock') return inRootFolder(row, 'shared');
    return true;
  };
  const menuItemFilter = (menuOption: MenuOption): boolean => {
    const optionId = menuOption.id.toString();
    /** Remove non folder options */
    if (row.folder) {
      return ![
        'Edit',
        'Duplicate',
        'Lock/unlock',
        'set-as-default',
        'Move',
      ].includes(optionId);
    } else if (
      /** Remove non System Administrator menu options */
      inRootFolder(row, 'system')
    ) {
      if (userService.isAdminUser()) {
        /** To remove for admin users */
        return !['Move'].includes(optionId);
      }
      /** To remove for non admin users */
      return !['Edit', 'Rename', 'Delete', 'Lock/unlock', 'Move'].includes(
        optionId
      );
    } else if (inRootFolder(row, 'shared')) {
      /**Allow move just for items created by the same user */
      if (row.createdBy !== userService.userInfo.id) {
        return !['Move'].includes(optionId);
      }
      return true;
    }

    return true;
  };

  const items: MenuOption[] = [
    { label: 'Edit', id: 'Edit', disabled: row.folder || row.isLocked },
    {
      label: 'Duplicate',
      id: 'Duplicate',
      afterClickContent: (): JSX.Element => (
        <Move
          duplicate
          title={'Duplicate map ' + row.title}
          row={{ ...row, createdBy: userService.userInfo.id }}
          rows={rows}
          menuRef={optionsMenuRef}
          okClick={(route): void => {
            duplicateUserMap({
              ...row,
              folderPath: route,
            });
          }}
        />
      ),
    },
    {
      disabled: row.isLocked,
      label: 'Move',
      id: 'Move',
      afterClickContent: (): JSX.Element => (
        <Move row={row} rows={rows} menuRef={optionsMenuRef} />
      ),
    },
    {
      disabled: row.isLocked,
      label: 'Rename',
      id: 'Rename',
    },
    {
      disabled: row.isLocked,
      label: 'Delete',
      id: 'Delete',
      afterClickContent: <DeleteMap row={row} />,
      isAlert: true,
    },
    {
      label: row.isLocked ? 'Unlock' : 'Lock',
      id: 'Lock/unlock',
      disabled:
        row.createdBy !== userService.userInfo.id && !userService.isAdminUser(),
    },
    {
      label: 'Set as default',
      id: 'set-as-default',
      disabled: row.isLocked,
    },
  ].filter((item) => [menuItemFilter, filterLocked].every((fn) => fn(item)));

  return (
    <OptionsMenu<MapsTreeViewRow>
      closeAfterPopover={false}
      ref={optionsMenuRef}
      row={row}
      onItemClick={onMenuItemClick}
      showTail={false}
      items={items}
    />
  );
}
