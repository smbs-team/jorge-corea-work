// CustomCardPropertyInfo.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useRef } from 'react';
import {
  CardPropertyInfo,
  CustomPopover,
  Alert,
} from '@ptas/react-public-ui-library';

import { makeStyles, Theme } from '@material-ui/core';
import More from './More';
import UpdateMailingAddress from './UpdateMailingAddress';
import GoPaperless from './GoPaperless';
import { omit } from 'lodash';
import * as fm from '../../routes/models/Views/Home/formatText';
import * as generalFm from '../../GeneralFormatMessage';
import useManageProperty from 'routes/models/Views/Home/useManageProperty';

const useStyles = makeStyles((theme: Theme) => ({
  propertyInfoRoot: {
    width: '100%',
    maxWidth: 668,
    marginBottom: 8,
    overflow: 'visible',
  },
  propertyInfoContent: {},
  popoverRoot: {
    width: 304,
    padding: '16px 32px 23px 32px ',
    boxSizing: 'border-box',
  },
  alertText: {
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
    color: theme.ptas.colors.theme.white,
  },
  button: {
    width: 128,
    height: 38,
    marginLeft: 'auto',
    marginRight: 'auto',
    display: 'block',
    fontSize: theme.ptas.typography.body.fontSize,
    fontWeight: theme.ptas.typography.buttonLarge.fontWeight,
    padding: 0,
  },
  paragraph: {
    display: 'block',
    marginBottom: 15,
    fontFamily: theme.ptas.typography.bodyFontFamily,
    fontSize: theme.ptas.typography.bodyLarge.fontSize,
  },
}));

interface Props {
  selected?: boolean;
  imageSrc: string;
  id: string;
  parcelNumber: string;
  propertyName: string;
  propertyAddress: string;
  mailName?: string;
  mailAddress?: string;
  upperStripText?: string;
  onClick?: (id: string) => void;
  remove?: (id: string) => void;
}

function CustomCardPropertyInfo(props: Props): JSX.Element {
  const { selected, remove } = props;
  const {
    setPropertyInfoTabsValue,
    propertyInfoTabsValue,
    deletePopoverAnchor,
    setDeletePopoverAnchor,
  } = useManageProperty();

  const classes = useStyles();
  const newProps = omit(props, ['selected']);
  const tabRef = useRef(null);

  const handleOnSelected = (tabSelected: number): void => {
    if (tabSelected === 3) {
      setDeletePopoverAnchor(tabRef.current);
      return;
    }

    setPropertyInfoTabsValue(tabSelected);
  };

  const options = [
    { label: fm.mailingAddress, disabled: false },
    { label: fm.goPaperless, disabled: false },
    { label: generalFm.more, disabled: false },
    { label: generalFm.remove, disabled: false, targetRef: tabRef },
  ];

  const renderChildrenStep = (): JSX.Element | null => {
    if (!selected) return null;

    switch (propertyInfoTabsValue) {
      case 0:
        return <UpdateMailingAddress />;
      case 1:
        return <GoPaperless />;
      case 2:
        return <More />;
      default:
        return null;
    }
  };

  const AlertContent = (
    <Fragment>
      <span className={classes.paragraph}>{fm.removeDescParagraph1}</span>
      <span className={classes.paragraph}>{fm.removeDescParagraph2}</span>
    </Fragment>
  );

  return (
    <Fragment>
      <CardPropertyInfo
        shadow={false}
        classes={{
          root: classes.propertyInfoRoot,
        }}
        type={'full'}
        options={options}
        onSelected={handleOnSelected}
        tabsBackgroundColor={'transparent'}
        itemTextColor={'#187089'}
        selectedItemTextColor={'#187089'}
        indicatorBackgroundColor={selected ? '#000000' : 'transparent'}
        selectedIndex={propertyInfoTabsValue}
        {...newProps}
      >
        {renderChildrenStep()}
      </CardPropertyInfo>
      <CustomPopover
        anchorEl={deletePopoverAnchor}
        onClose={(): void => {
          setDeletePopoverAnchor(null);
        }}
        ptasVariant="danger"
        showCloseButton
        tail
        tailPosition="end"
        anchorOrigin={{
          vertical: 'bottom',
          horizontal: 'right',
        }}
        transformOrigin={{
          vertical: 'top',
          horizontal: 'right',
        }}
      >
        <Alert
          contentText={AlertContent}
          ptasVariant="danger"
          okButtonText={generalFm.remove}
          okShowButton
          classes={{
            root: classes.popoverRoot,
            text: classes.alertText,
            buttons: classes.button,
          }}
          okButtonClick={(): void => {
            setDeletePopoverAnchor(null);
            remove?.(props.id);
          }}
        />
      </CustomPopover>
    </Fragment>
  );
}

export default CustomCardPropertyInfo;
