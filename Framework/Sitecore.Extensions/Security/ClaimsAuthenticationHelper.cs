/*
 --------------------------------------------------------------
 ***** Source code taken from *****
 http://webcmd.wordpress.com/2012/07/09/federated-authentication-with-sitecore-and-the-windows-identity-foundation/
 --------------------------------------------------------------
 */

using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Principal;
using System.Threading;
using System.Web;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;

namespace Framework.Sc.Extensions.Security
{
    public class ClaimsAuthenticationHelper : AuthenticationHelper
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsAuthenticationHelper"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public ClaimsAuthenticationHelper(AuthenticationProvider provider)
            : base(provider)
        {
        }

        #endregion

        #region AuthenticationHelper Overrides

        /// <summary>
        /// Sets the active user.
        /// </summary>
        /// <param name="user">The user object.</param>
        public override void SetActiveUser(User user)
        {
            Assert.ArgumentNotNull(user, "user");

            var name = user.Name;
            if (!name.Contains("\\"))
                Globalize(Context.Domain.Name, name);
            base.SetActiveUser(user);
        }

        /// <summary>
        /// Sets the active user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public override void SetActiveUser(string userName)
        {
            Assert.ArgumentNotNull(userName, "userName");
            var userName1 = userName;
            if (!userName1.Contains("\\"))
                Globalize(Context.Domain.Name, userName1);
            base.SetActiveUser(userName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether the specified user is disabled.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        protected virtual bool IsDisabled(User user)
        {
            Assert.ArgumentNotNull(user, "user");

            return !user.Profile.IsAnonymous && user.Profile.State.Contains("Disabled");
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>
        /// The current user; <c>null</c> if user is not defined (anonymous).
        /// </returns>
        protected override User GetCurrentUser()
        {
            HttpContext current = HttpContext.Current;
            if (current == null)
            {
                if (Thread.CurrentPrincipal != null)
                {
                    if (Thread.CurrentPrincipal is User)
                    {
                        return Thread.CurrentPrincipal as User;
                    }
                    if (!string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name))
                    {
                        //return base.GetUser(Thread.CurrentPrincipal.Identity.Name, Thread.CurrentPrincipal.Identity.IsAuthenticated);
                        return GetUser(Thread.CurrentPrincipal.Identity);
                    }
                }

                return null;
            }
            IPrincipal user = HttpContext.Current.User;
            if (user != null)
            {
                if (user is User)
                {
                    return user as User;
                }
                IIdentity identity = user.Identity;
                if (string.IsNullOrEmpty(identity.Name))
                {
                    return null;
                }
                //return base.GetUser(identity.Name, identity.IsAuthenticated);
                return GetUser(identity);
            }

            SessionSecurityToken sessionToken;
            FederatedAuthentication.SessionAuthenticationModule.TryReadSessionTokenFromCookie(out sessionToken);
            if (sessionToken != null && sessionToken.ClaimsPrincipal != null)
            {
                var identity = sessionToken.ClaimsPrincipal.Identity;
                if (!string.IsNullOrEmpty(identity.Name)) //&& User.Exists(Globalize(Context.Domain.Name, identity.Name)))
                    //return AuthenticationHelper.GetUser(Globalize(Context.Domain.Name, identity.Name), true);
                    return GetUser(sessionToken.ClaimsPrincipal);
            }

            return base.GetCurrentUser();
        }

        private static User GetUser(IPrincipal principal)
        {
            Assert.ArgumentNotNull(principal, "principal");

            return User.FromPrincipal(principal);
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns></returns>
        private static User GetUser(IIdentity identity)
        {
            Assert.ArgumentNotNull(identity, "identity");

            return User.FromPrincipal(new System.Security.Claims.ClaimsPrincipal(identity));
        }

        private new static User GetUser(string userName, bool isAuthenticated)
        {
            Assert.ArgumentNotNull(userName, "userName");

            return User.FromName(userName, isAuthenticated);
        }

        /// <summary>
        /// Globalizes the specified user in specified domain name.
        /// </summary>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Return globalized user name.</returns>
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
