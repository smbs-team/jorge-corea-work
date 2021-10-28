/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import {
  apiService,
  GlobalStoreItemId,
  GlobalStoreItemValue,
} from 'services/api';
import { UserInfo } from './userInfo';

type AppTeam = 'Residential Appraisers' | 'Commercial Appraisers';

class UserService {
  userInfo: UserInfo = new UserInfo();
  adminRoles: string[] = [];

  initUserInfo = async (): Promise<void> => {
    this.loadAdminRoles();
    const userInfoRes = await apiService.getCurrentUserInfo();
    if (!userInfoRes.data) {
      console.error('Error getting the current user information token');
    } else {
      this.userInfo.id = userInfoRes.data.id;
      this.userInfo.fullName = userInfoRes.data.fullName;
      this.userInfo.email = userInfoRes.data.email;
      this.userInfo.roles = userInfoRes.data.roles;
      this.userInfo.teams = userInfoRes.data.teams;
      if (
        !this._hasTeam('Commercial Appraisers') &&
        !this._hasTeam('Residential Appraisers')
      ) {
        console.warn('Residential or commercial team required');
      }
    }
  };

  isAdminUser = (): boolean => {
    return this.userInfo.roles.some((userRole) =>
      this.adminRoles.includes(userRole.name)
    );
  };

  loadAdminRoles = (): void => {
    if (process.env.REACT_APP_ADMIN_ROLES) {
      process.env.REACT_APP_ADMIN_ROLES.split(',').forEach((role) =>
        this.adminRoles.push(role)
      );
    }
  };

  private _hasTeam = (name: AppTeam): boolean =>
    !!this.userInfo.teams.find((team) => team.name === name);

  getFallBackDefaultMap = async (): Promise<number | undefined> => {
    const fn = async (id: GlobalStoreItemId): Promise<number | undefined> => {
      const res = await apiService.metaStore.getGlobalStoreItem<
        GlobalStoreItemValue[typeof id]
      >(id);
      if (res.hasError) {
        console.error('Error getting default map');
        return;
      }
      return res.data?.id;
    };
    if (this._hasTeam('Residential Appraisers')) {
      return fn('default-map-r');
    }
    if (this._hasTeam('Commercial Appraisers')) {
      return fn('default-map-c');
    }
    console.error('Residential or commercial team required');
  };
}

export default new UserService();
