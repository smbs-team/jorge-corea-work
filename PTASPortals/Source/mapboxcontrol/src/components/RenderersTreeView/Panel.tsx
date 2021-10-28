// BasePanel.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, PropsWithChildren, memo, useContext } from 'react';
import BasePanel from 'components/BasePanel';
import {
  AddCircleOutline as AddIcon,
  // CreateNewFolder as FolderIcon,
} from '@material-ui/icons';
import { ResizableProps } from 'libComponents/Resizable';
import { PanelHeader } from '@ptas/react-ui-library';
import { HomeContext } from 'contexts';
import MapsLayersManage from '../MapLayersManage';

interface Props {
  resizableProps?: ResizableProps;
}

export const Panel = memo(
  (props: PropsWithChildren<Props>): JSX.Element => {
    const homeContext = useContext(HomeContext);
    return (
      <BasePanel
        resizableProps={props.resizableProps}
        toolbarItems={
          <Fragment>
            <PanelHeader
              route={[<label key={1}>Renderers</label>]}
              icons={[
                {
                  text: 'New Renderer',
                  icon: <AddIcon />,
                  onClick: (): void => {
                    homeContext.setPanelContent(
                      <MapsLayersManage isSystemUserMap={true} />
                    );
                  },
                },
              ]}
            ></PanelHeader>
          </Fragment>
        }
        disableScrollY
      >
        {props.children}
      </BasePanel>
    );
  }
);
