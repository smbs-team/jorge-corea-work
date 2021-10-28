import {
  generateBlobUriObjectFromAFileArray,
  getBaseFileMetadataObject,
  generateFileListFromFileArray,
  getFileArrayFromFileNameCSV,
} from '../../../lib/data-mappings/fileMetadataToFileArray';
import {
  updateFileMetadata,
  createFileMetadata,
  deleteFileMetadata,
} from '../../../services/dataServiceProvider';
import { arrayNullOrEmpty } from '../../../lib/helpers/util';
import deepEqual from 'deep-equal';
import { cloneDeep } from 'lodash';

/**
 *Sets the FileMetadata Object structure and calls the API to create a new file metadata entity.
 *
 * @param {*} section
 * @param {*} document
 * @param {*} seniorAppId
 * @param {*} seniorDetailId
 * @param {*} accountNumber
 * @param {*} array
 * @param {*} arrayName
 */
const createFileMeta = async (
  section,
  document,
  seniorAppId,
  seniorDetailId,
  accountNumber,
  array,
  arrayName,
  useDataServiceProvider
) => {
  let fileMetadata = getBaseFileMetadataObject();
  fileMetadata.seniorExemptionApplicationId = seniorAppId;
  fileMetadata.seniorExemptionApplicationDetailId = seniorDetailId;
  fileMetadata.portalSection = section;
  fileMetadata.portalDocument = document;
  fileMetadata.accountNumber = accountNumber;
  fileMetadata.blobUrl = generateBlobUriObjectFromAFileArray(
    array,
    arrayName,
    null
  );
  fileMetadata.originalFilename = JSON.stringify(
    generateFileListFromFileArray(array)
  );

  if (fileMetadata.originalFilename.length > 2) {
    await createFileMetadata(fileMetadata, useDataServiceProvider);
  }
};

/**
 * Updates an existing FileMetadata object and calls the API to update the related entity.
 *
 * @param {*} currentFileMetadata
 * @param {*} accountNumber
 * @param {*} array
 * @param {*} arrayName
 * @param {*} cloneFileMetadata
 * @param {*} fileMetadata
 */
const updateFileMeta = async (
  currentFileMetadata,
  accountNumber,
  array,
  arrayName,
  useDataServiceProvider
) => {
  if (currentFileMetadata) {
    let updatedFileMetadata = cloneDeep(currentFileMetadata);
    updatedFileMetadata.blobUrl = generateBlobUriObjectFromAFileArray(
      array,
      arrayName,
      updatedFileMetadata.blobUrl
    );
    updatedFileMetadata.originalFilename = JSON.stringify(
      generateFileListFromFileArray(array)
    );
    updatedFileMetadata.accountNumber = accountNumber;

    // Only update if the collection changed
    if (!deepEqual(currentFileMetadata, updatedFileMetadata)) {
      await updateFileMetadata(updatedFileMetadata, useDataServiceProvider);
    }
  }
};

export /**
 * Creates or updates a file metadata entity based on the received parameters.
 *
 * @param {*} fileMetadata
 * @param {*} section
 * @param {*} document
 * @param {*} seniorAppId
 * @param {*} seniorDetailId
 * @param {*} accountNumber
 * @param {*} array
 * @param {*} arrayName
 * @param {*} getFilesMetadata
 */
const createOrUpdateFileMetadataEntities = async (
  fileMetadata,
  section,
  document,
  seniorAppId,
  seniorDetailId,
  accountNumber,
  array,
  arrayName,
  getFilesMetadata,
  useDataServiceProvider = true
) => {
  let cloneFileMetadata = cloneDeep(fileMetadata);
  let filteredMetadata = [];
  let toBeCleared = [];

  cloneFileMetadata = cloneFileMetadata.filter(item => {
    return item.originalFilename && item.originalFilename.length > 2;
  });

  toBeCleared = fileMetadata.filter(item => {
    return item.originalFilename && item.originalFilename.length <= 2;
  });

  if (seniorDetailId) {
    filteredMetadata = cloneFileMetadata.filter(
      f =>
        f.seniorExemptionApplicationId === seniorAppId &&
        f.seniorExemptionApplicationDetailId === seniorDetailId &&
        f.portalSection === section &&
        f.portalDocument === document &&
        f.isBlob
    );
  } else {
    filteredMetadata = cloneFileMetadata.filter(
      f =>
        f.seniorExemptionApplicationId === seniorAppId &&
        f.portalSection === section &&
        f.portalDocument === document &&
        f.isBlob
    );
  }

  if (arrayNullOrEmpty(filteredMetadata)) {
    await createFileMeta(
      section,
      document,
      seniorAppId,
      seniorDetailId,
      accountNumber,
      array,
      arrayName,
      useDataServiceProvider
    );
  } else {
    let currentFileMetadata = filteredMetadata[0];
    await updateFileMeta(
      currentFileMetadata,
      accountNumber,
      array,
      arrayName,
      useDataServiceProvider
    );
  }

  if (!arrayNullOrEmpty(toBeCleared)) {
    let promises = toBeCleared.map(async item => {
      await deleteFileMetadata(item.id);
    });
    await Promise.all(promises);
  }

  await getFilesMetadata(seniorAppId, useDataServiceProvider);
};

