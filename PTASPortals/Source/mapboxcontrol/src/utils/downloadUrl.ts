/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

export function downloadUrl(url: string): void {
  const downloadLink = document.createElement('a');
  downloadLink.href = url;
  downloadLink.click();
  downloadLink.remove();
}
