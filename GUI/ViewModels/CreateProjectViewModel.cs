using BUS.Exceptions;
using DTO;
using GUI.AppInitializer.Stores;
using GUI.Core.ProjectService;
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
    class CreateProjectViewModel: ViewModelBase
    {
        private string _projectKey;
        private string _projectName;

        public string ProjectKey
        {
            get
            {
                return _projectKey;
            }
            set
            {
                SetProperty(ref _projectKey, value);
            }
        }

        public string ProjectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                SetProperty(ref _projectName, value);
            }
        }

        private IRegionManager _regionManager;
        private IProjectService _createProject;
        private IStore _store;
        public CreateProjectViewModel(IProjectService createProject, IRegionManager regionManager, IStore store)
        {
            _regionManager = regionManager;
            _createProject = createProject;
            _store = store;
        }

        private void CreateProject()
        {
            Project project = null;
            try
            {
                project = _createProject.CreateProject(_store.GetCurrentUser().Id, ProjectKey, ProjectName);
                _regionManager.RequestNavigate("ContentRegion", "Projects");
                ProjectKey = "";
                ProjectName = "";
            }
            catch (CheckedException e)
            {
                ShowError("Error", e.Message);
            }
        }

        protected override void RegisterCommands()
        {
            CreateProjectCommand = new DelegateCommand(CreateProject);
        }

        public DelegateCommand CreateProjectCommand
        {
            get;
            set;
        }
    }
}
