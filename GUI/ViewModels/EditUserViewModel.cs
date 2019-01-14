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
    class EditUserViewModel: ViewModelBase, INavigationAware
    {
        private User _selectedUser;

        public User SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                SetProperty(ref _selectedUser, value);
            }
        }

        private IUserService _userService;
        private IRegionManager _regionManager;

        public EditUserViewModel(IUserService userService, IRegionManager regionManager)
        {
            _userService = userService;
            _regionManager = regionManager;
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
            SelectedUser = (User)navigationContext.Parameters["User"];
        }

        protected override void RegisterCommands()
        {
            EditUserCommand = new DelegateCommand(EditUser);
            DeleteUserCommand = new DelegateCommand(DeleteUser);
            BackCommand = new DelegateCommand(Back);
        }

        public DelegateCommand EditUserCommand
        {
            get;
            set;
        }

        public DelegateCommand DeleteUserCommand
        {
            get;
            set;
        }

        public DelegateCommand BackCommand
        {
            get;
            set;
        }

        private void EditUser()
        {
            try
            {
                _userService.EditUser(SelectedUser);
                _regionManager.RequestNavigate("ContentRegion", "Users");
            }
            catch (CheckedException ex)
            {
                ShowError("Error", ex.Message);
            }
        }

        private void DeleteUser()
        {
            try
            {
                _userService.DeleteUser(SelectedUser.Id);
                _regionManager.RequestNavigate("ContentRegion", "Users");
            }
            catch(CheckedException ex)
            {
                ShowError("Error", ex.Message);
            }
        }

        private void Back()
        {
            _regionManager.RequestNavigate("ContentRegion", "Users");
        }
    }
}
