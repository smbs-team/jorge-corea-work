// ptas_SketchEntitiesHandler.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
class SketchEntitiesHandler {
  /**
   * Constant to track the related entity important properties across different entities.
   *
   * @static
   * @memberof SketchEntitiesHandler
   */
  static relatedEntityKey = {
    ptas_buildingdetail: {
      id: "ptas_buildingdetailid",
      sketchId: "_ptas_buildingid_value",
      parcelId: "_ptas_parceldetailid_value"
    },
    ptas_accessorydetail: {
      id: "ptas_accessorydetailid",
      sketchId: "_ptas_accessoryid_value",
      parcelId: "_ptas_parceldetailid_value"
    },
    ptas_condounit: {
      id: "ptas_condounitid",
      sketchId: "_ptas_unitid_value",
      parcelId: "_ptas_parcelid_value"
    }
  };

  /**
   * Creates a new Sketch entity filling all the important fields.
   *
   * @static
   * @param {object} relatedEntity
   * @param {string} relatedEntityName
   * @param {object} [sketchTemplate=null]
   * @returns {object} a new sketch.
   * @memberof SketchEntitiesHandler
   */
  static createSketchObject(
    relatedEntity,
    relatedEntityName,
    userData,
    sketchTemplate = null
  ) {
    let newSketch = null;
    if (relatedEntity && relatedEntityName) {
      const relatedEntityKeys =
        SketchEntitiesHandler.relatedEntityKey[relatedEntityName];
      if (sketchTemplate) {
        newSketch = { ...sketchTemplate };
        newSketch._ptas_templateid_value = sketchTemplate.ptas_sketchid;
      } else {
        newSketch = {};
      }

      newSketch.ptas_sketchid = SketchControl.uuidv4();
      newSketch.ptas_isofficial = false;
      newSketch.ptas_iscomplete = false;
      newSketch.ptas_drawdate = new Date().toUTCString();

      // TODO: the version need to be oficial sketch'es version + 1.
      newSketch.ptas_version =
        "" +
        (Number(newSketch.ptas_version)
          ? Number(newSketch.ptas_version) + 1
          : 1);
      newSketch._ptas_parcelid_value =
        relatedEntity[relatedEntityKeys.parcelId];
      newSketch[relatedEntityKeys.sketchId] =
        relatedEntity[relatedEntityKeys.id];
    }

    return newSketch;
  }

  /**
   * Tries to create the first Sketch for the related entity. Updating the related entity as consequence.
   *
   * @static
   * @param {Object} sketchControl
   * @returns {Promise} returns a promise that will return the result of the create operation.
   * @memberof SketchEntitiesHandler
   */
  static async createFirstSketch(sketchControl) {
    let sketchEntity = SketchEntitiesHandler.createSketchObject(
      sketchControl.relatedEntity,
      sketchControl.relatedEntityName,
      sketchControl.userData
    );

    sketchControl.sketchEntityId = sketchEntity.ptas_sketchid;
    sketchControl.sketchIsOfficial = false;
    sketchControl.sketchEntity = sketchEntity;
    if (!sketchControl.sketchEntityLoaded)
      sketchControl.sketchEntityLoaded = { ...sketchEntity }
    const service = SketchAPIService.upsertSketch(
      sketchEntity.ptas_sketchid,
      JSON.parse(SketchToJSON.write(sketchControl)),
      sketchEntity,
      sketchControl.sketchAccessToken,
      sketchControl.relatedEntityName,
      sketchControl.relatedEntityId,
      sketchControl.relatedEntity
    );
    return service;
  }

  /**
   * Creates a draft sketch based on another sketch.
   *
   * @static
   * @param {Object} sketchControl
   * @returns {Promise} returns a Promise that will return the result of the create operation.
   * @memberof SketchEntitiesHandler
   */
  static async createDraftSketch(sketchControl) {
    let newSketchEntity = SketchEntitiesHandler.createSketchObject(
      sketchControl.relatedEntity,
      sketchControl.relatedEntityName,
      sketchControl.userData,
      sketchControl.sketchEntity
    );
    sketchControl.sketchEntityId = newSketchEntity.ptas_sketchid;
    sketchControl.sketchIsOfficial = false;
    sketchControl.sketchEntity = newSketchEntity;

    return SketchAPIService.upsertSketch(
      newSketchEntity.ptas_sketchid,
      JSON.parse(SketchToJSON.write(sketchControl)),
      newSketchEntity,
      sketchControl.sketchAccessToken
    );
  }

