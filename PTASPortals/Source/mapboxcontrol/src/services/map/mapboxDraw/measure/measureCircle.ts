/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
const w = 74;
const bgColor = '#fff';
const borderColor = 'rgba(0,0,0,1)';

export default (id: string): { id: string; imageData: ImageData } => {
  const canvas = document.createElement('canvas');
  canvas.width = w;
  canvas.height = w;
  const context = canvas.getContext('2d');

  const centerX = w / 2;
  const centerY = w / 2;
  const radius = w / 2;
  if (!context) throw new Error('Error getting context');
  context.beginPath();
  context.arc(centerX, centerY, radius - 5, 0, 2 * Math.PI, false);
  context.fillStyle = bgColor;
  context.fill();
  context.lineWidth = 2;
  context.strokeStyle = borderColor;
  context.stroke();

  // update this image's data with data from the canvas
  const data = context.getImageData(0, 0, w, w);
  return {
    id,
    imageData: data,
  };
};
