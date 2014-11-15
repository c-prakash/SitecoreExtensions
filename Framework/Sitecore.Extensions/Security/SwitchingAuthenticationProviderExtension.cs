using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using System.Reflection;

namespace Framework.Sc.Extensions.Security
{
    public class SwitchingAuthenticationProviderExtension : SwitchingAuthenticationProvider
    {
        //private readonly DomainMapHelper domainHelper;
        //public DomainMapHelper DomainHelper { get { return domainHelper; } }

        protected AuthenticationProvider CurrentProvider
        {
            get
            {
                var baseType = typeof(SwitchingAuthenticationProviderExtension).BaseType;
                if (baseType != null)
                {
                    var provider = baseType.GetProperty("CurrentProvider", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    return provider.GetValue(this) as AuthenticationProvider;
                }

                return null;
            }
        }

        //public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        //{
        //    base.Initialize(name, config);
        //    var domainHelper = new Sitecore.Security.DomainMapHelper(config);
        //}

        public override User GetActiveUser()
        {
            return CurrentProvider == null ? null : CurrentProvider.GetActiveUser();
        }
    }
}
