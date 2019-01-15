using ProjectManager.ViewModels.Bases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Prism.Interactivity.InteractionRequest;
using Prism.Commands;
using Prism.Regions;
using GUI.AppInitializer.Stores;
using GUI.Core.ProjectService;

namespace GUI.ViewModels
{
    class ProjectsViewModel: ViewModelBase, INavigationAware
    {
        private ObservableCollection<Project> _projects;
        private ObservableCollection<Project> _showingProjects;
        private string _searchText = "";

        private bool _canCreateProject;
        public ObservableCollection<Project> Projects
        {
            get
            {
                return _showingProjects;
            }
            set
            {
                SetProperty(ref _showingProjects, value);
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
                UpdateShowingProjects();
            }
        }

        private void UpdateShowingProjects()
        {
            Projects.Clear();
            
            var showingProjects = _projects.Where(p =>
            {
                if(SearchText == "")
                {
                    return true;
                }
                if (p.Name.Contains(SearchText))
                {
                    return true;
                }
                else if (p.Key.Contains(SearchText))
                {
                    return true;
                }
                return false;
            });
            if (CanCreateProject)
            {
                Projects.Add(null);
            }
            Projects.AddRange(showingProjects);
        }


        public bool CanCreateProject
        {
            get
            {
                return _canCreateProject;
            }
            set
            {
                SetProperty(ref _canCreateProject, value);
            }
        }

        public DelegateCommand ShowCreateProjectCommand
        {
            get;
            set;
        }

        public DelegateCommand<int?> ViewProjectCommand
        {
            get;
            set;
        }

        private IRegionManager _regionManager;
        private IStore _store;
        private IProjectService _projectService;

        public ProjectsViewModel(IRegionManager regionManager, IStore store, IProjectService projectService)
        {
            _regionManager = regionManager;
            _store = store;
            _projectService = projectService;

            var permission = _store.GetCurrentUser().Permission;
            CanCreateProject = permission == Permission.OWNER;
            _showingProjects = new ObservableCollection<Project>();
            _projects = new ObservableCollection<Project>();
            if(permission == Permission.ADMIN)
            {
                _projects.AddRange(_projectService.GetAllProjects());
            }
            else
            {
                _projects.AddRange(_projectService.GetAllProjectsByMember(_store.GetCurrentUser().Id));
            }
            UpdateShowingProjects();
        }

        private void ShowCreateProject()
        {
            _regionManager.RequestNavigate("ContentRegion", "CreateProject");
        }



        protected override void RegisterCommands()
        {
            ShowCreateProjectCommand = new DelegateCommand(ShowCreateProject);
            ViewProjectCommand = new DelegateCommand<int?>(ViewProject);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            CanCreateProject = _store.GetCurrentUser().Permission == Permission.OWNER;
            _projects.Clear();
            var permission = _store.GetCurrentUser().Permission;
            if (permission == Permission.ADMIN)
            {
                _projects.AddRange(_projectService.GetAllProjects());
            }
            else
            {
                _projects.AddRange(_projectService.GetAllProjectsByMember(_store.GetCurrentUser().Id));
            }
            UpdateShowingProjects();
        }

        private void ViewProject(int? projectId)
        {
            if (projectId == null)
            {
                return;
            }
            var project = Projects.FirstOrDefault(x => x != null && x.Id == projectId);
            if(project.CreatedUser.Id != _store.GetCurrentUser().Id)
            {
                return;
            }
            var navigationParameter = new NavigationParameters
            {
                { "Project", project }
            };
            _regionManager.RequestNavigate("ContentRegion", "ProjectDetails", navigationParameter);

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
