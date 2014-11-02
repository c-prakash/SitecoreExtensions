
namespace Framework.Core.Infrastructure.IoC
{
    public interface IDependencyContainerFactory
    {
        IDependencyContainer CurrentContainer { get; }
    }
}
