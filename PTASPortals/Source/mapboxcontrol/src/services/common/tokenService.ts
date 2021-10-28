// tokenService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { Subject } from 'rxjs';
import { ApiServiceResult, handleReq } from './httpClient';
import * as jwt from 'jsonwebtoken';
import axios from 'axios';

class TokenService {
  aadToken?: string;
  b2cToken?: string;
  $onB2CTokenInit = new Subject<string>();
  $onInvalidToken = new Subject<void>();

  constructor() {
    this.aadToken = localStorage.getItem('tokenUrl') ?? undefined;
    this.b2cToken = localStorage.getItem('magicToken') ?? undefined;

    if (this.b2cToken) {
      this.$onB2CTokenInit.next(this.b2cToken);
    }
  }

  async exchangeToken(tokenUrl: string): Promise<void> {
    if (!tokenUrl || this.aadToken === tokenUrl) {
      //Check if b2cToken expired
      const decoded = jwt.decode(this.b2cToken ?? '');
      if (
        !decoded ||
        typeof decoded !== 'object' ||
        !decoded.exp ||
        Date.now() >= decoded.exp * 1000
      ) {
        this.$onInvalidToken.next();
      }
      this.$onB2CTokenInit.next(this.b2cToken);
      return;
    }
    localStorage.setItem('tokenUrl', tokenUrl);
    this.aadToken = tokenUrl;

    const tokenRes = await this.getMagicLinkToken(tokenUrl);
    localStorage.setItem('magicToken', tokenRes.data ?? '');
    this.b2cToken = tokenRes.data;
    if (!this.b2cToken) {
      this.$onInvalidToken.next();
      return;
    }
    this.$onB2CTokenInit.next(this.b2cToken);
  }

  async getMagicLinkToken(
    accessToken: string
  ): Promise<ApiServiceResult<string | undefined>> {
    const headers = {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + accessToken,
    };

    return handleReq(async () => {
      const url = process.env.REACT_APP_MAGICLINK_URL ?? '';
      const tokenRes = (
        await axios.get<string>(url, {
          headers,
        })
      ).data;

      return new ApiServiceResult<string | undefined>({
        data: tokenRes,
      });
    });
  }
}
export const tokenService = new TokenService();
