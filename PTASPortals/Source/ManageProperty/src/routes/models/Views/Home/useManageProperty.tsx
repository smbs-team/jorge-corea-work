// useManagePropertyType.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { useContext } from 'react';
import { Props, ManagePropertyContext } from 'contexts/ManageProperty';
import { AppContext } from 'contexts/AppContext';
import { taskService } from 'services/api/apiService/task';
import { apiService } from 'services/api/apiService';
import { Task } from 'routes/models/Views/HomeDestroyedProperty/Task';
import { hiApiService } from 'services/api/apiService/homeImprovement';
import { currentUseService } from 'services/api/apiService/currentUse';
import { HomeImprovementApplication } from '../HomeImprovement/model/homeImprovementApplication';

// json file to get currentUseAppId (temp)
interface ExemptionData {
  currentUseApplicationId: string;
  exemptionId: string;
  fileAttachmentIds: string[];
}
interface JsonRelations {
  exemptionData: ExemptionData[];
}

interface ManagePropertyType extends Props {
  loadDestroyedPropertyApp: () => Promise<void>;
  loadHomeImprovementApps: () => Promise<void>;
  loadCurrentUseApp: () => Promise<void>;
  loadStates: () => Promise<void>;
}

const useManageProperty = (): ManagePropertyType => {
  const context = useContext(ManagePropertyContext);
  const { portalContact } = useContext(AppContext);

  const {
    selectedParcels,
    setStateCodeDestroyedProperty,
    setDestroyedPropertyApp,
    setStatusCodeDestroyProperty,
    setHomeImprovementApp,
    setStateCodeHI,
    setStatusCodeHI,
    setCurrentUseApp,
    setCurrentUseAppState,
  } = context;

  const getOptionSetMapping = async (
    entityName: string,
    fieldName: string
  ): Promise<Map<string, number>> => {
    const { hasError, data } = await apiService.getOptionSet(
      entityName,
      fieldName
    );

    if (hasError || !data) {
      return new Map<string, number>();
    }

    const optionSetMap = new Map<string, number>(
      data.map((code) => [code.value, code.attributeValue])
    );

    return optionSetMap;
  };

  /**
   * loads app statuses:
   * - home improvements
   * - destroyed property
   */
  const loadStates = async (): Promise<void> => {
    // destroyed property states
    const state = await getOptionSetMapping('ptas_task', 'statecode');

    setStateCodeDestroyedProperty(state);
    const status = await getOptionSetMapping('ptas_task', 'statuscode');

    setStatusCodeDestroyProperty(status);

    // home improvement states
    const stateHi = await getOptionSetMapping(
      'ptas_homeimprovement',
      'statecode'
    );
    setStateCodeHI(stateHi);
    const statusHi = await getOptionSetMapping(
      'ptas_homeimprovement',
      'statuscode'
    );
    setStatusCodeHI(statusHi);

    // currentUse state
    const statusCu = await getOptionSetMapping(
      'ptas_currentuseapplication',
      'statuscode'
    );
    setCurrentUseAppState(statusCu);
  };

  const loadDestroyedPropertyApp = async (): Promise<void> => {
    const contactId = portalContact?.id;

    if (!contactId || !selectedParcels) return;

    const {
      data: appsFound,
    } = await taskService.getDestroyedPropByParcelAndContact(
      selectedParcels,
      contactId
    );

    if (!appsFound || !appsFound.length) {
      return setDestroyedPropertyApp(new Task());
    }

    setDestroyedPropertyApp(appsFound[0]);
  };

  const loadHomeImprovementApps = async (): Promise<void> => {
    const contactId = portalContact?.id;

    if (!contactId || !selectedParcels) return;

    const {
      data: appsFound,
    } = await hiApiService.getHomeImprovementAppByParcel(selectedParcels);

    console.debug(
      'loadHomeImprovementApps parcel:',
      selectedParcels,
      'appsFound:',
      appsFound
    );

    // const activeApps = appsFound.filter(
    //   (app): boolean => app.stateCode === stateCodeHi.get(STATE_CODE_ACTIVE)
    // );

    // if (!activeApps || !activeApps.length) return;
    if (!appsFound || !appsFound.length) {
      setHomeImprovementApp(new HomeImprovementApplication());
      return;
    }

    //Exclude applications older than five years
    const recentApp = appsFound.find((app) => {
      const yearDiff = new Date().getFullYear() - app.exemptionBeginYear;
      return yearDiff <= 5;
    });
    if (!recentApp) return;

    setHomeImprovementApp(recentApp);
  };

  const loadCurrentUseApp = async (): Promise<void> => {
    let currentUseFound = undefined;
    if (portalContact?.id && selectedParcels) {
      const jsonRelationsUrl = `portals/currentuse/${
        portalContact?.id ?? ''
      }/relations`;
      const { data } = await apiService.getJson(
        `${jsonRelationsUrl}/relations`
      );
      if (data) {
        const json = (data as unknown) as JsonRelations;
        const currentUseAppId = (json?.exemptionData ?? []).find(
          (d) => d.exemptionId === selectedParcels
        )?.currentUseApplicationId;
        if (currentUseAppId) {
          currentUseFound = await (
            await currentUseService.getCurrentUse(currentUseAppId)
          ).data;
        }
      }
    }
    setCurrentUseApp(currentUseFound);
  };

  return {
    loadDestroyedPropertyApp,
    loadHomeImprovementApps,
    loadCurrentUseApp,
    loadStates,
    ...context,
  };
};

export default useManageProperty;
