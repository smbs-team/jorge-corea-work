// UpdatedInfoTable.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useEffect, useState } from 'react';
import { CustomButton } from '@ptas/react-public-ui-library';
import { Box, makeStyles, Modal } from '@material-ui/core';
import { Theme } from '@material-ui/core/styles';
import AssetsList, { BusinessAsset } from './AssetsList';
import clsx from 'clsx';
import * as fm from './formatText';
import { Close } from '@material-ui/icons';
import { businessApiService } from 'services/api/apiService/business';
import useUpdateBusiness from '../useUpdateBusiness';

//TEMP model used for showing UI while there is no integration with back-end
export interface Business {
  ownerName: string;
  businessName: string;
  ubiNumber: string;
  naicsNumber: string;
  businessType: string;
  stateOfIncorporation: string;
  contactName: string;
  attention: string;
  email: string;
  phone: string;
  mailingAddress: {
    title: string;
    city: string;
    state: string;
    zip: string;
  };
  locationAddress: {
    title: string;
    city: string;
    state: string;
    zip: string;
  };
  assets: BusinessAsset[];
}

interface Props {
  businessOld?: Business; //TODO: make field required
  businessNew?: Business; //TODO: make field required
  open: boolean;
  onClose: () => void;
}

const useStyles = makeStyles((theme: Theme) => ({
  modal: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
  },
  headerContent: {
    width: '790px',
    display: 'flex',
    alignItems: 'center',
    backgroundColor: '#F4EB49',
    margin: 0,
    padding: theme.spacing(1.5, 2),

    marginTop: '12px',
    borderRadius: '9px',
  },
  headerText: {
    marginRight: theme.spacing(2),
    fontSize: theme.ptas.typography.body.fontSize,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    lineHeight: '22px',
  },
  headerButton: {
    width: '168px',
    height: 30,
    cursor: 'pointer',

    [theme.breakpoints.up('sm')]: {
      margin: 'auto 0',
    },
  },
  paper: {
    position: 'absolute',
    backgroundColor: 'white',
    border: '2px solid #000',
    boxShadow: theme.shadows[5],
    padding: theme.spacing(2, 4, 3),
    borderRadius: '9px',
    top: '72px',
    maxHeight: 'calc(100% - 120px)',
    overflowY: 'auto',
    overflowX: 'hidden',
    width: '90%',

    '&::-webkit-scrollbar': {
      width: 8,
    },

    /* Track */
    '&::-webkit-scrollbar-track': {
      borderRadius: 10,
    },

    /* Handle */
    '&::-webkit-scrollbar-thumb': {
      background: '#666666',
      borderRadius: 4,
    },
  },
  closeIcon: {
    color: theme.ptas.colors.theme.black,
    width: 20,
  },
  closeButton: {
    position: 'absolute',
    right: '12px !important',
    top: '8px !important',
    cursor: 'pointer',
  },
  infoGrid: {
    display: 'grid',
    gridTemplateColumns: '25% 25% 25% 25%',
    columnGap: theme.spacing(1),
  },
  infoItem: {
    display: 'flex',
    flexDirection: 'column',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    marginBottom: '21px',
    marginRight: '16px',
    padding: '0 8px 4px',
    borderRadius: '3px',

    '&>span:nth-child(1)': {
      color: theme.ptas.colors.theme.grayMedium,
      fontSize: theme.ptas.typography.finePrintBold.fontSize,
      fontWeight: theme.ptas.typography.finePrintBold.fontWeight,
      lineHeight: '12px',
      marginBottom: '7px',
    },
    '&>span:nth-child(2)': {
      color: theme.ptas.colors.theme.black,
      fontSize: theme.ptas.typography.bodySmallBold.fontSize,
      fontWeight: theme.ptas.typography.bodySmallBold.fontWeight,
      lineHeight: '19px',
    },
  },
  highlight: {
    backgroundColor: theme.ptas.colors.utility.changed,
  },
  assetsList: {
    gridColumn: 'span 4',
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    width: '100%',
    margin: theme.spacing(2, 0),
    gridColumn: 'span 4',
  },
}));

