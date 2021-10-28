namespace CustomSearchesServicesLibrary.Gis.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomSearchesEFLibrary.Gis.Model;
    using CustomSearchesServicesLibrary.Auth;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.CustomSearches.Enumeration;
    using Newtonsoft.Json;

    /// <summary>
    /// Model for the data of the user map.
    /// </summary>
    public class UserMapData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapData"/> class.
        /// </summary>
        public UserMapData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserMapData" /> class.
        /// </summary>
        /// <param name="userMap">The user map.</param>
        /// <param name="initializationType">Type of the initialization.</param>
        /// <param name="userDetails">The user details.</param>
        public UserMapData(UserMap userMap, ModelInitializationType initializationType, Dictionary<Guid, UserInfoData> userDetails)
        {
            this.CreatedBy = userMap.CreatedBy;
            this.CreatedTimestamp = userMap.CreatedTimestamp;
            this.IsLocked = userMap.IsLocked;
            this.LastModifiedBy = userMap.LastModifiedBy;
            this.LastModifiedTimestamp = userMap.LastModifiedTimestamp;
            this.ParentFolderId = userMap.ParentFolderId;
            this.UserMapId = userMap.UserMapId;
            this.UserMapName = userMap.UserMapName;

            UserDetailsHelper.GatherUserDetails(userMap.CreatedByNavigation, userDetails);
            UserDetailsHelper.GatherUserDetails(userMap.LastModifiedByNavigation, userDetails);

            if (userMap.ParentFolder != null)
            {
                UserDetailsHelper.GatherUserDetails(userMap.ParentFolder.User, userDetails);
            }

            if (initializationType == ModelInitializationType.FullObject ||
                initializationType == ModelInitializationType.FullObjectWithDepedendencies)
            {
                var results = userMap.MapRenderer.ToArray();
                this.MapRenderers = new MapRendererData[results.Length];

                for (int i = 0; i < results.Length; i++)
                {
                    MapRenderer mapRenderer = results[i];
                    this.MapRenderers[i] = new MapRendererData(mapRenderer, initializationType, userDetails);
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier of the user map.
        /// </summary>
        public int UserMapId { get; set; }

        /// <summary>
        /// Gets or sets the user map name.
        /// </summary>
        public string UserMapName { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who created this user map.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who last modified this user map.
        /// </summary>
        public Guid LastModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the parent folder id.
        /// </summary>
        public int ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user map is locked.
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or sets the created timestamp.
        /// </summary>
        public DateTime CreatedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the last modified timestamp.
        /// </summary>
        public DateTime LastModifiedTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the folder path.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the type of the folder item.
        /// </summary>
        public string FolderItemType { get; set; }

        /// <summary>
        /// Gets or sets the map renderers.
        /// </summary>
        public MapRendererData[] MapRenderers { get; set; }

        /// <summary>
        /// Converts to an entity framework model.
        /// </summary>
        /// <returns>
        /// The entity framework model.
        /// </returns>
        public UserMap ToEfModel()
        {
            UserMap toReturn = new UserMap
            {
                CreatedBy = this.CreatedBy,
                CreatedTimestamp = this.CreatedTimestamp,
                IsLocked = this.IsLocked,
                LastModifiedBy = this.LastModifiedBy,
                LastModifiedTimestamp = this.LastModifiedTimestamp,
                ParentFolderId = this.ParentFolderId,
                UserMapName = this.UserMapName,
                UserMapId = this.UserMapId
            };

            if (this.MapRenderers != null)
            {
                foreach (var mapRendererData in this.MapRenderers)
                {
                    MapRenderer mapRenderer = mapRendererData.ToEfModel();
                    mapRenderer.UserMapId = toReturn.UserMapId;
                    mapRenderer.UserMap = toReturn;
                    toReturn.MapRenderer.Add(mapRenderer);
                }
            }

            return toReturn;
        }
    }
}
