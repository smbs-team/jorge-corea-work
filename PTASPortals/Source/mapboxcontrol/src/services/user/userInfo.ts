// userInfo.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export class UserInfo {
  id: string;
  fullName: string;
  email: string;
  roles: Role[];
  teams: Team[];

  constructor(fields?: UserInfo) {
    this.id = fields ? fields.id : '';
    this.fullName = fields ? fields.fullName : '';
    this.email = fields ? fields.email : '';
    this.roles = fields ? fields.roles : [];
    this.teams = fields ? fields.teams : [];
  }
}

export interface Role {
  id: string;
  name: string;
}

export interface Team {
  id: string;
  name: string;
}
