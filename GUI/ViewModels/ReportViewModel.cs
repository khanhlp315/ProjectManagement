using DTO;
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
    public class ReportViewModel: ViewModelBase, INavigationAware
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private ObservableCollection<ReportObject> _reportObjects;
        private IProjectService _projectService;

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                
                SetProperty(ref _startDate, value.Date);
                ReportObjects.Clear();
                ReportObjects.AddRange(_projectService.GetReports(StartDate, EndDate));

            }
        }

        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                SetProperty(ref _endDate, value.Date);
                ReportObjects.Clear();
                ReportObjects.AddRange(_projectService.GetReports(StartDate, EndDate));

            }
        }

        public ObservableCollection<ReportObject> ReportObjects
        {
            get
            {
                return _reportObjects;
            }
            set
            {
                SetProperty(ref _reportObjects, value);
            }
        }

        public ReportViewModel(IProjectService projectService)
        {
            _projectService = projectService;

            _reportObjects = new ObservableCollection<ReportObject>();
            _startDate = DateTime.Today;
            _endDate = DateTime.Today;

            _reportObjects.AddRange(_projectService.GetReports(StartDate, EndDate));
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ReportObjects.Clear();
            ReportObjects.AddRange(_projectService.GetReports(StartDate, EndDate));
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
