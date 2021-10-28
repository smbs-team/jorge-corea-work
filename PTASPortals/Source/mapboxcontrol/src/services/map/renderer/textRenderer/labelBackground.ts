/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
const w = 160;

export default ({
  id,
  background,
}: {
  id: string;
  background: string;
}): { id: string; imageData: ImageData } => {
  const canvas = document.createElement('canvas');
  canvas.width = w;
  canvas.height = w;
  const context = canvas.getContext('2d');

  if (!context) throw new Error('Error getting context');
  context.beginPath();
  context.fillStyle = background;
  context.fillRect(0, 0, canvas.width, canvas.height);
  const data = context.getImageData(0, 0, w, w);
  return {
    id,
    imageData: data,
  };
};
