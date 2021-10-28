// newTimeTrend.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useState, useContext, useEffect } from 'react';
import {
  CustomTextField,
  SimpleDropDown,
  SectionContainer,
  CustomTabs,
  CustomDatePicker,
  DropDownItem,
  IconToolBarItem,
  ErrorMessageContext,
} from '@ptas/react-ui-library';
import { Link } from 'react-router-dom';
import InsertDriveFileIcon from '@material-ui/icons/InsertDriveFile';
import { makeStyles, Box, Divider } from '@material-ui/core';

import SalesData from './SalesData';
import DatasetTree from './DatasetTree';
import {
  NewTimeTrendContext,
  SplitModelProperty,
} from 'context/NewTimeTrendContext';
import CustomHeader from 'components/common/CustomHeader';
import { AppContext } from 'context/AppContext';
import Select from 'react-select';
import { orderBy, tail } from 'lodash';
import { useAsync } from 'react-use';
import { getPtasYears } from 'services/common';
import { AssessmentYear } from 'services/map.typings';

const useStyles = makeStyles((theme) => ({
  projectDetailsContainer: {
    padding: theme.spacing(4, 2, 1, 3),
  },
  projectDetailsTop: {
    minWidth: 1040,
    width: 'fit-content',
    marginBottom: theme.spacing(3),
  },
  projectDetailsBottom: {
    width: 1040,
    display: 'flex',
    flexWrap: 'wrap',
    gap: 12,
    marginBottom: theme.spacing(3),
    marginTop: theme.spacing(3),
  },
  datePicker: {
    minWidth: 172,
    width: 172,
  },
  dropdown: {
    width: 200,
  },
  tabs: {
    marginLeft: theme.spacing(3),
  },
  projectTypeDropdown: {
    width: 200,
  },
  splitModelValue: {
    minWidth: 200,
  },
  splitModelProp: {
    width: 200,
  },
  nameTextField: {
    width: '100%',
  },
  commentsTextField: {
    width: '100%',
  },
  link: {
    color: theme.ptas.colors.theme.black,
  },
  dateInput: {
    height: 40,
  },
}));

/**
 * NewTimeTrend
 *
 * @param props - Component props
 * @returns A JSX element
 */
