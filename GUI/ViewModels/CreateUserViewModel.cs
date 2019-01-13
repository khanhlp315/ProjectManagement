using BUS.Exceptions;
using DTO;
using GUI.Core.UserService;
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
    public class CreateUserViewModel: ViewModelBase
    {
        private string _username;
        private string _password;
        private Permission _permission;

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

        public Permission UserPermission
        {
            get
            {
                return _permission;
            }
            set
            {
                SetProperty(ref _permission, value);
            }
        }

        private IRegionManager _regionManager;
        private IUserService _userService;
        public CreateUserViewModel(IUserService userService, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _userService = userService;
        }

        private void CreateUser()
        {
            User user = null;
            try
            {
                user = _userService.CreateUser(Username, Password, UserPermission);
                _regionManager.RequestNavigate("ContentRegion", "Users");
                Username = "";
                Password = "";
            }
            catch (CheckedException e)
            {
                ShowError("Error", e.Message);
            }
        }

        protected override void RegisterCommands()
        {
            CreateUserCommand = new DelegateCommand(CreateUser);
        }

        public DelegateCommand CreateUserCommand
        {
            get;
            set;
        }
    }
}
