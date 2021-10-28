// QueryControl.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  createStyles,
  makeStyles,
  useTheme,
  Theme,
  IconButton,
} from '@material-ui/core';
import { AddCircleOutline, Close } from '@material-ui/icons';
import {
  CustomDatePicker,
  CustomTextField,
  DropDownItem,
  IconToolBar,
  SimpleDropDown,
} from '@ptas/react-ui-library';
import React, { useContext, useEffect, useState } from 'react';
import { useUpdateEffect } from 'react-use';
import { utilService } from 'services/common';
import { OPERATORS_BY_TYPE, TagsManageContext } from './TagsManageContext';
import { LineQuery, PropertyItem } from './typing';

const lineOperators = [
  {
    label: 'And',
    value: 'and',
  },
  {
    label: 'Or',
    value: 'or',
  },
];

const booleanValues = [
  {
    label: 'True',
    value: 'true',
  },
  {
    label: 'False',
    value: 'false',
  },
];

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    lineItem: {
      display: 'flex',
      flexDirection: 'row',
      justifyContent: 'flex-end',
      minWidth: '600px',
    },
    layerDropdown: {
      marginRight: theme.spacing(2),
      marginBottom: theme.spacing(2),
      minWidth: '150px',
    },
    operatorDropdown: {
      marginRight: theme.spacing(2),
      marginBottom: theme.spacing(2),
      width: '100px',
    },
    closeBtnContainer: {
      marginTop: '-5px',
    },
  })
);

interface Props {
  properties: PropertyItem[];
}

const QueryControl = (props: Props): JSX.Element => {
  const { selectedLabel, setSelectedLabel } = useContext(TagsManageContext);
  const classes = useStyles(useTheme());
  const [properties, setProperties] = useState<DropDownItem[]>([]);
  const [lines, setLines] = useState<LineQuery[]>([]);

  useEffect(() => {
    if (selectedLabel) {
      const { queryFilter } = selectedLabel;
      queryFilter && setLines(queryFilter);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedLabel?.id]);

  useUpdateEffect(() => {
    updateSelectedLabel();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [lines]);

  useEffect(() => {
    const propertyList = (props.properties ?? []).map((p) => {
      return {
        label: p.name,
        value: p.name,
      };
    });
    setProperties(propertyList);
  }, [props.properties]);

  const updateSelectedLabel = (): void => {
    if (!selectedLabel) return;
    setSelectedLabel((prev) => {
      if (!prev) return;
      return {
        ...prev,
        queryFilter: lines,
      };
    });
  };

  const lineAdd = (): void => {
    const linesUpdated = [
      ...lines,
      {
        property: '',
        type: '',
        operator: '',
        value: '',
        operators: [],
      },
    ];
    setLines(linesUpdated);
  };

  const lineOperatorChange = (index: number) => (item: DropDownItem): void => {
    const linesUpdated = lines.map((l, idx) => {
      return idx !== index ? l : { ...l, lineOperator: item.value as string };
    });
    setLines(linesUpdated);
  };

  const operatorChange = (index: number) => (item: DropDownItem): void => {
    const linesUpdated = lines.map((l, idx) => {
      return idx !== index ? l : { ...l, operator: item.value as string };
    });
    setLines(linesUpdated);
  };

  const valueChange = (index: number) => (
    e: React.ChangeEvent<HTMLInputElement>
  ): void => {
    const linesUpdated = lines.map((l, idx) => {
      return idx !== index ? l : { ...l, value: e.currentTarget.value };
    });
    setLines(linesUpdated);
  };

  const getOperatorByType = (type: string): DropDownItem[] => {
    return OPERATORS_BY_TYPE?.filter((ot) => {
      return ot.type.some((t) => t === type);
    }) as DropDownItem[];
  };

  const propertyChange = (index: number) => (item: DropDownItem): void => {
    const typeFound = (props.properties ?? []).find(
      (p) => p.name === item.value
    )?.type;
    const operators = typeFound ? getOperatorByType(typeFound) : [];
    const linesUpdated = lines.map((l, idx) => {
      return idx !== index
        ? l
        : {
            ...l,
            type: typeFound as string,
            property: item.value as string,
            operators,
          };
    });
    setLines(linesUpdated);
  };

  const valueDropdownChange = (index: number) => (item: DropDownItem): void => {
    const linesUpdated = lines.map((l, idx) => {
      return idx !== index ? l : { ...l, value: item.value as string };
    });
    setLines(linesUpdated);
  };

  const removeLine = (index: number) => (): void => {
    const restOfItem = lines.filter((l, idx) => idx !== index);
    const lineUpdated =
      index === 0
        ? restOfItem.map((ri, idx) => {
            if (idx === 0) ri.lineOperator = ''; // remove operator
            return ri;
          })
        : restOfItem;
    setLines(lineUpdated);
  };

  const renderValueControl = (idx: number, l: LineQuery): JSX.Element => {
    if (l.type === 'date') {
      return (
        <CustomDatePicker
          onChange={(date): void => {
            const value = `${date?.getTime() ?? ''}`;
            const linesUpdated = lines.map((l, i) => {
              return i !== idx ? l : { ...l, value };
            });
            setLines(linesUpdated);
          }}
          label="Value"
          value={utilService.unixTimeToDate(l.value as string)}
        />
      );
    }

    if (l.type === 'boolean') {
      return (
        <SimpleDropDown
          classes={{
            root: classes.layerDropdown,
          }}
          label="Value"
          items={booleanValues}
          onSelected={valueDropdownChange(idx)}
          value={`${l.value}`}
        />
      );
    }

    return (
      <CustomTextField
        label="Value"
        type={l.type}
        value={l.value}
        onChange={valueChange(idx)}
      />
    );
  };

  return (
    <div>
      {lines.map((l, idx) => {
        return (
          <div key={`line-${idx}`} className={classes.lineItem}>
            {idx !== 0 && (
              <SimpleDropDown
                classes={{
                  root: classes.operatorDropdown,
                }}
                label="Operator"
                items={lineOperators}
                onSelected={lineOperatorChange(idx)}
                value={`${l.lineOperator ?? ''}`}
              />
            )}
            <SimpleDropDown
              classes={{
                root: classes.layerDropdown,
              }}
              label="Property"
              items={properties}
              onSelected={propertyChange(idx)}
              value={`${l.property}`}
            />
            <SimpleDropDown
              classes={{
                root: classes.operatorDropdown,
              }}
              label="Operator"
              items={l.operators}
              onSelected={operatorChange(idx)}
              value={`${l.operator}`}
            />

            {renderValueControl(idx, l)}

            <span className={classes.closeBtnContainer}>
              <IconButton
                color="primary"
                component="span"
                onClick={removeLine(idx)}
              >
                <Close />
              </IconButton>
            </span>
          </div>
        );
      })}
      <IconToolBar
        icons={[
          {
            text: 'New condition',
            icon: <AddCircleOutline />,
            onClick: lineAdd,
          },
        ]}
      />
    </div>
  );
};

export default QueryControl;
