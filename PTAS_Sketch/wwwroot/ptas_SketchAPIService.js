// ptas_SketchAPIService.js
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
 class SketchAPIService {
  /**
    * Loads the specified entity's items
    *
    * @param entityName - string
    * @param entityId - string with the ID as shown on the URL
    * @param accessToken - string containing the b2c token
    * @returns GetItems call
    *
    */
  static async getGenericEntity(entityName, entityId, accessToken) {
    if (SketchAPIService.tokenExpired(accessToken))
      return new Promise((resolve, reject) =>
        reject(SKETCH_MESSAGE.TOKEN_EXPIRED)
      );

    const reqData = {
      requests: [{ entityName, entityId }]
    };

    const options = {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken
      }
    };

    return axios
      .post(SKETCH_API.API_URL + "GenericDynamics/GetItems", reqData, options)
      .then(response => {
        let data = response.data;
        if (data.items && data.items.length > 0 && data.items[0].changes) {
          data = data.items[0].changes;
        }
        return data;
      })
      .catch(error => console.error(error));
  }

  /**
    * Loads a list of building IDs from the specified entity
    *
    * @param entityName - string
    * @param entityId - string with the ID as shown on the URL
    * @param accessToken - string containing the b2c token
    * @returns GetItems call
    *
    */
  static async getGenericList(entityName, query, accessToken) {
    if (SketchAPIService.tokenExpired(accessToken))
      return new Promise((resolve, reject) =>
        reject(SKETCH_MESSAGE.TOKEN_EXPIRED)
      );

    const reqData = {
      requests: [{ entityName, query }]
    };

    const options = {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken
      }
    };

    return axios
      .post(SKETCH_API.API_URL + "GenericDynamics/GetItems", reqData, options)
      .then(response => {
        return response.data;
      })
      .catch(error => console.error(error));
  }

  /**
   * Execute an upsert operation for the Generic controller
   * on the data service.
   *
   * @static
   * @param {string} entityName
   * @param {string} entityId
   * @param {object} entity
   * @param {string} accessToken
   * @returns {Promise} a promise that will return the result of the operation.
   * @memberof SketchAPIService
   */
  static async upsertGenericEntity(entityName, entityId, entity, accessToken) {
    if (SketchAPIService.tokenExpired(accessToken)) {
      return new Promise((resolve, reject) =>
        reject(SKETCH_MESSAGE.TOKEN_EXPIRED)
      );
    }

    const reqData = {
      items: [
        {
          changes: {
            ...entity
          },
          entityName: entityName,
          entityId: entityId
        }
      ]
    };
    
    reqData.items.map(item => {
      if (item.changes) {
        if (!isNaN(item.changes.statecode)) delete item.changes.statecode;
        if (!isNaN(item.changes.statuscode)) delete item.changes.statuscode;
      }
    });

    const options = {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken
      }
    };

    return axios
      .post(
        SKETCH_API.API_URL + "GenericDynamics/UpdateItems",
        reqData,
        options
      )
      .then(response => {
        let data = response.data;
        if (data.items && data.items.length > 0 && data.items[0].changes) {
          data = data.items[0].changes;
        }
        return data;
      })
      .catch(error =>
        console.error("GenericDynamics/UpdateItems Error:", error)
      );
  }

  /**
   * Tries to get the Sketch and the related entity information if provided.
   *
   * @static
   * @param {string} sketchEntityId
   * @param {string} accessToken
   * @param {string} [relatedEntityId=null]
   * @param {string} [relatedEntityName=null]
   * @returns {object} with the required Sketch information.
   * @memberof SketchAPIService
   */
  static async getSketch(
    sketchEntityId,
    accessToken,
    relatedEntityId = null,
    relatedEntityName = null
  ) {
    if (SketchAPIService.tokenExpired(accessToken))
      return new Promise((resolve, reject) =>
        reject(SKETCH_MESSAGE.TOKEN_EXPIRED)
      );

    const requests = [{
      entityId: sketchEntityId,
      entityName: "ptas_sketch",
      query: `$top=1&$filter=ptas_sketchid eq ${sketchEntityId}&$select=
        ptas_drawdate,
        ptas_iscomplete,
        ptas_isofficial,
        ptas_name,
        ptas_sketchid,
        ptas_tags,
        ptas_version,
        _ptas_accessoryid_value,
        _ptas_buildingid_value,
        _ptas_parcelid_value,
        _ptas_templateid_value,
        _ptas_unitid_value
    ` }];

    if (relatedEntityId && relatedEntityName) {
      let query = `$top=1&$filter=${relatedEntityName}id eq ${relatedEntityId.toLowerCase()}&$select=
      ptas_name,
      _ptas_parceldetailid_value,
      _ptas_sketchid_value,`
      
      switch (relatedEntityName) {
        case 'ptas_buildingdetail': query +=
          `ptas_buildingdetailid,
          ptas_1stflr_sqft,
          ptas_2ndflr_sqft,
          ptas_attachedgarage_sqft,
          ptas_basementgarage_sqft,
          ptas_buildinggross_sqft,
          ptas_buildingnet_sqft,
          ptas_deck_sqft,
          ptas_enclosedporch_sqft,
          ptas_finbsmt_sqft,
          ptas_halfflr_sqft,
          ptas_openporch_sqft,
          ptas_totalbsmt_sqft,
          ptas_totalliving_sqft,
          ptas_unfinished_full_sqft,
          ptas_unfinished_half_sqft,
          ptas_upperflr_sqft`;
          break;
        case 'ptas_accessorydetail': query +=
          `ptas_accessorydetailid,
          ptas_size`;
          break;
        case 'ptas_condounit': query +=
          `ptas_condounitid,
          ptas_totalliving,
          ptas_roomadditionalsqft,
          ptas_tipoutarea,
          ptas_openporchsqft,
          ptas_endporchsqft,
          ptas_decksqft`;
          break;
      }

      requests.push({
        entityId: relatedEntityId,
        entityName: relatedEntityName,
        query
      });
    }

    const reqData = {
      sketchId: sketchEntityId,
      requests: requests
    };

    const options = {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken
      }
    };
    
    return axios
      .post(SKETCH_API.API_URL + "Sketch/GetItemsAndSketch", reqData, options)
      .then(response => {
        const data = response.data;
        return data;
      })
      .catch(error => console.error(error));
  }

  /**
   * Executes an upsert operation on the data services Sketch controller.
   *
   * @static
   * @param {string} sketchId
   * @param {JSON} sketch
   * @param {Object} sketchEntity
   * @param {string} accessToken
   * @param {string} [relatedEntityName=null]
   * @param {string} [relatedEntityId=null]
   * @param {object} [relatedEntity=null]
   * @returns The result of API operation.
   * @memberof SketchAPIService
   */
  static async upsertSketch(
    sketchId,
    sketch,
    sketchEntity,
    accessToken,
    relatedEntityName = null,
    relatedEntityId = null,
    relatedEntity = null
  ) {
    if (SketchAPIService.tokenExpired(accessToken))
      return new Promise((resolve, reject) =>
        reject(SKETCH_MESSAGE.TOKEN_EXPIRED)
      );
    
    const sketchEntityFiltered = {}
    if (sketchEntity) {
      for (const [key, value] of Object.entries(sketchEntity)) {
        if (parent.sketchControl.sketchEntityLoaded[key] !== value || key == 'ptas_sketchid' || key == '_ptas_buildingid_value' ||
          key == '_ptas_parcelid_value' || key == '_ptas_accessoryid_value' || key == '_ptas_unitid_value')
          sketchEntityFiltered[key] = parent.sketchControl.sketchEntityLoaded[key] = value;
      }
    }

    let items = [
      {
        changes: {
          ...sketchEntityFiltered
        },
        entityName: "ptas_sketch",
        entityId: sketchId
      }
    ];

    if (relatedEntity && relatedEntityName && relatedEntityId) {
      const relatedEntityFiltered = {}
      for (const [key, value] of Object.entries(relatedEntity)) {
        if (parent.sketchControl.relatedEntityLoaded[key] !== value || key.substr(key.length - 2, 2) == 'id')
          relatedEntityFiltered[key] = parent.sketchControl.relatedEntityLoaded[key] = value;
      }
      items.push({
        changes: {
          ...relatedEntityFiltered
        },
        entityName: relatedEntityName,
        entityId: relatedEntityId
      });
    }
    
    const reqData = {
      sketchId: sketchId,
      sketch: sketch,
      items: items
    };

    const options = {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken
      }
    };
    
    return axios
      .post(
        SKETCH_API.API_URL + "Sketch/UpdateItemsAndSketch",
        reqData,
        options
      )
      .then(response => {
        return response.data;
      })
      .catch(error =>
        console.error("Sketch/UpdateItemsAndSketch Error:", error)
      );
  }

  /**
   * Loads a list of recently opened sketches.
   *
   * @static
   * @param {string} accessToken
   * @returns The result of API operation.
   * @memberof SketchAPIService
   */
  static async getRecentSketches(accessToken) {
    if (SketchAPIService.tokenExpired(accessToken))
      return new Promise((resolve, reject) =>
        reject(SKETCH_MESSAGE.TOKEN_EXPIRED)
      );

    const options = {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken
      }
    };
    return axios
      .get(SKETCH_API.API_URL + "Sketch/GetRecent", options)
      .then(response => {
        const data = response.data;
        return data;
      })
      .catch(error => console.error(error));
  }

  /**
   * Searches the given string within sketches to import
   *
   * @static
   * @param {string} string
   * @param {string} accessToken
   * @returns The result of API operation.
   * @memberof SketchAPIService
   */
  static async searchStringInSketch(string, accessToken) {
    if (SketchAPIService.tokenExpired(accessToken))
      return new Promise((resolve, reject) =>
        reject(SKETCH_MESSAGE.TOKEN_EXPIRED)
      );

    const cancelTokenSource = axios.CancelToken.source();
    const options = {
      params: {
        searchFor: string
      },
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken
      },
      cancelToken: cancelTokenSource.token
    };

    return axios
      .get(SKETCH_API.API_URL + "Sketch/Search", options)
      .then(response => {
        if (document.getElementById("searchField").value == string)
          return response.data;
        else
          cancelTokenSource.cancel()
      })
      .catch(error => console.error(error));
  }

  /**
   * Deletes the specified sketch
   *
   * @static
   * @param {string} sketchId
   * @param {string} accessToken
   * @returns The result of API operation.
   * @memberof SketchAPIService
   */
  static async deleteSketch(sketchId, accessToken) {
    if (SketchAPIService.tokenExpired(accessToken))
      return new Promise((resolve, reject) =>
        reject(SKETCH_MESSAGE.TOKEN_EXPIRED)
      );

    const options = {
      params: {
        sketchId: sketchId
      },
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken
      }
    };
    return axios
      .delete(SKETCH_API.API_URL + "Sketch/DeleteSketch", options)
      .then(response => {
        const data = response.data;
        return data;
      })
      .catch(error => console.error(error));
  }

  /**
   * Loads the sketch versions of the current document for the Info section
   *
   * @static
   * @param {string} sketchId
   * @param {string} accessToken
   * @returns The result of API operation.
   * @memberof SketchAPIService
   */
  static async getSketchHistory(sketchId, accessToken) {
    if (SketchAPIService.tokenExpired(accessToken))
      return new Promise((resolve, reject) =>
        reject(SKETCH_MESSAGE.TOKEN_EXPIRED)
      );

    const options = {
      params: {
        sketchId: sketchId
      },
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken
      }
    };
    return axios
      .get(SKETCH_API.API_URL + `Sketch//History?sketchId=${sketchId}`, options)
      .then(response => {
        const data = response.data;
        return data;
      })
      .catch(error => console.error(error));
  }

  /**
   * Loads the current user data
   *
   * @static
   * @param {string} sketchId
   * @param {string} accessToken
   * @returns The result of API operation.
   * @memberof SketchAPIService
   */
  static async getSystemUser(accessToken) {
    if (SketchAPIService.tokenExpired(accessToken))
      return new Promise((resolve, reject) =>
        reject(SKETCH_MESSAGE.TOKEN_EXPIRED)
      );

    const options = {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken
      }
    };
    return axios
      .get(SKETCH_API.API_URL + "SystemUser", options)
      .then(response => {
        const data = response.data;
        return data;
      })
      .catch(error => console.error(error));
  }

  /**
   * Loads the sketch thumbnail
   *
   * @static
   * @param {JSON} sketch
   * @param {string} accessToken
   * @returns The result of API operation.
   * @memberof SketchAPIService
   */
  static async getSketchSVG(sketch, accessToken) {
    if (SketchAPIService.tokenExpired(accessToken))
      return new Promise((resolve, reject) =>
        reject(SKETCH_MESSAGE.TOKEN_EXPIRED)
      );

    const options = {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + accessToken
      }
    };
    return axios
      .post(SKETCH_API.DOCUMENTS_URL + "SVG", sketch, options)
      .then(response => {
        const data = response.data;
        return data;
      })
      .catch(error => console.error(error));
  }

  /**
   * Loads a magic link token
   *
   * @static
   * @param {string} accessToken
   * @param {boolean} refreshToken
   * @returns The result of API operation.
   * @memberof SketchAPIService
   */
  static async getMagicLinkToken(accessToken, refreshToken = true) {
    if (refreshToken) {
      const options = {
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + accessToken
        }
      };
      return axios
        .get(SKETCH_API.MAGIC_LINK_URL + "ADTokenExchange", options)
        .then(response => {
          const data = response.data;
          return data;
        })
        .catch(error => console.error(error));
    } else {
      let b2cToken = localStorage.getItem("b2cToken");
      return new Promise(resolve => resolve(b2cToken));
    }
  }

  /**
   * Verifies whether the specified token has expired
   *
   * @static
   * @param {string} token
   * @returns {boolean}
   * @memberof SketchAPIService
   */
  static tokenExpired(token) {
    try {
      let tokenExpired = false;
      let decoded = jwt_decode(token);
      if (decoded && decoded.exp) {
        //Convert to seconds. Mark token as expired 1 minute before
        const now = (Date.now() - 6000) / 1000;

        if (now >= decoded.exp) {
          tokenExpired = true;
        }
      } else {
        tokenExpired = true;
      }
      return tokenExpired;
    } catch (err) {
      return true;
    }
  }
}