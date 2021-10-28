// index.ts
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import { MessageDescriptor } from '@formatjs/intl';

interface FormatMsgComp {
  props: MessageDescriptor;
}

export function getStringFromFormatMsgComp(comp: FormatMsgComp): string {
  return (comp.props.defaultMessage as string) ?? '';
}
