using Sitecore.Security.Accounts;
using System.Reflection;
using System.Security.Claims;

namespace Framework.Sc.Extensions.Helpers
{
    public static class UserExtensions
    {
        public static ClaimsPrincipal ToClaimsPrincipal(this User user)
        {
            var innerUserField = user.GetType().GetField("_innerUser", BindingFlags.NonPublic | BindingFlags.Instance);
            if (innerUserField != null)
                return innerUserField.GetValue(user) as ClaimsPrincipal;

            return null;
        }
    }
}
