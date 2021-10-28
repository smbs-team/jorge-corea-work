// AppBar.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { makeStyles } from '@material-ui/core';
import {
  CommAlert,
  CustomAppBar,
  CustomModalV2,
  CustomPopover,
  ExcelViewer,
  MenuButton,
  PanelHeader,
  SimpleDropDown,
} from '@ptas/react-ui-library';
import { Fragment, useEffect, useRef, useState } from 'react';
import useSubToolbarStore from 'stores/useSubToolbarStore';
import InsertDriveFileIcon from '@material-ui/icons/InsertDriveFile';
import GetAppIcon from '@material-ui/icons/GetApp';
import usePageManagerStore, {
  AssessmentYear,
  PtasYear,
} from 'stores/usePageManagerStore';
import useAreaTreeStore, { getRootFriendlyName } from 'stores/useAreaTreeStore';
import SettingsIcon from '@material-ui/icons/Settings';
import HelpOutlineIcon from '@material-ui/icons/HelpOutline';
import { navigate } from 'hookrouter';
import Pagination from './subHeader/Pagination';
import useContentHeaderStore from 'stores/useContentHeaderStore';
import { useAsync } from 'react-use';
import { get } from 'api/AxiosLoader';
import { orderBy } from 'lodash';

const useStyles = makeStyles({
  toolbar: {
    paddingLeft: 18,
    paddingRight: 9,
    display: 'flex',
  },
  panelHeader: {
    position: 'sticky',
    top: 48,
    zIndex: 2,
    minWidth: 1492,
  },
  modalRoot: {
    width: '80%',
    height: '80%',
  },
  dropdown: {
    width: 162,
    position: 'absolute',
    right: 24,
  },
});

function AppBar(): JSX.Element {
  const classes = useStyles();
  const { title } = useSubToolbarStore();
  const pageManagerStore = usePageManagerStore();
  const areaTreeStore = useAreaTreeStore();

  const buttonRef = useRef<HTMLButtonElement | null>(null);

  const [buttonAnchor, setButtonAnchor] = useState<HTMLButtonElement | null>(
    null
  );

  const [isOpen, setIsOpen] = useState<boolean>(false);
  const { hideYear } = useContentHeaderStore();

  const EXCEL_LINK =
    'https://www.cmu.edu/blackboard/files/evaluate/tests-example.xls';

  useEffect(() => {
    if (!pageManagerStore.validated) return;
    pageManagerStore.message === null
      ? setIsOpen(true)
      : setButtonAnchor(buttonRef.current);
    pageManagerStore.setValidated(false);
  }, [pageManagerStore]);

  useAsync(async () => {
    if (pageManagerStore.years.length) return;
    try {
      const response = await get<{ value: PtasYear[] }>(
        `${process.env.REACT_APP_API_URL}/PtasYear?$filter=statecode eq 0 and statuscode eq 1`
      );
      const years = response.data.value;
      const yearsToAdd: AssessmentYear[] = [];

      years.forEach((y) =>
        yearsToAdd.push({
          label: y.PtasName,
          value: y.PtasYearid,
          isCurrent: y.PtasIscurrentassessmentyear,
          isNext: y.PtasIsnextassessmentyear,
          isPrevious: y.PtasIspreviousassessmentyear,
        })
      );

      const currentYear = yearsToAdd.find((y) => y.isCurrent === true);
      const orderedYears = orderBy(
        yearsToAdd,
        (y) => parseInt(y.label),
        'desc'
      );
      if (currentYear) {
        pageManagerStore.setYears(
          orderedYears.filter(
            (y) => parseInt(y.label) <= parseInt(currentYear.label)
          )
        );
      } else {
        pageManagerStore.setYears(orderedYears);
      }
    } catch (error) {
      console.error(error);
    }
  }, [pageManagerStore, areaTreeStore.selectedItem]);

  const getSelectedYear = (): string => {
    if (pageManagerStore.selectedYear)
      return pageManagerStore.selectedYear.value as string;

    if (!pageManagerStore.years.length) return '';

    const defaultYear = pageManagerStore.years.find((y) => y.isCurrent);
    if (!defaultYear) return '';
    pageManagerStore.setSelectedYear(defaultYear);
    return defaultYear.value as string;
  };

  return (
    <Fragment>
      <CustomAppBar classes={{ toolbar: classes.toolbar }}>
        <MenuButton content="Income Tables" onClick={() => navigate('/')} />
        <MenuButton
          content={<SettingsIcon />}
          ButtonProps={{ style: { marginLeft: 'auto' } }}
        />
        <MenuButton content={<HelpOutlineIcon />} />
      </CustomAppBar>
      <PanelHeader
        classes={{ root: classes.panelHeader }}
        title={title}
        icons={
          areaTreeStore.selectedItem
            ? [
                {
                  icon: <InsertDriveFileIcon />,
                  text: 'Save Tables',
                  disabled: true,
                },
                {
                  icon: (
                    <i ref={buttonRef} style={{ height: 24 }}>
                      <GetAppIcon />
                    </i>
                  ),
                  text: 'Apply',
                  onClick: () => {
                    pageManagerStore.updateAndValidate();
                  },
                },
              ]
            : []
        }
        children={
          <div>
            <span style={{ position: 'absolute', left: '39%' }}>
              <Pagination />
            </span>
            {!hideYear && (
              <SimpleDropDown
                items={pageManagerStore.years}
                label="Assessment year"
                onSelected={(item) => {
                  pageManagerStore.reset();
                  pageManagerStore.setSelectedYear(
                    pageManagerStore.years.find(
                      (y) => y.value === item.value
                    ) ?? null
                  );
                }}
                classes={{ root: classes.dropdown }}
                value={getSelectedYear()}
              />
            )}
          </div>
        }
      />
      <CustomPopover
        anchorEl={buttonAnchor}
        onClose={() => {
          setButtonAnchor(null);
        }}
      >
        <CommAlert
          title="Error!"
          content={pageManagerStore.message ?? ''}
          onButtonClick={() => {
            setButtonAnchor(null);
          }}
        />
      </CustomPopover>
      <CustomModalV2
        open={isOpen}
        classes={{ root: classes.modalRoot }}
        onClose={() => setIsOpen(false)}
      >
        <ExcelViewer
          fileUrl={EXCEL_LINK}
          details={[
            {
              key: getRootFriendlyName(areaTreeStore.selectedItem?.entityName),
              value: areaTreeStore.selectedItem
                ? `${areaTreeStore.selectedItem.area?.ptasName} - ${areaTreeStore.selectedItem.area?.description}`
                : 'X - Area',
            },
            {
              key: 'Neighborhood:',
              value: areaTreeStore.selectedItem
                ? `${areaTreeStore.selectedItem.ptasName} - ${areaTreeStore.selectedItem.description}`
                : 'X - Neighborhood',
            },
          ]}
          title="Indicated Income Value Review"
          onCancel={() => setIsOpen(false)}
          onSave={() => {
            console.log('Saved!');
            setIsOpen(false);
          }}
        />
      </CustomModalV2>
    </Fragment>
  );
}

export default AppBar;
