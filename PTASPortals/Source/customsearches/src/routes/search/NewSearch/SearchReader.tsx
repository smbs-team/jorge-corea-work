// SearchReader.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { AxiosLoader } from 'services/AxiosLoader';
import React, { useEffect, useState, Fragment } from 'react';
import {
  FormDefinition,
  FieldType,
  Field,
  FormSection,
  FormValues,
  Option,
  VariableValue,
} from 'components/FormBuilder/FormBuilder';
import {
  CustomSearchParameter,
  SearchParam,
  SearchParameters,
} from 'services/map.typings';
import FormBuilder from 'components/FormBuilder';
import Loading from 'components/Loading';
import { v4 as uuidv4 } from 'uuid';
import _ from 'lodash';

const getFormDefinition = (data: CustomSearchParameter[]): FormDefinition => {
  const dataMapping: { [id: string]: FieldType } = {
    Int32: 'number',
    DateTime: 'date',
    Boolean: 'boolean',
  };
  const fields = data.reduce(
    (prevFields, searchParameter, _index, original) => {
      const newType = searchParameter.lookupValues
        ? 'dropdown'
        : dataMapping[searchParameter.type] || 'textbox';
      const newItem: Field = {
        defaultValue: searchParameter.defaultValue,
        label: searchParameter.description,
        fieldName: searchParameter.name,
        fieldId: searchParameter.id,
        originalParam: searchParameter,
        validations: searchParameter.isRequired
          ? [
              {
                type: 'required',
                message: `Field ${searchParameter.name} is required.`,
              },
            ]
          : [],
        options:
          searchParameter.lookupValues?.map(
            (v) => ({ title: v.Key ?? v.Value, value: v.Value } as Option)
          ) ?? [],
        type: newType,
      };
      if (searchParameter.parameterRangeType === 'RangeStart') {
        const foundRange = original.find(
          (itm) =>
            itm.parameterGroupName === searchParameter.parameterGroupName &&
            itm.id !== searchParameter.id
        );
        return [
          ...prevFields,
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

      if (searchParameter.parameterRangeType === 'RangeEnd') {
        return prevFields;
      }
      return [...prevFields, newItem];
    },
    [] as Field[]
  );

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

const getSearchParameters = (
  values: FormValues,
  fields: Field[]
): SearchParameters[] => {
  const parameters = Object.keys(values).reduce(
    (prevValue: SearchParameters[], fieldKey, _i): SearchParameters[] => {
      let itemId: number;
      let name: string;
      let foundField = fields.find((field) => field.fieldName === fieldKey);
      if (!foundField) {
        foundField = fields.find((field) => field.toRangeField === fieldKey);
        itemId = foundField?.toRangeFieldId || -1;
        name = foundField?.toRangeField || 'failed' + fieldKey;
      } else {
        itemId = foundField?.fieldId || -1;
        name = fieldKey;
      }

      const value = values[fieldKey];
      const fieldMustbeIncluded =
        value || foundField?.originalParam?.isRequired;
      if (!fieldMustbeIncluded) {
        return prevValue;
      }
      const v: unknown = getFieldValue(
        foundField?.originalParam?.type,
        value,
        foundField?.originalParam?.defaultValue
      );
      const newField = [{ id: itemId, value: v, name: name }, ...prevValue];
      return newField;
    },
    []
  );
  return parameters;
};

const SearchReader = ({
  searchId,
  onValuesChanged,
  onValidChanged,
}: {
  searchId: string;
  onValuesChanged: (searchParams: SearchParam) => void;
  onValidChanged: (value: boolean) => void;
}): JSX.Element => {
  const [formDefinition, setFormDefinition] = useState<FormDefinition | null>(
    null
  );
  const [formData, setFormData] = useState<FormValues | null>(null);
  const [newFormValues, setNewFormValues] = useState<FormValues>({});
  const [customSearchParameters, setCustomSearchParameters] =
    useState<CustomSearchParameter[]>();

  const [formKey] = useState(uuidv4());

  useEffect(() => {
    const allFields =
      formDefinition?.sections.reduce(
        (a: Field[], b: FormSection): Field[] => [...a, ...b.fields],
        []
      ) || [];
    const parameters = getSearchParameters(newFormValues, allFields);
    const toSend: SearchParam = {
      parameters,
      datasetName: null,
      folderPath: null,
      comments: '',
    };
    onValuesChanged(toSend);
  }, [formDefinition, newFormValues, onValuesChanged]);

  const callGetDependedLookups = (): void => {
    getDependedLookups();
  };

  useEffect(callGetDependedLookups, [newFormValues]);

  const getDependedLookups = async (): Promise<void> => {
    if (!newFormValues || !customSearchParameters) return;
    //eslint-disable-next-line
    const loader = new AxiosLoader<any, {}>();
    const dependendParameters = customSearchParameters.filter(
      (cus) => cus.parameterExtensions?.parameterValues?.length
    );
    const newCustomSearchParameters = [...customSearchParameters];
    for (const cus of dependendParameters) {
      const data = await loader.PutInfo(
        `CustomSearches/GetCustomSearchParameterLookupValues/${searchId}/${cus.description === 'Sub Area' ? 'SubArea' : cus.description}`,
        cus.parameterExtensions?.parameterValues?.map((p) => ({
          Name: p.Name,
          Value: newFormValues[p.Name] ?? p.DefaultValue,
        })) ?? [],
        {}
      );
      if (!data.results.length) return;

      const index = newCustomSearchParameters.findIndex(
        (nw) => nw.description === cus.description
      );
      if (index !== -1) {
        newCustomSearchParameters[index].lookupValues = [...data.results];
        newCustomSearchParameters[index].defaultValue =
          data.results[0]?.Value ?? 1;
      }
    }
    const formDef = getFormDefinition(newCustomSearchParameters);
    if (!_.isEqual(formDef, formDefinition)) {
      setFormDefinition(formDef);
    }
  };

  useEffect(() => {
    let fetching = false;
    const fetchData = async (): Promise<void> => {
      if (fetching) return;
      fetching = true;
      const loader = new AxiosLoader<
        { customSearchParameters: CustomSearchParameter[] },
        {}
      >();
      const data = await loader.GetInfo(
        `CustomSearches/GetCustomSearchParameters/${searchId}`,
        {
          includeLookupValues: true,
        }
      );
      if (!data) return;
      const formDef = getFormDefinition(data.customSearchParameters);
      setCustomSearchParameters(data.customSearchParameters);
      setFormDefinition(formDef);
      fetching = false;
    };
    fetchData();
  }, [searchId]);

  useEffect(() => {
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
  }, [formDefinition]);

  // useEffect(() => {
  //   setFormKey(uuidv4());
  // }, [formData]);

  if (!formDefinition) return <Loading />;

  if (!formData || !formDefinition) return <Loading />;

  const manageDataChange = (formValues: FormValues): void => {
    if (_.isEqual(formValues, newFormValues)) return;
    setNewFormValues(formValues);
  };

  return (
    <Fragment>
      {/* <pre>{JSON.stringify({ data, formDefinition }, null, 3)}</pre> */}
      <div style={{ height: 10 }}></div>
      <div style={{ borderLeft: '1px solid silver' }}>
        <FormBuilder
          key={formKey}
          onDataChange={manageDataChange}
          onValidChange={onValidChanged}
          formData={formData}
          formInfo={formDefinition}
        />
      </div>
    </Fragment>
  );
};

export default SearchReader;

function getFieldValue(
  type: string | undefined,
  value: string | number | boolean | Date | string[] | VariableValue[] | null,
  defaultValue: unknown
): unknown {
  if (value === '') return defaultValue;
  if (type === 'Int16' || type === 'Int32' || type === 'Int64') {
    return +`${value}`;
  }
  return value;
}
