// useBubble.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { RenderMenuProps } from '@ptas/react-ui-library';
import { useState } from 'react';
import bubbleService from 'services/map/bubble/bubbleService';
import { BubbleSettingsProps } from 'components/BubbleSettings';
import { mapService } from 'services/map';

type UseBubble = {
  onBubbleOptionClick: (closeMenu: RenderMenuProps['closeMenu']) => () => void;
  bubbleSettingsProps: BubbleSettingsProps;
};

export const useBubble = (): UseBubble => {
  const [bubble, setBubble] = useState<BubbleSettingsProps>();
  const map = mapService.map;

  const onBubbleOptionClick = (
    closeMenu: RenderMenuProps['closeMenu']
  ) => (): void => {
    if (map === undefined) return;
    bubbleService.placeBubble(map);
    closeMenu();
  };

  const closeEditBubble = (): void => {
    setBubble((b) => ({
      ...b,
      anchorEl: null,
    }));
  };

  bubbleService.onEditBubble((b: BubbleSettingsProps): void => {
    setBubble(b);
  });

  return {
    onBubbleOptionClick,
    bubbleSettingsProps: {
      ...bubble,
      close: closeEditBubble,
      updateBubbleSettings: bubbleService.updateBubbleSettings,
    } as BubbleSettingsProps,
  };
};
