// More.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext } from 'react';
import { makeStyles, Theme } from '@material-ui/core';
import * as fm from '../../routes/models/Views/Home/formatText';
import {
  CardWithActions,
  CustomCardAction,
} from '@ptas/react-public-ui-library';
import { useHistory } from 'react-router-dom';
import { AppContext } from 'contexts/AppContext';
import {
  STATE_CODE_ACTIVE,
  STATE_CODE_INACTIVE,
  STATUS_CODE_ASSIGNED,
  STATUS_CODE_CANCELED,
  STATUS_CODE_COMPLETED,
  STATUS_CODE_NOT_STARTED,
  STATUS_CODE_PENDING_MORE_INFO,
  STATUS_CODE_UNDER_REVIEW,
} from 'routes/models/Views/HomeDestroyedProperty/constants';
import {
  STATE_CODE_ACTIVE as HI_STATE_CODE_ACTIVE,
  STATE_CODE_INACTIVE as HI_STATE_CODE_INACTIVE,
  STATUS_CODE_APP_CREATED as HI_STATUS_CODE_APP_CREATED,
  STATUS_CODE_UNSUBMITTED as HI_STATUS_CODE_UNSUBMITTED,
  STATUS_CODE_COMPLETE as HI_STATUS_CODE_COMPLETE,
} from 'routes/models/Views/HomeImprovement/constants';
import {
  APP_STATE_IN_PROGRESS as CURRENT_USE_IN_PROGRESS,
  APP_STATE_NEW as CURRENT_USE_NEW,
} from 'routes/models/Views/CurrentUse/constants';
import useManageProperty from 'routes/models/Views/Home/useManageProperty';

interface Props {
  test?: string;
}

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    fontFamily: theme.ptas.typography.bodyFontFamily,
    width: '100%',
    maxWidth: 604,
    textAlign: 'center',
  },
  title: {
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    marginBottom: 16,
    display: 'block',
  },
  cardWrapper: {
    display: 'flex',
    justifyContent: 'space-between',
    width: '100%',
    maxWidth: 569,
    margin: '0 auto',
    flexDirection: 'column',
    alignItems: 'center',

    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
    },
  },
  cardMoreOptionsWrapper: {
    display: 'flex',
    justifyContent: 'space-between',
    width: '100%',

    flexDirection: 'column',
    alignItems: 'center',

    [theme.breakpoints.up('sm')]: {
      flexDirection: 'row',
    },
  },
  card: {
    width: 216,
  },
  cardRootWrap: {
    boxSizing: 'border-box',
    marginBottom: 8,

    '&:last-child': {
      marginBottom: 0,
    },

    [theme.breakpoints.up('sm')]: {
      marginBottom: 0,
    },
  },
  businessPersonalCard: {
    width: 214,
    boxSizing: 'border-box',
    marginBottom: 8,
    [theme.breakpoints.up('sm')]: {
      marginBottom: 0,
    },
  },
}));

