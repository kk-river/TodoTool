namespace TodoTool.Repositories;

public interface IProjectRepository
{
    public ValueTask InitializeAsync(CancellationToken token = default);

    public ValueTask<Project?> GetByIdAsync(string id, CancellationToken token = default);
    public ValueTask<List<Project>> GetAllAsync(CancellationToken token = default);
    public ValueTask AddAsync(Project project, CancellationToken token = default);
    public ValueTask UpdateAsync(Project project, CancellationToken token = default);
    public ValueTask DeleteAsync(string id, CancellationToken token = default);
}

public record Project(string Id, string Name, string Description, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt)
{
    public Project(string name, string description) : this(Guid.NewGuid().ToString(), name, description, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow) { }
}
