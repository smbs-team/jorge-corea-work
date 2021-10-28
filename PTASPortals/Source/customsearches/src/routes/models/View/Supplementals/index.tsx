// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React, { Fragment, useContext, useEffect, useState } from 'react';
import {
  GenericGridRowData,
  IdOnly,
  PostProcess,
  Project,
  ProjectDataset,
} from 'services/map.typings';
// import DatasetViewer from '../DatasetViewer';
import {
  FormDefinition,
  FormValues,
  Params,
  VariableValue,
} from 'components/FormBuilder/FormBuilder';
import FormBuilder from 'components/FormBuilder';
import { ProjectContext } from 'context/ProjectsContext';
import Loading from 'components/Loading';
import { ReactComponent as Beaker } from '../../../../assets/images/icons/beaker.svg';
import ModelDetailsHeader from 'routes/models/ModelDetailsHeader';
import { AxiosLoader } from 'services/AxiosLoader';
import { useHistory, useLocation, useParams } from 'react-router-dom';
import { LoadPostProcess, LoadProject } from '../Regression/common';
import { AppContext } from 'context/AppContext';
import { deleteDatasetPostProcess } from 'services/common';
import { SalesDropdown } from '../Regression/SalesDropdown';
import { RegressionGrid } from '../Regression/RegressionGrid';
import AgGridService from 'services/AgGridService';
import ErrorMessage, {
  ImportErrorMessageType,
} from 'components/ErrorMessage/ErrorMessage';

import _ from 'underscore';
import { RunningRegression } from '../Projects/Land/RunningRegression';

const gridData: GenericGridRowData[] = [
  {
    name: 'NewEMV',
    type: 'Calculated',
  },
  {
    name: 'Supplemental',
    type: 'Calculated',
  },
  {
    name: 'Exception',
    type: 'Calculated',
  },
  {
    name: 'Major',
    type: 'Independent',
  },
  {
    name: 'Minor',
    type: 'Independent',
  },
];

const gridDataCreate: GenericGridRowData[] = [
  {
    name: 'NewEMV',
    type: 'Calculated',
  },
  {
    name: 'Major',
    type: 'Independent',
  },
  {
    name: 'Minor',
    type: 'Independent',
  },
];

const suppForm: FormDefinition = {
  sections: [
    {
      title: 'Supplementals',
      className: 'with-large-title with-default-field-headers',
      fields: [
        {
          type: 'grid',
          fieldName: 'conditions',
          itemTemplates: [
            {
              newItem: { action: 'EMV', filter: '', expression: '', note: '' },
              title: 'New Condition',
            },
          ],
          vars: [
            {
              flex: 1,
              fieldName: 'action',
              title: 'Action',
              values: ['EMV', 'Supplemental'],
              columnColor: 'calculated',
            },
            {
              flex: 3,
              fieldName: 'filter',
              title: 'Filter',
              required: true,
            },
            {
              flex: 3,
              fieldName: 'expression',
              title: 'Expression',
              required: true,
            },
            { flex: 1, fieldName: 'note', title: 'Note', required: false },
          ],
        },
      ],
    },
  ],
};

interface SupplementalLocationState {
  jobId: number;
}

const styles = {
  width: '100%',
  borderBottom: '4px solid #333333',
  borderTop: '4px solid #333333',
};

let debounced: (() => Promise<void>) & _.Cancelable;

