using UnitGenerator;

namespace TodoTool.Entities;

public record Project(ProjectId Id, string Name, string Description, ProjectId ParentId, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt)
{
    public Project(string name, string description) : this(Guid.NewGuid().ToString(), name, description, ProjectId.Empty, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow) { }
}


[UnitOf<string>(UnitGenerateOptions.ImplicitOperator)]
public partial struct ProjectId
{
    public static readonly ProjectId Empty = new(string.Empty);
}
