const fs = require('fs');
const svg2img = require('svg2img');

const svgString = `<svg xmlns="http://www.w3.org/2000/svg" height="40" viewBox="0 0 24 24" width="40">
<path d="M0 0h24v24H0z" fill="none" />
<path fill="#A5C727"
    d="M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z" />
</svg>`;
//1. convert from svg string
svg2img(svgString, function (error, buffer) {
  if (error) throw error;
  //returns a Buffer
  fs.writeFileSync('converted-images/location-yellow.png', buffer);
});
