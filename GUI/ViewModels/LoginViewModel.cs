using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.AppInitializer.Stores;
using GUI.Core.UserService;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using ProjectManager.ViewModels.Bases;
using DTO;
using BUS.Exceptions;

namespace GUI.ViewModels
{
    public class LoginViewModel: ViewModelBase, INavigationAware
    {
        private string _username;
        private string _password;

        private IStore _store;
        private IUserService _login;

        private IRegionManager _regionManager;

        public LoginViewModel(IUserService login, IStore store, IRegionManager regionManager)
        {
            _login = login;
            _regionManager = regionManager;
            _store = store;
            _store.SetCurrentUser(null);
            Username = "";
            Password = "";
        }

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                SetProperty(ref _username, value);
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                SetProperty(ref _password, value);
            }
        }

        public DelegateCommand LoginCommand
        {
            get;
            set;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Username = "";
            Password = "";
        }

        protected override void RegisterCommands()
        {
            LoginCommand = new DelegateCommand(Login);
        }

        private void Login()
        {
            User user = null;
            try
            {
                user = _login.Login(Username, Password);
                _store.SetCurrentUser(user);
                _regionManager.RequestNavigate("MainRegion", "Home");
            }
            catch (CheckedException e)
            {
                ShowError("Error", e.Message);
            }
        }
    }
}
