using System;
using System.Collections.Generic;
using System.Text;
using static SyncDataBase.SyncDB;

namespace SyncDataBase.Auth
{
    public class AuthenticationParameters
    {
        private const string AuthorityProd = "https://login.microsoftonline.com/bae5059a-76f0-49d7-9996-72dfe95d69c7";
        private const string ScopeResourceUriProd = "0f209150-ad01-4160-9629-66746e3ed1ee";
        private const string ApplicationIdProd = "0f209150-ad01-4160-9629-66746e3ed1ee";
        private const string ReturnUriProd = "myapp://auth";

        private const string AuthorityTest = "https://login.microsoftonline.com/bae5059a-76f0-49d7-9996-72dfe95d69c7";
        private const string ScopeResourceUriTest = "0f209150-ad01-4160-9629-66746e3ed1ee";
        private const string ApplicationIdTest = "0f209150-ad01-4160-9629-66746e3ed1ee";
        private const string ReturnUriTest = "myapp://auth";

        private const string AuthorityDev = "https://login.microsoftonline.com/bae5059a-76f0-49d7-9996-72dfe95d69c7";
        private const string ScopeResourceUriDev = "0f209150-ad01-4160-9629-66746e3ed1ee";
        private const string ApplicationIdDev = "0f209150-ad01-4160-9629-66746e3ed1ee";
        private const string ReturnUriDev = "myapp://auth";

        public static string Authority(PTASEnvironment env)
        {
            switch (env)
            {
                case PTASEnvironment.Development:
                    return AuthorityDev;
                case PTASEnvironment.Production:
                    return AuthorityProd;
                case PTASEnvironment.Testing:
                    return AuthorityTest;
                default:
                    return null;
            }
        }

        public static string ScopeResourceUri(PTASEnvironment env)
        {
            switch (env)
            {
                case PTASEnvironment.Development:
                    return ScopeResourceUriDev;
                case PTASEnvironment.Production:
                    return ScopeResourceUriProd;
                case PTASEnvironment.Testing:
                    return ScopeResourceUriTest;
                default:
                    return null;
            }
        }

        public static string ApplicationId(PTASEnvironment env)
        {
            switch (env)
            {
                case PTASEnvironment.Development:
                    return ApplicationIdDev;
                case PTASEnvironment.Production:
                    return ApplicationIdProd;
                case PTASEnvironment.Testing:
                    return ApplicationIdTest;
                default:
                    return null;
            }
        }

        public static string ReturnUri(PTASEnvironment env)
        {
            switch (env)
            {
                case PTASEnvironment.Development:
                    return ReturnUriDev;
                case PTASEnvironment.Production:
                    return ReturnUriProd;
                case PTASEnvironment.Testing:
                    return ReturnUriTest;
                default:
                    return null;
            }
        }


    }
}
