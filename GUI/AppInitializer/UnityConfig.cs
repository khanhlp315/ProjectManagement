using GUI.AppInitializer.Stores;
using GUI.Core.ProjectService;
using GUI.Core.UserService;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.AppInitializer
{
    public static class UnityConfig
    {
        public static IUnityContainer RegisterInstances(this IUnityContainer container)
        {
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IProjectService, ProjectService>();

            container.RegisterType<IStore, Store>(new ContainerControlledLifetimeManager());
            return container;
        }
    }

}
