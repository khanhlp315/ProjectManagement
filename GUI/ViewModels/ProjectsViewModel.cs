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


        private bool _canCreateProject;
        public ObservableCollection<Project> Projects
        {
            get
            {
                return _projects;
            }
            set
            {
                SetProperty(ref _projects, value);
            }
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

            CanCreateProject = _store.GetCurrentUser().Permission == Permission.OWNER;

            Projects = new ObservableCollection<Project>();
            if(CanCreateProject)
            {
                Projects.Add(null);
            }
            Projects.AddRange(_projectService.GetAllProjectsByMember(_store.GetCurrentUser().Id));
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
            Projects.Clear();
            if(CanCreateProject)
            {
                Projects.Add(null);
            }
            Projects.AddRange(_projectService.GetAllProjectsByMember(_store.GetCurrentUser().Id));
        }

        private void ViewProject(int? projectId)
        {
            if (projectId == null)
            {
                return;
            }
            var project = Projects.FirstOrDefault(x => x != null && x.Id == projectId);
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
