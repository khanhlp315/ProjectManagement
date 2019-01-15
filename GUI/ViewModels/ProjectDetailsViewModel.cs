using BUS.Exceptions;
using DTO;
using GUI.Core.ProjectService;
using GUI.Utils;
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
    public class ProjectDetailsViewModel: ViewModelBase, INavigationAware
    {
        private Project _selectedProject;

        private string _projectName;
        private string _projectKey;

        private Role _selectedRole;

        private User _addingUser;
        private int _changingMemberId;
        private int _userStoryEpicId;
        private int _taskUserStoryId;
        private int _assigningTaskId;

        private bool _selectingRole;
        private bool _changingRole;
        private bool _addingEpic;
        private bool _addingUserStory;
        private bool _addingTask;
        private bool _startingSprint;
        private bool _assigningMember;

        private string _epicTitle;
        private string _epicDescription;

        private string _searchText = "";

        private string _taskTitle;

        private string _userStoryDescription;
        private string _userStoryTitle;
        private int _userStoryStoryPoints;

        private Member _assignedMember;

        private bool _isProjectStarted;

        private Sprint _currentSprint;

        private ObservableCollection<User> _notInProjectUsers = new ObservableCollection<User>();
        private ObservableCollection<User> _showingUsers = new ObservableCollection<User>();


        private Member _selectedMember;

        private ObservableCollection<Member> _members = new ObservableCollection<Member>();

        private void UpdateMembers()
        {
            var members = SelectedProject.Members.Where(m => m.User.Username.Contains(SearchText) || SearchText == "");
            Members.Clear();
            Members.AddRange(members);
        }

        private void UpdateUsers()
        {
            var users = _notInProjectUsers.Where(u => u.Username.Contains(SearchText) || SearchText == "");
            ShowingUsers.Clear();
            ShowingUsers.AddRange(users);
        }

        public ObservableCollection<Member> Members
        {
            get
            {
                return _members;
            }
            set
            {
                SetProperty(ref _members, value);
            }
        }

        public ObservableCollection<User> ShowingUsers
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

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                SetProperty(ref _searchText, value);
                UpdateMembers();
                UpdateUsers();
            }
        }

        public ObservableCollection<User> NotInProjectUsers
        {
            get
            {
                return _notInProjectUsers;
            }
            set
            {
                SetProperty(ref _notInProjectUsers, value); 
            }
        }

        public Member SelectedMember
        {
            get
            {
                return _selectedMember;
            }
            set
            {
                SetProperty(ref _selectedMember, value);
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

        public bool IsProjectStarted
        {
            get
            {
                return _isProjectStarted;
            }
            set
            {
                SetProperty(ref _isProjectStarted, value);
            }
        }

        public Project SelectedProject
        {
            get
            {
                return _selectedProject;
            }
            set
            {
                SetProperty(ref _selectedProject, value);
                foreach (var epic in SelectedProject.Epics)
                {
                    for (int i = 0; i < epic.UserStories.Count; ++i)
                    {
                        if (epic.UserStories[i].State != UserStoryState.BACKLOG)
                        {
                            epic.UserStories.Remove(epic.UserStories[i]);
                            i--;
                        }
                    }
                }
            }
        }

        public Role SelectedRole
        {
            get
            {
                return _selectedRole;
            }
            set
            {
                SetProperty(ref _selectedRole, value);
            }
        }

        public string EpicTitle
        {
            get
            {
                return _epicTitle;
            }
            set
            {
                SetProperty(ref _epicTitle, value);
            }
        }

        public string EpicDescription
        {
            get
            {
                return _epicDescription;
            }
            set
            {
                SetProperty(ref _epicDescription, value);
            }
        }

        public string UserStoryTitle
        {
            get
            {
                return _userStoryTitle;
            }
            set
            {
                SetProperty(ref _userStoryTitle, value);
            }
        }

        public string UserStoryDescription
        {
            get
            {
                return _userStoryDescription;
            }
            set
            {
                SetProperty(ref _userStoryDescription, value);
            }
        }

        public int UserStoryStoryPoints
        {
            get
            {
                return _userStoryStoryPoints;
            }
            set
            {
                SetProperty(ref _userStoryStoryPoints, value);

            }
        }

        public string TaskTitle
        {
            get
            {
                return _taskTitle;
            }
            set
            {
                SetProperty(ref _taskTitle, value);
            }
        }

        public Member AssignedMember
        {
            get
            {
                return _assignedMember;
            }
            set
            {
                SetProperty(ref _assignedMember, value);
            }
        }

        public Sprint CurrentSprint
        {
            get
            {
                return _currentSprint;
            }
            set
            {
                SetProperty(ref _currentSprint, value);
            }
        }

        public DelegateCommand<User> ShowAddMemberCommand
        {
            get;
            set;
        }

        public DelegateCommand AddMemberCommand
        {
            get;
            set;
        }

        public DelegateCommand CancelAddMemberCommand
        {
            get;
            set;
        }

        public DelegateCommand EditProjectCommand
        {
            get;
            set;
        }

        public DelegateCommand DeleteProjectCommand
        {
            get;
            set;
        }

        public DelegateCommand RemoveFromProjectCommand
        {
            get;
            set;
        }

        public DelegateCommand ChangeMemberRoleCommand
        {
            get;
            set;
        }

        public DelegateCommand CancelChangeRoleCommand
        {
            get;
            set;
        }

        public DelegateCommand ShowChangeRoleCommand
        {
            get;
            set;
        }

        public DelegateCommand ShowAddEpicCommand
        {
            get;
            set;
        }

        public DelegateCommand AddEpicCommand
        {
            get;
            set;
        }

        public DelegateCommand CancelAddEpicCommand
        {
            get;
            set;
        }

        public DelegateCommand AddUserStoryCommand
        {
            get;
            set;
        }

        public DelegateCommand CancelAddUserStoryCommand
        {
            get;
            set;
        }

        public DelegateCommand<int?> ShowAddUserStoryCommand
        {
            get;
            set;
        }

        public DelegateCommand StartProjectCommand
        {
            get;
            set;
        }

        public DelegateCommand<UserStory> AddUserStoryToSprintCommand
        {
            get;
            set;
        }

        public DelegateCommand ChangeSprintStateCommand
        {
            get;
            set;
        }

        public DelegateCommand<int?> ShowAddTaskCommand
        {
            get;
            set;
        }

        public DelegateCommand AddTaskCommand
        {
            get;
            set;
        }

        public DelegateCommand CancelAddTaskCommand
        {
            get;
            set;
        }

        public DelegateCommand<int?> ShowAssignMemberCommand
        {
            get;
            set;
        }

        public DelegateCommand AssignMemberCommand
        {
            get;
            set;
        }

        public DelegateCommand CancelAssignMemberCommand
        {
            get;
            set;
        }

        public bool SelectingRole
        {
            get
            {
                return _selectingRole;
            }
            set
            {
                SetProperty(ref _selectingRole, value);
            }
        }

        public bool ChangingRole
        {
            get
            {
                return _changingRole;
            }
            set
            {
                SetProperty(ref _changingRole, value);
            }
        }

        public bool AddingEpic
        {
            get
            {
                return _addingEpic;
            }
            set
            {
                SetProperty(ref _addingEpic, value);
            }
        }

        public bool AddingUserStory
        {
            get
            {
                return _addingUserStory;
            }
            set
            {
                SetProperty(ref _addingUserStory, value);
            }
        }

        public bool AddingTask
        {
            get
            {
                return _addingTask;
            }
            set
            {
                SetProperty(ref _addingTask, value);
            }
        }

        public bool StartingSprint
        {
            get
            {
                return _startingSprint;
            }
            set
            {
                SetProperty(ref _startingSprint, value);
            }
        }

        public bool AssigningMember
        {
            get
            {
                return _assigningMember;
            }
            set
            {
                SetProperty(ref _assigningMember, value);
            }
        }

        protected override void RegisterCommands()
        {
            ShowAddMemberCommand = new DelegateCommand<User>(ShowAddMember);
            AddMemberCommand = new DelegateCommand(AddMember);
            CancelAddMemberCommand = new DelegateCommand(CancelAddMember);
            EditProjectCommand = new DelegateCommand(EditProject);
            DeleteProjectCommand = new DelegateCommand(DeleteProject);
            RemoveFromProjectCommand = new DelegateCommand(RemoveMemberFromProject);
            ChangeMemberRoleCommand = new DelegateCommand(ChangeMemberRole);
            CancelChangeRoleCommand = new DelegateCommand(CancelChangeRole);
            ShowChangeRoleCommand = new DelegateCommand(ShowChangeRole);
            ShowAddEpicCommand = new DelegateCommand(ShowAddEpic);
            AddEpicCommand = new DelegateCommand(AddEpic);
            CancelAddEpicCommand = new DelegateCommand(CancelAddEpic);
            AddUserStoryCommand = new DelegateCommand(AddUserStory);
            CancelAddUserStoryCommand = new DelegateCommand(CancelAddUserStory);
            ShowAddUserStoryCommand = new DelegateCommand<int?>(ShowAddUserStory);
            StartProjectCommand = new DelegateCommand(StartProject);
            AddUserStoryToSprintCommand = new DelegateCommand<UserStory>(AddToSprint);
            ChangeSprintStateCommand = new DelegateCommand(ChangeSprintState);
            ShowAddTaskCommand = new DelegateCommand<int?>(ShowAddTask);
            AddTaskCommand = new DelegateCommand(AddTask);
            CancelAddTaskCommand = new DelegateCommand(CancelAddTask);
            ShowAssignMemberCommand = new DelegateCommand<int?>(ShowAssignMember);
            AssignMemberCommand = new DelegateCommand(AssignMember);
            CancelAssignMemberCommand = new DelegateCommand(CancelAssignMember);
        }

        private void CancelAssignMember()
        {
            AssigningMember = false;
        }

        private void ShowAssignMember(int? taskId)
        {
            AssignedMember = SelectedProject.Members[0];
            AssigningMember = true;
            _assigningTaskId = (int)taskId;
        }

        private void CancelAddTask()
        {
            AddingTask = false;
        }

        private void AddTask()
        {
            try
            {
                _projectService.AddTask(_taskUserStoryId, TaskTitle);
                SelectedProject = _projectService.GetProjectById(SelectedProject.Id);
                SelectedMember = null;
                UpdateMembers();
                AddingTask = false;
            }
            catch(CheckedException e)
            {
                ShowError("Error", e.Message);
            }
        }

        private void ShowAddTask(int? userStoryId)
        {
            _taskUserStoryId = (int)userStoryId;
            AddingTask = true;
        }

        private void ChangeSprintState()
        {
            if(CurrentSprint.State == SprintState.QUEUING)
            {
                //start sprint
                try
                {
                    _projectService.StartSprint(CurrentSprint);
                    CurrentSprint = new Sprint()
                    {
                        Id = CurrentSprint.Id,
                        State = SprintState.ACTIVE,
                        StartDate = CurrentSprint.StartDate,
                        EndDate = CurrentSprint.EndDate,
                        Order = CurrentSprint.Order,
                        UserStories = CurrentSprint.UserStories
                    };
                }
                catch(CheckedException e)
                {
                    ShowError("Error", e.Message);
                }
            }
            else if (CurrentSprint.State == SprintState.ACTIVE)
            {
                try
                {
                    _projectService.EndSprint(SelectedProject.Id, CurrentSprint);
                    SelectedProject = _projectService.GetProjectById(SelectedProject.Id);
                    SelectedMember = null;
                    UpdateMembers();
                    CurrentSprint = SelectedProject.Sprints.OrderByDescending(p => p.Order).First();

                }
                catch (CheckedException e)
                {
                    ShowError("Error", e.Message);
                }

            }
        }

        private void AddToSprint(UserStory userStory)
        {
            try
            {
                _projectService.AddUserStoryToSprint(CurrentSprint.Id, userStory);
                CurrentSprint.UserStories.Add(userStory);
                Refresh();
            }
            catch(CheckedException e)
            {
                ShowError("Error", e.Message);
            }
        }

        private void ShowAddUserStory(int? id)
        {
            _userStoryEpicId = (int)id;
            AddingUserStory = true;
        }

        private void AddUserStory()
        {
            try
            {
                _projectService.AddUserStory(_userStoryEpicId, UserStoryTitle, UserStoryDescription, UserStoryStoryPoints);
                SelectedProject = _projectService.GetProjectById(SelectedProject.Id);
                SelectedMember = null;
                UpdateMembers();
                AddingUserStory = false;
            }
            catch (CheckedException e)
            {
                ShowError("Error", e.Message);
            }
        }

        private void Refresh()
        {
            ChangingRole = false;
            AddingEpic = false;
            AddingUserStory = false;
            
            SelectedProject = _projectService.GetProjectById(SelectedProject.Id);
            SelectedMember = null;
            UpdateMembers();
            NotInProjectUsers.Clear();
            NotInProjectUsers.AddRange(_projectService.GetAllUsersNotInProject(SelectedProject.Id));
            UpdateUsers();
        }

        private void CancelAddUserStory()
        {
            AddingUserStory = false;
        }

        private void AddEpic()
        {
            try
            {
                Epic epic = _projectService.AddEpic(SelectedProject.Id,EpicTitle, EpicDescription);
                SelectedProject.Epics.Add(epic);
                AddingEpic = false;
            }
            catch (CheckedException ex)
            {
                ShowError("Error", ex.Message);
            }
        }

        private void CancelAddEpic()
        {
            AddingEpic = false;
        }

        private void ShowAddEpic()
        {
            AddingEpic = true;
        }

        private void ShowChangeRole()
        {
            if(SelectedMember == null)
            {
                return;
            }
            _changingMemberId = SelectedMember.Id;
            ChangingRole = true;
        }

        private void CancelChangeRole()
        {
            ChangingRole = false;
        }

        private void ChangeMemberRole()
        {
            _projectService.ChangeMemberRole(_changingMemberId, SelectedRole);
            SelectedProject = _projectService.GetProjectById(SelectedProject.Id);
            SelectedMember = null;
            UpdateMembers();
            ChangingRole = false;
        }

        private void RemoveMemberFromProject()
        {
            if(SelectedMember == null)
            {
                return;
            }
            try
            {
                _projectService.RemoveMemberFromProject(SelectedProject.Id, (int)SelectedMember.Id);
                var deletedMember = SelectedProject.Members.FirstOrDefault(m => m.Id == SelectedMember.Id);
                if(deletedMember != null)
                {
                    SelectedProject.Members.Remove(deletedMember);
                }
                Refresh();
            }
            catch (CheckedException e)
            {
                ShowError("Error", e.Message);
            }

        }

        private void DeleteProject()
        {

        }

        private void EditProject()
        {
            _projectService.EditProject(SelectedProject.Id, ProjectName, ProjectKey);
        }

        private void CancelAddMember()
        {
            SelectingRole = false;
        }

        private void AddMember()
        {
            try
            {
                var member = _projectService.AddUserToProject(_addingUser, SelectedProject, SelectedRole);
                SelectedProject.Members.Add(member);
                member.User = _addingUser;
                member.Project = SelectedProject;
                NotInProjectUsers.Remove(NotInProjectUsers.FirstOrDefault(user => user.Id == _addingUser.Id));
                UpdateUsers();
                UpdateMembers();
                SelectingRole = false;
            }
            catch (Exception e)
            {
                ShowError("Error", e.Message);
            }
        }

        public ProjectDetailsViewModel()
        {
            SelectingRole = false;
        }

        private void ShowAddMember(User data)
        {
            _addingUser = data;
            SelectingRole = true;
        }

        private void AssignMember()
        {
            try
            {
                _projectService.AssignToTask(_assignedMember.Id, _assigningTaskId);
                SelectedProject = _projectService.GetProjectById(SelectedProject.Id);
                SelectedMember = null;
                UpdateMembers();
                CurrentSprint = SelectedProject.Sprints.OrderByDescending(p => p.Order).First();
                AssigningMember = false;
            }
            catch(Exception e)
            {
                ShowError("Error", e.Message);
            }
        }

        private IProjectService _projectService;

        public ProjectDetailsViewModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedProject = (Project)navigationContext.Parameters["Project"];
            SelectedMember = null;
            ProjectName = SelectedProject.Name;
            ProjectKey = SelectedProject.Key;
            NotInProjectUsers.Clear();
            NotInProjectUsers.AddRange(_projectService.GetAllUsersNotInProject(SelectedProject.Id));
            UpdateUsers();
            IsProjectStarted = SelectedProject.Sprints.Count != 0;
            if(IsProjectStarted)
            {
                CurrentSprint = SelectedProject.Sprints.OrderByDescending(p => p.Order).First();
            }
            UpdateMembers();
        }

        public void StartProject()
        {
            var sprint = _projectService.GetNextSprint(SelectedProject.Id);
            
            SelectedProject.Sprints.Add(sprint);
            CurrentSprint = sprint;
            IsProjectStarted = true;
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