const Supplementals = (): JSX.Element => {
  const { id, regressionid }: { id: string; regressionid: string } =
    useParams();
  const location = useLocation<SupplementalLocationState>();
  const appContext = useContext(AppContext);
  const history = useHistory();
  const [isDatasetChanged, setChangeDataset] = useState<boolean>(false);

  const [isValidated, setIsValidated] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<ImportErrorMessageType>();
  const [runningJobId, setRunningJobId] = useState<boolean>(false);
  const [disableButton, setDisableButton] = useState<boolean>(true);
  const [jobId, setJobId] = useState<number>(0);

  const [formData, setFormData] = useState<FormValues>({});
  const [secondaryDatasets, setSecondaryDatasets] = useState<string[]>([]);
  const [datasetId, setDatasetId] = useState<string>('');

  const [datasets, setDatasets] = useState<ProjectDataset[]>([]);
  const context = useContext(ProjectContext);
  const [loading, setLoading] = useState<boolean>(false);
  const [postProcess, setPostProcess] = useState<PostProcess>();

  useEffect(() => {
    const validateJobId = async (): Promise<void> => {
      const jobLocalId = location.state?.jobId;
      if (jobLocalId) {
        setRunningJobId(true);
        try {
          const jobStatus = await AgGridService.getJobStatus(jobLocalId);
          const status = jobStatus?.jobStatus === 'Finished';
          if (!status) {
            debounced = _.debounce(validateJobId, 5000);
            debounced();
          } else {
            setRunningJobId(false);
          }
          setErrorMessage(undefined);
        } catch (error) {
          if (error?.message && error?.validationError) {
            setErrorMessage({
              message: error?.message,
              reason: error?.validationError || [],
            });
          } else {
            setErrorMessage({
              message: error,
              reason: [],
            });
          }
        }
      }
    };
    validateJobId();
    //eslint-disable-next-line
  }, [location]);

  useEffect(() => {
    const validateJobId = async (): Promise<void> => {
      const jobLocalId = jobId;
      if (jobLocalId) {
        setRunningJobId(true);
        try {
          const jobStatus = await AgGridService.getJobStatus(jobLocalId);
          const status = jobStatus?.jobStatus === 'Finished';
          if (!status) {
            debounced = _.debounce(validateJobId, 5000);
            debounced();
          } else {
            setRunningJobId(false);
          }
          setErrorMessage(undefined);
        } catch (error) {
          if (error?.message && error?.validationError) {
            setErrorMessage({
              message: error?.message,
              reason: error?.validationError || [],
            });
          } else {
            setErrorMessage({
              message: error,
              reason: [],
            });
          }
        }
      }
    };
    validateJobId();
    //eslint-disable-next-line
  }, [jobId]);

  const [project, setProject] = useState<Project>();

  useEffect(() => {
    const fetchData = async (): Promise<void> => {
      const project = await LoadProject(id, true);
      if (project) {
        setProject(project.project);
        const datasets = project.project.projectDatasets;
        setDatasets(project.project.projectDatasets);
        let datasetId = datasets[0].datasetId;
        let secondaryDataset: string[] = [];
        if (!regressionid) {
          datasets.forEach((d: ProjectDataset) => {
            const pp = d.dataset.dependencies?.postProcesses?.find(
              (p) =>
                p.priority === 3000 && p.primaryDatasetPostProcessId === null
            );
            if (pp) {
              datasetId = pp.datasetId;
              if (pp.secondaryDatasets?.length)
                secondaryDataset = pp.secondaryDatasets;
            }
          });
        } else {
          datasets.forEach((d: ProjectDataset) => {
            const pp = d.dataset.dependencies?.postProcesses?.find(
              (p) =>
                p.postProcessRole === 'SupplementalAndException' &&
                p.primaryDatasetPostProcessId === null
            );
            if (pp) {
              datasetId = pp.datasetId;
              if (pp.secondaryDatasets?.length)
                secondaryDataset = pp.secondaryDatasets;
            }
          });
        }
        setDatasetId(datasetId);
        setSecondaryDatasets(secondaryDataset);
      }
    };
    fetchData();
    //eslint-disable-next-line
  }, [id]);

  const fetchPostProcessData = async (): Promise<void> => {
    const pp = await LoadPostProcess(regressionid);
    const conditions: VariableValue[] = [];
    // const comment = `${pp?.postProcessName}`;
    pp && setPostProcess(pp);
    if (Array.isArray(pp?.exceptionPostProcessRules))
      pp?.exceptionPostProcessRules
        .filter((r) => r.customSearchExpressions[0].script !== 'default')
        ?.forEach((exp) => {
          const item = {
            action:
              exp.customSearchExpressions[0].expressionExtensions?.action ||
              'EMV',
            filter: exp.customSearchExpressions[0].script,
            expression: exp.customSearchExpressions[1].script,
            note: exp.description,
          };
          conditions.push(item);
        });

    setFormData({
      ...formData,
      // Comments: comment,
      conditions,
    });
  };

  //eslint-disable-next-line
  const callPostProcess = () => {
    if (regressionid) {
      fetchPostProcessData();
    }
    return (): void => {
      debounced?.cancel();
    };
  };

  useEffect(() => {
    const validateDatasetSelected = (): void => {
      if (!datasetId.length) return;
      if (!isDatasetChanged) return;
      if (
        datasets.some(
          (d) => d.datasetRole === 'Population' && d.datasetId === datasetId
        )
      ) {
        setSecondaryDatasets([]);
      } else {
        setSecondaryDatasets(
          datasets
            .filter((d) => d.datasetRole === 'Population')
            .map((d) => d.datasetId)
        );
      }
    };
    validateDatasetSelected();
    //eslint-disable-next-line
  }, [datasetId, isDatasetChanged]);

  useEffect(callPostProcess, []);

  const isValidData = (): void => {
    if (Object.values(formData).length) {
      if (getValidForm()) {
        setIsValidated(true);
      }
    }
  };

  const getValidForm = (): number | false =>
    // typeof formData?.Comments === 'string' &&
    // formData?.Comments?.length &&
    Array.isArray(formData?.conditions) && formData?.conditions?.length;

  useEffect(isValidData, [formData]);

  const handleFormDataChange = (newData: FormValues): void => {
    if (Object.values(newData).length) {
      setFormData(newData);
    }
  };

  if (!context?.modelDetails) return <Loading />;

  const addPostProcessUpdate = (jobId: string): void => {
    const postProcessId = parseInt(regressionid);
    const pp = appContext.postProcessInProgress ?? [];
    const index = pp?.findIndex((p) => p.postProcessId === postProcessId);
    if (index !== -1) {
      pp[index].jobId = parseInt(jobId);
      if (pp) appContext.setPostProcessInProgress?.([...pp]);
      return;
    }
    appContext?.handlePostProcessInProgress?.({
      jobId: parseInt(jobId),
      postProcessId: postProcessId,
      datasetId: datasetId,
      postProcessRole: 'supplementalandexception',
    });
  };

  const runTest = async (): Promise<void> => {
    if (!formData) return;
    const pop = getPopulation(context.modelDetails);
    if (!pop) return;
    try {
      const toSend = createSendValues(
        datasetId,
        formData,
        secondaryDatasets,
        regressionid
      );
      const loader = new AxiosLoader<IdOnly, {}>();
      setLoading(true);
      if (
        toSend?.exceptionPostProcessRules &&
        Array.isArray(toSend?.exceptionPostProcessRules) &&
        toSend?.exceptionPostProcessRules?.length === 0
      ) {
        try {
          await deleteDatasetPostProcess(regressionid);
          setTimeout(() => {
            history.push(`/models/view/${id}`);
          }, 2000);
          setErrorMessage(undefined);
        } catch (error) {
          if (error?.message && error?.validationError) {
            setErrorMessage({
              message: error?.message,
              reason: error?.validationError || [],
            });
          } else {
            setErrorMessage({
              message: error,
              reason: [],
            });
          }
        } finally {
          setLoading(false);
        }
      } else {
        const postprocessInfo = await loader.PutInfo(
          `CustomSearches/ImportExceptionPostProcess`,
          toSend,
          {}
        );
        const executer = new AxiosLoader<IdOnly, {}>();
        const job = await executer.PutInfo(
          `CustomSearches/ExecuteDatasetPostProcess/${datasetId}/${postprocessInfo?.id}`,
          [
            {
              Id: 0,
              Name: '',
              Value: '',
            },
          ],
          {}
        );
        setJobId(parseInt(`${job?.id}`));
        if (!regressionid) {
          appContext?.handlePostProcessInProgress?.({
            jobId: parseInt(`${job?.id}`),
            postProcessId: parseInt(postprocessInfo?.id ?? '0'),
            datasetId: datasetId,
            postProcessRole: 'supplementalandexception',
          });
          history.push({
            pathname: `/models/supplementals_edit/${id}/${postprocessInfo?.id}`,
            state: {
              jobId: parseInt(`${job?.id}`),
            },
          });
        } else {
          addPostProcessUpdate(job?.id ?? '');
        }
      }
      setErrorMessage(undefined);
    } catch (error) {
      if (error?.length) {
        const findDataset = datasets.find((d) => error?.includes(d.datasetId));
        const message = error?.replace(
          findDataset?.datasetId,
          findDataset?.dataset.datasetName
        );
        appContext.setSnackBar?.({
          severity: 'error',
          text: message,
        });
      }
      setErrorMessage({
        message: error?.message ?? error,
        reason: error?.validationError || [],
      });
    } finally {
      setLoading(false);
    }
  };

  const getSecondaryDataset = (datasets: string[]): void => {
    setSecondaryDatasets(datasets);
  };

  const getDatasetId = (id: string): void => {
    if (!isDatasetChanged) return;
    setDatasetId(id);
  };

  const onChange = (): void => {
    setDisableButton(false);
  };

  const onChangeDataset = (): void => {
    setChangeDataset(true);
  };

  const renderGrid = (): JSX.Element => {
    if (postProcess && project) {
      return (
        <RegressionGrid
          project={project}
          datasets={datasets?.filter((s) =>
            [datasetId, ...secondaryDatasets].includes(s.datasetId)
          )}
          postProcess={postProcess}
          gridData={gridData}
          datasetId={datasetId}
          reloadGrid={`${datasetId}-${runningJobId}`}
          edit
        />
      );
    }
    return (
      <RegressionGrid
        datasets={datasets}
        datasetId={datasetId}
        gridData={gridDataCreate}
        // reloadGrid={datasetId}
      />
    );
  };

  if (loading) return <Loading />;

  return (
    <Fragment>
      <RunningRegression runningRegression={runningJobId} />
      <ModelDetailsHeader
        modelDetails={context.modelDetails}
        links={[<span>Supplementals</span>]}
        icons={[
          {
            icon: <Beaker />,
            disabled: !isValidated && disableButton,
            text: 'Save supplemental',
            onClick: runTest,
          },
        ]}
      />
      {datasets.length && (
        <SalesDropdown
          edit={postProcess ? true : false}
          datasetId={datasetId}
          options={datasets}
          getDatasetId={getDatasetId}
          getSecondaryDataset={getSecondaryDataset}
          selectedValues={secondaryDatasets}
          onChange={onChange}
          changeDataset={onChangeDataset}
          role={'MultipleRegression'}
          previousModel="EMV Model"
          actualModel="Supplementals"
        />
      )}
      <div style={styles}>
        <FormBuilder
          formInfo={suppForm}
          formData={formData}
          onDataChange={handleFormDataChange}
          isLocked={context.modelDetails.isLocked}
        />
      </div>
      <ErrorMessage message={errorMessage} />
      {renderGrid()}

      {/* <DatasetViewer
        priorVars={[]}
        postVars={[]}
        datasets={
          getPopulation(context.modelDetails).map((ds: ProjectDataset) => ({
            id: ds.datasetId || '',
            description: ds.datasetRole || '',
          })) || []
        }
      ></DatasetViewer> */}
    </Fragment>
  );
};
export default Supplementals;

