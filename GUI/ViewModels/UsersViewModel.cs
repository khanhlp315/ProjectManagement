using DTO;
using GUI.AppInitializer.Stores;
using GUI.Core.UserService;
using Prism.Commands;
using Prism.Regions;
using ProjectManager.ViewModels.Bases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
    class UsersViewModel: ViewModelBase, INavigationAware
    {
        private ObservableCollection<User> _users;


        private bool _canCreateUser;
        private bool _canEditUser;
        public ObservableCollection<User> Users
        {
            get
            {
                return _users;
            }
            set
            {
                SetProperty(ref _users, value);
            }
        }

        public bool CanCreateUser
        {
            get
            {
                return _canCreateUser;
            }
            set
            {
                SetProperty(ref _canCreateUser, value);
            }
        }

        public bool CanEditUser
        {
            get
            {
                return _canEditUser;
            }
            set
            {
                SetProperty(ref _canEditUser, value);
            }
        }

        public DelegateCommand ShowCreateUserCommand
        {
            get;
            set;
        }

        public DelegateCommand<int?> EditUserCommand
        {
            get;
            set;
        }

        private IUserService _userService;
        private IRegionManager _regionManager;
        private IStore _store;

        public UsersViewModel(IRegionManager regionManager, IUserService userService, IStore store)
        {
            _userService = userService;
            _regionManager = regionManager;
            _store = store;

            CanCreateUser = _store.GetCurrentUser().Permission == Permission.ADMIN;
            CanEditUser = _store.GetCurrentUser().Permission == Permission.ADMIN;

            Users = new ObservableCollection<User>();
            if(CanCreateUser)
            {
                Users.Add(null);
            }
            Users.AddRange(_userService.GetAllUsers());


        }

        private void ShowCreateUser()
        {
            _regionManager.RequestNavigate("ContentRegion", "CreateUser");
        }

        private void EditUser(int? userId)
        {
            if(userId == null)
            {
                return;
            }
            var user = Users.FirstOrDefault(x => x!= null && x.Id == userId);
            var navigationParameter = new NavigationParameters
            {
                { "User", user }
            };
            _regionManager.RequestNavigate("ContentRegion", "EditUser", navigationParameter);

        }



        protected override void RegisterCommands()
        {
            ShowCreateUserCommand = new DelegateCommand(ShowCreateUser);
            EditUserCommand = new DelegateCommand<int?>(EditUser);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            CanCreateUser = _store.GetCurrentUser().Permission == Permission.ADMIN;
            CanEditUser = _store.GetCurrentUser().Permission == Permission.ADMIN;

            Users.Clear();
            if (CanCreateUser)
            {
                Users.Add(null);
            }
            Users.AddRange(_userService.GetAllUsers());


        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
