/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Theme, makeStyles, Divider } from '@material-ui/core';
import clsx from 'clsx';
import { ParcelInfoContext } from 'contexts/ParcelInfoContext';
import React, { useContext, memo } from 'react';
import { GisMapDataFields } from 'services/map';
import SummaryProgress from './SummaryProgress';

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    textAlign: 'center',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontWeight: 'bold',
    height: '58%',
    marginTop: 12,
    overflowY: 'auto',
  },
  field: {
    marginBottom: 10,
  },
  divider: {
    backgroundColor: 'unset',
    height: 12,
  },
}));

type Props = {
  parcelData: GisMapDataFields;
};

function FieldComp({
  text,
  show,
}: {
  text?: string;
  show?: boolean;
}): JSX.Element | null {
  const classes = useStyles();
  if (show === false || !text?.length) return null;
  return <div className={classes.field}>{text}</div>;
}

const ParcelSummary = memo(
  function ({ parcelData }: Props): JSX.Element {
    const classes = useStyles();
    const getFields = (): JSX.Element | undefined => {
      switch (parcelData.GeneralClassif) {
        case 'ResImp':
        case 'ResVac':
        case 'ResMH':
        case 'ResAccy':
        case 'CmlImp':
        case 'CmlAccy':
        case 'CmlVac':
        case 'Other': {
          return (
            <>
              <FieldComp
                show={parcelData.GeneralClassif === 'ResImp'}
                text={[
                  parcelData.BldgGrade,
                  parcelData.CondDescr,
                  parcelData.YrBltRen,
                  parcelData.SqFtTotLiving,
                  ' sqft',
                ].join(' ')}
              />
              <FieldComp
                text="ResMH"
                show={parcelData.GeneralClassif === 'ResMH'}
              />
              <FieldComp
                text={`${parcelData.NbrCmlAccys} ResAccys`}
                show={parcelData.GeneralClassif === 'ResAccy'}
              />
              <FieldComp
                text={parcelData.PropName}
                show={['CmlImp', 'CmlAccy', 'CmlVac', 'Other'].includes(
                  parcelData.GeneralClassif
                )}
              />
              {parcelData.GeneralClassif === 'CmlImp' && (
                <>
                  <FieldComp text={parcelData.PresentUse} />
                  <FieldComp text={`${parcelData.CmlPredominantUse}`} />
                  <FieldComp text={`NetSqFt=${parcelData.CmlNetSqFtAllBldg}`} />
                </>
              )}
              <FieldComp
                text={`${parcelData.NbrCmlAccys} CmlAccys`}
                show={parcelData.GeneralClassif === 'CmlAccy'}
              />
              <FieldComp text={parcelData.CurrentZoning} />
              <FieldComp
                text={[
                  'SqFtLot=',
                  parcelData.SqFtLot,
                  parcelData.WfntLabel,
                ].join(' ')}
              />
              <FieldComp
                text={[
                  parcelData.LandProbDescrPart1,
                  parcelData.LandProbDescrPart2,
                ].join(' ')}
              />
              <FieldComp text={parcelData.ViewDescr} />
              <FieldComp
                text={`${parcelData.BaseLandValTaxYr} ry BLV=${parcelData.BaseLandVal} (${parcelData.BLVSqFtCalc})`}
              />
              <FieldComp
                text={[
                  'New:',
                  parcelData.LandVal,
                  '+',
                  parcelData.ImpsVal,
                  '=',
                  parcelData.TotVal,
                ].join(' ')}
              />
              <FieldComp
                text={[
                  'Prev:',
                  parcelData.PrevLandVal,
                  '+',
                  parcelData.PrevImpsVal,
                  '=',
                  parcelData.PrevTotVal,
                ].join(' ')}
              />
              <FieldComp
                text={`LandChg=${parcelData.PcntChgLand}%  ImpsChg=${parcelData.PcntChgImps}%  TotChg=${parcelData.PcntChgTotal}%`}
              />
              <FieldComp text={parcelData.AddrLine} />
              {parcelData.GeneralClassif !== 'Other' && (
                <>
                  <Divider classes={{ root: classes.divider }} />
                  <FieldComp
                    text={
                      ['ResImp', 'ResVac', 'ResMH', 'ResAccy'].includes(
                        parcelData.GeneralClassif
                      )
                        ? `${parcelData.PropType}/${parcelData.ApplGroup} ${parcelData.ResAreaSub}`
                        : `${parcelData.GeoAreaNbhd} ${parcelData.SpecAreaNbhd}`
                    }
                  />
                </>
              )}
            </>
          );
        }
        default:
          return;
      }
    };

    return (
      <div className={clsx(classes.root)}>
        <FieldComp text={parcelData.TaxStatus} />
        <FieldComp text={parcelData.TaxpayerName} />
        <Divider classes={{ root: classes.divider }} />
        {getFields()}
      </div>
    );
  },
  (prev, next) =>
    JSON.stringify(prev.parcelData) === JSON.stringify(next.parcelData)
);

export default (): JSX.Element | null => {
  const { summaryState, selectedParcelInfo, pin } = useContext(
    ParcelInfoContext
  );
  return pin &&
    selectedParcelInfo &&
    selectedParcelInfo.Major + selectedParcelInfo.Minor === pin ? (
    <ParcelSummary parcelData={selectedParcelInfo} />
  ) : summaryState === 'loading' ? (
    <SummaryProgress />
  ) : null;
};
