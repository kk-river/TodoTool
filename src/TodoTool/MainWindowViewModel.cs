using R3;
using TodoTool.Repositories;

namespace TodoTool;

public class MainWindowViewModel
{
    private readonly IProjectRepository _projectRepository;
    private readonly ReactiveProperty<string> _projectId = new();
    private readonly BindableReactiveProperty<string> _projectName = new();

    public MainWindowViewModel(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));

        ProjectId = _projectId.ToReadOnlyBindableReactiveProperty(string.Empty);
        ProjectName = _projectName;

        SaveProjectCommand = new ReactiveCommand(SaveProjectAsync);
        _ = LoadProjectsAsync();
    }

    private async Task LoadProjectsAsync()
    {
        await _projectRepository.InitializeAsync();

        try
        {
            List<Project> projects = await _projectRepository.GetAllAsync();
            if (projects.Count > 0)
            {
                _projectId.Value = projects[0].Id;
                _projectName.Value = projects[0].Name;
            }
            else
            {
                _projectId.Value = string.Empty;
                _projectName.Value = string.Empty;
            }
        }
        catch (Exception)
        {
        }
    }

    public async ValueTask SaveProjectAsync(Unit _, CancellationToken token)
    {
        try
        {
            Project project = new(_projectName.Value, "test");
            await _projectRepository.AddAsync(project, token);
        }
        catch (Exception)
        {
        }
    }

    public IReadOnlyBindableReactiveProperty<string> ProjectId { get; }
    public IBindableReactiveProperty<string> ProjectName { get; }

    public ReactiveCommand<Unit> SaveProjectCommand { get; }
}
