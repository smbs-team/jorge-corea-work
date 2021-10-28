// FILENAME
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { ICellEditorParams } from 'ag-grid-community';

interface DropdownCellEditorParams extends ICellEditorParams {
  values: { Value: string | number; Key?: string }[];
}

export class DropdownCellEditor {
  private _eInput: HTMLSelectElement | null = null;
  cancelBeforeStart = false;
  private _value = '';

  init(params: DropdownCellEditorParams): void {
    this._value = params.value;
    const input = document.createElement('select');

    this._eInput = input;
    this._eInput.id = 'input';
    this._eInput.className = 'dropdown';
    this._eInput.value = this._value;

    // const exist = params.values.some(
    //   (ppp) => {
    //     console.log({ ppp, params });
    //     return ppp.Value === parseInt(params.value)
    //   }
    // );

    // console.log(`exists`, exist);

    // const values = exist
    //   ? params.values
    //   : [{ Key: '', Value: params.value }, ...params.values];

    const values = [{ Key: '', Value: params.value }, ...params.values];

    values
      .map((strItm) => {
        const option = document.createElement('option');
        option.value = `${strItm.Value}`;
        option.innerText = `${strItm.Key} (${option.value})`;
        return option;
      })
      .forEach((itm) => input.options.add(itm));

    //eslint-disable-next-line
    this._eInput.addEventListener('input', (event: any) => {
      console.log(`event`, event)
      this._value = event?.target?.value ?? params.value;
    });
  }

  public getGui(): HTMLElement {
    if (this._eInput) return this._eInput;
    throw new Error('Fail');
  }

  public afterGuiAttached(): void {
    console.log(`afterGuiAttached`)
    if (this._eInput) this._eInput.focus();
  }

  public isCancelBeforeStart(): boolean {
    return this.cancelBeforeStart;
  }

  public getValue(): string | null {
    return this._value || null;
  }

  public isCancelAfterEnd(): boolean {
    return false;
  }

  public destroy(): void {
    //
  }

  public isPopup(): boolean {
    return false;
  }
}