function createSendValues(
  datasetId: string,
  currvars: FormValues,
  secondaryDatasets: string[],
  _regressionid: string | undefined
): Params {
  const getAction = (action: string): string => {
    if (action === 'EMV') return 'NewEMV';
    return action;
  };
  const rules = (currvars.conditions as VariableValue[]).map((t, index) => ({
    description: (t.note || 'Supplemental ') + index,
    customSearchExpressions: [
      {
        expressionType: 'TSQL',
        expressionRole: 'FilterExpression',
        script: t.filter.trim(),
        columnName: getAction(t.action),
        note: t.note,
        expressionExtensions: {
          action: t.action,
        },
      },
      {
        expressionType: 'TSQL',
        expressionRole: 'CalculatedColumn',
        script: t.expression.trim(),
        columnName: getAction(t.action),
        note: t.note,
      },
    ],
  }));
  rules.push({
    description: 'Supplemental default',
    customSearchExpressions: [
      {
        expressionType: 'TSQL',
        expressionRole: 'FilterExpression',
        script: 'default',
        columnName: 'Supplemental',
        note: 'Supplemental',
      },
      {
        expressionType: 'TSQL',
        expressionRole: 'CalculatedColumn',
        script: 'null',
        columnName: 'Supplemental',
        note: 'Supplemental',
      },
    ],
  });

  rules.push({
    description: 'Exception default',
    customSearchExpressions: [
      {
        expressionType: 'TSQL',
        expressionRole: 'FilterExpression',
        script: 'default',
        columnName: 'Exception',
        note: 'Exception',
      },
      {
        expressionType: 'TSQL',
        expressionRole: 'CalculatedColumn',
        script: 'null',
        columnName: 'Exception',
        note: 'Exception',
      },
    ],
  });

  return {
    datasetId: datasetId,
    postProcessName: 'Supplementals',
    postProcessRole: 'SupplementalAndException',
    priority: 4000,
    postProcessDefinition: 'Supplemental and exceptions',
    PostProcessSubType: 'UniqueConditionSelector',
    secondaryDatasets: secondaryDatasets,
    exceptionPostProcessRules: rules,
  };
}

function getPopulation(project: Project | null | undefined): ProjectDataset[] {
  return (
    project?.projectDatasets.filter(
      (pds) => pds.datasetRole === 'Population'
    ) || []
  );
}
