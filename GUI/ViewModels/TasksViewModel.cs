using BUS.Exceptions;
using DTO;
using GUI.AppInitializer.Stores;
using GUI.Core.ProjectService;
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
    public class TasksViewModel: ViewModelBase, INavigationAware
    {
        public ObservableCollection<Project> UndoneProjects
        {
            get
            {
                return _undoneProjects;
            }
            set
            {
                SetProperty(ref _undoneProjects, value);
            }
        }

        public DelegateCommand<int?> UpdateTaskCommand
        {
            get;
            set;
        }

        private ObservableCollection<Project> _undoneProjects;
        private IProjectService _projectService;
        private IStore _store;

        public TasksViewModel(IProjectService projectService, IStore store)
        {
            _store = store;
            _projectService = projectService;
            _undoneProjects = new ObservableCollection<Project>();
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

        protected override void RegisterCommands()
        {
            base.RegisterCommands();
            UpdateTaskCommand = new DelegateCommand<int?>(UpdateTask);
        }

        private void UpdateTask(int? taskId)
        {
            try
            {
                _projectService.UpdateTaskCompletion((int)taskId);
                UndoneProjects.Clear();
                UndoneProjects.AddRange(_projectService.GetAllUndoneProjects(_store.GetCurrentUser().Id));
            }
            catch (CheckedException e)
            {
                ShowError("Error", e.Message);
            }
        }
    }
}
