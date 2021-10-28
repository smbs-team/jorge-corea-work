/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, {
  createContext,
  ComponentType,
  FC,
  PropsWithChildren,
  useState,
  Dispatch,
  SetStateAction,
  useRef,
  MutableRefObject,
  useContext,
  useEffect,
  Key,
} from 'react';
import {
  ColorConfiguration,
  ErrorMessageAlertCtx,
} from '@ptas/react-ui-library';
import {
  RendererDataSetColumn,
  Rule,
  EmbeddedDataFields,
  RendererDataset,
  layerService,
} from 'services/map';

import { BreakOptionKey, RuleType } from 'services/map/model';
import {
  RendererClassBreakRule,
  RendererSimpleRule,
  RendererUniqueValuesRule,
} from 'services/map/model';
import { useIsParcelRenderer } from 'hooks/map/useIsParcelRenderer';
import { RendererLabel } from 'services/map/renderer/textRenderer/textRenderersService';
import { HomeContext } from 'contexts';
import { useAsync, useUpdateEffect } from 'react-use';
import { apiService } from 'services/api';
import { getErrorStr } from 'utils/getErrorStr';

interface ColorConfigState {
  colorConfigFill: ColorConfiguration | undefined;
  colorConfigOutline: ColorConfiguration | undefined;
  count: number;
}

export interface FieldRangeValues {
  min: number;
  max: number;
}

