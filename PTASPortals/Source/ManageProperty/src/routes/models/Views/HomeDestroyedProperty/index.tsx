// Home.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, Fragment, useContext, useEffect } from 'react';
import {
  CustomCard,
  CustomTextButton,
  ItemSuggestion,
  PropertySearcher,
} from '@ptas/react-public-ui-library';
import { makeStyles, Theme } from '@material-ui/core';
import MainLayout from '../../../../components/Layouts/MainLayout';
import { useHistory, useParams } from 'react-router-dom';
import * as fm from './formatText';
import * as generalFm from '../../../../GeneralFormatMessage';
import SimpleCardPropertyInfo, {
  DestroyedPropertyParams,
} from './SimpleCardPropertyInfo';
import { useUpdateEffect } from 'react-use';
import { AppContext } from '../../../../contexts/AppContext';
import { withDestroyedPropProvider } from '../../../../contexts/DestroyedProperty';
import {
  STATE_CODE_INACTIVE,
  STATE_CODE_ACTIVE,
  STATUS_CODE_PENDING_MORE_INFO,
  STATUS_CODE_NOT_STARTED,
} from './constants';
import ProfileOptionsButton from 'components/ProfileOptionsButton';
import { useLocation } from 'react-router-dom';
import useDestroyedProperty from './useDestroyedProperty';
import { Task } from 'routes/models/Views/HomeDestroyedProperty/Task';

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
    marginTop: 0,
    borderRadius: 0,

    [theme.breakpoints.up('sm')]: {
      borderRadius: 24,
      marginTop: 8,
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
    fontSize: theme.ptas.typography.h6.fontSize,
    fontFamily: theme.ptas.typography.titleFontFamily,
    margin: 0,
    marginBottom: 32,
    fontWeight: theme.ptas.typography.bodyExtraBold.fontWeight,
    width: 188,

    [theme.breakpoints.up('sm')]: {
      width: '100%',
      fontSize: theme.ptas.typography.h5.fontSize,
    },
  },
  textHelp: {
    fontSize: theme.ptas.typography.body.fontSize,
    fontWeight: theme.ptas.typography.bodyBold.fontWeight,
    marginBottom: 16,
  },
  name: {
    position: 'absolute',
    top: 10,
    right: 16,
  },
  border: {
    background:
      'linear-gradient(90deg, rgba(0, 0, 0, 0) 0%, #000000 49.48%, rgba(0, 0, 0, 0) 100%)',
    height: 1,
    width: '100%',
    marginBottom: 8,
    marginTop: 32,
    display: 'block',
    maxWidth: 310,
  },
  marginBottom: {
    marginBottom: 32,
  },
  description: {
    fontSize: theme.ptas.typography.bodySmall.fontSize,
    maxWidth: 310,
    width: '100%',
    marginTop: 16.72,
    display: 'block',
  },
  cardPropertyInfo: {
    width: 623,
    marginBottom: theme.spacing(4),
  },
  cardPropertyInfoContent: {
    background: 'rgba(0,0,0,0)',
  },
  cardPropertyInfoChildrenContent: {
    background: 'rgba(255, 255, 255, 0.5)',
  },
  cardChildren: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
  },
  signAndFinalizeButton: {
    width: 191,
    height: 38,
  },
}));

interface ManagePropertyState {
  parcelId?: string;
}

