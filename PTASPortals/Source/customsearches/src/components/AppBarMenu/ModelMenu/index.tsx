// filename
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import { MenuRoot, MenuRootContent } from '@ptas/react-ui-library';
import LinkMenuItem from '../LinkMenuItem';

interface Props {
  isFrozen?: boolean;
  id?: number;
  handleFreeze?: (id?: number) => void;
}

const ModelMenu = (props: Props): JSX.Element => {
  const { isFrozen, id, handleFreeze } = props;

  console.log(`id`, id, isFrozen);

  const renderFrozenOption = (): JSX.Element => {
    if (isFrozen) return <button className="Models-button">Unfreeze</button>;
    return (
      <button className="Models-button" onClick={onFreeze?.(id)}>
        Freeze
      </button>
    );
  };

  const onFreeze = (id?: number) => (): void => {
    handleFreeze?.(id);
  };

  return (
    <MenuRoot text="Models">
      {({ menuProps, closeMenu }): JSX.Element => (
        <MenuRootContent {...menuProps}>
          <LinkMenuItem
            display="New Model..."
            link="/models/new-model"
            closeMenu={closeMenu}
          />
          <LinkMenuItem
            display="All models"
            link="/models"
            closeMenu={closeMenu}
          />
          {renderFrozenOption()}
        </MenuRootContent>
      )}
    </MenuRoot>
  );
};

export default ModelMenu;
