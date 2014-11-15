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
           // var nameClaim = incomingPrincipal.Identities.First().FindFirst(ClaimTypes.Name);
            var id = new ClaimsIdentity(new[] { new Claim(ClaimTypes.MobilePhone, "Hi") });
            var principal = new ClaimsPrincipal(id);
            return principal;
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
