using Dapper;
using Microsoft.Extensions.Logging;
using R3;
using System.Data;
using TodoTool.Entities;
using ZLogger;

namespace TodoTool.Repositories;

internal class ProjectRepository(ILogger<ProjectRepository> logger, IDbConnection dbConnection) : IProjectRepository
{
    private readonly ILogger<ProjectRepository> _logger = logger;
    private readonly IDbConnection _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

    private readonly Subject<Project> _added = new();
    private readonly Subject<Project> _updated = new();
    private readonly Subject<ProjectId> _deleted = new();

    public Observable<Project> Added => _added;
    public Observable<Project> Updated => _updated;
    public Observable<ProjectId> Deleted => _deleted;

    public async ValueTask InitializeAsync(CancellationToken token = default)
    {
        string sql = """
            CREATE TABLE IF NOT EXISTS Projects (
                Id TEXT PRIMARY KEY,
                Name TEXT NOT NULL,
                Description TEXT,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL
            )
            """;

        try
        {
            await _dbConnection.ExecuteAsync(new(sql, cancellationToken: token));
        }
        catch (Exception ex)
        {
            _logger.ZLogError(ex, $"Failed to initialize the database.");
            throw;
        }
    }

    public async ValueTask<Project?> GetByIdAsync(ProjectId id, CancellationToken token = default)
    {
        string sql = "SELECT * FROM Projects WHERE Id = @Id";

        return await _dbConnection.QuerySingleOrDefaultAsync<Project>(new(sql, new { Id = id }, cancellationToken: token));
    }

    public async ValueTask<List<Project>> GetAllAsync(CancellationToken token = default)
    {
        string sql = "SELECT * FROM Projects";

        return [.. await _dbConnection.QueryAsync<Project>(new(sql, cancellationToken: token))];
    }

    public async ValueTask AddAsync(Project project, CancellationToken token = default)
    {
        string sql = """
            INSERT INTO Projects (Id, Name, Description, CreatedAt, UpdatedAt)
            VALUES (@Id, @Name, @Description, @CreatedAt, @UpdatedAt)
            """;

        await _dbConnection.ExecuteAsync(new(sql, project, cancellationToken: token));

        _added.OnNext(project);
    }

    public async ValueTask UpdateAsync(Project project, CancellationToken token = default)
    {
        string sql = """
            UPDATE Projects
            SET Name = @Name, Description = @Description, CreatedAt = @CreatedAt, UpdatedAt = @UpdatedAt
            WHERE Id = @Id
            """;

        await _dbConnection.ExecuteAsync(new(sql, project, cancellationToken: token));

        _updated.OnNext(project);
    }

    public async ValueTask DeleteAsync(ProjectId id, CancellationToken token = default)
    {
        string sql = "DELETE FROM Projects WHERE Id = @Id";

        await _dbConnection.ExecuteAsync(new(sql, new { Id = id }, cancellationToken: token));

        _deleted.OnNext(id);
    }
}
