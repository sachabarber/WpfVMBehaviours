using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace WpfBehaviours.Infrastructure.ExtensionMethods
{
    public static class ContainerExtensions
    {
        public static void RegisterTypeForNavigation<T>(this IUnityContainer container, LifetimeManager lifetimeManager)
        {
            container.RegisterType(typeof(Object), typeof(T), typeof(T).FullName, lifetimeManager);
        }

        public static void RegisterTypeForNavigationWithChildContainer<T>(this IUnityContainer container, LifetimeManager lifetimeManager)
        {
            container.RegisterType(typeof(Object), typeof(T), typeof(T).FullName,
                lifetimeManager,
                new InjectionMethod("AddDisposable", new object[] { container }));
        }
    }
}
