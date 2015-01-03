using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Web;

namespace Framework.Sc.Extensions.Security
{
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
                return incomingPrincipal;

            var newPrincipal = Transform(incomingPrincipal);
            EstablishSession(newPrincipal);
            return newPrincipal;
        }

        protected virtual ClaimsPrincipal Transform(ClaimsPrincipal incomingPrincipal)
        {
            var identity = incomingPrincipal.Identity as ClaimsIdentity;
            if (identity != null && !incomingPrincipal.HasClaim(ClaimTypes.MobilePhone, "Hi"))
            {
                identity.AddClaim(new Claim(ClaimTypes.MobilePhone, "Hi"));
                identity.AddClaim(new Claim(ClaimTypes.Role, "Secured"));
            }

            return incomingPrincipal;
        }

        private void EstablishSession(ClaimsPrincipal principal)
        {
            if (HttpContext.Current != null)
            {
                var sessionToken = new SessionSecurityToken(principal);
                FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(sessionToken);
            }
        }
    }
}