export /**
 *Return an object containing all the respective file arrays with the information already mapped according to the 
 file metadata
 *
 * @param {object []} filesMetadata
 * @param {string} seniorAppId
 * @param {string} seniorDetailId
 * @param {string} section
 * @param {object []} documents
 * @returns {object} that can be set directly on the state to set the created arrays
 */
const createFileArraysFromMetadata = async (
  placeholder,
  filesMetadata,
  seniorAppId,
  seniorDetailId,
  section,
  documents
) => {
  let fileArrays = null;
  let parentFileArrays = null;
  let toBeCleared = [];
  if (documents) {
    let promises = documents.map(async doc => {
      let filteredFilesMetadata = [];

      toBeCleared = filesMetadata.filter(item => {
        return item.originalFilename.length <= 2;
      });

      if (seniorDetailId) {
        filteredFilesMetadata = filesMetadata.filter(
          f =>
            f.seniorExemptionApplicationId === seniorAppId &&
            f.seniorExemptionApplicationDetailId === seniorDetailId &&
            f.portalSection === section &&
            f.portalDocument === doc.document &&
            f.isBlob
        );
      } else {
        filteredFilesMetadata = filesMetadata.filter(
          f =>
            f.seniorExemptionApplicationId === seniorAppId &&
            f.portalSection === section &&
            f.portalDocument === doc.document &&
            f.isBlob
        );
      }

      if (!arrayNullOrEmpty(filteredFilesMetadata)) {
        const array = await getFileArrayFromFileNameCSV(
          placeholder,
          filteredFilesMetadata[0].originalFilename,
          seniorAppId,
          seniorDetailId,
          section,
          doc.document
        );

        if (doc.parentObjectProperty) {
          if (!parentFileArrays) {
            parentFileArrays = {};
          }
          if (array[0] && array[0].scheduleId) {
            parentFileArrays[doc.parentObjectProperty][doc.arrayName] = array;
          } else {
            parentFileArrays[doc.parentObjectProperty] = {
              [doc.arrayName]: array,
              index: doc.parentObjectIndex,
            };
          }
        }
        if (!fileArrays) {
          fileArrays = {};
        }
        // fileArrays[doc.arrayName] = array;

        fileArrays[doc.arrayName] = array;
        // fileArrays[doc.arrayName] = fileArrays[doc.arrayName].filter(
        //   a => a.url != ''
        // );
      }
    });

    await Promise.all(promises);

    // if (!arrayNullOrEmpty(toBeCleared)) {
    //   let promises = toBeCleared.map(async item => {
    //     await deleteFileMetadata(item.id);
    //   });
    //   await Promise.all(promises);
    // }
  }
  /*console.log('MyInfo uplaoderHelper createFileArraysFromMetadata FINAL', {
    rootFileArrays: fileArrays,
    parentFileArrays: parentFileArrays,
  });*/
  return { rootFileArrays: fileArrays, parentFileArrays: parentFileArrays };
};
export /**
 *
 *
 * @param {*} filesMetadata
 * @param {*} seniorAppId
 * @param {*} detailId
 * @param {*} section
 * @param {*} documents
 * @param {*} setParentState
 * @param {*} setState
 */
const setFileArrays = async (
  filesMetadata,
  seniorAppId,
  detailId,
  section,
  documents,
  setParentState,
  setState,
  state
) => {
  const arrays = await createFileArraysFromMetadata(
    filesMetadata,
    seniorAppId,
    detailId,
    section,
    documents
  );

  if (arrays.parentFileArrays) {
    Object.keys(arrays.parentFileArrays).forEach(key => {
      let currentParent = cloneDeep(state[key]);
      Object.keys(currentParent).forEach(array => {
        if (array !== 'index') {
          currentParent[array] = arrays.parentFileArrays[key][array];
        }
      });

      setParentState([key], currentParent);
    });
  }
  if (arrays.rootFileArrays) {
    console.log('uplooadhelper', arrays);
    setState(arrays.rootFileArrays);
  }
};
