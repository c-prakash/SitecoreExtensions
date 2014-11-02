using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IoC.Registration
{
    public class NamedRegistrationDefinition<TRegisteredAs, TResolvedTo> : RegistrationDefinition<TRegisteredAs, TResolvedTo>, INamedRegistrationDefinition
        where TRegisteredAs : class
        where TResolvedTo : TRegisteredAs
    {
        public NamedRegistrationDefinition(string name)
        {
            Name = name;
        }

        public string Name { get; protected set; }
    }

    public interface INamedRegistrationDefinition
    {
        string Name { get; }
    }

    public class RegistrationDefinition<TRegisteredAs, TResolvedTo> : IRegistrationDefinition<TRegisteredAs, TResolvedTo>
        where TRegisteredAs : class
        where TResolvedTo : TRegisteredAs
    {
        public LifetimeScope Scope { get; set; }
    }

    public interface IRegistrationDefinition<TRegisteredAs, TResolvedTo>
        where TRegisteredAs : class
        where TResolvedTo : TRegisteredAs
    {
        LifetimeScope Scope { get; set; }
    }

    
}
