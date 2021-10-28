// Home.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect } from 'react';
import {
  CustomCard,
  CustomTextButton,
  ItemSuggestion,
  PropertySearcher,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import { useHistory } from 'react-router-dom';
import * as fm from './formatText';
import * as fmGeneral from '../../../../GeneralFormatMessage';
import CardPropertyInfo from '../../../../components/CardPropertyInfo';
import { apiService } from 'services/api/apiService';
import { useUpdateEffect } from 'react-use';
import { AppContext } from '../../../../contexts/AppContext';
import { withManagePropertyProvider } from 'contexts/ManageProperty';
import ProfileOptionsButton from 'components/ProfileOptionsButton';
import useManageProperty from './useManageProperty';
import { PropertyDescription } from 'services/map/model/parcel';

const useStyles = makeStyles((theme: Theme) => ({
  card: {
    width: '100%',
    display: 'block',
    boxSizing: 'border-box',
    fontFamily: theme.ptas.typography.bodyFontFamily,
    paddingTop: 8,
    marginBottom: 15,
    maxWidth: 702,
    marginLeft: 'auto',
    marginRight: 'auto',
    overflow: 'visible',
    paddingBottom: 24,
    marginTop: 0,
    borderRadius: 0,

    [theme.breakpoints.up('sm')]: {
      marginTop: 8,
      borderRadius: 24,
    },
  },
  contentWrap: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
    width: '100%',
    maxWidth: 800,
    margin: '0 auto',
  },
  title: {
    fontFamily: theme.ptas.typography.titleFontFamily,
    fontSize: theme.ptas.typography.h6.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    width: 228,
    margin: 0,
    [theme.breakpoints.up('sm')]: {
      fontSize: theme.ptas.typography.h5.fontSize,
      width: 292,
    },
  },
  helpText: {
    fontSize: theme.ptas.typography.body.fontSize,
    fontWeight: theme.ptas.typography.bodyBold.fontWeight,
    marginBottom: 9,
    color: theme.ptas.colors.theme.black,
  },
  link: {
    display: 'block',
    fontSize: theme.ptas.typography.body.fontSize,
    color: theme.ptas.colors.theme.gray,
    marginBottom: 4,

    '&:hover': {
      color: theme.ptas.colors.theme.gray,
    },
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    width: '100%',
    maxWidth: 320,
    marginBottom: 8,
    marginTop: 32,
    display: 'block',
  },
  marginBottom: {
    marginBottom: 32,
  },
  propertyInfoRoot: {
    width: '100%',
    maxWidth: 668,
  },
  propertyInfoContent: {},
  textButtonRoot: {
    fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
  },
  header: {
    width: '100%',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    marginBottom: 32,
  },
  signInButton: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
  },
}));

interface JsonToSave {
  portalContactId: string;
  parcelItemSelected: PropertyDescription[];
  selectedParcels: string;
}

export interface ManagePropertyState {
  createFromHome: boolean;
}

