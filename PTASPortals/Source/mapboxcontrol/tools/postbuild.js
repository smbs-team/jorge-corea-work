const syncDirectory = require('sync-directory');
const path = require('path');
const fs = require('fs');
const rimraf = require('rimraf');

fs.renameSync(
  path.join(__dirname, '..', 'build'),
  path.join(__dirname, '..', 'build-old')
);

/**
 * Copy to node modules
 */
syncDirectory(
  path.join(__dirname, '..', 'build-old'),
  path.join(__dirname, '../build/mapboxcontrol'),
  {
    exclude: [/node\_modules/, 'example'],
    afterSync({ type, relativePath }) {
      console.log(`Sync type= ${type}, relative path= ${relativePath}`);
    },
  }
);

rimraf.sync(path.join(__dirname, '..', 'build-old'));
