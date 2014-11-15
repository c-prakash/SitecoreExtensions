/*
 --------------------------------------------------------------
 ***** Source code taken from *****
 http://webcmd.wordpress.com/2012/07/09/federated-authentication-with-sitecore-and-the-windows-identity-foundation/
 --------------------------------------------------------------
 */

using System.Collections.Specialized;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;

namespace Framework.Sc.Extensions.Security
{
    public class ClaimsAuthenticationProvider : MembershipAuthenticationProvider
    {
        #region Fields

        private ClaimsAuthenticationHelper _helper;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the helper object.
        /// </summary>
        /// <value>
        /// The helper.
        /// </value>
        protected override AuthenticationHelper Helper
        {
            get
            {
                var authenticationHelper = _helper;
                Assert.IsNotNull(authenticationHelper, "AuthenticationHelper has not been set. It must be set in Initialize.");
                return authenticationHelper;
            }
        }

        #endregion

        #region MembershipAuthenticationProvider Overrides

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            Assert.ArgumentNotNullOrEmpty(name, "name");
            Assert.ArgumentNotNull(config, "config");

            base.Initialize(name, config);
            _helper = new ClaimsAuthenticationHelper(this);
        }

        /// <summary>
        /// Gets the active user.
        /// </summary>
        /// <returns>
        /// Active User.
        /// </returns>
        public override User GetActiveUser()
        {
            var activeUser = Helper.GetActiveUser();
            Assert.IsNotNull(activeUser, "Active user cannot be empty.");
            return activeUser;
        }

        /// <summary>
        /// Logs the specified user into the system without checking password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="persistent">If set to <c>true</c> (and the provider supports it), the login will be persisted.</param>
        /// <returns></returns>
        public override bool Login(string userName, bool persistent)
        {
            Assert.ArgumentNotNullOrEmpty(userName, "userName");

            SessionSecurityToken sessionToken;
            if (!FederatedAuthentication.SessionAuthenticationModule.TryReadSessionTokenFromCookie(out sessionToken))
            {
                var claims = new[] { new Claim(ClaimTypes.Name, Globalize(Context.Domain.Name, userName)), new Claim(ClaimTypes.Role, "Secured") };
                var id = new ClaimsIdentity(claims, "Forms");
                var cp = new ClaimsPrincipal(id);

                var token = new SessionSecurityToken(cp);
                var sam = FederatedAuthentication.SessionAuthenticationModule;
                sam.WriteSessionTokenToCookie(token);
            }

            return true;
        }

        /// <summary>
        /// Logs in the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public override bool Login(User user)
        {
            Assert.ArgumentNotNull(user, "user");

            return Login(user.Name, false);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        public override void Logout()
        {
            SessionSecurityToken sessionToken;
            if (!FederatedAuthentication.SessionAuthenticationModule.TryReadSessionTokenFromCookie(out sessionToken))
            {
                // Clean up
            }
            base.Logout();
            FederatedAuthentication.SessionAuthenticationModule.SignOut();
        }

        /// <summary>
        /// Sets the active user.
        /// </summary>
        /// <param name="user">The user object.</param>
        public override void SetActiveUser(User user)
        {
            Helper.SetActiveUser(user);
        }

        /// <summary>
        /// Sets the active user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public override void SetActiveUser(string userName)
        {
            Assert.ArgumentNotNullOrEmpty(userName, "userName");
            Helper.SetActiveUser(userName);
        }

        #endregion

        #region Methods

        private static string Globalize(string domainName, string userName)
        {
            var str = userName;
            if (!userName.StartsWith(domainName + "\\"))
                str = domainName + "\\" + userName;
            return str;
        }

        #endregion
    }
}
