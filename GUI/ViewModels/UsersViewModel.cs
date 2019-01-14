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
        private ObservableCollection<User> _showingUsers;
        private string _searchText = "";


        private bool _canCreateUser;
        private bool _canEditUser;

        public ObservableCollection<User> Users
        {
            get
            {
                return _showingUsers;
            }
            set
            {
                SetProperty(ref _showingUsers, value);
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

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                SetProperty(ref _searchText, value);
                UpdateShowingUsers();
            }
        }

        private void UpdateShowingUsers()
        {
            Users.Clear();
            var showingUsers = _users.Where(u =>
            {
                if (SearchText == "")
                {
                    return true;
                }
                if (u.Username.Contains(SearchText))
                {
                    return true;
                }
                return false;
            });
            if (CanCreateUser)
            {
                Users.Add(null);
            }
            Users.AddRange(showingUsers);
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

            _users = new ObservableCollection<User>();
            _showingUsers = new ObservableCollection<User>();
            _users.AddRange(_userService.GetAllUsers());
            UpdateShowingUsers();
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

            _users.Clear();
            _users.AddRange(_userService.GetAllUsers());
            UpdateShowingUsers();

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
