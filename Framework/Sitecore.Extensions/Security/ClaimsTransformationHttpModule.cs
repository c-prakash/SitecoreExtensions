using System;
using System.IdentityModel.Services;
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace Framework.Sc.Extensions.Security
{
    public class ClaimsTransformationHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += context_PostAuthenticateRequest;
        }

        void context_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            
            if (context == null)
                return;

            if (FederatedAuthentication.SessionAuthenticationModule == null)
                return;

            if (!FederatedAuthentication.SessionAuthenticationModule.ContainsSessionTokenCookie(context.Request.Cookies))
                return;

            var transformer = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager;

            if (transformer != null)
            {
                var identity = context.User.Identity as ClaimsIdentity;
                if (identity == null)
                    return;

                var transformedPrincipal = transformer.Authenticate(context.Request.RawUrl, new ClaimsPrincipal(identity));

                //context.User = transformedPrincipal;
                //Thread.CurrentPrincipal = transformedPrincipal;
            }
        }

        public void Dispose() { }
    }
}