function NewTimeTrend(): JSX.Element {
  const classes = useStyles();
  const [salesSwitch, setSalesSwitch] = useState<number>(0);
  const [populationSwitch, setPopulationSwitch] = useState<number>(0);
  const [projectTypesItems, setProjectTypesItems] = useState<DropDownItem[]>(
    []
  );

  const [areas, setAreas] = useState<DropDownItem[]>([]);
  const [areaDropdownValue, setAreaDropdownValue] = useState<React.ReactText>();

  const context = useContext(NewTimeTrendContext);

  const appContext = useContext(AppContext);
  const { showErrorMessage } = useContext(ErrorMessageContext);
  const [assessmentYears, setAssessmentYears] = useState<AssessmentYear[]>();

  useAsync(async () => {
    try {
      const years = await getPtasYears();

      const yearsToAdd: AssessmentYear[] = [];
      if (!years) return;

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
        context.setAssessmentYear?.(parseInt(currentYear.label));
        setAssessmentYears(
          orderedYears.filter(
            (y) => parseInt(y.label) <= parseInt(currentYear.label)
          )
        );
      } else {
        setAssessmentYears(orderedYears);
      }
    } catch (error) {
      console.error(error);
    }
  }, []);

  useEffect(() => {
    appContext?.setCallFolders?.(true);
  }, [appContext]);

  useEffect(() => {
    if (
      !context.setAssessmentDateFrom ||
      !context.assessmentYear ||
      !context.setAssessmentDateTo
    )
      return;

    context.setAssessmentDateFrom(
      new Date(`01/01/${context.assessmentYear - 3}`)
    );
    context.setAssessmentDateTo(new Date(`01/01/${context.assessmentYear}`));
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [context.assessmentYear]);

  useEffect(() => {
    const items: DropDownItem[] = [];
    context.projectTypes?.projectTypes.forEach((p) => {
      items.push({
        label: p.projectTypeName,
        value: p.projectTypeId,
      });
    });
    setProjectTypesItems(items);
  }, [context.projectTypes]);

  useEffect(() => {
    const items: DropDownItem[] = [];
    context.salesAreas?.results.forEach((a) => {
      items.push({
        label: a.Key,
        value: a.Value,
      });
    });
    setAreas(items);
  }, [context.salesAreas]);

  const headerIcons: IconToolBarItem[] = [
    {
      icon: <InsertDriveFileIcon />,
      text: 'Create model',
      onClick: async (): Promise<void> => {
        if (context.createModel) {
          const message = await context.createModel(
            salesSwitch,
            populationSwitch
          );

          if (message) {
            showErrorMessage(message, 'CS', false);
          }
        }
      },
      disabled:
        !context.selectedProjectType ||
        !context.assessmentYear ||
        !context.assessmentDateTo ||
        !context.assessmentDateFrom ||
        !context.projectName ||
        !context.selectedProjectArea ||
        !context.selectedSplitModelValues ||
        context.selectedSplitModelValues.length === 0 ||
        !context.selectedPropertyType ||
        (salesSwitch === 0
          ? context.salesSelectedAreas &&
            context.salesSelectedAreas.length === 0
          : !context.salesTreeSelection) ||
        (populationSwitch === 0
          ? context.populationSelectedAreas &&
            context.populationSelectedAreas.length === 0
          : !context.populationTreeSelection),
    },
  ];

  const handleSelected = (e: DropDownItem): void => {
    const filtered = context.projectTypes?.projectTypes.filter(
      (f) => f.projectTypeId === e.value
    );
    context.setSelectedProjectType &&
      context.setSelectedProjectType(
        filtered && filtered.length > 0 ? filtered[0] : null
      );
  };

  const handleDropdownSelect = (e: DropDownItem): void => {
    if (
      !context.populationSelectedAreas ||
      !context.salesSelectedAreas ||
      !context.setPopulationSelectedAreas ||
      !context.setSalesSelectedAreas ||
      !context.setSelectedProjectArea
    )
      return;

    setAreaDropdownValue(e.value);
    context.setSelectedProjectArea(e.value as number);

    context.salesSelectedAreas.shift();
    context.populationSelectedAreas.shift();

    const filteredSales = context.salesSelectedAreas.filter(
      (a) => a !== (e.value as number)
    );

    const existInSales = context.salesAreas?.results.find(
      (a) => a.Value === (e.value as number)
    );

    const existsInPopulation = context.populationAreas?.results.find(
      (a) => a.Value === (e.value as number)
    );

    if (existInSales) {
      context.setSalesSelectedAreas([e.value as number, ...filteredSales]);
    } else {
      context.setSalesSelectedAreas(tail(context.salesSelectedAreas));
    }

    if (existsInPopulation) {
      context.setPopulationSelectedAreas([
        e.value as number,
        ...context.populationSelectedAreas,
      ]);
    } else {
      context.setPopulationSelectedAreas(tail(context.populationSelectedAreas));
    }
  };

  return (
    <Fragment>
      <CustomHeader
        route={[
          <Link to="/models" className={classes.link}>
            Models
          </Link>,
          <label>New model</label>,
        ]}
        icons={headerIcons}
      />
      <Box className={classes.projectDetailsContainer}>
        <Box className={classes.projectDetailsTop}>
          <div style={{ display: 'flex', gap: 17 }}>
            <SimpleDropDown
              label="Model type"
              items={projectTypesItems}
              onSelected={(e): void => handleSelected(e)}
              classes={{ root: classes.projectTypeDropdown }}
              value={
                !context.selectedProjectType
                  ? ''
                  : projectTypesItems.find(
                      (p) =>
                        p.label === context.selectedProjectType?.projectTypeName
                    )?.value
              }
            />
            <SimpleDropDown
              label="Area"
              items={areas}
              onSelected={handleDropdownSelect}
              classes={{ root: classes.dropdown }}
              disabled={!context.selectedProjectType}
              value={areaDropdownValue}
            />
            <SimpleDropDown
              items={assessmentYears ?? []}
              label="Assessment year"
              onSelected={(item): void => {
                context.setAssessmentYear?.(parseInt(item.label));
              }}
              classes={{ root: classes.datePicker }}
              value={assessmentYears?.find((y) => y.isCurrent)?.value}
            />
            <SimpleDropDown
              label="Split Model Property"
              items={context.splitModelPropertyOptions ?? []}
              disabled={!context.selectedProjectArea}
              onSelected={(e): void =>
                context.setSelectedPropertyType?.(e.value as SplitModelProperty)
              }
              classes={{ root: classes.splitModelProp }}
            />
            <Select
              isMulti
              placeholder="Split Model Value"
              value={context.selectedSplitModelValues}
              options={context.splitModelValues}
              className={classes.splitModelValue}
              isLoading={context.isSplitModelLoading}
              onChange={(items): void => {
                const isOtherSelected = items.find((s) => s.value === 'other');
                if (isOtherSelected) {
                  context.setSelectedSplitModelValues?.([
                    { value: 'other', label: 'Other' },
                  ]);
                  return;
                }

                context.setSelectedSplitModelValues?.([...items]);
              }}
              isOptionDisabled={(): boolean =>
                !!context?.selectedSplitModelValues?.find(
                  (s) => s.value === 'other'
                )
              }
              isDisabled={
                !context.selectedPropertyType || context.isSplitModelLoading
              }
              styles={{
                // eslint-disable-next-line @typescript-eslint/explicit-function-return-type
                control: (base, state) => ({
                  ...base,
                  minHeight: 41,
                  boxShadow: state.isFocused
                    ? '0px 0px 1px #769500'
                    : base.boxShadow,
                  borderColor: state.isFocused
                    ? '#769500'
                    : state.isDisabled
                    ? base.borderColor
                    : 'black',
                  backgroundColor: state.isDisabled
                    ? 'none'
                    : base.backgroundColor,
                  '&:hover': {
                    borderColor: state.isFocused ? '#769500' : 'black',
                  },
                }),
                // eslint-disable-next-line @typescript-eslint/explicit-function-return-type
                placeholder: (base, state) => ({
                  color: state.isDisabled ? 'hsl(0, 0%, 70%)' : base.color,
                }),
              }}
            />
          </div>
          <div style={{ display: 'flex', marginTop: 12 }}>
            <CustomTextField
              label="Name"
              className={classes.nameTextField}
              onChange={(e): void =>
                context.setProjectName && context.setProjectName(e.target.value)
              }
            />
          </div>
        </Box>
        <Divider />
        <Box className={classes.projectDetailsBottom}>
          <CustomDatePicker
            label="Sale Date From"
            onChange={(e): void =>
              context.setAssessmentDateFrom && context.setAssessmentDateFrom(e)
            }
            className={classes.datePicker}
            value={context.assessmentDateFrom}
          />
          <CustomDatePicker
            label="Sale Date To"
            onChange={(e): void =>
              context.setAssessmentDateTo && context.setAssessmentDateTo(e)
            }
            className={classes.datePicker}
            value={context.assessmentDateTo}
          />
          <CustomTextField
            className={classes.commentsTextField}
            label="Comments"
            onChange={(e): void =>
              context.setComments && context.setComments(e.target.value)
            }
            multiline
          />
        </Box>
      </Box>
      <SectionContainer
        title="Sales data"
        miscContent={
          <CustomTabs
            items={['Area', 'Dataset']}
            defaultSelection={salesSwitch}
            onSelected={(e: number): void => setSalesSwitch(e)}
            classes={{
              root: classes.tabs,
            }}
            switchVariant
            invertColors
            disabled={!context.selectedProjectType}
          />
        }
      >
        {salesSwitch === 0 ? (
          <SalesData
            onSelectedItem={(values, isSelected): void =>
              context.onSetArea &&
              context.onSetArea(isSelected, 'sales', values)
            }
            selectedItems={context.salesSelectedAreas}
            type="sales"
          />
        ) : (
          <DatasetTree
            onRowClick={context.setSalesTreeSelection}
            type="sales"
          />
        )}
      </SectionContainer>
      <SectionContainer
        title="Population data"
        miscContent={
          <CustomTabs
            items={['Area', 'Dataset']}
            defaultSelection={populationSwitch}
            onSelected={(e: number): void => setPopulationSwitch(e)}
            classes={{
              root: classes.tabs,
            }}
            switchVariant
            invertColors
            disabled={!context.selectedProjectType}
          />
        }
      >
        {populationSwitch === 0 ? (
          <SalesData
            onSelectedItem={(values, isSelected): void =>
              context.onSetArea &&
              context.onSetArea(isSelected, 'population', values)
            }
            selectedItems={context.populationSelectedAreas}
            type="population"
          />
        ) : (
          <DatasetTree
            onRowClick={(e): void =>
              context.setPopulationTreeSelection &&
              context.setPopulationTreeSelection(e)
            }
            type="population"
          />
        )}
      </SectionContainer>
    </Fragment>
  );
}

export default NewTimeTrend;
