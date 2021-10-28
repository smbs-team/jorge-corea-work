namespace CustomSearchesServicesLibrary.Auth
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Validates access tokes that have been submitted as part of a request.
    /// </summary>
    public interface IAuthProvider
    {
        /// <summary>
        /// Gets or sets the security principal.
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the user info.
        /// </summary>
        UserInfoData UserInfoData { get; set; }

        /// <summary>
        /// Gets or sets the azure active directory object id.
        /// </summary>
        public Guid? AzureActiveDirectoryObjectId { get; set; }

        /// <summary>
        /// Initializes the authorization provider from the JWT token in the http request.
        /// </summary>
        /// <param name="request">The http request with the JWT token.</param>
        /// <returns>The task.</returns>
        Task InitFromHttpRequestAsync(HttpRequest request);

        /// <summary>
        /// Initializes the authorization provider from an user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="runningInJobContext">Value indicating whether it is running in job context.</param>
        /// <returns>The task.</returns>
        Task InitFromUserIdAsync(Guid userId, bool runningInJobContext);

        /// <summary>
        /// Initializes the authorization provider from the worker.
        /// </summary>
        void InitFromWorker();

        /// <summary>
        /// Gets a value indicating whether the user is administrator.
        /// The provider should be initialized before call this method.
        /// </summary>
        /// <returns>
        /// True if the user is administrator; otherwise, false.
        /// </returns>
        bool GetIsAdminUser();

        /// <summary>
        /// Checks if the user has access to any of the specified roles.
        /// </summary>
        /// <param name="roles">The roles separated by ',',.</param>
        /// <returns>
        /// True if the user is authorized; otherwise, false.
        /// </returns>
        bool IsAuthorizedToAnyRole(string roles);

        /// <summary>
        /// Checks if the user has access to any of the specified roles.
        /// </summary>
        /// <param name="roles">The roles separated by ','.</param>
        /// <param name="errorMessage">The error message.</param>
        void AuthorizeAnyRole(string roles, string errorMessage);

        /// <summary>
        /// Checks if the user has permissions to perform the operation for the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="operationName">The operation name.</param>
        void AuthorizeCurrentUser(Guid userId, string operationName);

        /// <summary>
        /// Checks if the user has permissions to perform the operation for the user or is administrator.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="operationName">The operation name.</param>
        void AuthorizeCurrentUserOrAdminRole(Guid userId, string operationName);

        /// <summary>
        /// Checks if the user has permissions to perform the operation for the user in the specified folder.
        /// </summary>
        /// <typeparam name="T">The type of each folder element.</typeparam>
        /// <param name="userId">The user id.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="isLocked">A value indicating whether the entity is locked.</param>
        /// <param name="operationName">The operation name.</param>
        void AuthorizeFolderItemOperation<T>(Guid userId, IFolder<T> folder, bool isLocked, string operationName)
            where T : class, IFolder<T>, new();

        /// <summary>
        /// Checks if the user has permissions to move the entity between folders.
        /// </summary>
        /// <param name="entityOwnerId">The entity owner id.</param>
        /// <param name="prevFolderType">The previous folder type.</param>
        /// <param name="newFolderType">The new folder type.</param>
        /// <param name="operationName">The operation name.</param>
        void AuthorizeChangeItemFolderOperation(Guid entityOwnerId, CustomSearchFolderType prevFolderType, CustomSearchFolderType newFolderType, string operationName);

        /// <summary>
        /// Checks if the user has permissions to perform the operation for the user in the specified project.
        /// </summary>
        /// <param name="userProject">The user project.</param>
        /// <param name="operationName">The operation name.</param>
        void AuthorizeProjectItemOperation(UserProject userProject, string operationName);

        /// <summary>
        /// Checks if the user has permissions to perform the operation for the user in the specified project or folder.
        /// </summary>
        /// <typeparam name="T">The type of each folder element.</typeparam>
        /// <param name="userId">The user id.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="isLocked">A value indicating whether the entity is locked.</param>
        /// <param name="userProject">The user project.</param>
        /// <param name="operationName">The operation name.</param>
        void AuthorizeProjectOrFolderItemOperation<T>(Guid userId, IFolder<T> folder, bool isLocked, UserProject userProject, string operationName)
            where T : class, IFolder<T>, new();

        /// <summary>
        /// Checks if the user has access to the specified role.
        /// </summary>
        /// <param name="role">The role name.</param>
        /// <param name="operationName">The operation name.</param>
        void AuthorizeRole(string role, string operationName);

        /// <summary>
        /// Checks if the user has access to the specified role or is administrator.
        /// </summary>
        /// <param name="role">The role name.</param>
        /// <param name="operationName">The operation name.</param>
        void AuthorizeRoleOrAdmin(string role, string operationName);

        /// <summary>
        /// Checks if the user has access to an administrator role.
        /// </summary>
        /// <param name="operationName">The operation name.</param>
        void AuthorizeAdminRole(string operationName);
    }
}