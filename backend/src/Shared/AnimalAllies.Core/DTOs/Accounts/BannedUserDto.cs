namespace AnimalAllies.Core.DTOs.Accounts;

public class BannedUserDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RelationId { get; set; }
    public DateTime BannedAt { get; set; }
}