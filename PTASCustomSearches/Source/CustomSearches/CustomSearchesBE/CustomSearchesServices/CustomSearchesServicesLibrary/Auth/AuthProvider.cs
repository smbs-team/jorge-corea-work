namespace CustomSearchesServicesLibrary.Auth
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesEFLibrary.CustomSearches.Model;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.Enumeration;
    using CustomSearchesServicesLibrary.Exception;
    using CustomSearchesServicesLibrary.Gis.Folder;
    using Microsoft.AspNetCore.Http;
    using PTASServicesCommon.DependencyInjection;

    /// <summary>
    /// Validates a incoming request and extracts any <see cref="ClaimsPrincipal"/> contained within the bearer token.
    /// </summary>
    public class AuthProvider : IAuthProvider
    {
        /// <summary>
        /// Gets the authorization header name.
        /// </summary>
        private const string AuthHeaderName = "Authorization";

        /// <summary>
        /// Gets the bearer prefix.
        /// </summary>
        private const string BearerPrefix = "Bearer ";

        /// <summary>
        /// Gets the unique name claim type.
        /// </summary>
        private const string UniqueNameClaimType = "unique_name";

        /// <summary>
        /// Gets the sub claim type.
        /// </summary>
        private const string SubClaimType = "sub";

        /// <summary>
        /// Gets the iss claim type.
        /// </summary>
        private const string IssClaimType = "iss";

        /// <summary>
        /// Gets the b2clogin name.
        /// </summary>
        private const string B2cloginName = "b2clogin";

        /// <summary>
        /// Gets the invalid authorization token error message.
        /// </summary>
        private const string InvalidTokenErrorMessage = "Invalid authorization token.";

        /// <summary>
        /// Gets the missing authorization token error message.
        /// </summary>
        private const string MissingTokenErrorMessage = "Missing authorization token.";

        /// <summary>
        /// Gets the authorization error message.
        /// </summary>
        private const string AuthorizationErrorMessage = "Current user doesn't have permissions to perform the operation '{0}'.";

        /// <summary>
        /// Gets the current user authorization error message.
        /// </summary>
        private const string CurrentUserAuthorizationErrorMessage = "Current user doesn't have permissions to perform the operation '{0}' for user: '{1}'.";

        /// <summary>
        /// Gets the move entity authorization error message.
        /// </summary>
        private const string SystemFolderErrorMessage = "Current user doesn't have permissions to access system folder to perform the operation '{0}'.";

        /// <summary>
        /// Gets the moving from/to system folder authorization error message.
        /// </summary>
        private const string MovingFromToSystemFolderErrorMessage =
            "Moving entities from a non-system folder to a system folder or from a system folder to a non-system folder is not allowed.";

        /// <summary>
        /// Gets the move entity authorization error message.
        /// </summary>
        private const string MoveEntityAuthorizationErrorMessage = "Current user doesn't have permissions to move the entity to the specified folder." +
            " Only entity owners can remove sharing of an entity and users can't take ownership from entities that are owned by a different user.";

        /// <summary>
        /// Gets the project locked error message.
        /// </summary>
        private const string ProjectLockedErrorMessage = "The user project is locked.";

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly IFactory<CustomSearchesDbContext> dbContextFactory;

        /// <summary>
        /// The SQL Server db context factory.
        /// </summary>
        private readonly string[] adminRoles;

        /// <summary>
        /// Gets or sets a value indicating whether it is running in job context.
        /// </summary>
        private bool runningInJobContext;

        /// <summary>
        /// Gets a value indicating whether the user is administrator.
        /// </summary>
        private bool isAdminUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthProvider"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The SQL Server db context factory.</param>
        /// <param name="adminRoles">The administrator roles.</param>
        public AuthProvider(IFactory<CustomSearchesDbContext> dbContextFactory, string adminRoles)
        {
            this.dbContextFactory = dbContextFactory;
            this.adminRoles = adminRoles.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).ToArray();
        }

        /// <summary>
        /// Gets or sets the security principal.
        /// </summary>
        public ClaimsPrincipal ClaimsPrincipal { get; set; }

        /// <summary>
        /// Gets or sets the user info.
        /// </summary>
        public UserInfoData UserInfoData { get; set; }

        /// <summary>
        /// Gets or sets the azure active directory object id.
        /// </summary>
        public Guid? AzureActiveDirectoryObjectId { get; set; }

        /// <summary>
        /// Initializes the authorization provider from the JWT token in the http request.
        /// </summary>
        /// <param name="request">The http request with the JWT token.</param>
        /// <returns>The task.</returns>
        public async Task InitFromHttpRequestAsync(HttpRequest request)
        {
            if (request != null &&
                request.Headers.ContainsKey(AuthHeaderName) &&
                request.Headers[AuthHeaderName].ToString().StartsWith(BearerPrefix))
            {
                var token = request.Headers[AuthHeaderName].ToString().Substring(BearerPrefix.Length);

                var handler = new JwtSecurityTokenHandler();

                JwtSecurityToken securityToken;
                try
                {
                    securityToken = handler.ReadJwtToken(token);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidTokenException(InvalidTokenErrorMessage, ex);
                }

                string iss = AuthHelper.GetClaimValue(securityToken.Claims, IssClaimType);

                string email;
                if (iss.ToLower().Contains(B2cloginName))
                {
                    email = AuthHelper.GetClaimValue(securityToken.Claims, SubClaimType);
                }
                else
                {
                    email = AuthHelper.GetClaimValue(securityToken.Claims, UniqueNameClaimType);
                }

                await this.InitFromEmailAsync(email);

                // Impersonates if it is required.
                if (this.IsAdminUser() && request.Query != null && request.Query.ContainsKey("impersonate"))
                {
                    var impersonateEmail = request.Query["impersonate"].ToString();
                    if (!string.IsNullOrWhiteSpace(impersonateEmail))
                    {
                        await this.InitFromEmailAsync(impersonateEmail);
                    }
                }
            }
            else
            {
                throw new InvalidTokenException(MissingTokenErrorMessage, null);
            }
        }

        /// <summary>
        /// Initializes the authorization provider from an user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="runningInJobContext">Value indicating whether it is running in job context.</param>
        /// <returns>The task.</returns>
        public async Task InitFromUserIdAsync(Guid userId, bool runningInJobContext)
        {
            ClaimsPrincipal claimsPrincipal;
            using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
            {
                claimsPrincipal = await AuthHelper.CreateClaimsPrincipalFromUserIdAsync(userId, dbContext);
            }

            this.ClaimsPrincipal = claimsPrincipal;
            this.UserInfoData = AuthHelper.GetUserInfo(this.ClaimsPrincipal);
            this.AzureActiveDirectoryObjectId = AuthHelper.GetAzureActiveDirectoryObjectId(this.ClaimsPrincipal);
            this.runningInJobContext = runningInJobContext;
            this.isAdminUser = this.IsAdminUser();
        }

        /// <summary>
        /// Initializes the authorization provider from the worker.
        /// </summary>
        public void InitFromWorker()
        {
            this.runningInJobContext = true;
        }

        /// <summary>
        /// Gets a value indicating whether the user is administrator.
        /// The provider should be initialized before call this method.
        /// </summary>
        /// <returns>
        /// True if the user is administrator; otherwise, false.
        /// </returns>
        public bool GetIsAdminUser()
        {
            return this.isAdminUser;
        }

        /// <summary>
        /// Checks if the user has access to any of the specified roles.
        /// </summary>
        /// <param name="roles">The roles separated by ','.</param>
        /// <returns>
        /// True if the user is authorized; otherwise, false.
        /// </returns>
        public bool IsAuthorizedToAnyRole(string roles)
        {
            if (this.runningInJobContext || this.isAdminUser || string.IsNullOrWhiteSpace(roles))
            {
                return true;
            }

            var userRoles = this.UserInfoData.Roles.Select(r => r.Name.ToLower().Trim()).ToHashSet();
            var executionRoles = roles.ToLower().Split(",").Select(r => r.Trim()).ToHashSet();
            if (userRoles.Intersect(executionRoles).Count() > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the user has access to any of the specified roles.
        /// </summary>
        /// <param name="roles">The roles separated by ','.</param>
        /// <param name="errorMessage">The error message.</param>
        public void AuthorizeAnyRole(string roles, string errorMessage)
        {
            if (this.runningInJobContext)
            {
                return;
            }

            if (!this.IsAuthorizedToAnyRole(roles))
            {
                throw new AuthorizationException(errorMessage, innerException: null);
            }
        }

        /// <summary>
        /// Checks if the user has permissions to perform the operation for the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="operationName">The operation name.</param>
        public void AuthorizeCurrentUser(Guid userId, string operationName)
        {
            if (this.runningInJobContext)
            {
                return;
            }

            if (this.UserInfoData.Id != userId)
            {
                throw new AuthorizationException(string.Format(CurrentUserAuthorizationErrorMessage, operationName, userId), innerException: null);
            }
        }

        /// <summary>
        /// Checks if the user has permissions to perform the operation for the user or is administrator.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="operationName">The operation name.</param>
        public void AuthorizeCurrentUserOrAdminRole(Guid userId, string operationName)
        {
            if (this.runningInJobContext || this.isAdminUser)
            {
                return;
            }

            this.AuthorizeCurrentUser(userId, operationName);
        }

        /// <summary>
        /// Checks if the user has permissions to perform the operation for the user in the specified folder.
        /// </summary>
        /// <typeparam name="T">The type of each folder element.</typeparam>
        /// <param name="userId">The user id.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="isLocked">A value indicating whether the entity is locked.</param>
        /// <param name="operationName">The operation name.</param>
        public void AuthorizeFolderItemOperation<T>(Guid userId, IFolder<T> folder, bool isLocked, string operationName)
            where T : class, IFolder<T>, new()
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            if (this.runningInJobContext || this.isAdminUser)
            {
                return;
            }

            if (FolderManagerHelper.IsSystemFolderType(folder.FolderType))
            {
                throw new AuthorizationException(string.Format(SystemFolderErrorMessage, operationName), innerException: null);
            }
            else if ((isLocked == false) && (folder.FolderType.ToLower() == CustomSearchFolderType.Shared.ToString().ToLower()))
            {
                return;
            }

            this.AuthorizeCurrentUser(userId, operationName);
        }

        /// <summary>
        /// Checks if the user has permissions to move the entity between folders.
        /// </summary>
        /// <typeparam name="T">The type of each folder element.</typeparam>
        /// <param name="entityOwnerId">The entity owner id.</param>
        /// <param name="prevFolderType">The previous folder type.</param>
        /// <param name="newFolderType">The new folder type.</param>
        /// <param name="operationName">The operation name.</param>
        public void AuthorizeChangeItemFolderOperation(Guid entityOwnerId, CustomSearchFolderType prevFolderType, CustomSearchFolderType newFolderType, string operationName)
        {
            if (this.runningInJobContext)
            {
                return;
            }

            if (FolderManagerHelper.IsSystemFolderType(prevFolderType) || FolderManagerHelper.IsSystemFolderType(newFolderType))
            {
                if (newFolderType != prevFolderType)
                {
                    throw new AuthorizationException(string.Format(MovingFromToSystemFolderErrorMessage, operationName), innerException: null);
                }
                else if (!this.isAdminUser)
                {
                    throw new AuthorizationException(string.Format(SystemFolderErrorMessage, operationName), innerException: null);
                }
            }

            if ((newFolderType == CustomSearchFolderType.User || newFolderType != prevFolderType) &&
                (entityOwnerId != this.UserInfoData.Id))
            {
                throw new AuthorizationException(string.Format(MoveEntityAuthorizationErrorMessage, operationName), innerException: null);
            }
        }

        /// <summary>
        /// Checks if the user has permissions to perform the operation for the user in the specified project.
        /// </summary>
        /// <param name="userProject">The user project.</param>
        /// <param name="operationName">The operation name.</param>
        public void AuthorizeProjectItemOperation(UserProject userProject, string operationName)
        {
            if (userProject != null)
            {
                int limitYear = DateTime.UtcNow.Year - 1;
                if (userProject.IsLocked || userProject.AssessmentYear < limitYear)
                {
                    throw new AuthorizationException(string.Format(ProjectLockedErrorMessage, operationName), innerException: null);
                }
            }
        }

        /// <summary>
        /// Checks if the user has permissions to perform the operation for the user in the specified project or folder.
        /// </summary>
        /// <typeparam name="T">The type of each folder element.</typeparam>
        /// <param name="userId">The user id.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="isLocked">A value indicating whether the entity is locked.</param>
        /// <param name="userProject">The user project.</param>
        /// <param name="operationName">The operation name.</param>
        public void AuthorizeProjectOrFolderItemOperation<T>(Guid userId, IFolder<T> folder, bool isLocked, UserProject userProject, string operationName)
            where T : class, IFolder<T>, new()
        {
            if (userProject != null)
            {
                this.AuthorizeProjectItemOperation(userProject, operationName);
            }
            else
            {
                this.AuthorizeFolderItemOperation<T>(userId, folder, isLocked, operationName);
            }
        }

        /// <summary>
        /// Checks if the user has access to the specified role.
        /// </summary>
        /// <param name="role">The role name.</param>
        /// <param name="operationName">The operation name.</param>
        public void AuthorizeRole(string role, string operationName)
        {
            if (this.runningInJobContext)
            {
                return;
            }

            if ((this.UserInfoData.Roles == null) ||
                this.UserInfoData.Roles.FirstOrDefault(r => r.Name.ToLower() == role.ToLower()) == null)
            {
                throw new AuthorizationException(string.Format(AuthorizationErrorMessage, operationName), innerException: null);
            }
        }

        /// <summary>
        /// Checks if the user has access to the specified role or is administrator.
        /// </summary>
        /// <param name="role">The role name.</param>
        /// <param name="operationName">The operation name.</param>
        public void AuthorizeRoleOrAdmin(string role, string operationName)
        {
            if (this.runningInJobContext || this.isAdminUser)
            {
                return;
            }

            this.AuthorizeRole(role, operationName);
        }

        /// <summary>
        /// Checks if the user has access to an administrator role.
        /// </summary>
        /// <param name="operationName">The operation name.</param>
        public void AuthorizeAdminRole(string operationName)
        {
            if (this.runningInJobContext || this.isAdminUser)
            {
                return;
            }

            throw new AuthorizationException(string.Format(AuthorizationErrorMessage, operationName), innerException: null);
        }

        /// <summary>
        /// Checks if the user is administrator.
        /// </summary>
        private bool IsAdminUser()
        {
            if (this.UserInfoData.Roles != null)
            {
                foreach (var userRole in this.UserInfoData.Roles)
                {
                    if (this.adminRoles.FirstOrDefault(ar => ar.ToLower() == userRole.Name.ToLower()) != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Initializes the authorization provider from the user email.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <returns>The task.</returns>
        private async Task InitFromEmailAsync(string email)
        {
            ClaimsPrincipal claimsPrincipal;
            using (CustomSearchesDbContext dbContext = this.dbContextFactory.Create())
            {
                claimsPrincipal = await AuthHelper.CreateClaimsPrincipalFromUserEmailAsync(email, dbContext);
            }

            this.ClaimsPrincipal = claimsPrincipal;
            this.UserInfoData = AuthHelper.GetUserInfo(this.ClaimsPrincipal);
            this.AzureActiveDirectoryObjectId = AuthHelper.GetAzureActiveDirectoryObjectId(this.ClaimsPrincipal);
            this.runningInJobContext = false;
            this.isAdminUser = this.IsAdminUser();
        }
    }
}