using BUS.Exceptions;
using DTO;
using GUI.AppInitializer.Stores;
using GUI.Core.ProjectService;
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
    public class TasksViewModel: ViewModelBase, INavigationAware
    {
        public ObservableCollection<Project> UndoneProjects
        {
            get;
            set;
        }

        private IProjectService _projectService;
        private IStore _store;

        public TasksViewModel(IProjectService projectService, IStore store)
        {
            _store = store;
            _projectService = projectService;
            UndoneProjects = new ObservableCollection<Project>();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                UndoneProjects.Clear();
                UndoneProjects.AddRange(_projectService.GetAllUndoneProjects(_store.GetCurrentUser().Id));
            }
            catch (CheckedException e)
            {
                ShowError("Error", e.Message);
            }
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
