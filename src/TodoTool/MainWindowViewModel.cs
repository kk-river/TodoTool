using Microsoft.Extensions.Logging;
using ObservableCollections;
using R3;
using TodoTool.Entities;
using TodoTool.Repositories;
using ZLogger;

namespace TodoTool;

public class MainWindowViewModel
{
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IProjectRepository _projectRepository;
    private readonly ObservableDictionary<ProjectId, Project> _projects = [];

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger, IProjectRepository projectRepository)
    {
        _logger = logger;
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

        Projects = _projects
            .CreateView(kvp => kvp.Value)
            .ToNotifyCollectionChanged();

        AddProjectCommand = new ReactiveCommand(AddProjectAsync);
        SaveProjectCommand = new ReactiveCommand(SaveProjectAsync);

        projectRepository.Added.Subscribe(project =>
        {
            _logger.ZLogInformation($"Project added: {project.Name}");

            if (_projects.ContainsKey(project.Id))
            {
                _logger.ZLogWarning($"Project with ID {project.Id} already exists.");
                return;
            }

            _projects[project.Id] = project;
        });

        projectRepository.Updated.Subscribe(project =>
        {
            _logger.ZLogInformation($"Project updated: {project.Name}");
            if (!_projects.ContainsKey(project.Id)) { _logger.ZLogWarning($"Project with ID {project.Id} does not exist."); }

            _projects[project.Id] = project;
        });

        projectRepository.Deleted.Subscribe(id =>
        {
            _logger.ZLogInformation($"Project deleted: {id}");
            if (!_projects.ContainsKey(id)) { _logger.ZLogWarning($"Project with ID {id} does not exist."); }

            _projects.Remove(id);
        });

        _ = LoadProjectsAsync();
    }

    public INotifyCollectionChangedSynchronizedViewList<Project> Projects { get; }
    public BindableReactiveProperty<string> SelectedProjectPath { get; } = new(string.Empty);

    public ReactiveCommand<Unit> AddProjectCommand { get; }
    public ReactiveCommand<Unit> SaveProjectCommand { get; }

    private async Task LoadProjectsAsync()
    {
        await _projectRepository.InitializeAsync();

        try
        {
            List<Project> projects = await _projectRepository.GetAllAsync();
            _logger.ZLogInformation($"Loaded {projects.Count} projects from repository.");

            _projects.Clear();
            foreach (Project project in projects) { _projects[project.Id] = project; }
        }
        catch (Exception)
        {
            _logger.ZLogError($"Failed to load projects from repository.");
        }
    }

    private async ValueTask AddProjectAsync(Unit _, CancellationToken token)
    {
        try
        {
            Project project = new(Guid.NewGuid().ToString(), "New Project");

            await _projectRepository.AddAsync(project, token);
        }
        catch (Exception)
        {
            _logger.ZLogError($"Failed to add new project.");
        }
    }

    public ValueTask SaveProjectAsync(Unit _, CancellationToken token)
    {
        try
        {
        }
        catch (Exception)
        {
        }

        return ValueTask.CompletedTask;
    }
}