function Home(): JSX.Element {
  const classes = useStyles();
  const history = useHistory();
  const {
    setParcelDetails,
    setSelectedParcels,
    fetchCountries,
    loadStates,
    selectedParcels,
    setParcelSuggestions,
    parcelSuggestions,
    valueParcelSearch,
    setValueParcelSearch,
    loadingParcel,
    setLoadingParcel,
    parcelItemSelected,
    setParcelItemSelected,
    loadDestroyedPropertyApp,
    loadHomeImprovementApps,
    loadCurrentUseApp,
  } = useManageProperty();

  const { mediaToken, portalContact } = useContext(AppContext);
  const jsonParcelListUrl = `portals/manageProperty/${portalContact?.id ??
    ''}/parcelList`;

  useEffect(() => {
    fetchCountries && fetchCountries();
    loadStates();

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (!portalContact) return;
    getJsonParcels();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [portalContact]);

  useEffect(() => {
    loadDestroyedPropertyApp();
    loadHomeImprovementApps();
    loadCurrentUseApp();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedParcels]);

  useEffect(() => {
    // update context data
    setParcelDetails && setParcelDetails(parcelItemSelected);
    saveParcelListByContact();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [parcelItemSelected]);

  const getJsonParcels = async (): Promise<void> => {
    const { data, hasError } = await apiService.getJson(
      `${jsonParcelListUrl}/parcelList`
    );
    if (hasError || !data) return;
    //return data as unknown as JsonToSave;
    const json = (data as unknown) as JsonToSave;
    const parcelItemSelFound = json.parcelItemSelected ?? [];
    const parcelSel =
      json.selectedParcels ?? parcelItemSelFound[0]?.ptas_parceldetailid ?? '';
    setSelectedParcels(parcelSel);
    setParcelItemSelected(parcelItemSelFound);
  };

  const saveParcelListByContact = (): void => {
    if (!portalContact) return;
    const jsonToSave = {
      portalContactId: portalContact.id,
      parcelItemSelected,
      selectedParcels,
    } as JsonToSave;
    apiService.saveJson(jsonParcelListUrl, jsonToSave);
  };

  const handleClick = (): void => {
    history.push('/instruction');
  };

  const handleOnChange = (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ): void => {
    const inputValue = e.currentTarget.value;

    if (inputValue.length < 2) return setLoadingParcel(false);

    setLoadingParcel(true);
    setValueParcelSearch(inputValue);
  };

  /**
   * api service
   */
  const getData = async (valueData: string): Promise<void> => {
    const { data, hasError } = await apiService.getParcel(valueData);

    if (hasError) {
      return setParcelSuggestions([]);
    }

    if (!data || !data.length) return;

    const itemSuggestionData = data?.map(propertyItem => {
      return {
        title: propertyItem.ptas_address,
        id: propertyItem.ptas_parceldetailid,
        subtitle: `${
          propertyItem.ptas_district
        }, WA ${propertyItem.ptas_zipcode ?? ''}`,
      };
    });

    setLoadingParcel(false);

    setParcelSuggestions(itemSuggestionData ?? []);
  };

  const handleSelectedSearcher = async (
    item: ItemSuggestion
  ): Promise<void> => {
    if (!item.id) return;

    const selectedCard = parcelItemSelected?.find(
      itemPropDesc => itemPropDesc.ptas_parceldetailid === (item.id as string)
    );

    setSelectedParcels(item.id as string);

    if (selectedCard) return;

    const { data: selectedCardInfo } = await apiService.getParcelDescription({
      entityId: item.id as string,
      entityName: 'ptas_parceldetail',
    });

    if (!selectedCardInfo) return;

    // get parcel photo
    const { data: imageParcel } = await apiService.getMediaForParcel(
      selectedCardInfo.ptas_name,
      mediaToken
    );

    const photo = imageParcel?.length ? imageParcel[0] : '';

    setParcelItemSelected(prevState => {
      // eslint-disable-next-line @typescript-eslint/camelcase
      return [...prevState, { ...selectedCardInfo, ptas_photo: photo }];
    });
  };

  const handleClickCardSelected = (cardId: string): void => {
    if (selectedParcels === cardId) return;

    setSelectedParcels && setSelectedParcels(cardId);
  };

  const handleRemove = (id: string): void => {
    const newList = parcelItemSelected.filter(
      card => card.ptas_parceldetailid !== id
    );

    setParcelItemSelected(newList);
  };

  const renderCardPropertyInfo = (): JSX.Element[] | void => {
    if (!parcelItemSelected?.length) return;

    return parcelItemSelected.map(item => {
      return (
        <CardPropertyInfo
          imageSrc={item.ptas_photo}
          key={`${item.ptas_name}-${item.ptas_parceldetailid}`}
          id={item.ptas_parceldetailid}
          parcelNumber={item.ptas_name}
          propertyName={item.ptas_namesonaccount}
          propertyAddress={`${item.ptas_address} ${
            item.ptas_district
          }, WA ${item.ptas_zipcode ?? ''}`}
          mailName={item.ptas_namesonaccount}
          mailAddress={`${item.ptas_address} ${
            item.ptas_district
          }, WA ${item.ptas_zipcode ?? ''}`}
          onClick={handleClickCardSelected}
          selected={selectedParcels === item.ptas_parceldetailid}
          remove={handleRemove}
        />
      );
    });
  };

  const renderBorder = (): JSX.Element => {
    if (!parcelItemSelected?.length) return <Fragment />;

    return (
      <span className={`${classes.border} ${classes.marginBottom}`}></span>
    );
  };

  const handleClickLocateMe = (): void => {
    setValueParcelSearch('124');
  };

  useUpdateEffect(() => {
    getData(valueParcelSearch);
  }, [valueParcelSearch]);

  return (
    <MainLayout>
      <CustomCard
        variant="wrapper"
        classes={{
          rootWrap: classes.card,
        }}
      >
        <div className={classes.header}>
          <h2 className={classes.title}>{fm.title}</h2>
          <ProfileOptionsButton />
        </div>

        <div className={classes.contentWrap}>
          {renderCardPropertyInfo()}
          {renderBorder()}

          <PropertySearcher
            label={fm.findMyProperty}
            textButton={fm.locateMe}
            textDescription={fm.enterAnAddress}
            onChange={handleOnChange}
            value={valueParcelSearch}
            onChangeDelay={500}
            suggestion={{
              List: parcelSuggestions,
              onSelected: handleSelectedSearcher,
              loading: !!valueParcelSearch && loadingParcel,
            }}
            onClickTextButton={handleClickLocateMe}
          />
          <span className={classes.border}></span>
          <span className={classes.helpText}>{fmGeneral.help}</span>
          <CustomTextButton onClick={handleClick}>
            {fmGeneral.instruction}
          </CustomTextButton>
        </div>
      </CustomCard>
    </MainLayout>
  );
}

export default withManagePropertyProvider(Home);
