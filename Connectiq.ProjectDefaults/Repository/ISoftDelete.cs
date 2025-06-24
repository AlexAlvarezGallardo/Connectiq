namespace Connectiq.ProjectDefaults.Repository;

public interface ISoftDelete
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
}
