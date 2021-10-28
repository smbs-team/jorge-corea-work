//-----------------------------------------------------------------------
// <copyright file="signalRHelper.js" company="King County">
//     Copyright (c) King County. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import {
  arrayNullOrEmpty,
  replaceDotBySymbol,
} from '../../../lib/helpers/util';
import deepEqual from 'deep-equal';
import { cloneDeep } from 'lodash';
import { postValue, getValue } from '../../../services/jsonStoreService';
import { setPlaceHolderOnFileArray } from '../../../lib/data-mappings/fileMetadataToFileArray';
import uuid from 'uuid';
export /**
 * Uses the parameters to create a new Uploads object and push it to the JSON Store.
 *
 * @param {object} contact
 * @param {string} seniorAppId
 * @param {string} detailId
 * @param {string} section
 * @param {object []} documents
 */
const updateUploadsData = async (
  contact,
  seniorAppId,
  detailId,
  section,
  documents,
  currentUploadData,
  sendMessage,
  setUploads,
  page
) => {
  if (contact) {
    let uploads = {
      id: uuid.v4(),
      fullName: `${contact.firstname ? contact.firstname : ''} ${
        contact.middlename ? contact.middlename : ''
      } ${contact.lastname ? contact.lastname : ''}`,
      email: contact.emailaddress,
      contactId: contact.contactid,
      seniorAppId: seniorAppId,
      seniorApplicationDetailsId: detailId ? detailId : null,
      page: page ? page : section,
      refreshDate: new Date().toUTCString(),
      status: 'active',
      uploads: [],
    };

    if (!arrayNullOrEmpty(documents)) {
      // documents is an array with objects with the format: {arrayName, document, isVisible,  files[]}
      documents.map(d => {
        if (d.isVisible) {
          let upload = {
            name: d.document,
            section: section,
            document: d.document,
            isVisible: d.isVisible,
            files: [],
          };

          if (!arrayNullOrEmpty(d.files)) {
            d.files.map(f => {
              // Only upload valid state images
              if (
                !f.isDirty &&
                !f.isLoading &&
                !f.isUploading &&
                f.isUploaded &&
                f.isValid
              ) {
                let newFile = cloneDeep(f);
                // Add the image id
                newFile.imageId = `${seniorAppId}${
                  detailId ? '.' + detailId : ''
                }.${section}.${d.document}.${replaceDotBySymbol(
                  f.imageName,
                  '_'
                )}`;

                // Clean the url property to avoid sending the base64 encoded image.
                newFile.url = '';

                upload.files.push(newFile);
              }
            });
          }

          uploads.uploads.push(upload);
        }
      });
    }

    // Only post the upload and send the notification if the uploads data changed.
    if (
      !currentUploadData ||
      !deepEqual(currentUploadData.uploads, uploads.uploads)
    ) {
      await postValue(
        `${contact.contactid}/${seniorAppId}/uploads`,
        uploads,
        null
      );
      setUploads(uploads);
      await sendMessage(`Notification sent, Upload Id: ${uploads.id}`);
    } else {
      uploads = currentUploadData;
    }

    return uploads;
  }
};

export /**
 * Uses the latest Uploads data from the Json Store to update the current files arrays and file metadata.
 *
 * @param {*} contactId
 * @param {*} seniorAppId
 * @param {*} section
 * @param {*} documents
 * @param {*} handleMetadataUpdate
 * @param {*} setUpload
 * @param {*} setFileArrayPlaceHolders
 */
const setUploadsData = async (
  contactId,
  seniorAppId,
  section,
  documents,
  handleMetadataUpdate,
  setUpload,
  setFileArrayPlaceHolders
) => {
  const latestUpload = await getValue(
    `${contactId}/${seniorAppId}/uploads/uploads`
  );
  setUpload(latestUpload);

  // if (latestUpload.section === section) {
  // First on all documents add the placeholders
  latestUpload.uploads.map(u => {
    setFileArrayPlaceHolders(u.document, setPlaceHolderOnFileArray(u.files));
  });

  // Then update the metadata
  let promises = latestUpload.uploads.map(async u => {
    await handleMetadataUpdate(
      u.files,
      documents.filter(d => d.document === u.document)[0],
      false
    );
  });

  await Promise.all(promises);
  // }
};
