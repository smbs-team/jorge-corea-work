/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect, useState } from 'react';
import { makeStyles, Theme } from '@material-ui/core';
import { useMount, useObservable, useUnmount } from 'react-use';
import { SimplePagination } from '@ptas/react-ui-library';
import { layerService, mapService } from 'services/map';
import { useSelectedDots } from 'hooks/map';
import selectionService from 'services/parcel/selection';
import { DotFeature } from 'services/map/dot';
import { $onSelectedLayersChange } from 'services/map/mapServiceEvents';
import { DrawToolBarContext } from 'contexts/DrawToolbarContext';
import { ParcelInfoContext } from 'contexts/ParcelInfoContext';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    display: 'flex',
    alignItems: 'center',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodySmall.fontWeight,
    lineHeight: theme.ptas.typography.lineHeight,
  },
}));

const useOnPageChange = (): ((page: number, dots: DotFeature[]) => void) => {
  const { setPin } = useContext(ParcelInfoContext);
  const { map } = mapService;

  return (page: number, dots: DotFeature[]): void => {
    if (!dots.length) return;
    console.log('Page changed to %d', page);
    if (!map) return;
    setPin(dots[page]?.properties.major + dots[page]?.properties.minor);
    selectionService.currentPagePin =
      dots[page]?.properties.major + dots[page]?.properties.minor;
    selectionService.render({
      doApiRequest: false,
    });
  };
};

const flyToPage = (index: number): void => {
  const center = selectionService.getSelectedDotsList()[index]?.geometry
    ?.coordinates as [number, number] | undefined;

  if (!center?.length) return;
  mapService.map?.panTo(center, { duration: 0 });
};

function ParcelPagination(): JSX.Element {
  const classes = useStyles();
  const dots = useSelectedDots();
  const onPageChange = useOnPageChange();
  const [page, setPage] = useState<number>(0);
  const { setSelectedParcelInfo } = useContext(ParcelInfoContext);
  const selection = useObservable(selectionService.$onChangeSelection);
  const selectedLayers = useObservable($onSelectedLayersChange);
  const [show, setShow] = useState(false);

  useUnmount(() => {
    setSelectedParcelInfo(undefined);
    selectionService.currentPagePin = undefined;
  });

  useMount(() => {
    flyToPage(0);
  });

  useEffect(() => {
    const showVal =
      !!dots.length &&
      selectionService.isEnabled() &&
      layerService.hasParcelLayer();
    setShow(showVal);
  }, [dots.length, selectedLayers, selection]);

  useEffect(() => {
    if (!show) return;
    if (dots.length - 1 < page) {
      return setPage(0);
    }
    onPageChange(page, dots);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page, dots.length, show]);

  return (
    <Fragment>
      {show && page >= 0 && (
        <SimplePagination
          currentPage={page}
          onArrowClick={(val): void => {
            if (val === undefined) return;
            flyToPage(val);
          }}
          itemsCount={dots.length}
          classes={{ root: classes.root }}
          onPageChange={(_page): void => {
            setPage(_page);
          }}
        />
      )}
    </Fragment>
  );
}

export default (): JSX.Element => {
  const { actionMode } = useContext(DrawToolBarContext);
  return <Fragment>{actionMode === 'draw' && <ParcelPagination />}</Fragment>;
};