export interface RuleContextProps {
  ruleType: RuleType | undefined;
  setRuleType: Dispatch<SetStateAction<RuleType | undefined>>;
  datasetId?: string;
  setDatasetId: Dispatch<SetStateAction<string | undefined>>;
  datasetField: RendererDataSetColumn | undefined;
  embeddedDataField: EmbeddedDataFields | undefined;
  setEmbeddedDataField: Dispatch<
    SetStateAction<EmbeddedDataFields | undefined>
  >;
  setDatasetField: Dispatch<SetStateAction<RendererDataSetColumn | undefined>>;
  rule:
    | RendererSimpleRule
    | RendererClassBreakRule
    | RendererUniqueValuesRule
    | undefined;
  setRule: Dispatch<
    SetStateAction<
      | RendererSimpleRule
      | RendererClassBreakRule
      | RendererUniqueValuesRule
      | undefined
    >
  >;
  label: RendererLabel | undefined;
  setLabel: Dispatch<SetStateAction<RendererLabel | undefined>>;
  fieldRangeValues: Record<string, FieldRangeValues> | undefined;
  setFieldRangeValues: Dispatch<
    React.SetStateAction<Record<string, FieldRangeValues> | undefined>
  >;
  labelDefinitionVisible: boolean;
  setLabelDefinitionVisible: Dispatch<React.SetStateAction<boolean>>;
  stDeviationInterval: number;
  setStDeviationInterval: Dispatch<SetStateAction<number>>;
  cbRuleRef: MutableRefObject<RendererClassBreakRule | undefined> | undefined;
  cbInitColorRampRef: MutableRefObject<boolean> | undefined;
  userRendererDatasets: RendererDataset[] | undefined;
  setUserRendererDatasets: Dispatch<
    SetStateAction<RendererDataset[] | undefined>
  >;
  dataSetColumns: RendererDataSetColumn[];
  setDataSetColumns: Dispatch<SetStateAction<RendererDataSetColumn[]>>;
  embeddedDataFields: EmbeddedDataFields[];
  setDataSetColumnValues: Dispatch<SetStateAction<Key[] | undefined>>;
  dataSetColumnValues: Key[] | undefined;
  breakOption: BreakOptionKey | undefined;
  colorConfig: ColorConfigState | undefined;
  numberOfBreaks: number;
  selectedRampTab: number | undefined;
  setSelectedRampTab: Dispatch<SetStateAction<number>>;
  setBreakOption: Dispatch<BreakOptionKey>;
  setColorConfig: Dispatch<SetStateAction<ColorConfigState>>;
  setNumberOfBreaks: Dispatch<SetStateAction<number>>;
  currentLayerId: string | undefined;
  colorRendererSectionRef:
    | React.MutableRefObject<HTMLDivElement | null>
    | undefined;
  rendError: string | undefined;
  setRendError: React.Dispatch<React.SetStateAction<string | undefined>>;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const RuleContext = createContext<RuleContextProps>(null as any);

function RuleProvider(props: PropsWithChildren<object>): JSX.Element {
  const [rendError, setRendError] = useState<string>();
  const { showErrorMessage } = useContext(ErrorMessageAlertCtx);
  const colorRendererSectionRef = useRef<HTMLDivElement | null>(null);
  const { currentLayer, colorRuleAction } = useContext(HomeContext);
  const [currentLayerId, setCurrentLayerId] = useState<string>();
  const isParcelRenderer = useIsParcelRenderer();
  const [dataSetColumnValues, setDataSetColumnValues] = useState<
    RuleContextProps['dataSetColumnValues']
  >();
  const [embeddedDataFields, setEmbeddedDataFields] = useState<
    EmbeddedDataFields[]
  >([]);
  const [dataSetColumns, setDataSetColumns] = useState<RendererDataSetColumn[]>(
    []
  );
  const [userRendererDatasets, setUserRendererDatasets] = useState<
    RendererDataset[]
  >();
  const [datasetId, setDatasetId] = useState<string>();
  const [datasetField, setDatasetField] = useState<RendererDataSetColumn>();
  const [embeddedDataField, setEmbeddedDataField] = useState<
    EmbeddedDataFields
  >();
  const [ruleType, setRuleType] = useState<RuleType>();
  const [rule, setRule] = useState<Rule>();
  const [label, setLabel] = useState<RendererLabel>();
  const [fieldRangeValues, setFieldRangeValues] = useState<
    Record<string, FieldRangeValues>
  >();
  const [labelDefinitionVisible, setLabelDefinitionVisible] = useState<boolean>(
    false
  );
  const [stDeviationInterval, setStDeviationInterval] = useState(1);
  const [breakOption, setBreakOption] = useState<BreakOptionKey>(
    BreakOptionKey.equalInterval
  );
  const [colorConfig, setColorConfig] = useState<ColorConfigState>({
    colorConfigFill: undefined,
    colorConfigOutline: undefined,
    count: 0,
  });
  const [numberOfBreaks, setNumberOfBreaks] = useState<number>(3);
  const [selectedRampTab, setSelectedRampTab] = useState(0);
  const cbRuleRef = useRef<RendererClassBreakRule | undefined>();

  /**
   * Ref used in CB rules to check whether a default color ramp should be set
   * Should only be used when color renderer action is 'create'
   */
  const cbInitColorRampRef = useRef<boolean>(false);

  useEffect(() => {
    setCurrentLayerId(currentLayer?.rendererRules.layer.id);
  }, [currentLayer]);

  useEffect(() => {
    if (isParcelRenderer || !currentLayerId) return setEmbeddedDataFields([]);

    const _embeddedDataFields =
      layerService.layersConfiguration[currentLayerId ?? '']
        ?.embeddedDataFields ?? [];
    return setEmbeddedDataFields(_embeddedDataFields);
  }, [currentLayerId, isParcelRenderer]);

  /**
   * On embedded data fields changed, set embedded data field
   */
  useUpdateEffect(() => {
    if (!embeddedDataFields) return;
    if (!colorRuleAction) return;
    if (colorRuleAction === 'update') {
      if (
        currentLayer &&
        currentLayer.mapRendererId &&
        currentLayer.rendererRules.colorRule?.rule
      ) {
        const field = embeddedDataFields.find(
          (item) =>
            item.FieldName ===
            currentLayer.rendererRules.colorRule?.rule.columnName
        );
        if (!field) return;
        setEmbeddedDataField(field);
        return;
      }
    } else {
      setEmbeddedDataField(embeddedDataFields[0]);
    }
  }, [colorRuleAction, embeddedDataFields]);

  /** Set dataset column lookup values for simple and unique renderer */
  useAsync(async () => {
    try {
      if (!datasetField) return;
      if (!ruleType) return;
      if (
        datasetField?.canBeUsedAsLookup &&
        ['Simple', 'Unique'].includes(ruleType)
      ) {
        const values = await apiService.getDataSetLookUpColumnValues(
          datasetField.datasetId,
          datasetField.columnName
        );
        setDataSetColumnValues(values);
      }
    } catch (e) {
      showErrorMessage(getErrorStr(e));
    }
  }, [datasetField, ruleType, setDataSetColumnValues]);

  return (
    <RuleContext.Provider
      value={{
        colorRendererSectionRef,
        setDataSetColumnValues,
        dataSetColumnValues,
        dataSetColumns,
        embeddedDataFields,
        userRendererDatasets,
        setUserRendererDatasets,
        datasetId,
        setDatasetId,
        datasetField,
        setDatasetField,
        embeddedDataField,
        setEmbeddedDataField,
        ruleType,
        setRuleType,
        rule,
        setRule,
        label,
        setLabel,
        fieldRangeValues,
        setFieldRangeValues,
        labelDefinitionVisible,
        setLabelDefinitionVisible,
        stDeviationInterval,
        setStDeviationInterval,
        cbRuleRef,
        cbInitColorRampRef,
        breakOption,
        colorConfig,
        numberOfBreaks,
        selectedRampTab,
        setBreakOption,
        setColorConfig,
        setNumberOfBreaks,
        setSelectedRampTab,
        setDataSetColumns,
        currentLayerId,
        rendError,
        setRendError,
      }}
    >
      {props.children}
    </RuleContext.Provider>
  );
}

export const withRuleProvider = <P extends object>(
  Component: ComponentType<P>
): FC<P> => (props: PropsWithChildren<P>): JSX.Element => {
  return (
    <RuleProvider>
      <Component {...props} />
    </RuleProvider>
  );
};
