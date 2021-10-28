// ToolsMenu.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  Fragment,
  useContext,
  useEffect,
  useRef,
  useState,
} from 'react';
import { Box } from '@material-ui/core';
import { MenuRootContent } from '@ptas/react-ui-library';
import '@ptas/react-ui-library/dist/index.css';
import { HomeContext } from 'contexts';
import OverlapCalculator from 'components/Tools/Calculate/OverlapCalculator';
import WalkingDistance from 'components/Tools/Calculate/WalkingDistance';
import DrivingTime from 'components/Tools/Calculate/DrivingTime';
import { mapService } from 'services/map';
import PrintMap from 'components/PrintMap';
import { CustomMenuRoot } from 'components/common';
import { useBubble } from '../common/useBubble';
import ModalGrid from 'components/common/ModalGrid';
import InfoMode from 'components/Tools/InfoMode';
import BubbleSettings from 'components/BubbleSettings';
import { useOnSelectedLayers } from 'hooks/map/useOnSelectedLayers';
import { MenuItem, NestedMenu } from '../common';
import { CalculateContext } from 'contexts/CalculateContext';
import { BaseMapService } from 'services/map/BaseMapService';

/**
 * Tools menu
 */
function ToolsMenu(): JSX.Element {
  const {
    setOpenExportMapImgModal,
    setMapImgCaptureSrc,
    setPanelContent,
  } = useContext(HomeContext);
  const { fetching } = useContext(CalculateContext);
  const onSelectedLayers = useOnSelectedLayers();
  const { onBubbleOptionClick, bubbleSettingsProps } = useBubble();
  const printMapTriggerRef = useRef<HTMLButtonElement>();
  const [imgPrintMap, setImgPrintMap] = useState<string>('');
  const [openModalGrid, setOpenModalGrid] = useState<boolean>(false);
  const [usingBaseMap, setUsingBaseMap] = useState(
    !!BaseMapService.instance?.useBaseMap
  );

  useEffect(() => {
    BaseMapService.instance?.toggle();
  }, [usingBaseMap]);

  return (
    <Fragment>
      <CustomMenuRoot text="Tools">
        {({ closeMenu, isMenuRootOpen, menuProps }): JSX.Element => (
          <MenuRootContent {...menuProps}>
            <MenuItem
              onClick={(): void => {
                setOpenModalGrid(true);
                closeMenu();
              }}
            >
              Add Layer...
            </MenuItem>
            <NestedMenu
              label={
                <Box component="span" width="100%">
                  Calculate
                </Box>
              }
              parentMenuOpen={isMenuRootOpen}
            >
              <MenuItem
                onClick={(): void => {
                  setPanelContent(<OverlapCalculator />);
                  closeMenu();
                }}
              >
                Overlap Calculator
              </MenuItem>
              <MenuItem
                disabled={fetching}
                onClick={(): void => {
                  setPanelContent(<WalkingDistance />);
                  closeMenu();
                }}
              >
                Walking distance
              </MenuItem>
              <MenuItem
                disabled={fetching}
                onClick={(): void => {
                  setPanelContent(<DrivingTime />);
                  closeMenu();
                }}
              >
                Driving time
              </MenuItem>
            </NestedMenu>
            <MenuItem
              onClick={(): void => {
                setPanelContent(<InfoMode />);
                closeMenu();
              }}
            >
              Info Mode
            </MenuItem>
            <NestedMenu
              label={
                <Box component="span" width="100%">
                  Export
                </Box>
              }
              parentMenuOpen={isMenuRootOpen}
            >
              <MenuItem
                onClick={async (): Promise<void> => {
                  if (!mapService.map) return;
                  await mapService.takeScreenshot().then((srcImg: string) => {
                    setOpenExportMapImgModal(true);
                    setMapImgCaptureSrc(srcImg);
                  });
                  closeMenu();
                }}
              >
                Map as image...
              </MenuItem>
            </NestedMenu>
            <NestedMenu
              label={
                <Box component="span" width="100%">
                  Bubbles
                </Box>
              }
              parentMenuOpen={isMenuRootOpen}
            >
              <MenuItem onClick={onBubbleOptionClick(closeMenu)}>
                Bubble
              </MenuItem>
            </NestedMenu>
            <MenuItem
              onClick={(): void => {
                if (!mapService.map) return;
                mapService.takeScreenshot().then((srcImg: string) => {
                  setImgPrintMap(srcImg);
                  printMapTriggerRef.current &&
                    printMapTriggerRef.current.click();
                });
                closeMenu();
              }}
            >
              Print
            </MenuItem>
            {BaseMapService.instance?.useBaseMap && (
              <MenuItem
                onClick={(): void => {
                  setUsingBaseMap((prev) => !prev);
                }}
              >
                {`${usingBaseMap ? 'Hide' : 'Show'}  base map`}
              </MenuItem>
            )}
          </MenuRootContent>
        )}
      </CustomMenuRoot>
      <PrintMap imgSrc={imgPrintMap} btnPrintRef={printMapTriggerRef} />
      {openModalGrid && (
        <ModalGrid
          isOpen={true}
          onClose={(): void => setOpenModalGrid(false)}
          onButtonClick={(selectedRows): void => {
            onSelectedLayers(selectedRows);
          }}
        />
      )}
      <BubbleSettings {...bubbleSettingsProps} />
    </Fragment>
  );
}

export default ToolsMenu;
