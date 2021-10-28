// Ribbon.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useState } from 'react';
import { ButtonBase, Collapse, makeStyles } from '@material-ui/core';
import clsx from 'clsx';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import Configuration from './ribbonSections/Configuration';
import RentableArea from './ribbonSections/RentableArea';
import AdjustmentFactors from './ribbonSections/AdjustmentFactors';
import ValuationReady from './ribbonSections/ValuationReady';
import useRibbonStore from 'stores/useRibbonStore';
import Settings from './ribbonSections/Settings';
import HotelsConfig from './ribbonSections/HotelsConfig';

const useStyles = makeStyles({
  valuationReady: {
    borderLeft: '1px solid #c0c0c0',
  },
  rentableArea: {
    borderLeft: '1px solid #c0c0c0',
    borderRight: '1px solid #c0c0c0',
  },
  settings: {
    borderLeft: '1px solid #c0c0c0',
  },
  padding: {
    paddingLeft: 24,
    paddingRight: 24,
  },
  unfold: {
    position: 'absolute',
    top: -2,
    right: 0,
  },
  wrapperInner: {
    display: 'flex',
  },
});

function Ribbon(): JSX.Element {
  const classes = useStyles();
  const [isOpen, setIsOpen] = useState<boolean>(true);
  const store = useRibbonStore();

  return (
    <div style={{ minHeight: 16 }}>
      <Collapse in={isOpen} classes={{ wrapperInner: classes.wrapperInner }}>
        {store.showHotelsConfig && (
          <div className={clsx(classes.padding)}>
            <HotelsConfig />
          </div>
        )}
        {!store.hideConfiguration && (
          <div className={classes.padding}>
            <Configuration />
          </div>
        )}
        {!store.hideRentableArea && (
          <div className={clsx(classes.rentableArea, classes.padding)}>
            <RentableArea />
          </div>
        )}
        {!store.hideAdjustmentFactors && (
          <div className={classes.padding}>
            <AdjustmentFactors />
          </div>
        )}
        {!store.hideSettings && (
          <div className={clsx(classes.padding, classes.settings)}>
            <Settings />
          </div>
        )}
        {!store.hideValuationReady && (
          <div className={clsx(classes.valuationReady, classes.padding)}>
            <ValuationReady />
          </div>
        )}
      </Collapse>
      <ButtonBase
        className={classes.unfold}
        onClick={() => setIsOpen(!isOpen)}
        disableRipple
        title={isOpen ? 'Collapse ribbon' : 'Open ribbon'}
      >
        <ExpandMoreIcon
          style={{
            transform: isOpen ? 'rotate(180deg)' : undefined,
          }}
        />
      </ButtonBase>
    </div>
  );
}

export default Ribbon;