  /**
   * Update the Sketch file and entity on the backend.
   *
   * @static
   * @param {object} sketchControl
   * @returns {Promise} a Promise the will return the result of the operation.
   * @memberof SketchEntitiesHandler
   */
  static async autoSaveSketch(sketchControl, sketch) {
    sketchControl.sketchEntity.ptas_drawdate = new Date().toUTCString();
    return SketchAPIService.upsertSketch(
      sketchControl.sketchEntityId,
      JSON.parse(sketch),
      sketchControl.sketchEntity,
      sketchControl.sketchAccessToken
    );
  }

  /**
   * Executes an update of the Sketch entity only.
   *
   * @static
   * @param {object} sketchControl
   * @returns Promise that will provide the result of the operation.
   * @memberof SketchEntitiesHandler
   */
  static async updateSketchEntityOnly(
    sketchEntityId,
    sketchEntity,
    accessToken
  ) {
    return SketchAPIService.upsertGenericEntity(
      "ptas_sketch",
      sketchEntityId,
      sketchEntity,
      accessToken
    );
  }

  /**
   * Update an existing draft to become the official version for the related entity.
   *
   * @static
   * @param {SketchControl} sketchControl
   * @param {object} draftEntity
   * @param {string} draftSketch
   * @returns Promise that will provide the result of the operation.
   * @memberof SketchEntitiesHandler
   */
  static async promoteDraftToOfficial(sketchControl, draftEntity, draftSketch) {
    draftEntity.ptas_isofficial = true;
    draftEntity.ptas_iscomplete = true;
    sketchControl.sketchIsOfficial = true;
    sketchControl.sketchEntity = draftEntity;

    return SketchAPIService.upsertSketch(
      sketchControl.sketchEntity.ptas_sketchid,
      JSON.parse(draftSketch),
      sketchControl.sketchEntity,
      sketchControl.sketchAccessToken,
      sketchControl.relatedEntityName,
      sketchControl.relatedEntityId,
      sketchControl.relatedEntity
    );
  }

  /**
   * Update an existing draft version to become the official version for the related entity.
   *
   * @static
   * @param {object} sketchControl
   * @returns Promise that will provide the result of the operation.
   * @memberof SketchEntitiesHandler
   */
  static async promoteDraftVersionToOfficial(
    sketchControl,
    draftEntity,
    draftSketch
  ) {
    draftEntity.ptas_isofficial = true;
    draftEntity.ptas_iscomplete = true;
    sketchControl.relatedEntity._ptas_sketchid_value =
      draftEntity.ptas_sketchid;

    return SketchAPIService.upsertSketch(
      draftEntity.ptas_sketchid,
      JSON.parse(draftSketch),
      draftEntity,
      sketchControl.sketchAccessToken,
      sketchControl.relatedEntityName,
      sketchControl.relatedEntityId,
      sketchControl.relatedEntity
    );
  }

  /**
   * Promotes a previously complete version sketch as the official sketch of the related entity
   * by creating a new Sketch that is based on that previous version.
   *
   * @static
   * @param {object} sketchControl
   * @param {object} completedEntity
   * @param {string} completedSketch
   * @returns Promise that will provide the result of the operation.
   * @memberof SketchEntitiesHandler
   */
  static async promoteCompletedVersionToOfficial(
    sketchControl,
    completedEntity,
    completedSketch
  ) {
    let newSketchEntity = SketchEntitiesHandler.createSketchObject(
      sketchControl.relatedEntity,
      sketchControl.relatedEntityName,
      sketchControl.userData,
      completedEntity
    );
    newSketchEntity.ptas_isofficial = true;
    newSketchEntity.ptas_iscomplete = true;
    sketchControl.relatedEntity._ptas_sketchid_value =
      newSketchEntity.ptas_sketchid;

    return SketchAPIService.upsertSketch(
      newSketchEntity.ptas_sketchid,
      JSON.parse(completedSketch),
      newSketchEntity,
      sketchControl.sketchAccessToken,
      sketchControl.relatedEntityName,
      sketchControl.relatedEntityId,
      sketchControl.relatedEntity
    );
  }
}
