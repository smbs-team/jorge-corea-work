// utilService.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { differenceInDays, differenceInYears, isToday } from 'date-fns';

/**
 * An utility for all the app
 */
class UtilService {
  /**
   * Converts Unix timestamp string (with ms) to Date
   * @param unixTimestamp - Unix timestamp
   */
  unixTimeToDate(unixTimestamp: string): Date | undefined {
    if (unixTimestamp) {
      return new Date(Number.parseInt(unixTimestamp));
    }
    return undefined;
  }

  /**
   * Add comma separator for thousands
   * @param x - number to be converted
   * @returns string with commas (if applicable)
   */
  numberWithCommas = (x: number): string => {
    return x.toString().replace(/\B(?<!\.\d*)(?=(\d{3})+(?!\d))/g, ',');
  };

  validEmail(email: string): boolean {
    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
  }

  validPhone(phoneNumber: string): boolean {
    if (!phoneNumber) return false;

    const number = phoneNumber.split('-').join('');

    return number.trim().length >= 10;
  }

  validDate(date: Date | string): boolean {
    if (!date) return false;

    const getTime = typeof date === 'string' ? new Date(date) : date;

    return !isNaN(getTime.getTime());
  }

  validDateInRange(date: Date): boolean {
    if (Number.isNaN(date)) return false;
    if (isToday(date)) return true;

    const initialDate = new Date();

    const diffDay = differenceInDays(initialDate, date);

    if (diffDay <= 0) return false;

    const diff = differenceInYears(initialDate, date);

    return (
      diff < parseInt(process.env.REACT_APP_YEARS_RANGE ?? '0') && diff >= 0
    );
  }

  validDateStr(dateStr: string): boolean {
    const date: Date = new Date(dateStr);
    return date instanceof Date && !isNaN(date.getTime());
  }

  validNumber(num: string): boolean {
    return !isNaN(parseInt(num));
  }
}

export default new UtilService();
