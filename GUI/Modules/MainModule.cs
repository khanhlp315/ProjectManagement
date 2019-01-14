using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace GUI.Modules
{
    class MainModule: IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public MainModule(RegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterTypeForNavigation<Login>();
            _container.RegisterTypeForNavigation<Home>();
            _container.RegisterTypeForNavigation<Projects>();
            _container.RegisterTypeForNavigation<Users>();
            _container.RegisterTypeForNavigation<Report>();
            _container.RegisterTypeForNavigation<CreateProject>();
            _container.RegisterTypeForNavigation<CreateUser>();
            _container.RegisterTypeForNavigation<EditUser>();
            _container.RegisterTypeForNavigation<ProjectDetails>();




            _regionManager.RegisterViewWithRegion("MainRegion", typeof(Login));
            _regionManager.RegisterViewWithRegion("ContentRegion", typeof(Projects));

        }

    }
}
