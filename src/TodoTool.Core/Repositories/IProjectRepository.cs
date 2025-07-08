using R3;
using TodoTool.Entities;

namespace TodoTool.Repositories;

public interface IProjectRepository
{
    public Observable<Project> Added { get; }
    public Observable<Project> Updated { get; }
    public Observable<ProjectId> Deleted { get; }

    public ValueTask InitializeAsync(CancellationToken token = default);

    public ValueTask<Project?> GetByIdAsync(ProjectId id, CancellationToken token = default);
    public ValueTask<List<Project>> GetAllAsync(CancellationToken token = default);
    public ValueTask AddAsync(Project project, CancellationToken token = default);
    public ValueTask UpdateAsync(Project project, CancellationToken token = default);
    public ValueTask DeleteAsync(ProjectId id, CancellationToken token = default);
}
