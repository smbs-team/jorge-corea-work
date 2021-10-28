// LinearRegression.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { useState, useEffect, Fragment, useContext } from 'react';
import {
  Project,
  Expression,
  GenericGridRowData,
  RegressionType,
  ResultPayload,
  ExpressionType,
  CustomSearchParameter,
  SearchParameters,
  PostProcess,
  RScriptModelItem,
  GenericDropdownType,
} from 'services/map.typings';
import {
  getRScriptExpressions,
  getRScriptModel,
  getRScriptModels,
} from './common';
import GenericGrid from 'components/GenericGrid/GenericGrid';
// import { CustomTextField } from '@ptas/react-ui-library';
import { Grid, InputLabel, MenuItem, Select } from '@material-ui/core';
import moment from 'moment';
import { Link } from 'react-router-dom';
import {
  Field,
  FieldType,
  FormDefinition,
  FormSection,
  FormValues,
  Option,
} from 'components/FormBuilder/FormBuilder';
import FormBuilder from 'components/FormBuilder';
import { AppContext } from 'context/AppContext';
import ErrorMessage, {
  ImportErrorMessageType,
} from 'components/ErrorMessage/ErrorMessage';

let fieldTimeout: NodeJS.Timeout;
export interface LinearRegressionProps {
  datasets?: GenericDropdownType[];
  useDropdownDataset?: boolean;
  regression?: number;
  id?: number | string;
  regressionid?: number | string;
  modelDetails: Project | null;
  getFormData: (
    regression: number,
    gridData: GenericGridRowData[],
    expressionPayload: GenericGridRowData[],
    params?: SearchParameters[] | undefined
  ) => void;
  viewReport?: boolean;
  variableGridData?: Expression[];
  rowData?: GenericGridRowData[];
  postProcessResult?: ResultPayload;
  postProcess?: PostProcess | null | undefined;
  changeGrid?: () => void;
  useLandRegression?: boolean;
  getDatasetId?: (id: string) => void;
  datasetId?: string;
  useDropdown?: boolean;
  errorMessage?: ImportErrorMessageType;
  regressionType?: string;
  usePostProcessRegression?: boolean;
}