function HomeDestroyedProperty(): JSX.Element {
  const classes = useStyles();
  const {
    destroyedPropertyList,
    createInitialTaskApp,
    loadDestroyedPropertyApps,
    task,
    setTask,
    setOldDesPropertyApp,
    statusCode,
    stateCode,
    loadInitialFilesByDestroyedProp,
    loadStateStatusCode,
    parcelInfo,
    getParcelInfo,
    loadFileLibrary,
    initialApps,
  } = useDestroyedProperty();

  const { state } = useLocation<ManagePropertyState>();
  const { portalContact } = useContext(AppContext);
  const { destroyedPropertyId } = useParams<DestroyedPropertyParams>();

  const [list, setList] = useState<ItemSuggestion[]>([]);
  const [value, setValue] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(true);

  const history = useHistory();

  const handleClick = (): void => {
    history.push('/instruction');
  };

  const handleOnChange = (
    e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>
  ): void => {
    const inputValue = e.currentTarget.value;

    setValue(inputValue);
  };

  useUpdateEffect(() => {
    const getInfo = async (): Promise<void> => {
      const info = await getParcelInfo(value);

      if (!info) return;

      setLoading(info.isLoading);
      setList(info.list);
    };

    getInfo();
  }, [value]);

  const handleSelectedSearcher = async (
    parcelItem: ItemSuggestion
  ): Promise<void> => {
    if (!parcelItem.id) return;

    const existItem = destroyedPropertyList?.find(
      (itemSearcher): boolean => itemSearcher.parcelId === parcelItem.id
    );

    if (existItem && task?.parcelId === parcelItem.id) return;
    if (existItem) return setTask(existItem);

    createInitialTaskApp(parcelItem.id as string);
  };

  useUpdateEffect(() => {
    loadInitialFilesByDestroyedProp();
  }, [task?.id]);

  const getDateSigned = (date: string): Date | undefined => {
    if (!date) return;

    return new Date(date);
  };

  /**
   * Return true if task is completed
   *
   * @param task - destroyed property task
   * @returns boolean
   *
   */
  const handleCompleted = (task: Task): boolean => {
    if (
      !(
        task.stateCode === stateCode.get(STATE_CODE_ACTIVE) &&
        task.statusCode === statusCode.get(STATUS_CODE_NOT_STARTED)
      )
    ) {
      return true;
    }

    return false;
  };

  const getStateCodeText = (task: Task): JSX.Element | undefined => {
    const date = (
      getDateSigned(task?.dateSigned ?? '') ?? new Date()
    ).toLocaleDateString();

    if (
      task.stateCode === stateCode.get(STATE_CODE_ACTIVE) &&
      task.statusCode === statusCode.get(STATUS_CODE_PENDING_MORE_INFO)
    )
      return fm.pendingMoreInfoExemption(date);

    return fm.appliedExemption(date);
  };

  const renderCardPropertyInfo = (): JSX.Element[] | void => {
    if (!destroyedPropertyList.length) return;

    return destroyedPropertyList.map((cardItem) => {
      const completed = handleCompleted(cardItem);

      const parcelSelected = parcelInfo.find(
        (parcelItem) => parcelItem.ptas_parceldetailid === cardItem.parcelId
      );

      return (
        <SimpleCardPropertyInfo
          key={cardItem.id}
          onClick={(): void => {
            setTask(cardItem);

            const existApp =
              initialApps.find((af) => af.id === cardItem.id) || {};

            if (
              existApp &&
              JSON.stringify(existApp) === JSON.stringify(cardItem) &&
              !completed
            ) {
              setOldDesPropertyApp(cardItem);
            }
          }}
          selected={task?.id === cardItem.id}
          completed={completed}
          upperStripText={getStateCodeText(cardItem)}
          hideUploadMoreFileInput={
            task?.stateCode === stateCode.get(STATE_CODE_INACTIVE)
          }
          contactId={portalContact?.id}
          destroyedPropertyId={task?.id}
          id={cardItem.id}
          imageSrc={parcelSelected?.picture ?? ''}
          parcelNumber={parcelSelected?.ptas_name ?? ''}
          propertyAddress={parcelSelected?.ptas_address ?? ''}
          propertyName={parcelSelected?.ptas_namesonaccount ?? ''}
        />
      );
    });
  };

  const handleClickLocateMe = (): void => {
    setValue('124');
  };

  const renderPropertySearcher = (): JSX.Element => {
    const taskIncomplete = !!destroyedPropertyList.find(
      (exemptionItem) => !handleCompleted(exemptionItem)
    );

    if (typeof taskIncomplete === 'boolean' && taskIncomplete)
      return <Fragment />;

    return (
      <Fragment>
        {renderBorder()}
        <PropertySearcher
          label="Find my property"
          textButton="Locate me"
          textDescription="Enter an address, parcel #, or commercial project"
          onChange={handleOnChange}
          onChangeDelay={600}
          value={value}
          suggestion={{
            List: list,
            onSelected: handleSelectedSearcher,
            loading: !!value && loading,
          }}
          onClickTextButton={handleClickLocateMe}
        />
        <span className={classes.description}>{fm.description}</span>
      </Fragment>
    );
  };

  const renderBorder = (): JSX.Element => {
    if (!destroyedPropertyList?.length) return <Fragment />;

    return (
      <span className={`${classes.border} ${classes.marginBottom}`}></span>
    );
  };

  useEffect(() => {
    if (!portalContact?.id || !statusCode.size || !stateCode.size) return;

    loadDestroyedPropertyApps(destroyedPropertyId ?? '', state?.parcelId ?? '');

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [portalContact?.id, statusCode, stateCode]);

  useEffect(() => {
    console.log(`task changed`, task);
  }, [task]);

  useEffect(() => {
    loadStateStatusCode();
    loadFileLibrary();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <MainLayout>
      <CustomCard
        variant="wrapper"
        classes={{
          rootWrap: classes.card,
        }}
      >
        <h2 className={classes.title}>{fm.title}</h2>
        <div className={classes.contentWrap}>
          {renderCardPropertyInfo()}
          {renderPropertySearcher()}
          <span className={classes.border}></span>
          <span className={classes.textHelp}>{generalFm.help}</span>
          <CustomTextButton onClick={handleClick}>
            {generalFm.instruction}
          </CustomTextButton>
          <div className={classes.name}>
            <ProfileOptionsButton />
          </div>
        </div>
      </CustomCard>
    </MainLayout>
  );
}

export default withDestroyedPropProvider(HomeDestroyedProperty);
