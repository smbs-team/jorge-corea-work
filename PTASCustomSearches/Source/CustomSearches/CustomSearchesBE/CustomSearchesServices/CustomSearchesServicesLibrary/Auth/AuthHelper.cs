namespace CustomSearchesServicesLibrary.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using CustomSearchesEFLibrary.CustomSearches;
    using CustomSearchesServicesLibrary.Auth.Model;
    using CustomSearchesServicesLibrary.Exception;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The authorization helper.
    /// </summary>
    public class AuthHelper
    {
        /// <summary>
        /// Gets the id claim type.
        /// </summary>
        private const string IdClaimType = "id";

        /// <summary>
        /// Gets the full name claim type.
        /// </summary>
        private const string FullNameClaimType = "fullname";

        /// <summary>
        /// Gets the email claim type.
        /// </summary>
        private const string EmailClaimType = "email";

        /// <summary>
        /// Gets the azure active directory object id claim type.
        /// </summary>
        private const string AzureActiveDirectoryObjectIdClaimType = "azureactivedirectoryobjectid";

        /// <summary>
        /// Gets the role id claim type.
        /// </summary>
        private const string RoleIdClaimType = "roleid";

        /// <summary>
        /// Gets the role name claim type.
        /// </summary>
        private const string RoleNameClaimType = "rolename";

        /// <summary>
        /// Gets the role claim type.
        /// </summary>
        private const string RoleClaimType = "role";

        /// <summary>
        /// Gets the team claim type.
        /// </summary>
        private const string TeamClaimType = "team";

        /// <summary>
        /// Gets the team id claim type.
        /// </summary>
        private const string TeamIdClaimType = "teamid";

        /// <summary>
        /// Gets the team name claim type.
        /// </summary>
        private const string TeamNameClaimType = "teamname";

        /// <summary>
        /// Gets the not found error message.
        /// </summary>
        private const string NotFoundErrorMessage = "User '{0}' does not exist or does not belong to a supported Role or Team.";

        /// <summary>
        /// Gets user info from the claims principal.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>
        /// The user info data.
        /// </returns>
        public static UserInfoData GetUserInfo(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException(nameof(claimsPrincipal));
            }

            UserInfoData userInfoData = new UserInfoData();
            if (claimsPrincipal != null)
            {
                userInfoData.Id = Guid.Parse(GetClaimValue(claimsPrincipal, IdClaimType));
                userInfoData.FullName = GetClaimValue(claimsPrincipal, FullNameClaimType);
                userInfoData.Email = GetClaimValue(claimsPrincipal, EmailClaimType);
                string[] roleIds = GetClaimValues(claimsPrincipal, RoleIdClaimType);
                string[] roleNames = GetClaimValues(claimsPrincipal, RoleNameClaimType);
                string[] teamIds = GetClaimValues(claimsPrincipal, TeamIdClaimType);
                string[] teamNames = GetClaimValues(claimsPrincipal, TeamNameClaimType);

                int roleLength = roleIds.Length;
                if (roleLength > 0)
                {
                    RoleData[] roles = new RoleData[roleLength];
                    for (int i = 0; i < roleLength; i++)
                    {
                        roles[i] = new RoleData { Id = roleIds[i], Name = roleNames[i] };
                    }

                    userInfoData.Roles = roles;
                }

                int teamLength = teamIds.Length;
                if (teamLength > 0)
                {
                    TeamData[] teams = new TeamData[teamLength];
                    for (int i = 0; i < teamLength; i++)
                    {
                        teams[i] = new TeamData { Id = teamIds[i], Name = teamNames[i] };
                    }

                    userInfoData.Teams = teams;
                }
            }

            return userInfoData;
        }

        /// <summary>
        /// Gets the azure active directory object id from the claims principal.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>
        /// The azure active directory object id.
        /// </returns>
        public static Guid? GetAzureActiveDirectoryObjectId(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
            {
                throw new ArgumentNullException(nameof(claimsPrincipal));
            }

            string azureActiveDirectoryObjectId = GetClaimValue(claimsPrincipal, AzureActiveDirectoryObjectIdClaimType);
            if (azureActiveDirectoryObjectId == null)
            {
                return null;
            }

            return Guid.Parse(azureActiveDirectoryObjectId);
        }

        /// <summary>
        /// Gets the claim value of the specified type.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <param name="claimType">The claim type.</param>
        /// <returns>
        /// The claim value.
        /// </returns>
        public static string GetClaimValue(ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return GetClaimValues(claimsPrincipal.Claims, claimType).FirstOrDefault();
        }

        /// <summary>
        /// Gets the claims values of the specified type.
        /// </summary>
        /// <param name="claimsPrincipal">The claims.</param>
        /// <param name="claimType">The claim type.</param>
        /// <returns>
        /// The claims values.
        /// </returns>
        public static string[] GetClaimValues(ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return GetClaimValues(claimsPrincipal.Claims, claimType);
        }

        /// <summary>
        /// Gets the claim value of the specified type.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="claimType">The claim type.</param>
        /// <returns>
        /// The claim value.
        /// </returns>
        public static string GetClaimValue(IEnumerable<Claim> claims, string claimType)
        {
            return GetClaimValues(claims, claimType).FirstOrDefault();
        }

        /// <summary>
        /// Gets the claims values of the specified type.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="claimType">The claim type.</param>
        /// <returns>
        /// The claims values.
        /// </returns>
        public static string[] GetClaimValues(IEnumerable<Claim> claims, string claimType)
        {
            return (from claim in claims
                    where claim.Type == claimType
                    select claim.Value).ToArray();
        }

        /// <summary>
        /// Creates claims principal from user email.
        /// </summary>
        /// <param name="userEmail">The user email.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The claims principal.
        /// </returns>
        public static async Task<ClaimsPrincipal> CreateClaimsPrincipalFromUserEmailAsync(string userEmail, CustomSearchesDbContext dbContext)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                throw new ArgumentNullException(nameof(userEmail));
            }

            List<Claim> claims = new List<Claim>();

            string userIdClaim = null;
            string roleId = string.Empty;
            string roleName = string.Empty;
            await (from u in dbContext.Systemuser
                   join ur in dbContext.Systemuserroles
                   on u.Systemuserid equals ur.Systemuserid
                   join r in dbContext.Role
                   on ur.Roleid equals r.Roleid
                   where u.Internalemailaddress == userEmail
                   select new { Id = r.Roleid, r.Name, UserId = u.Systemuserid.ToString(), UserFullName = u.Fullname, u.Azureactivedirectoryobjectid, Claimtype = RoleClaimType }).
                   Union(from u in dbContext.Systemuser
                         join tm in dbContext.Teammembership
                         on u.Systemuserid equals tm.Systemuserid
                         join t in dbContext.Team
                         on tm.Teamid equals t.Teamid
                         where u.Internalemailaddress == userEmail
                         select new { Id = t.Teamid, t.Name, UserId = u.Systemuserid.ToString(), UserFullName = u.Fullname, u.Azureactivedirectoryobjectid, Claimtype = TeamClaimType }).
                       ForEachAsync(entity =>
                       {
                           if (userIdClaim == null)
                           {
                               userIdClaim = entity.UserId;
                               string azureActiveDirectoryObjectId = entity.Azureactivedirectoryobjectid != null ? entity.Azureactivedirectoryobjectid.ToString() : null;
                               claims.Add(new Claim(IdClaimType, userIdClaim));
                               claims.Add(new Claim(FullNameClaimType, entity.UserFullName));
                               claims.Add(new Claim(EmailClaimType, userEmail));
                               claims.Add(new Claim(AzureActiveDirectoryObjectIdClaimType, azureActiveDirectoryObjectId));
                           }

                           bool isRole = entity.Claimtype == RoleClaimType;
                           claims.Add(new Claim(isRole ? RoleIdClaimType : TeamIdClaimType, entity.Id.ToString()));
                           claims.Add(new Claim(isRole ? RoleNameClaimType : TeamNameClaimType, entity.Name));
                       });

            if (claims.Count == 0)
            {
                throw new AuthorizationException(string.Format(NotFoundErrorMessage, userEmail), innerException: null);
            }

            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }

        /// <summary>
        /// Creates claims principal from user id.
        /// </summary>
        /// <param name="userId">The user email.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns>
        /// The claims principal.
        /// </returns>
        public static async Task<ClaimsPrincipal> CreateClaimsPrincipalFromUserIdAsync(Guid userId, CustomSearchesDbContext dbContext)
        {
            List<Claim> claims = new List<Claim>();

            string userIdClaim = null;
            string roleId = string.Empty;
            string roleName = string.Empty;
            await (from u in dbContext.Systemuser
                   join ur in dbContext.Systemuserroles
                   on u.Systemuserid equals ur.Systemuserid
                   join r in dbContext.Role
                   on ur.Roleid equals r.Roleid
                   where u.Systemuserid == userId
                   select new { Id = r.Roleid, r.Name, UserEmail = u.Internalemailaddress, UserFullName = u.Fullname, u.Azureactivedirectoryobjectid, Claimtype = RoleClaimType }).
                   Union(from u in dbContext.Systemuser
                         join tm in dbContext.Teammembership
                         on u.Systemuserid equals tm.Systemuserid
                         join t in dbContext.Team
                         on tm.Teamid equals t.Teamid
                         where u.Systemuserid == userId
                         select new { Id = t.Teamid, t.Name, UserEmail = u.Internalemailaddress, UserFullName = u.Fullname, u.Azureactivedirectoryobjectid, Claimtype = TeamClaimType }).
                       ForEachAsync(entity =>
                       {
                           if (userIdClaim == null)
                           {
                               userIdClaim = userId.ToString();
                               string azureactivedirectoryobjectid = entity.Azureactivedirectoryobjectid != null ? entity.Azureactivedirectoryobjectid.ToString() : null;
                               claims.Add(new Claim(IdClaimType, userIdClaim));
                               claims.Add(new Claim(FullNameClaimType, entity.UserFullName));
                               claims.Add(new Claim(EmailClaimType, entity.UserEmail));
                               claims.Add(new Claim(AzureActiveDirectoryObjectIdClaimType, azureactivedirectoryobjectid));
                           }

                           bool isRole = entity.Claimtype == RoleClaimType;
                           claims.Add(new Claim(isRole ? RoleIdClaimType : TeamIdClaimType, entity.Id.ToString()));
                           claims.Add(new Claim(isRole ? RoleNameClaimType : TeamNameClaimType, entity.Name));
                       });

            if (claims.Count == 0)
            {
                throw new AuthorizationException(string.Format(NotFoundErrorMessage, userId), innerException: null);
            }

            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
