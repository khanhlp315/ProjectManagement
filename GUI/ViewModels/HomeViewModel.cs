using GUI.AppInitializer.Stores;
using Prism.Commands;
using Prism.Regions;
using ProjectManager.ViewModels.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    public class HomeViewModel: ViewModelBase, IRegionMemberLifetime
    {
        private IStore _store;
        private IRegionManager _regionManager;

        public DelegateCommand LogoutCommand
        {
            get;
            set;
        }

        public DelegateCommand NavigateToProjectsCommand
        {
            get;
            set;
        }

        public DelegateCommand NavigateToUsersCommand
        {
            get;
            set;
        }

        public DelegateCommand NavigateToReportCommand
        {
            get;
            set;
        }

        public bool KeepAlive => false;

        protected override void RegisterCommands()
        {
            LogoutCommand = new DelegateCommand(Logout);
            NavigateToProjectsCommand = new DelegateCommand(NavigateToProjects);
            NavigateToUsersCommand = new DelegateCommand(NavigateToUsers);
            NavigateToReportCommand = new DelegateCommand(NavigateToReport);

        }

        private void NavigateToUsers()
        {
            _regionManager.RequestNavigate("ContentRegion", "Users");
        }

        private void NavigateToProjects()
        {
            _regionManager.RequestNavigate("ContentRegion", "Projects");
        }

        private void NavigateToReport()
        {
            _regionManager.RequestNavigate("ContentRegion", "Report");
        }

        private void Logout()
        {
            _store.SetCurrentUser(null);
            _regionManager.RequestNavigate("MainRegion", "Login");
        }

        public HomeViewModel(IStore store, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _store = store;
        }
    }
}
