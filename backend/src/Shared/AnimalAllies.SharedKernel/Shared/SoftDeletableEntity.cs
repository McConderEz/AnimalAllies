namespace AnimalAllies.SharedKernel.Shared;

public abstract class SoftDeletableEntity<TId> : Entity<TId>
    where TId : notnull
{
    protected SoftDeletableEntity(TId id) : base(id){}
    
    public bool IsDeleted { get; protected set; }
    public DateTime DeletionDate { get; protected set; }    
    
    public virtual void Delete()
    {
        IsDeleted = true;
    }

    public virtual void Restore()
    {
        IsDeleted = false;
    }
}