function UpdatedInfoTable(props: Props): JSX.Element {
  const classes = useStyles();
  const {
    initialBusiness,
    updatedBusiness,
    propertyType,
    stateOrProvince,
    initialAssets,
    assetsList,
  } = useUpdateBusiness();

  const { onClose, open } = props;
  const [businessOld, setBusinessOld] = useState<Business | undefined>();
  const [businessNew, setBusinessNew] = useState<Business | undefined>();

  useEffect(() => {
    if (!props.businessOld) {
      setBusinessOld({
        ownerName: 'WTCWEND',
        businessName: 'Auburn Center Dry Cleaning',
        ubiNumber: '897651435',
        naicsNumber: '001',
        businessType: 'Sole proprietor',
        stateOfIncorporation: 'TN - Tennessee',
        contactName: 'Kimberly',
        attention: '-',
        email: 'kbass@smsholdings.com',
        phone: '615-799-1535',
        mailingAddress: {
          title: '615-799-1535',
          city: 'Auburn',
          state: 'WA',
          zip: '98002',
        },
        locationAddress: {
          title: '615-799-1535',
          city: 'Auburn',
          state: 'WA',
          zip: '98002',
        },
        assets: [],
      });
    }

    if (!props.businessNew) {
      setBusinessNew({
        ownerName: '__WTCWEND',
        businessName: 'Auburn Center Dry Cleaning',
        ubiNumber: '897651435',
        naicsNumber: '001',
        businessType: 'Sole proprietor',
        stateOfIncorporation: 'TN - Tennessee',
        contactName: 'Kimberly',
        attention: '-',
        email: 'kbass@smsholdings.com',
        phone: '*615-799-1535',
        mailingAddress: {
          title: '615-799-1535',
          city: 'Auburn',
          state: 'WA',
          zip: '98002',
        },
        locationAddress: {
          title: '615-799-1535',
          city: 'Auburn',
          state: 'WA',
          zip: '98002',
        },
        assets: [],
      });
    }
  }, [props.businessOld, props.businessNew]);

  const getItemClasses = (oldVal: unknown, newVal: unknown): string => {
    return clsx(classes.infoItem, oldVal !== newVal ? classes.highlight : '');
  };

  const getPropertyTypeLabel = (): string => {
    return (
      propertyType.find(
        (type) =>
          parseInt(type.value.toString()) === updatedBusiness.propertyType
      )?.label ?? ''
    );
  };

  const getStateLabel = (): string => {
    return (
      stateOrProvince.find(
        (type) => type.value === updatedBusiness.stateOfIncorporationId
      )?.label ?? ''
    );
  };

  const handleChangeUpdateBusiness = async (): Promise<void> => {
    await businessApiService.updateBusiness(initialBusiness, updatedBusiness);
    saveAssets();
  };

  const saveAssets = async (): Promise<void> => {
    const oldAssetsIds = initialAssets.map((ia) => ia.id);

    /**
     * create promise array for assets that have been updated
     */
    const assetsUpdated = assetsList.map((a) => {
      const oldAsset = initialAssets.find((ia) => ia.id === a.id);

      if (!oldAsset || JSON.stringify(oldAsset) === JSON.stringify(a))
        return null;

      return businessApiService.updateAssetBusiness(oldAsset, a);
    });

    // array of new assets
    const newAssets = assetsList.filter((al) => !oldAssetsIds.includes(al.id));

    // save the request to create new assets to be used in promise.all
    const newAssetsPromise = businessApiService.createAssetBusiness(newAssets);

    Promise.all([...assetsUpdated, newAssetsPromise]).catch(() => {
      console.log('error to save assets');
    });
  };

  return (
    <Modal
      open={open}
      onClose={onClose}
      aria-labelledby="simple-modal-title"
      aria-describedby="simple-modal-description"
      className={classes.modal}
    >
      <Fragment>
        <Box className={classes.headerContent}>
          <Box className={classes.headerText}>{fm.headerText}</Box>
          <CustomButton
            classes={{ root: classes.headerButton }}
            onClick={handleChangeUpdateBusiness}
          >
            {fm.headerButtonText}
          </CustomButton>
        </Box>
        <Box className={clsx(classes.infoGrid, classes.paper)}>
          <Box onClick={onClose} className={classes.closeButton}>
            <Close classes={{ root: classes.closeIcon }} />
          </Box>
          <Fragment>
            <Box
              className={getItemClasses(
                initialBusiness?.preparerName,
                updatedBusiness?.preparerName
              )}
            >
              <span>{fm.ownerName}</span>
              <span>{updatedBusiness?.preparerName}</span>
            </Box>
            <Box
              className={getItemClasses(
                initialBusiness?.businessName,
                updatedBusiness?.businessName
              )}
            >
              <span>{fm.businessName}</span>
              <span>{updatedBusiness?.businessName}</span>
            </Box>
            <Box
              className={getItemClasses(
                initialBusiness?.ubi,
                updatedBusiness?.ubi
              )}
            >
              <span>{fm.ubiNumber}</span>
              <span>{updatedBusiness?.ubi}</span>
            </Box>
            <Box
              className={getItemClasses(
                initialBusiness?.naicsNumber,
                updatedBusiness?.naicsNumber
              )}
            >
              <span>{fm.naicsNumber}</span>
              <span>{updatedBusiness?.naicsDescription}</span>
            </Box>
          </Fragment>
          <Fragment>
            <Box
              className={getItemClasses(
                initialBusiness?.propertyType,
                updatedBusiness?.propertyType
              )}
            >
              <span>{fm.businessType}</span>
              <span>{getPropertyTypeLabel()}</span>
            </Box>
            <Box
              className={getItemClasses(
                initialBusiness?.stateOfIncorporationId,
                updatedBusiness?.stateOfIncorporationId
              )}
            >
              <span>{fm.stateOfIncorporation}</span>
              <span>{getStateLabel()}</span>
            </Box>
            <Box></Box>
            <Box></Box>
          </Fragment>
          <span className={classes.border}></span>
          <Fragment>
            <Box
              className={getItemClasses(
                businessOld?.contactName,
                businessNew?.contactName
              )}
            >
              <span>{fm.contactName}</span>
              <span>{businessNew?.contactName}</span>
            </Box>
            <Box
              className={getItemClasses(
                initialBusiness?.preparerAttention,
                updatedBusiness?.preparerAttention
              )}
            >
              <span>{fm.attention}</span>
              <span>{updatedBusiness?.preparerAttention}</span>
            </Box>
            <Box
              className={getItemClasses(
                updatedBusiness?.preparerEmail,
                initialBusiness?.preparerEmail
              )}
            >
              <span>{fm.email}</span>
              <span>{updatedBusiness?.preparerEmail}</span>
            </Box>
            <Box
              className={getItemClasses(
                initialBusiness?.preparerCellphone,
                updatedBusiness?.preparerCellphone
              )}
            >
              <span>{fm.phone}</span>
              <span>{updatedBusiness?.preparerCellphone}</span>
            </Box>
          </Fragment>
          <span className={classes.border}></span>
          <Fragment>
            <Box
              className={getItemClasses(
                businessOld?.mailingAddress.title,
                businessNew?.mailingAddress.title
              )}
            >
              <span>{fm.mailingAddress}</span>
              <span>{businessNew?.mailingAddress.title}</span>
            </Box>
            <Box
              className={getItemClasses(
                initialBusiness.preparerCityId,
                updatedBusiness.preparerCityId
              )}
            >
              <span>{fm.city}</span>
              <span>{updatedBusiness.preparerCityLabel}</span>
            </Box>
            <Box
              className={getItemClasses(
                initialBusiness.preparerStateId,
                updatedBusiness.preparerStateId
              )}
            >
              <span>{fm.state}</span>
              <span>{updatedBusiness.preparerStateLabel}</span>
            </Box>
            <Box
              className={getItemClasses(
                initialBusiness.preparerZipCodeId,
                updatedBusiness.preparerZipCodeId
              )}
            >
              <span>{fm.zip}</span>
              <span>{updatedBusiness.preparerZipcodeLabel}</span>
            </Box>
          </Fragment>
          <span className={classes.border}></span>
          <Fragment>
            <Box
              className={getItemClasses(
                businessOld?.locationAddress.title,
                businessNew?.locationAddress.title
              )}
            >
              <span>{fm.locationAddress}</span>
              <span>{businessNew?.locationAddress.title}</span>
            </Box>
            <Box
              className={getItemClasses(
                updatedBusiness?.cityId,
                initialBusiness.cityId
              )}
            >
              <span>{fm.city}</span>
              <span>{updatedBusiness.addrCityLabel}</span>
            </Box>
            <Box
              className={getItemClasses(
                updatedBusiness.stateId,
                initialBusiness.stateId
              )}
            >
              <span>{fm.state}</span>
              <span>{updatedBusiness.addrStateLabel}</span>
            </Box>
            <Box
              className={getItemClasses(
                updatedBusiness.zipId,
                initialBusiness.zipId
              )}
            >
              <span>{fm.zip}</span>
              <span>{updatedBusiness.addrZipcodeLabel}</span>
            </Box>
          </Fragment>
          <span className={classes.border}></span>
          <Box className={clsx(classes.infoItem, classes.assetsList)}>
            <span>{fm.assets}</span>
            <AssetsList />
          </Box>
        </Box>
      </Fragment>
    </Modal>
  );
}

export default UpdatedInfoTable;