function More(props: Props): JSX.Element {
  const classes = useStyles(props);
  const history = useHistory();

  const { portalContact } = useContext(AppContext);
  const {
    selectedParcels,
    stateCodeDestroyProperty,
    statusCodeDestroyedProperty,
    destroyedPropertyApp,
    homeImprovementApp,
    currentUseApp,
    stateCodeHI,
    statusCodeHI,
    currentUseAppState,
  } = useManageProperty();

  //#region home improvement card
  const renderHomeImprovementApp = (): JSX.Element => {
    if (!homeImprovementApp.isSaved) {
      //This means that no HI application with the required conditions was found
      return (
        <CardWithActions
          shadow={false}
          title={fm.homeImprovement}
          text={fm.homeImprovementDes}
          actions={[{ text: 'Apply', type: 'button', buttonStyle: 'default' }]}
          classes={{
            rootWrap: classes.cardRootWrap,
          }}
          onActionClick={handleActionClickHI}
        />
      );
    }

    return (
      <CardWithActions
        shadow={false}
        title={fm.homeImprovement}
        text={fm.homeImprovementDes}
        actions={showControlsByStatusHI()}
        classes={{
          rootWrap: classes.cardRootWrap,
        }}
        onActionClick={handleActionClickHI}
      />
    );
  };

  const handleActionClickHI = (action: CustomCardAction): void => {
    if (action.type !== 'button') return;

    if (action.text === 'Apply') {
      history.push(
        `/home-improvement/${portalContact?.id}/${homeImprovementApp.homeImprovementId}/new/${selectedParcels}`
      );
      return;
    }

    history.push(
      `/home-improvement/${portalContact?.id}/${homeImprovementApp.homeImprovementId}`
    );
  };

  const showControlsByStatusHI = (): CustomCardAction[] => {
    if (
      stateCodeHI.get(HI_STATE_CODE_ACTIVE) === homeImprovementApp.stateCode
    ) {
      return stateCodeActiveHI();
    }
    if (
      stateCodeHI.get(HI_STATE_CODE_INACTIVE) === homeImprovementApp.stateCode
    ) {
      return stateCodeInactiveHI();
    }
    return [];
  };

  const stateCodeActiveHI = (): CustomCardAction[] => {
    let actions: CustomCardAction[] = [];

    //If existing HI application belongs to another contact
    if (
      homeImprovementApp.portalContactId &&
      homeImprovementApp.portalContactId !== portalContact?.id
    ) {
      // if (
      //   statusCodeHI.get(HI_STATUS_CODE_UNSUBMITTED) ===
      //   homeImprovementApp.statusCode
      // ) {
      actions = [
        {
          text: 'Active by another user',
          type: 'label',
          labelStyle: 'success',
          buttonStyle: 'text',
        },
      ];
      // }
    } else {
      //Existing HI application belongs to current contact
      if (
        statusCodeHI.get(HI_STATUS_CODE_UNSUBMITTED) ===
          homeImprovementApp.statusCode ||
        //TODO: TEMP checking status 591500014 as UNSUBMITTED
        //Pending change of status codes on DB
        homeImprovementApp.statusCode === 591500014
      ) {
        actions = [
          {
            text: 'Continue',
            type: 'button',
            labelStyle: 'success',
            buttonStyle: 'default',
          },
        ];
      } else if (
        homeImprovementApp.statusCode ===
          statusCodeHI.get(HI_STATUS_CODE_APP_CREATED) ||
        homeImprovementApp.statusCode ===
          statusCodeHI.get(HI_STATUS_CODE_COMPLETE)
      ) {
        actions = [
          {
            text: 'Applied',
            type: 'button',
            labelStyle: 'success',
            buttonStyle: 'text',
          },
        ];
      } /*else if (
      statusCodeDP.get(STATUS_CODE_PENDING_MORE_INFO) ===
      destroyedPropertyApp.statusCode
    ) {
      actions = [
        {
          text: 'More info requested',
          type: 'button',
          labelStyle: 'warning',
          buttonStyle: 'text',
        },
      ];
    }*/
    }
    return actions;
  };

  const stateCodeInactiveHI = (): CustomCardAction[] => {
    let actions: CustomCardAction[] = [];

    if (
      statusCodeHI.get(HI_STATUS_CODE_COMPLETE) ===
      homeImprovementApp.statusCode
    ) {
      actions = [
        {
          text: 'Complete',
          type: 'label',
          labelStyle: 'success',
        },
      ];
    } else {
      actions = [
        {
          text: 'Rejected',
          type: 'label',
          labelStyle: 'error',
        },
      ];
    }

    return actions;
  };

  //#endregion

  const stateCodeActive = (): CustomCardAction[] => {
    let actions: CustomCardAction[] = [];

    if (
      statusCodeDestroyedProperty.get(STATUS_CODE_NOT_STARTED) ===
      destroyedPropertyApp.statusCode
    ) {
      actions = [
        {
          text: 'Continue',
          type: 'button',
          labelStyle: 'success',
          buttonStyle: 'default',
        },
      ];
    } else if (
      statusCodeDestroyedProperty.get(STATUS_CODE_ASSIGNED) ===
        destroyedPropertyApp.statusCode ||
      statusCodeDestroyedProperty.get(STATUS_CODE_UNDER_REVIEW) ===
        destroyedPropertyApp.statusCode
    ) {
      actions = [
        {
          text: 'Applied',
          type: 'button',
          labelStyle: 'success',
          buttonStyle: 'text',
        },
      ];
    } else if (
      statusCodeDestroyedProperty.get(STATUS_CODE_PENDING_MORE_INFO) ===
      destroyedPropertyApp.statusCode
    ) {
      actions = [
        {
          text: 'More info requested',
          type: 'button',
          labelStyle: 'warning',
          buttonStyle: 'text',
        },
      ];
    }

    return actions;
  };

  const stateCodeInactive = (): CustomCardAction[] => {
    let actions: CustomCardAction[] = [];

    if (
      statusCodeDestroyedProperty.get(STATUS_CODE_COMPLETED) ===
      destroyedPropertyApp.statusCode
    ) {
      actions = [
        {
          text: 'Complete',
          type: 'label',
          labelStyle: 'success',
        },
      ];
    }

    if (
      statusCodeDestroyedProperty.get(STATUS_CODE_CANCELED) ===
      destroyedPropertyApp.statusCode
    ) {
      actions = [
        {
          text: 'Rejected',
          type: 'label',
          labelStyle: 'error',
        },
      ];
    }

    return actions;
  };

  const showControlByStatus = (): CustomCardAction[] => {
    if (
      stateCodeDestroyProperty.get(STATE_CODE_ACTIVE) ===
      destroyedPropertyApp.stateCode
    )
      return stateCodeActive();

    if (
      stateCodeDestroyProperty.get(STATE_CODE_INACTIVE) ===
      destroyedPropertyApp.stateCode
    )
      return stateCodeInactive();

    return [];
  };

  const renderPropertyApp = (): JSX.Element => {
    return (
      <CardWithActions
        shadow={false}
        title={fm.destroyProperty}
        text={fm.destroyPropertyDesc}
        actions={
          showControlByStatus().length
            ? showControlByStatus()
            : [{ text: 'Apply', type: 'button', buttonStyle: 'default' }]
        }
        classes={{
          rootWrap: classes.cardRootWrap,
        }}
        onActionClick={handleActionClick}
      />
    );
  };

  const handleActionClick = (action: CustomCardAction): void => {
    if (action.type !== 'button') return;

    if (action.text === 'Apply') {
      return history.push(
        `/home-destroyed-property/${portalContact?.id}/${destroyedPropertyApp.id}`,
        {
          parcelId: selectedParcels,
        }
      );
    }

    history.push(
      `/home-destroyed-property/${portalContact?.id}/${destroyedPropertyApp.id}`
    );
  };

  //#endregion destroyed property card

  //# region Current use card

  const handleActionClickCurrentUse = (action: CustomCardAction): void => {
    if (action.type !== 'button') return;

    if (action.text === 'Apply') {
      history.push(`/current-use/`);
      return;
    }

    history.push(`/current-use/${selectedParcels}`);
  };

  const showCurrentUseControlsByStatus = (): CustomCardAction[] => {
    let actions: CustomCardAction[] = [];

    if (
      currentUseAppState.get(CURRENT_USE_IN_PROGRESS) ===
      currentUseApp?.statuscode
    ) {
      actions = [
        {
          text: 'Continue',
          type: 'button',
          labelStyle: 'success',
          buttonStyle: 'default',
        },
      ];
    } else if (
      currentUseAppState.get(CURRENT_USE_NEW) === currentUseApp?.statuscode
    ) {
      actions = [
        {
          text: 'Applied',
          type: 'button',
          labelStyle: 'success',
          buttonStyle: 'text',
        },
      ];
    }

    return actions;
  };

  const renderCurrentUseApp = (): JSX.Element => {
    if (!currentUseApp) {
      return (
        <CardWithActions
          shadow={false}
          title={fm.currentUse}
          text={fm.currentUseDes}
          actions={[{ text: 'Apply', type: 'button', buttonStyle: 'default' }]}
          classes={{
            rootWrap: classes.cardRootWrap,
          }}
          onActionClick={handleActionClickCurrentUse}
        />
      );
    }

    return (
      <CardWithActions
        shadow={false}
        title={fm.currentUse}
        text={fm.currentUseDes}
        actions={showCurrentUseControlsByStatus()}
        classes={{
          rootWrap: classes.cardRootWrap,
        }}
        onActionClick={handleActionClickCurrentUse}
      />
    );
  };

  //# end region Current use card

  const MainCard = (
    <Fragment>
      <CardWithActions
        shadow={false}
        title={fm.seniorExemption}
        text={fm.seniorExemptionDesc}
        actions={[{ text: 'Apply', type: 'button', buttonStyle: 'default' }]}
        classes={{
          rootWrap: classes.cardRootWrap,
        }}
        onActionClick={handleActionClick}
      />
      {renderHomeImprovementApp()}
      {renderCurrentUseApp()}
    </Fragment>
  );

  const MoreOptionCards = (
    <Fragment>
      <CardWithActions
        shadow={false}
        title={fm.propertyValueAppeal}
        text={fm.propertyValueAppealDesc}
        actions={[
          { text: 'Applied', type: 'label', labelStyle: 'success' },
          { text: 'View', type: 'button', buttonStyle: 'text' },
        ]}
        classes={{
          rootWrap: classes.cardRootWrap,
        }}
      />
      <CardWithActions
        shadow={false}
        title={fm.businessPersonalProperty}
        text={fm.businessPersonalPropertyDes}
        actions={[{ text: 'Manage', type: 'button', buttonStyle: 'default' }]}
        classes={{
          rootWrap: classes.businessPersonalCard,
        }}
        onActionClick={handleActionClick}
      />
      {renderPropertyApp()}
    </Fragment>
  );

  return (
    <div className={classes.root}>
      <p className={classes.title}>{fm.moreTitle}</p>
      <div className={classes.cardWrapper}>{MainCard}</div>
      <p className={classes.title}>{fm.moreTitle}</p>
      <div className={classes.cardMoreOptionsWrapper}>{MoreOptionCards}</div>
    </div>
  );
}

export default More;
