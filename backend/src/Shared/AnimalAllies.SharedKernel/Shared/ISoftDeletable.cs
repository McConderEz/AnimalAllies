namespace AnimalAllies.SharedKernel.Shared;

public interface ISoftDeletable
{
    public bool IsDeleted { get; }
    public DateTime? DeletionDate { get; }
    void Delete();
    void Restore();
}