using Sitecore.Security;
using Sitecore.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class SwitchingAuthenticationProvider : Sitecore.Security.Authentication.SwitchingAuthenticationProvider
    {
        //private readonly DomainMapHelper domainHelper;
        //public DomainMapHelper DomainHelper { get { return domainHelper; } }

        protected AuthenticationProvider CurrentProvider
        {
            get
            {
                var provider = typeof(SwitchingAuthenticationProvider).BaseType.GetProperty("CurrentProvider", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                return provider.GetValue(this) as AuthenticationProvider;
            }
        }

        //public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        //{
        //    base.Initialize(name, config);
        //    var domainHelper = new Sitecore.Security.DomainMapHelper(config);
        //}

        public override Sitecore.Security.Accounts.User GetActiveUser()
        {
            return this.CurrentProvider.GetActiveUser();
        }
    }
}