const LinearRegression = (props: LinearRegressionProps): JSX.Element => {
  const [expressionPayload, setExpressionsPayload] = useState<
    GenericGridRowData[]
  >([]);
  const [rowData, setRowData] = useState<GenericGridRowData[]>([]);
  const [gridData, setGridData] = useState<GenericGridRowData[]>([]);
  const [lockPrecommitExpressions, setLockPrecommitExpressions] =
    useState(false);
  const [regression, setRegression] = useState<number>(1);
  const [regressionType, setRegressionType] = useState<RegressionType[]>([]);
  const [formDefinition, setFormDefinition] = useState<FormDefinition | null>(
    null
  );
  const [formData, setFormData] = useState<FormValues | null>(null);
  const [newFormValues, setNewFormValues] = useState<FormValues>({});
  const appContext = useContext(AppContext);
  const [dataset, setDataset] = useState<string>('');

  const setDatasetInit = (): void => {
    if (props.datasets?.length && props.getDatasetId) {
      if (props.datasetId) {
        setDataset(props.datasetId);
      } else {
        setDataset(props.datasets[0].value);
        props.getDatasetId(props.datasets[0].value);
      }
    }
  };

  useEffect(setDatasetInit, [props.datasets, props.datasetId]);

  const getFormDefinition = (data: CustomSearchParameter[]): FormDefinition => {
    const dataMapping: { [id: string]: FieldType } = {
      Int32: 'number',
      DateTime: 'date',
      Boolean: 'boolean',
    };
    const fields = data?.reduce((a, b, _index, original) => {
      const newType = b.lookupValues
        ? 'dropdown'
        : dataMapping[b.type] || 'textbox';
      const newItem: Field = {
        defaultValue: b.defaultValue,
        label: b.name || b.description,
        fieldName: b.name,
        fieldId: b.id,
        validations: b.isRequired
          ? [{ type: 'required', message: `Field ${b.name} is required.` }]
          : [],
        options: b.lookupValues
          ?.map((v) => v.Value)
          .map((v) => {
            return { title: v, value: v } as Option;
          }),
        type: newType,
      };
      if (b.parameterRangeType === 'RangeStart') {
        const foundRange = original.find(
          (itm) =>
            itm.parameterGroupName === b.parameterGroupName && itm.id !== b.id
        );
        return [
          ...a,
          {
            ...newItem,
            isRange: true,
            toRangeField: foundRange?.name,
            toRangeFieldId: foundRange?.id,
            toRangeDefaultValue: foundRange?.defaultValue,
            title: 'From',
            label: newItem.label?.replace('From', '').replace('Start', ''),
          },
        ];
      }

      if (b.parameterRangeType === 'RangeEnd') {
        return a;
      }
      return [...a, newItem];
    }, [] as Field[]);

    const f: FormDefinition = {
      title: '',
      className: 'search-form',
      sections: [
        {
          fields: fields,
        },
      ],
    };

    return f;
  };

  const dateValidation = (field: string): boolean => {
    if (moment(field).isValid() && field !== '') {
      return true;
    } else {
      return false;
    }
  };

  const getTypeParameter = (script: string): string => {
    if (dateValidation(script)) {
      return 'DateTime';
    }
    if (parseInt(script)) {
      return 'Int32';
    }
    return 'String';
  };

  const setDinamicFormFields = (): void => {
    if (
      regressionType?.some(
        (rt) =>
          rt.value === regression &&
          rt.label.trim() !== ExpressionType.LinearRegression.trim()
      )
    ) {
      const expressions = props.postProcess?.customSearchExpressions
        ?.filter((exp) => exp.expressionRole === 'RScriptParameter')
        // eslint-disable-next-line
        ?.map<any>((ce: any) => {
          return {
            name: ce.columnName,
            defaultValue: ce.script,
            description: ce.columnName,
            expressions: null,
            forceEditLookupExpression: false,
            hasEditLookupExpression: false,
            id: 126,
            isRequired: true,
            lookupValues: null,
            parameterGroupName: ce.columnName,
            parameterRangeType: 'NotRanged',
            type: getTypeParameter(ce.script),
            typeLength: 0,
          };
        });

      if (getFormDefinition) {
        const formDef = getFormDefinition(expressions || []);
        setFormDefinition(formDef);
      }
    }
  };

  useEffect(setDinamicFormFields, [regression, regressionType.length]);

  const setRegressionPropType = (): void => {
    if (props.regression) {
      setRegression(props.regression);
    }
  };

  useEffect(setRegressionPropType, [props.regression]);

  const setFormDinamicValues = (): void => {
    if (!formDefinition) return;
    const fieldValues: { [id: string]: number | Date | boolean | string } = {
      number: '0',
      date: new Date(),
      textbox: '',
      dropdown: '',
      display: '',
      boolean: true,
    };
    const ttt = formDefinition?.sections.reduce(
      (f: Field[], b: FormSection) => {
        return [...f, ...b.fields];
      },
      [] as Field[]
    );
    const sss =
      ttt?.reduce((a, b) => {
        const t = fieldValues[b.type as string];
        a[b.fieldName] = b.defaultValue === null ? t : b.defaultValue || '';
        if (b.isRange) {
          a[b.toRangeField || ''] =
            b.toRangeDefaultValue === null ? t : b.toRangeDefaultValue || '';
        }
        return a;
      }, {} as { [id: string]: number | Date | boolean | string }) || {};
    setFormData(sss);
  };

  useEffect(setFormDinamicValues, [formDefinition]);

  const fetchRegressionsType = async (): Promise<void> => {
    const regressionTypes = await getRScriptModels();
    let role = 'TimeTrendRegression';
    let dropdownData = [];
    if (regressionTypes?.length) {
      if (props.useLandRegression) {
        role = 'LandRegression';
      }
      dropdownData = regressionTypes.filter(
        (e: RScriptModelItem) => e.rscriptModelRole === role
      );
      if (!props.regressionid) {
        dropdownData = dropdownData.filter((e) => !e.isDeleted);
      }
      dropdownData = dropdownData.map<RegressionType>((e: RScriptModelItem) => {
        return {
          value: e.rscriptModelId,
          label: e.rscriptModelName,
          resultDefinitions: e.resultsDefinitions,
          lockPrecommitExpressions: e.lockPrecommitExpressions,
        };
      });
      setRegressionType(dropdownData);
      if (!props.usePostProcessRegression) {
        const value = dropdownData[0].value;
        setRegression(value);
        setLockPrecommitExpressions(dropdownData[0].lockPrecommitExpressions);
        getRScriptModelParams(value);
      }
    }
  };

  const callRegressionTypes = (): void => {
    fetchRegressionsType();
  };

  useEffect(callRegressionTypes, []);

  const passFormDataToParent = (): void => {
    const allFields =
      formDefinition?.sections.reduce(
        (a: Field[], b: FormSection): Field[] => [...a, ...b.fields],
        []
      ) || [];
    const parameters = getSearchParameters(newFormValues, allFields);

    clearTimeout(fieldTimeout);
    fieldTimeout = setTimeout(() => {
      if (props.getFormData) {
        props.getFormData(regression, gridData, expressionPayload, parameters);
      }
    }, 800);
  };

  useEffect(passFormDataToParent, [regression, gridData]);

  const getSearchParameters = (
    values: FormValues,
    fields: Field[]
  ): SearchParameters[] => {
    const parameters = Object.keys(values).reduce(
      (prevValue: SearchParameters[], b, _i): SearchParameters[] => {
        let fff = fields.find((field) => field.fieldName === b);
        let itemId: number;
        let name: string;
        if (!fff) {
          fff = fields.find((field) => {
            return field.toRangeField === b;
          });
          itemId = fff?.toRangeFieldId || -1;
          name = fff?.toRangeField || 'failed' + b;
        } else {
          itemId = fff?.fieldId || -1;
          name = b;
        }
        return [{ id: itemId, value: values[b], name: name }, ...prevValue];
      },
      []
    );

    return parameters;
  };

  const passDinamicFormDataToParent = (): void => {
    const allFields =
      formDefinition?.sections.reduce(
        (a: Field[], b: FormSection): Field[] => [...a, ...b.fields],
        []
      ) || [];
    const parameters = getSearchParameters(newFormValues, allFields);

    clearTimeout(fieldTimeout);
    fieldTimeout = setTimeout(() => {
      if (props.getFormData) {
        props.getFormData(regression, gridData, expressionPayload, parameters);
      }
    }, 800);
  };

  useEffect(passDinamicFormDataToParent, [formDefinition, newFormValues]);

  const setVariableGridData = (): void => {
    if (props.rowData?.length) return;
    let rowData: GenericGridRowData[] = [];
    if (props?.variableGridData?.length) {
      rowData = getExpressions(props.variableGridData);
    }
    setRowData(rowData);
  };

  const setVariableData = (): void => {
    if (props.rowData?.length) {
      setRowData(props.rowData);
    }
  };

  useEffect(setVariableData, [props.rowData]);

  useEffect(setVariableGridData, [props.variableGridData]);

  const fetchData = async (regression: number): Promise<void> => {
    const expressions = await getRScriptExpressions(regression);
    let rowData: GenericGridRowData[] = [];
    if (expressions?.length) {
      rowData = getExpressions(expressions);
    }
    setRowData(rowData);
  };

  const callData = (): void => {
    if (!props.regressionid) {
      fetchData(regression);
    }
    if (regression && regressionType.length) {
      setLockPrecommitExpressions(
        regressionType.find((re) => re.value === regression)
          ?.lockPrecommitExpressions || false
      );
    }
  };

  useEffect(callData, [props.modelDetails, regression]);

  const getExpressions = (expressions: Expression[]): GenericGridRowData[] => {
    const dependent = expressions.filter(
      (e) => e.expressionRole === 'CalculatedColumnPreCommit_Dependent'
    );
    const independent = expressions.filter(
      (e) => e.expressionRole === 'CalculatedColumnPreCommit_Independent'
    );
    const CalculatedColumn = expressions.filter(
      (e) => e.expressionRole === 'CalculatedColumn'
    );

    const calculatedrowdata = expressions
      .filter((e) => e.expressionRole === 'CalculatedColumn')
      .map((e) => ({
        type: 'CalculatedColumn',
        expressionRole: e.expressionRole,
        expressionType: e.expressionType,
        name: e.columnName,
        transformation: 'Calculated by RScript',
        category: e.category,
      }));

    const rowData = expressions
      .filter(
        (e) =>
          ![
            'CalculatedColumnPreCommit_Independent',
            'CalculatedColumnPreCommit_Dependent',
            'CalculatedColumn',
            'RScriptParameter',
          ].includes(e.expressionRole)
      )
      .map<GenericGridRowData>((e) => {
        return {
          type: getType(e, dependent, independent),
          expressionRole: e.expressionRole,
          expressionType: e.expressionType,
          name: e.columnName,
          transformation: e.script,
          category: e.category,
          note: e.note,
          isNewRow: false,
        };
      });
    const expressionPayload = [
      ...dependent,
      ...independent,
      ...CalculatedColumn,
    ].map<GenericGridRowData>((e) => {
      return {
        type: 'Expression',
        expressionRole: e.expressionRole,
        expressionType: e.expressionType,
        name: e.columnName,
        transformation: e.script,
        category: e.category,
        note: e.note,
        // isNewRow: false,
      };
    });
    setExpressionsPayload(expressionPayload);
    return [...calculatedrowdata, ...rowData];
  };

  const getType = (
    e: Expression,
    dependent: Expression[],
    independent: Expression[]
  ): string => {
    if (
      dependent.some(
        (d) =>
          d.script === e.columnName &&
          e.expressionRole === 'CalculatedColumnPreCommit'
      )
    ) {
      return 'Dependent';
    }
    if (
      independent.some(
        (d) =>
          d.script === e.columnName &&
          e.expressionRole === 'CalculatedColumnPreCommit'
      )
    ) {
      return 'Independent';
    }
    if (e.expressionRole === 'CalculatedColumnPostCommit') {
      return 'Calculated';
    }
    return 'Calculated';
  };

  const saveColData = (): boolean => {
    return true;
  };

  const updateColData = (
    newData: GenericGridRowData[],
    change: boolean
  ): void => {
    if (change && props.changeGrid) {
      props.changeGrid();
    }
    setGridData(newData);
  };

  const renderReportSection = (): JSX.Element => {
    if (props.viewReport && props.postProcessResult?.FileResults?.length) {
      return (
        <Grid sm={3} className="TimeTrend-results report">
          <InputLabel className="" id="label-for-dd">
            {props.postProcessResult?.FileResults?.filter(
              (file) =>
                file.FileName.includes('.html') && file.Type === 'Report'
            ).map((fileResult) => (
              <Link
                to={`/models/reports/${props.id}/${props.regressionid}/${fileResult.FileName}`}
                onClick={(): void =>
                  appContext.setPostProcessName?.(
                    props.postProcess?.postProcessName ?? null
                  )
                }
              >
                <span className="TimeTrend-report">{fileResult.Title}</span>
              </Link>
            ))}
          </InputLabel>
        </Grid>
      );
    }
    return <Fragment></Fragment>;
  };

  const renderResult = (name: string): string => {
    let resultString = 'To Be Calculated';
    if (props.postProcessResult?.Results?.length) {
      const result = props.postProcessResult?.Results[0];
      Object.values(result).forEach((re, index) => {
        if (Object.keys(result)[index] === name) {
          resultString = re;
        }
      });
    }
    return resultString;
  };

  const renderVariablesRegression = (): JSX.Element => {
    const renderVariables = regressionType?.find(
      (rt) =>
        rt.value === regression &&
        rt.label.trim() !== ExpressionType.LinearRegression.trim()
    );
    if (renderVariables && formDefinition && formData) {
      return (
        <Fragment>
          <FormBuilder
            onDataChange={(formValues: FormValues): void => {
              setNewFormValues(formValues);
              if (props.changeGrid) {
                props.changeGrid();
              }
            }}
            formData={formData}
            formInfo={formDefinition}
          />
        </Fragment>
      );
    }
    return <Fragment></Fragment>;
  };

  const getRScriptModelParams = async (regression: number): Promise<void> => {
    const scriptModel = await getRScriptModel(regression);
    if (scriptModel.parameters) {
      const formDef = getFormDefinition(scriptModel.parameters);
      setFormDefinition(formDef);
    }
  };

  const renderVariablesResult = (): JSX.Element => {
    return (
      <Fragment>
        {regressionType.length
          ? regressionType
              ?.find((r) => r.value === regression)
              ?.resultDefinitions?.map((rd) => (
                <InputLabel className="" id="label-for-dd">
                  <strong>{rd.name}:</strong>{' '}
                  <span
                    className={
                      props.viewReport ? 'Result-strong' : 'Result-calculated'
                    }
                  >
                    {renderResult(rd.name)}
                  </span>
                </InputLabel>
              ))
          : ''}
      </Fragment>
    );
  };

  const renderUserInput = (): JSX.Element => {
    if (props.useDropdownDataset && props.datasets?.length) {
      return (
        <Fragment>
          <InputLabel className="TimeTrend-label" id="label-for-dd">
            Dataset:
          </InputLabel>
          <Select
            variant="outlined"
            className="drop-down"
            labelId="label-for-dd"
            fullWidth
            placeholder={'Land dataset'}
            // defaultValue={props.datasets.find(data=>data.value === dataset)?.value}
            value={dataset}
            onChange={(e): void => {
              if (typeof e.target.value === 'string') {
                setDataset(e.target.value);
                if (props.getDatasetId) {
                  props.getDatasetId(e.target.value);
                }
              }
            }}
          >
            {props.datasets?.map((o, i) => (
              <MenuItem key={i} value={o?.value}>
                {o?.label}
              </MenuItem>
            ))}
          </Select>
        </Fragment>
      );
    }

    return <Fragment></Fragment>;
  };

  const renderRsQuaredResult = (name: string[]): string => {
    let resultString = 'To Be Calculated';
    if (props.postProcessResult?.Results?.length) {
      const result = props.postProcessResult?.Results[1];
      Object.values(result).forEach((re, index) => {
        if (name.includes(Object.keys(result)[index])) {
          resultString = re;
        }
      });
    }
    return resultString;
  };

  return (
    <Fragment>
      <Grid container className="TimeTrend-labelForm">
        <Grid sm={3}>
          <InputLabel className="" id="label-for-dd">
            {props.regressionType || 'Regression:'}
          </InputLabel>
        </Grid>
        <Grid sm={8}>
          <InputLabel className="TimeTrend-labelresult" id="label-for-dd">
            Result:
          </InputLabel>
        </Grid>
      </Grid>
      <Grid container className="TimeTrend-formWrapper">
        <Grid sm={4} className="TimeTrend-form">
          <div className="TimeTrend-formGroup ml">
            <InputLabel className="TimeTrend-label" id="label-for-dd">
              Type:
            </InputLabel>
            <Select
              variant="outlined"
              className="drop-down"
              labelId="label-for-dd"
              fullWidth
              disabled={!!props.postProcess}
              placeholder={'Linear'}
              value={regression}
              onChange={(e): void => {
                setRegression(parseInt(`${e.target.value}`));
                getRScriptModelParams(parseInt(`${e.target.value}`));
                fetchData(parseInt(`${e.target.value}`));
              }}
            >
              {regressionType?.map((o, i) => (
                <MenuItem key={i} value={o?.value}>
                  {o?.label}
                </MenuItem>
              ))}
            </Select>
          </div>
          <div className="TimeTrend-formGroup">{renderUserInput()}</div>
          {renderVariablesRegression()}
        </Grid>
        <Grid sm={3} className="TimeTrend-results">
          <InputLabel className="" id="label-for-dd">
            <strong>
              ValuationDate ={' '}
              {moment(props?.modelDetails?.assessmentDateTo).format(
                'MM-DD-YYYY'
              )}
            </strong>
          </InputLabel>
          {renderVariablesResult()}
        </Grid>
        <Grid sm={2} className="TimeTrend-results">
          <InputLabel className="" id="label-for-dd">
            <strong>R-squared = </strong>{' '}
            <span
              className={
                props.viewReport ? 'Result-strong' : 'Result-calculated'
              }
            >
              {renderRsQuaredResult(['RSquared', 'R-Squared'])}
            </span>
          </InputLabel>
          <InputLabel className="" id="label-for-dd">
            <strong>Adjusted R-squared = </strong>{' '}
            <span
              className={
                props.viewReport ? 'Result-strong' : 'Result-calculated'
              }
            >
              {renderRsQuaredResult(['AdjustedRSquared', 'AdjustedR-Squared'])}
            </span>
          </InputLabel>
        </Grid>
        {renderReportSection()}
      </Grid>
      <div className="TimeTrend-grid">
        <GenericGrid
          rowData={rowData}
          saveColData={saveColData}
          updateColData={updateColData}
          useDropdown={props.useDropdown}
          badExpressions={props.errorMessage?.reason}
          lockPrecommitExpressions={lockPrecommitExpressions}
        />
        <ErrorMessage message={props.errorMessage} />
      </div>
    </Fragment>
  );
};

export default LinearRegression;
