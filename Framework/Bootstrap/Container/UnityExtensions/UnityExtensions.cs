using Microsoft.Practices.Unity;

namespace Framework.Bootstrap.Container.UnityExtensions
{
    public interface IUnityInstaller
    {
        void Install(IUnityContainer container);
    }

    public static class UnityExtensions
    {
        public static void Register(this IUnityContainer container, IUnityInstaller installer)
        {
            installer.Install(container);
        }

        public static void Install(this IUnityContainer container, params IUnityInstaller[] installers)
        {
            foreach (var installer in installers)
                installer.Install(container);
        }
    }
}
